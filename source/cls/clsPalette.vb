Imports System.IO

Public Class clsPalette


    Dim pal_FileName As String = ""
    Dim pal_colors As New List(Of System.Drawing.Color)
    Dim pal_parent As clsGraphic2 = Nothing


    Public Sub New(myParent As clsGraphic2)
        pal_parent = myParent
    End Sub
    Public Property parent As clsGraphic2
        ' What is the parent object (clsGraphic2) of our frame? 
        ' Or in other words: which ZT1 Graphic does this frame belong to?
        Get
            Return pal_parent
        End Get
        Set(value As clsGraphic2)
            pal_parent = value
        End Set
    End Property
    Public Property fileName As String
        ' The filename of the palette.
        Get
            Return pal_FileName
        End Get
        Set(value As String)
            pal_FileName = value
        End Set
    End Property


    Public Property colors As List(Of System.Drawing.Color)
        ' This will contain all colors.
        Get
            Return pal_colors
        End Get
        Set(value As List(Of System.Drawing.Color))
            pal_colors = value
        End Set
    End Property

    Function readPal(Optional strFileName As String = vbNullString) As Integer


        If strFileName <> vbNullString Then pal_FileName = strFileName

        ' File does not exist.
        If File.Exists(pal_FileName) = False Then
            MsgBox("Warning: could not find '" & pal_FileName & "')." & vbCrLf &
                   Application.ProductName & " will quit since this would cause errors.", vbOKOnly + vbCritical, "No palette")
            Return 0

        End If

        ' Read full file.
        Dim bytes As Byte() = IO.File.ReadAllBytes(pal_FileName)
        Dim hex As String() = Array.ConvertAll(bytes, Function(b) b.ToString("X2"))

        Dim pal_numColors As Integer = 0

        ' Now, the first bytes tell us how many colors there are.
        ' We will cheat a little bit: ZT1 Graphics only support a limited amount of colors (255?)
        ' So only the first 2 bytes (rather than the first 4) signal how many blocks of 4 bytes we have.
        pal_numColors = CInt("&H" & hex(1) & hex(0)) '- 1 

        ' Jump to what matters.
        hex = hex.Skip(4).ToArray()
        Dim pal As New List(Of System.Drawing.Color)

        ' Read number of colors. Only 3 bytes per color are relevant. So starting from byte 8, 12, 16, 20...
        ' We can ignore the other byte (FF) since it might just refer to opacity (not implemented) or nothing at all.


        While hex.Length > 0


            ' Debug.Print("Color: " & strHexColorVals(0) & " " & strHexColorVals(1) & " " & strHexColorVals(2))
            ' Debug.Print("Color: " & CInt("&H" & strHexColorVals(0)) & " " & CInt("&H" & strHexColorVals(1)) & " " & CInt("&H" & strHexColorVals(2)))


            ' Turn into a color
            pal.Add(System.Drawing.Color.FromArgb( _
                CInt("&H" & hex(3)), _
                CInt("&H" & hex(0)), _
                CInt("&H" & hex(1)), _
                CInt("&H" & hex(2))
                    ), False)


            hex = hex.Skip(4).ToArray()

        End While


        pal_colors = pal

        Return 1


    End Function




    Sub fillPaletteGrid(dgv As DataGridView)



        ' This function fills a DataGridView.
        ' It changes the row heading to the index number of the color, 
        ' and the background color of the first column is the palette color.

        ' This is done to greatly improve the speed of drawing.
        ' Something weird is going on though. Later in this code, we go over the colors.
        dgv.GetType.InvokeMember("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.SetProperty, Nothing, dgv, New Object() {True})

        ' Clear previous colors
        ' ( this might be necessary if a palette was rendered previously in the same DataGridView)
        dgv.Rows.Clear()

        ' In our code, we used to create the rows and put them into an array.
        ' The benefit of that is that we could add it all at once.
        ' However, it turned out addRange took 5-6 seconds; while adding it to the DGV right away takes 2/3 secs
        Dim x As Integer = 0 ' we use this for autonumbering. We could've relied on .indexOf or something too, but this is quicker.


        ' Prevent visible updates in between.
        dgv.Visible = False

        For Each col As System.Drawing.Color In pal_colors

            Dim drRow As New DataGridViewRow


            With drRow

                ' The first color is actually transparent, but we want to show it in the DataGridView 
                .DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, col)
                .DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, col)

                .CreateCells(dgv)
                .HeaderCell.Value = x.ToString("0")
                .Cells(0).Value = x.ToString("X2")

            End With


            dgv.Rows.Add(drRow)
            x += 1 ' Autonumbering (row header)

        Next

        ' Make our DataGridView visible again, everything has bene added.
        dgv.Visible = True
        Debug.Print(Now.ToString & " finished")

        ' Unfortunately, this is slow. It takes 5-6 seconds.
        ' dgv.Rows.AddRange(addRows.ToArray())
        ' dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)





    End Sub

    Public Function getColorIndex(c As System.Drawing.Color, Optional addToPalette As Boolean = True) As Integer

        ' This function is used to find the index of a color in our color palette.
        ' If not found, it is added - until the maximum number of 255 (+1 transparent) colors has been reached.


        If Me.colors.Count = 0 Then
            ' We are working with a brand new color palette.
            ' There's no transparent color just yet. Define.
            ' was 0, 255, 0, 255 (pink)
            Me.colors.Add(System.Drawing.Color.FromArgb(0, cfg_grid_BackGroundColor), False)
        End If

        If Me.colors.Contains(c) = True Then

            ' Color has been found, return the index
            ' restrant.pal has a color listed twice.
            ' it seems to rely on the last index.
            Return Me.colors.LastIndexOf(c)



        ElseIf c.A = 0 Then

            ' We have a different transparent color
            Return 0

        ElseIf c.A = 255 And c.R = Me.colors(0).R And c.G = Me.colors(0).G And c.B = Me.colors(0).B Then

            ' Hotfix for opacity issue
            Return 0

        Else
            ' It doesn't contain our color. Can we still add it?
            If Me.colors.Count < 256 And addToPalette = True Then ' number of colors = [0-255]
                ' Yeah sure, just add it to the color palette.
                Me.colors.Add(c, False)
                Return Me.colors.Count - 1  ' return last item index
            Else
                '    "Failed to add (" & c.R.ToString() & ", " & c.G.ToString() & ", " & c.B.ToString() & ", " & c.A.ToString() & ")." & vbCrLf & _


                For Each col As System.Drawing.Color In Me.colors
                    Debug.Print(Me.colors.IndexOf(col).ToString("000") & " | " & col.ToString())
                Next



                MsgBox("The current palette (" & Me.fileName & ") already contains " & Me.colors.Count & " colors." & vbCrLf & vbCrLf & _
                       "Color: " & c.ToString() & vbCrLf & _
                       "Transparent color: " & Me.colors(0).ToString & vbCrLf & _
                       "Graphic: " & Me.parent.fileName & vbCrLf & _
                       "ZT Studio will close to prevent program or game crashes.", _
                       vbOKOnly + vbCritical + vbApplicationModal, "Too many colors!")




                Return -1
            End If

        End If





    End Function



    Public Function writePal(strFileName As String, blnOverwrite As Boolean) As Integer


        Debug.Print("Write .PAL to " & strFileName)


        On Error GoTo dBug

