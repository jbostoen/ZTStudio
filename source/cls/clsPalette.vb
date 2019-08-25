Imports System.IO

''' <summary>
''' ClsPalette is a class to process the ZT1 Color Palette (.pal file)
''' </summary>
Public Class ClsPalette


    Dim Pal_FileName As String = vbNullString ' The filename
    Dim Pal_Colors As New List(Of System.Drawing.Color) ' The actual list of colors. Should be 256 maximum, with the first one signifying the color that will be considered 'transparent'.
    Dim Pal_Parent As ClsGraphic = Nothing ' The graphic which is owner of this palette (not always set!)


    ''' <summary>
    ''' Creates a new instance of this class. Sets the parent graphic.
    ''' </summary>
    ''' <param name="ObjParent">ClsGraphic or Nothing. The parent of this color palette.</param>
    Public Sub New(ObjParent As ClsGraphic)
        Pal_Parent = ObjParent
    End Sub

    ''' <summary>
    ''' The parent of this graphic. A ClsGraphic or nothing.
    ''' </summary>
    ''' <returns>ClsGraphic or Nothing</returns>
    Public Property Parent As ClsGraphic
        Get
            Return Pal_Parent
        End Get
        Set(value As ClsGraphic)
            Pal_Parent = value
        End Set
    End Property

    ''' <summary>
    ''' Filename of this color palette
    ''' </summary>
    ''' <returns></returns>
    Public Property FileName As String
        Get
            Return Pal_FileName
        End Get
        Set(value As String)
            Pal_FileName = value
        End Set
    End Property

    ''' <summary>
    ''' The color palette. Maximum 256 colors in total, the first one being a color that will be rendered entirely transparent.
    ''' </summary>
    ''' <returns></returns>
    Public Property Colors As List(Of System.Drawing.Color)
        Get
            Return Pal_Colors
        End Get
        Set(value As List(Of System.Drawing.Color))
            Pal_Colors = value
        End Set
    End Property

    ''' <summary>
    ''' Reads a ZT1 Color palette (.pal file).
    ''' </summary>
    ''' <param name="StrFileName">Optional source filename. If not specified, defaults to filename property if already set</param>
    Sub ReadPal(Optional StrFileName As String = vbNullString)

        If StrFileName <> vbNullString Then
            Pal_FileName = StrFileName
        End If

        ' File does not exist.
        If File.Exists(Pal_FileName) = False Then
            ' Fatal error if used for a graphic. Any further processing of graphics could lead to issues.
            MdlZTStudio.ExpectedError(Me.GetType().FullName, "ReadPal", "Could not find '" & Pal_FileName & "'", (IsNothing(Me.Parent) = False))
        End If

        ' Read full file.
        Dim ArrBytes As Byte() = IO.File.ReadAllBytes(Pal_FileName)
        Dim ArrHex As String() = Array.ConvertAll(ArrBytes, Function(b) b.ToString("X2"))

        ' Now, the first bytes tell us how many colors there are.
        ' ZT1 Graphics only support a limited amount of colors (255?)
        ' So only the first 2 bytes (rather than the first 4) signal how many blocks of 4 bytes will follow.
        Dim Pal_NumberOfColors As Integer = CInt("&H" & Hex(1) & Hex(0)) '- 1 

        ' Jump to what matters.
        ArrHex = ArrHex.Skip(4).ToArray()
        Me.Colors = New List(Of System.Drawing.Color)

        ' Read number of colors. Only 3 bytes per color are relevant. So starting from byte 8, then 12, 16, 20...
        ' One byte can be ignored safely as it is nearly always (FF). Refers to opacity of a color, but unused in the game.
        While ArrHex.Length > 0

            ' Turn bytes/hex values into a color
            Dim ObjColor As Color = System.Drawing.Color.FromArgb(CInt("&H" & ArrHex(3)), CInt("&H" & ArrHex(0)), CInt("&H" & ArrHex(1)), CInt("&H" & ArrHex(2)))

            MdlZTStudio.Trace(Me.GetType().FullName, "ReadPal", "Color is " & ObjColor.ToArgb().ToString())

            Me.Colors.Add(objColor, False)

            ' Remove these bytes
            ArrHex = ArrHex.Skip(4).ToArray()

        End While

    End Sub

    ''' <summary>
    ''' Fills a datagridview with all colors. Changes the row heading to match the color. Implements some performance-boosts.
    ''' </summary>
    ''' <param name="ObjDataGridView">The datagridview where colors should be shown in</param>
    Sub FillPaletteGrid(ObjDataGridView As DataGridView)

        MdlZTStudio.Trace(Me.GetType().FullName, "FillPaletteGrid", "Preparing to add all colors to datagridview")

        ' This is done to greatly improve the speed of drawing.
        ' Something weird is going on though. Later in this code, all colors will be processed
        ObjDataGridView.GetType.InvokeMember("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.SetProperty, Nothing, ObjDataGridView, New Object() {True})

        ' Clear previous colors
        ' (this might be necessary if a palette was rendered previously in the same DataGridView)
        ObjDataGridView.Rows.Clear()

        ' In the past, the rows were created and added into an array. The benefit is that all rows could be added at once.
        ' However, it turned out the AddRange() method took 5-6 seconds; while adding it to the DataGridView right away takes only 2/3 secs
        Dim IntAutoNumber As Integer = 0 ' Using this for autonumbering. Alternative would have been .IndexOf , but this is quicker.

        ' Prevent visible updates in between.
        ObjDataGridView.Visible = False

        For Each ObjColor As System.Drawing.Color In Me.Colors

            Dim ObjRow As New DataGridViewRow
            With ObjRow

                ' The first color is actually transparent, but show it opaque in the DataGridView 
                .DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, ObjColor)
                .DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, ObjColor)

                .CreateCells(ObjDataGridView)
                .HeaderCell.Value = IntAutoNumber.ToString("0")
                .Cells(0).Value = IntAutoNumber.ToString("X2")

            End With

            ObjDataGridView.Rows.Add(ObjRow)
            IntAutoNumber += 1

        Next

        ' Make the DataGridView visible again, everything has bene added.
        ObjDataGridView.Visible = True
        MdlZTStudio.Trace(Me.GetType().FullName, "FillPaletteGrid", "Added all colors to datagridview")

    End Sub

    ''' <summary>
    ''' Returns the index of a color within this palette.
    ''' </summary>
    ''' <param name="ObjColor">The color of which the index in this palette should be returned</param>
    ''' <param name="BlnAddToPalette">Add the color to the palette if it's not present</param>
    ''' <returns></returns>
    Public Function GetColorIndex(ObjColor As System.Drawing.Color, Optional BlnAddToPalette As Boolean = True) As Integer

        If Me.Colors.Count = 0 Then
            ' This is a new color palette with no colors defined yet.
            ' Define the first color (transparent color) in this palette.
            Me.Colors.Add(System.Drawing.Color.FromArgb(0, Cfg_grid_BackGroundColor), False)
        End If

        ' Store so we don't need to call both .Contains() and .LastIndexOf()
        Dim IntColorIndex As Integer = Me.Colors.LastIndexOf(ObjColor)

        If IntColorIndex >= 0 Then

            ' Color has been found, return the index
            ' restrant.pal has a color listed twice.
            ' The graphic seems to rely on the last index.
            Return IntColorIndex

        ElseIf ObjColor.A = 0 Then

            ' This color palette uses a different transparent color.
            ' However, the .PNG contained a color with with alpha = 0 (transparent)
            Return 0

        ElseIf ObjColor = Cfg_grid_BackGroundColor Then

            ' The images being imported use a color which has been explicitly set as the background (or transparent) color in ZT Studio.
            Return 0

        ElseIf ObjColor.A = 255 And ObjColor.R = Me.Colors(0).R And ObjColor.G = Me.Colors(0).G And ObjColor.B = Me.Colors(0).B Then

            ' Hotfix for opacity issue
            ' The specified color is opaque, but RGB-values are identical to the transparent color within this palette
            Return 0

        Else

            ' The color was not found in this palette. Add it?

            If Me.Colors.Count < 256 And BlnAddToPalette = True Then

                ' Add the color: the maximum number of colors has not been reached and it's been explicitly allowed to do so.
                Me.Colors.Add(ObjColor, False)
                Return Me.Colors.Count - 1  ' Return last item index

            ElseIf Me.Colors.Count = 256 Then

                MdlZTStudio.Trace(Me.GetType().FullName, "GetColorIndex", "Reached maximum amount of colors.")

                ' No decision made yet
                If Cfg_palette_quantization = 0 Then

                    If MsgBox("The current palette (" & Me.FileName & ") already contains the maximum amount of colors (" & Me.Colors.Count & ")." & vbCrLf & vbCrLf &
                           "Color: " & ObjColor.ToString() & vbCrLf &
                           "Transparent color: " & Me.Colors(0).ToString & vbCrLf &
                           "Graphic: " & Me.Parent.FileName & vbCrLf & vbCrLf &
                           "ZT Studio can pick the closest matching color used so far." & vbCrLf &
                           "You can expect a degradation in graphic quality." & vbCrLf &
                           "Press [Yes] to ignore all warnings until you close ZT Studio." & vbCrLf &
                           "Press [No] to quit ZT Studio and fix things first.",
                           vbYesNo + vbCritical + vbApplicationModal, "Too many colors!") = vbYes Then
                        Cfg_palette_quantization = 1


                        MdlZTStudio.Trace(Me.GetType().FullName, "GetColorIndex", "User opted to use color quantization.")

                    Else

                        ' Quit ZT Studio, there will be too many errors popping up after this.
                        MdlZTStudio.Trace(Me.GetType().FullName, "GetColorIndex", "User opted to quit ZT Studio.")
                        End

                    End If
                End If

                ' Color quantization method by HENDRIX 
                ' Checking in HSV space to find the closest color in the full palette.
                Dim h1 As Single
                Dim S1 As Single
                Dim v1 As Single
                Dim h2 As Single
                Dim S2 As Single
                Dim v2 As Single
                Dim LstDistances As New List(Of Short)
                h1 = ObjColor.GetHue()
                S1 = ObjColor.GetSaturation()
                v1 = ObjColor.GetBrightness()

                For Each ObjColorInPalette As System.Drawing.Color In Me.Colors

                    h2 = h1 - ObjColorInPalette.GetHue()
                    S2 = S1 - ObjColorInPalette.GetSaturation()
                    v2 = v1 - ObjColorInPalette.GetBrightness()

                    ' In HSV it's possible to use simple euclidean distance. Results are reasonably good.
                    LstDistances.Add(Math.Sqrt(h2 * h2 + S2 * S2 + v2 * v2))

                Next

                ' See at which index in the existing color palette the least distance occured
                Return LstDistances.LastIndexOf(LstDistances.Min())

            Else
                MdlZTStudio.ExpectedError(Me.GetType().FullName, "GetColorIndex", "Unexpected case: not allowed to add colors, but only " & Me.Colors.Count & " colors in the palette?", True)

            End If

        End If

        Return -1 ' Will never be reached, just for the sake of not getting a warning about this function not returning anything in some paths.

    End Function


    ''' <summary>
    ''' Writes the color palette to the specified filename.
    ''' </summary>
    ''' <param name="StrFileName">Destination filename. Must be specified.</param>
    ''' <param name="BlnOverwrite">Overwrite the destination file.</param>
    ''' <remarks>
    ''' This sub always overwrites the destination file at this point.
    ''' </remarks>
    Public Sub WritePal(StrFileName As String, BlnOverwrite As Boolean)

        On Error GoTo dBug
1:
        ' This check is redundant as of now (24th of August 2019), but could be re-implemented in the future.
        If File.Exists(StrFileName) = True And BlnOverwrite = False Then
            MdlZTStudio.ExpectedError(Me.GetType().FullName, "WritePal", "Can not overwrite the color palette file '" & StrFileName & "'.", True)

        End If

        Me.FileName = StrFileName

        MdlZTStudio.Trace(Me.GetType().FullName, "WritePal", "Writing color palette of " & Me.Colors.Count & " colors to " & Me.FileName)

10:

        Dim LstOutputHexValues As New List(Of String) ' output hex
        Dim IntX As Integer ' used to loop through the colors

        ' Start with the number of colors to process
        With LstOutputHexValues
            .Add(Strings.Right(Me.Colors.Count.ToString("X4"), 2), False)
            .Add(Strings.Left(Me.Colors.Count.ToString("X4"), 2), False)
            .Add("00", False)
            .Add("00", False)

        End With

20:
        For IntX = 0 To (Me.Colors.Count - 1)

            With LstOutputHexValues
                .Add(Me.Colors(IntX).R.ToString("X2"), False)
                .Add(Me.Colors(IntX).G.ToString("X2"), False)
                .Add(Me.Colors(IntX).B.ToString("X2"), False)

                ' Only the first color is transparent (00), all others are opaque (FF)
                If IntX = 0 Then
                    .Add("00", False)
                Else
                    .Add("FF", False)
                End If

            End With

        Next

1000:
        Dim ObjFileStream As New FileStream(StrFileName, FileMode.OpenOrCreate, FileAccess.Write)
        For Each StrHexValue As String In LstOutputHexValues
            ObjFileStream.WriteByte(CByte("&H" & StrHexValue))
        Next

        ObjFileStream.Close()
        ObjFileStream.Dispose()

        MdlZTStudio.Trace(Me.GetType().FullName, "WritePal", "Finished writing .pal file")

        Exit Sub

dBug:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "WritePal", Information.Err)

    End Sub


    ' === extra functions ===

    ''' <summary>
    ''' Unused function. Should be moved to a module instead.
    ''' Was originally intended for a feature where ZT Studio would automatically improve graphics, also by combining color palettes (shared palette)
    ''' </summary>
    ''' <param name="LstColorPalettes">List of ClsPalette objects to combine</param>
    ''' <returns>ObjColorPalette - a new color palette (combination of all the source palettes)</returns>
    Function CombineColorPalettes(LstColorPalettes As List(Of ClsPalette)) As ClsPalette

        ' This function should allow to create/combine color palettes.
        ' There needs to be a check if there aren't too many colors! 

        Debug.Print("Combine color palettes.")

        Dim ObjCombinedPalette As New ClsPalette(Nothing)

        ' for each color palette: check if color exists in the new palette.
        For Each ObjColorPalette As ClsPalette In LstColorPalettes
            For Each ObjColor As System.Drawing.Color In ObjColorPalette.Colors

                ' Add color if it's new
                ObjCombinedPalette.GetColorIndex(ObjColor, True)

            Next
        Next

        Return ObjCombinedPalette

    End Function

    ''' <summary>
    ''' Exports color palette to a .PNG file. Dimensions are 16x16, which leaves room for 256 colors. 
    ''' <para>Warning: overwrites file without asking</para>
    ''' </summary>
    ''' <remarks>
    ''' Recoloring is popular in ZT1 circles to create "new" animals, but they often relied on two main methods:
    ''' * recoloring each frame individually, then re-import it into APE
    ''' * recolor one frame which contains most colors used in the animal, then use this to replace the .pal file
    ''' 
    ''' The idea is that the .PNG can easily be colored with any third party graphic image manipulation program (such as GIMP, but also Paint.NET, PhotoShop etc.)
    ''' The entire palette of an existing animal can be recolored at once. And it can be reimported in a later step.
    ''' </remarks>
    ''' <param name="StrExportFileName">Destination file name</param>
    Sub ExportToPNG(StrExportFileName As String)

        Dim Bmp As New Bitmap(16, 16)

        ' Perform Drawing here
        Dim IntX As Integer = 0 ' Will be used to process a bitmap from left to right
        Dim IntY As Integer = 0 ' Will be used to process a bitmap from top to bottom
        Dim IntColor As Integer

        MdlZTStudio.Trace(Me.GetType().FullName, "ExportToPNG", "Exporting color palette as .PNG to " & StrExportFileName)

        ' Todo: optimize SetPixel()
        ' For each row
        While IntY < 16

            ' for each col
            While IntX < 16 And IntColor < Me.Colors.Count

                Bmp.SetPixel(IntX, IntY, Me.Colors(IntColor))
                IntColor += 1
                IntX += 1
            End While

            ' reset, next line
            IntX = 0
            IntY += 1

        End While


        If File.Exists(StrExportFileName) = True Then
            MdlZTStudio.Trace(Me.GetType().FullName, "ExportToPNG", "Overwriting existing file!")
            File.Delete(StrExportFileName)
        End If

        Bmp.Save(StrExportFileName, System.Drawing.Imaging.ImageFormat.Png)

        Bmp.Dispose()

        MdlZTStudio.Trace(Me.GetType().FullName, "ExportToPNG", "Finished exporting color palette as .PNG")

    End Sub

    ''' <summary>
    ''' Imports color palette from a specially prepared .PNG file
    ''' </summary>
    ''' <remarks>See ExportToPNG()</remarks>
    ''' <param name="StrFileName">Source file name</param>
    Sub ImportFromPNG(StrFileName As String)

        On Error GoTo dBg

        Dim BmpSource As Bitmap = Image.FromFile(StrFileName)
        MdlZTStudio.Trace(Me.GetType().FullName, "ImportFromPNG", "Importing color palette from .PNG: " & StrFileName)
        MdlZTStudio.Trace(Me.GetType().FullName, "ImportFromPNG", "Forcefully add colors: " & Cfg_palette_import_png_force_add_colors)

        Dim IntX As Integer = 0 ' Used to process bitmap from left to right
        Dim IntY As Integer = 0 ' Used to process bitmap from top to bottom

        ' Todo: implement better method than GetPixel(), although performance boost will be minimal here.

        ' Clear current palette (please prevent redraws at this point)
        Me.Colors.Clear(False)

        ' Row by row
        While IntY < BmpSource.Height

            While IntX < BmpSource.Width

                ' Do not add duplicate colors, e.g. transparent stuff etc; UNLESS it's forced (= a user setting)
                ' Use case: After recoloring, some colors are suddenly identical (especially after they're made brighter or darker). 
                ' Keep in mind that the graphics still refer to the original indexes of their colors.
                ' Adding duplicate colors in that case causes least problems.

                ' Color is unknown or it's forcefully added
                If Me.Colors.IndexOf(BmpSource.GetPixel(IntX, IntY)) < 0 Or Cfg_palette_import_png_force_add_colors = 1 Then
                    Me.Colors.Add(BmpSource.GetPixel(IntX, IntY), False)
                End If

                IntX += 1

            End While

            ' Reset and start next line
            IntX = 0
            IntY += 1

        End While

200:

        ' There's actually two possibilities here.
        ' Either regenerate the list of hex values for each frame in the parent graphic, since colors might have switched places.
        ' Or regenerate the image, since it might just be a recolor (relying on this option for now) 
        If IsNothing(Me.Parent) = False Then
            For Each ObjFrame As ClsFrame In Me.Parent.Frames
                ObjFrame.CoreImageBitmap = Nothing
                ObjFrame.GetCoreImageBitmap()
            Next
        End If

        MdlZTStudio.Trace(Me.GetType().FullName, "ImportFromPNG", "Finished importing color palette from .PNG")

        Exit Sub

dBg:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "ImportFromGPL", Information.Err)

    End Sub

    ''' <summary>
    ''' Import colors from a GIMP Color Palette (.gpl)
    ''' </summary>
    ''' <remarks>
    ''' This is specifically developed due to the original author's preference for GIMP and since it's open source.
    ''' It is not intended to support other file formats at this point.
    ''' </remarks>
    ''' <param name="StrFileName">Source filename</param>
    Sub ImportFromGIMPPalette(StrFileName As String)

        On Error GoTo dBg

0:
        ' Typical file contents of a .GPL file: 

        ' GIMP Palette
        ' Name:   NameOfPaletteGoesHere
        ' Columns: 16
        ' #
        ' 0   1   0	#0
        ' <line for each color>
        ' 254 255 252	#254

10:

        Dim ObjReader As New System.IO.StreamReader(StrFileName)
        Dim StrTextLine As String = ""
        Dim IntLine As Integer = 1 ' Keep in mind, started line numbering, so starting from 1 !

        ' Clear current palette (please prevent redraws at this point)
        Me.Colors.Clear(False)

        ' Read file.
        Do While objReader.Peek() <> -1

11:
            StrTextLine = objReader.ReadLine()

            ' Remove double white spaces etc 
            StrTextLine = Strings.Trim(System.Text.RegularExpressions.Regex.Replace(StrTextLine, "\s+", " "))

            ' Ignore the first few lines of the GPL file (5 in that GIMP version) AND the transparent color
            If IntLine = 5 And StrTextLine <> "" Then
21:
                ' The GetColorIndex() method would add a transparent color if called.
                ' Transparent color must be added manually, without looking up.
                Me.Colors.Add(System.Drawing.Color.FromArgb(Split(StrTextLine, " ")(0), Split(StrTextLine, " ")(1), Split(StrTextLine, " ")(2)))

            ElseIf IntLine > 5 And strtextLine <> "" Then

22:
                ' Add to this color palette. Using GetColorIndex(color, True), it will prevent duplicates.
                Me.GetColorIndex(System.Drawing.Color.FromArgb(Split(StrTextLine, " ")(0), Split(StrTextLine, " ")(1), Split(StrTextLine, " ")(2)), True)

            End If

            ' Next
            IntLine += 1

        Loop

200:
        ' There's actually two possibilities here.
        ' Either regenerate the list of hex values for each frame in the parent graphic, since colors might have switched places.
        ' Or regenerate the image, since it might just be a recolor (relying on this option for now) 
        If IsNothing(Me.Parent) = False Then
            For Each ObjFrame As ClsFrame In Me.Parent.Frames
                ObjFrame.CoreImageBitmap = Nothing
                ObjFrame.GetCoreImageBitmap()
            Next
        End If

        MdlZTStudio.Trace(Me.GetType().FullName, "ImportFromPNG", "Finished importing color palette from .GPL")

        Exit Sub

dBg:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "ImportFromGPL", Information.Err)


    End Sub





End Class

