Option Explicit On
Imports System.IO




Module clsTasks



    ' The graphic writer will need to take care of the following things:

    ' (1) Write frames properly.
     


    Sub config_load()



        Dim sFile As String = System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.cfg"

        On Error Resume Next

        'iniWrite(sFile, "preview", "fgColor", Color.Red.ToArgb())


        ' Preview
        cfg_grid_BackGroundColor = System.Drawing.Color.FromArgb(iniRead(sFile, "preview", "bgColor", ""))
        cfg_grid_ForeGroundColor = System.Drawing.Color.FromArgb(iniRead(sFile, "preview", "fgColor", ""))
        cfg_grid_numPixels = iniRead(sFile, "preview", "numPixels", "")
        cfg_grid_zoom = iniRead(sFile, "preview", "zoom", "")
        cfg_grid_footPrintX = iniRead(sFile, "preview", "footPrintX", "")
        cfg_grid_footPrintY = iniRead(sFile, "preview", "footPrintY", "")

        ' Reads from ini and configures all.
        cfg_path_Root = iniRead(sFile, "paths", "root", "")
        cfg_path_recentPNG = iniRead(sFile, "paths", "recentPNG", "")
        cfg_path_recentZT1 = iniRead(sFile, "paths", "recentZT1", "")
        'cfg_path_ColorPals8 = iniRead(sFile, "paths", "pal8", "")
        'cfg_path_ColorPals16 = iniRead(sFile, "paths", "pal16", "")
        cfg_path_ColorPals8 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal8"
        cfg_path_ColorPals16 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal16"


        ' Export (PNG)
        cfg_export_PNG_CanvasSize = iniRead(sFile, "exportOptions", "pngSize", "")
        cfg_export_PNG_RenderBGFrame = iniRead(sFile, "exportOptions", "pngRenderExtraFrame", "")
        cfg_export_PNG_RenderBGZT1 = iniRead(sFile, "exportOptions", "pngRenderExtraGraphic", "")

        ' Export (ZT1)
        cfg_export_ZT1_Ani = iniRead(sFile, "exportOptions", "zt1Ani", "1")
        cfg_export_ZT1_AlwaysAddZTAFBytes = iniRead(sFile, "exportOptions", "zt1AlwaysAddZTAFBytes", "")


        ' Convert ( ZT1 <=> PNG, other way around )
        cfg_convert_startIndex = iniRead(sFile, "conversionOptions", "pngFilesIndex", "")
        cfg_convert_deleteOriginal = iniRead(sFile, "conversionOptions", "deleteOriginal", "")


        ' Frame editing
        cfg_editor_rotFix_individualFrame = iniRead(sFile, "editing", "individualRotationFix", "")

        ' Grid
        frmMain.tsbFrame_fpX.Text = cfg_grid_footPrintX
        frmMain.tsbFrame_fpY.Text = cfg_grid_footPrintY



    End Sub


    Sub config_write()


        Dim sFile As String = System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.cfg"


        ' Preview
        iniWrite(sFile, "preview", "bgColor", cfg_grid_BackGroundColor.ToArgb())
        iniWrite(sFile, "preview", "fgColor", cfg_grid_ForeGroundColor.ToArgb())
        iniWrite(sFile, "preview", "numPixels", cfg_grid_numPixels.ToString())
        iniWrite(sFile, "preview", "zoom", cfg_grid_zoom.ToString())
        iniWrite(sFile, "preview", "footPrintX", cfg_grid_footPrintX.ToString())
        iniWrite(sFile, "preview", "footPrintY", cfg_grid_footPrintY.ToString())


        ' Reads from ini and configures all.
        iniWrite(sFile, "paths", "root", cfg_path_Root)
        iniWrite(sFile, "paths", "recentPNG", cfg_path_recentPNG)
        iniWrite(sFile, "paths", "recentZT1", cfg_path_recentZT1)
        'iniWrite(sFile, "paths", "pal8", cfg_path_ColorPals8)
        'iniWrite(sFile, "paths", "pal16", cfg_path_ColorPals16)


        ' Export PNG (frames)
        iniWrite(sFile, "exportOptions", "pngSize", cfg_export_PNG_CanvasSize.ToString())
        iniWrite(sFile, "exportOptions", "pngRenderExtraFrame", cfg_export_PNG_RenderBGFrame.ToString())
        iniWrite(sFile, "exportOptions", "pngRenderExtraGraphic", cfg_export_PNG_RenderBGZT1.ToString())

        ' Export ZT1 (entire graphic)
        iniWrite(sFile, "exportOptions", "zt1Ani", cfg_export_ZT1_Ani.ToString())
        iniWrite(sFile, "exportOptions", "zt1AlwaysAddZTAFBytes", cfg_export_ZT1_AlwaysAddZTAFBytes.ToString())

        ' Convert options ( ZT1 <=> PNG )
        iniWrite(sFile, "conversionOptions", "pngFilesIndex", cfg_convert_startIndex.ToString())
        iniWrite(sFile, "conversionOptions", "deleteOriginal", cfg_convert_deleteOriginal.ToString())
        iniWrite(sFile, "conversionOptions", "overwrite", cfg_convert_deleteOriginal.ToString())

        ' Frame editing
        iniWrite(sFile, "editing", "individualRotationFix", cfg_editor_rotFix_individualFrame.ToString())



    End Sub

    Public Sub cleanUp_ZT1(strPath As String)
        On Error GoTo dBug

