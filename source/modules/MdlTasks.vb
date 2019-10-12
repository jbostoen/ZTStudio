Option Explicit On


Imports System.IO
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

' This module contains several methods.

Module MdlTasks

    ''' <summary>
    ''' Cleans up files in a path, based on extension.
    ''' </summary>
    ''' <remarks>
    ''' Used to clean up .pal-files and files without a file extension (=ZT1 Graphic files)
    ''' </remarks>
    ''' <param name="StrPath"></param>
    ''' <param name="StrExtension"></param>
    ''' <returns></returns>
    Public Function CleanUpFiles(StrPath As String, StrExtension As String) As Integer

        On Error GoTo dBug

0:
5:
        ' Creating a recursive list.

        ' This list stores the results.
        Dim LstResult As New List(Of String)

        ' This stack stores the directories within our <root> folder to process.
        ' Then process each subdirectory.
        Dim Stack As New Stack(Of String)

        ' Add the initial directory
        Stack.Push(StrPath)

10:
        ' Continue processing for each stacked directory
        Do While (Stack.Count > 0)

15:
            ' Get top directory name
            Dim StrDirectoryName As String = Stack.Pop

20:
            ' Get all files and check if they match the extension (.pal, .png) or have no extension (ZT1 graphic)
            ' In this 'for' construction the wildcard '*' is used; which may also match other files WITH extension.
            For Each f As String In Directory.GetFiles(StrDirectoryName, "*")
                ' Does the extension match? (or for ZT1 Graphic files: this should match an empty string)
                If Path.GetExtension(f) = StrExtension Then
                    LstResult.Add(f)
                End If
            Next

25:
            ' Loop through all subdirectories and add them to the stack, so they're processed as well.
            Dim StrSubDirectoryName As String
            For Each StrSubDirectoryName In Directory.GetDirectories(StrDirectoryName)
                Stack.Push(StrSubDirectoryName)
            Next

        Loop

1000:
        ' For each file that matched the specified extension/pattern
        For Each StrFileName As String In LstResult
            MdlZTStudio.Trace("MdlTasks", "CleanUpFiles", "Delete file: " & StrFileName)
            System.IO.File.Delete(StrFileName)
        Next

        Exit Function

dBug:
        Dim StrMessage As String = "An error occured while trying to clean up ZT1 Graphic files in this folder: " & vbCrLf & StrPath
        MdlZTStudio.HandledError("MdlTasks", "CleanUpFiles", StrMessage, False, Information.Err)


    End Function

    ''' <summary>
    ''' Converts a ZT1 Graphic file to one or more PNG files.
    ''' </summary>
    ''' <param name="StrFileName"></param>
    Public Sub ConvertFileZT1ToPNG(StrFileName As String)

        BlnTaskRunning = True

        ' It will first render the ZT1 Graphic and then it will export it to one or multiple .PNG-files.
        ' Warning: do NOT implement a clean up of files here (ZT1 Graphic/ZT1 Color Palette).
        ' Reason: The color palette could be shared with other images, which would cause issues in a batch conversion!

        On Error GoTo dBg
        MdlZTStudio.Trace("MdlTasks", "ConvertFileZT1ToPNG", "Convert ZT1 to PNG: " & StrFileName)

5:
        ' Create a new instance of a ZT1 Graphic object.
        Dim ObjGraphic As New ClsGraphic(Nothing)

        ' Read the ZT1 Graphic
        ObjGraphic.Read(StrFileName)

        ' Render the set of frames within this ZT1 Graphic.
        ' There are some options when exporting.
        ' - canvas size options
        ' - render background frame or export it separately

10:
        ' Loop over each frame of the ZT1 Graphic
        For Each ObjFrame As ClsFrame In ObjGraphic.Frames

11:
            GoTo 12


            ' The bitmap's save function does not overwrite, nor warn that the file already exists.
            ' So it is safer to delete any existing files.
            System.IO.File.Delete(StrFileName & Cfg_Convert_FileNameDelimiter & (ObjGraphic.Frames.IndexOf(ObjFrame) + Cfg_Convert_StartIndex).ToString("0000") & ".png")

            ' Save frames as PNG, just autonumber the frames.
            ' Exception: if we have an extra frame which should be rendered separately rather than as background. 
            ' In that case, we will create a .PNG-file named <graphicname>_extra.png
            ' Since we are processing in batch, we (currently) do not offer the option to render a background ZT1 Graphic.
            ' This might however make a nice addition :)

            ' RenderBGFrame: this is read as: 'render this as BG for every frame'
            If Cfg_Export_PNG_RenderBGFrame = 0 And ObjGraphic.HasBackgroundFrame = 1 Then
                If ObjGraphic.Frames.IndexOf(ObjFrame) = (ObjGraphic.Frames.Count - 1) Then
                    ObjFrame.SavePNG(StrFileName & Cfg_Convert_FileNameDelimiter & "extra.png")
                Else
                    ObjFrame.SavePNG(StrFileName & Cfg_Convert_FileNameDelimiter & (ObjGraphic.Frames.IndexOf(ObjFrame) + Cfg_Convert_StartIndex).ToString("0000") & ".png")
                End If
            Else
                ObjFrame.SavePNG(StrFileName & Cfg_Convert_FileNameDelimiter & (ObjGraphic.Frames.IndexOf(ObjFrame) + Cfg_Convert_StartIndex).ToString("0000") & ".png")

            End If

12:
            ' Experimental. Export info such as offsets, height, width, mystery bytes...
            If Cfg_Convert_Write_Graphic_Data_To_Text_File = 1 Then
                ObjFrame.WriteDetailsToTextFile()
            End If

        Next

13:


        MdlZTStudio.Trace("MdlTasks", "ConvertFileZT1ToPNG", "Conversion finished.")

        BlnTaskRunning = False

        ' Paint job
        Application.DoEvents()


        Exit Sub

dBg:
        Dim StrMessage As String = "An error occurred while converting a ZT1 Graphics file to PNG files:" & vbCrLf & StrFileName
        MdlZTStudio.HandledError("MdlTasks", "ConvertFileZT1ToPNG", StrMessage, False, Information.Err)

        BlnTaskRunning = False

    End Sub

    ''' <summary>
    ''' Task to convert one or more PNG files to one ZT1 Graphic
    ''' </summary>
    ''' <param name="StrDestinationFileName"></param>
    ''' <param name="BlnSingleConversion"></param>
    Public Sub ConvertFilePNGToZT1(StrDestinationFileName As String, Optional BlnSingleConversion As Boolean = True)

        On Error GoTo dBg

        BlnTaskRunning = True

        ' Get the name(s) of the PNG file(s) that will be combined into the ZT1 Graphic.
        ' Find out what the final name of the ZT1 Graphic will be.
        ' Note: Cleanup of .PNG files only happens automatically in batch conversions (if enabled in Settings)

