Public Class frmBatchConversion


    Private Sub BtnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click

        ' Prevent double click, clicking too fast etc. 
        ' We'll re-enable this when the batch process has finished.
        Me.Enabled = False

        If rbPNG_to_ZT1.Checked = True Then

            ' Convert entire folder containing PNG-files to ZT1-graphics
            MdlTasks.convert_folder_PNG_to_ZT1(Cfg_path_Root, PB)

        Else

            ' Convert entire folder containing ZT1-graphics to PNG-files
            MdlTasks.convert_folder_ZT1_to_PNG(Cfg_path_Root, PB)


        End If

        Me.Enabled = True

        MsgBox("Conversion finished.", vbOKOnly + vbInformation, "Conversion finished")

    End Sub

    Private Sub cmdSettings_Click(sender As Object, e As EventArgs) Handles cmdSettings.Click
        frmSettings.ShowDialog(Me)

    End Sub
End Class