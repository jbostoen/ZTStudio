''' <summary>
''' Form which briefly explains to users how to prepare files and what to expect from a batch graphic conversion.
''' </summary>
Public Class FrmBatchConversion

    ''' <summary>
    ''' Handles what happens when the user clicks the Convert button.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub BtnConvert_Click(sender As Object, e As EventArgs) Handles BtnConvert.Click

        ' Prevent double click, clicking too fast etc. 
        ' Re-enable this when the batch process has finished.
        Me.Enabled = False

        If RbPNG_to_ZT1.Checked = True Then

            ' Convert entire folder containing PNG-files to ZT1-graphics
            MdlTasks.ConvertFolderPNGToZT1(Cfg_Path_Root, PBBatchProgress)

        Else

            ' Convert entire folder containing ZT1-graphics to PNG-files
            MdlTasks.ConvertFolderZT1ToPNG(Cfg_Path_Root, PBBatchProgress)

        End If

        ' After batch conversion, clean up of files may have happend; or new files created
        MdlZTStudioUI.UpdateExplorerPane()

        Me.Enabled = True

        ZTStudio.InfoBox(Me.GetType().FullName, "Click", "Batch conversion finished successfully.")

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

        Me.Dispose()

    End Sub
End Class