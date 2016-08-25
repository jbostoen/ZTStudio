Imports System.IO

Public Class clsPalette


    Dim pal_FileName As String = ""
    Dim pal_colors As new List(Of System.Drawing.Color)


    Public Property fileName As String
        Get
            Return pal_FileName
        End Get
        Set(value As String)
            pal_FileName = value
        End Set
    End Property


    Public Property colors As List(Of System.Drawing.Color)
        Get
            Return pal_colors
        End Get
        Set(value As List(Of System.Drawing.Color))
            pal_colors = value
        End Set
    End Property

    Function readPalette(Optional strFileName As String = vbNullString) As Integer

        If strFileName <> vbNullString Then pal_FileName = strFileName


        Debug.Print("Reading palette.")

        If File.Exists(pal_FileName) = False Then
            MsgBox("Warning: could not find '" & pal_FileName & "')." & vbCrLf &
                   Application.ProductName & " will quit since this would cause errors.", vbOKOnly + vbCritical, "No palette")
            Return 0

        End If

        ' Read full file.
        Dim bytes As Byte() = IO.File.ReadAllBytes(pal_FileName)
        Dim hex As String() = Array.ConvertAll(bytes, Function(b) b.ToString("X2"))

        Dim pal_numColors As Integer = 0


        'frmMain.txtHex.Text = String.Join(" ", hex)

        ' Now, the first bytes tell us how many colors there are.
        ' We will cheat a little bit: ZT1 seems to only support a limited amount of colors (255?)

        ' The first 2 bytes signal how many blocks of 4 bytes we have.
        pal_numColors = CInt("&H" & hex(1) & hex(0)) '- 1


        Debug.Print("Colors in this palette: " & pal_numColors)

        hex = hex.Skip(4).ToArray()



        Dim pal As New List(Of System.Drawing.Color)

        ' Read number of colors x 3 bytes, starting from byte 8, 12, 16, 20...
        ' We can ignore the FF since it might just refer to opacity or nothing at all.


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

    Function readPaletteBytes(intStart As Integer, intStop As Integer, Optional strFileName As String = vbNullString) As String

        If strFileName <> vbNullString Then pal_FileName = strFileName

        ' Read full. We could implement a faster reading later.
        Dim bytes As Byte() = IO.File.ReadAllBytes(pal_FileName)
        Dim hex As String() = Array.ConvertAll(bytes, Function(b) b.ToString("X2"))


        Dim ret As String = ""
        Dim x As Integer

        For x = intStart To intStop
            ret = ret & " " & hex(x)
        Next

        Return Strings.Trim(ret)


    End Function





    Sub fillPaletteGrid(dgv As DataGridView)



        dgv.GetType.InvokeMember("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.SetProperty, Nothing, dgv, New Object() {True})


        ' This function fills a datagridview.
        ' It changes the row heading to the index number of the color, 
        ' and the background color of the first column is the palette color.


        'Dim dgv As DataGridView = frmMain.dgvPalette



        ' Clear previous colors

        dgv.Rows.Clear()


        ' Currently just one palette grid.
        ' In our code, we used to create the rows and put them into an array.
        ' The benefit of that is that we could add it all at once.
        ' However, it turned out addRange took 5-6 seconds; while adding it to the DGV right away takes 2/3 secs


        Dim x As Integer = 0

        Dim addRows As New List(Of DataGridViewRow)

        Debug.Print(Now.ToString() & " Color palette - create array")

        dgv.Visible = False

        For Each col As System.Drawing.Color In pal_colors

            Dim drRow As New DataGridViewRow
            'drRow.DefaultCellStyle.BackColor = col
            'drRow.DefaultCellStyle.SelectionBackColor = col ' prevent selection highlighting (blue)

            With drRow
                .CreateCells(dgv)
                .HeaderCell.Value = x.ToString("0")
                .Cells(0).Value = x.ToString("X2")
                '.Cells(0).Style.BackColor = col
                '.Cells(0).Style.SelectionBackColor = col
                .DefaultCellStyle.BackColor = col
                .DefaultCellStyle.SelectionBackColor = col
            End With



            'dgv.Rows.Add(drRow)
            'dgv.Rows(x).Cells(0).Value = x.ToString("X2")
             

            'addRows.Add(drRow)

            x += 1

            dgv.Rows.Add(drRow)

        Next

        dgv.Visible = True
        Debug.Print(Now.ToString & " finished")
        ' Unfortunately, this is slow. It takes 5-6 seconds.
        ' dgv.Rows.AddRange(addRows.ToArray())

        'dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)





    End Sub

    Public Function getColorIndex(c As System.Drawing.Color, Optional addToPalette As Boolean = True) As Integer


        'Debug.Print("COLOR = " & c.ToString())


        If Me.colors.Count = 0 Then
            ' add transparent color!
            Me.colors.Add(System.Drawing.Color.FromArgb(0, 255, 0, 255), False)
        End If

        If Me.colors.Contains(c) = True Then

            'Debug.Print("Color found: " & Me.colors.IndexOf(c))


            Return Me.colors.IndexOf(c)

        Else
            ' It doesn't contain our color. Can we add it?
            If Me.colors.Count < 255 And addToPalette = True Then ' number of colors = [0-255]
                Me.colors.Add(c, False)
                'Debug.Print("Color added: " & (Me.colors.Count - 1))
                Return Me.colors.Count - 1  ' return last item index
            Else
                ' Debug.Print("Color overflow: " & Me.colors.Count & " - " & addToPalette & " - " & Me.colors.IndexOf(c))

                MsgBox("The current palette already contains " & Me.colors.Count & " colors." & vbCrLf & _
                       "This is the maximum we can allow." & vbCrLf & "Further errors could arise!", _
                       vbOKOnly + vbCritical, "Too many colors!")
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

        Dim comPal As New clsPalette

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


        Debug.Print("Loaded")

    End Function

End Class

