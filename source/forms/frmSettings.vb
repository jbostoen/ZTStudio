Public Class frmSettings

    Private Sub frmSettings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        ' Just re-load the settings here to apply them.
        clsTasks.config_write()

        clsTasks.config_load()


    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        With cboPNGExport_Crop.Items
            .Clear()
            .Add("Keep canvas size (" & cfg_grid_numPixels & " x " & cfg_grid_numPixels & ")")
            .Add("Crop to largest relevant width / height in this graphic")
            .Add("Crop to relevant pixels of this frame")
        End With

        ' Paths
        txtRootFolder.Text = cfg_path_Root
        txtFolderPal8.Text = cfg_path_ColorPals8
        txtFolderPal16.Text = cfg_path_ColorPals16

        ' Export stuff (to PNG)
        chkRenderFrame_BGGraphic.Checked = cfg_export_PNG_RenderBGZT1
        chkRenderFrame_RenderExtraFrame.Checked = cfg_export_PNG_RenderBGFrame
        cboPNGExport_Crop.SelectedIndex = cfg_export_PNG_CanvasSize

        Debug.Print("Value of ZT Ani = " & cfg_export_ZT1_Ani & " - " & (cfg_export_ZT1_Ani = 1))

        ' Export to ZT1
        chkExportZT1_Ani.Checked = (cfg_export_ZT1_Ani = 1)
        chkExportZT1_AddZTAFBytes.Checked = (cfg_export_ZT1_AlwaysAddZTAFBytes = 1)
         

        ' Conversion
        chkConvert_DeleteOriginal.Checked = (cfg_convert_deleteOriginal = 1)
        chkConvert_SharedColorPalette.Checked = (cfg_convert_sharedPalette = 1)
        chkConvert_Overwrite.Checked = (cfg_convert_overwrite = 1)
        numConvert_PNGStartIndex.Value = cfg_convert_startIndex


        ' Frame
        chkEditor_Frame_Offsets_SingleFrame.Checked = (cfg_editor_rotFix_individualFrame = 1)


    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click

        With dlgBrowseFolder

            .SelectedPath = txtRootFolder.Text

            .ShowNewFolderButton = True
            .Description = "Select the root folder which contains a ZT1-folder structure where graphics will come." & vbCrLf & "You are looking for something like this:" & vbCrLf & "[root folder]\objects\bamboo\idle\SE"
            .ShowDialog()

            txtRootFolder.Text = .SelectedPath
            cfg_path_Root = .SelectedPath


        End With

    End Sub

    Private Sub btnBrowsePal8_Click(sender As Object, e As EventArgs) Handles btnBrowsePal8.Click

        With dlgBrowseFolder

            .SelectedPath = txtFolderPal8.Text

            .ShowNewFolderButton = True
            .Description = "Select a folder which contains ZT1 Color Palettes (.pal)," & vbCrLf & "with 8 colors: (usually the filenames are blue8.pal etc)"
            .ShowDialog()

            txtFolderPal8.Text = .SelectedPath
            iniWrite(System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.ini", "paths", "pal8", .SelectedPath)


        End With

    End Sub

    Private Sub btnBrowsePal16_Click(sender As Object, e As EventArgs) Handles btnBrowsePal16.Click

        With dlgBrowseFolder

            .SelectedPath = txtFolderPal16.Text

            .ShowNewFolderButton = True
            .Description = "Select a folder which contains ZT1 Color Palettes (.pal)," & vbCrLf & "with 16 colors: (usually the filenames are blue16.pal etc)"
            .ShowDialog()

            txtFolderPal16.Text = .SelectedPath
            iniWrite(System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.ini", "paths", "pal16", .SelectedPath)


        End With

    End Sub




    Private Sub cboPNGExport_Crop_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboPNGExport_Crop.SelectedValueChanged


        ' 0 = normal;
        ' 1 = cropped to relevant pixels of the largest frame;
        ' 2 = cropped to relevant pixels of this frame;
        If cboPNGExport_Crop.IsHandleCreated = False Then Exit Sub
        cfg_export_PNG_CanvasSize = cboPNGExport_Crop.SelectedIndex



    End Sub

    Private Sub chkRenderFrame_RenderExtraFrame_CheckedChanged(sender As Object, e As EventArgs) Handles chkRenderFrame_RenderExtraFrame.CheckedChanged

        If chkRenderFrame_RenderExtraFrame.IsHandleCreated = False Then Exit Sub
        cfg_export_PNG_RenderBGFrame = CByte(chkRenderFrame_RenderExtraFrame.Checked * -1)


    End Sub

    Private Sub chkExportPNG_BGGraphic_CheckedChanged(sender As Object, e As EventArgs) Handles chkRenderFrame_BGGraphic.CheckedChanged

        If chkRenderFrame_BGGraphic.IsHandleCreated = False Then Exit Sub
        cfg_export_PNG_RenderBGZT1 = CByte(chkRenderFrame_BGGraphic.Checked * -1)

    End Sub

    Private Sub numExportPNG_StartIndex_ValueChanged(sender As Object, e As EventArgs) Handles numConvert_PNGStartIndex.ValueChanged

        If numConvert_PNGStartIndex.IsHandleCreated = False Then Exit Sub
        cfg_convert_startIndex = numConvert_PNGStartIndex.Value

        Debug.Print("Value changed")

    End Sub

    Private Sub chkFrame_Offsets_SingleFrame_CheckedChanged(sender As Object, e As EventArgs) Handles chkEditor_Frame_Offsets_SingleFrame.CheckedChanged

        If chkEditor_Frame_Offsets_SingleFrame.IsHandleCreated = False Then Exit Sub
        cfg_editor_rotFix_individualFrame = CByte(chkEditor_Frame_Offsets_SingleFrame.Checked * -1)

    End Sub


    Private Sub chkConvert_AddZTAFBytes_CheckedChanged(sender As Object, e As EventArgs) Handles chkExportZT1_AddZTAFBytes.CheckedChanged

        If chkExportZT1_AddZTAFBytes.IsHandleCreated = False Then Exit Sub
        cfg_export_ZT1_AlwaysAddZTAFBytes = CByte(chkExportZT1_AddZTAFBytes.Checked * -1)

    End Sub

    Private Sub chkConvert_DeleteOriginal_CheckedChanged(sender As Object, e As EventArgs) Handles chkConvert_DeleteOriginal.CheckedChanged

        If chkConvert_DeleteOriginal.IsHandleCreated = False Then Exit Sub
        cfg_convert_deleteOriginal = CByte(chkConvert_DeleteOriginal.Checked * -1)


    End Sub

    Private Sub chkConvert_Overwrite_CheckedChanged(sender As Object, e As EventArgs) Handles chkConvert_Overwrite.CheckedChanged

        If chkConvert_Overwrite.IsHandleCreated = False Then Exit Sub
        cfg_convert_overwrite = CByte(chkConvert_Overwrite.Checked * -1)

    End Sub


    Private Sub chkExportZT1_Ani_CheckedChanged(sender As Object, e As EventArgs) Handles chkExportZT1_Ani.CheckedChanged

        If chkExportZT1_Ani.IsHandleCreated = False Then Exit Sub
        cfg_export_ZT1_Ani = CByte(chkExportZT1_Ani.Checked * -1) 

    End Sub


    Private Sub tpConversions_Click(sender As Object, e As EventArgs) Handles tpConversions.Click

    End Sub

    Private Sub chkConvert_SharedColorPalette_CheckedChanged(sender As Object, e As EventArgs) Handles chkConvert_SharedColorPalette.CheckedChanged

        If chkConvert_SharedColorPalette.IsHandleCreated = False Then Exit Sub
        cfg_convert_sharedPalette = CByte(chkConvert_SharedColorPalette.Checked * -1)

    End Sub

    Private Sub txtConvert_fileNameDelimiter_TextChanged(sender As Object, e As EventArgs) Handles txtConvert_fileNameDelimiter.TextChanged
        cfg_convert_fileNameDelimiter = txtConvert_fileNameDelimiter.Text
    End Sub
End Class