0:
        ' Convert to lower (force similar filenames everywhere)
        StrDestinationFileName = Strings.LCase(StrDestinationFileName)

        Dim StrPathDir As String = Path.GetDirectoryName(StrDestinationFileName) ' Gets the path where the graphic is stored
        Dim LstPNGFiles As String() ' Will be used to build a list of the filenames of all the frames (PNG set)
        Dim ObjGraphic As New ClsGraphic(Nothing)
        Dim ObjFrame As ClsFrame
        Dim StrGraphicName As String = System.IO.Path.GetFileName(StrDestinationFileName)
        Dim StrFrameGraphicPath As String = Strings.Left(StrDestinationFileName, StrDestinationFileName.Length - StrGraphicName.Length)

        Dim StrErrorMessage As String ' For error details
        Dim StrPngName As String

10:
        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Path: " & StrFrameGraphicPath)
        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Graphic name: " & StrGraphicName)

        ' Get the entire list of .PNG files matching the naming convention for this graphic.
        ' Any filename not matching this pattern is irrelevant to process.
        LstPNGFiles = System.IO.Directory.GetFiles(StrFrameGraphicPath, StrGraphicName & Cfg_Convert_FileNameDelimiter & "????.png")

11:
        ' Check if files match the expected pattern, so far
        Dim IntIndex As Integer = 0
        For Each StrPNGFile As String In LstPNGFiles
            If StrPNGFile.ToLower() <> (StrFrameGraphicPath & StrGraphicName & Cfg_Convert_FileNameDelimiter & (IntIndex - 1 + Cfg_Convert_StartIndex).ToString("0000") & ".png").ToLower() Then

                StrErrorMessage =
                    "The numbering in the PNG file(s) does not seem to be consecutive." & vbCrLf &
                    "Your settings specify that the first PNG file should be " & StrGraphicName & Cfg_Convert_FileNameDelimiter & Cfg_Convert_StartIndex.ToString("0000") & " .png" & vbCrLf &
                    "Avoid storing any other PNG files in the directory (except for " & StrGraphicName & Cfg_Convert_FileNameDelimiter & "_extra.png if required)."
                MdlZTStudio.HandledError("MdlTasks", "ConvertFilePNGToZT1", StrErrorMessage, False)
                Exit Sub
            End If
        Next

