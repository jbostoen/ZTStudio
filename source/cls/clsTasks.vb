Option Explicit On


Imports System.IO
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

' This module just combines a few of the functions we've created.



Module clsTasks



    ' The graphic writer will need to take care of the following things:

    ' (1) Write frames properly.



    Public Function config_load() As Integer

        ' This tasks reads all settings from the .ini-file.
        ' For an explanation of these parameters: check mdlSettings.vb

        On Error GoTo dBug


10:
        Dim sFile As String = System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.cfg"

        If File.Exists(sFile) = False Then
            If MsgBox("ZT Studio is missing the settings.cfg file.", vbOKOnly + vbCritical + vbOK) = vbOK Then
                End
            End If
        End If
        'On Error Resume Next



20:
        ' Preview
        cfg_grid_BackGroundColor = System.Drawing.Color.FromArgb(CInt(iniRead(sFile, "preview", "bgColor", "")))
        cfg_grid_ForeGroundColor = System.Drawing.Color.FromArgb(CInt(iniRead(sFile, "preview", "fgColor", "")))
        cfg_grid_numPixels = CInt(iniRead(sFile, "preview", "numPixels", ""))
        cfg_grid_zoom = CInt(iniRead(sFile, "preview", "zoom", ""))
        cfg_grid_footPrintX = CByte(iniRead(sFile, "preview", "footPrintX", ""))
        cfg_grid_footPrintY = CByte(iniRead(sFile, "preview", "footPrintY", ""))

30:
        ' Reads from ini and configures all.
        cfg_path_Root = iniRead(sFile, "paths", "root", "")
        cfg_path_recentPNG = iniRead(sFile, "paths", "recentPNG", "")
        cfg_path_recentZT1 = iniRead(sFile, "paths", "recentZT1", "")
        cfg_path_ColorPals8 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal8"
        cfg_path_ColorPals16 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal16"

40:
        ' Export (PNG)
        cfg_export_PNG_CanvasSize = CInt(iniRead(sFile, "exportOptions", "pngCrop", ""))
        cfg_export_PNG_RenderBGFrame = CByte(iniRead(sFile, "exportOptions", "pngRenderExtraFrame", ""))
        cfg_export_PNG_RenderBGZT1 = CByte(iniRead(sFile, "exportOptions", "pngRenderExtraGraphic", ""))
        cfg_export_PNG_TransparentBG = CByte(iniRead(sFile, "exportOptions", "pngRenderTransparentBG", ""))

        ' Export (ZT1)
        cfg_export_ZT1_Ani = CByte(iniRead(sFile, "exportOptions", "zt1Ani", "1"))
        cfg_export_ZT1_AlwaysAddZTAFBytes = CByte(iniRead(sFile, "exportOptions", "zt1AlwaysAddZTAFBytes", ""))

50:
        ' Convert ( ZT1 <=> PNG, other way around )
        cfg_convert_startIndex = CInt(iniRead(sFile, "conversionOptions", "pngFilesIndex", ""))
        cfg_convert_deleteOriginal = CByte(iniRead(sFile, "conversionOptions", "deleteOriginal", ""))
        cfg_convert_overwrite = CByte(iniRead(sFile, "conversionOptions", "overwrite", ""))
        cfg_convert_sharedPalette = CByte(iniRead(sFile, "conversionOptions", "sharedPalette", ""))
        cfg_convert_fileNameDelimiter = CStr(iniRead(sFile, "conversionOptions", "fileNameDelimiter", ""))

60:
        ' Frame editing
        cfg_editor_rotFix_individualFrame = CByte(iniRead(sFile, "editing", "individualRotationFix", ""))
        cfg_frame_defaultAnimSpeed = CInt(iniRead(sFile, "editing", "animationSpeed", ""))

70:
        ' Palette
        cfg_palette_import_png_force_add_colors = CByte(iniRead(sFile, "palette", "importPNGforceAddColors", ""))


100:

        ' Now, if our path is no longer valid, pop up 'Settings'-window automatically
        If System.IO.Directory.Exists(cfg_path_Root) = False Then


            ' But let's give some suggestions.
            cfg_path_Root = System.IO.Path.GetFullPath(Application.StartupPath)

            ' Also give suggestions for color palettes.
            If System.IO.Directory.Exists(cfg_path_ColorPals8) = False And System.IO.Directory.Exists(Application.StartupPath & "\pal8") = True Then
                cfg_path_ColorPals8 = cfg_path_Root & "\pal8"
            End If
            If System.IO.Directory.Exists(cfg_path_ColorPals16) = False And System.IO.Directory.Exists(Application.StartupPath & "\pal16") = True Then
                cfg_path_ColorPals8 = cfg_path_Root & "\pal16"
            End If

            ' Now show the settings dialog.
            frmSettings.ShowDialog()

        End If

200:

        ' No recent paths yet?
        If cfg_path_recentPNG = "" Then
            cfg_path_recentPNG = cfg_path_Root
        End If
        If cfg_path_recentZT1 = "" Then
            cfg_path_recentZT1 = cfg_path_Root
        End If

        ' Paths invalid?
        If System.IO.Directory.Exists(cfg_path_recentPNG) = False Then
            cfg_path_recentPNG = cfg_path_Root
        End If
        If System.IO.Directory.Exists(cfg_path_recentZT1) = False Then
            cfg_path_recentZT1 = cfg_path_Root
        End If



        ' Only now should we create our objects.

        editorGraphic = New clsGraphic2         ' The clsGraphic2 object we use.
        editorBgGraphic = New clsGraphic2       ' The background graphic, e.g. toy
         



        Return 0

