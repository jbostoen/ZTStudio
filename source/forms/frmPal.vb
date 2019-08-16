Public Class frmPal

    Private Sub ToolStripStatusLabel1_Click(sender As Object, e As EventArgs) Handles ssFileName.Click

    End Sub


    Private Sub BtnUseInMainPal_Click(sender As Object, e As EventArgs) Handles btnUseInMainPal.Click


        'g2.colorPalette.colors().Length
        If IsNothing(editorGraphic.Frames) Then

            MsgBox("You will need to open a ZT1 graphic file first," & vbCrLf & "then you can use the recolor feature.",
                   vbOKOnly + vbCritical,
                   "No graphic available")

            Exit Sub

        End If


        Dim input As String = InputBox("" &
                "Index of first color to replace (can not be 0 since we ignore transparent colors)." & vbCrLf &
                "For example, the index for the Restaurant would be 248 (roof 8 colors) or 232 (roof 16 colors)", "Index of the first color to replace", "1")
        If input = vbNullString Then Exit Sub 'user chickened out

        Dim intStart As Integer = CInt(input)

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
            ' Ignore first = transparent color. It doesn't matter.
            ' However, when we are replacing a color, it DOES matter.
            ' Pal8 and Pal16 files contain the transparent color, then the 8 or 16 shades of a color.


            If drRow.Index <> 0 Then ' dummy color

                If editorGraphic.ColorPalette.Colors.Count = (intStart + drRow.Index - 1) Then
                    ' Color index does not exist.
                    editorGraphic.ColorPalette.Colors.Add(drRow.DefaultCellStyle.BackColor)
                Else
                    ' Color index already existed. Overwrite.
                    editorGraphic.ColorPalette.Colors(intStart + drRow.Index - 1) = drRow.DefaultCellStyle.BackColor
                End If

            End If
        Next

        ' Force redraw. In this case, we should ignore the rendered images and make them regenerate from HEX.
        For Each ztFrame As ClsFrame In editorGraphic.Frames
            ztFrame.CoreImageBitmap = Nothing
        Next


        editorGraphic.LastUpdated = Now.ToString("yyyyMMddHHmmss")
        clsTasks.Update_preview(FrmMain.TbFrames.Value - 1)

        ' Show palette again?
        editorGraphic.ColorPalette.FillPaletteGrid(FrmMain.dgvPaletteMain)


    End Sub

    Private Sub DgvPal_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPal.CellContentClick

    End Sub

    Private Sub MnuExport_GIMP_TextFile_Click(sender As Object, e As EventArgs)



    End Sub
End Class