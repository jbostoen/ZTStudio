Option Explicit On

''' <summary>
''' Form which allows the user to change some settings.
''' </summary>
Public Class FrmSettings

    ''' <summary>
    ''' On unload form, save config.
    ''' </summary>
    ''' <param name="sender">ObjectObjectObjectObjectObject</param>
    ''' <param name="e">FormClosingEventArgs</param>
    Private Sub FrmSettings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        ' Just re-load the settings here to apply them.
        MdlConfig.write()

        ' 20191005 Not recalling why reloading is necessary? Likely redundant.
        MdlConfig.load()


    End Sub

    ''' <summary>
    ''' On load form, initialize some controls to display the current configuration.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub FrmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Icon = FrmMain.Icon

        ' Dynamically sets size.
        With CboPNGExport_Crop.Items
            .Clear()
            .Add("Keep canvas size (" & (Cfg_grid_numPixels * 2) & " x " & (Cfg_grid_numPixels * 2) & ")")
            .Add("Crop to largest relevant width / height in this graphic")
            .Add("Crop to relevant pixels of this frame")
            .Add("Crop around center (fast but experimental)")
        End With

        ' Paths
        txtRootFolder.Text = Cfg_path_Root
        txtFolderPal8.Text = Cfg_path_ColorPals8
        txtFolderPal16.Text = Cfg_Path_ColorPals16

        ' Export stuff (to PNG)
        ChkRenderFrame_BGGraphic.Checked = CBool(Cfg_Export_PNG_RenderBGZT1)
        ChkPNGRenderBGFrame.Checked = (Cfg_Export_PNG_RenderBGFrame = 1)
        CboPNGExport_Crop.SelectedIndex = Cfg_export_PNG_CanvasSize

        ' Export to ZT1
        ChkExportZT1_Ani.Checked = (Cfg_export_ZT1_Ani = 1)
        ChkExportZT1_AddZTAFBytes.Checked = (Cfg_export_ZT1_AlwaysAddZTAFBytes = 1)

        ' Conversion
        ChkConvert_DeleteOriginal.Checked = (Cfg_convert_deleteOriginal = 1)
        ChkConvert_SharedColorPalette.Checked = (Cfg_convert_sharedPalette = 1)
        ChkConvert_Overwrite.Checked = (Cfg_convert_overwrite = 1)
        NumConvert_PNGStartIndex.Value = Cfg_Convert_StartIndex

        ' exportOptions PNG
        ChkPNGTransparentBG.Checked = (Cfg_Export_PNG_TransparentBG = 1)
        ChkPNGRenderBGFrame.Checked = (Cfg_Export_PNG_RenderBGFrame = 1)

        ' Graphic
        numFrameDefaultAnimSpeed.Value = Cfg_frame_defaultAnimSpeed

        ' Palette
        ChkPalImportPNGForceAddAll.Checked = (Cfg_palette_import_png_force_add_colors = 1)


    End Sub

    ''' <summary>
    ''' Button click triggers select folder dialog, so user can select root project folder
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles BtnBrowse.Click

        With dlgBrowseFolder

            .SelectedPath = txtRootFolder.Text

            .ShowNewFolderButton = True
            .Description = "Select the root folder which contains a ZT1-folder structure where graphics will come." & vbCrLf &
                "You are looking for something like this:" & vbCrLf & vbCrLf &
                "[root folder]\animals\dolphin\..." & vbCrLf &
                "[root folder]\objects\bamboo\..."
            .ShowDialog()

            txtRootFolder.Text = .SelectedPath
            Cfg_path_Root = .SelectedPath

            ' Update Explorer pane on main window
            MdlZTStudioUI.UpdateExplorerPane()

        End With

    End Sub

    ''' <summary>
    ''' Button click triggers select folder dialog, so user can select a folder which contains color palettes with 8 (+1) colors in them.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub BtnBrowsePal8_Click(sender As Object, e As EventArgs) Handles BtnBrowsePal8.Click

        With dlgBrowseFolder

            .SelectedPath = txtFolderPal8.Text

            .ShowNewFolderButton = True
            .Description = "Select a folder which contains ZT1 Color Palettes (.pal)," & vbCrLf &
                "with 8 colors: (usually the filenames are blue8.pal etc)"
            .ShowDialog()

            txtFolderPal8.Text = .SelectedPath
            IniWrite(System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.ini", "paths", "pal8", .SelectedPath)


        End With

    End Sub

    ''' <summary>
    ''' Button click triggers select folder dialog, so user can select a folder which contains color palettes with 16 (+1) colors in them.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub BtnBrowsePal16_Click(sender As Object, e As EventArgs) Handles BtnBrowsePal16.Click

        With dlgBrowseFolder

            .SelectedPath = txtFolderPal16.Text

            .ShowNewFolderButton = True
            .Description = "Select a folder which contains ZT1 Color Palettes (.pal)," & vbCrLf & "with 16 colors: (usually the filenames are blue16.pal etc)"
            .ShowDialog()

            txtFolderPal16.Text = .SelectedPath
            IniWrite(System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.ini", "paths", "pal16", .SelectedPath)


        End With

    End Sub

    ''' <summary>
    ''' Handles selection of different PNG Export method
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub CboPNGExport_Crop_SelectedValueChanged(sender As Object, e As EventArgs) Handles CboPNGExport_Crop.SelectedValueChanged

        ' 0 = normal
        ' 1 = cropped to relevant pixels of the largest frame
        ' 2 = cropped to relevant pixels of this frame
        ' 3 = cropped around center (experimental)
        If CboPNGExport_Crop.IsHandleCreated = False Then
            Exit Sub
        End If

        Cfg_export_PNG_CanvasSize = CboPNGExport_Crop.SelectedIndex

    End Sub

    ''' <summary>
    ''' Handles toggling of whether there's a ZT1 Graphic selected which needs to be rendered as background
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkExportPNG_BGGraphic_CheckedChanged(sender As Object, e As EventArgs) Handles ChkRenderFrame_BGGraphic.CheckedChanged

        If ChkRenderFrame_BGGraphic.IsHandleCreated = False Then
            Exit Sub
        End If

        Cfg_Export_PNG_RenderBGZT1 = CByte(ChkRenderFrame_BGGraphic.Checked * -1)

        ' Update preview in main window instantly
        MdlZTStudioUI.UpdatePreview(False, True)


    End Sub

    ''' <summary>
    ''' Handles changes of the index numbering for PNG files
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub NumExportPNG_StartIndex_ValueChanged(sender As Object, e As EventArgs) Handles NumConvert_PNGStartIndex.ValueChanged

        If NumConvert_PNGStartIndex.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Convert_StartIndex = NumConvert_PNGStartIndex.Value

        Debug.Print("Value changed")

    End Sub



    ''' <summary>
    ''' Handles toggling of whether ZTAF bytes should always be added to the beginning of a ZT1 Graphic file on creation
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkConvert_AddZTAFBytes_CheckedChanged(sender As Object, e As EventArgs) Handles ChkExportZT1_AddZTAFBytes.CheckedChanged

        If ChkExportZT1_AddZTAFBytes.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Export_ZT1_AlwaysAddZTAFBytes = CByte(ChkExportZT1_AddZTAFBytes.Checked * -1)

    End Sub

    ''' <summary>
    ''' Handles toggling of whether the source files should automatically be deleted after a conversion from one format to another (ZT1 - PNG)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkConvert_DeleteOriginal_CheckedChanged(sender As Object, e As EventArgs) Handles ChkConvert_DeleteOriginal.CheckedChanged

        If ChkConvert_DeleteOriginal.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Convert_DeleteOriginal = CByte(ChkConvert_DeleteOriginal.Checked * -1)


    End Sub

    ''' <summary>
    ''' Handles toggling of whether files can automatically be overwritten
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkConvert_Overwrite_CheckedChanged(sender As Object, e As EventArgs) Handles ChkConvert_Overwrite.CheckedChanged

        If ChkConvert_Overwrite.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Convert_Overwrite = CByte(ChkConvert_Overwrite.Checked * -1)

    End Sub


    ''' <summary>
    ''' Handles toggling of whether an .ani-file should be generated
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkExportZT1_Ani_CheckedChanged(sender As Object, e As EventArgs) Handles ChkExportZT1_Ani.CheckedChanged

        If ChkExportZT1_Ani.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Export_ZT1_Ani = CByte(ChkExportZT1_Ani.Checked * -1)

    End Sub


    ''' <summary>
    ''' Handles toggling of whether ZT Studio should try to use a shared color palette, if present, when creating ZT1 Graphics
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkConvert_SharedColorPalette_CheckedChanged(sender As Object, e As EventArgs) Handles ChkConvert_SharedColorPalette.CheckedChanged

        If ChkConvert_SharedColorPalette.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Convert_SharedPalette = CByte(CInt(ChkConvert_SharedColorPalette.Checked) * -1)

    End Sub

    ''' <summary>
    ''' Handles changes in the file name delimiter
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TxtConvert_fileNameDelimiter_TextChanged(sender As Object, e As EventArgs) Handles TxtConvert_fileNameDelimiter.TextChanged
        Cfg_Convert_FileNameDelimiter = TxtConvert_fileNameDelimiter.Text
    End Sub


    ''' <summary>
    ''' Handles toggling of whether PNGs should be exported with a transparent background or the chosen background color in ZT Studio
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkPNGTransparentBG_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPNGTransparentBG.CheckedChanged

        If ChkPNGTransparentBG.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Export_PNG_TransparentBG = CByte(CInt(ChkPNGTransparentBG.Checked) * -1)
    End Sub

    ''' <summary>
    ''' Handles toggling of whether PNGs should be exported with the last ("extra") frame as background
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkPNGRenderBGFrame_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPNGRenderBGFrame.CheckedChanged

        If ChkPNGRenderBGFrame.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Export_PNG_RenderBGFrame = CByte(CInt(ChkPNGRenderBGFrame.Checked) * -1)

        ' Update preview in main window instantly
        MdlZTStudioUI.UpdatePreview(True, True)

    End Sub


    ''' <summary>
    ''' Handles changes in the default animation speed for new graphics
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub NumFrameAnimSpeed_ValueChanged(sender As Object, e As EventArgs) Handles numFrameDefaultAnimSpeed.ValueChanged

        If numFrameDefaultAnimSpeed.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Frame_DefaultAnimSpeed = numFrameDefaultAnimSpeed.Value

    End Sub


    ''' <summary>
    ''' Handles toggling of whether colors should always be added (rather than only when unique) on importing PNG files
    ''' </summary>
    ''' <remarks>After recolors, the palette may contain identical colors. However, hex indexes in the frame may not be updated yet!</remarks>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkPalImportPNGForceAddAll_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPalImportPNGForceAddAll.CheckedChanged

        If ChkPalImportPNGForceAddAll.IsHandleCreated = False Then
            Exit Sub
        End If
        Cfg_Palette_Import_PNG_Force_Add_Colors = CByte(CInt(ChkPalImportPNGForceAddAll.Checked) * -1)

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub LblRootFolder_MouseHover(sender As Object, e As EventArgs) Handles LblRootFolder.MouseHover

        MdlZTStudioUI.ShowToolTip(LblRootFolder, "This is the project folder. Common subfolders are 'animals', 'objects', ...")

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub LblColorPal8_MouseHover(sender As Object, e As EventArgs) Handles LblColorPal8.MouseHover

        MdlZTStudioUI.ShowToolTip(LblRootFolder, "The folder containing .pal-files which consist of 8 colors.")

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub LblColorPal16_MouseHover(sender As Object, e As EventArgs) Handles LblColorPal16.MouseHover

        MdlZTStudioUI.ShowToolTip(LblRootFolder, "The folder containing .pal-files which consist of 16 colors.")

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkRenderFrame_BGGraphic_MouseHover(sender As Object, e As EventArgs) Handles ChkRenderFrame_BGGraphic.MouseHover

        MdlZTStudioUI.ShowToolTip(ChkRenderFrame_BGGraphic, "" &
            "Allows to see two graphics combined. " & vbCrLf &
            "User can load the Orang Utan's swinging animation and use the Rope Swing toy as background." & vbCrLf &
            "Hint: using this technique, Blue Fang could use 2 color palettes = 2x 255 colors for this animation!"
            )
    End Sub



    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkConvert_DeleteOriginal_MouseHover(sender As Object, e As EventArgs) Handles ChkConvert_DeleteOriginal.MouseHover
        MdlZTStudioUI.ShowToolTip(ChkConvert_DeleteOriginal, "If enabled, the source files of any conversion will be deleted." & vbCrLf &
            "When converting from PNG to ZT1 Graphics, the PNG files will be deleted." & vbCrLf &
            "When converting from ZT1 Graphics to PNG, the ZT1 Graphics will be deleted.")
    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub LblConvert_fileNameDelimiter_MouseHover(sender As Object, e As EventArgs) Handles LblConvert_fileNameDelimiter.MouseHover
        MdlZTStudioUI.ShowToolTip(LblConvert_fileNameDelimiter, "The character used in filenames, between the name of the graphic And the frame." & vbCrLf &
            "For example, _ Is the delimiter in NE_0000.png ")
    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkConvert_SharedColorPalette_MouseHover(sender As Object, e As EventArgs) Handles ChkConvert_SharedColorPalette.MouseHover
        MdlZTStudioUI.ShowToolTip(ChkConvert_SharedColorPalette, "Rather than creating a separate color palette (.pal) for each view of each animation," & vbCrLf &
            "this feature checks if there's a shared palette (.pal, .gpl or .png - in this order) provided by the user." & vbCrLf &
            "Palette names should be the same as the folder they're in: for example 'm.pal' in 'animals/redpanda/m'" & vbCrLf &
            "This feature respects folder hierarchy: it uses the palette it can find at the closest level to the graphic. " & vbCrLf &
            "Warning: a color palette provides maximum 255 visible colors among all frames!")

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkConvert_Overwrite_MouseHover(sender As Object, e As EventArgs) Handles ChkConvert_Overwrite.MouseHover
        MdlZTStudioUI.ShowToolTip(ChkConvert_Overwrite, "Overwrites any existing files during conversions.")
    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkExportZT1_AddZTAFBytes_MouseHover(sender As Object, e As EventArgs) Handles ChkExportZT1_AddZTAFBytes.MouseHover
        MdlZTStudioUI.ShowToolTip(ChkExportZT1_AddZTAFBytes, "Always adds 'ZTAF' bytes at beginning of graphic file." & vbCrLf &
            "ZTAF-bytes are usually found at the beginning of graphic files which contain a background frame." & vbCrLf &
            "They don't seem to have any real function.")

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkExportZT1_Ani_MouseHover(sender As Object, e As EventArgs) Handles ChkExportZT1_Ani.MouseHover
        MdlZTStudioUI.ShowToolTip(ChkExportZT1_Ani, "Tries to generate a .ani file containing information related to offsets." & vbCrLf &
            "It will do so based on the most commonly found filenames and try to determine the object type." & vbCrLf &
            "It should work for most graphics, although there are exceptions (such as dustcloud)" & vbCrLf &
            "Finds 'N': assumes icon" & vbCrLf &
            "Finds 'NE', 'SE', 'SW', 'NW': assumes object (foliage, scenery, building, ...)" & vbCrLf &
            "Finds 'N', 'NE', 'E', 'SE', 'S': assumes moving creature (animal, guest, staff, ...)" & vbCrLf &
            "Finds '1' , '2', ... , '20': assumes path")

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkPNGTransparentBG_MouseHover(sender As Object, e As EventArgs) Handles ChkPNGTransparentBG.MouseHover
        MdlZTStudioUI.ShowToolTip(ChkPNGTransparentBG, "Rather than exporting PNGs with the chosen background color in ZT Studio," & vbCrLf &
            "PNG files will be created with a transparent (invisible) background.")
    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkPNGRenderBGFrame_MouseHover(sender As Object, e As EventArgs) Handles ChkPNGRenderBGFrame.MouseHover
        MdlZTStudioUI.ShowToolTip(ChkPNGRenderBGFrame,
            "Some images (such as the Restaurant) have one frame which serves as background in all other frames." & vbCrLf &
            "Toggling this option will either render this background in each frame and NOT export the background frame;" & vbCrLf &
            "or it will NOT render the background and export this background frame as 'extra'")

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param> 
    Private Sub LblHowToExportPNG_MouseHover(sender As Object, e As EventArgs) Handles LblHowToExportPNG.MouseHover
        MdlZTStudioUI.ShowToolTip(LblHowToExportPNG, "Each of these methods has benefits and downsides." & vbCrLf &
            "Keep canvas size (" & (Cfg_Grid_NumPixels * 2) & " x " & (Cfg_Grid_NumPixels * 2) & "): slower export; keeps offsets on re-import; easy to animate in other programs" & vbCrLf &
            "Crop to largest relevant width / height in this graphic: faster export; offsets lost on re-import; easy to animate in other programs" & vbCrLf &
            "Crop to relevant pixels of this frame: fastest export; offsets lost on re-import; more difficult to animate in other programs" & vbCrLf &
            "Crop around center (fast but experimental) : fast export; keeps offsets on re-import; easy to animate in other programs")



    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ChkPalImportPNGForceAddAll_MouseHover(sender As Object, e As EventArgs) Handles ChkPalImportPNGForceAddAll.MouseHover
        MdlZTStudioUI.ShowToolTip(ChkPalImportPNGForceAddAll, "Sometimes there may be duplicates in the color palette." & vbCrLf &
            "Usually only unique colors are added." & vbCrLf &
            "In some cases (such as recolors), it may be desired to forcefully add them to the color palette anyway.")

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub LblExportPNG_Index_MouseHover(sender As Object, e As EventArgs) Handles LblExportPNG_Index.MouseHover
        MdlZTStudioUI.ShowToolTip(LblExportPNG_Index, "This is meant for batch conversions. Some programs start numbering the first frame with 0, others with 1.")

    End Sub

    ''' <summary>
    ''' Show help on mousehover
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub LblDefaultAnimSpeed_MouseHover(sender As Object, e As EventArgs) Handles LblDefaultAnimSpeed.MouseHover
        MdlZTStudioUI.ShowToolTip(LblDefaultAnimSpeed, "The animation speed (in milliseconds) determines the interval before the next frame is shown.")

    End Sub

End Class