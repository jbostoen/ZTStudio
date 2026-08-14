Imports System.Threading

''' <summary>
''' Form which briefly explains to users how to prepare files and what to expect from a batch graphic conversion.
''' </summary>
Public Class FrmBatchConversion

    ''' <summary>
    ''' Tracks cancellation of the batch conversion currently running, if any.
    ''' </summary>
    Private ObjCancellationTokenSource As CancellationTokenSource

    ''' <summary>
    ''' Handles what happens when the user clicks the Convert button.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Async Sub BtnConvert_Click(sender As Object, e As EventArgs) Handles BtnConvert.Click

        ' Prevent double click, clicking too fast etc.
        ' Re-enable this when the batch process has finished.
        BtnConvert.Enabled = False
        gbType.Enabled = False
        BtnSettings.Enabled = False
        BtnCancel.Enabled = True

        ObjCancellationTokenSource = New CancellationTokenSource()
        Dim ObjToken As CancellationToken = ObjCancellationTokenSource.Token

        Try
            If RbPNG_to_ZT1.Checked = True Then

                ' Convert entire folder containing PNG-files to ZT1-graphics
                Await MdlTasks.ConvertFolderPNGToZT1(Cfg_Path_Root, PBBatchProgress, ObjToken)

            Else

                ' Convert entire folder containing ZT1-graphics to PNG-files
                Await MdlTasks.ConvertFolderZT1ToPNG(Cfg_Path_Root, PBBatchProgress, ObjToken)

            End If
        Finally
            ObjCancellationTokenSource.Dispose()
            ObjCancellationTokenSource = Nothing
        End Try

        ' After batch conversion, clean up of files may have happend; or new files created
        MdlZTStudioUI.UpdateExplorerPane()

        BtnConvert.Enabled = True
        gbType.Enabled = True
        BtnSettings.Enabled = True
        BtnCancel.Enabled = False

        If ObjToken.IsCancellationRequested = True Then
            ZTStudio.InfoBox(Me.GetType().FullName, "Click", "Batch conversion cancelled.")
        Else
            ZTStudio.InfoBox(Me.GetType().FullName, "Click", "Batch conversion finished successfully.")
        End If

    End Sub

    ''' <summary>
    ''' Handles what happens when the user clicks the Cancel button, requesting cancellation of the running batch conversion.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click

        If IsNothing(ObjCancellationTokenSource) = False Then
            ObjCancellationTokenSource.Cancel()
        End If

        BtnCancel.Enabled = False

    End Sub

    ''' <summary>
    ''' Handles clicking on "Settings" button and shows the Settings window
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub BtnSettings_Click(sender As Object, e As EventArgs) Handles BtnSettings.Click

        ' Show Settings form (shortcut)
        FrmSettings.ShowDialog(Me)

    End Sub

    ''' <summary>
    ''' Handles form loading
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub FrmBatchConversion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Icon = FrmMain.Icon

    End Sub

    Private Sub FrmBatchConversion_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        ' Request cancellation of a batch conversion that may still be running in the background,
        ' so it does not keep trying to marshal progress updates onto controls that are about to be disposed.
        If IsNothing(ObjCancellationTokenSource) = False Then
            ObjCancellationTokenSource.Cancel()
        End If

        Me.Dispose()

    End Sub
End Class