1:

        If File.Exists(strFileName) = True And blnOverwrite = False Then
            MsgBox("Error: could not create color palette." & vbCrLf & _
                "There is already a color palette at this location: " & vbCrLf & _
                "'" & strFileName & "'", vbOKOnly + vbCritical, "Failed to create .pal-file")

            Return 0
        End If

10:

        ' We have our number of colors. Piece of cake.
        ' We have our colors stored in .colors()
        ' So simply build and write.

        Dim opHex As New List(Of String) ' output hex
        Dim x As Integer

        Debug.Print(pal_colors.Count.ToString("X4"))

        With opHex
            .Add(Strings.Right(pal_colors.Count.ToString("X4"), 2), False)
            .Add(Strings.Left(pal_colors.Count.ToString("X4"), 2), False)
            .Add("00", False)
            .Add("00", False)

        End With

20:
        For x = 0 To (pal_colors.Count - 1)

            With opHex
                .Add(pal_colors(x).R.ToString("X2"), False)
                .Add(pal_colors(x).G.ToString("X2"), False)
                .Add(pal_colors(x).B.ToString("X2"), False)

                ' Only the first color is transparent
                If x = 0 Then
                    .Add("00", False)
                Else
                    .Add("FF", False)
                End If

            End With



        Next

        'Debug.Print(Strings.Join(opHex.ToArray(), " "))

1000:
        Dim fs As New FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Write)
        For Each s As String In opHex
            fs.WriteByte(CByte("&H" & s))
        Next


        fs.Close()
        fs.Dispose()


        Return 1


