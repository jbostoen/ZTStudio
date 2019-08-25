''' <summary>
''' Form which briefly explains to users how to prepare files and what to expect from a batch graphic conversion.
''' </summary>
Public Class FrmBatchConversion

    ''' <summary>
    ''' Handles what happens when the user clicks the Convert button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnConvert_Click(sender As Object, e As EventArgs) Handles BtnConvert.Click

        ' Prevent double click, clicking too fast etc. 
        ' Re-enable this when the batch process has finished.
        Me.Enabled = False

        If RbPNG_to_ZT1.Checked = True Then

            ' Convert entire folder containing PNG-files to ZT1-graphics
            MdlTasks.Convert_folder_PNG_to_ZT1(Cfg_path_Root, PBBatchProgress)

        Else

            ' Convert entire folder containing ZT1-graphics to PNG-files
            MdlTasks.Convert_folder_ZT1_to_PNG(Cfg_path_Root, PBBatchProgress)

        End If

        Me.Enabled = True

        ZTStudio.InfoBox(Me.GetType().FullName, "Click", "Batch conversion finished successfully.")

    End Sub

    Private Sub BtnSettings_Click(sender As Object, e As EventArgs) Handles BtnSettings.Click

        ' Show Settings form (shortcut)
        FrmSettings.ShowDialog(Me)

    End Sub

    Private Sub FrmBatchConversion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class