Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup


            Debug.Print("Config: load.")

            clsTasks.config_load()

            Debug.Print("Config: OK.")


            ' We will configure our parameters.
            'Dim strArgAction = vbNullString

            For Each arg As String In Environment.GetCommandLineArgs()
                Debug.Print("Arg: '" & arg & "'")



                Dim argK As String = Strings.Split(arg.ToLower & ":", ":")(0)
                Dim argV As String = Strings.Replace(arg, argK & ":", "", , , CompareMethod.Text)

                ' set arguments etc
                Select Case argK

                    Case "/crop" : cfg_export_PNG_CanvasSize = CByte(argV)
                    Case "/startindex" : cfg_convert_startIndex = CByte(argV)
                    Case "/cleanup" : cfg_convert_deleteOriginal = CByte(argV)
                    Case "/ztaf" : cfg_export_ZT1_AlwaysAddZTAFBytes = CByte(argV)
                    Case "/rootfolder" : cfg_path_Root = argV


                    Case "/convertfolder"
                        'MsgBox(argV)
                        'added a parameter for FPS from blender
                        'could be ushort or mayyyybe even byte
                        clsTasks.convert_folder_PNG_to_ZT1(cfg_path_Root, Int(argV))
                        'MsgBox("Done with batch conversion!")
                        End
                    Case "/convertfile"

                End Select
                ' Parameters?


                ' Process action


            Next

            ' Now perform action.


            ' Now, end.
            'Select Case vbNullString
            'Case "convertfile"

            ' Do conversion
            'End

            'Case "convertfolder"

            '        ' Do conversion
            '        End
            '    Case Else
            '        ' Default.
            '        ' Just load.

            'End Select

        End Sub
    End Class



End Namespace

