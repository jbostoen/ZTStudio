Option Explicit On


Imports System.IO
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

' This module contains several methods.

Module MdlTasks


    Public Function CleanUp_files(strPath As String, strExtension As String) As Integer

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
        Dim Stack As New Stack(Of String)

        ' Add the initial directory
        Stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (Stack.Count > 0)
            ' Get top directory string

15:


            Dim dir As String = Stack.Pop

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
                Stack.Push(directoryName)
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

        MsgBox("An error occured while trying to clean up ZT1 Graphic files in this folder: " & vbCrLf &
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description,
            vbOKOnly + vbCritical, "Error during clean up")


    End Function


    Public Sub Convert_file_ZT1_to_PNG(strFile As String)

        ' This function will convert a ZT1 Graphic to a PNG file.
        ' It will first render the ZT1 Graphic and then it will export it to one or multiple .PNG-files.
        ' DO NOT implement a clean up of ZT1 Graphic/ZT1 Color Palette here.
        ' The color palette could be shared with other images, which would cause issues in a batch conversion!


        On Error GoTo dBg

5:
        ' Create a new instance of a ZT1 Graphic object.
        Dim g As New ClsGraphic

        ' Read the ZT1 Graphic
        g.Read(strFile)

        ' We will render this set of frames within this ZT1 Graphic.
        ' However, there are two main options:
        ' - keep canvas size / to relevant frame area / to relevant graphic area
        ' - render extra frame or not


10:
        ' Loop over each frame of the ZT1 Graphic
        For Each ztFrame As ClsFrame In g.Frames

11:

            ' the bitmap's save function does not overwrite, nor warn 
            System.IO.File.Delete(strFile & Cfg_convert_fileNameDelimiter & (g.Frames.IndexOf(ztFrame) + Cfg_convert_startIndex).ToString("0000") & ".png")

            ' Save frames as PNG, just autonumber the frames.
            ' Exception: if we have an extra frame which should be rendered separately rather than as background. 
            ' In that case, we will create a .PNG-file named <graphicname>_extra.png
            ' Since we are processing in batch, we (currently) do not offer the option to render a background ZT1 Graphic.
            ' This might however make a nice addition :)

            ' RenderBGFrame: this is read as: 'render this as BG for every frame'
            If Cfg_export_PNG_RenderBGFrame = 0 And g.ExtraFrame = 1 Then
                If g.Frames.IndexOf(ztFrame) <> (g.Frames.Count - 1) Then
                    ztFrame.SavePNG(strFile & Cfg_convert_fileNameDelimiter & (g.Frames.IndexOf(ztFrame) + Cfg_convert_startIndex).ToString("0000") & ".png")
                Else
                    ztFrame.SavePNG(strFile & Cfg_convert_fileNameDelimiter & "extra.png")
                End If
            Else
                ztFrame.SavePNG(strFile & Cfg_convert_fileNameDelimiter & (g.Frames.IndexOf(ztFrame) + Cfg_convert_startIndex).ToString("0000") & ".png")

            End If

12:



        Next

13:


        Debug.Print("Converted file from ZT1 to PNG.")

        Exit Sub