0:
5:


        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string

15:


            Dim dir As String = stack.Pop

            ' Add all immediate file paths
            'result.AddRange(Directory.GetFiles(dir, "*.*"))

20:
            For Each f As String In Directory.GetFiles(dir, "*")
                ' Only ZT1 files
                If Path.GetExtension(f) = "" Then
                    result.Add(f)
                End If
            Next

25:
            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop



1000:
        ' For each file that is a ZT1 Graphic:
        For Each f As String In result
            'Debug.Print(f)
            System.IO.File.Delete(f)

        Next

        Exit Sub

dBug:

        MsgBox("An error occured while trying to clean up ZT1 Graphic files in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during clean up")


    End Sub

    Public Sub cleanUp_ZT1Pals(strPath As String)
        On Error GoTo dBug

0:
5:


        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string

15:


            Dim dir As String = stack.Pop

            ' Add all immediate file paths
            'result.AddRange(Directory.GetFiles(dir, "*.*"))

20:
            For Each f As String In Directory.GetFiles(dir, "*.pal")
                ' Only ZT1 files
                If Path.GetExtension(f) = ".pal" Then
                    result.Add(f)
                End If
            Next

25:
            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop



1000:
        ' For each file that is a ZT1 Graphic:
        For Each f As String In result
            'Debug.Print(f)
            System.IO.File.Delete(f)

        Next

        Exit Sub

dBug:

        MsgBox("An error occured while trying to clean up ZT1 Color palettes in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during clean up")


    End Sub
    Public Sub cleanUp_PNG(strPath As String)
        On Error GoTo dBug

0:
5:


        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string

15:


            Dim dir As String = stack.Pop

            ' Add all immediate file paths
            'result.AddRange(Directory.GetFiles(dir, "*.*"))

20:
            For Each f As String In Directory.GetFiles(dir, "*")
                ' Only ZT1 files
                If Path.GetExtension(f).ToLower() = ".png" Then
                    result.Add(f)
                End If
            Next

25:
            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop



1000:
        ' For each file that is a ZT1 Graphic:
        For Each f As String In result
            'Debug.Print(f)
            System.IO.File.Delete(f)

        Next

        Exit Sub

dBug:

        MsgBox("An error occured while trying to clean up PNG files in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during clean up")

    End Sub

    Public Sub convert_file_ZT1_to_PNG(strFile As String)

        ' This function will convert a ZT1 file to a PNG file.
        ' It will take the rendering and exporting options into account.


        On Error GoTo dBg

5:

        Dim g As New clsGraphic2


        g.read(strFile)


        ' We have the graphic. We will need to render each frame.
        'Debug.Print("Frames in " & Path.GetFileName(strFile) & " : " & g.frames.Count)


        ' We will render this set of images.
        ' However, there are two main options:
        ' - keep canvas size / to relevant frame area / to relevant graphic area
        ' - render extra frame or not

