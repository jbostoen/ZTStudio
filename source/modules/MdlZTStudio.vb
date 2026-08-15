Imports System.IO

''' <summary>
''' Which kind of ZTStudioException subclass should be raised by HandledError/UnhandledError.
''' Defaults to General everywhere existing call sites don't specify one, so adding this did not
''' require touching every existing call site.
''' </summary>
Public Enum ZTStudioErrorCategory
    General
    FileFormat  ' Parsing/writing a ZT1 Graphic, .pal, .ani, or .gpl/.PNG palette import.
    Config      ' Loading/writing ZT Studio's own settings.cfg.
End Enum

''' <summary>
''' Handles various tasks related to the program
''' </summary>
Module MdlZTStudio


    ''' <summary>
    ''' Loads settings
    ''' Processes command line parameters
    ''' </summary>
    Sub StartUp()

        Try

            ' Load the initial config.
            ' settings.cfg contains the default values.
            ' Some parameters can be overwritten by the command line parameters; but they are not stored permanently.
            MdlConfig.Load()

            ' Configure parameters.
        Dim StrArgAction = vbNullString
        Dim StrArgActionValue = vbNullString
        Dim argK As String
        Dim argV As String

        For Each arg As String In Environment.GetCommandLineArgs()

            Debug.Print(arg)

            ' Arguments are specified as:  ZTStudio.exe /arg1:<val1> /argN:<valN>
            ' Expecting valid arguments.
            argK = Strings.Split(arg.ToLower & ":", ":")(0)
            argV = Strings.Replace(arg, argK & ":", "", , , CompareMethod.Text)

            ' set arguments etc
            Select Case argK

                ' These are actual settings.
                ' If specified; they take priority over the values defined in settings.cfg

                ' Preview
                Case "/preview.bgcolor" : Cfg_grid_BackGroundColor = System.Drawing.Color.FromArgb(CInt(argV))
                Case "/preview.fgcolor" : Cfg_grid_ForeGroundColor = System.Drawing.Color.FromArgb(CInt(argV))
                Case "/preview.zoom" : Cfg_grid_zoom = CInt(argV)
                Case "/preview.footprintX" : Cfg_grid_footPrintX = CByte(argV)
                Case "/preview.footprinty" : Cfg_grid_footPrintY = CByte(argV)

                    ' Paths
                Case "/paths.root" : Cfg_path_Root = argV
                    ' ignore recent paths


                    ' Export options
                Case "/exportoptions.pngcrop" : Cfg_export_PNG_CanvasSize = CByte(argV)
                Case "/exportoptions.pngrenderextraframe" : Cfg_export_PNG_RenderBGFrame = (CByte(argV) = 1)
                Case "/exportoptions.pngrenderextragraphic" : Cfg_export_PNG_RenderBGZT1 = (CByte(argV) = 1) ' this would require to supply the BG graphic. To implement.
                Case "/exportoptions.pngrendertransparentbg" : Cfg_export_PNG_TransparentBG = (CByte(argV) = 1)

                Case "/exportoptions.zt1alwaysaddztafbytes" : Cfg_export_ZT1_AlwaysAddZTAFBytes = (CByte(argV) = 1)
                Case "/exportoptions.zt1ani" : Cfg_export_ZT1_Ani = (CByte(argV) = 1)

                    ' Conversion options
                Case "/conversionoptions.deleteoriginal" : Cfg_convert_deleteOriginal = (CByte(argV) = 1)
                Case "/conversionoptions.filenamedelimiter" : Cfg_convert_deleteOriginal = argV
                Case "/conversionoptions.overwrite" : Cfg_convert_overwrite = (CByte(argV) = 1)
                Case "/conversionoptions.pngfilesindex" : Cfg_convert_startIndex = CByte(argV)
                Case "/conversionoptions.sharedpalette" : Cfg_convert_sharedPalette = (CByte(argV) = 1)

                    ' Editing options
                Case "/editing.animationspeed" : Cfg_frame_defaultAnimSpeed = CInt(argV)
                Case "/editing.individualrotationfix" : Cfg_editor_rotFix_individualFrame = (CByte(argV) = 1)

                    ' Not remembered but can be supplied:  
                Case "/extra.colorquantization" : Cfg_palette_quantization = CByte(argV)

                    ' These are actions. 
                    ' An action can be an automated process doing lots of stuff (e.g. convertfolder)
                Case "/action.convertfolder.topng"
                    StrArgAction = "convertfolder.topng"
                    StrArgActionValue = argV
                Case "/action.convertfolder.tozt1"
                    StrArgAction = "convertfolder.tozt1"
                    StrArgActionValue = argV
                Case "/action.convertfile.topng"
                    StrArgAction = "convertfile.topng"
                    StrArgActionValue = argV
                Case "/action.convertfile.tozt1"
                    StrArgAction = "convertfile.tozt1"
                    StrArgActionValue = argV


                Case "/action.listhashes"
                    StrArgAction = "listhashes"
                    StrArgActionValue = argV

                Case "/action.saveconfig"
                    StrArgAction = "saveconfig"
                    StrArgActionValue = argV

            End Select
            ' Parameters?


            ' Process action


        Next

        ' See which action was specified and only do the conversion now.
        ' Users could assume the order of parameters doesn't matter, for instance:
        ' ZTStudio.exe /convertFolder:<path> /ZTAF:1 -> would have been converted already while not respecting this configuration option. 
        ' ZTStudio.exe /ZTAF:1 /convertFolder:<path> -> would correctly apply the configuration option.
        ' Assume users are unaware and make it easy for them not to get frustrated, so only convert at the en:

        Select Case StrArgAction

            Case "convertfile.topng"
                ' Do conversion.
                ' Then exit.
                MdlTasks.ConvertFileZT1ToPNG(StrArgActionValue)
                Application.DoEvents()
                End

            Case "convertfile.tozt1"
                ' Do conversion.
                ' Then exit.
                MdlTasks.ConvertFilePNGToZT1(StrArgActionValue)
                Application.DoEvents()
                End

            Case "convertfolder.topng"
                ' Do conversion.
                ' Then exit.
                ' There is no UI to keep responsive on this command-line path, so simply block until the batch finishes.
                MdlTasks.ConvertFolderZT1ToPNG(StrArgActionValue).Wait()
                Application.DoEvents()
                End

            Case "convertfolder.tozt1"

                ' Do conversion.
                ' Then exit.
                ' There is no UI to keep responsive on this command-line path, so simply block until the batch finishes.
                MdlTasks.ConvertFolderPNGToZT1(StrArgActionValue).Wait()
                Application.DoEvents()
                End


            Case "listhashes"
                MdlTests.GetHashesOfFilesInFolder(StrArgActionValue, StrArgActionValue & "\hashes.cfg")
                Application.DoEvents()
                End

            Case "saveconfig"
                If StrArgActionValue = 1 Then
                    MdlConfig.Write()
                    Application.DoEvents()
                    End
                End If

            Case Else
                ' Default.
                ' Just load.

        End Select

        Catch ex As Exception
            MdlZTStudio.UnhandledError("MdlZTStudio", "StartUp", ex, True)
        End Try

    End Sub



    ''' <summary>
    ''' Constructs the right ZTStudioException subclass for the given category, so callers of
    ''' HandledError/UnhandledError can Catch a specific failure kind (e.g. ZTStudioFileFormatException)
    ''' instead of having to string-match the generic base type's message.
    ''' </summary>
    Private Function CreateException(StrClass As String, StrMethod As String, ObjException As Exception, ObjCategory As ZTStudioErrorCategory) As ZTStudioException

        Select Case ObjCategory
            Case ZTStudioErrorCategory.FileFormat
                Return New ZTStudioFileFormatException(StrClass, StrMethod, ObjException)
            Case ZTStudioErrorCategory.Config
                Return New ZTStudioConfigException(StrClass, StrMethod, ObjException)
            Case Else
                Return New ZTStudioException(StrClass, StrMethod, ObjException)
        End Select

    End Function

    ''' <summary>
    ''' To make unexpected errors look more generic, most of them are now handled by this method.
    ''' </summary>
    ''' <param name="StrClass">Class </param>
    ''' <param name="StrMethod">Method</param>
    ''' <param name="ObjException">Exception that occurred</param>
    ''' <param name="BlnRaiseException">Boolean</param>
    ''' <param name="ObjCategory">Which ZTStudioException subclass to raise. Defaults to General.</param>
    Sub UnhandledError(StrClass As String, StrMethod As String, ObjException As Exception, BlnRaiseException As Boolean, Optional ObjCategory As ZTStudioErrorCategory = ZTStudioErrorCategory.General)

        MdlZTStudio.Trace(StrClass, StrMethod, "Unexpected error occurred in " & StrClass & "::" & StrMethod & "()")
        MdlZTStudio.LogError(StrClass, StrMethod, "Unhandled error", ObjException)

        If BlnBatchOperationRunning = True Then
            ' Running as part of a batch operation (folder conversion/offset fix): do not block
            ' with a dialog, and do not terminate the whole application over a single file.
            ' Raise instead, so the batch loop's per-file Try/Catch can log the failure (with
            ' the filename) and continue with the next file.
            Throw CreateException(StrClass, StrMethod, ObjException, ObjCategory)
        End If

        Dim StrMessage As String = "" &
            "Sorry, but an unexpected error occurred in " & StrClass & "::" & StrMethod & "()" & vbCrLf &
            ObjException.Message & vbCrLf & vbCrLf &
            "------------------------------------" & vbCrLf &
            "As a precaution, " & Application.ProductName & " will close." & vbCrLf &
            "If you can repeat this error, feel free to report it at " & Cfg_GitHub_URL & "." & vbCrLf &
            "Add as many details (steps to reproduce) as possible, include relevant files in your report." & vbCrLf & vbCrLf &
            "A detailed log entry was written to " & MdlZTStudio.StrLogFilePath & "."

        If MsgBox(StrMessage, MsgBoxStyle.ApplicationModal + MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Unexpected error occurred") = MsgBoxResult.Ok Then
            End
        End If

        If BlnRaiseException = True Then
            Throw CreateException(StrClass, StrMethod, ObjException, ObjCategory)
        End If

    End Sub


    ''' <summary>
    ''' To make expected errors look more generic, most of them are now handled by this method. Some parameters are only meant for tracing details.
    ''' </summary>
    ''' <param name="StrClass">Class </param>
    ''' <param name="StrMethod">Method</param>
    ''' <param name="StrMessage">Message</param>
    ''' <param name="BlnFatal">Fatal error. Defaults to false.</param>
    ''' <param name="ObjException">Exception that occurred, if any. Defaults to Nothing.</param>
    ''' <param name="ObjCategory">Which ZTStudioException subclass to raise. Defaults to General.</param>
    Sub HandledError(StrClass As String, StrMethod As String, StrMessage As String, Optional BlnFatal As Boolean = False, Optional ObjException As Exception = Nothing, Optional ObjCategory As ZTStudioErrorCategory = ZTStudioErrorCategory.General)

        ' Tracing info was provided
        If IsNothing(ObjException) = False Then
            MdlZTStudio.Trace(StrClass, StrMethod, "Expected error occurred in " & StrClass & "::" & StrMethod & "()")
        End If

        MdlZTStudio.LogError(StrClass, StrMethod, StrMessage, ObjException)

        If BlnBatchOperationRunning = True Then
            ' Running as part of a batch operation (folder conversion/offset fix): the failure was
            ' already logged above. Skip the blocking dialog (it would stall an unattended batch)
            ' and skip the fatal End (it would abort the whole batch over a single file). Raise
            ' instead, so the batch loop's per-file Try/Catch can record the failure and continue.
            Throw CreateException(StrClass, StrMethod, If(ObjException, New Exception(StrMessage)), ObjCategory)
        End If

        Dim StrDisplayMessage As String = StrMessage

        If BlnFatal = True Then
            StrDisplayMessage = StrDisplayMessage & vbCrLf & vbCrLf & "Since this error may lead to other issues, " & Application.ProductName & " will now close completely."
        End If

        If MsgBox(StrDisplayMessage, MsgBoxStyle.ApplicationModal + MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Error occurred") = MsgBoxResult.Ok Then
            If BlnFatal = True Then
                End
            End If
        End If

    End Sub


    ''' <summary>
    ''' Full path to the error log file, alongside the executable.
    ''' </summary>
    ReadOnly Property StrLogFilePath As String
        Get
            Return Path.Combine(Application.StartupPath, "ZTStudio_errors.log")
        End Get
    End Property

    ''' <summary>
    ''' Guards writes to the error log file. Batch operations run on a background thread (Task.Run),
    ''' so it is possible for a batch's per-file failures and a foreground error to call LogError at
    ''' the same time; without this, two concurrent File.AppendAllText calls could interleave and
    ''' produce a garbled log entry.
    ''' </summary>
    Private ReadOnly ObjLogFileLock As New Object()

    ''' <summary>
    ''' Appends a structured entry to the error log file. Failures while logging are swallowed;
    ''' logging must never be a source of crashes itself.
    ''' </summary>
    ''' <param name="StrClass">Class</param>
    ''' <param name="StrMethod">Method</param>
    ''' <param name="StrMessage">Contextual message, e.g. which file was being processed</param>
    ''' <param name="ObjException">Exception that occurred, if any. Defaults to Nothing.</param>
    Sub LogError(StrClass As String, StrMethod As String, StrMessage As String, Optional ObjException As Exception = Nothing)

        Try

            Dim StrEntry As String = "" &
                "----------------------------------------" & vbCrLf &
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") & " UTC" & vbCrLf &
                "Class: " & StrClass & vbCrLf &
                "Method: " & StrMethod & vbCrLf &
                "Message: " & StrMessage & vbCrLf

            If IsNothing(ObjException) = False Then
                StrEntry = StrEntry &
                    "Exception: " & ObjException.GetType().FullName & ": " & ObjException.Message & vbCrLf &
                    "StackTrace: " & ObjException.StackTrace & vbCrLf
            End If

            ' Serialize log writes: LogError can be called concurrently from a background batch
            ' thread and the UI thread at the same time, and File.AppendAllText alone does not
            ' guarantee two concurrent callers won't interleave their writes.
            SyncLock ObjLogFileLock
                File.AppendAllText(MdlZTStudio.StrLogFilePath, StrEntry)
            End SyncLock

        Catch
            ' Logging itself must never crash the application; failures here are intentionally ignored.
        End Try

    End Sub

    ''' <summary>
    ''' To make tracing look more generic
    ''' </summary>
    ''' <param name="StrClass">Class</param>
    ''' <param name="StrMethod">Method</param>
    ''' <param name="StrMessage">Message</param>
    Sub Trace(StrClass As String, StrMethod As String, StrMessage As String)
        If cfg_trace = 1 Then
            Debug.Print(Now.ToString("yyyy-MM-dd HH:mm:ss") & ": " & StrClass & "::" & StrMethod & "(): " & StrMessage)
        End If

    End Sub


    ''' <summary>
    ''' To make information message boxes more generic, most of them are now handled by this method. Some parameters are only meant for tracing details.
    ''' </summary>
    ''' <param name="StrClass">Class </param>
    ''' <param name="StrMethod">Method</param>
    ''' <param name="StrMessage">Message</param>
    Sub InfoBox(StrClass As String, StrMethod As String, StrMessage As String)

        MdlZTStudio.Trace(StrClass, StrMethod, "Information shown by " & StrClass & "::" & StrMethod & "()")
        MsgBox(StrMessage, MsgBoxStyle.Information + MsgBoxStyle.ApplicationModal + MsgBoxStyle.OkOnly, "ZT Studio")

    End Sub

End Module