dBg:
        MsgBox("Error occured in convert_file_ZT1_to_PNG:" & vbCrLf & "Line: " & Erl() & vbCrLf &
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error in conversion ZT1 -> PNG")


    End Sub
    Public Function Convert_file_PNG_to_ZT1(strPath As String, Optional blnSingleConversion As Boolean = True) As Integer

        On Error GoTo dBg


        ' In this sub, we get the file name of a PNG image we'll convert.
        ' But the filename should NOT be that of the  .PNG, but rather the one for the final ZT1 graphic. 
        ' That's why it's named strPath for now. This functoin will find others in the same series.  
        ' Advise: Cleanup of .PNG files only happens automatically in batch conversions.

0:

        strPath = Strings.LCase(strPath)

        Dim StrPathDir As String = Path.GetDirectoryName(strPath)


        Dim paths As New List(Of String)
        Dim g As New ClsGraphic
        Dim ZtFrame As ClsFrame
        Dim graphicName As String = System.IO.Path.GetFileName(strPath)
        Dim frameGraphicPath As String = Strings.Left(strPath, strPath.Length - graphicName.Length)


        Dim pngName As String


10:
        Debug.Print("Convert PNG to ZT1: " & strPath & " (" & graphicName & ") " & Now.ToString("HH:mm:ss"))



        ' Get the entire list of .PNG files matching the naming convention for this graphic.
        ' Anything else is irrelevant to process.
        paths.AddRange(System.IO.Directory.GetFiles(frameGraphicPath, graphicName & Cfg_convert_fileNameDelimiter & "????.png"))

        ' Fix for graphic_extra.PNG (legacy)
        If File.Exists(frameGraphicPath & graphicName & Cfg_convert_fileNameDelimiter & "extra.png") = True Then
            paths.Add(frameGraphicPath & graphicName & Cfg_convert_fileNameDelimiter & "extra.png")
            g.ExtraFrame = 1
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

                ' This could occur if for some reason we have for instance a ne.png file instead of the expected ne[delimiter]0000.png file
                MsgBox("A .PNG-file has been detected which does not match the pattern expected." & vbCrLf &
                    "The files should be named something similar to: " & vbCrLf &
                    graphicName & Cfg_convert_fileNameDelimiter & "000" & Cfg_convert_startIndex & ".png (number increases)" & vbCrLf &
                    "or " & graphicName & Cfg_convert_fileNameDelimiter & "extra.png (for the extra frame in certain ZT1 Graphics." & vbCrLf & vbCrLf &
                    "File which caused this error: " & vbCrLf & "'" & s & "'" & vbCrLf &
                       "ZT Studio will close to prevent program or game crashes.",
                        vbOKOnly + vbCritical + vbApplicationModal,
                        "Invalid filename (pattern)")

                End


            Else

120:

                If pngName = "extra" Then
                    ' There's an extra background frame.
                    g.ExtraFrame = 1

125:
                ElseIf IsNumeric(pngName) = True Then

                    ' Here we check whether the pattern is still appropriate.
                    ' The specification is that - depending on a variable to see if we start counting from 0 or 1 - 
                    ' the user should have PNGs named <graphicName><delimiter>0000.png, <graphicName><delimiter>0001.png, ... 
                    ' This checks if no number is skipped or invalid.
                    If (CInt(pngName) - Cfg_convert_startIndex) <> g.Frames.Count Then

135:
                        ' Check if file name pattern is okay
                        MsgBox("The file name ('" & frameGraphicPath & graphicName & Cfg_convert_fileNameDelimiter & (CInt(pngName)).ToString("0000") & ".png') does not match the expected name " &
                               "('" & frameGraphicPath & graphicName & Cfg_convert_fileNameDelimiter & (g.Frames.Count + Cfg_convert_startIndex).ToString("0000") & ".png')" & vbCrLf & vbCrLf &
                               "Your current starting index is: " & Cfg_convert_startIndex & vbCrLf &
                               "Do not store other .png-files starting with '" & frameGraphicPath & graphicName & "' in that folder.", vbOKOnly + vbCritical, "Error")

                        Return -1

                    End If

140:

                ElseIf pngName = Path.GetFileName(StrPathDir) Then

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
                    MsgBox("The file name ('" & frameGraphicPath & graphicName & Cfg_convert_fileNameDelimiter & pngName & ".png') does not match the expected name " &
                           "('" & frameGraphicPath & graphicName & Cfg_convert_fileNameDelimiter & (g.Frames.Count + Cfg_convert_startIndex).ToString("0000") & ".png')" & vbCrLf & vbCrLf &
                           "Your current starting index is: " & Cfg_convert_startIndex & vbCrLf &
                           "Do not store other .png-files starting with '" & frameGraphicPath & graphicName & "' in that folder.", vbOKOnly + vbCritical, "Error")

                    Return -1

                End If

200:
                ZtFrame = New ClsFrame(g)


201:
                ' In case of a batch conversion, we might rely on a shared .PAL file 
                ' usually, this would be objects/restrant/restrant.pal
                ' animals/ibex/ibex.pal 

                ' to make it a bit more simple, and to allow for easier recoloring of baby (working on Red Panda now), 
                ' it would be better if the palette is not under animals/redpanda/redpanda.pal but animals/redpanda/m/redpanda.pal
                ' this should work for fences etc as well.

202:

                If Cfg_convert_sharedPalette = 1 And blnSingleConversion = False Then

                    ' 20170513: changed behavior for even more flexibility. 
                    ' We try to get a shared palette:
                    ' - in the same folder as the graphic (animals/redpanda/m/walk - walk.pal) - in case this animation uses colors not used anywhere else.
                    ' - in the folder one level up (animals/redpanda/m - m.pal) - in case a palette is shared for the gender (male, female, young)
                    ' - in the folder two levels up (animals/redpanda - redpanda.pal) - in case a palette is shared for (most of) the animal
                    ' This method should also work just fine for objects.

                    Dim SPath0 As String
                    Dim SPath1 As String
                    Dim SPath2 As String


                    SPath0 = Path.GetDirectoryName(StrPathDir)
                    SPath1 = Path.GetDirectoryName(SPath0)
                    SPath2 = Path.GetDirectoryName(SPath1)

                    SPath0 = SPath0 & "\" & Path.GetFileName(SPath0)
                    SPath1 = SPath1 & "\" & Path.GetFileName(SPath1)
                    SPath2 = SPath2 & "\" & Path.GetFileName(SPath2)

                    ' N should not be the only view (icon etc) in this folder.
                    ' If it does seem to be the only view, we should NOT fall back on higher level.
                    ' An icon is NOT animated and often contains very different colors (plaque, icon in menu). 
                    ' An exception to this rule could be the list icon, but it's not worth making an exception for it.

                    If Directory.GetFiles(StrPathDir, graphicName & "*.png").Length <>
                            Directory.GetFiles(StrPathDir, "*.png").Length Then


                        ' 20170502 Optimized by Hendrix.
                        Dim InPaths() As String = {SPath0, SPath1, SPath2}
                        Dim exts() As String = {".pal", ".gpl", ".png"}

                        ' No palette has been saved/set yet for this graphic.
                        If ZtFrame.Parent.ColorPalette.FileName = vbNullString Then

                            ' Figure out if there is a preferred palette (perhaps already prepared by the user) to be used.
                            ' Two ideas come to mind here:
                            ' (1) Palette at lower level folder gets priority over palette in higher level folder
                            '     For example: an animal might use one palette for nearly all animations, except one
                            '   
                            ' (2) Palette of certain type (file extension) gets priority over another one.
                            '     Order: .pal(ZT1 Graphic) > .gpl (GIMP Palette) > .png

                            MdlZTStudio.Trace("MdlTasks", "Convert_file_PNG_to_ZT1", "Batch conversion and shared palette = 1. Trying to find proper palette.")

                            For Each inPath As String In InPaths
                                For Each ext As String In exts

                                    If File.Exists(inPath & ext) = True Then
                                        With ZtFrame.Parent.ColorPalette
                                            ' Read a new palette once
                                            ' Ignore different extensions, so reloading within the loop is skipped

                                            ' Set filename.
                                            .FileName = inPath & ".pal"

                                            ' Now go by priority.
                                            ' Go-to is usually a bad practice, but it's good here to break out of our 2 (!) loops.
                                            Select Case ext
                                                Case ".pal"
                                                    .ReadPal(.FileName)
                                                    GoTo paletteReady
                                                Case ".gpl"
                                                    .Import_from_GimpPalette(inPath & ext)
                                                    .WritePal(.FileName, True)
                                                    GoTo paletteReady
                                                Case ".png"
                                                    .Import_from_PNG(inPath & ext)
                                                    .WritePal(.FileName, True)
                                                    GoTo paletteReady
                                            End Select

                                        End With
                                    End If
                                Next ext
                            Next inPath

                            ' Todo: does this lead to issues?
                            MdlZTStudio.Trace("MdlTasks", "Convert_file_PNG_to_ZT1", "Warning: no shared palette found for " & ZtFrame.Parent.FileName)

                        Else
                            ' Color palette has already been set for this graphic.
                            ' No further action needed.
                            MdlZTStudio.Trace("MdlTasks", "Convert_file_PNG_to_ZT1", "Skip. Specific color palette defined for " & ZtFrame.Parent.FileName)
                        End If

                    End If




                End If

paletteReady:

245:
                ' Add this frame to our parent graphic's frame collection 
                g.Frames.Add(ZtFrame)

250:
                ' Create a frame from the .PNG-file
                ZtFrame.LoadPNG(s)

            End If
        Next

        Debug.Print("Before write: " & Now.ToString("HH:mm:ss"))


1530:
        ' Create our ZT1 Graphic. 
        g.Write(strPath)


        'Debug.Print("After write: " & Now.ToString("HH:mm:ss"))

1555:
        If Cfg_export_ZT1_Ani = 1 And blnSingleConversion = True Then
            Debug.Print(Now.ToString() & ": convert_file_PNG_to_ZT1: Generate .ani file (single conversion)")

            ' Only 1 graphic file is being generated. This is the case for icons, for example.
            ' A .ani-file can be generated automatically.       
            ' [folder path] + \ + [folder name] + .ani
            Dim CAni As New ClsAniFile(StrPathDir & "\" & Path.GetFileName(StrPathDir) & ".ani")
            CAni.CreateAniConfig()

        End If



        Debug.Print("Graphic converted: " & strPath & " (" & graphicName & ") " & Now.ToString("HH:mm:ss"))

9999:
        ' Clear everything.
        g = Nothing




        Return 0


dBg:
        MsgBox("Error occured in convert_file_PNG_to_ZT1:" & vbCrLf & "Line: " & Erl() & vbCrLf &
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error in conversion PNG -> ZT1")


    End Function
    Public Sub Convert_folder_ZT1_to_PNG(strPath As String, Optional PB As ProgressBar = Nothing)

        ' This will find all ZT1 Graphics in a folder and generate PNGs from it. It works recursively.
        ' The progress can be shown in a progress bar.
        ' Batch conversion offers the feature to automatically clean up everything afterwards.

        On Error GoTo dBug

0:

        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim Stack As New Stack(Of String)

        ' Add the initial directory
        Stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (Stack.Count > 0)
            ' Get top directory string

15:


            Dim dir As String = Stack.Pop

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
                Stack.Push(directoryName)
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
            MdlTasks.Convert_file_ZT1_to_PNG(f)
            If IsNothing(PB) = False Then
                PB.Value += 1
            End If
        Next


1050:
        ' Should we do a clean up?
        If Cfg_convert_deleteOriginal = 1 Then
            ' Currently ZT1 Graphics and ZT1 Color palettes have their own sub in which the files get deleted.
            ' It might be possible to merge them at some point and you could even gain a small performance boost.
            MdlTasks.CleanUp_files(strPath, "")
            MdlTasks.CleanUp_files(strPath, ".pal")
        End If

        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and convert ZT1 Graphic files in this folder: " & vbCrLf &
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description,
            vbOKOnly + vbCritical, "Error during ZT1 to PNG batch conversion")

    End Sub
    Public Sub Convert_folder_PNG_to_ZT1(strPath As String, Optional PB As ProgressBar = Nothing)

        ' We have the path containing .PNG-files which need to be converted into a ZT1 Graphic.
        ' We should get the unique prefixes. (eg. e_0001.png => e is the prefix. So 'e' should be the name of the view.


        On Error GoTo dBug

0:
5:


        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim Stack As New Stack(Of String)

        ' Add the initial directory
        Stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (Stack.Count > 0)
            ' Get top directory string

15:


            Dim dir As String = Stack.Pop
            Dim StrGraphicName As String

            ' Add all immediate file paths 

20:
            For Each f As String In Directory.GetFiles(dir, "*.png")

                ' Add future graphic name ("full" path, eg animals/redpanda/m/walk/NE)
                If Strings.Right(Path.GetFileNameWithoutExtension(f).ToLower, 5) = "extra" Then
                    ' 5 (extra) + 4 (.png) = 9 chars.
                    ' eg objects/yourobj/NE_extra.png 
                    StrGraphicName = Strings.Left(f, Len(f) - 9 - Len(Cfg_convert_fileNameDelimiter))
                    ' Debug.Print("strgraphicname extra='" & strGraphicName & "'")
                Else
                    ' 4 (0000) + 4 (.png) = 8 chars. 
                    ' eg objects/yourobj/NE_0001.png 
                    StrGraphicName = Strings.Left(f, Len(f) - 8 - Len(Cfg_convert_fileNameDelimiter))
                    ' Debug.Print("strgraphicname='" & strGraphicName & "'")
                End If

                If result.Contains(StrGraphicName) = False Then
                    result.Add(StrGraphicName)
                End If

            Next

25:
            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)

                ' Just a warning, so users don't accidentally have "sitscratch" as animation name.
                ' Actually '-' is supported as well.
                If Path.GetFileName(directoryName).Length > 8 Or System.Text.RegularExpressions.Regex.IsMatch(Strings.Replace(Path.GetFileName(directoryName), "-", ""), "^[a-zA-Z0-9]+$") = False Then
                    MsgBox("Directory name '" & Path.GetFileName(directoryName) & "' is invalid." & vbCrLf &
                        "The limit of a folder name is a maximum of 8 alphanumeric characters." & vbCrLf &
                        "You will need to rename the folder manually and then retry." & vbCrLf &
                       "ZT Studio will close to prevent program or game crashes.",
                        vbOKOnly + vbCritical + vbApplicationModal,
                        "Invalid directory name")

                    ' better:
                    End

                End If

                Stack.Push(directoryName)
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
            MdlTasks.Convert_file_PNG_to_ZT1(f, False)
            If IsNothing(PB) = False Then
                PB.Value += 1
            End If

            Application.DoEvents()

        Next


1100:
        ' Generate a .ani-file in each directory. 
        ' Add the initial directory
        Batch_Generate_Ani(strPath)


1150:
        ' Do a clean up of our .PNG files if we had a successful conversion.
        If Cfg_convert_deleteOriginal = 1 Then
            MdlTasks.CleanUp_files(strPath, ".png")
        End If

        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and convert PNG files in this folder: " & vbCrLf &
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description,
            vbOKOnly + vbCritical, "Error during PNG to ZT1 batch conversion")

    End Sub







    ' === Extra ===

    ''' <summary>
    ''' Saves the main graphic as a ZT1 Graphic file.
    ''' Saves as the specified filename.
    ''' </summary>
    ''' <param name="sFileName">Filename</param>
    Sub Save_Graphic(sFileName As String)

        ' 20150624. We have <filename>.pal here. 
        ' We do this to avoid issues with shared color palettes, if users are NOT familiar with them.
        ' We are assuming pro users will only tweak and use the batch conversion.
        With EditorGraphic
            .FileName = sFileName
            .ColorPalette.FileName = EditorGraphic.FileName & ".pal"
            .Write(sFileName, True)
        End With

50:
        If Cfg_export_ZT1_Ani = 1 Then
            Debug.Print("Try .ani")
            ' Get the folder + name of the folder + .ani
            Dim CAni As New ClsAniFile(Path.GetDirectoryName(sFileName) & "\" & Path.GetFileName(Path.GetDirectoryName(sFileName)) & ".ani")
            CAni.CreateAniConfig()
        End If

60:
        FrmMain.ssFileName.Text = Now.ToString("yyyy-MM-dd HH:mm:ss") & ": saved " & sFileName


    End Sub



    ''' <summary>
    ''' Replaces color (specified by index) in the main color palette
    ''' </summary>
    ''' <param name="intIndex">Index of color to be replaced</param>
    Sub Pal_ReplaceColor(intIndex As Integer)

        With FrmMain.DlgColor
            .Color = FrmMain.dgvPaletteMain.Rows(intIndex).DefaultCellStyle.BackColor

            .AllowFullOpen = True
            .FullOpen = True
            .SolidColorOnly = True
            .ShowDialog()

        End With

        EditorGraphic.ColorPalette.Colors(intIndex) = FrmMain.DlgColor.Color

        'frmMain.dgvPaletteMain.Rows(intIndex).DefaultCellStyle.BackColor = frmMain.dlgColor.Color
        'frmMain.dgvPaletteMain.Rows(intIndex).DefaultCellStyle.SelectionBackColor = frmMain.dlgColor.Color  ' prevent selection highlighting (blue)

        'On Error Resume Nex

        EditorGraphic.ColorPalette.FillPaletteGrid(FrmMain.dgvPaletteMain)

    End Sub

    ''' <summary>
    ''' Moves color in the palette to a new position.
    ''' This has repercussions: order of colors changes, hex values need to be updated!
    ''' </summary>
    ''' <param name="intIndexNow">Current index</param>
    ''' <param name="intIndexDest">Wanted index</param>
    Sub Pal_MoveColor(intIndexNow As Integer, intIndexDest As Integer)

        ' Get color
        Dim CColorToMove As System.Drawing.Color = EditorGraphic.ColorPalette.Colors(intIndexNow)

        ' Delete the original.
        EditorGraphic.ColorPalette.Colors.RemoveAt(intIndexNow)

        ' We had the color. Insert it at the position we want.
        EditorGraphic.ColorPalette.Colors.Insert(intIndexDest, CColorToMove)

        ' Refresh
        EditorGraphic.ColorPalette.FillPaletteGrid(FrmMain.dgvPaletteMain)

        ' Update coreImageHex for each frame. Color indexes have changed.
        For Each ztFrame As ClsFrame In EditorGraphic.Frames
            ztFrame.CoreImageHex = Nothing
            ztFrame.BitMapToHex() ' 20170519 - is it necessary to update this already? It could be generated when called.
        Next

    End Sub

    ''' <summary>
    ''' Adds a new color entry at the specified index. 
    ''' The color hasn't been picked yet, so by default it's transparent.
    ''' </summary>
    ''' <param name="intIndexNow">Index</param>
    Sub Pal_AddColor(intIndexNow As Integer)

        If EditorGraphic.ColorPalette.Colors.Count = 256 Then
            MsgBox("You can't add any more colors to this palette." & vbCrLf & "The maximum of 255 (+1 transparent) colors has been reached.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Maximum amount of colors reached")
        End If


        ' Get color
        Dim CColor As System.Drawing.Color = Cfg_grid_BackGroundColor


        With FrmMain.DlgColor
            .Color = CColor

            .AllowFullOpen = True
            .FullOpen = True
            .SolidColorOnly = True
            .ShowDialog()

        End With

        CColor = FrmMain.DlgColor.Color

        ' Insert it at the position we want.
        EditorGraphic.ColorPalette.Colors.Insert(intIndexNow + 1, CColor)

        ' Refresh
        EditorGraphic.ColorPalette.FillPaletteGrid(FrmMain.dgvPaletteMain)


    End Sub


    ''' <summary>
    ''' Opens a color palette, displays it as a separate window.
    ''' </summary>
    ''' <param name="strFileName">Filename of the ZT1 color palette (.pal)</param>
    Sub Pal_Open(strFileName As String)

        If File.Exists(strFileName) Then

            If Path.GetExtension(strFileName) <> ".pal" Then
                MsgBox("You did not select a ZT1 Color Palette (.pal file).", vbOKOnly + vbCritical, "Invalid color palette")

            Else

                Dim CpPallete As New ClsPalette(Nothing)
                Dim frmColPal As New frmPal

                ' Read the .pal file
                If CpPallete.ReadPal(strFileName) <> 0 Then

                    CpPallete.FillPaletteGrid(frmColPal.dgvPal)

                    frmColPal.ssFileName.Text = Path.GetFileName(strFileName)

                    ' This feature was originally meant to show a preview using the pal8 or pal16-files.
                    ' That's why initially when loading another color palette (separate window), the button to replace the main palette with this palette, 
                    ' was originally exclusive to color palettes which contained 8 or 16 colors (+1 transparent)
                    ' However, it might be extended so it can be used with recolors. In which case the number of colors is unknown.
                    ' If cpPallete.Colors.Count <> 9 And cpPallete.Colors.Count <> 17 Then
                    '   frmColPal.Controls.Remove(frmColPal.btnUseInMainPal)
                    ' End If

                    frmColPal.Show()

                End If

            End If

        End If

    End Sub

    ''' <summary>
    ''' Batch rotation fixes all animations in a selected folder.
    ''' This sub will find all ZT1 Graphics in the folder and adjust the offsets of each frame.
    '''  
    ''' It's especially useful when importing frames from another program, such as Blender, and the user sees the animal should just be a bit more central (up/down).
    ''' </summary>
    ''' <param name="StrPath">Path to folder</param>
    ''' <param name="PntOffset">The offsets to apply</param>
    ''' <param name="ObjProgressBar">The bar which will indicate progress</param>
    Public Sub Batch_RotationFix_Folder_ZT1(StrPath As String, PntOffset As Point, Optional ObjProgressBar As ProgressBar = Nothing)

        ' Todo: check needed to see if strPath is subfolder of Cfg_Path_Root ?


        On Error GoTo dBug

0:

        ' Creating a recursive file list.

        ' This list stores the results.
        Dim LstFiles As New List(Of String)

        ' This stack stores the directories to process.
        Dim StackDirectories As New Stack(Of String)

        ' Add the initial directory
        StackDirectories.Push(StrPath)

10:

        ' Continue processing for each stacked directory
        Do While (StackDirectories.Count > 0)
            ' Get top directory string

15:
            Dim StrDirectory As String = StackDirectories.Pop

20:
            For Each strFile As String In Directory.GetFiles(StrDirectory, "*")
                ' Only ZT1 files
                If Path.GetExtension(strFile) = vbNullString Then
                    LstFiles.Add(strFile)
                End If
            Next

25:
            ' Loop through all subdirectories and add them to the stack.
            Dim StrSubDirectoryName As String
            For Each StrSubDirectoryName In Directory.GetDirectories(StrDirectory)
                StackDirectories.Push(StrSubDirectoryName)
            Next

        Loop

        ' Set the initial configuration for a (optional) progress bar.
        ' We want the max value to be the number of ZT1 Graphics we're trying to convert.
        If IsNothing(ObjProgressBar) = False Then
            ObjProgressBar.Minimum = 0
            ObjProgressBar.Value = 0
            ObjProgressBar.Maximum = LstFiles.Count
        End If

1000:
        ' For each file that is a ZT1 Graphic:
        For Each StrCurrentFile As String In LstFiles

            MdlZTStudio.Trace("MdlTasks", "Batch_RotationFix_Folder_ZT1", "Processing file " & StrCurrentFile)


            ' Read graphic, update offsets of frames, save.
            Dim ObjGraphic As New ClsGraphic

1100:
            ObjGraphic.Read(StrCurrentFile)

1105:
            ObjGraphic.Frames(0).UpdateOffsets(PntOffset, True)

1110:
            ObjGraphic.Write(StrCurrentFile)

            If IsNothing(ObjProgressBar) = False Then
                ObjProgressBar.Value += 1
            End If
        Next

1200:
        ' Generate a .ani-file in each directory. 
        ' Add the initial directory
        Batch_Generate_Ani(StrPath)

1950:
        MsgBox("Finished batch rotation fixing.", vbOKOnly + vbInformation, "Finished job")

        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and batch rotation fix ZT1 Graphic files in this folder: " & vbCrLf &
            StrPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description,
            vbOKOnly + vbCritical, "Error during batch rotation fixing")

    End Sub

    ''' <summary>
    ''' Attempts to create .ani file for each animation. Experimental.
    ''' </summary>
    ''' <param name="strPath">Path to folder</param>
    Sub Batch_Generate_Ani(strPath As String)

        Dim StackDirectories As New Stack(Of String)

        StackDirectories.Push(strPath)

        ' Continue processing for each stacked directory
        Do While (StackDirectories.Count > 0)
            ' Get top directory string

            Dim StrDirectoryName As String = StackDirectories.Pop

            If Cfg_export_ZT1_Ani = 1 Then
                Dim CAni As New ClsAniFile(StrDirectoryName & "\" & Path.GetFileName(StrDirectoryName) & ".ani")
                Debug.Print(Now.ToString() & ": Generate .ani file (batch conversion)")
                cAni.CreateAniConfig()
            End If

            ' Loop through all subdirectories and add them to the stack.
            Dim StrSubDirectoryName As String
            For Each strSubDirectoryName In Directory.GetDirectories(strDirectoryName)
                stackDirectories.Push(strSubDirectoryName)
            Next

        Loop

        ' Make sure everything is finished.
        Application.DoEvents()

        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and batch rotation fix ZT1 Graphic files in this folder: " & vbCrLf &
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description,
            vbOKOnly + vbCritical, "Error during batch rotation fixing")


    End Sub


End Module