10:

        For Each ztFrame As clsFrame In g.frames

11:

            'Debug.Print("Current name: " & (g.frames.IndexOf(ztFrame) + cfg_convert_startIndex))


            ' the bitmap's save function does not overwrite, nor warn 
            System.IO.File.Delete(strFile & "_" & (g.frames.IndexOf(ztFrame) + cfg_convert_startIndex).ToString("0000") & ".png")

            ' Save frames as PNG.
            ' Just autonumber the frames.
            ' Eexcept: if we have an extra frame which should be rendered separately rather than as background
            If cfg_export_PNG_RenderBGFrame = True And g.extraFrame = 1 Then
                If g.frames.IndexOf(ztFrame) <> (g.frames.Count - 1) Then
                    ztFrame.savePNG(strFile & "_" & (g.frames.IndexOf(ztFrame) + cfg_convert_startIndex).ToString("0000") & ".png")
                Else

                    ztFrame.savePNG(strFile & "_extra.png")
                End If
            Else
                ztFrame.savePNG(strFile & "_" & (g.frames.IndexOf(ztFrame) + cfg_convert_startIndex).ToString("0000") & ".png")

            End If

12:

            '            'Debug.Print("TESTING " & editorBgGraphic.frames.Count)

            '            ' Is there a BG ZT1 Graphic?
            '            If editorBgGraphic.frames.Count > 0 And cfg_export_PNG_RenderBGZT1 = True Then

            '                bm = editorBgGraphic.frames(0).renderFrame().Clone()

            '            Else

            '                bm = New Bitmap(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2)

            '            End If



            '            ' There's an extra frame. What's the rendering option?
            '            If g.extraFrame = 1 Then

            '20:
            '                ' Does the user want us to render the individual frames?

            '21:
            '                ' Write out ALL frames as seperate items.
            '                If g.frames.IndexOf(ztFrame) <> (g.frames.Count - 1) Then

            '                    ' Regular frames
            '                    ztFrame.renderFrame(bm, , cfg_export_PNG_RenderBGFrame).Save(strFile & "_" & (g.frames.IndexOf(ztFrame) + cfg_convert_startIndex).ToString("0000") & ".png")

            '                Else
            '23:
            '                    ' The extra frame
            '                    ztFrame.renderFrame(bm, , False).Save(strFile & "_extra.png")

            '                End If



            '            Else

            '30:
            '                ' Write out all frames
            '                Debug.Print("TESTING render a regular frame. " & g.frames.IndexOf(ztFrame) & " + " & cfg_convert_startIndex)
            '                ztFrame.renderFrame(bm, g.colorPalette).Save(strFile & "_" & (g.frames.IndexOf(ztFrame) + cfg_convert_startIndex).ToString("0000") & ".png")

            '            End If


            'Debug.Print("Frame OK.")

        Next



        Debug.Print("Converted file from ZT1 to PNG.")

        Exit Sub

dBg:
        MsgBox("Error occured in convert_file_ZT1_to_PNG:" & vbCrLf & "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error in conversion ZT1 -> PNG")


    End Sub
    Public Sub convert_file_PNG_to_ZT1(strPath As String, Optional blnSingleConversion As Boolean = True)

        On Error GoTo dBg


        ' In this sub, we get the file name of a PNG image we'll convert..


0:



        Dim paths() As String
        Dim g As New clsGraphic2
        Dim ztFrame As clsFrame
        Dim frameName As String = System.IO.Path.GetFileName(strPath)
        Dim frameGraphicPath As String = Strings.Left(strPath, strPath.Length - frameName.Length)


        Dim pngName As String

10:
        Debug.Print("Graphic: " & strPath)



        paths = System.IO.Directory.GetFiles(frameGraphicPath, frameName & "_*.png")

20:
        ' 20150624 : this should be done automatically when writing (?)
        ' We're creating the image from scratch.
        ' This means we need to start defining a color palette.
        '  g.colorPalette.fileName = strPath & ".pal"


100:

        For Each s As String In paths

            ' The order is alphabetical.
            ' We could implement a way to check if the filename matches the expected frame

105:
            ' only last part!
            pngName = Split(System.IO.Path.GetFileNameWithoutExtension(s), "_")(1)

