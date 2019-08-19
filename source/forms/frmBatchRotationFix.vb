Public Class FrmBatchRotationFix

    Private Sub FrmBatchRotationFix_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        ' up:
        ' editorFrame.updateOffsets(New Point(0, 16))
        ' left: 
        '  editorFrame.updateOffsets(New Point(16, 0))

        If txtFolder.Text = "" Then txtFolder.Text = Cfg_path_Root



    End Sub

    Private Sub CmdBatchFix_Click(sender As Object, e As EventArgs) Handles cmdBatchFix.Click

        ' find all ZT1 Graphics in this folder
        MdlTasks.Batch_RotationFix_Folder_ZT1(txtFolder.Text, New Point(numLeftRight.Value, numUpDown.Value), PB)


    End Sub

    Private Sub BtnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click

        With dlgBrowseFolder

            .SelectedPath = txtFolder.Text

            .ShowNewFolderButton = True
            .Description = "Select the folder which contains the ZT1 Graphics."
            .ShowDialog()

            txtFolder.Text = .SelectedPath


        End With


    End Sub
End Class