20:

        ' Now if there is a background frame (ends in extra.png), add this as well.
        If File.Exists(StrFrameGraphicPath & StrGraphicName & Cfg_Convert_FileNameDelimiter & "extra.png") = True Then
            LstPNGFiles.Append(StrFrameGraphicPath & StrGraphicName & Cfg_Convert_FileNameDelimiter & "extra.png")
            ObjGraphic.HasBackgroundFrame = 1
        End If


21:
        ' There should be at least two frames if a background frame is specified
        If ObjGraphic.HasBackgroundFrame = 1 Then

            If LstPNGFiles.Count = 1 Then
                MdlZTStudio.HandledError("MdlTasks", "ConvertFilePNGToZT1", "A ZT1 Graphic needs at least one frame, if a background frame (extra.png) is specified.", False, Nothing)
                Exit Sub
            End If

        End If

100:

        For Each StrPNGFile As String In LstPNGFiles

105:
            ' Extract the index of the frame (or _extra) from the filename
            If Strings.Right(System.IO.Path.GetFileName(StrPNGFile).ToLower(), 9) = "extra.png" Then
                StrPngName = "extra"
            Else
                StrPngName = Strings.Right(System.IO.Path.GetFileNameWithoutExtension(StrPNGFile), 4)
            End If

120:

            If StrPngName = "extra" Then
                ' There's an extra background frame.
                ObjGraphic.HasBackgroundFrame = 1

            End If

200:
            ObjFrame = New ClsFrame(ObjGraphic)

201:
            ' In case of a batch conversion, it's possible a shared color palette (.pal) is enforced.
            ' usually, this would be something like this:
            ' objects/restrant/restrant.pal
            ' animals/ibex/ibex.pal 

            ' To make it a bit more simple for the users of ZT Studio and to allow for easier recoloring 
            ' (for example: lighter graphics of Red Panda will be used for the female), 
            ' it would be better if the palette is not under animals/redpanda/redpanda.pal but animals/redpanda/m/redpanda.pal
            ' This should work for fences etc as well.

