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

        On Error GoTo dBug


10:
        Dim StrSettingsFile As String = System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.cfg"

        If System.IO.File.Exists(StrSettingsFile) = False Then

            Dim StrMessage As String = "" &
                "ZT Studio is missing the settings.cfg file." & vbCrLf &
                "It should be in the same folder as ZTStudio.exe" & vbCrLf & vbCrLf &
                "Get the file at:" & vbCrLf &
                Cfg_GitHub_URL

            If MsgBox(StrMessage, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal) = MsgBoxResult.Ok Then
                End
            End If

        End If
        'On Error Resume Next


20:
        ' Preview
        Cfg_Grid_BackGroundColor = System.Drawing.Color.FromArgb(CInt(IniRead(StrSettingsFile, "preview", "bgColor", "")))
        Cfg_Grid_ForeGroundColor = System.Drawing.Color.FromArgb(CInt(IniRead(StrSettingsFile, "preview", "fgColor", "")))
        Cfg_Grid_NumPixels = CInt(IniRead(StrSettingsFile, "preview", "numPixels", ""))
        Cfg_Grid_zoom = CInt(IniRead(StrSettingsFile, "preview", "zoom", ""))
        Cfg_grid_footPrintX = CByte(IniRead(StrSettingsFile, "preview", "footPrintX", ""))
        Cfg_grid_footPrintY = CByte(IniRead(StrSettingsFile, "preview", "footPrintY", ""))

30:
        ' Reads from ini and configures all.
        Cfg_Path_Root = IniRead(StrSettingsFile, "paths", "root", "")
        Cfg_Path_RecentPNG = IniRead(StrSettingsFile, "paths", "recentPNG", "")
        Cfg_Path_RecentZT1 = IniRead(StrSettingsFile, "paths", "recentZT1", "")
        Cfg_Path_ColorPals8 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal8"
        Cfg_Path_ColorPals16 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal16"

40:
        ' Export (PNG)
        Cfg_Export_PNG_CanvasSize = CInt(IniRead(StrSettingsFile, "exportOptions", "pngCrop", ""))
        Cfg_Export_PNG_RenderBGZT1 = CByte(IniRead(StrSettingsFile, "exportOptions", "pngRenderExtraGraphic", ""))
        Cfg_Export_PNG_RenderBGFrame = CByte(IniRead(StrSettingsFile, "exportOptions", "pngRenderExtraFrame", ""))
        Cfg_Export_PNG_TransparentBG = CByte(IniRead(StrSettingsFile, "exportOptions", "pngRenderTransparentBG", ""))

        ' Export (ZT1)
        Cfg_Export_ZT1_Ani = CByte(IniRead(StrSettingsFile, "exportOptions", "zt1Ani", "1"))
        Cfg_Export_ZT1_AlwaysAddZTAFBytes = CByte(IniRead(StrSettingsFile, "exportOptions", "zt1AlwaysAddZTAFBytes", ""))

50:
        ' Convert ( ZT1 <=> PNG, other way around )
        Cfg_Convert_StartIndex = CInt(IniRead(StrSettingsFile, "conversionOptions", "pngFilesIndex", ""))
        Cfg_Convert_DeleteOriginal = CByte(IniRead(StrSettingsFile, "conversionOptions", "deleteOriginal", ""))
        Cfg_Convert_Overwrite = CByte(IniRead(StrSettingsFile, "conversionOptions", "overwrite", ""))
        Cfg_Convert_SharedPalette = CByte(IniRead(StrSettingsFile, "conversionOptions", "sharedPalette", ""))
        Cfg_Convert_FileNameDelimiter = CStr(IniRead(StrSettingsFile, "conversionOptions", "fileNameDelimiter", ""))

60:
        ' Frame editing
        Cfg_Editor_RotFix_IndividualFrame = CByte(IniRead(StrSettingsFile, "editing", "individualRotationFix", ""))
        Cfg_Frame_DefaultAnimSpeed = CInt(IniRead(StrSettingsFile, "editing", "animationSpeed", ""))

70:
        ' Palette
        Cfg_Palette_Import_PNG_Force_Add_Colors = CByte(IniRead(StrSettingsFile, "palette", "importPNGForceAddColors", ""))


100:

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

200:

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

205:


        ' Only now should the objects be created, if they don't exist yet
        ' 20190817: wait, there were no conditions here. So on saving settings, editorGraphic and editorBgGraphic were reset?
        If IsNothing(EditorGraphic) = True Then
            EditorGraphic = New ClsGraphic(Nothing) ' The ClsGraphic object
        End If
        If IsNothing(EditorBgGraphic) = True Then
            EditorBgGraphic = New ClsGraphic(Nothing) ' The background graphic, e.g. toy
        End If

        Exit Sub

dBug:
        MdlZTStudio.HandledError("MdlConfig", "Load", "Error while processing ZT Studio Settings", True, Information.Err)


    End Sub

    ''' <summary>
    ''' Saves configuration to .INI file
    ''' </summary>
    Public Sub Write()

        ' This tasks writes all settings to the .ini-file.
        ' For an explanation of these parameters: check MdlSettings.vb

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



    End Sub


End Module
