Option Explicit On

Public Class FrmSettings

    Private Sub FrmSettings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        ' Just re-load the settings here to apply them.
        MdlConfig.write()

        MdlConfig.load()


    End Sub

    Private Sub FrmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        With cboPNGExport_Crop.Items
            .Clear()
            .Add("Keep canvas size (" & (Cfg_grid_numPixels * 2) & " x " & (Cfg_grid_numPixels * 2) & ")")
            .Add("Crop to largest relevant width / height in this graphic")
            .Add("Crop to relevant pixels of this frame")
            .Add("Crop around center (fast but experimental)")
        End With

        ' Paths
        txtRootFolder.Text = Cfg_path_Root
        txtFolderPal8.Text = Cfg_path_ColorPals8
        txtFolderPal16.Text = Cfg_path_ColorPals16

        ' Export stuff (to PNG)
        chkRenderFrame_BGGraphic.Checked = CBool(Cfg_export_PNG_RenderBGZT1)
        chkRenderFrame_RenderExtraFrame.Checked = CBool(Cfg_export_PNG_RenderBGFrame)
        cboPNGExport_Crop.SelectedIndex = Cfg_export_PNG_CanvasSize

        Debug.Print("Value of ZT Ani = " & Cfg_export_ZT1_Ani & " - " & (Cfg_export_ZT1_Ani = 1))

        ' Export to ZT1
        chkExportZT1_Ani.Checked = (Cfg_export_ZT1_Ani = 1)
        chkExportZT1_AddZTAFBytes.Checked = (Cfg_export_ZT1_AlwaysAddZTAFBytes = 1)


        ' Conversion
        chkConvert_DeleteOriginal.Checked = (Cfg_convert_deleteOriginal = 1)
        chkConvert_SharedColorPalette.Checked = (Cfg_convert_sharedPalette = 1)
        chkConvert_Overwrite.Checked = (Cfg_convert_overwrite = 1)
        numConvert_PNGStartIndex.Value = Cfg_convert_startIndex
        chkPNGTransparentBG.Checked = (Cfg_export_PNG_TransparentBG = 1)

        ' Graphic
        numFrameDefaultAnimSpeed.Value = Cfg_frame_defaultAnimSpeed

        ' Palette
        chkPalImportPNGForceAddAll.Checked = (Cfg_palette_import_png_force_add_colors = 1)


    End Sub

    Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click

        With dlgBrowseFolder

            .SelectedPath = txtRootFolder.Text

            .ShowNewFolderButton = True
            .Description = "Select the root folder which contains a ZT1-folder structure where graphics will come." & vbCrLf & "You are looking for something like this:" & vbCrLf & "[root folder]\objects\bamboo\idle\SE"
            .ShowDialog()

            txtRootFolder.Text = .SelectedPath
            Cfg_path_Root = .SelectedPath

            ' Update Explorer pane on main window
            MdlZTStudioUI.UpdateExplorerPane()

        End With

    End Sub

    Private Sub BtnBrowsePal8_Click(sender As Object, e As EventArgs) Handles btnBrowsePal8.Click

        With dlgBrowseFolder

            .SelectedPath = txtFolderPal8.Text

            .ShowNewFolderButton = True
            .Description = "Select a folder which contains ZT1 Color Palettes (.pal)," & vbCrLf & "with 8 colors: (usually the filenames are blue8.pal etc)"
            .ShowDialog()

            txtFolderPal8.Text = .SelectedPath
            IniWrite(System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.ini", "paths", "pal8", .SelectedPath)


        End With

    End Sub

    Private Sub BtnBrowsePal16_Click(sender As Object, e As EventArgs) Handles btnBrowsePal16.Click

        With dlgBrowseFolder

            .SelectedPath = txtFolderPal16.Text

            .ShowNewFolderButton = True
            .Description = "Select a folder which contains ZT1 Color Palettes (.pal)," & vbCrLf & "with 16 colors: (usually the filenames are blue16.pal etc)"
            .ShowDialog()

            txtFolderPal16.Text = .SelectedPath
            IniWrite(System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.ini", "paths", "pal16", .SelectedPath)


        End With

    End Sub




    Private Sub CboPNGExport_Crop_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboPNGExport_Crop.SelectedValueChanged

        ' 0 = normal
        ' 1 = cropped to relevant pixels of the largest frame
        ' 2 = cropped to relevant pixels of this frame
        ' 3 = cropped around center (experimental)
        If cboPNGExport_Crop.IsHandleCreated = False Then
            Exit Sub
        End If

        Cfg_export_PNG_CanvasSize = cboPNGExport_Crop.SelectedIndex

    End Sub

    Private Sub ChkRenderFrame_RenderExtraFrame_CheckedChanged(sender As Object, e As EventArgs) Handles chkRenderFrame_RenderExtraFrame.CheckedChanged

        If chkRenderFrame_RenderExtraFrame.IsHandleCreated = False Then Exit Sub
        Cfg_export_PNG_RenderBGFrame = CByte(chkRenderFrame_RenderExtraFrame.Checked * -1)


    End Sub

    Private Sub ChkExportPNG_BGGraphic_CheckedChanged(sender As Object, e As EventArgs) Handles chkRenderFrame_BGGraphic.CheckedChanged

        If chkRenderFrame_BGGraphic.IsHandleCreated = False Then Exit Sub
        Cfg_export_PNG_RenderBGZT1 = CByte(chkRenderFrame_BGGraphic.Checked * -1)

    End Sub

    Private Sub NumExportPNG_StartIndex_ValueChanged(sender As Object, e As EventArgs) Handles numConvert_PNGStartIndex.ValueChanged

        If numConvert_PNGStartIndex.IsHandleCreated = False Then Exit Sub
        Cfg_convert_startIndex = numConvert_PNGStartIndex.Value

        Debug.Print("Value changed")

    End Sub



    Private Sub ChkConvert_AddZTAFBytes_CheckedChanged(sender As Object, e As EventArgs) Handles chkExportZT1_AddZTAFBytes.CheckedChanged

        If chkExportZT1_AddZTAFBytes.IsHandleCreated = False Then Exit Sub
        Cfg_export_ZT1_AlwaysAddZTAFBytes = CByte(chkExportZT1_AddZTAFBytes.Checked * -1)

    End Sub

    Private Sub ChkConvert_DeleteOriginal_CheckedChanged(sender As Object, e As EventArgs) Handles chkConvert_DeleteOriginal.CheckedChanged

        If chkConvert_DeleteOriginal.IsHandleCreated = False Then Exit Sub
        Cfg_convert_deleteOriginal = CByte(chkConvert_DeleteOriginal.Checked * -1)


    End Sub

    Private Sub ChkConvert_Overwrite_CheckedChanged(sender As Object, e As EventArgs) Handles chkConvert_Overwrite.CheckedChanged

        If chkConvert_Overwrite.IsHandleCreated = False Then Exit Sub
        Cfg_convert_overwrite = CByte(chkConvert_Overwrite.Checked * -1)

    End Sub


    Private Sub ChkExportZT1_Ani_CheckedChanged(sender As Object, e As EventArgs) Handles chkExportZT1_Ani.CheckedChanged

        If chkExportZT1_Ani.IsHandleCreated = False Then Exit Sub
        Cfg_export_ZT1_Ani = CByte(chkExportZT1_Ani.Checked * -1)

    End Sub


    Private Sub TpConversions_Click(sender As Object, e As EventArgs) Handles tpConversions.Click

    End Sub

    Private Sub ChkConvert_SharedColorPalette_CheckedChanged(sender As Object, e As EventArgs) Handles chkConvert_SharedColorPalette.CheckedChanged

        If chkConvert_SharedColorPalette.IsHandleCreated = False Then Exit Sub
        Cfg_convert_sharedPalette = CByte(CInt(chkConvert_SharedColorPalette.Checked) * -1)

    End Sub

    Private Sub TxtConvert_fileNameDelimiter_TextChanged(sender As Object, e As EventArgs) Handles txtConvert_fileNameDelimiter.TextChanged
        Cfg_convert_fileNameDelimiter = txtConvert_fileNameDelimiter.Text
    End Sub

    Private Sub TpRenderingFrames_Click(sender As Object, e As EventArgs) Handles tpRenderingFrames.Click

    End Sub

    Private Sub FrmSettings_MaximizedBoundsChanged(sender As Object, e As EventArgs) Handles Me.MaximizedBoundsChanged

    End Sub

    Private Sub TxtFolderPal8_TextChanged(sender As Object, e As EventArgs) Handles txtFolderPal8.TextChanged

    End Sub

    Private Sub TpFolders_Click(sender As Object, e As EventArgs) Handles tpFolders.Click

    End Sub

    Private Sub TpWritePNG_Click(sender As Object, e As EventArgs) Handles tpWritePNG.Click

    End Sub

    Private Sub ChkPNGTransparentBG_CheckedChanged(sender As Object, e As EventArgs) Handles chkPNGTransparentBG.CheckedChanged

        If chkPNGTransparentBG.IsHandleCreated = False Then Exit Sub
        Cfg_export_PNG_TransparentBG = CByte(CInt(chkPNGTransparentBG.Checked) * -1)
    End Sub

    Private Sub NumFrameAnimSpeed_ValueChanged(sender As Object, e As EventArgs) Handles numFrameDefaultAnimSpeed.ValueChanged

        If numFrameDefaultAnimSpeed.IsHandleCreated = False Then Exit Sub
        Cfg_frame_defaultAnimSpeed = numFrameDefaultAnimSpeed.Value

    End Sub

    Private Sub NumFrameAnimSpeed_VisibleChanged(sender As Object, e As EventArgs) Handles numFrameDefaultAnimSpeed.VisibleChanged

    End Sub

    Private Sub CboPNGExport_Crop_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPNGExport_Crop.SelectedIndexChanged

    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles tpPalette.Click


    End Sub

    Private Sub ChkPalImportPNGForceAddAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkPalImportPNGForceAddAll.CheckedChanged

        If chkPalImportPNGForceAddAll.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_palette_import_png_force_add_colors = CByte(CInt(chkPalImportPNGForceAddAll.Checked) * -1)

    End Sub

    Private Sub TxtRootFolder_TextChanged(sender As Object, e As EventArgs) Handles txtRootFolder.TextChanged

    End Sub

    Private Sub LblRootFolder_MouseHover(sender As Object, e As EventArgs) Handles LblRootFolder.MouseHover

        MdlZTStudioUI.ShowToolTip(LblRootFolder, "Root folder", "This is the project folder. Common subfolders are 'animals', 'objects', ...")

    End Sub

    Private Sub LblRootFolder_Click(sender As Object, e As EventArgs) Handles LblRootFolder.Click

    End Sub

    Private Sub LblPalette8_Click(sender As Object, e As EventArgs) Handles lblPalette8.Click

    End Sub

    Private Sub lblPalette8_MouseHover(sender As Object, e As EventArgs) Handles lblPalette8.MouseHover

        MdlZTStudioUI.ShowToolTip(LblRootFolder, "Folder with 8-color palettes", "The folder containing .pal-files which consist of 8 colors.")

    End Sub
    Private Sub lblPalette16_MouseHover(sender As Object, e As EventArgs) Handles lblPalette8.MouseHover

        MdlZTStudioUI.ShowToolTip(LblRootFolder, "Folder with 16-color palettes", "The folder containing .pal-files which consist of 16 colors.")

    End Sub
End Class