''' <summary>
''' Color Palette window
''' </summary>
Public Class FrmPal

    ''' <summary>
    ''' Handles color replacement in main color palette (in main window). 
    ''' This allows to quickly preview how graphics would look like too, with a different color pattern.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub BtnUseInMainPal_Click(sender As Object, e As EventArgs) Handles BtnUseInMainPal.Click

        ' If no main graphic has been loaded or created with at least 1 frame, there is nothing to replace.
        If IsNothing(EditorGraphic.Frames) Then

            MdlZTStudio.HandledError(Me.GetType().FullName, "BtnUseInMainPal_Click", "You will need to open a ZT1 graphic file first before you can use this recolor feature.")
            Exit Sub

        End If

        Dim IntMaxColorIndex As Integer = FrmMain.DgvPaletteMain.Rows.Count - Me.DgvPal.Rows.Count + 1

        ' Can't be negative (main color palette color count is smaller than the count of this palette)
        If IntMaxColorIndex < 1 Then
            MdlZTStudio.HandledError(Me.GetType().FullName, "BtnUseInMainPal_Click", "The color palette in the main window has less colors than this palette. Replacing is not possible.")
            Exit Sub
        End If

        Dim StrMessage As String = "" &
                "Index of first color to replace (can not be 0 since the transparent color is ignored)." & vbCrLf & vbCrLf &
                "For example, the index for the Restaurant would be 248 (roof 8 colors) or 232 (other roof 16 colors)" & vbCrLf & vbCrLf &
                "With the current palette, the index to start from should be between 1 and " & IntMaxColorIndex

        Dim strInput As String = InputBox(StrMessage, "Index of the first color to replace", "1")

        If strInput = vbNullString Then
            Exit Sub 'user pressed cancel

        ElseIf IsNumeric(strInput) = False Then

            ' Verify
            MdlZTStudio.HandledError(Me.GetType().FullName, "BtnUseInMainPal_Click", "You need to specify a positive number, at least 1 and maximum " & IntMaxColorIndex & ".")
            Exit Sub

        ElseIf Int(strInput) > IntMaxColorIndex Then

            MdlZTStudio.HandledError(Me.GetType().FullName, "BtnUseInMainPal_Click", "You need to specify a positive number, at least 1 and maximum " & IntMaxColorIndex & ".")
            Exit Sub

        End If

        ' Replace
        For Each ObjDataRow As DataGridViewRow In DgvPal.Rows

            ' Careful: the game uses color palettes consisting of 1 transparent color; followed by 8 or 16 shades of a color for color customization.

            If ObjDataRow.Index <> 0 Then ' Transparent color, ignore this, it doesn't matter.

                If EditorGraphic.ColorPalette.Colors.Count = (CInt(strInput) + ObjDataRow.Index - 1) Then
                    ' Color index does not exist.
                    EditorGraphic.ColorPalette.Colors.Add(ObjDataRow.DefaultCellStyle.BackColor)
                Else
                    ' Color index already existed. Overwrite.
                    EditorGraphic.ColorPalette.Colors(CInt(strInput) + ObjDataRow.Index - 1) = ObjDataRow.DefaultCellStyle.BackColor
                End If

            End If
        Next

        ' The color palette in the main window has been changed.
        ' This means cached versions of images (CoreImageBitmap) should be regenerated from hex values.
        ' SO, set CoreImageBitMap to Nothing (will be rendered when needed; not instantly!)
        For Each ObjFrame As ClsFrame In EditorGraphic.Frames
            ObjFrame.CoreImageBitmap = Nothing
        Next


        EditorGraphic.LastUpdated = Now.ToString("yyyyMMddHHmmss")
        MdlZTStudioUI.UpdatePreview(True, False, FrmMain.TbFrames.Value - 1)

        ' Show palette again?
        EditorGraphic.ColorPalette.FillPaletteGrid(FrmMain.DgvPaletteMain)


    End Sub


End Class