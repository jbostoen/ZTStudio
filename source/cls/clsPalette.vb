Imports System.IO

Public Class ClsPalette


    Dim pal_FileName As String = vbNullString
    Dim pal_colors As New List(Of System.Drawing.Color)
    Dim pal_parent As ClsGraphic = Nothing


    Public Sub New(myParent As ClsGraphic)
        pal_parent = myParent
    End Sub
    Public Property Parent As ClsGraphic
        ' What is the parent object (ClsGraphic) of our frame? 
        ' Or in other words: which ZT1 Graphic does this frame belong to?
        Get
            Return pal_parent
        End Get
        Set(value As ClsGraphic)
            pal_parent = value
        End Set
    End Property
    Public Property FileName As String
        ' The filename of the palette.
        Get
            Return pal_FileName
        End Get
        Set(value As String)
            pal_FileName = value
        End Set
    End Property


    Public Property Colors As List(Of System.Drawing.Color)
        ' This will contain all colors.
        Get
            Return pal_colors
        End Get
        Set(value As List(Of System.Drawing.Color))
            pal_colors = value
        End Set
    End Property

    Function ReadPal(Optional strFileName As String = vbNullString) As Integer


        If strFileName <> vbNullString Then
            pal_FileName = strFileName
        End If

        ' File does not exist.
        If File.Exists(pal_FileName) = False Then
            MsgBox("Warning: could not find '" & pal_FileName & "')." & vbCrLf &
                   Application.ProductName & " will quit since this would cause errors.", vbOKOnly + vbCritical, "No palette")
            Return 0

        End If

        ' Read full file.
        Dim Bytes As Byte() = IO.File.ReadAllBytes(pal_FileName)
        Dim hex As String() = Array.ConvertAll(Bytes, Function(b) b.ToString("X2"))

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
            pal.Add(System.Drawing.Color.FromArgb(
                CInt("&H" & hex(3)),
                CInt("&H" & hex(0)),
                CInt("&H" & hex(1)),
                CInt("&H" & hex(2))
                    ), False)


            hex = hex.Skip(4).ToArray()

        End While


        pal_colors = pal

        Return 1


    End Function




    Sub FillPaletteGrid(dgv As DataGridView)



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

    ''' <summary>
    ''' Returns the index of a color within this palette.
    ''' </summary>
    ''' <param name="cColor">The color of which the index in this palette should be returned</param>
    ''' <param name="blnAddToPalette">Add the color to the palette if it's not present</param>
    ''' <returns></returns>
    Public Function GetColorIndex(cColor As System.Drawing.Color, Optional blnAddToPalette As Boolean = True) As Integer

        ' This function is used to find the index of a color in our color palette.
        ' If not found, it is added - until the maximum number of 255 (+1 transparent) colors has been reached.

        If Me.Colors.Count = 0 Then
            ' This is a new color palette with no colors defined yet.
            ' Define the first color (transparent color) in this palette.
            Me.Colors.Add(System.Drawing.Color.FromArgb(0, Cfg_grid_BackGroundColor), False)
        End If

        ' Store so we don't need to call both .contains() and .lastindexof()
        Dim IntColorIndex As Integer = Me.Colors.LastIndexOf(cColor)

        If IntColorIndex >= 0 Then

            ' Color has been found, return the index
            ' restrant.pal has a color listed twice.
            ' it seems to rely on the last index.
            Return IntColorIndex

        ElseIf cColor.A = 0 Then

            ' This color palette uses a different transparent color.
            ' However, the .PNG contained a color with with alpha = 0 (transparent)
            Return 0

        ElseIf cColor = Cfg_grid_BackGroundColor Then

            ' The images being imported use a color which has been explicitly set as the background (or transparent) color in ZT Studio.
            Return 0

        ElseIf cColor.A = 255 And cColor.R = Me.Colors(0).R And cColor.G = Me.Colors(0).G And cColor.B = Me.Colors(0).B Then

            ' Hotfix for opacity issue
            ' The specified color is opaque, but all color values are the same of the transparent color within this palette
            Return 0

        Else
            ' It doesn't contain our color. Can we still add it?
            If Me.Colors.Count < 256 And blnAddToPalette = True Then ' number of colors = [0-255]
                ' Yeah sure, just add it to the color palette.
                Me.Colors.Add(cColor, False)
                Return Me.Colors.Count - 1  ' return last item index
            Else

                ' Failed to add color: (" & c.R.ToString() & ", " & c.G.ToString() & ", " & c.B.ToString() & ", " & c.A.ToString() & ")." & vbCrLf & _
                ' For Each col As System.Drawing.Color In Me.colors
                ' Debug.Print(Me.colors.IndexOf(col).ToString("000") & " | " & col.ToString())
                '  Next

                ' No decision made yet
                If Cfg_palette_quantization = 0 Then
                    If MsgBox("The current palette (" & Me.FileName & ") already contains " & Me.Colors.Count & " colors." & vbCrLf & vbCrLf &
                           "Color: " & cColor.ToString() & vbCrLf &
                           "Transparent color: " & Me.Colors(0).ToString & vbCrLf &
                           "Graphic: " & Me.Parent.FileName & vbCrLf & vbCrLf &
                           "Zoo Tycoon 1 only supports 255 colors in each color palette." & vbCrLf &
                           "ZT Studio can pick the closest matching color used so far." & vbCrLf &
                           "You can expect a degradation in quality." & vbCrLf &
                           "Press [Yes] to ignore all warnings until you close ZT Studio." & vbCrLf &
                           "Press [No] to quit ZT Studio and fix things first.",
                           vbYesNo + vbCritical + vbApplicationModal, "Too many colors!") = vbYes Then
                        Cfg_palette_quantization = 1
                    Else
                        ' Quit ZT Studio, we'll just get too many errors otherwise.
                        End
                    End If
                End If

                ' Color quantization method by HENDRIX 
                'now checking in HSV space to find the closest color in the full palette - pretty good!'
                Dim h1 As Single
                Dim S1 As Single
                Dim v1 As Single
                Dim h2 As Single
                Dim S2 As Single
                Dim v2 As Single
                Dim dists As New List(Of Short)
                h1 = cColor.GetHue()
                S1 = cColor.GetSaturation()
                v1 = cColor.GetBrightness()
                For Each col As System.Drawing.Color In Me.Colors
                    h2 = h1 - col.GetHue()
                    S2 = S1 - col.GetSaturation()
                    v2 = v1 - col.GetBrightness()
                    'in HSV we can use simple euclidean distance and it is reasonably good
                    dists.Add(Math.Sqrt(h2 * h2 + S2 * S2 + v2 * v2))
                Next
                'see at which index in the existing color palette the least distance occured
                Return dists.LastIndexOf(dists.Min())



            End If

        End If





    End Function



    Public Function WritePal(strFileName As String, blnOverwrite As Boolean) As Integer


        'Debug.Print("... Write .PAL to " & strFileName   )


        On Error GoTo dBug

