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


            MdlZTStudio.StartUp()


        End Sub

        ''' <summary>
        ''' Last-resort safety net for exceptions that were not caught anywhere else in the application.
        ''' Logs the exception, shows the standard friendly error dialog, and lets the user decide
        ''' whether to try to continue working or close the application.
        ''' </summary>
        Private Sub MyApplication_UnhandledException(sender As Object, e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException

            Try

                MdlZTStudio.LogError("MyApplication", "UnhandledException", "Unhandled exception reached the application-wide safety net.", e.Exception)

                Dim StrMessage As String = "" &
                    "Sorry, but an unexpected error occurred that was not handled anywhere else in " & Application.ProductName & "." & vbCrLf &
                    e.Exception.Message & vbCrLf & vbCrLf &
                    "------------------------------------" & vbCrLf &
                    "If you can repeat this error, feel free to report it at " & Cfg_GitHub_URL & "." & vbCrLf &
                    "Add as many details (steps to reproduce) as possible, include relevant files in your report." & vbCrLf & vbCrLf &
                    "A detailed log entry was written to " & MdlZTStudio.StrLogFilePath & "." & vbCrLf & vbCrLf &
                    "Do you want to try to continue working? Choosing ""No"" will close " & Application.ProductName & "."

                If MsgBox(StrMessage, MsgBoxStyle.ApplicationModal + MsgBoxStyle.YesNo + MsgBoxStyle.Critical, "Unexpected error occurred") = MsgBoxResult.No Then
                    e.ExitApplication = True
                Else
                    e.ExitApplication = False
                End If

            Catch
                ' As a last-resort handler, this must not itself throw; let the application close if it does.
                e.ExitApplication = True
            End Try

        End Sub
    End Class



End Namespace

