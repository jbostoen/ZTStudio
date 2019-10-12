Imports System.IO
Imports System.ComponentModel

''' <summary>
''' Class to handle the main graphic, which consist of one or multiple frames which share the same color palette.
''' </summary>

Public Class ClsGraphic

    Implements INotifyPropertyChanged

    ' This class handles ZT1 graphic files, e.g. "N". 
    ' All handling of palette files is done by a different class

    Private ClsGraphic_FileName As String = vbNullString ' File name of graphic
    Private ClsGraphic_Palette As New ClsPalette(Me) ' Main color palette

    Private ClsGraphic_AnimationSpeed As Integer = Cfg_frame_defaultAnimSpeed ' Speed in milliseconds for this animation

    Private ClsGraphic_HasBackgroundFrame As Byte = 0 ' Basic files, FATZ-files with byte 9 = 0: no extra background frame. Byte 9 = 1: graphic contain a background frame.

    Private ClsGraphic_Frames As New List(Of ClsFrame)
    Private ClsGraphic_LastUpdated As String = Now.ToString("yyyyMMddHHmmss") ' For caching purposes for larger frames.

    ''' <summary>
    ''' Filename of the ZT1 Graphic. Contrary to a regular filename, it has no file extension.
    ''' </summary>
    ''' <returns>String</returns>
    Public Property FileName As String
        Get
            Return ClsGraphic_FileName
        End Get
        Set(value As String)
            ClsGraphic_FileName = value.ToLower()
            NotifyPropertyChanged("FileName")
        End Set
    End Property

    ''' <summary>
    ''' The color palette used in this graphic and shared among its frames.
    ''' </summary>
    ''' <returns>ClsPalette - color palette</returns>
    Public Property ColorPalette As ClsPalette
        Get
            Return ClsGraphic_Palette
        End Get
        Set(value As ClsPalette)
            ClsGraphic_Palette = value
            NotifyPropertyChanged("ColorPalette")
        End Set
    End Property

    ''' <summary>
    ''' Array of frames (ClsFrame) in this graphic. Includes background frame (as last frame), if present.
    ''' </summary>
    ''' <returns>List(Of ClsFrame) - list of ZT1 frames</returns>
    Public Property Frames As List(Of ClsFrame)
        Get
            Return ClsGraphic_frames
        End Get
        Set(value As List(Of ClsFrame))
            ClsGraphic_frames = value
            NotifyPropertyChanged("Frames")
        End Set
    End Property

    ''' <summary>
    ''' Animation speed of the frame, in milliseconds. How much time passes before the next frame is shown?
    ''' </summary>
    ''' <returns>Integer - number of milli seconds</returns>
    Public Property AnimationSpeed As Integer
        Get
            Return ClsGraphic_animationSpeed
        End Get
        Set(value As Integer)
            ClsGraphic_animationSpeed = value
            NotifyPropertyChanged("AnimationSpeed")
        End Set
    End Property

    ''' <summary>
    ''' <para>Whether this graphic contains an extra background frame.</para>
    ''' <para>In some cases, such as the Restaurant, only changing pixels are in the regular set of frames. The last frame is always rendered as a background in this case.</para>
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Kept as a byte for calculations</remarks>
    Public Property HasBackgroundFrame As Byte
        Get
            Return ClsGraphic_HasBackgroundFrame
        End Get
        Set(value As Byte)
            ClsGraphic_HasBackgroundFrame = value
            NotifyPropertyChanged("ExtraFrame")
        End Set
    End Property

    ''' <summary>
    ''' Timestamp of last update.
    ''' </summary>
    ''' <returns></returns>
    Public Property LastUpdated As String
        Get
            Return ClsGraphic_lastUpdated
        End Get
        Set(value As String)
            ClsGraphic_lastUpdated = value
            NotifyPropertyChanged("LastUpdated")
        End Set
    End Property

    ''' <summary>
    ''' Reads the graphic (from a file)
    ''' </summary>
    ''' <param name="StrFileName">Source file name</param>
    Public Sub Read(Optional StrFileName As String = vbNullString)

        'On Error GoTo dBg

1:
        ' 20190815 Before this just set the filename; but not the (assumed) .pal file
        If StrFileName <> vbNullString Then
            Me.FileName = StrFileName
        End If

        Dim IntX As Integer = 0
        Dim IntCurByte As Integer = 0
        Dim IntTemplength As Integer = 0

        Dim IntNumberOfFrames As Integer = 0 ' Number of frames for this animation (at least 1)

        Dim StrOriginalColorPaletteFileName As String = Me.ColorPalette.FileName
        Dim StrNewColorPaletteFileName As String = Cfg_Path_Root & "/"

        MdlZTStudio.Trace(Me.gettype().FullName, "Read", "Reading graphic " & Me.FileName & " ...")


