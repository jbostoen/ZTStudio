Public Class frmBatchRotationFix

    Private Sub frmBatchRotationFix_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        ' up:
        ' editorFrame.updateOffsets(New Point(0, 16))
        ' left: 
        '  editorFrame.updateOffsets(New Point(16, 0))

        If txtFolder.Text = "" Then txtFolder.Text = cfg_path_Root



    End Sub

    Private Sub cmdBatchFix_Click(sender As Object, e As EventArgs) Handles cmdBatchFix.Click


        ' find all ZT1 Graphics in this folder
        clsTasks.batch_rotationfix_folder_ZT1(txtFolder.Text, New Point(numLeftRight.Value, numUpDown.Value), PB)


    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click

        With dlgBrowseFolder

            .SelectedPath = txtFolder.Text

            .ShowNewFolderButton = True
            .Description = "Select the folder which contains the ZT1 Graphics."
            .ShowDialog()

            txtFolder.Text = .SelectedPath


        End With


    End Sub
End Class