dBug:
        If MsgBox("Error occurred when loading ZT Studio settings at line " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Failed to load settings") = vbOK Then End


    End Function


    Public Function config_write() As Integer

        ' This tasks writes all settings to the .ini-file.
        ' For an explanation of these parameters: check mdlSettings.vb


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


        ' Export PNG (frames)
        iniWrite(sFile, "exportOptions", "pngCrop", cfg_export_PNG_CanvasSize.ToString())
        iniWrite(sFile, "exportOptions", "pngRenderExtraFrame", cfg_export_PNG_RenderBGFrame.ToString())
        iniWrite(sFile, "exportOptions", "pngRenderExtraGraphic", cfg_export_PNG_RenderBGZT1.ToString())
        iniWrite(sFile, "exportOptions", "pngRenderTransparentBG", cfg_export_PNG_TransparentBG.ToString())

        ' Export ZT1 (entire graphic)
        iniWrite(sFile, "exportOptions", "zt1Ani", cfg_export_ZT1_Ani.ToString())
        iniWrite(sFile, "exportOptions", "zt1AlwaysAddZTAFBytes", cfg_export_ZT1_AlwaysAddZTAFBytes.ToString())

        ' Convert options ( ZT1 <=> PNG )
        iniWrite(sFile, "conversionOptions", "pngFilesIndex", cfg_convert_startIndex.ToString())
        iniWrite(sFile, "conversionOptions", "deleteOriginal", cfg_convert_deleteOriginal.ToString())
        iniWrite(sFile, "conversionOptions", "overwrite", cfg_convert_overwrite.ToString())
        iniWrite(sFile, "conversionOptions", "sharedPalette", cfg_convert_sharedPalette.ToString())
        iniWrite(sFile, "conversionOptions", "fileNameDelimiter", cfg_convert_fileNameDelimiter)

        ' Frame editing
        iniWrite(sFile, "editing", "individualRotationFix", cfg_editor_rotFix_individualFrame.ToString())
        iniWrite(sFile, "editing", "animationSpeed", cfg_frame_defaultAnimSpeed.ToString())

        ' Palette
        iniWrite(sFile, "palette", "importPNGforceAddColors", cfg_palette_import_png_force_add_colors.ToString())


        Return 0

    End Function

    Public Function cleanUp_files(strPath As String, strExtension As String) As Integer

        ' Expected as strExtension:
        ' ".png"
        ' ".pal"
        ' "" (ZT1 Graphics)

        On Error GoTo dBug

0:
5:


        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories within our <root> folder to process.
        ' We'll go through each subdirectory.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string

15:


            Dim dir As String = stack.Pop

20:
            ' Get all files and check if they match the extension (.pal, .png) or have no extension (ZT1 graphic)
            For Each f As String In Directory.GetFiles(dir, "*")
                If Path.GetExtension(f) = strExtension Then
                    result.Add(f)
                End If
            Next

25:
            ' Loop through all subdirectories and add them to the stack, so they're processed as well.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop



1000:
        ' For each file that matches:
        For Each f As String In result
            Debug.Print("Delete: " & f)
            System.IO.File.Delete(f)
        Next

        Return 0

dBug:

        MsgBox("An error occured while trying to clean up ZT1 Graphic files in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during clean up")


    End Function


    Public Sub convert_file_ZT1_to_PNG(strFile As String)

        ' This function will convert a ZT1 Graphic to a PNG file.
        ' It will first render the ZT1 Graphic and then it will export it to one or multiple .PNG-files.
        ' DO NOT implement a clean up of ZT1 Graphic/ZT1 Color Palette here.
        ' The color palette could be shared with other images, which would cause issues in a batch conversion!


        On Error GoTo dBg

5:
        ' Create a new instance of a ZT1 Graphic object.
        Dim g As New clsGraphic2

        ' Read the ZT1 Graphic
        g.read(strFile)

        ' We will render this set of frames within this ZT1 Graphic.
        ' However, there are two main options:
        ' - keep canvas size / to relevant frame area / to relevant graphic area
        ' - render extra frame or not



10:
        ' Loop over each frame of the ZT1 Graphic
        For Each ztFrame As clsFrame2 In g.frames

11:

            ' the bitmap's save function does not overwrite, nor warn 
            System.IO.File.Delete(strFile & cfg_convert_fileNameDelimiter & (g.frames.IndexOf(ztFrame) + cfg_convert_startIndex).ToString("0000") & ".png")

            ' Save frames as PNG, just autonumber the frames.
            ' Exception: if we have an extra frame which should be rendered separately rather than as background. 
            ' In that case, we will create a .PNG-file named <graphicname>_extra.png
            ' Since we are processing in batch, we (currently) do not offer the option to render a background ZT1 Graphic.
            ' This might however make a nice addition :)

            ' RenderBGFrame: this is read as: 'render this as BG for every frame'
            If cfg_export_PNG_RenderBGFrame = 0 And g.extraFrame = 1 Then
                If g.frames.IndexOf(ztFrame) <> (g.frames.Count - 1) Then
                    ztFrame.savePNG(strFile & cfg_convert_fileNameDelimiter & (g.frames.IndexOf(ztFrame) + cfg_convert_startIndex).ToString("0000") & ".png")
                Else
                    ztFrame.savePNG(strFile & cfg_convert_fileNameDelimiter & "extra.png")
                End If
            Else
                ztFrame.savePNG(strFile & cfg_convert_fileNameDelimiter & (g.frames.IndexOf(ztFrame) + cfg_convert_startIndex).ToString("0000") & ".png")

            End If

12:



        Next

13:


        Debug.Print("Converted file from ZT1 to PNG.")

        Exit Sub

dBg:
        MsgBox("Error occured in convert_file_ZT1_to_PNG:" & vbCrLf & "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error in conversion ZT1 -> PNG")


    End Sub
    Public Function convert_file_PNG_to_ZT1(strPath As String, Optional blnSingleConversion As Boolean = True) As Integer

        On Error GoTo dBg


        ' In this sub, we get the file name of a PNG image we'll convert.
        ' But the filename should NOT be that of the  .PNG, but rather the one for the final ZT1 graphic. 
        ' That's why it's named strPath for now. This functoin will find others in the same series.  
        ' Advise: Cleanup of .PNG files only happens automatically in batch conversions.

0:

        strPath = Strings.LCase(strPath)

        Dim strPathDir As String = Path.GetDirectoryName(strPath)


        Dim paths As New List(Of String)
        Dim g As New clsGraphic2
        Dim ztFrame As clsFrame2
        Dim graphicName As String = System.IO.Path.GetFileName(strPath)
        Dim frameGraphicPath As String = Strings.Left(strPath, strPath.Length - graphicName.Length)


        Dim pngName As String


10:
        Debug.Print("Convert PNG to ZT1: " & strPath & " (" & graphicName & ") " & Now.ToString("HH:mm:ss"))



        ' Get the entire list of .PNG files matching the naming convention for this graphic.
        ' Anything else is irrelevant to process.
        paths.AddRange(System.IO.Directory.GetFiles(frameGraphicPath, graphicName & cfg_convert_fileNameDelimiter & "????.png"))

        ' Fix for graphic_extra.PNG (legacy)
        If File.Exists(frameGraphicPath & graphicName & cfg_convert_fileNameDelimiter & "extra.png") = True Then
            paths.Add(frameGraphicPath & graphicName & cfg_convert_fileNameDelimiter & "extra.png")
            g.extraFrame = 1
        End If

20:
        ' 20150624 : this should be done automatically when writing (?)
        ' We're creating the image from scratch.
        ' This means we need to start defining a color palette.
        ' The lazy approach is to just take the graphic's name and add a .pal-extension.
        ' The benefit of this is that you can use more than 255 colors *in total* - although you're still limited to those for views.
        '  g.colorPalette.fileName = strPath & ".pal"


100:

        Debug.Print("Before going over each PNG: " & Now.ToString("HH:mm:ss"))
        For Each s As String In paths

            'Debug.Print(s)


            ' The order is alphabetical.
            ' We could implement a way to check if the filename matches the expected frame.
            ' We should also check if there's at least 2 frames when someone starts to use <name>_extra.PNG
            ' We should also keep track of the numbering, although this shouldn't cause too many issues.

105:
            ' Extract the number of the frame (or _extra) from the filename
            'pngName = Split(System.IO.Path.GetFileNameWithoutExtension(s), "_")(1)
            ' 20161007 - changed this to adapt to different filename delimiters, including an empty one.
            If Strings.Right(System.IO.Path.GetFileName(s).ToLower(), 9) = "extra.png" Then
                pngName = "extra"
            Else
                pngName = Strings.Right(System.IO.Path.GetFileNameWithoutExtension(s), 4)
            End If



110:


            If pngName.Length <> 4 And pngName <> "extra" Then

                ' This could occur if for some reason we have for instance a ne.png file instead of the expected ne[delimeter]0000.png file
                MsgBox("A .PNG-file has been detected which does not match the pattern expected." & vbCrLf & _
                    "The files should be named something similar to: " & vbCrLf & _
                    graphicName & cfg_convert_fileNameDelimiter & "000" & cfg_convert_startIndex & ".png (number increases)" & vbCrLf & _
                    "or " & graphicName & cfg_convert_fileNameDelimiter & "extra.png (for the extra frame in certain ZT1 Graphics." & vbCrLf & vbCrLf & _
                    "File which caused this error: " & vbCrLf & "'" & s & "'" & vbCrLf & _
                       "ZT Studio will close to prevent program or game crashes.", _
                        vbOKOnly + vbCritical + vbApplicationModal, _
                        "Invalid filename (pattern)")

                End


            Else

120:

                If pngName = "extra" Then
                    ' There's an extra background frame.
                    g.extraFrame = 1

125:
                ElseIf IsNumeric(pngName) = True Then

                    ' Here we check whether the pattern is still appropriate.
                    ' The specification is that - depending on a variable to see if we start counting from 0 or 1 - 
                    ' the user should have PNGs named <graphicName><delimeter>0000.png, <graphicName><delimeter>0001.png, ... 
                    ' This checks if no number is skipped or invalid.
                    If (CInt(pngName) - cfg_convert_startIndex) <> g.frames.Count Then

135:
                        ' Check if file name pattern is okay
                        MsgBox("The file name ('" & frameGraphicPath & graphicName & cfg_convert_fileNameDelimiter & (CInt(pngName)).ToString("0000") & ".png') does not match the expected name " & _
                               "('" & frameGraphicPath & graphicName & cfg_convert_fileNameDelimiter & (g.frames.Count + cfg_convert_startIndex).ToString("0000") & ".png')" & vbCrLf & vbCrLf & _
                               "Your current starting index is: " & cfg_convert_startIndex & vbCrLf & _
                               "Do not store other .png-files starting with '" & frameGraphicPath & graphicName & "' in that folder.", vbOKOnly + vbCritical, "Error")

                        Return -1

                    End If

140:

                ElseIf pngName = Path.GetFileName(strPathDir) Then

                    ' This checks the last part of the directory path of this graphic.
                    ' One exception which we could accept, is an extremely uncommon one.
                    ' if it's a .PNG palette file, it could be a coincidence.
                    ' Difficult to come up with such an example at the moment.

145:
                Else

                    ' Non-numeric.
                    ' Non-extra.
                    ' This PNG is quite unexpected...


                    ' Check if file name pattern is okay
                    MsgBox("The file name ('" & frameGraphicPath & graphicName & cfg_convert_fileNameDelimiter & pngName & ".png') does not match the expected name " & _
                           "('" & frameGraphicPath & graphicName & cfg_convert_fileNameDelimiter & (g.frames.Count + cfg_convert_startIndex).ToString("0000") & ".png')" & vbCrLf & vbCrLf & _
                           "Your current starting index is: " & cfg_convert_startIndex & vbCrLf & _
                           "Do not store other .png-files starting with '" & frameGraphicPath & graphicName & "' in that folder.", vbOKOnly + vbCritical, "Error")

                    Return -1

                End If

200:
                ztFrame = New clsFrame2(g)


201:
                ' In case of a batch conversion, we might rely on a shared .PAL file 
                ' usually, this would be objects/restrant/restrant.pal
                ' animals/ibex/ibex.pal 

                ' to make it a bit more simple, and to allow for easier recoloring of baby (working on Red Panda now), 
                ' it would be better if the palette is not under animals/redpanda/redpanda.pal but animals/redpanda/m/redpanda.pal
                ' this should work for fences etc as well.

202:

                If cfg_convert_sharedPalette = 1 And blnSingleConversion = False Then

                    ' 20170513: changed behavior for even more flexibility. 
                    ' We try to get a shared palette:
                    ' - in the same folder as the graphic (animals/redpanda/m/walk - walk.pal) - in case this animation uses colors not used anywhere else.
                    ' - in the folder one level up (animals/redpanda/m - m.pal) - in case a palette is shared for the gender (male, female, young)
                    ' - in the folder two levels up (animals/redpanda - redpanda.pal) - in case a palette is shared for (most of) the animal
                    ' This method should also work just fine for objects.

                    Dim sPath0 As String
                    Dim sPath1 As String
                    Dim sPath2 As String


                    sPath0 = Path.GetDirectoryName(strPathDir)
                    sPath1 = Path.GetDirectoryName(sPath0)
                    sPath2 = Path.GetDirectoryName(sPath1)

                    sPath0 = sPath0 & "\" & Path.GetFileName(sPath0)
                    sPath1 = sPath1 & "\" & Path.GetFileName(sPath1)
                    sPath2 = sPath2 & "\" & Path.GetFileName(sPath2)




                    ' N should not be the only view (icon etc) in this folder.
                    ' If it does seem to be the only view, we should NOT fall back on higher level.
                    ' An icon is NOT animated and often contains very different colors (plaque, icon in menu). 
                    ' An exception to this rule could be the list icon, but it's not worth making an exception for it.

                    If Directory.GetFiles(strPathDir, graphicName & "*.png").Length <> _
                            Directory.GetFiles(strPathDir, "*.png").Length Then


                        ' 20170502 Optimized by Hendrix.
                        Dim inpaths() As String = {sPath0, sPath1, sPath2}
                        Dim exts() As String = {".pal", ".gpl", ".png"}

                        ' No palette has been saved/set yet for this graphic.
                        If ztFrame.parent.colorPalette.fileName = "" Then

                            ' Figure out if there is a preferred, already prepared palette to be used.
                            ' Two ideas come to mind here:
                            ' - palette at lower level folder gets priority over palette in higher level folder
                            ' - .pal (ZT1 Graphic) > .gpl (GIMP Palette) > .png

                            ' Debug.Print("### Trying to detect color palette. ")

                            For Each inpath As String In inpaths
                                For Each ext As String In exts

                                    If File.Exists(inpath & ext) = True Then
                                        With ztFrame.parent.colorPalette
                                            ' We only want to read a new palette once
                                            ' But we must ignore different extensions, so we don't reload in each loop

                                            ' Set filename.
                                            .fileName = inpath & ".pal"

                                            ' Now go by priority.
                                            ' Go-to is usually a bad practice, but it's good here to break our 2 (!) loops.
                                            Select Case ext
                                                Case ".pal"
                                                    .readPal(.fileName)
                                                    GoTo paletteReady
                                                Case ".gpl"
                                                    .import_from_GimpPalette(inpath & ext)
                                                    .writePal(.fileName, True)
                                                    GoTo paletteReady
                                                Case ".png"
                                                    .import_from_PNG(inpath & ext)
                                                    .writePal(.fileName, True)
                                                    GoTo paletteReady
                                            End Select

                                        End With
                                    End If
                                Next ext
                            Next inpath

paletteReady:

                        Else
                            ' Color palette has already been set for this graphic.
                            ' No further action needed.
                            ' Debug.Print(ztFrame.parent.fileName & " --- Color palette already exists.")
                        End If

                    End If




                End If


245:
                ' Add this frame to our parent graphic's frame collection 
                g.frames.Add(ztFrame)


250:
                ' Create a frame from the .PNG-file
                ztFrame.loadPNG(s)

            End If
        Next

        Debug.Print("Before write: " & Now.ToString("HH:mm:ss"))


1530:
        ' Create our ZT1 Graphic. 
        g.write(strPath)


        'Debug.Print("After write: " & Now.ToString("HH:mm:ss"))

1555:
        If cfg_export_ZT1_Ani = 1 And blnSingleConversion = True Then
            Debug.Print(Now.ToString() & ": convert_file_PNG_to_ZT1: Generate .ani file (single conversion)")

            ' Only 1 graphic file is being generated. This is the case for icons, for example.
            ' A .ani-file can be generated automatically.       
            ' [folder path] + \ + [folder name] + .ani
            Dim cAni As New clsAniFile(strPathDir & "\" & Path.GetFileName(strPathDir) & ".ani")
            cAni.createAniConfig()

        End If



        Debug.Print("Graphic converted: " & strPath & " (" & graphicName & ") " & Now.ToString("HH:mm:ss"))

9999:
        ' Clear everything.
        g = Nothing




        Return 0


dBg:
        MsgBox("Error occured in convert_file_PNG_to_ZT1:" & vbCrLf & "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error in conversion PNG -> ZT1")


    End Function
    Public Sub convert_folder_ZT1_to_PNG(strPath As String, Optional PB As ProgressBar = Nothing)

        ' This will find all ZT1 Graphics in a folder and generate PNGs from it. It works recursively.
        ' The progress can be shown in a progress bar.
        ' Batch conversion offers the feature to automatically clean up everything afterwards.

        On Error GoTo dBug

0:

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

        ' Set the initial configuration for a (optional) progress bar.
        ' We want the max value to be the number of ZT1 Graphics we're trying to convert.
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
            If IsNothing(PB) = False Then
                PB.Value += 1
            End If
        Next


1050:
        ' Should we do a clean up?
        If cfg_convert_deleteOriginal = 1 Then
            ' Currently ZT1 Graphics and ZT1 Color palettes have their own sub in which the files get deleted.
            ' It might be possible to merge them at some point and you could even gain a small performance boost.
            clsTasks.cleanUp_files(strPath, "")
            clsTasks.cleanUp_files(strPath, ".pal")
        End If

        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and convert ZT1 Graphic files in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during ZT1 to PNG batch conversion")

    End Sub
    Public Sub convert_folder_PNG_to_ZT1(strPath As String, Optional PB As ProgressBar = Nothing)

        ' We have the path containing .PNG-files which need to be converted into a ZT1 Graphic.
        ' We should get the unique prefixes. (eg. e_0001.png => e is the prefix. So 'e' should be the name of the view.


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
            Dim strGraphicName As String

            ' Add all immediate file paths 

20:
            For Each f As String In Directory.GetFiles(dir, "*.png")

                ' Add future graphic name ("full" path, eg animals/redpanda/m/walk/NE)
                If Strings.Right(Path.GetFileNameWithoutExtension(f).ToLower, 5) = "extra" Then
                    ' 5 (extra) + 4 (.png) = 9 chars.
                    ' eg objects/yourobj/NE_extra.png 
                    strGraphicName = Strings.Left(f, Len(f) - 9 - Len(cfg_convert_fileNameDelimiter))
                    ' Debug.Print("strgraphicname extra='" & strGraphicName & "'")
                Else
                    ' 4 (0000) + 4 (.png) = 8 chars. 
                    ' eg objects/yourobj/NE_0001.png 
                    strGraphicName = Strings.Left(f, Len(f) - 8 - Len(cfg_convert_fileNameDelimiter))
                    ' Debug.Print("strgraphicname='" & strGraphicName & "'")
                End If

                If result.Contains(strGraphicName) = False Then
                    result.Add(strGraphicName)
                End If

            Next

25:
            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)

                ' Just a warning, so users don't accidentally have "sitscratch" as animation name.
                ' Actually '-' is supported as well.
                If Path.GetFileName(directoryName).Length > 8 Or System.Text.RegularExpressions.Regex.IsMatch(Strings.Replace(Path.GetFileName(directoryName), "-", ""), "^[a-zA-Z0-9]+$") = False Then
                    MsgBox("Directory name '" & Path.GetFileName(directoryName) & "' is invalid." & vbCrLf & _
                        "The limit of a folder name is a maximum of 8 alphanumeric characters." & vbCrLf & _
                        "You will need to rename the folder manually and then retry." & vbCrLf & _
                       "ZT Studio will close to prevent program or game crashes.", _
                        vbOKOnly + vbCritical + vbApplicationModal, _
                        "Invalid directory name")

                    ' better:
                    End

                End If

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

            'Debug.Print("Convert file PNG to ZT1: " & f)
            clsTasks.convert_file_PNG_to_ZT1(f, False)
            If IsNothing(PB) = False Then
                PB.Value += 1
            End If

            Application.DoEvents()

        Next


1100:
        ' Generate a .ani-file in each directory. 
        ' Add the initial directory
        batch_generate_Ani(strPath)


1150:
        ' Do a clean up of our .PNG files if we had a successful conversion.
        If cfg_convert_deleteOriginal = 1 Then
            clsTasks.cleanUp_files(strPath, ".png")
        End If



        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and convert PNG files in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during PNG to ZT1 batch conversion")

    End Sub




    Public Sub update_preview(Optional intIndexFrameNumber As Integer = -1)


1:
        If editorGraphic.frames.Count = 0 Then Exit Sub

2:
        If intIndexFrameNumber = -1 Then
            intIndexFrameNumber = frmMain.tbFrames.Value - 1
        End If


3:
        ' The sub gets triggered when a new frame has been added, but no .PNG has been loaded yet
        If editorGraphic.frames(intIndexFrameNumber).coreImageHex.Count = 0 Then Exit Sub


20:
        frmMain.picBox.Image = editorGraphic.frames(intIndexFrameNumber).getImage(True)


21:
        ' Frame index 
        ' frmMain.tslFrame_Index.Text = intIndexFrameNumber & "/" & (editorGraphic.frames.Count - 1 - editorGraphic.extraFrame)
        ' frmMain.tslFrame_Index.Text = IIf(editorGraphic.frames.Count = 0, "-", (intIndexFrameNumber + cfg_convert_startIndex) & " / " & (editorGraphic.frames.Count - cfg_convert_startIndex - editorGraphic.extraFrame))

        'Debug.Print("updated preview") 

30:
        editorFrame = editorGraphic.frames(intIndexFrameNumber)

35:
        clsTasks.update_info("Preview updated.")
        'Debug.Print("# Preview updated, now showing frame " & intIndexFrameNumber & ". Default: " & (frmMain.tbFrames.Value - 1))


    End Sub



    Function bitmap_getCropped(bmInput As Bitmap, rect As Rectangle) As Bitmap

        Debug.Print("w,h=" & bmInput.Width & "," & bmInput.Height)


        Return bmInput.Clone(rect, bmInput.PixelFormat)




    End Function


    Function bitmap_getDefiningRectangle(bmInput As Bitmap) As Rectangle

        On Error GoTo dBug

        ' This new method using LockBits is much faster than a previous version where we were using GetPixel. 
        ' This is a big performance boost when loading 512x512 (canvas size) images.
        ' For now, the old function still exists to make sure we have a comparison if regressions are detected.

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


102:


        Const px As System.Drawing.GraphicsUnit = GraphicsUnit.Pixel
        Const fmtArgb As Imaging.PixelFormat = Imaging.PixelFormat.Format32bppArgb
        Dim boundsF As RectangleF = bmInput.GetBounds(px)
        Dim bounds As New Rectangle
        bounds.Location = New Point(CInt(boundsF.X), CInt(boundsF.Y))
        bounds.Size = New Size(CInt(boundsF.Width), CInt(boundsF.Height))
        Dim bmClone As Bitmap = bmInput.Clone(bounds, fmtArgb)
        Dim bmData As System.Drawing.Imaging.BitmapData = bmClone.LockBits(bounds, Imaging.ImageLockMode.ReadWrite, bmClone.PixelFormat)
        Dim offsetToFirstPixel As IntPtr = bmData.Scan0
        Dim byteCount As Integer = Math.Abs(bmData.Stride) * bmClone.Height
        Dim bitmapBytes(byteCount - 1) As Byte
        System.Runtime.InteropServices.Marshal.Copy(offsetToFirstPixel, bitmapBytes, 0, byteCount)

110:
        ' rectangle = bounds ?
        Dim StartOffset As Integer = (0 * bmData.Stride)
        Dim EndOffset As Integer = StartOffset + ((bmInput.Height) * bmData.Stride) - 1
        Dim RectLeftOffset As Integer = (0 * 4)
        Dim RectRightOffset As Integer = (0 + (bmInput.Width) * 4) - 1
        Dim X As Integer = 0
        Dim y As Integer = 0



        Debug.Print("from " & StartOffset & " to " & EndOffset & " (total bytes: " & bitmapBytes.Length & ")")
        Debug.Print("offset left from " & RectLeftOffset & " to " & RectRightOffset)


        Dim pixelLocation As Point



251:
        For FirstOffsetInEachLine As Integer = StartOffset To EndOffset Step bmData.Stride
            X = 0
            For PixelOffset As Integer = RectLeftOffset To RectRightOffset Step 4
                pixelLocation = New Point(X, y)
                'bitmapBytes(FirstOffsetInEachLine + PixelOffset) = FillColor.B
                'bitmapBytes(FirstOffsetInEachLine + PixelOffset + 1) = FillColor.G
                'bitmapBytes(FirstOffsetInEachLine + PixelOffset + 2) = FillColor.R
                'bitmapBytes(FirstOffsetInEachLine + PixelOffset + 3) = FillColor.A

                'If X > 510 And y > 510 Then
                '    Debug.Print(bitmapBytes(FirstOffsetInEachLine + PixelOffset + 3))
                'End If

                ' Non-transparent
                If bitmapBytes(FirstOffsetInEachLine + PixelOffset + 3) = 255 And _
                    bitmapBytes(FirstOffsetInEachLine + PixelOffset) <> curTransparentColor.B And _
                    bitmapBytes(FirstOffsetInEachLine + PixelOffset + 1) <> curTransparentColor.G And _
                    bitmapBytes(FirstOffsetInEachLine + PixelOffset + 2) <> curTransparentColor.R Then

                    ' We found a non transparent color
                    If X < coordA.X Then coordA.X = X ' Topleft: move to left
                    If y < coordA.Y Then coordA.Y = y ' Topleft: move to top

                    If y > coordB.Y Then coordB.Y = y ' Bottomright: move to bottom 
                    If X > coordB.X Then coordB.X = X ' Bottomright: move to right


                End If


                X += 1
            Next
            y += 1
        Next

        'Debug.Print("Last read pixel x=" & X & ",y=" & y)
        'Debug.Print(coordA.ToString() & " --- " & coordB.ToString())


        ' Unlock
        System.Runtime.InteropServices.Marshal.Copy(bitmapBytes, 0, offsetToFirstPixel, byteCount)
        bmClone.UnlockBits(bmData)




901:
        'MsgBox("w,h=" & coordA.X & "," & coordA.Y & " --- " & coordB.X & "," & coordB.Y)
        ' enabled for cropping of frames, 20150619
        coordB.X += 1
        coordB.Y += 1


999:
        ' 20170512 
        ' HENDRIX found out that transparent frames can cause issues.
        ' This is a more simple fix, since it seems this is valid in ZT1 after all?
        If coordA.X = bmInput.Width And coordA.Y = bmInput.Height Then
            coordA = New Point(0, 0)
            coordB = New Point(1, 1)
        End If



        ' Test comparison
        'Debug.Print("New defining x1,y1=" & coordA.X & "," & coordA.Y & " --- x2,y2 " & coordB.X & "," & coordB.Y)
        'Dim rectOld = bitmap_getDefiningRectangle_old(bmInput)
        'Debug.Print("Old defining x1,y1=" & rectOld.X & "," & rectOld.Y & " --- x2,y2 " & rectOld.Width + rectOld.X & "," & rectOld.Height + rectOld.Y)

        Return New Rectangle(coordA.X, coordA.Y, coordB.X - coordA.X, coordB.Y - coordA.Y)


        Exit Function

dBug:
        MsgBox("Error while obtaining the 'defining rectangle' of this graphic." & vbCrLf & _
            "Erl " & Erl() & " - " & Err.Number & " - " & Err.Description & vbCrLf & _
            "Last processed: " & pixellocation.x & " - " & pixellocation.y, _
            vbOKOnly + vbCritical, "Critical error")

    End Function
    Function bitmap_getDefiningRectangle_old(bmInput As Bitmap) As Rectangle

        On Error GoTo dBug


        ' Todo: this must be easier to go through. 
        ' It doesn't make sense to search through all pixels from left to right, top to bottom. 

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

        ' Optimized by HENDRIX
        ' I like the new rectangle code, seems to be a good speedup!

        ' However, I think  "If coordX >= coordA.X And coordX < coordB.X Then" can never be true. It works for Y. If we split it into
        ' two loops, we can make use Of that condition. First Loop As it Is, getting left, top and bottom.
        ' Second loop would swap While coordX and While coordY from the first loop, and only check the area that the other loop had not
        ' covered to get the right coord.

        ' I had this idea before reading through your new code:
        ' how about we split the while loops into four ones And break out of each as soon as we find a non-transparent pixel?
        ' go over each line from the top -> get coordA.Y
        ' go over each column from the left -> get coordA.X
        ' go over each line from the bottom -> get coordB.Y
        ' go over each column from the right -> get coordB.X
        ' that would speed things up in cases where there is relatively little padding on each side, but a large defining rectangle

        ' My new idea is a hybrid of that and your new code, and it runs a tiny bit faster and produces better results

        'first, crop away stuff from top and bottom
        ' Left to right 
        While coordX <= (bmInput.Width - 1)

            ' Top to bottom
            coordY = 0
            While coordY <= (bmInput.Height - 1)

                ' Get color
                curColor = bmInput.GetPixel(coordX, coordY)

                If curColor <> curTransparentColor And curColor.A = 255 Then
                    ' Color pixel

                    'in this iteration it makes sense to check the other three
                    If coordX < coordA.X Then coordA.X = coordX ' Topleft: move to left
                    If coordY < coordA.Y Then coordA.Y = coordY ' Topleft: move to top

                    'test is pointless, because coordX is always at least coordB.X+1
                    coordB.X = coordX ' Bottomright: move to right
                    If coordY > coordB.Y Then coordB.Y = coordY ' Bottomright: move to bottom

                End If

                ' If the current pixel is larger than a.Y and smaller than b.Y, we should skip.
                ' It's a bit late so I'm not thinking straight, this might be a pixel off. 
                If coordY >= coordA.Y And coordY < coordB.Y Then
                    coordY = coordB.Y
                Else
                    ' Default 
                    coordY += 1
                End If

            End While

            coordX += 1

        End While


200:
        ' MsgBox("w,h=" & coordA.X & "," & coordA.Y & " --- " & coordB.X & "," & coordB.Y)
        ' then crop away stuff from right
        ' but only the area we have not yet processed
        coordY = coordA.Y
        ' Top to bottom
        While coordY <= (coordB.Y)

            ' Right to left 
            coordX = bmInput.Width - 1
            While coordX > coordB.X

                ' Get color
                curColor = bmInput.GetPixel(coordX, coordY)

                If curColor <> curTransparentColor And curColor.A = 255 Then
                    ' Color pixel
                    If coordX > coordB.X Then coordB.X = coordX ' Bottomright: move to right

                End If
                'I don't think we need any test here, do we?
                coordX -= 1

            End While

            coordY += 1

        End While

901:
        'MsgBox("w,h=" & coordA.X & "," & coordA.Y & " --- " & coordB.X & "," & coordB.Y)
        ' enabled for cropping of frames, 20150619
        coordB.X += 1
        coordB.Y += 1


999:
        ' 20170512 
        ' HENDRIX found out that transparent frames can cause issues.
        ' This is a more simple fix, since it seems this is valid in ZT1 after all?
        If coordA.X = bmInput.Width And coordA.Y = bmInput.Height Then
            coordA = New Point(0, 0)
            coordB = New Point(1, 1)
        End If

        'Debug.Print("x1,y1=" & coordA.X & "," & coordA.Y & " --- x2,y2 " & coordB.X & "," & coordB.Y)
        Return New Rectangle(coordA.X, coordA.Y, coordB.X - coordA.X, coordB.Y - coordA.Y)


        Exit Function

dBug:
        MsgBox("Error while obtaining the 'defining rectangle' of this graphic." & vbCrLf & _
            "Erl " & Erl() & " - " & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Critical error")

    End Function

    Public Function images_Combine(ByVal imgBack As Image, ByVal imgFront As Image) As Image
        'this can now combine images of any size and will center them on each other

        Dim x_max As Integer = Math.Max(imgBack.Width, imgFront.Width)
        Dim y_max As Integer = Math.Max(imgBack.Height, imgFront.Height)

        Dim bmp As New Bitmap(x_max, y_max)
        Dim g As Graphics = Graphics.FromImage(bmp)

        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor ' prevent softening
        g.DrawImage(imgBack, CInt((x_max - imgBack.Width) / 2), CInt((y_max - imgBack.Height) / 2), imgBack.Width, imgBack.Height)
        g.DrawImage(imgFront, CInt((x_max - imgFront.Width) / 2), CInt((y_max - imgFront.Height) / 2), imgFront.Width, imgFront.Height)
        g.Dispose()
        Return bmp

    End Function


    ' === Extra ===
    Sub update_info(strReason As String)

        ' Displays updated info.

        With frmMain


            .tstZT1_AnimSpeed.Text = editorGraphic.animationSpeed
            .tslFrame_Index.Text = IIf(editorGraphic.frames.Count = 0, "-", (editorGraphic.frames.IndexOf(editorFrame) + cfg_convert_startIndex) & " / " & (editorGraphic.frames.Count - 1 + cfg_convert_startIndex - editorGraphic.extraFrame))

            Debug.Print("updated info. [" & strReason & "]. Total frames = " & (editorGraphic.frames.Count - cfg_convert_startIndex - editorGraphic.extraFrame))
            'Debug.Print(editorGraphic.extraFrame)


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
                If IsNothing(editorFrame.coreImageBitmap) = False Then
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



    Function grid_drawFootPrintXY(intFootPrintX As Integer, intFootPrintY As Integer, view As Byte) As Bitmap

        ' Draws a certain amount of squares.
        ' ZT1 uses either 1/4th of a square, or complete squares from there on. 
        ' Anything else doesn't seem to be too reliable!


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

        'every grid square adds this much for x and y - we must consider both directions to be efficient!
        Dim x_dim As Integer = intFootPrintX * 16 + intFootPrintY * 16
        Dim y_dim As Integer = intFootPrintY * 8 + intFootPrintX * 8
        Dim bmInput As New Bitmap(x_dim * 2, y_dim * 2)

        ' eerste punt tov onze gegenereerde grid: +16px Y (center)
        ' eerste punt tov onze gegenereerde grid: intFootprintX * 32

        ' we need to find the center of our generated grid.
        ' next, we need to align it with the center of our image.

        ' We know the starting coordinate. We know the number of squares. We know if it's even or not (= extra row) 
        ' We will keep track of how many squares we draw.
        ' We will not draw more squares than our max width.
        Dim intRow As Integer = 1


        Dim coord As New Point(0, 0)


        coord.X = (intFootPrintX / 2) * 32 ' possibly adjustment of 16px needed (o the left?
        Debug.Print(intHeight)

        coord.Y = 0 'cfg_grid_numPixels '- (intHeight / 2)

        ' Think with X=10,Y=8
        Dim intCurFootPrintX As Integer
        Dim intCurFootPrintY As Integer

        For intCurFootPrintX = 2 To intFootPrintX Step 2

            ' Starting point:
            coord.X = x_dim - (intWidth / 2) + (intFootPrintX / 2 * 32)

            coord.X = x_dim - (intWidth / 2)  ' Move to the left
            coord.X = coord.X + ((intFootPrintX - intCurFootPrintX) / 2) * 32  ' What can we add?

            'Debug.Print("X = " & coord.X)

            coord.Y = y_dim - (intHeight / 2) + 16 * (intCurFootPrintX / 2)
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

        ' Update coreImageHex for each frame. Color indexes have changed.
        For Each ztFrame As clsFrame2 In editorGraphic.frames
            ztFrame.coreImageHex = Nothing
            ztFrame.bitmap_to_hex() ' 20170519 - is it necessary to update this already? It could be generated when called.
        Next


    End Sub

    Sub pal_addColor(intIndexNow As Integer)

        ' Get color
        Dim c As System.Drawing.Color = Color.Transparent


        ' We had the color. Insert it at the position we want.
        editorGraphic.colorPalette.colors.Insert(intIndexNow + 1, c)

        ' Refresh
        editorGraphic.colorPalette.fillPaletteGrid(frmMain.dgvPaletteMain)



    End Sub



    Sub pal_open(strFileName As String)

        If File.Exists(strFileName) Then

            If Path.GetExtension(strFileName) <> ".pal" Then
                MsgBox("You did not select a ZT1 Color Palette (.pal file).", vbOKOnly + vbCritical, "Invalid color palette")

            Else

                Dim c As New clsPalette(Nothing)
                Dim frmColPal As New frmPal

                ' Read the .pal file
                If c.readPal(strFileName) <> 0 Then


                    c.fillPaletteGrid(frmColPal.dgvPal)

                    frmColPal.ssFileName.Text = Path.GetFileName(strFileName)

                    If c.colors.Count <> 9 And c.colors.Count <> 17 Then
                        ' This feature was originally meant to show a preview using the pal8 or pal16-files.
                        ' However, it might be extended for use with recolors - there, we don't know the number of colors in the palette.

                        'frmColPal.Controls.Remove(frmColPal.btnUseInMainPal)
                    End If

                    frmColPal.Show()


                End If

            End If


        End If



    End Sub





    Public Sub batch_rotationfix_folder_ZT1(strPath As String, pntOffset As Point, Optional PB As ProgressBar = Nothing)

        ' This will find all ZT1 Graphics in a folder and adjust the rotations within that folder. 
        ' It's especially useful when you import frames from a program like Blender and you notice the animal should be a bit more central (up/down).
        ' The progress can be shown in a progress bar.

        On Error GoTo dBug

0:

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

        ' Set the initial configuration for a (optional) progress bar.
        ' We want the max value to be the number of ZT1 Graphics we're trying to convert.
        If IsNothing(PB) = False Then
            PB.Minimum = 0
            PB.Value = 0
            PB.Maximum = result.Count
        End If

1000:
        ' For each file that is a ZT1 Graphic:
        For Each f As String In result
            Debug.Print("Processing: " & f)


            ' Read graphic, update offsets of frames, save.
            Dim g As New clsGraphic2

1100:
            g.read(f)

1105:
            g.frames(0).updateOffsets(pntOffset, True)

1110:
            g.write(f)

            If IsNothing(PB) = False Then
                PB.Value += 1
            End If
        Next


1200:
        ' Generate a .ani-file in each directory. 
        ' Add the initial directory
        batch_generate_Ani(strPath)



1950:
        MsgBox("Finished batch rotation fixing.", vbOKOnly + vbInformation, "Finished job")


        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and batch rotation fix ZT1 Graphic files in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during batch rotation fixing")

    End Sub


    Function batch_generate_Ani(strPath As String) As Integer

        Dim stack As New Stack(Of String)

        stack.Push(strPath)
        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string

            Dim dir As String = stack.Pop

            If cfg_export_ZT1_Ani = 1 Then
                Dim cAni As New clsAniFile(dir & "\" & Path.GetFileName(dir) & ".ani")
                Debug.Print(Now.ToString() & ": Generate .ani file (batch conversion)")
                cAni.createAniConfig()
            End If

            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop

        ' Make sure everything is finished.
        Application.DoEvents()

        Return 0

        Exit Function

dBug:

        MsgBox("An error occured while trying to list and batch rotation fix ZT1 Graphic files in this folder: " & vbCrLf & _
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error during batch rotation fixing")

        Return -1

    End Function


    Function ztStudio_StartUp() As Byte

        On Error GoTo dBug





10:

        ' Load the initial config. 
        ' settings.cfg contains the default values.
        ' Some parameters can be overwritten by the command line parameters; but they are not stored permanently.
        clsTasks.config_load()

20:

        ' We will configure our parameters.
        Dim strArgAction = vbNullString
        Dim strArgActionValue = vbNullString
        Dim argK As String
        Dim argV As String

        For Each arg As String In Environment.GetCommandLineArgs()

            Debug.Print(arg)


            ' Arguments are specified as:  ZTStudio.exe /arg1:<val1> /argN:<valN>
            ' We are expecting valid arguments.
            argK = Strings.Split(arg.ToLower & ":", ":")(0)
            argV = Strings.Replace(arg, argK & ":", "", , , CompareMethod.Text)

25:
            ' set arguments etc
            Select Case argK

                ' These are actual settings.
                ' If specified; they take priority over the values defined in settings.cfg

                ' Preview
                Case "/preview.bgcolor" : cfg_grid_BackGroundColor = System.Drawing.Color.FromArgb(CInt(argV))
                Case "/preview.fgcolor" : cfg_grid_ForeGroundColor = System.Drawing.Color.FromArgb(CInt(argV))
                Case "/preview.zoom" : cfg_grid_zoom = CInt(argV)
                Case "/preview.footprintX" : cfg_grid_footPrintX = CByte(argV)
                Case "/preview.footprinty" : cfg_grid_footPrintY = CByte(argV)

                    ' Paths
                Case "/paths.root" : cfg_path_Root = argV
                    ' ignore recent paths


                    ' Export options
                Case "/exportoptions.pngcrop" : cfg_export_PNG_CanvasSize = CByte(argV)
                Case "/exportoptions.pngrenderextraframe" : cfg_export_PNG_RenderBGFrame = (CByte(argV) = 1)
                Case "/exportoptions.pngrenderextragraphic" : cfg_export_PNG_RenderBGZT1 = (CByte(argV) = 1) ' this would require to supply the BG graphic. To implement.
                Case "/exportoptions.pngrendertransparentbg" : cfg_export_PNG_TransparentBG = (CByte(argV) = 1)

                Case "/exportoptions.zt1alwaysaddztafbytes" : cfg_export_ZT1_AlwaysAddZTAFBytes = (CByte(argV) = 1)
                Case "/exportoptions.zt1ani" : cfg_export_ZT1_Ani = (CByte(argV) = 1)

                    ' Conversion options
                Case "/conversionoptions.deleteoriginal" : cfg_convert_deleteOriginal = (CByte(argV) = 1)
                Case "/conversionoptions.filenamedelimeter" : cfg_convert_deleteOriginal = argV
                Case "/conversionoptions.overwrite" : cfg_convert_overwrite = (CByte(argV) = 1)
                Case "/conversionoptions.pngfilesindex" : cfg_convert_startIndex = CByte(argV)
                Case "/conversionoptions.sharedpalette" : cfg_convert_sharedPalette = (CByte(argV) = 1)

                    ' Editing options
                Case "/editing.individualrotationfix" : cfg_editor_rotFix_individualFrame = (CByte(argV) = 1)
                Case "/editing.animationspeed" : cfg_frame_defaultAnimSpeed = CInt(argV)

                    ' Not remembered but can be supplied:  
                Case "/extra.colorquantization" : cfg_palette_quantization = CByte(argV)



                    ' These are actions. 
                    ' An action can be an automated process doing lots of stuff (e.g. convertfolder)
                Case "/action.convertfolder.topng"
                    strArgAction = "convertfolder.topng"
                    strArgActionValue = argV
                Case "/action.convertfolder.tozt1"
                    strArgAction = "convertfolder.tozt1"
                    strArgActionValue = argV
                Case "/action.convertfile.topng"
                    strArgAction = "convertfile.topng"
                    strArgActionValue = argV
                Case "/action.convertfile.tozt1"
                    strArgAction = "convertfile"
                    strArgActionValue = argV




            End Select
            ' Parameters?


            ' Process action


        Next


30:
        ' See which action was specified. 
        ' We do this now because users might quickly change the order of arguments. 
        ' Eg if they say  ZTStudio.exe /convertFolder:<path> /ZTAF:1 instead of /ZTAF:1 /convertFolder:<path>, they might not get the right result.
        Select Case strArgAction

            Case "convertfile.topng"
                ' Do conversion.
                ' Then exit.
                clsTasks.convert_file_ZT1_to_PNG(strArgActionValue)

                End
            Case "convertfile.tozt1"
                ' Do conversion.
                ' Then exit.
                clsTasks.convert_file_PNG_to_ZT1(strArgActionValue)

                End

            Case "convertfolder.topng"

                ' Do conversion.
                ' Then exit.
                clsTasks.convert_folder_ZT1_to_PNG(strArgActionValue)

                End

            Case "convertfolder.tozt1"

                ' Do conversion.
                ' Then exit.
                clsTasks.convert_folder_PNG_to_ZT1(strArgActionValue)

                End


            Case Else
                ' Default.
                ' Just load.

        End Select



        Return 0



dBug:

        If MsgBox("It seems an invalid value for a command line argument was specified (" & argK & ")." & vbCrLf & _
            "Please read the proper documentation and specify values properly." & vbCrLf & _
            vbCrLf & _
            "Example:" & vbCrLf & _
            "ZTStudio.exe /convertFolder:path-to-folder /ZTAF:1" & vbCrLf & vbCrLf & _
            "Details: error in ztStudio_StartUp at line " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Invalid value for command line argument") = vbOK Then
            End
        End If


    End Function


    Function ztStudio_TestMainFeatures() As Byte

        ' To be implemented. 
        ' This function is meant to check if our (main) functions still give the results we expect. 
        ' We can do this most efficiently of we go for a batch conversion ( ZT1 <=> PNG or reversed ) and check hashes of the files.
        ' Other things to check, might include modifications to the color palette.

        ' The idea is to add this check once the Red Panda gets released at Zoo Tek Phoenix.



        Return 0

    End Function

End Module