5:
        ' Read full file.
        Dim Bytes As Byte() = IO.File.ReadAllBytes(ClsGraphic_FileName)
        Dim LstBytesToHex As String() = Array.ConvertAll(Bytes, Function(b) b.ToString("X2"))
        Dim LstHexValues As New List(Of String)
        LstHexValues.AddRange(LstBytesToHex)

10:
        ' Here at least 3 variants of ZT1 Graphic format, which can be identified by the first 9 bytes of the file.
        If LstHexValues(0) = "46" And LstHexValues(1) = "41" And LstHexValues(2) = "54" And LstHexValues(3) = "5A" Then
            MdlZTStudio.Trace(Me.GetType().FullName, "Read", "FATZ-file (ZT Animation File)")
            ' 46 41 54 5A 00 | 00 00 00 01
            MdlZTStudio.Trace(Me.GetType().FullName, "Read", "Background frame: " & LstHexValues(8))
            Me.HasBackgroundFrame = LstHexValues(8)
            LstHexValues.Skip(9)
        Else
            MdlZTStudio.Trace(Me.GetType().FullName, "Read", "Basic graphic format")
        End If

15:
        ' === ANIMATION SPEED ===
        Me.AnimationSpeed = CInt("&H" & LstHexValues(3) & LstHexValues(2) & LstHexValues(1) & LstHexValues(0))
        MdlZTStudio.Trace(Me.GetType().FullName, "Read", "Animation speed: " & Me.AnimationSpeed)

20:
        ' === FILENAME ===
        ' The next bytes contain the length of the filename of the color palette
        IntTemplength = CInt("&H" & LstHexValues(7) & LstHexValues(6) & LstHexValues(5) & LstHexValues(4)) - 1

30:


        IntX = 0
        While IntX < IntTemplength
            StrNewColorPaletteFileName &= Chr(CInt("&H" & LstHexValues(8 + IntX)))
            IntX += 1
        End While



        MdlZTStudio.Trace(Me.GetType().FullName, "Read", "Color palette filename '" & Me.ColorPalette.FileName & "' (length: " & IntTemplength & ")")

35:
        ' Remove all processed bytes
        LstHexValues.Skip(8 + IntTemplength + 1)


40:
        ' === READ COLOR PALETTE ===

        ' Read the color palette
        ' In case of failure, such as missing palette file, ZTStudio will throw a fatal error.
        ' Only necessary if different name! (optimize performance)
        If StrNewColorPaletteFileName <> StrOriginalColorPaletteFileName Then

            MdlZTStudio.Trace(Me.GetType().FullName, "Read", "Reading color palette...")

            ' Read palette
            Me.ColorPalette.Colors.Clear()
            Me.ColorPalette.ReadPal(StrNewColorPaletteFileName)

        Else

            ' Graphic uses same palette; no need to reload
            MdlZTStudio.Trace(Me.GetType().FullName, "Read", "Color palette already loaded for previous graphic...")

        End If

50:
        ' === NUMBER OF FRAMES ===
        ' This is actually not used anymore, although it could be considered as a check at the very end to see if the expected amount of frames has been processed
        IntNumberOfFrames = CInt("&H" & LstHexValues(IntCurByte + 3) & LstHexValues(IntCurByte + 2) & LstHexValues(IntCurByte + 1) & LstHexValues(IntCurByte))
        MdlZTStudio.Trace(Me.GetType().FullName, "Read", "Number of frames: " & IntNumberOfFrames)

        ' Remove all processed bytes
        LstHexValues.Skip(4)


100:
        ' ==================================== FOR EACH FRAME... ===================================
        Dim IntCurrentFrame As Integer = 0
        Dim IntFrameBytes As Integer = 0
        Dim IntFrameBytesCurrent As Integer = 0

        Me.Frames = New List(Of ClsFrame) ' List of hex values will be stored here, for each frame

        While LstHexValues.Count > 0

101:
            '  The next 4 bytes determine the length of bytes to follow for one of the frames in this graphic
            IntFrameBytes = CInt("&H" & LstHexValues(IntCurByte + 3) & LstHexValues(IntCurByte + 2) & LstHexValues(IntCurByte + 1) & LstHexValues(IntCurByte))

            ' Remove all processed bytes.
            LstHexValues.Skip(4)

            MdlZTStudio.Trace(Me.GetType().FullName, "Read", "Number of bytes for frame " & Me.Frames.Count & ":  " & IntFrameBytes)
102:

            Dim ObjFrame As New ClsFrame(Me)
            Dim LstHexForOneFrame As New List(Of String)

            ' Build  hex string first.
            For IntFrameBytesCurrent = 0 To (IntFrameBytes - 1)
                LstHexForOneFrame.Add(LstHexValues(IntFrameBytesCurrent))
            Next

103:
            ' Set the hex values of this frame object
            ObjFrame.CoreImageHex = LstHexForOneFrame

