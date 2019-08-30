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
        Dim SFile As String = System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.cfg"

        If System.IO.File.Exists(SFile) = False Then

            Dim StrMessage As String = "ZT Studio is missing the settings.cfg file." & vbCrLf &
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
        Cfg_grid_BackGroundColor = System.Drawing.Color.FromArgb(CInt(IniRead(SFile, "preview", "bgColor", "")))
        Cfg_grid_ForeGroundColor = System.Drawing.Color.FromArgb(CInt(IniRead(SFile, "preview", "fgColor", "")))
        Cfg_grid_numPixels = CInt(IniRead(SFile, "preview", "numPixels", ""))
        Cfg_grid_zoom = CInt(IniRead(SFile, "preview", "zoom", ""))
        Cfg_grid_footPrintX = CByte(IniRead(SFile, "preview", "footPrintX", ""))
        Cfg_grid_footPrintY = CByte(IniRead(SFile, "preview", "footPrintY", ""))

30:
        ' Reads from ini and configures all.
        Cfg_path_Root = IniRead(SFile, "paths", "root", "")
        Cfg_path_recentPNG = IniRead(SFile, "paths", "recentPNG", "")
        Cfg_path_recentZT1 = IniRead(SFile, "paths", "recentZT1", "")
        Cfg_path_ColorPals8 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal8"
        Cfg_path_ColorPals16 = System.IO.Path.GetFullPath(Application.StartupPath) & "\pal16"

40:
        ' Export (PNG)
        Cfg_export_PNG_CanvasSize = CInt(IniRead(SFile, "exportOptions", "pngCrop", ""))
        Cfg_export_PNG_RenderBGFrame = CByte(IniRead(SFile, "exportOptions", "pngRenderExtraFrame", ""))
        Cfg_export_PNG_RenderBGZT1 = CByte(IniRead(SFile, "exportOptions", "pngRenderExtraGraphic", ""))
        Cfg_export_PNG_TransparentBG = CByte(IniRead(SFile, "exportOptions", "pngRenderTransparentBG", ""))

        ' Export (ZT1)
        Cfg_export_ZT1_Ani = CByte(IniRead(SFile, "exportOptions", "zt1Ani", "1"))
        Cfg_export_ZT1_AlwaysAddZTAFBytes = CByte(IniRead(SFile, "exportOptions", "zt1AlwaysAddZTAFBytes", ""))

50:
        ' Convert ( ZT1 <=> PNG, other way around )
        Cfg_convert_startIndex = CInt(IniRead(SFile, "conversionOptions", "pngFilesIndex", ""))
        Cfg_convert_deleteOriginal = CByte(IniRead(SFile, "conversionOptions", "deleteOriginal", ""))
        Cfg_convert_overwrite = CByte(IniRead(SFile, "conversionOptions", "overwrite", ""))
        Cfg_convert_sharedPalette = CByte(IniRead(SFile, "conversionOptions", "sharedPalette", ""))
        Cfg_convert_fileNameDelimiter = CStr(IniRead(SFile, "conversionOptions", "fileNameDelimiter", ""))

60:
        ' Frame editing
        Cfg_editor_rotFix_individualFrame = CByte(IniRead(SFile, "editing", "individualRotationFix", ""))
        Cfg_frame_defaultAnimSpeed = CInt(IniRead(SFile, "editing", "animationSpeed", ""))

70:
        ' Palette
        Cfg_palette_import_png_force_add_colors = CByte(IniRead(SFile, "palette", "importPNGForceAddColors", ""))


100:

        ' Now, if our path is no longer valid, pop up 'Settings'-window automatically
        If System.IO.Directory.Exists(Cfg_path_Root) = False Then


            ' But let's give some suggestions.
            Cfg_path_Root = System.IO.Path.GetFullPath(Application.StartupPath)

            ' Also give suggestions for color palettes.
            If System.IO.Directory.Exists(Cfg_path_ColorPals8) = False And System.IO.Directory.Exists(Application.StartupPath & "\pal8") = True Then
                Cfg_path_ColorPals8 = Cfg_path_Root & "\pal8"
            End If
            If System.IO.Directory.Exists(Cfg_path_ColorPals16) = False And System.IO.Directory.Exists(Application.StartupPath & "\pal16") = True Then
                Cfg_path_ColorPals8 = Cfg_path_Root & "\pal16"
            End If

            ' Now show the settings dialog.
            FrmSettings.ShowDialog()

        End If