1:

        If File.Exists(strFileName) = True And blnOverwrite = False Then
            MsgBox("Error: could not create color palette." & vbCrLf &
                "There is already a color palette at this location: " & vbCrLf &
                "'" & strFileName & "'", vbOKOnly + vbCritical, "Failed to create .pal-file")

            Return 0
        End If

10:

        ' We have our number of colors. Piece of cake.
        ' We have our colors stored in .colors()
        ' So simply build and write.

        Dim opHex As New List(Of String) ' output hex
        Dim x As Integer

        'Debug.Print(pal_colors.Count.ToString("X4"))

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
        MsgBox("Error while writing a color palette. " & vbCrLf & "Line: " & Erl() & vbCrLf &
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Failed to create color palette")
        Return 0

    End Function


    ' === extra functions ===

    Function CombineColorPalettes(lstPals As List(Of ClsPalette)) As ClsPalette


        ' This function should allow to create/combine color palettes.
        ' We'll need to check somewhere if we don't have too many colours! (more than 255 + 1 transparent!)

        Debug.Print("Combine color palettes.")


        Dim ComPal As New ClsPalette(Nothing)

        ' for each color palette: check if color exists in our new palette.
        For Each pal As ClsPalette In lstPals
            For Each col As System.Drawing.Color In pal.Colors

                ' add color if it's not in our list
                If ComPal.Colors.IndexOf(col) < 0 Then
                    ComPal.Colors.Add(col, False)
                End If


            Next
        Next


        ' also:
        Me.Colors = ComPal.Colors


        Return ComPal


    End Function




    Sub Export_to_PNG(strExportFileName As String)

        ' This is for a feature where we first exported a color palette, by writing all known colors in a single image.
        ' The idea is that the .PNG can easily be recolored with a 3rd party program (eg GIMP)
        ' This way, the entire palette of an existing animal can be recolored at once. (recoloring was a well known method to create 'new' animals)
        ' Next, we reimport this. We'd only need to fix the shadow.

        Dim Bmp As New Bitmap(16, 16)

        ''Perform Drawing here

        Dim IntX As Integer = 0
        Dim IntY As Integer = 0
        Dim IntColor As Integer


        ' for each row
        While IntY < 16

            ' for each col
            While IntX < 16 And IntColor < Me.Colors.Count

                Bmp.SetPixel(IntX, IntY, Me.Colors(IntColor))
                IntColor += 1
                IntX += 1
            End While

            ' reset, next line
            IntX = 0
            IntY += 1

        End While


        If File.Exists(strExportFileName) = True Then
            File.Delete(strExportFileName)
        End If

        Bmp.Save(strExportFileName, System.Drawing.Imaging.ImageFormat.Png)

        Bmp.Dispose()


    End Sub


    Sub Import_from_PNG(sFileName As String)

        ' This is for a feature where we first exported a color palette, by writing all known colors in a single image.
        ' The idea is that the .PNG can easily be recolored with a 3rd party program (eg GIMP)
        ' This way, the entire palette of an existing animal can be recolored at once. (recoloring was a well known method to create 'new' animals)
        ' Next, we reimport this. We'd only need to fix the shadow.
        ' We do not rely on native GIMP Palettes (.gpl) because some people might prefer Paint.NET or other programs.
        ' By importing from .PNG, we have a general approach.

        Dim Bmp As Bitmap = Image.FromFile(sFileName)

        Dim IntX As Integer = 0
        Dim IntY As Integer = 0

        ' Clear current palette (please prevent redraws at this point)
        Me.Colors.Clear(False)

        ' Row by row
        While IntY < Bmp.Height

            While IntX < Bmp.Width

                ' Do not add duplicate colors, e.g. transparent stuff etc; UNLESS it's forced.
                ' Use case: After recoloring, some colors are suddenly identical (especially after they're made brighter or darker). 
                ' We have to add duplicate colors in that case, because we re-generate the preview from the hex values. 
                ' The hex values still reference the original indexes of their colors. So changes there would screw things up and raise errors.

                ' Because if a user is replacing an existing palette, the indexes to the colors might not have been changed in the actual graphic.
                If Me.Colors.IndexOf(Bmp.GetPixel(IntX, IntY)) < 0 Or Cfg_palette_import_png_force_add_colors = 1 Then
                    Me.Colors.Add(Bmp.GetPixel(IntX, IntY), False)
                End If

                IntX += 1

            End While

            ' reset & next line
            IntX = 0
            IntY += 1

        End While