202:

            If Cfg_Convert_SharedPalette = 1 And BlnSingleConversion = False Then

                ' 20170513: changed behavior for even more flexibility. 
                ' ZT Studio tries to detect a color palette:
                ' - in the same folder as the graphic (animals/redpanda/m/walk - walk.pal) - in case this animation uses colors not used anywhere else.
                ' - in the folder one level up (animals/redpanda/m - m.pal) - in case a palette is shared for the gender (male, female, young)
                ' - in the folder two levels up (animals/redpanda - redpanda.pal) - in case a palette is shared for (most of) the animal
                ' This method should also work just fine for objects.

                Dim StrPath0 As String
                Dim StrPath1 As String
                Dim StrPath2 As String

                StrPath0 = Path.GetDirectoryName(StrPathDir)
                StrPath1 = Path.GetDirectoryName(StrPath0)
                StrPath2 = Path.GetDirectoryName(StrPath1)

                ' Basically the filename also reflects the name of the folder the graphic is in.
                ' Using .NETs Path.GetFileName() method, the last part of the directory derived above is retrieved and appended.
                ' Only thing missing for a full filename, is the extension (see below)
                StrPath0 = StrPath0 & "\" & Path.GetFileName(StrPath0)
                StrPath1 = StrPath1 & "\" & Path.GetFileName(StrPath1)
                StrPath2 = StrPath2 & "\" & Path.GetFileName(StrPath2)

                ' The current graphic should not be the only view (icon etc) in this processed folder.
                ' If it does seem to be the only view (for instance an icon/graphic 'N'), this method should NOT fall back on higher level.
                ' An icon is NOT animated and often contains very different colors (plaque, icon in menu) than the actual animations.
                ' An exception to this rule could be the list icon, but it's not worth making an exception for it in this code.
                ' One way to find out, is if there are any other PNG files in this folder and not just for this particular graphic.
                If LstPNGFiles.Count <> Directory.GetFiles(StrPathDir, "*.png").Count Then

                    ' 20170502 Optimized by Hendrix.
                    Dim StrColorPaletteFileNamesWithoutExt() As String = {StrPath0, StrPath1, StrPath2}
                    Dim StrExtensions() As String = {".pal", ".gpl", ".png"}

                    ' No palette has been saved/set yet for this graphic.
                    If ObjGraphic.FileName = vbNullString Then

                        ' Figure out if there is a preferred palette (perhaps already prepared by the user) to be used.
                        ' Two ideas come to mind here:
                        '
                        ' (1) Palette at deeper level folder gets priority over palette in higher level folder
                        '     For example: an animal might use one palette for nearly all animations, except one
                        '   
                        ' (2) Palette of certain type (file extension) gets priority over another one.
                        '     Order: .pal(ZT1 Graphic) > .gpl (GIMP Palette) > .png

                        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Batch conversion and shared palette = 1. Trying to find existing palette.")

                        Do
                            For Each StrColorPaletteFileNameWithoutExt As String In StrColorPaletteFileNamesWithoutExt
                                For Each StrExtension As String In StrExtensions

                                    If File.Exists(StrColorPaletteFileNameWithoutExt & StrExtension) = True Then
                                        With ObjGraphic.ColorPalette
                                            ' Read a new palette once
                                            ' Ignore different extensions, so reloading within the loop is skipped

                                            ' Set filename.
                                            .FileName = StrColorPaletteFileNameWithoutExt & ".pal"

                                            ' Now go by priority.
                                            ' Go-to is usually a bad practice, but it's good here to break out of our 2 (!) loops.
                                            Select Case StrExtension
                                                Case ".pal"
                                                    .ReadPal(.FileName)
                                                    Exit Do
                                                Case ".gpl"
                                                    .ImportFromGIMPPalette(StrColorPaletteFileNameWithoutExt & StrExtension)
                                                    .WritePal(.FileName, True)
                                                    Exit Do
                                                Case ".png"
                                                    .ImportFromPNG(StrColorPaletteFileNameWithoutExt & StrExtension)
                                                    .WritePal(.FileName, True)
                                                    Exit Do
                                            End Select

                                        End With
                                    End If
                                Next StrExtension
                            Next StrColorPaletteFileNameWithoutExt

                            ' Todo: does this lead to issues?
                            MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Warning: no shared palette found.")
                            MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Procedure will continue and use specific stand-alone palette.")

                        Loop While False

                    Else
                        ' Color palette has already been set for this graphic.
                        ' No further action needed.
                        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Skip. Specific color stand-alone palette defined.")
                    End If

                End If

            End If


