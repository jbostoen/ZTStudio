

Imports System.Threading

''' <summary>
''' <para>
'''     Form which allows to specify offsets which are applied to all views within the selected folder.
''' </para>
''' <para>
'''     This is specifically meant for importing images from a different program, such as Blender;
'''     where it is possible the animal's (Y) offset needs to be adjusted for each frame in every view.
''' </para>
''' </summary>
Public Class FrmBatchOffsetFix

    ''' <summary>
    ''' Tracks cancellation of the batch offset fix currently running, if any.
    ''' </summary>
    Private ObjCancellationTokenSource As CancellationTokenSource

    ''' <summary>
    ''' Initializes window
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub FrmBatchRotationFix_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Take icon from main screen
        Me.Icon = FrmMain.Icon

        ' Assume default folder
        If TxtFolder.Text = "" Then
            TxtFolder.Text = Cfg_path_Root
        End If

    End Sub

    ''' <summary>
    ''' On click, run batch rotation fixing
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Async Sub BtnBatchOffsetting_Click(sender As Object, e As EventArgs) Handles BtnBatchOffsettFix.Click

        ' Prevent double click, clicking too fast etc.
        ' Re-enable this when the batch process has finished.
        BtnBatchOffsettFix.Enabled = False
        btnSelect.Enabled = False
        BtnCancel.Enabled = True

        ObjCancellationTokenSource = New CancellationTokenSource()

        Try
            ' Runs procedure
            Await MdlTasks.BatchOffsetFixFolderZT1(TxtFolder.Text, New Point(numLeftRight.Value, numUpDown.Value), PBProgress, ObjCancellationTokenSource.Token)
        Finally
            ObjCancellationTokenSource.Dispose()
            ObjCancellationTokenSource = Nothing
        End Try

        BtnBatchOffsettFix.Enabled = True
        btnSelect.Enabled = True
        BtnCancel.Enabled = False

    End Sub

    ''' <summary>
    ''' On click, requests cancellation of the running batch offset fix.
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
    ''' On click, show folder selection dialog
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub BtnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click

        ' Allows user to select a different folder
        With dlgBrowseFolder

            .SelectedPath = TxtFolder.Text

            .ShowNewFolderButton = False ' 20190825 this used to be true?! Why would a new folder be created at this point?
            .Description = "Select the folder which contains the ZT1 Graphics."
            .ShowDialog()

            TxtFolder.Text = .SelectedPath

        End With

    End Sub

    ''' <summary>
    ''' Handles form closing. Requests cancellation of a batch offset fix that may still be running in the
    ''' background, so it does not keep trying to marshal progress updates onto controls that are about to be disposed.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">FormClosingEventArgs</param>
    Private Sub FrmBatchOffsetFix_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If IsNothing(ObjCancellationTokenSource) = False Then
            ObjCancellationTokenSource.Cancel()
        End If

    End Sub
End Class