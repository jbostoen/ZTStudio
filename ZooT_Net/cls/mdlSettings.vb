
Imports System
Imports System.Reflection
Imports System.Windows.Forms
Module mdlSettings

    Private Declare Unicode Function WritePrivateProfileString Lib "kernel32" _
    Alias "WritePrivateProfileStringW" (ByVal lpApplicationName As String, _
    ByVal lpKeyName As String, ByVal lpString As String, _
    ByVal lpFileName As String) As Int32

    Private Declare Unicode Function GetPrivateProfileString Lib "kernel32" _
    Alias "GetPrivateProfileStringW" (ByVal lpApplicationName As String, _
    ByVal lpKeyName As String, ByVal lpDefault As String, _
    ByVal lpReturnedString As String, ByVal nSize As Int32, _
    ByVal lpFileName As String) As Int32



    Public editorGraphic As New clsGraphic2         ' The clsGraphic2 object we use.
    Public editorBgGraphic As New clsGraphic2       ' The background graphic, e.g. toy
    Public editorFrame As clsFrame                  ' The clsFrame we are currently viewing/editing

    Public bm As Bitmap


    Public cfg_grid_BackGroundColor As Color = Color.White
    Public cfg_grid_ForeGroundColor As Color = Color.Black

    Public cfg_grid_numPixels As Integer = 256 ' 256
    Public cfg_grid_zoom As Integer = 1

    Public cfg_path_Root As String = "C:\Program Files (x86)\Microsoft Games\Zoo Tycoon"
    Public cfg_path_ColorPals8 As String = "X:\Projecten\Animal Antics\Tools\Zoot_Net\res\pal8"
    Public cfg_path_ColorPals16 As String = "X:\Projecten\Animal Antics\Tools\Zoot_Net\res\pal16"

    ' Export
    Public cfg_export_PNG_RenderBGFrame As Byte = 1
    Public cfg_export_PNG_CanvasSize As Integer = 0
    Public cfg_export_PNG_RenderBGZT1 As Byte = 0

    ' Write ZT1
    Public cfg_export_ZT1_Ani As Byte = 1
    Public cfg_export_ZT1_AlwaysAddZTAFBytes As Byte = 0


    ' Convert
    Public cfg_convert_deleteOriginal As Byte = 1
    Public cfg_convert_overwrite As Byte = 1
    Public cfg_convert_startIndex As Integer = 0

    ' Frame
    Public cfg_editor_rotFix_individualFrame As Byte = 0


    Public cfg_grid_footPrintX As Byte = 2
    Public cfg_grid_footPrintY As Byte = 2


    ' Recent files
    Public cfg_path_recentZT1 As String = ""
    Public cfg_path_recentPNG As String = ""


    ' Todo:
    ' - on load png frame (either UI or conversion): check max size. = 00 00 ? = 256 + 256 ?

    ' - move over pixel, get color. then find the color index in a palette.
    ' - render and save all frames in an animation. Suggest name pattern.
    ' - allow crop to image (to either relevant pixels in frame, or graphic's relevant pixels)


    ' - search and replace 8 characters and replace them with 8 in HEX
    ' (although this feature should not be necessary since we can properly create graphics of our own)

    ' - combine 2 different graphics?
    ' -- eg bounce (guest) + bounce (object)
    ' -- eg swing (guest) + swing (object)
    ' -- ringtoss (guest) + ringtoss (object)


    ' - detect missing color palettes (=crash)
    ' cachedfame <=> writeFrame()




    ' wanted commands: (case insensitive)
    ' crop, clean up will take defaults from settings
    ' /action:convert 
    '   /file:<name of ZT1 graphic> 
    '   /to:ZT1|png 
    '   /crop:0|1|2 es|no
    '   /cleanup:yes|no 
    '   /renderBGFrame:y 
    '   /renderBGFile:<filename>
    '   /startIndex

    ' /action:convert
    '   /folder:<name of folder>
    '   - settings above  -

    Public Sub DoubleBuffered(ByVal dgv As DataGridView, ByVal setting As Boolean)
        Dim dgvType As Type = dgv.[GetType]()
        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgv, setting, Nothing)
    End Sub


    Public Sub iniWrite(ByVal iniFileName As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamVal As String)
        Dim Result As Integer = WritePrivateProfileString(Section, ParamName, ParamVal, iniFileName)
    End Sub

    Public Function iniRead(ByVal IniFileName As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamDefault As String) As String
        Dim ParamVal As String = Space$(1024)
        Dim LenParamVal As Long = GetPrivateProfileString(Section, ParamName, ParamDefault, ParamVal, Len(ParamVal), IniFileName)
        iniRead = Left$(ParamVal, LenParamVal)
    End Function


End Module
