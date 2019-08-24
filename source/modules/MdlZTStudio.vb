''' <summary>
''' Handles various tasks related to the program
''' </summary>
Module MdlZTStudio


    ''' <summary>
    ''' Loads settings
    ''' Processes command line parameters
    ''' </summary>
    Sub StartUp()

        On Error GoTo dBug

10:
        ' Load the initial config. 
        ' settings.cfg contains the default values.
        ' Some parameters can be overwritten by the command line parameters; but they are not stored permanently.
        MdlConfig.Load()

20:

        ' We will configure our parameters.
        Dim StrArgAction = vbNullString
        Dim StrArgActionValue = vbNullString
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
                    StrArgAction = "convertfile"
                    StrArgActionValue = argV

            End Select
            ' Parameters?


            ' Process action


        Next

30:
        ' See which action was specified and only do the conversion now.
        ' Users could assume the order of parameters doesn't matter, for instance:
        ' ZTStudio.exe /convertFolder:<path> /ZTAF:1 -> would have been converted already while not respecting this configuration option. 
        ' ZTStudio.exe /ZTAF:1 /convertFolder:<path> -> would correctly apply the configuration option.
        ' Assume users are unaware and make it easy for them not to get frustrated, so only convert at the en:

        Select Case StrArgAction

            Case "convertfile.topng"
                ' Do conversion.
                ' Then exit.
                MdlTasks.Convert_file_ZT1_to_PNG(StrArgActionValue)
                End

            Case "convertfile.tozt1"
                ' Do conversion.
                ' Then exit.
                MdlTasks.Convert_file_PNG_to_ZT1(StrArgActionValue)
                End

            Case "convertfolder.topng"
                ' Do conversion.
                ' Then exit.
                MdlTasks.Convert_folder_ZT1_to_PNG(StrArgActionValue)
                End

            Case "convertfolder.tozt1"

                ' Do conversion.
                ' Then exit.
                MdlTasks.Convert_folder_PNG_to_ZT1(StrArgActionValue)
                End


            Case Else
                ' Default.
                ' Just load.

        End Select

        Exit Sub

dBug:

        If MsgBox("It seems an invalid value for a command line argument was specified (" & argK & ")." & vbCrLf &
            "Please read the proper documentation and specify values properly." & vbCrLf &
            vbCrLf &
            "Example:" & vbCrLf &
            "ZTStudio.exe /convertFolder:path-to-folder /ZTAF:1" & vbCrLf & vbCrLf &
            "Details: error in ClsTasks::ZTStudio_StartUp() at line " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Invalid value for command line argument") = vbOK Then
            End
        End If


    End Sub



    ''' <summary>
    ''' To make unexpected errors look more generic, most of them are now handled by this method.
    ''' </summary>
    ''' <param name="StrClass">Class </param>
    ''' <param name="StrMethod">Method</param>
    ''' <param name="ObjError">Error object (contains number and message)</param>
    Sub UnexpectedError(StrClass As String, strMethod As String, ObjError As ErrObject)

        MdlZTStudio.Trace(StrClass, strMethod, "Unexpected error occurred in " & StrClass & "::" & strMethod & "()")

        Dim StrMessage As String = "" &
            "Sorry, but an unexpected error occurred in " & StrClass & "::" & strMethod & "() at line " & Information.Erl.ToString() & vbCrLf &
            "Error code: " & ObjError.Number.ToString() & vbCrLf &
            ObjError.Description & vbCrLf & vbCrLf &
            "------------------------------------" & vbCrLf &
            "As a precaution, " & Application.ProductName & " will close." & vbCrLf &
            "If you can repeat this error, feel free to report it at " & Cfg_GitHub_URL & "." & vbCrLf &
            "Add as many details (steps to reproduce) as possible, include relevant files in your report."

        If MsgBox(StrMessage, MsgBoxStyle.ApplicationModal + MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Unexpected error occurred") = MsgBoxResult.Ok Then
            End

        End If

    End Sub


    ''' <summary>
    ''' To make expected errors look more generic, most of them are now handled by this method. Some parameters are only meant for tracing details.
    ''' </summary>
    ''' <param name="StrClass">Class </param>
    ''' <param name="StrMethod">Method</param>
    ''' <param name="StrMessage">Message</param>
    ''' <param name="BlnFatal">Fatal error. Defaults to false.</param>
    ''' <param name="ObjError">Error object (contains number and message). Defaults to Nothing.</param>
    Sub ExpectedError(StrClass As String, strMethod As String, StrMessage As String, Optional BlnFatal As Boolean = False, Optional ObjError As ErrObject = Nothing)

        If BlnFatal = True Then
            StrMessage = StrMessage & vbCrLf & vbCrLf & "Since this error may lead to other issues, " & Application.ProductName & " will now close completely."
        End If

        ' Tracing info was provided
        If IsNothing(ObjError) = False Then
            MdlZTStudio.Trace(StrClass, strMethod, "Expected error occurred in " & StrClass & "::" & strMethod & "()")
        End If

        If MsgBox(StrMessage, MsgBoxStyle.ApplicationModal + MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Error occurred") = MsgBoxResult.Ok Then
            If BlnFatal = True Then
                End
            End If
        End If


    End Sub

    ''' <summary>
    ''' To make tracing look more generic
    ''' </summary>
    ''' <param name="StrClass">Class</param>
    ''' <param name="StrMethod">Method</param>
    ''' <param name="StrMessage">Message</param>
    Sub Trace(StrClass As String, StrMethod As String, StrMessage As String)
        Debug.Print(Now.ToString("yyyy-MM-dd HH:mm:ss") & ": " & StrClass & "::" & StrMethod & "(): " & StrMessage)
    End Sub

End Module