200:

        ' No recent paths yet?
        If Cfg_path_recentPNG = "" Then
            Cfg_path_recentPNG = Cfg_path_Root
        End If
        If Cfg_path_recentZT1 = "" Then
            Cfg_path_recentZT1 = Cfg_path_Root
        End If

        ' Paths invalid?
        If System.IO.File.Exists(Cfg_path_recentPNG) = False Then
            Cfg_path_recentPNG = Cfg_path_Root
        End If
        If System.IO.File.Exists(Cfg_path_recentZT1) = False Then
            Cfg_path_recentZT1 = Cfg_path_Root
        End If



        ' Only now should the objects be created, if they don't exist yet
        ' 20190817: wait, there were no conditions here. So on saving settings, editorGraphic and editorBgGraphic were reset?
        If IsNothing(editorGraphic) = True Then
            editorGraphic = New ClsGraphic ' The ClsGraphic object
        End If
        If IsNothing(editorBgGraphic) = True Then
            editorBgGraphic = New ClsGraphic ' The background graphic, e.g. toy
        End If

        Exit Sub

dBug:
        If MsgBox("Error occurred when loading ZT Studio settings at line " & Erl() & vbCrLf & Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Failed to load settings") = vbOK Then End


    End Sub

    ''' <summary>
    ''' Saves configuration to .INI file
    ''' </summary>
    ''' <returns></returns>
    Public Function Write()

        ' This tasks writes all settings to the .ini-file.
        ' For an explanation of these parameters: check MdlSettings.vb

        Dim SFile As String = System.IO.Path.GetFullPath(Application.StartupPath) & "\settings.cfg"

        ' Preview
        IniWrite(sFile, "preview", "bgColor", Cfg_grid_BackGroundColor.ToArgb())
        IniWrite(sFile, "preview", "fgColor", Cfg_grid_ForeGroundColor.ToArgb())
        IniWrite(sFile, "preview", "numPixels", Cfg_grid_numPixels.ToString())
        IniWrite(sFile, "preview", "zoom", Cfg_grid_zoom.ToString())
        IniWrite(sFile, "preview", "footPrintX", Cfg_grid_footPrintX.ToString())
        IniWrite(sFile, "preview", "footPrintY", Cfg_grid_footPrintY.ToString())


        ' Reads from ini and configures all.
        IniWrite(sFile, "paths", "root", Cfg_path_Root)
        IniWrite(sFile, "paths", "recentPNG", Cfg_path_recentPNG)
        IniWrite(sFile, "paths", "recentZT1", Cfg_path_recentZT1)


        ' Export PNG (frames)
        IniWrite(sFile, "exportOptions", "pngCrop", Cfg_export_PNG_CanvasSize.ToString())
        IniWrite(sFile, "exportOptions", "pngRenderExtraFrame", Cfg_export_PNG_RenderBGFrame.ToString())
        IniWrite(sFile, "exportOptions", "pngRenderExtraGraphic", Cfg_export_PNG_RenderBGZT1.ToString())
        IniWrite(sFile, "exportOptions", "pngRenderTransparentBG", Cfg_export_PNG_TransparentBG.ToString())

        ' Export ZT1 (entire graphic)
        IniWrite(sFile, "exportOptions", "zt1Ani", Cfg_export_ZT1_Ani.ToString())
        IniWrite(sFile, "exportOptions", "zt1AlwaysAddZTAFBytes", Cfg_export_ZT1_AlwaysAddZTAFBytes.ToString())

        ' Convert options ( ZT1 <=> PNG )
        IniWrite(sFile, "conversionOptions", "pngFilesIndex", Cfg_convert_startIndex.ToString())
        IniWrite(sFile, "conversionOptions", "deleteOriginal", Cfg_convert_deleteOriginal.ToString())
        IniWrite(sFile, "conversionOptions", "overwrite", Cfg_convert_overwrite.ToString())
        IniWrite(sFile, "conversionOptions", "sharedPalette", Cfg_convert_sharedPalette.ToString())
        IniWrite(sFile, "conversionOptions", "fileNameDelimiter", Cfg_convert_fileNameDelimiter)

        ' Frame editing
        IniWrite(sFile, "editing", "individualRotationFix", Cfg_editor_rotFix_individualFrame.ToString())
        IniWrite(sFile, "editing", "animationSpeed", Cfg_frame_defaultAnimSpeed.ToString())

        ' Palette
        IniWrite(sFile, "palette", "importPNGForceAddColors", Cfg_palette_import_png_force_add_colors.ToString())


        Return 0

    End Function


End Module
