Public Class frmPal

    Private Sub ToolStripStatusLabel1_Click(sender As Object, e As EventArgs) Handles ssFileName.Click

    End Sub


    Private Sub btnUseInMainPal_Click(sender As Object, e As EventArgs) Handles btnUseInMainPal.Click



        'g2.colorPalette.colors().Length
        If IsNothing(editorGraphic.frames) Then
            MsgBox("You will need to open a ZT1 graphic file first," & vbCrLf & "then you can use the recolor feature.", vbOKOnly + vbCritical, "No graphic available")
            Exit Sub

        End If


        Dim intStart As Integer = CInt(InputBox("Index of first color to replace:"))

        'If intStart > (editorGraphic.colorPalette.colors.Count - dgvPal.Rows.Count + 1) Then
        'MsgBox("The index is too high. The max index for this image is " & (editorGraphic.colorPalette.colors.Count - dgvPal.Rows.Count + 1))
        'Exit Sub
        'End If

        If (intStart + dgvPal.Rows.Count - 1) > 256 Then
            MsgBox("ZT1 Color palettes are limited to 256 colors.", vbOKOnly + vbCritical, "Can't replace colors")
            Exit Sub

        End If



        ' Replace
        For Each drRow As DataGridViewRow In dgvPal.Rows
            If drRow.Index <> 0 Then ' dummy color
                If editorGraphic.colorPalette.colors.Count = (intStart + drRow.Index - 1) Then
                    editorGraphic.colorPalette.colors.Add(drRow.DefaultCellStyle.BackColor)
                Else
                    editorGraphic.colorPalette.colors(intStart + drRow.Index - 1) = drRow.DefaultCellStyle.BackColor
                End If
            End If
        Next

        ' Force redraw
        editorGraphic.lastUpdated = Now.ToString("yyyyMMddHHmmss")
        clsTasks.preview_update(frmMain.tbFrames.Value - 1)

        ' Show palette again?
        editorGraphic.colorPalette.fillPaletteGrid(frmMain.dgvPaletteMain)


    End Sub

    Private Sub dgvPal_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPal.CellContentClick

    End Sub

    Private Sub mnuExport_GIMP_TextFile_Click(sender As Object, e As EventArgs)



    End Sub
End Class