dBug:
        MsgBox("Error while writing a color palette. " & vbCrLf & "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Failed to create color palette")
        Return 0

    End Function


    ' === extra functions ===

    Function combineColorPalettes(lstPals As List(Of clsPalette)) As clsPalette


        ' This function should allow to create/combine color palettes.
        ' We'll need to check somewhere if we don't have too many colours! (more than 255 + 1 transparent!)

        Debug.Print("Combine color palettes.")


        Dim comPal As New clsPalette(Nothing)

        ' for each color palette: check if color exists in our new palette.
        For Each pal As clsPalette In lstPals
            For Each col As System.Drawing.Color In pal.colors

                ' add color if it's not in our list
                If comPal.colors.IndexOf(col) < 0 Then
                    comPal.colors.Add(col, False)
                End If


            Next
        Next


        ' also:
        Me.colors = comPal.colors


        Return comPal


    End Function




    Function export_to_PNG(strExportFileName As String)

        ' This is for a feature where we first exported a color palette, by writing all known colors in a single image.
        ' The idea is that the .PNG can easily be recolored with a 3rd party program (eg GIMP)
        ' This way, the entire palette of an existing animal can be recolored at once. (recoloring was a well known method to create 'new' animals)
        ' Next, we reimport this. We'd only need to fix the shadow.



        Dim bmp As New Bitmap(16, 16)

        ''Perform Drawing here

        Dim intX As Integer = 0
        Dim intY As Integer = 0
        Dim intColor As Integer

        Debug.Print("testing export.")

        ' for each row
        While intY < 16

            ' for each col
            While intX < 16 And intColor < Me.colors.Count

                bmp.SetPixel(intX, intY, Me.colors(intColor))
                intColor += 1
                intX += 1
            End While

            ' reset, next line
            intX = 0
            intY += 1

        End While


        If File.Exists(strExportFileName) = True Then
            File.Delete(strExportFileName)
        End If

        bmp.Save(strExportFileName, System.Drawing.Imaging.ImageFormat.Png)

        Debug.Print("Exported .pal to .png => " & strExportFileName)



    End Function


    Function import_from_PNG(sFileName As String, Optional blnForceAddColor As Boolean = False)

        ' This is for a feature where we first exported a color palette, by writing all known colors in a single image.
        ' The idea is that the .PNG can easily be recolored with a 3rd party program (eg GIMP)
        ' This way, the entire palette of an existing animal can be recolored at once. (recoloring was a well known method to create 'new' animals)
        ' Next, we reimport this. We'd only need to fix the shadow.
        ' We do not rely on native GIMP Palettes (.gpl) because some people might prefer Paint.NET or other programs.
        ' By importing from .PNG, we have a general approach.

        Dim bmp As Bitmap = Image.FromFile(sFileName)


        Dim intX As Integer = 0
        Dim intY As Integer = 0

        ' Clear current palette (please prevent redraws at this point)
        Me.colors.Clear(False)

        ' Row by row
        While intY < bmp.Height

            While intX < bmp.Width

                ' Do not add duplicate colors, e.g. transparent stuff etc
                ' 20150815: unless it's forced. After recoloring, it seems some colors are identical?
                If Me.colors.IndexOf(bmp.GetPixel(intX, intY)) < 0 Or blnForceAddColor = True Then
                    Me.colors.Add(bmp.GetPixel(intX, intY), False)
                End If

                intX += 1

            End While

            ' reset & next line
            intX = 0
            intY += 1

        End While


200:

        ' There's actually two possibilities here.
        ' Either we should regenerate the hex, since colors might have switched places.
        ' Or we should regenerate the image, since it might just be a recolor. 
        If IsNothing(Me.parent) = False Then
            ' TODO: do something
            For Each ztFrame As clsFrame2 In Me.parent.frames
                ztFrame.coreImageBitmap = Nothing
                ztFrame.getCoreImageBitmap()
            Next
        Else
        End If

    End Function





    Function import_from_GimpPalette(sFileName As String, Optional blnForceAddColor As Boolean = False)

        On Error GoTo dBg

0:

        ' Contrary to import_from_PNG, this one is specifically designed for the free and open source GIMP

        ' Typical file: 


        ' GIMP Palette
        ' Name:   RedPanda copy
        ' Columns: 16
        ' #
        ' 0   1   0	#0
        ' <line for each color>
        ' 254 255 252	#254


10:

        Dim objReader As New System.IO.StreamReader(sFileName)

        Dim textLine As String = ""
        Dim intLine As Integer = 1

        ' Clear current palette (please prevent redraws at this point)
        Me.colors.Clear(False)

        ' Read file.
        Do While objReader.Peek() <> -1

11:
            textLine = objReader.ReadLine()

            ' Remove double white spaces etc 
            textLine = Strings.Trim(System.Text.RegularExpressions.Regex.Replace(textLine, "\s+", " "))



            ' Ignore the first few lines of the GPL file AND the transparent color
            If intLine = 5 And textLine <> "" Then
21:
                Me.colors.Add(System.Drawing.Color.FromArgb(Split(textLine, " ")(0), Split(textLine, " ")(1), Split(textLine, " ")(2)))


            ElseIf intLine > 5 And textLine <> "" Then

22:
                ' Add to this color palette
                Me.getColorIndex(System.Drawing.Color.FromArgb(Split(textLine, " ")(0), Split(textLine, " ")(1), Split(textLine, " ")(2)), True)


            End If

            ' Next
            intLine += 1

        Loop

200:

        ' There's actually two possibilities here.
        ' Either we should regenerate the hex, since colors might have switched places.
        ' Or we should regenerate the image, since it might just be a recolor. 
        If IsNothing(Me.parent) = False Then
            ' TODO: do something
            For Each ztFrame As clsFrame2 In Me.parent.frames
                ztFrame.coreImageBitmap = Nothing
                ztFrame.getCoreImageBitmap()
            Next
            'MsgBox("Updated ZTFrame's coreImageBitmap?")
        Else
            'MsgBox("No parent?")
            'MsgBox(Me.parent.frames.Count)
        End If

        Exit Function

dBg:
        MsgBox("Unable to use the GIMP Color Palette:" & vbCrLf & sFileName & vbCrLf & Err.Number & " - " & Err.Description & vbCrLf & "Line in .gpl: " & textLine & vbCrLf & "Line in import_from_GimpPalette: " & Erl(), vbOKOnly + vbInformation, "Error using GIMP Palette")


    End Function





End Class