110:

            If pngName.Length <> 4 And pngName <> "extra" Then

                MsgBox("A .PNG-file has been detected which does not match the pattern expected." & vbCrLf & _
                    "The files should be named something similar to: " & vbCrLf & _
                    frameName & "_000" & cfg_convert_startIndex & ".png (number increases)" & vbCrLf & _
                    "or " & frameName & "_extra.png (for the extra frame in certain ZT1 Graphics." & vbCrLf & vbCrLf & _
                    "File which caused this error:" & vbCrLf & s & vbCrLf & pngName, vbOKOnly + vbCritical, "Error")
                Exit Sub

            Else

120:

                If pngName = "extra" Then
                    ' there's an extra frame
                    g.extraFrame = 1

                ElseIf (CInt(pngName) - cfg_convert_startIndex).ToString("0000") <> g.frames.Count.ToString("0000") Then

135:
                    ' Check if file name pattern is okay
                    MsgBox("The file name ('" & frameGraphicPath & "_" & (CInt(pngName) - cfg_convert_startIndex).ToString("0000") & ".png') does not match the expected name " & _
                           "('" & frameGraphicPath & "_" & g.frames.Count.ToString("0000") & ".png')" & vbCrLf & vbCrLf & _
                           "Your current starting index is:" & cfg_convert_startIndex & vbCrLf & _
                           "Do not store other .png-files starting with '" & frameGraphicPath & "' in that folder.", vbOKOnly + vbCritical, "Error")
                    Exit Sub

                End If

130:
                ztFrame = New clsFrame
                'ztFrame.cachedFrame = Bitmap.FromFile(s)
                ztFrame.parent = g ' Reference needed for color palette
                g.frames.Add(ztFrame)


140:
                'Dim bm As Bitmap
                'bm = Bitmap.FromFile(s)
                'Debug.Print(s)

150:
                With ztFrame

151:
                    .loadPNG(s)
185:
                    .renderFrame(Nothing, Nothing, False)

                End With


199:

                'Debug.Print("----- exp: " & ztFrame.width & ", " & ztFrame.height & " - " & bm.Width & ", " & bm.Height)

200:
                'frmMain.picBox.Image = ztFrame.cachedFrame


            End If


        Next


500:


510:

        'Debug.Print("Configure speed")
        'g.animationSpeed = 1000 ' no idea. Would it be possible to batch-apply this?


530:

        Debug.Print("Write to " & strPath)
        g.write(strPath)


        ' For each ZT1 File
        ' For Each s As String In filesZT1
        'Debug.Print("ZT1 File: " & s)
        ' Next


55:
        If cfg_export_ZT1_Ani = 1 And blnSingleConversion = True Then
            Dim cAni As New clsAniFile
            cAni.fileName = strPath.Replace(IO.Path.GetFileName(strPath), "")
            cAni.fileName = cAni.fileName & IO.Path.GetFileName(cAni.fileName.Substring(0, cAni.fileName.Length - 1)) & ".ani"
            cAni.guessConfig()
        End If



        Debug.Print("Converted file from ZT1 to PNG.")

        Exit Sub

dBg:
        MsgBox("Error occured in convert_file_PNG_to_ZT1:" & vbCrLf & "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error in conversion PNG -> ZT1")


    End Sub
    Public Sub convert_folder_ZT1_to_PNG(strPath As String, Optional PB As ProgressBar = Nothing)


        On Error GoTo dBug

0:
5:


        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(strPath)



10:

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string

15:


            Dim dir As String = stack.Pop

            ' Add all immediate file paths
            'result.AddRange(Directory.GetFiles(dir, "*.*"))

20:



            For Each f As String In Directory.GetFiles(dir, "*")
                ' Only ZT1 files
                If Path.GetExtension(f) = vbNullString Then
                    result.Add(f)
                End If
            Next

25:
            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop





        If IsNothing(PB) = False Then
            PB.Minimum = 0
            PB.Value = 0
            PB.Maximum = result.Count
        End If

