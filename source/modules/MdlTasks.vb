Option Explicit On


Imports System.IO
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.Tasks

' This module contains several methods.

Module MdlTasks

    ''' <summary>
    ''' Resets a ProgressBar's range and value. Marshals to the UI thread if called from a background thread (e.g. a batch task).
    ''' </summary>
    ''' <param name="ObjProgressBar">Progress bar to reset. No action taken if Nothing.</param>
    ''' <param name="IntMaximum">Maximum value to configure on the progress bar.</param>
    Private Sub ResetProgressBar(ObjProgressBar As ProgressBar, IntMaximum As Integer)

        If IsNothing(ObjProgressBar) = True Then
            Exit Sub
        End If

        Try
            If ObjProgressBar.IsHandleCreated = True AndAlso ObjProgressBar.InvokeRequired = True Then
                ObjProgressBar.Invoke(Sub()
                                           ObjProgressBar.Minimum = 0
                                           ObjProgressBar.Value = 0
                                           ObjProgressBar.Maximum = IntMaximum
                                       End Sub)
            Else
                ObjProgressBar.Minimum = 0
                ObjProgressBar.Value = 0
                ObjProgressBar.Maximum = IntMaximum
            End If
        Catch ex As ObjectDisposedException
            ' The owning form (and its ProgressBar) may already have been disposed if the user closed
            ' the form while this batch was still running in the background. Nothing left to update.
        End Try

    End Sub

    ''' <summary>
    ''' Advances a ProgressBar by one step. Marshals to the UI thread if called from a background thread (e.g. a batch task).
    ''' </summary>
    ''' <param name="ObjProgressBar">Progress bar to update. No action taken if Nothing.</param>
    Private Sub StepProgressBar(ObjProgressBar As ProgressBar)

        If IsNothing(ObjProgressBar) = True Then
            Exit Sub
        End If

        Try
            If ObjProgressBar.IsHandleCreated = True AndAlso ObjProgressBar.InvokeRequired = True Then
                ObjProgressBar.Invoke(Sub() ObjProgressBar.Value += 1)
            Else
                ObjProgressBar.Value += 1
            End If
        Catch ex As ObjectDisposedException
            ' The owning form (and its ProgressBar) may already have been disposed if the user closed
            ' the form while this batch was still running in the background. Nothing left to update.
        End Try

    End Sub

    ''' <summary>
    ''' Runs ObjAction once per item in LstItems, in order, with per-item error isolation (a failure
    ''' is logged and counted, but does not stop the batch), cooperative cancellation between items,
    ''' and progress-bar stepping. Used by the three folder-batch methods below to avoid duplicating
    ''' the same loop/cancellation/error-isolation/progress-bar skeleton three times (see issue #52).
    ''' </summary>
    ''' <typeparam name="T">Item type, e.g. a file path.</typeparam>
    ''' <param name="LstItems">Items to process, e.g. file paths.</param>
    ''' <param name="ObjProgressBar">Progress bar to reset up-front and step once per item. May be Nothing.</param>
    ''' <param name="ObjCancellationToken">Checked between items; on cancellation, stops early.</param>
    ''' <param name="ObjAction">The per-item work to run. May throw; failures are caught, logged, and counted.</param>
    ''' <param name="StrLogContext">Class/method name to attribute logged per-item failures to.</param>
    ''' <param name="ObjItemLabel">Turns an item into a short label (e.g. its filename) for logging/the failure list.</param>
    ''' <returns>Succeeded count, the labels of items that failed, and whether cancellation stopped the batch early.</returns>
    ''' <remarks>
    ''' Does NOT set/reset BlnBatchOperationRunning - that needs to be active for the whole duration of
    ''' the call (so HandledError/UnhandledError raise instead of showing a blocking dialog for anything
    ''' ObjAction does), so each caller wraps its own call to this helper in a Try/Finally around that flag.
    ''' </remarks>
    Private Function RunFileBatch(Of T)(
        LstItems As List(Of T),
        ObjProgressBar As ProgressBar,
        ObjCancellationToken As CancellationToken,
        ObjAction As Action(Of T),
        StrLogContext As String,
        ObjItemLabel As Func(Of T, String)
    ) As (Succeeded As Integer, Failed As List(Of String), Cancelled As Boolean)

        ResetProgressBar(ObjProgressBar, LstItems.Count)

        Dim IntSucceeded As Integer = 0
        Dim LstFailed As New List(Of String)
        Dim BlnCancelled As Boolean = False

        For Each ObjItem As T In LstItems

            ' Allow the batch to be cancelled cleanly between items.
            If ObjCancellationToken.IsCancellationRequested = True Then
                BlnCancelled = True
                Exit For
            End If

            Try
                ObjAction(ObjItem)
                IntSucceeded += 1

            Catch exItem As Exception
                MdlZTStudio.LogError(StrLogContext, "RunFileBatch", "Failed to process: " & ObjItemLabel(ObjItem), exItem)
                LstFailed.Add(ObjItemLabel(ObjItem))
            End Try

            StepProgressBar(ObjProgressBar)
        Next

        Return (IntSucceeded, LstFailed, BlnCancelled)

    End Function

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

        Try

        ' Creating a recursive list.

        ' This list stores the results.
        Dim LstResult As New List(Of String)

        ' This stack stores the directories within the <root> folder to process.
        ' Then process each subdirectory.
        Dim Stack As New Stack(Of String)

        ' Add the initial directory
        Stack.Push(StrPath)

        ' Continue processing for each stacked directory
        Do While (Stack.Count > 0)

            ' Get top directory name
            Dim StrDirectoryName As String = Stack.Pop

            ' Get all files and check if they match the extension (.pal, .png) or have no extension (ZT1 graphic)
            ' In this 'for' construction the wildcard '*' is used; which may also match other files WITH extension.
            For Each f As String In Directory.GetFiles(StrDirectoryName, "*")
                ' Does the extension match? (or for ZT1 Graphic files: this should match an empty string)
                If Path.GetExtension(f) = StrExtension Then
                    LstResult.Add(f)
                End If
            Next

            ' Loop through all subdirectories and add them to the stack, so they're processed as well.
            Dim StrSubDirectoryName As String
            For Each StrSubDirectoryName In Directory.GetDirectories(StrDirectoryName)
                Stack.Push(StrSubDirectoryName)
            Next

        Loop

        ' For each file that matched the specified extension/pattern
        For Each StrFileName As String In LstResult
            MdlZTStudio.Trace("MdlTasks", "CleanUpFiles", "Delete file: " & StrFileName)
            System.IO.File.Delete(StrFileName)
        Next

        ' UpdateExplorerPane touches the TVExplorer control, so this needs to be marshaled to the UI thread
        ' when this method is called from a background thread (e.g. from a folder batch task).
        If FrmMain.IsHandleCreated = True AndAlso FrmMain.InvokeRequired = True Then
            FrmMain.Invoke(Sub() MdlZTStudioUI.UpdateExplorerPane())
        Else
            MdlZTStudioUI.UpdateExplorerPane()
        End If

        Return LstResult.Count

        Catch ex As Exception
            ' Previously this Function had no Return on any path (neither here nor on the success
            ' path above), silently always yielding 0. Now returns the number of files actually
            ' deleted on success, and -1 here so a caller can tell "cleaned up 0 files" apart from
            ' "clean-up failed". HandledError below re-throws while a batch is running (see
            ' BlnBatchOperationRunning), so this Return is only reached outside a batch.
            Dim StrMessage As String = "An error occured while trying to clean up ZT1 Graphic files in this folder: " & vbCrLf & StrPath
            MdlZTStudio.HandledError("MdlTasks", "CleanUpFiles", StrMessage, False, ex)
            Return -1
        End Try

    End Function

    ''' <summary>
    ''' Task to convert a ZT1 Graphic file to one or more PNG files.
    ''' </summary>
    ''' <param name="StrSourceFileName">Filename of ZT1 Graphic</param>
    Public Sub ConvertFileZT1ToPNG(StrSourceFileName As String)

        BlnTaskRunning = True

        ' It will first render the ZT1 Graphic and then it will export it to a set of PNG files.
        ' Warning: do NOT implement a clean up of files here (ZT1 Graphic/ZT1 Color Palette).
        ' Reason: The color palette could be shared with other images, which would cause issues during a batch conversion!

        Try

            MdlZTStudio.Trace("MdlTasks", "ConvertFileZT1ToPNG", "Convert ZT1 to PNG: " & StrSourceFileName)

            ' Create a new instance of a ZT1 Graphic object.
            Dim ObjGraphic As New ClsGraphic(Nothing)

            ' Read the ZT1 Graphic
            ObjGraphic.Read(StrSourceFileName)

            ' Render the set of frames within this ZT1 Graphic.
            ' There are some options when exporting.
            ' - canvas size options
            ' - render background frame or export it separately

            ' Loop over each frame of the ZT1 Graphic. Using the loop index directly instead of
            ' repeatedly calling ObjGraphic.Frames.IndexOf(ObjFrame) (up to 4 times per frame below,
            ' each an O(n) scan) - the index is already known from iterating the list.
            For IntFrameIndex As Integer = 0 To ObjGraphic.Frames.Count - 1

                Dim ObjFrame As ClsFrame = ObjGraphic.Frames(IntFrameIndex)

                ' The bitmap's save function does not overwrite, nor warn that the file already exists.
                ' So it is safer to delete any existing files.
                System.IO.File.Delete(StrSourceFileName & Cfg_Convert_FileNameDelimiter & (IntFrameIndex + Cfg_Convert_StartIndex).ToString("0000") & ".png")

                ' Save frames as PNG, just autonumber the frames.
                ' Exception: if there is an extra frame which should be rendered separately rather than as background.
                ' In that case, output a .PNG-file named <graphicname>_extra.png
                ' Since this is a batch process, (currently) not offering the option to render a background ZT1 Graphic.
                ' This might however make a nice addition :)

                ' RenderBGFrame: this is read as: 'render this as BG for every frame'
                If Cfg_Export_PNG_RenderBGFrame = 0 And ObjGraphic.HasBackgroundFrame = 1 Then
                    If IntFrameIndex = (ObjGraphic.Frames.Count - 1) Then
                        ObjFrame.SavePNG(StrSourceFileName & Cfg_Convert_FileNameDelimiter & "extra.png")
                    Else
                        ObjFrame.SavePNG(StrSourceFileName & Cfg_Convert_FileNameDelimiter & (IntFrameIndex + Cfg_Convert_StartIndex).ToString("0000") & ".png")
                    End If
                Else
                    ObjFrame.SavePNG(StrSourceFileName & Cfg_Convert_FileNameDelimiter & (IntFrameIndex + Cfg_Convert_StartIndex).ToString("0000") & ".png")

                End If

                ' Experimental. Export info such as offsets, height, width, mystery bytes...
                If Cfg_Convert_Write_Graphic_Data_To_Text_File = 1 Then
                    MdlZTStudio.Trace("MdlTasks", "ConvertFileZT1ToPNG", "Export graphic details to text file...")
                    ObjFrame.WriteDetailsToTextFile()
                End If

            Next

            MdlZTStudio.Trace("MdlTasks", "ConvertFileZT1ToPNG", "Conversion finished.")

            ' Paint job
            Application.DoEvents()

        Catch ex As Exception
            Dim StrErrorMessage As String =
                "An error occurred while converting a ZT1 Graphics file to PNG files:" & vbCrLf &
                StrSourceFileName

            MdlZTStudio.HandledError("MdlTasks", "ConvertFileZT1ToPNG", StrErrorMessage, False, ex)

        Finally
            BlnTaskRunning = False
        End Try

    End Sub

    ''' <summary>
    ''' Task to convert one or more PNG files to one ZT1 Graphic
    ''' </summary>
    ''' <param name="StrDestinationFileName"></param>
    ''' <param name="BlnSingleConversion"></param>
    Public Sub ConvertFilePNGToZT1(StrDestinationFileName As String, Optional BlnSingleConversion As Boolean = True)

        BlnTaskRunning = True

        ' Get the name(s) of the PNG file(s) that will be combined into the ZT1 Graphic.
        ' Find out what the final name of the ZT1 Graphic will be.
        ' Note: Cleanup of .PNG files only happens automatically in batch conversions (if enabled in Settings)

        Try

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

        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Path: " & StrFrameGraphicPath)
        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Graphic name: " & StrGraphicName)

        ' Get the entire list of .PNG files matching the naming convention for this graphic.
        ' Any filename not matching this pattern is irrelevant to process.
        LstPNGFiles = System.IO.Directory.GetFiles(StrFrameGraphicPath, StrGraphicName & Cfg_Convert_FileNameDelimiter & "????.png")

        ' Check if files match the expected pattern, so far
        Dim IntIndex As Integer = 0
        For Each StrPNGFile As String In LstPNGFiles
            If StrPNGFile.ToLower() <> (StrFrameGraphicPath & StrGraphicName & Cfg_Convert_FileNameDelimiter & (IntIndex + Cfg_Convert_StartIndex).ToString("0000") & ".png").ToLower() Then

                StrErrorMessage =
                    "The numbering in the PNG file(s) does not seem to be consecutive." & vbCrLf &
                    "Your settings specify that the first PNG file should be " & StrGraphicName & Cfg_Convert_FileNameDelimiter & Cfg_Convert_StartIndex.ToString("0000") & " .png" & vbCrLf &
                    "Avoid storing any other PNG files in the directory (except for " & StrGraphicName & Cfg_Convert_FileNameDelimiter & "extra.png if required)."
                MdlZTStudio.HandledError("MdlTasks", "ConvertFilePNGToZT1", StrErrorMessage, False)
                Exit Sub
            End If
            IntIndex += 1
        Next

        ' Now if there is a background frame (ends in extra.png), add this as well.
        If File.Exists(StrFrameGraphicPath & StrGraphicName & Cfg_Convert_FileNameDelimiter & "extra.png") = True Then
            LstPNGFiles.Append(StrFrameGraphicPath & StrGraphicName & Cfg_Convert_FileNameDelimiter & "extra.png")
            ObjGraphic.HasBackgroundFrame = 1
        End If

        ' There should be at least two frames if a background frame is specified
        If ObjGraphic.HasBackgroundFrame = 1 Then

            If LstPNGFiles.Count = 1 Then
                MdlZTStudio.HandledError("MdlTasks", "ConvertFilePNGToZT1", "A ZT1 Graphic needs at least one frame, if a background frame (extra.png) is specified.", False, Nothing)
                Exit Sub
            End If

        End If

        For Each StrPNGFile As String In LstPNGFiles

            ' Extract the index of the frame (or _extra) from the filename
            If Strings.Right(System.IO.Path.GetFileName(StrPNGFile).ToLower(), 9) = "extra.png" Then
                StrPngName = "extra"
            Else
                StrPngName = Strings.Right(System.IO.Path.GetFileNameWithoutExtension(StrPNGFile), 4)
            End If

            If StrPngName = "extra" Then
                ' There's an extra background frame.
                ObjGraphic.HasBackgroundFrame = 1

            End If

            ObjFrame = New ClsFrame(ObjGraphic)

            ' In case of a batch conversion, it's possible a shared color palette (.pal) is enforced.
            ' usually, this would be something like this:
            ' objects/restrant/restrant.pal
            ' animals/ibex/ibex.pal

            ' To make it a bit more simple for the users of ZT Studio and to allow for easier recoloring
            ' (for example: lighter graphics of Red Panda will be used for the female),
            ' it would be better if the palette is not under animals/redpanda/redpanda.pal but animals/redpanda/m/redpanda.pal
            ' This should work for fences etc as well.

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
                                            ' Go-to is usually a bad practice, but it's good here to break out of the 2 (!) loops.
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

            ' Add this frame to the graphic's frame collection
            ObjGraphic.Frames.Add(ObjFrame)

            ' Create a frame from the .PNG-file
            ObjFrame.LoadPNG(StrPNGFile)

        Next StrPNGFile

        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Write graphic...")

        ' Create the ZT1 Graphic.
        ObjGraphic.Write(StrDestinationFileName)

        If Cfg_Export_ZT1_Ani = 1 And BlnSingleConversion = True Then

            MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Generate .ani file")

            ' Only 1 graphic file is being generated (example: icon)
            ' A .ani-file can be generated automatically.
            ' [folder path] + \ + [folder name] + .ani
            Dim ObjAniFile As New ClsAniFile(StrPathDir & "\" & Path.GetFileName(StrPathDir) & ".ani")
            ObjAniFile.CreateAniConfig()

        End If

        MdlZTStudio.Trace("MdlTasks", "ConvertFilePNGToZT1", "Converted PNG-set to ZT1 Graphic")

        ' Clear everything.
        ObjGraphic = Nothing

        Catch ex As Exception
            MdlZTStudio.UnhandledError("MdlTasks", "ConvertFolderPNGToZT1", ex, True)

        Finally
            BlnTaskRunning = False
        End Try

    End Sub

    ''' <summary>
    ''' Task to convert a whole set of folders containing ZT1 Graphics to PNG sets
    ''' </summary>
    ''' <param name="StrPath">Path to search recursively for ZT1 Graphics</param>
    ''' <param name="ObjProgressBar">Progress bar to show progress in</param>
    ''' <param name="ObjCancellationToken">Token which allows cancelling the batch between files</param>
    Public Function ConvertFolderZT1ToPNG(StrPath As String, Optional ObjProgressBar As ProgressBar = Nothing, Optional ObjCancellationToken As CancellationToken = Nothing) As Task

        ' The actual folder walk and per-file conversion run on a background thread, so the UI thread is not blocked.
        ' The work is delegated to a private Sub rather than being written inline in the Task.Run lambda,
        ' so it stays readable and independently callable/testable.
        Return Task.Run(Sub() ConvertFolderZT1ToPNGCore(StrPath, ObjProgressBar, ObjCancellationToken), ObjCancellationToken)

    End Function

    ''' <summary>
    ''' Contains the actual folder walk/conversion logic for <see cref="ConvertFolderZT1ToPNG"/>.
    ''' Runs on whatever thread it is invoked from (a background thread, when called via ConvertFolderZT1ToPNG).
    ''' </summary>
    ''' <param name="StrPath">Path to search recursively for ZT1 Graphics</param>
    ''' <param name="ObjProgressBar">Progress bar to show progress in</param>
    ''' <param name="ObjCancellationToken">Token which allows cancelling the batch between files</param>
    Private Sub ConvertFolderZT1ToPNGCore(StrPath As String, ObjProgressBar As ProgressBar, ObjCancellationToken As CancellationToken)

        Try

            ' Create a recursive list of files

            ' This list stores the results.
            Dim LstResult As New List(Of String)

            ' This stack stores the directories to process.
            Dim Stack As New Stack(Of String)

            ' Add the initial directory
            Stack.Push(StrPath)

            ' Continue processing for each stacked directory
            Do While (Stack.Count > 0)
                ' Get top directory string

                Dim StrDirectoryName As String = Stack.Pop

                For Each StrFileName As String In Directory.GetFiles(StrDirectoryName, "*")
                    ' Only ZT1 files
                    If Path.GetExtension(StrFileName) = vbNullString Then
                        LstResult.Add(StrFileName)
                    End If
                Next

                ' Loop through all subdirectories and add them to the stack.
                Dim StrSubDirectoryName As String
                For Each StrSubDirectoryName In Directory.GetDirectories(StrDirectoryName)
                    Stack.Push(StrSubDirectoryName)
                Next

            Loop

            ' Process the discovered files with per-file error isolation, cancellation, and progress
            ' reporting (see issue #52 - shared by all three folder-batch methods in this module).
            ' BlnBatchOperationRunning must stay active for the whole call: HandledError/UnhandledError
            ' inside ConvertFileZT1ToPNG rely on it being True to raise (so RunFileBatch's per-item
            ' Try/Catch can log and continue) instead of showing a blocking dialog.
            Dim ObjResult As (Succeeded As Integer, Failed As List(Of String), Cancelled As Boolean)
            BlnBatchOperationRunning = True
            Try
                ObjResult = RunFileBatch(
                    LstResult,
                    ObjProgressBar,
                    ObjCancellationToken,
                    Sub(StrZT1GraphicFileName) MdlTasks.ConvertFileZT1ToPNG(StrZT1GraphicFileName),
                    "MdlTasks.ConvertFolderZT1ToPNG",
                    Function(StrZT1GraphicFileName) StrZT1GraphicFileName
                )
            Finally
                BlnBatchOperationRunning = False
            End Try

            ' Skip clean up and the summary if the batch was cancelled: some files were never
            ' processed, so deleting originals or reporting a "final" summary would be misleading.
            If ObjResult.Cancelled = False Then

                ' Clean up original ZT1 Graphic files? (includes palette, does not include .ani file for now!)
                If Cfg_Convert_DeleteOriginal = 1 Then
                    ' Currently clean up of ZT1 Graphics and ZT1 Color palettes is called seperately.
                    ' It might be possible to merge them at some point and you could even gain a small performance boost.
                    MdlTasks.CleanUpFiles(StrPath, "")
                    MdlTasks.CleanUpFiles(StrPath, ".pal")
                End If

                ' Report a summary of the batch run, rather than silently succeeding or hard-crashing.
                Dim StrSummary As String = ObjResult.Succeeded & " file(s) converted successfully."
                If ObjResult.Failed.Count > 0 Then
                    StrSummary = StrSummary & vbCrLf & ObjResult.Failed.Count & " file(s) failed:" & vbCrLf & String.Join(vbCrLf, ObjResult.Failed)
                End If
                MdlZTStudio.InfoBox("MdlTasks", "ConvertFolderZT1ToPNG", StrSummary)

            End If

        Catch ex As Exception
            ' Genuinely fatal: e.g. the target folder itself could not be scanned at all.
            MdlZTStudio.HandledError("MdlTasks", "ConvertFolderZT1ToPNG", "Unexpected error occurred.", True, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Task to convert files in a folder (recursively) from PNG sets to ZT1 Graphics
    ''' </summary>
    ''' <param name="StrSourcePath">Folder (recursive) containing PNG sets</param>
    ''' <param name="ObjProgressBar">ProgressBar</param>
    ''' <param name="ObjCancellationToken">Token which allows cancelling the batch between files</param>
    Public Function ConvertFolderPNGToZT1(StrSourcePath As String, Optional ObjProgressBar As ProgressBar = Nothing, Optional ObjCancellationToken As CancellationToken = Nothing) As Task

        ' The actual folder walk and per-file conversion run on a background thread, so the UI thread is not blocked.
        ' The work is delegated to a private Sub rather than being written inline in the Task.Run lambda,
        ' so it stays readable and independently callable/testable.
        Return Task.Run(Sub() ConvertFolderPNGToZT1Core(StrSourcePath, ObjProgressBar, ObjCancellationToken), ObjCancellationToken)

    End Function

    ''' <summary>
    ''' Contains the actual folder walk/conversion logic for <see cref="ConvertFolderPNGToZT1"/>.
    ''' Runs on whatever thread it is invoked from (a background thread, when called via ConvertFolderPNGToZT1).
    ''' </summary>
    ''' <param name="StrSourcePath">Folder (recursive) containing PNG sets</param>
    ''' <param name="ObjProgressBar">ProgressBar</param>
    ''' <param name="ObjCancellationToken">Token which allows cancelling the batch between files</param>
    Private Sub ConvertFolderPNGToZT1Core(StrSourcePath As String, ObjProgressBar As ProgressBar, ObjCancellationToken As CancellationToken)

        Try

            ' Create a recursive list.

            ' This list stores the results.
            Dim LstFiles As New List(Of String)

            ' This stack stores the directories to process.
            Dim StackDirectories As New Stack(Of String)

            ' Longer error messages
            Dim StrErrorMessage As String

            ' Add the initial directory
            StackDirectories.Push(StrSourcePath)

            ' Continue processing for each stacked directory
            Do While (StackDirectories.Count > 0)
                ' Get top directory string

                Dim StrDirectory As String = StackDirectories.Pop
                Dim StrGraphicName As String

                ' Add all immediate file paths

                For Each StrFileName In Directory.GetFiles(StrDirectory, "*.png")

                    ' Add future graphic name ("full" path, eg animals/redpanda/m/walk/NE)
                    If Strings.Right(Path.GetFileNameWithoutExtension(StrFileName).ToLower, 5 + Strings.Len(Cfg_Convert_FileNameDelimiter)) = Cfg_Convert_FileNameDelimiter & "extra" Then
                        ' 5 (extra) + 4 (.png) + x (delimiter) = 9 + x characters.
                        ' eg objects/yourobj/NE_extra.png
                        StrGraphicName = Strings.Left(StrFileName, Len(StrFileName) - 9 - Strings.Len(Cfg_Convert_FileNameDelimiter))
                    Else
                        ' 4 (0000) + 4 (.png) = 8 chars.
                        ' eg objects/yourobj/NE_0001.png
                        StrGraphicName = Strings.Left(StrFileName, Strings.Len(StrFileName) - 8 - Strings.Len(Cfg_Convert_FileNameDelimiter))
                    End If

                    If LstFiles.Contains(StrGraphicName) = False Then
                        LstFiles.Add(StrGraphicName)
                    End If

                Next

                ' Loop through all subdirectories and add them to the stack.
                Dim StrDirectoryName As String
                For Each StrDirectoryName In Directory.GetDirectories(StrDirectory)

                    ' Just a warning, so users don't accidentally have "sitscratch" as animation name.
                    ' Actually '-' is supported as well.
                    If Path.GetFileName(StrDirectoryName).Length > 8 Or System.Text.RegularExpressions.Regex.IsMatch(Strings.Replace(Path.GetFileName(StrDirectoryName), "-", ""), "^[a-zA-Z0-9_-]+$") = False Then

                        StrErrorMessage =
                            "Directory name '" & Path.GetFileName(StrDirectoryName) & "' is invalid." & vbCrLf &
                            "The limit of a folder name is a maximum of 8 alphanumeric characters." & vbCrLf &
                            "You will need to rename the folder manually and then retry."

                        MdlZTStudio.HandledError("MdlTasks", "ConvertFolderPNGToZT1", StrErrorMessage, True, Nothing)

                    End If

                    StackDirectories.Push(StrDirectoryName)
                Next

            Loop

            ' Process the discovered graphic names with per-item error isolation, cancellation, and
            ' progress reporting (see issue #52 - shared by all three folder-batch methods in this
            ' module). BlnBatchOperationRunning must stay active for the whole call: HandledError/
            ' UnhandledError inside ConvertFilePNGToZT1 rely on it being True to raise (so
            ' RunFileBatch's per-item Try/Catch can log and continue) instead of showing a blocking dialog.
            Dim ObjResult As (Succeeded As Integer, Failed As List(Of String), Cancelled As Boolean)
            BlnBatchOperationRunning = True
            Try
                ObjResult = RunFileBatch(
                    LstFiles,
                    ObjProgressBar,
                    ObjCancellationToken,
                    Sub(StrDestinationGraphicName) MdlTasks.ConvertFilePNGToZT1(StrDestinationGraphicName, False),
                    "MdlTasks.ConvertFolderPNGToZT1",
                    Function(StrDestinationGraphicName) StrDestinationGraphicName
                )
            Finally
                BlnBatchOperationRunning = False
            End Try

            ' Skip clean up and the summary if the batch was cancelled: some files were never
            ' processed, so deleting originals or reporting a "final" summary would be misleading.
            If ObjResult.Cancelled = False Then

                ' Generate a .ani-file in each directory.
                ' Add the initial directory
                MdlBatch.WriteAniFile(StrSourcePath)

                ' Do a clean up of .PNG files if conversion was successful and setting is enabled
                If Cfg_Convert_DeleteOriginal = 1 Then
                    MdlTasks.CleanUpFiles(StrSourcePath, ".png")
                End If

                ' Report a summary of the batch run, rather than silently succeeding or hard-crashing.
                Dim StrSummary As String = ObjResult.Succeeded & " file(s) converted successfully."
                If ObjResult.Failed.Count > 0 Then
                    StrSummary = StrSummary & vbCrLf & ObjResult.Failed.Count & " file(s) failed:" & vbCrLf & String.Join(vbCrLf, ObjResult.Failed)
                End If
                MdlZTStudio.InfoBox("MdlTasks", "ConvertFolderPNGToZT1", StrSummary)

            End If

        Catch ex As Exception
            ' Genuinely fatal: e.g. the source folder itself could not be scanned, or contains an invalid directory name.
            MdlZTStudio.UnhandledError("MdlTasks", "ConvertFolderPNGToZT1", ex, True)
        End Try

    End Sub


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

        If Cfg_Export_ZT1_Ani = 1 Then
            MdlZTStudio.Trace("MdlTasks", "SaveGraphic", "Try .ani")
            ' Get the folder + name of the folder + .ani
            Dim CAni As New ClsAniFile(Path.GetDirectoryName(StrFileName) & "\" & Path.GetFileName(Path.GetDirectoryName(StrFileName)) & ".ani")
            CAni.CreateAniConfig()
        End If

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
    ''' <param name="ObjCancellationToken">Token which allows cancelling the batch between files</param>
    Public Function BatchOffsetFixFolderZT1(StrPath As String, PntOffset As Point, Optional ObjProgressBar As ProgressBar = Nothing, Optional ObjCancellationToken As CancellationToken = Nothing) As Task

        ' The actual folder walk and per-file processing run on a background thread, so the UI thread is not blocked.
        ' The work is delegated to a private Sub rather than being written inline in the Task.Run lambda,
        ' so it stays readable and independently callable/testable.
        Return Task.Run(Sub() BatchOffsetFixFolderZT1Core(StrPath, PntOffset, ObjProgressBar, ObjCancellationToken), ObjCancellationToken)

    End Function

    ''' <summary>
    ''' Contains the actual folder walk/offset-fix logic for <see cref="BatchOffsetFixFolderZT1"/>.
    ''' Runs on whatever thread it is invoked from (a background thread, when called via BatchOffsetFixFolderZT1).
    ''' </summary>
    ''' <param name="StrPath">Path to folder</param>
    ''' <param name="PntOffset">The offsets to apply</param>
    ''' <param name="ObjProgressBar">The bar which will indicate progress</param>
    ''' <param name="ObjCancellationToken">Token which allows cancelling the batch between files</param>
    Private Sub BatchOffsetFixFolderZT1Core(StrPath As String, PntOffset As Point, ObjProgressBar As ProgressBar, ObjCancellationToken As CancellationToken)

        ' Todo: check needed to see if strPath is subfolder of Cfg_Path_Root ?

        Try

            ' Creating a recursive file list.

            ' This list stores the results.
            Dim LstFiles As New List(Of String)

            ' This stack stores the directories to process.
            Dim StackDirectories As New Stack(Of String)

            ' Add the initial directory
            StackDirectories.Push(StrPath)

            ' Continue processing for each stacked directory
            Do While (StackDirectories.Count > 0)
                ' Get top directory string

                Dim StrDirectory As String = StackDirectories.Pop

                For Each strFile As String In Directory.GetFiles(StrDirectory, "*")
                    ' Only ZT1 files
                    If Path.GetExtension(strFile) = vbNullString Then
                        LstFiles.Add(strFile)
                    End If
                Next

                ' Loop through all subdirectories and add them to the stack.
                Dim StrSubDirectoryName As String
                For Each StrSubDirectoryName In Directory.GetDirectories(StrDirectory)
                    StackDirectories.Push(StrSubDirectoryName)
                Next

            Loop

            ' Process the discovered files with per-file error isolation, cancellation, and progress
            ' reporting (see issue #52 - shared by all three folder-batch methods in this module).
            ' BlnBatchOperationRunning must stay active for the whole call: HandledError/UnhandledError
            ' inside the per-file action rely on it being True to raise (so RunFileBatch's per-item
            ' Try/Catch can log and continue) instead of showing a blocking dialog.
            Dim ObjResult As (Succeeded As Integer, Failed As List(Of String), Cancelled As Boolean)
            BlnBatchOperationRunning = True
            Try
                ObjResult = RunFileBatch(
                    LstFiles,
                    ObjProgressBar,
                    ObjCancellationToken,
                    Sub(StrCurrentFile)
                        MdlZTStudio.Trace("MdlTasks", "BatchOffsetFixFolderZT1", "Processing file " & StrCurrentFile)

                        ' Read graphic, update offsets of frames, save.
                        Dim ObjGraphic As New ClsGraphic(Nothing)

                        ObjGraphic.Read(StrCurrentFile)

                        ObjGraphic.Frames(0).UpdateOffsets(PntOffset, True)

                        ObjGraphic.Write(StrCurrentFile)
                    End Sub,
                    "MdlTasks.BatchOffsetFixFolderZT1",
                    Function(StrCurrentFile) StrCurrentFile
                )
            Finally
                BlnBatchOperationRunning = False
            End Try

            ' Skip clean up and the summary if the batch was cancelled: some files were never
            ' processed, so reporting a "final" summary would be misleading.
            If ObjResult.Cancelled = False Then

                ' Generate a .ani-file in each directory.
                ' Add the initial directory
                MdlBatch.WriteAniFile(StrPath)

                ' Report a summary of the batch run, rather than silently succeeding or hard-crashing.
                Dim StrSummary As String = "Finished batch rotation fixing." & vbCrLf &
                    ObjResult.Succeeded & " file(s) fixed successfully."
                If ObjResult.Failed.Count > 0 Then
                    StrSummary = StrSummary & vbCrLf & ObjResult.Failed.Count & " file(s) failed:" & vbCrLf & String.Join(vbCrLf, ObjResult.Failed)
                End If
                MdlZTStudio.InfoBox("MdlTasks", "BatchOffsetFixFolderZT1", StrSummary)

            End If

        Catch ex As Exception
            ' Genuinely fatal: e.g. the target folder itself could not be scanned at all.
            MdlZTStudio.HandledError("MdlTasks", "BatchOffsetFixFolderZT1", "Unexpected error.", False, ex)
        End Try

    End Sub



End Module
