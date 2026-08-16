Imports System.IO.File


''' <summary>
''' Contains methods related to ZT Studio's configuration
''' </summary>
Module MdlConfig


    ''' <summary>
    ''' Initializes the configuration settings, read from the .INI file
    ''' </summary>
    Sub Load()

        ' This tasks reads all settings from the .INI-file.
        ' For an explanation of these parameters: check mMlSettings.vb

        Try

        Dim StrSettingsFile As String = System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.cfg"

        If System.IO.File.Exists(StrSettingsFile) = False Then

            Dim StrErrorMessage As String = "" &
                "ZT Studio is missing the settings.cfg file." & vbCrLf &
                "It should be in the same folder as ZTStudio.exe" & vbCrLf & vbCrLf &
                "Get the file at:" & vbCrLf &
                Cfg_GitHub_URL

            MdlZTStudio.HandledError("MdlConfig", "Load", StrErrorMessage, True, Nothing, ZTStudioErrorCategory.Config)


        End If

        ' Preview
        ' Note: the string passed as the last argument to IniRead is the default used when the key
        ' is missing from settings.cfg (e.g. an older settings.cfg predating a newer setting). It
        ' must be a valid numeric string matching the corresponding Cfg_* default in MdlSettings.vb -
        ' CInt("")/CByte("") both throw, which previously aborted Load() partway through and left
        ' EditorGraphic/EditorBgGraphic (only constructed at the very end of this method) as Nothing.
        Cfg_Grid_BackGroundColor = System.Drawing.Color.FromArgb(CInt(IniRead(StrSettingsFile, "preview", "bgColor", Color.White.ToArgb().ToString())))
        Cfg_Grid_ForeGroundColor = System.Drawing.Color.FromArgb(CInt(IniRead(StrSettingsFile, "preview", "fgColor", Color.Black.ToArgb().ToString())))
        Cfg_Grid_NumPixels = CInt(IniRead(StrSettingsFile, "preview", "numPixels", "256"))
        Cfg_Grid_zoom = CInt(IniRead(StrSettingsFile, "preview", "zoom", "1"))
        Cfg_grid_footPrintX = CByte(IniRead(StrSettingsFile, "preview", "footPrintX", "2"))
        Cfg_grid_footPrintY = CByte(IniRead(StrSettingsFile, "preview", "footPrintY", "2"))

        ' Reads from ini and configures all.
        ' These are String settings: an empty default doesn't throw, and an empty/missing path is
        ' already handled gracefully further below (falls back to Cfg_Path_Root, or Application.StartupPath).
        Cfg_Path_Root = IniRead(StrSettingsFile, "paths", "root", "")
        Cfg_Path_RecentPNG = IniRead(StrSettingsFile, "paths", "recentPNG", "")
        Cfg_Path_RecentZT1 = IniRead(StrSettingsFile, "paths", "recentZT1", "")
        Cfg_Path_ColorPals8 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal8"
        Cfg_Path_ColorPals16 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal16"

        ' Export (PNG)
        Cfg_Export_PNG_CanvasSize = CInt(IniRead(StrSettingsFile, "exportOptions", "pngCrop", "0"))
        Cfg_Export_PNG_RenderBGZT1 = CByte(IniRead(StrSettingsFile, "exportOptions", "pngRenderExtraGraphic", "0"))
        Cfg_Export_PNG_RenderBGFrame = CByte(IniRead(StrSettingsFile, "exportOptions", "pngRenderExtraFrame", "1"))
        Cfg_Export_PNG_TransparentBG = CByte(IniRead(StrSettingsFile, "exportOptions", "pngRenderTransparentBG", "0"))

        ' Export (ZT1)
        Cfg_Export_ZT1_Ani = CByte(IniRead(StrSettingsFile, "exportOptions", "zt1Ani", "1"))
        Cfg_Export_ZT1_AlwaysAddZTAFBytes = CByte(IniRead(StrSettingsFile, "exportOptions", "zt1AlwaysAddZTAFBytes", "0"))

        ' Convert ( ZT1 <=> PNG, other way around )
        Cfg_Convert_StartIndex = CInt(IniRead(StrSettingsFile, "conversionOptions", "pngFilesIndex", "0"))
        Cfg_Convert_DeleteOriginal = CByte(IniRead(StrSettingsFile, "conversionOptions", "deleteOriginal", "1"))
        Cfg_Convert_Overwrite = CByte(IniRead(StrSettingsFile, "conversionOptions", "overwrite", "1"))
        Cfg_Convert_SharedPalette = CByte(IniRead(StrSettingsFile, "conversionOptions", "sharedPalette", "1"))
        Cfg_Convert_FileNameDelimiter = CStr(IniRead(StrSettingsFile, "conversionOptions", "fileNameDelimiter", "_"))

        ' Frame editing
        Cfg_Editor_RotFix_IndividualFrame = CByte(IniRead(StrSettingsFile, "editing", "individualRotationFix", "0"))
        Cfg_Frame_DefaultAnimSpeed = CInt(IniRead(StrSettingsFile, "editing", "animationSpeed", "125"))

        ' Palette
        Cfg_Palette_Import_PNG_Force_Add_Colors = CByte(IniRead(StrSettingsFile, "palette", "importPNGForceAddColors", "0"))

        ' Now, if our path is no longer valid, pop up 'Settings'-window automatically
        If System.IO.Directory.Exists(Cfg_Path_Root) = False Then

            ' But let's give some suggestions.
            Cfg_Path_Root = System.IO.Path.GetFullPath(Application.StartupPath)

            ' Also give suggestions for color palettes.
            If System.IO.Directory.Exists(Cfg_Path_ColorPals8) = False And System.IO.Directory.Exists(Application.StartupPath & "\pal8") = True Then
                Cfg_Path_ColorPals8 = Cfg_Path_Root & "\pal8"
            End If
            If System.IO.Directory.Exists(Cfg_Path_ColorPals16) = False And System.IO.Directory.Exists(Application.StartupPath & "\pal16") = True Then
                Cfg_Path_ColorPals8 = Cfg_Path_Root & "\pal16"
            End If

            ' Now show the settings dialog.
            FrmSettings.ShowDialog()

        End If

        ' No recent paths yet?
        If Cfg_Path_RecentPNG = "" Then
            Cfg_Path_RecentPNG = Cfg_Path_Root
        End If
        If Cfg_Path_RecentZT1 = "" Then
            Cfg_Path_RecentZT1 = Cfg_Path_Root
        End If

        ' Paths invalid?
        If System.IO.File.Exists(Cfg_Path_RecentPNG) = False Then
            Cfg_Path_RecentPNG = Cfg_Path_Root
        End If
        If System.IO.File.Exists(Cfg_Path_RecentZT1) = False Then
            Cfg_Path_RecentZT1 = Cfg_Path_Root
        End If

        ' Only now should the objects be created, if they don't exist yet
        ' 20190817: wait, there were no conditions here. So on saving settings, editorGraphic and editorBgGraphic were reset?
        If IsNothing(EditorGraphic) = True Then
            EditorGraphic = New ClsGraphic(Nothing) ' The ClsGraphic object
        End If
        If IsNothing(EditorBgGraphic) = True Then
            EditorBgGraphic = New ClsGraphic(Nothing) ' The background graphic, e.g. toy
        End If

        Catch ex As Exception
            MdlZTStudio.HandledError("MdlConfig", "Load", "Error while processing ZT Studio Settings", True, ex, ZTStudioErrorCategory.Config)
        End Try

    End Sub

    ''' <summary>
    ''' Saves configuration to .INI file
    ''' </summary>
    Public Sub Write()

        ' This tasks writes all settings to the .ini-file.
        ' For an explanation of these parameters: check MdlSettings.vb

        Try

            Dim StrSettingsFile As String = System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.cfg"

            ' Preview
            IniWrite(StrSettingsFile, "preview", "bgColor", Cfg_Grid_BackGroundColor.ToArgb())
            IniWrite(StrSettingsFile, "preview", "fgColor", Cfg_Grid_ForeGroundColor.ToArgb())
            IniWrite(StrSettingsFile, "preview", "numPixels", Cfg_Grid_NumPixels.ToString())
            IniWrite(StrSettingsFile, "preview", "zoom", Cfg_Grid_zoom.ToString())
            IniWrite(StrSettingsFile, "preview", "footPrintX", Cfg_grid_footPrintX.ToString())
            IniWrite(StrSettingsFile, "preview", "footPrintY", Cfg_grid_footPrintY.ToString())


            ' Reads from ini and configures all.
            IniWrite(StrSettingsFile, "paths", "root", Cfg_Path_Root)
            IniWrite(StrSettingsFile, "paths", "recentPNG", Cfg_Path_RecentPNG)
            IniWrite(StrSettingsFile, "paths", "recentZT1", Cfg_Path_RecentZT1)


            ' Export PNG (frames)
            IniWrite(StrSettingsFile, "exportOptions", "pngCrop", Cfg_Export_PNG_CanvasSize.ToString())
            IniWrite(StrSettingsFile, "exportOptions", "pngRenderExtraFrame", Cfg_Export_PNG_RenderBGFrame.ToString())
            IniWrite(StrSettingsFile, "exportOptions", "pngRenderExtraGraphic", Cfg_Export_PNG_RenderBGZT1.ToString())
            IniWrite(StrSettingsFile, "exportOptions", "pngRenderTransparentBG", Cfg_Export_PNG_TransparentBG.ToString())

            ' Export ZT1 (entire graphic)
            IniWrite(StrSettingsFile, "exportOptions", "zt1Ani", Cfg_Export_ZT1_Ani.ToString())
            IniWrite(StrSettingsFile, "exportOptions", "zt1AlwaysAddZTAFBytes", Cfg_Export_ZT1_AlwaysAddZTAFBytes.ToString())

            ' Convert options ( ZT1 <=> PNG )
            IniWrite(StrSettingsFile, "conversionOptions", "pngFilesIndex", Cfg_Convert_StartIndex.ToString())
            IniWrite(StrSettingsFile, "conversionOptions", "deleteOriginal", Cfg_Convert_DeleteOriginal.ToString())
            IniWrite(StrSettingsFile, "conversionOptions", "overwrite", Cfg_Convert_Overwrite.ToString())
            IniWrite(StrSettingsFile, "conversionOptions", "sharedPalette", Cfg_Convert_SharedPalette.ToString())
            IniWrite(StrSettingsFile, "conversionOptions", "fileNameDelimiter", Cfg_Convert_FileNameDelimiter)

            ' Frame editing
            IniWrite(StrSettingsFile, "editing", "individualRotationFix", Cfg_Editor_RotFix_IndividualFrame.ToString())
            IniWrite(StrSettingsFile, "editing", "animationSpeed", Cfg_Frame_DefaultAnimSpeed.ToString())

            ' Palette
            IniWrite(StrSettingsFile, "palette", "importPNGForceAddColors", Cfg_Palette_Import_PNG_Force_Add_Colors.ToString())

        Catch ex As Exception
            ' Load() (above) already guards against a failure while reading settings; Write() lacked the
            ' same protection, so e.g. a locked or read-only settings.cfg would previously crash uncaught.
            MdlZTStudio.HandledError("MdlConfig", "Write", "Error while writing ZT Studio Settings", False, ex, ZTStudioErrorCategory.Config)
        End Try

    End Sub


End Module