1000:
        ' For each file that is a ZT1 Graphic:
        For Each f As String In result
            Debug.Print(f)
            clsTasks.convert_file_ZT1_to_PNG(f)
            pb.value += 1
        Next

        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and convert ZT1 Graphic files in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during ZT1 to PNG batch conversion")

    End Sub
    Public Sub convert_folder_PNG_to_ZT1(strPath As String, Optional PB As ProgressBar = Nothing)

        ' We have the paths.
        ' We should get the unique prefixes.


        On Error GoTo dBug

0:
5:


        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string

15:


            Dim dir As String = stack.Pop

            ' Add all immediate file paths
            'result.AddRange(Directory.GetFiles(dir, "*.*"))

20:
            For Each f As String In Directory.GetFiles(dir, "*")
                ' Only ZT1 files
                If Path.GetExtension(f).ToLower = ".png" Then
                    If result.Contains(Strings.Split(f, "_")(0)) = False Then
                        result.Add(Split(f, "_")(0))

                    End If
                End If
            Next

25:
            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop



        If IsNothing(PB) = False Then
            PB.Minimum = 0
            PB.Value = 0
            PB.Maximum = result.Count
        End If

1000:
        ' For each file that is a ZT1 Graphic:
        For Each f As String In result
            'Debug.Print(f)
            clsTasks.convert_file_PNG_to_ZT1(f, False)
            PB.Value += 1

            Application.DoEvents()

        Next


1100:
        ' For each folder:
        Debug.Print("For stack")
        Debug.Print(stack.Count)



        ' Add the initial directory
        stack.Push(strPath)
        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string

            Dim dir As String = stack.Pop


            If cfg_export_ZT1_Ani = 1 Then
                Dim cAni As New clsAniFile
                cAni.fileName = dir & "\" & Path.GetFileName(dir) & ".ani"
                cAni.guessConfig()
            End If

            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop





        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and convert PNG files in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during PNG to ZT1 batch conversion")

    End Sub




    Public Sub preview_set_background(strZT1GraphicFileName As String)

        ' Read.
        editorBgGraphic.read(strZT1GraphicFileName)

    End Sub
    Public Sub preview_update(Optional intIndexFrameNumber As Integer = -1)

        If editorGraphic.frames.Count = 0 Then Exit Sub


        If intIndexFrameNumber = -1 Then
            intIndexFrameNumber = frmMain.tbFrames.Value - 1
        End If

20:
        frmMain.picBox.Image = editorGraphic.frames(intIndexFrameNumber).getImage(True)

21:
        ' Frame index 
        '  frmMain.tslFrame_Index.Text = intIndexFrameNumber & "/" & (editorGraphic.frames.Count - 1 - editorGraphic.extraFrame)
        'frmMain.tslFrame_Index.Text = IIf(editorGraphic.frames.Count = 0, "-", (intIndexFrameNumber + cfg_convert_startIndex) & " / " & (editorGraphic.frames.Count - cfg_convert_startIndex - editorGraphic.extraFrame))

        'Debug.Print("updated preview")


30:
        editorFrame = editorGraphic.frames(intIndexFrameNumber)

35:
        clsTasks.update_Info("Preview updated.")


        Debug.Print("# Preview updated, now showing frame " & intIndexFrameNumber & ". Default: " & (frmMain.tbFrames.Value - 1))


    End Sub



    Function bitmap_getCropped(bmInput As Bitmap, rect As Rectangle) As Bitmap


        Return bmInput.Clone(rect, bmInput.PixelFormat)


    End Function


    Function bitmap_getDefiningRectangle(bmInput As Bitmap) As Rectangle


