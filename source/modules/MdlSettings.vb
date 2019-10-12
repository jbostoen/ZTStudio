Option Explicit On
 
Imports System
Imports System.Reflection
Imports System.Windows.Forms
Module MdlSettings

    Private Declare Unicode Function WritePrivateProfileString Lib "kernel32" _
    Alias "WritePrivateProfileStringW" (ByVal lpApplicationName As String,
    ByVal lpKeyName As String, ByVal lpString As String,
    ByVal lpFileName As String) As Int32

    Private Declare Unicode Function GetPrivateProfileString Lib "kernel32" _
    Alias "GetPrivateProfileStringW" (ByVal lpApplicationName As String,
    ByVal lpKeyName As String, ByVal lpDefault As String,
    ByVal lpReturnedString As String, ByVal nSize As Int32,
    ByVal lpFileName As String) As Int32

    Public BMEmpty As ClsDirectBitmap ' Empty bitmap on initializing. PictureBox's image sometimes gets reset to this. Todo: review if actually needed...

    Public EditorGraphic As ClsGraphic             ' The ClsGraphic object we use.
    Public EditorBgGraphic As ClsGraphic           ' The background graphic, e.g. toy
    Public EditorFrame As ClsFrame                 ' The ClsFrame we are currently viewing/editing
    Public BlnTaskRunning As Boolean = False          ' Prevents certain UI updates if a task is running

    Public Cfg_Grid_BackGroundColor As Color = Color.White ' The default background color?
    Public Cfg_Grid_ForeGroundColor As Color = Color.Black ' The default foreground color for the grid lines?

    Public Cfg_Grid_NumPixels As Integer = 256 ' 256 - The maximum number of pixels
    Public Cfg_Grid_zoom As Integer = 1 ' Default zoom level

    Public Cfg_Path_Root As String = "C:\Program Files (x86)\Microsoft Games\Zoo Tycoon" ' Default location of our project folder. The user should select a <root>folder with contents similar to <root>/animals/ibex/m/
    Public Cfg_Path_ColorPals8 As String = "C:\Users\root\Documents\GitHub\ZTStudio\source\bin\Release\pal8" ' Location of color palettes (for color replacement) - 8 shades
    Public Cfg_Path_ColorPals16 As String = "C:\Users\root\Documents\GitHub\ZTStudio\source\bin\Release\pal16" ' Location of color palettes (for color replacement) - 16 shades

    ' Export
    Public Cfg_Export_PNG_RenderBGFrame As Byte = 1 ' If a background frame is present: should it be rendered in all PNG output files (or separately?)
    Public Cfg_Export_PNG_CanvasSize As Integer = 0 ' Should the PNG be the size (height/width) of the canvas, or cropped? 
    Public Cfg_Export_PNG_RenderBGZT1 As Byte = 0 ' If a background ZT1 Graphic was chosen, should it be rendered in the PNG output files?
    Public Cfg_Export_PNG_TransparentBG As Byte = 0 ' 0 = use ZT Studio background color; 1 = write transparent color


    ' Write ZT1
    Public Cfg_Export_ZT1_Ani As Byte = 1 ' Try to generate an .ani file
    Public Cfg_Export_ZT1_AlwaysAddZTAFBytes As Byte = 0 ' Should we add ZTAF-bytes even for a simple object? 


    ' Convert
    Public Cfg_Convert_DeleteOriginal As Byte = 1 ' Should the original image(s) be deleted upon conversion?
    Public Cfg_Convert_Overwrite As Byte = 1 ' Should we overwrite existing files?
    Public Cfg_Convert_StartIndex As Integer = 0 ' Does the index start at for example N_0000.png or N_0001.png ?
    Public Cfg_Convert_SharedPalette As Byte = 1 ' Do we (try to) share a color palette?
    Public Cfg_Convert_FileNameDelimiter As String = "_" ' The file name delimiter. eg _ in NE_0000.png

    ' Experimental
    Public Cfg_Convert_Write_Graphic_Data_To_Text_File = 0 ' Should a text file be created? Contains info on frames (offsets, mystery bytes, width, height...)

    ' Frame
    Public Cfg_Editor_RotFix_IndividualFrame As Byte = 0 ' determines whether we are fixing the position of an object in 1 frame or in the entire graphic
    Public Cfg_Frame_DefaultAnimSpeed As Integer = 125 ' Default animation speed


    ' Palette
    Public Cfg_Palette_Quantization As Byte = 0 ' Set to 1 to allow quantization
    Public Cfg_Palette_Import_PNG_Force_Add_Colors As Byte = 0 ' Set to 1 to force duplicate colors to be processed (recommended after recolors - some colors, especially when making things brighter or darker, may end up the same.)


    ' Grid
    Public Cfg_grid_footPrintX As Byte = 2 ' the X-footprint in Zoo Tycoon.
    Public Cfg_grid_footPrintY As Byte = 2 ' the Y-footprint in Zoo Tycoon.


    ' Recent files
    Public Cfg_Path_RecentZT1 As String = "" ' Most recent path to select a ZT1 Graphic (file!)
    Public Cfg_Path_RecentPNG As String = "" ' Most recent path to select a PNG graphic (file!)


    ' GitHub
    Public Cfg_GitHub_URL As String = "https://github.com/jbostoen/ZTStudio"

    ' Debugging
    Public Cfg_Trace As Integer = 1 ' Set to 1 for detailed logging


    ' Todo:
    ' - on load png frame (either UI or conversion): check max size. = 00 00 ? = 256 + 256 ?

    ' - move over pixel, get color. then find the color index in a palette.
    ' - render and save all frames in an animation. Suggest name pattern.
    ' - allow crop to image (to either relevant pixels in frame, or graphic's relevant pixels)



    ' - combine 2 different graphics?
    ' -- eg bounce (guest) + bounce (object)
    ' -- eg swing (guest) + swing (object)
    ' -- ringtoss (guest) + ringtoss (object)


    ' - detect missing color palettes (=crash)
    ' cachedfame <=> writeFrame()




    'Public Sub DoubleBuffered(ByVal Obj As Object, ByVal setting As Boolean)
    'Dim ObjType As Type = Obj.[GetType]()
    'Dim pi As PropertyInfo = ObjType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
    '    pi.SetValue(Obj, setting, Nothing)
    'End Sub



    Public Function IniWrite(ByVal iniFileName As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamVal As String) As Integer
        Dim Result As Integer = WritePrivateProfileString(Section, ParamName, ParamVal, iniFileName)
        Return 0
    End Function

    Public Function IniRead(ByVal IniFileName As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamDefault As String) As String
        Dim ParamVal As String = Space$(1024)
        Dim LenParamVal As Integer = GetPrivateProfileString(Section, ParamName, ParamDefault, ParamVal, Len(ParamVal), IniFileName)
        IniRead = Left$(ParamVal, LenParamVal)


    End Function


End Module