104:
            ' Render the bitmap. This also sets offsets etc.
            ObjFrame.RenderCoreImageFromHex()

105:
            ' Add to the frame collection
            Me.Frames.Add(ObjFrame, False)

110:
            ' Remove all processed bytes.
            LstHexValues.Skip(IntFrameBytes)

155:
            IntCurrentFrame += 1

        End While

205:
        Me.LastUpdated = Now.ToString("yyyyMMddHHmmss")

        Exit Sub

dBg:
        ' Unexpected error
        MdlZTStudio.UnhandledError(Me.GetType().FullName, "Read", Information.Err)

    End Sub


    ''' <summary>
    ''' Writing/saving a ZT1 Graphic File
    ''' </summary>
    ''' <param name="StrFileName">Destination file name</param>
    ''' <param name="BlnOverwrite">Overwrite without warning</param>
    Public Sub Write(Optional StrFileName As String = vbNullString, Optional BlnOverwrite As Boolean = True)

        On Error GoTo dBug

        Dim LstHexGraphic As New List(Of String)

1:
        If StrFileName <> vbNullString Then
            Me.FileName = StrFileName
        End If

        MdlZTStudio.Trace(Me.GetType().FullName, "Write", "Outputting to " & Me.FileName)

2:
        ' 20190815: Set default .pal filename, even if it doesn't exist (for when 'write' occurs)
        If Me.ColorPalette.FileName = vbNullString Then
            MdlZTStudio.Trace(Me.GetType().FullName, "Write", "No filename for color palette specified. Defaulting to " & Me.FileName & ".pal")
            Me.ColorPalette.FileName = Me.FileName & ".pal"
        End If

5:
        If File.Exists(StrFileName) = True And BlnOverwrite = False Then

            Dim StrErrorMessage As String =
                "Error: could Not create ZT1 Graphic." & vbCrLf &
                "There is already a file at this location:  " & vbCrLf &
                "'" & StrFileName & "'"
            MdlZTStudio.HandledError(Me.GetType().FullName, "Write", StrerrorMessage, False, Nothing)
            Exit Sub

        End If

10:
        ' === Currently only output of basic files is supported. ===
        ' Simply output frames as hex etc
        ' set path to use '/' instead of '\'

        Dim StrPalName As String = Me.ColorPalette.FileName
        StrPalName = Strings.Replace(StrPalName, "\", "/")
        StrPalName = Strings.Replace(StrPalName, Strings.Replace(Cfg_Path_Root, "\", "/") & "/", "", , , CompareMethod.Text)

        With LstHexGraphic

            ' === Always ZTAF? Or background frame ===
            If (Me.HasBackgroundFrame = 1 Or Cfg_Export_ZT1_AlwaysAddZTAFBytes = 1) Then

                ' "FATZ" - reversed hex for Zoo  Tycoon Animation File.
                .Add("46", False)
                .Add("41", False)
                .Add("54", False)
                .Add("5A", False)

                .Add("00", False)
                .Add("00", False)
                .Add("00", False)
                .Add("00", False)

                ' If the file is marked FATZ, then there are two possibilities.
                ' Either there's an extra frame (background), e.g. for the restaurant; 
                ' or there simply isn't. This is reflected in the 9th byte
                If Me.HasBackgroundFrame = 1 Then
                    .Add("01", False)
                Else
                    .Add("00", False)
                End If
            End If

            ' === Animation speed ===
            .AddRange(Strings.Split((Me.AnimationSpeed).ToString("X8").ReverseHex(), " "), False)

            ' === Palette file name length ===
            .AddRange(Strings.Split((StrPalName.Length + 1).ToString("X8").ReverseHex(), " "), False)

            ' === Palette file name ===
            For Each StrChar As Char In StrPalName
                .Add(Convert.ToString(Convert.ToInt32(StrChar), 16), False)
            Next StrChar
            .Add("00", False) ' Add null character.

            ' === Number of frames ====
            ' Limit - todo: find out if the theoretical number of frames is 255 (FF - X2) or the number can be larger (other bytes?)
            .AddRange(Strings.Split((Me.Frames.Count - Me.HasBackgroundFrame).ToString("X8").ReverseHex(), " "), False)

            ' === Find out the total length. This could be a lot. Todo: determine limit. ===

            ' === Now, for each frame ===
            Dim LstHexSub As New List(Of String)
            Dim LstHexFrame As New List(Of String)

            For Each ObjFrame As ClsFrame In Me.Frames

                ' Get the amount of bytes for each frame
                ' But: the ZT1 Graphic format also expects to specify FIRST how many bytes there are for the frame

                ' 20190823: the comment below doesn't make sense, together with LastUpdated. CoreImageHex is a property.
                ' Just to make sure: force re-rendering
                ' ObjFrame.LastUpdated = vbNullString
                LstHexFrame = ObjFrame.CoreImageHex

                ' Specify number of bytes of this frame first.
                LstHexSub.AddRange(Strings.Split(LstHexFrame.Count.ToString("X8").ReverseHex(), " "), False)

                ' Add those frame bytes.
                LstHexSub.AddRange(LstHexFrame, False)

            Next

800:
            .AddRange(LstHexSub, False)

        End With

        MdlZTStudio.Trace(Me.GetType().FullName, "Write", "Processed all hex values, ready To write file")

1000:
        ' Working around a possible bug?
        ' 20190823 - which bug? Warning? Or nothing at all?
        File.Delete(Me.FileName)

        Dim ObjFileStream As New FileStream(Me.FileName, FileMode.CreateNew, FileAccess.Write)

1001:
        For Each StrHexValue As String In LstHexGraphic
1002:
            ObjFileStream.WriteByte(CByte("&H" & StrHexValue))
        Next

1003:
        ObjFileStream.Close()
        ObjFileStream.Dispose()

        ' Do not forget: color palette must also be created!
        ' This is only done if it has the same name (to avoid messing up shared palettes)
1100:
        If Me.ColorPalette.FileName = Me.FileName & ".pal" Then
            MdlZTStudio.Trace(Me.GetType().FullName, "Write", "Graphic uses its own color palette. Write.")
            Me.ColorPalette.WritePal(Me.ColorPalette.FileName, True)
        End If


1200:
        MdlZTStudio.Trace(Me.GetType().FullName, "Write", "Output complete")

        Exit Sub

dBug:
        MdlZTStudio.UnhandledError(Me.GetType().FullName, "Write", Information.Err)

    End Sub


    Sub RenderFrames()

        ' This will render all frames
        For Each ObjFrame As ClsFrame In Me.Frames
            ObjFrame.GetImage()
        Next

    End Sub

    ''' <summary>
    ''' Just an event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' Some updates to the graphic object should result in an update of displayed information.
    ''' </summary>
    ''' <param name="StrProperty"></param>
    Private Sub NotifyPropertyChanged(ByVal StrProperty As String)

        Dim LstProperties As New List(Of String) From {"CoreImageBitmap", "CoreImageHex", "FileName"}

        If LstProperties.Contains(StrProperty) = True Then
            ' no need to change LastUpdated here?
            Exit Sub
        End If

        ' This will trigger a refresh if it's the main graphic
        If BlnTaskRunning = False Then
            MdlZTStudioUI.UpdateFrameInfo("Property of graphic changed: " & StrProperty)
        End If

        'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub



    ''' <summary>
    ''' On initializing, set parent of color palette
    ''' </summary>
    Public Sub New(ObjPalette As ClsPalette)

        If IsNothing(ObjPalette) = False Then

            Me.ColorPalette = ObjPalette

        End If

        ' Make sure the color palette knows this graphic is it's parent
        ' (not remembered by the actual .pal file)
        Me.ColorPalette.Parent = Me


    End Sub

    ''' <summary>
    ''' Experimental method. Writes info about graphic and each frame to a .nfo file (same name as graphic).
    ''' To be used for re-importing (keeping correct offsets), perhaps discovering mystery bytes etc.
    ''' </summary>
    Public Sub WriteInfo()

        Dim StrDestinationFileName As String = Me.FileName & ".nfo"
        Dim IntFrameIndex As Integer = 0

        IniWrite(StrDestinationFileName, "Graphic", "animationSpeed", Me.AnimationSpeed)
        IniWrite(StrDestinationFileName, "Graphic", "frameCount", Me.Frames.Count)
        IniWrite(StrDestinationFileName, "Graphic", "palFile", Me.ColorPalette.FileName)
        IniWrite(StrDestinationFileName, "Graphic", "hasBackgroundFrame", Me.HasBackgroundFrame)

        For Each ObjFrame In Me.Frames
            ' todo: check if this works with identical frames? Still proper index?
            Dim StrSection As String = "Frame" & IntFrameIndex.ToString()

            IniWrite(StrDestinationFileName, StrSection, "offsetX", ObjFrame.OffsetX)
            IniWrite(StrDestinationFileName, StrSection, "offsetY", ObjFrame.OffsetY)
            IniWrite(StrDestinationFileName, StrSection, "height", ObjFrame.CoreImageBitmap.Height)
            IniWrite(StrDestinationFileName, StrSection, "width", ObjFrame.CoreImageBitmap.Width)
            IniWrite(StrDestinationFileName, StrSection, "numBytes", ObjFrame.CoreImageHex.Count)
            IniWrite(StrDestinationFileName, StrSection, "mysteryBytes", String.Join(" ", ObjFrame.MysteryHEX))

            IntFrameIndex += 1

        Next


    End Sub

End Class
