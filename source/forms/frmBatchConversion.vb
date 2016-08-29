Public Class frmBatchConversion


    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click


        Me.Enabled = False

        If rbPNG_to_ZT1.Checked = True Then

            clsTasks.convert_folder_PNG_to_ZT1(cfg_path_Root, PB)

        Else


            clsTasks.convert_folder_ZT1_to_PNG(cfg_path_Root, PB)


        End If

        Me.Enabled = True


        MsgBox("Conversion finished.", vbOKOnly + vbInformation, "Conversion finished")

    End Sub

    Private Sub cmdSettings_Click(sender As Object, e As EventArgs) Handles cmdSettings.Click
        frmSettings.ShowDialog(Me)

    End Sub
End Class