200:

        ' There's actually two possibilities here.
        ' Either we should regenerate the hex, since colors might have switched places.
        ' Or we should regenerate the image, since it might just be a recolor. 
        If IsNothing(Me.Parent) = False Then
            ' TODO: do something
            For Each ztFrame As ClsFrame In Me.Parent.Frames
                ztFrame.CoreImageBitmap = Nothing
                ztFrame.GetCoreImageBitmap()
            Next
        Else
        End If


    End Sub


    Sub Import_from_GimpPalette(sFileName As String)

        On Error GoTo dBg

0:
        ' Contrary to Import_from_PNG, this one is specifically designed for the free and open source GIMP

        ' Typical file contents: 

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
        Dim IntLine As Integer = 1

        ' Clear current palette (please prevent redraws at this point)
        Me.Colors.Clear(False)

        ' Read file.
        Do While objReader.Peek() <> -1

11:
            textLine = objReader.ReadLine()

            ' Remove double white spaces etc 
            textLine = Strings.Trim(System.Text.RegularExpressions.Regex.Replace(textLine, "\s+", " "))

            ' Ignore the first few lines of the GPL file AND the transparent color
            If intLine = 5 And textLine <> "" Then
21:
                Me.Colors.Add(System.Drawing.Color.FromArgb(Split(textLine, " ")(0), Split(textLine, " ")(1), Split(textLine, " ")(2)))


            ElseIf intLine > 5 And textLine <> "" Then

22:
                ' Add to this color palette. Using GetColorIndex(color, True), it will prevent duplicates.
                Me.GetColorIndex(System.Drawing.Color.FromArgb(Split(textLine, " ")(0), Split(textLine, " ")(1), Split(textLine, " ")(2)), True)


            End If

            ' Next
            intLine += 1

        Loop

200:

        ' There's actually two possibilities here.
        ' Either regenerate the hex, since colors might have switched places.
        ' Or regenerate the image, since it might just be a recolor. 
        If IsNothing(Me.Parent) = False Then
            ' TODO: do something
            For Each ztFrame As ClsFrame In Me.Parent.Frames
                ztFrame.CoreImageBitmap = Nothing
                ztFrame.GetCoreImageBitmap()
            Next
            'MsgBox("Updated ZTFrame's coreImageBitmap?")
        Else
            'MsgBox("No parent?")
            'MsgBox(Me.parent.frames.Count)
        End If


dBg:
        MsgBox("Unable to use the GIMP Color Palette:" & vbCrLf & sFileName & vbCrLf & Err.Number & " - " & Err.Description & vbCrLf & "Line in .gpl: " & textLine & vbCrLf & "Line in import_from_GimpPalette: " & Erl(), vbOKOnly + vbInformation, "Error using GIMP Palette")


    End Sub





End Class