101:
        ' Find most left
        ' Find most top
        ' Find most right
        ' Find most bottom

        Dim coordX As Integer = 0
        Dim coordY As Integer = 0

        Dim coordA As New Point(bmInput.Width, bmInput.Height)
        Dim coordB As New Point(0, 0)
        Dim curColor As System.Drawing.Color
        Dim curTransparentColor As System.Drawing.Color = bmInput.GetPixel(0, 0)


        'Debug.Print("bitmap - getDefiningRectangle - transp: " & curTransparentColor.ToString())


        While coordX <= (bmInput.Width - 1)

            coordY = 0
            While coordY <= (bmInput.Height - 1)

                ' Get color
                curColor = bmInput.GetPixel(coordX, coordY)
                'Debug.Print(coordX & "," & coordY & " => " & curColor.ToString())

                If IsNothing(curColor) = True And curColor.A <> 255 Then
                    curColor = bmInput.GetPixel(coordX, coordY)
                End If

                If curColor <> curTransparentColor And curColor.A = 255 Then
                    ' Color pixel
                    'Debug.Print(curColor.ToString())

                    If coordX < coordA.X Then coordA.X = coordX ' Topleft: move to left
                    If coordY < coordA.Y Then coordA.Y = coordY ' Topleft: move to top

                    If coordX > coordB.X Then coordB.X = coordX ' Bottomright: move to right
                    If coordY > coordB.Y Then coordB.Y = coordY ' Bottomright: move to bottom

                End If

                coordY += 1

            End While
            coordX += 1

        End While


        ' enabled for cropping of frames, 20150619
        coordB.X += 1
        coordB.Y += 1


        Debug.Print("w,h=" & coordA.X & "," & coordA.Y & " --- " & coordB.X & "," & coordB.Y)
        Return New Rectangle(coordA.X, coordA.Y, coordB.X - coordA.X, coordB.Y - coordA.Y)

        'Debug.Print("Rectangle definition: x,y,width,height: " & r.X & " - " & r.Y & " - " & r.Width & " - " & r.Height)

        ' Return r


    End Function


    Public Function images_Combine(ByVal imgBack As Image, ByVal imgFront As Image) As Image

        Dim bmp As New Bitmap(imgBack.Width, imgBack.Height)
        Dim g As Graphics = Graphics.FromImage(bmp)

        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor ' prevent softening
        g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height)
        g.DrawImage(imgFront, 0, 0, imgFront.Width, imgFront.Height)
        g.Dispose()
        Return bmp

    End Function


    ' === Extra ===
    Sub update_Info(strReason As String)

        ' Displays updated info.

        With frmMain


            .tstZT1_AnimSpeed.Text = editorGraphic.animationSpeed
            .tslFrame_Index.Text = IIf(editorGraphic.frames.Count = 0, "-", (editorGraphic.frames.IndexOf(editorFrame) + cfg_convert_startIndex) & " / " & (editorGraphic.frames.Count - 1 + cfg_convert_startIndex - editorGraphic.extraFrame))

            Debug.Print("updated info. [" & strReason & "]. Total frames = " & (editorGraphic.frames.Count - cfg_convert_startIndex - editorGraphic.extraFrame))


            With .tbFrames
                .Minimum = 1
                .Maximum = (editorGraphic.frames.Count - editorGraphic.extraFrame) ' for actually generated files: - editorGraphic.extraFrame)

                If .Maximum < 1 Then
                    .Minimum = 1
                    .Maximum = 1
                End If
                If .Value < .Minimum Then .Value = .Minimum

            End With


            ' == Graphic
            .tsbGraphic_ExtraFrame.Enabled = (editorGraphic.frames.Count > 1)
            .tsbGraphic_ExtraFrame.Checked = (editorGraphic.extraFrame = 1)

            ' == Frame
            .tsbFrame_Delete.Enabled = (editorGraphic.frames.Count > 1)
            .tsbFrame_ExportPNG.Enabled = False

            '(IsNothing(editorGraphic.frames(0).cachedFrame) = False)

            If IsNothing(editorFrame) = False Then
                If IsNothing(editorFrame.cachedFrame) = False Then
                    .tsbFrame_ExportPNG.Enabled = True
                End If
            End If


            .tsbFrame_ImportPNG.Enabled = (editorGraphic.frames.Count > 0)

            .tsbFrame_OffsetDown.Enabled = (editorGraphic.frames.Count > 0)
            .tsbFrame_OffsetUp.Enabled = (editorGraphic.frames.Count > 0)
            .tsbFrame_OffsetLeft.Enabled = (editorGraphic.frames.Count > 0)
            .tsbFrame_OffsetRight.Enabled = (editorGraphic.frames.Count > 0)

            .tsbFrame_IndexIncrease.Enabled = (editorGraphic.frames.Count > 1 And editorGraphic.frames.IndexOf(editorFrame) <> (editorGraphic.frames.Count - 1 - editorGraphic.extraFrame))
            .tsbFrame_IndexDecrease.Enabled = (editorGraphic.frames.Count > 1 And editorGraphic.frames.IndexOf(editorFrame) <> 0)

            .picBox.BackColor = cfg_grid_BackGroundColor

            If IsNothing(editorFrame) Then
                .tslFrame_Offset.Text = "0 , 0"

            Else
                '.tbFrames.Value = editorGraphic.frames.IndexOf(editorFrame) + 1
                .tslFrame_Offset.Text = editorFrame.offsetX & " , " & editorFrame.offsetY
            End If




        End With





    End Sub



    Function grid_drawFootPrintXY(intFootPrintX As Integer, intFootPrintY As Integer, view As Byte, Optional bmInput As Bitmap = Nothing) As Bitmap



        If IsNothing(bmInput) = True Then
            bmInput = New Bitmap(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2)
        End If

        ' This function calculates where to put the center of the image.
        ' View:
        ' 0 = SE
        ' 1 = SW
        ' 2 = NE
        ' 3 = NW

        ' SE, x10, y8 = front view, 5 squares. Side: 4 squares.

        ' We will need to draw a bitmap with our squares.
        ' But first, we'll need to calculate the top left pixel of the center of our grid.

        Dim intWidth As Integer = (intFootPrintX + intFootPrintY) * 16
        Dim intHeight As Integer = intWidth / 2

        ' eerste punt tov onze gegenereerde grid: +16px Y (center)
        ' eerste punt tov onze gegenereerde grid: intFootprintX * 32



        ' we need to find the center of our generated grid.
        ' next, we need to align it with the center of our image.



        ' We know the starting coordinate. We know the number of squares. We know if it's even or not (= extra row) 
        ' We will keep track of how many squares we draw.
        ' We will not draw more squares than our max width.
        Dim intRow As Integer = 1


        Dim coord As New Point(0, 0)

        '
        'Dim curPoint As Integer = 1
        'Dim numSquaresInRow As Integer = 1
        'Dim curSquare As Integer = 0

        Dim intAddition As Integer = 1



        coord.X = (intFootPrintX / 2) * 32 ' possibly adjustment of 16px needed (o the left?
        Debug.Print(intHeight)

        coord.Y = 0 'cfg_grid_numPixels '- (intHeight / 2)

        ' Think with X=10,Y=8
        Dim intCurFootPrintX As Integer
        Dim intCurFootPrintY As Integer

        For intCurFootPrintX = 2 To intFootPrintX Step 2

            ' Starting point:
            coord.X = cfg_grid_numPixels - (intWidth / 2) + (intFootPrintX / 2 * 32)
            ' coord.X = coord.X - (intFootPrintX / 2) * 32
            'coord.X = coord.X + (intCurFootPrintX / 2) * 32
            'coord.X = coord.X - ((intFootPrintX / 2) * 32 / 2) + (intCurFootPrintX / 2) * 32


            'coord.X = cfg_grid_numPixels - (((intFootPrintX - intCurFootPrintX) / 2) * 32)

            coord.X = cfg_grid_numPixels - (intWidth / 2)  ' Move to the left
            coord.X = coord.X + ((intFootPrintX - intCurFootPrintX) / 2) * 32  ' What can we add?

            'Debug.Print("X = " & coord.X)



            coord.Y = cfg_grid_numPixels - (intHeight / 2) + 16 * (intCurFootPrintX / 2)
            coord.Y -= 16


            'Debug.Print("Start: " & coord.X & "," & coord.Y)


            ' Draw the first block, which is easy. 
            For intCurFootPrintY = 2 To intFootPrintY Step 2

                ' For each
                coord.X += 32
                coord.Y += 16

                'Debug.Print(" --> " & coord.X & "," & coord.Y)

                grid_drawSquare(coord, bmInput)

            Next





        Next

        Return bmInput




    End Function

    Function grid_drawSquare(coordTopLeft As Point, Optional bmInput As Bitmap = Nothing) As Bitmap

        If IsNothing(bmInput) = True Then
            bmInput = bm
        End If

        Dim intX As Integer = 0
        Dim intY As Integer = 0

        ' === Top left
        intY = 0
        For intX = -31 To 0

            bmInput.SetPixel(coordTopLeft.X + intX, coordTopLeft.Y + intY, cfg_grid_ForeGroundColor)

            ' Mirror to the right
            bmInput.SetPixel(coordTopLeft.X + 1 - intX, coordTopLeft.Y + intY, cfg_grid_ForeGroundColor)

            ' Same for bottom
            bmInput.SetPixel(coordTopLeft.X + intX, coordTopLeft.Y - intY + 1, cfg_grid_ForeGroundColor)
            bmInput.SetPixel(coordTopLeft.X + 1 - intX, coordTopLeft.Y - intY + 1, cfg_grid_ForeGroundColor)

            If intX Mod 2 = 0 Then
                intY -= 1
            End If

        Next

        ' === Center = 4px
        bmInput.SetPixel(coordTopLeft.X, coordTopLeft.Y, cfg_grid_ForeGroundColor)
        bmInput.SetPixel(coordTopLeft.X, coordTopLeft.Y + 1, cfg_grid_ForeGroundColor)
        bmInput.SetPixel(coordTopLeft.X + 1, coordTopLeft.Y, cfg_grid_ForeGroundColor)
        bmInput.SetPixel(coordTopLeft.X + 1, coordTopLeft.Y + 1, cfg_grid_ForeGroundColor)



        'picBox.Image = bmInput

        Return bmInput


    End Function


    Sub pal_replaceColor(intIndex As Integer)


        With frmMain.dlgColor
            .Color = frmMain.dgvPaletteMain.Rows(intIndex).DefaultCellStyle.BackColor

            .AllowFullOpen = True
            .FullOpen = True
            .SolidColorOnly = True
            .ShowDialog()

        End With


        editorGraphic.colorPalette.colors(intIndex) = frmMain.dlgColor.Color



        'frmMain.dgvPaletteMain.Rows(intIndex).DefaultCellStyle.BackColor = frmMain.dlgColor.Color
        'frmMain.dgvPaletteMain.Rows(intIndex).DefaultCellStyle.SelectionBackColor = frmMain.dlgColor.Color  ' prevent selection highlighting (blue)

        'On Error Resume Nex

        editorGraphic.colorPalette.fillPaletteGrid(frmMain.dgvPaletteMain)

    End Sub

    Sub pal_moveColor(intIndexNow As Integer, intIndexDest As Integer)

        ' Get color
        Dim c As System.Drawing.Color = editorGraphic.colorPalette.colors(intIndexNow)

        ' Delete the original.
        editorGraphic.colorPalette.colors.RemoveAt(intIndexNow)

        ' We had the color. Insert it at the position we want.
        editorGraphic.colorPalette.colors.Insert(intIndexDest, c)

        ' Refresh
        editorGraphic.colorPalette.fillPaletteGrid(frmMain.dgvPaletteMain)



    End Sub

    Sub pal_addColor(intIndexNow As Integer)

        ' Get color
        Dim c As System.Drawing.Color = Color.Transparent
         

        ' We had the color. Insert it at the position we want.
        editorGraphic.colorPalette.colors.Insert(intIndexNow + 1, c)

        ' Refresh
        editorGraphic.colorPalette.fillPaletteGrid(frmMain.dgvPaletteMain)



    End Sub



    Sub openPal(strFileName As String)

        If File.Exists(strFileName) Then

            If Path.GetExtension(strFileName) <> ".pal" Then
                MsgBox("You did not select a .pal file.", vbOKOnly + vbCritical, "Invalid file")

            Else

                Dim c As New clsPalette
                Dim frmColPal As New frmPal

                If c.readPalette(strFileName) <> 0 Then


                    c.fillPaletteGrid(frmColPal.dgvPal)

                    frmColPal.ssFileName.Text = Path.GetFileName(strFileName)

                    If c.colors.Count <> 9 And c.colors.Count <> 17 Then
                        'frmColPal.Controls.Remove(frmColPal.btnUseInMainPal)
                    End If

                    frmColPal.Show()


                End If

            End If


        End If



    End Sub


End Module