245:
            ' Add this frame to the graphic's frame collection 
            ObjGraphic.Frames.Add(ObjFrame)

250:
            ' Create a frame from the .PNG-file
            ObjFrame.LoadPNG(StrPNGFile)

        Next StrPNGFile

1530:
        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Write graphic...")

        ' Create the ZT1 Graphic. 
        ObjGraphic.Write(StrDestinationFileName)

1555:
        If Cfg_Export_ZT1_Ani = 1 And BlnSingleConversion = True Then

            MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Generate .ani file")

            ' Only 1 graphic file is being generated (example: icon)
            ' A .ani-file can be generated automatically.       
            ' [folder path] + \ + [folder name] + .ani
            Dim ObjAniFile As New ClsAniFile(StrPathDir & "\" & Path.GetFileName(StrPathDir) & ".ani")
            ObjAniFile.CreateAniConfig()

        End If

        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Converted PNG-set to ZT1 Graphic")

9999:
        ' Clear everything.
        ObjGraphic = Nothing

        BlnTaskRunning = False

        Exit Sub

dBg:
        StrErrorMessage = "Unexpected error while converting a PNG set to ZT1 Graphic"
        MdlZTStudio.HandledError("MdlTasks", "ConvertFilePNGToZT1", StrErrorMessage, True, Information.Err)


    End Sub
    Public Sub ConvertFolderZT1ToPNG(strPath As String, Optional PB As ProgressBar = Nothing)

        ' This will find all ZT1 Graphics in a folder and generate PNGs from it. It works recursively.
        ' The progress can be shown in a progress bar.
        ' Batch conversion offers the feature to automatically clean up everything afterwards.

        On Error GoTo dBug

0:
        ' First we will create a recursive list.

        ' This list stores the results.
        Dim LstResult As New List(Of String)

        ' This stack stores the directories to process.
        Dim Stack As New Stack(Of String)

        ' Add the initial directory
        Stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (Stack.Count > 0)
            ' Get top directory string

15:
            Dim StrDirectoryName As String = Stack.Pop

20:
            For Each StrFileName As String In Directory.GetFiles(StrDirectoryName, "*")
                ' Only ZT1 files
                If Path.GetExtension(StrFileName) = vbNullString Then
                    LstResult.Add(StrFileName)
                End If
            Next

25:
            ' Loop through all subdirectories and add them to the stack.
            Dim StrSubDirectoryName As String
            For Each StrSubDirectoryName In Directory.GetDirectories(StrDirectoryName)
                Stack.Push(StrSubDirectoryName)
            Next

        Loop

        ' Set the initial configuration for a (optional) progress bar.
        ' We want the max value to be the number of ZT1 Graphics we're trying to convert.
        If IsNothing(PB) = False Then
            PB.Minimum = 0
            PB.Value = 0
            PB.Maximum = LstResult.Count
        End If

1000:
        ' For each file that is a ZT1 Graphic:
        For Each StrFileName As String In LstResult
            MdlTasks.ConvertFileZT1ToPNG(StrFileName)
            If IsNothing(PB) = False Then
                PB.Value += 1
            End If
        Next


1050:
        ' Clean up original ZT1 Graphic files? (includes palette, does not include .ani file for now!)
        If Cfg_Convert_DeleteOriginal = 1 Then
            ' Currently ZT1 Graphics and ZT1 Color palettes have their own sub in which the files get deleted.
            ' It might be possible to merge them at some point and you could even gain a small performance boost.
            MdlTasks.CleanUpFiles(strPath, "")
            MdlTasks.CleanUpFiles(strPath, ".pal")
        End If

        Exit Sub

dBug:

        MdlZTStudio.UnhandledError("MdlTasks", "ConvertFolderZT1ToPNG", Information.Err)


    End Sub
    Public Sub ConvertFolderPNGToZT1(strPath As String, Optional PB As ProgressBar = Nothing)

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
                    StrGraphicName = Strings.Left(f, Len(f) - 9 - Len(Cfg_Convert_FileNameDelimiter))
                    ' Debug.Print("strgraphicname extra='" & strGraphicName & "'")
                Else
                    ' 4 (0000) + 4 (.png) = 8 chars. 
                    ' eg objects/yourobj/NE_0001.png 
                    StrGraphicName = Strings.Left(f, Len(f) - 8 - Len(Cfg_Convert_FileNameDelimiter))
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
                If Path.GetFileName(directoryName).Length > 8 Or System.Text.RegularExpressions.Regex.IsMatch(Strings.Replace(Path.GetFileName(directoryName), "-", ""), "^[a-zA-Z0-9_-]+$") = False Then
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
            MdlTasks.ConvertFilePNGToZT1(f, False)
            If IsNothing(PB) = False Then
                PB.Value += 1
            End If

            Application.DoEvents()

        Next


1100:
        ' Generate a .ani-file in each directory. 
        ' Add the initial directory
        MdlBatch.WriteAniFile(strPath)


1150:
        ' Do a clean up of our .PNG files if we had a successful conversion.
        If Cfg_Convert_DeleteOriginal = 1 Then
            MdlTasks.CleanUpFiles(strPath, ".png")
        End If

        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and convert PNG files in this folder: " & vbCrLf &
            strPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description,
            vbOKOnly + vbCritical, "Error during PNG to ZT1 batch conversion")

    End Sub







    ' === Extra ===

    ''' <summary>
    ''' Saves the main graphic as a ZT1 Graphic file (simple, using UI)
    ''' Saves as the specified filename.
    ''' </summary>
    ''' <param name="StrFileName">Filename</param>
    Sub SaveGraphic(StrFileName As String)

        ' 20150624. Assume having <filename>.pal here. 
        ' This was done to avoid issues with shared color palettes, if users are NOT familiar with them.
        ' Pro users will only tweak and use the batch conversion.
        With EditorGraphic
            .FileName = StrFileName
            .ColorPalette.FileName = EditorGraphic.FileName & ".pal"
            .Write(StrFileName, True)
        End With

50:
        If Cfg_Export_ZT1_Ani = 1 Then
            Debug.Print("Try .ani")
            ' Get the folder + name of the folder + .ani
            Dim CAni As New ClsAniFile(Path.GetDirectoryName(StrFileName) & "\" & Path.GetFileName(Path.GetDirectoryName(StrFileName)) & ".ani")
            CAni.CreateAniConfig()
        End If

60:
        FrmMain.ssFileName.Text = Now.ToString("yyyy-MM-dd HH:mm:ss") & ": saved " & StrFileName


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
    Public Sub BatchOffsetFixFolderZT1(StrPath As String, PntOffset As Point, Optional ObjProgressBar As ProgressBar = Nothing)

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

            MdlZTStudio.Trace("MdlTasks", "BatchOffsetFixFolderZT1", "Processing file " & StrCurrentFile)


            ' Read graphic, update offsets of frames, save.
            Dim ObjGraphic As New ClsGraphic(Nothing)

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
        MdlBatch.WriteAniFile(StrPath)

1950:
        MsgBox("Finished batch rotation fixing.", vbOKOnly + vbInformation, "Finished job")

        Exit Sub

dBug:

        MsgBox("An error occured while trying to list and batch rotation fix ZT1 Graphic files in this folder: " & vbCrLf &
            StrPath & vbCrLf & vbCrLf & "Line: " & Erl() & vbCrLf & Err.Number & " - " & Err.Description,
            vbOKOnly + vbCritical, "Error during batch rotation fixing")

    End Sub



End Module
