Imports System.IO
Imports System.ComponentModel



Public Class ClsGraphic

    Implements INotifyPropertyChanged

    ' This class handles ZT1 graphic files, e.g. "N". 
    ' All handling of palette files is done by a different class


    Private ClsGraphic_FileName As String = vbNullString                 ' File name of graphic
    Private ClsGraphic_Palette As New ClsPalette(Me)         ' Main color palette

    Private ClsGraphic_animationSpeed As Integer = 125     ' Speed in milliseconds for this animation

    Private ClsGraphic_Byte9 As Byte = 0                 ' Basic files, FATZ-files with byte 9 = 0: 0. Byte 9 = 1: 1. Extra byte.

    Private ClsGraphic_frames As New List(Of ClsFrame)
    Private ClsGraphic_lastUpdated As String = Now.ToString("yyyyMMddHHmmss")            ' For caching purposes for larger frames.


    Public Property FileName As String
        Get
            Return ClsGraphic_FileName
        End Get
        Set(value As String)
            ClsGraphic_FileName = value.ToLower()

            NotifyPropertyChanged("fileName")
        End Set
    End Property

    Public Property ColorPalette As ClsPalette
        Get
            Return ClsGraphic_Palette
        End Get
        Set(value As ClsPalette)
            ClsGraphic_Palette = value
            NotifyPropertyChanged("colorPalette")
        End Set
    End Property

    Public Property Frames As List(Of ClsFrame)
        Get
            Return ClsGraphic_frames
        End Get
        Set(value As List(Of ClsFrame))
            ClsGraphic_frames = value
            NotifyPropertyChanged("frames")
        End Set
    End Property

    Public Property AnimationSpeed As Integer
        Get
            Return ClsGraphic_animationSpeed
        End Get
        Set(value As Integer)
            ClsGraphic_animationSpeed = value
            NotifyPropertyChanged("animationSpeed")
        End Set
    End Property


    Public Property ExtraFrame As Byte
        Get
            Return ClsGraphic_Byte9
        End Get
        Set(value As Byte)
            ClsGraphic_Byte9 = value
            NotifyPropertyChanged("extraFrame")
        End Set
    End Property

    Public Property LastUpdated As String
        Get
            Return ClsGraphic_lastUpdated
        End Get
        Set(value As String)
            ClsGraphic_lastUpdated = value
            NotifyPropertyChanged("lastUpdated")
        End Set
    End Property


    Public Sub Read(Optional strFileName As String = vbNullString)

        'On Error GoTo dBg


1:
        ' 20190815 Before this just set the filename; but not the (assumed) .pal file
        If strFileName <> vbNullString Then
            Me.FileName = strFileName
        End If

        Dim X As Integer = 0
        Dim CurByte As Integer = 0
        Dim IntTemplength As Integer = 0

        Dim ClsGraphic_numFrames As Integer = 0          ' Number of frames for this animation (at least 1)


        Debug.Print("Graphic: reset defaults.")
        ' Read graphics file.
        ' Derive our main color palette.
        ' Get details about frames etc. 
        ' Resets all defaults.

        ' Reset palettes
        Me.ColorPalette = New ClsPalette(Me)

        ' Reset other info 
        ClsGraphic_animationSpeed = 0
        ClsGraphic_Byte9 = 0


5:

        ' === Read file contents ===

        Debug.Print("   : file: " & ClsGraphic_FileName)
        Debug.Print("   : read file contents...")

        ' Read full file.
        Dim Bytes As Byte() = IO.File.ReadAllBytes(ClsGraphic_FileName)
        Dim tHex As String() = Array.ConvertAll(Bytes, Function(b) b.ToString("X2"))
        Dim hexBytes As New List(Of String)
        hexBytes.AddRange(tHex)



        'Debug.Print(Strings.Join(hex, " "))

10:

        If hexBytes(0) = "46" And hexBytes(1) = "41" And hexBytes(2) = "54" And hexBytes(3) = "5A" Then
            Debug.Print("   : confirmed a FATZ-file (ZT Anim File?).")
            ' 46 41 54 5a 00 00 00 00 01
            ' what's the 01?
            ClsGraphic_Byte9 = hexBytes(8)
            hexBytes.Skip(9)
        Else
            Debug.Print("   : likely a basic ZT1 graphics file.")
        End If


        Debug.Print("   : process basic information...")

15:
        ' === ANIMATION SPEED ===
        ClsGraphic_animationSpeed = CInt("&H" & hexBytes(3) & hexBytes(2) & hexBytes(1) & hexBytes(0))
        Debug.Print("   : animation speed = " & ClsGraphic_animationSpeed)

20:
        ' === FILENAME ===
        ' How many bytes is the palette file name?
        Debug.Print("&H" & hexBytes(7) & hexBytes(6) & hexBytes(5) & hexBytes(4))
        IntTemplength = CInt("&H" & hexBytes(7) & hexBytes(6) & hexBytes(5) & hexBytes(4)) - 1

30:

        X = 0
        While X < IntTemplength
            Me.ColorPalette.FileName &= Chr(CInt("&H" & hexBytes(8 + X)))
            X += 1
        End While
        Debug.Print("   : palette name = '" & Me.ColorPalette.FileName & "' (length: " & IntTemplength & ")")


        ' remove all previous bytes.
        hexBytes.Skip(8 + IntTemplength + 1)


40:
        ' === READ COLOR PALETTE ===
        Debug.Print("Graphics: read palette: '" & Cfg_path_Root & "/" & Me.ColorPalette.FileName & "'...")
        If (Me.ColorPalette.ReadPal(Cfg_path_Root & "/" & Me.ColorPalette.FileName) = 0) Then Exit Sub


        'Debug.Print(Strings.Join(hex, " "))


50:
        ' === NUM OF FRAMES ===
        ' we might need more bytes for this.
        'Debug.Print("Graphics: Determining number of frames... ")
        ClsGraphic_numFrames = CInt("&H" & hexBytes(CurByte + 3) & hexBytes(CurByte + 2) & hexBytes(CurByte + 1) & hexBytes(CurByte))
        Debug.Print("Graphics: number of frames = " & ClsGraphic_numFrames)

        ' remove all these bytes.
        hexBytes.Skip(4)





100:
        ' ==================================== FOR EACH FRAME... ===================================
        Dim IntCurrentFrame As Integer = 0
        Dim IntFrameBytes As Integer = 0
        Dim IntFrameBytesCurrent As Integer = 0

        Dim ZtFrames As New List(Of ClsFrame)  ' Strings of HEX will be stored here, for each frame

        While hexBytes.Count > 0

101:

            ' Now, the next 4 bytes determine the length of bytes to follow for this particular animation
            IntFrameBytes = CInt("&H" & hexBytes(CurByte + 3) & hexBytes(CurByte + 2) & hexBytes(CurByte + 1) & hexBytes(CurByte))

            ' remove all these bytes.
            hexBytes.Skip(4)

            Debug.Print("   Bytes to follow for the entire frame: " & IntFrameBytes)
102:

            Dim ZtFrame As New ClsFrame(Me)
            Dim frameEntireHex As New List(Of String)

            ' Build our hex string first.
            For IntFrameBytesCurrent = 0 To (IntFrameBytes - 1)
                frameEntireHex.Add(hexBytes(IntFrameBytesCurrent))
            Next

103:



            ' Write our hex string to our frame
            ZtFrame.CoreImageHex = frameEntireHex

104:
            ' It's best to render the bitmap. This also sets offsets etc.
            ZtFrame.RenderCoreImageFromHex()


            ZtFrames.Add(ZtFrame, False)



110:
            ' Remove those frame bytes
            hexBytes.Skip(IntFrameBytes)

155:
            'Debug.Print("Graphics: total bytes of frame " & (ztFrames.Count).ToString("00") & "/" & (ClsGraphic_numFrames) & " = " & (Strings.Replace(ztFrame.hexString, " ", "").Length / 2) & vbTab & "Bytes left: " & hex.Length)


            IntCurrentFrame += 1

        End While


200:
        ' if ClsGraphic_Byte9 = 1, then animated, last frame = still
        ClsGraphic_frames = ZtFrames

201:
        ' pre-render last image.
        Me.Frames(Me.Frames.Count - 1).RenderCoreImageFromHex()



        ' Find and split animations.

205:
        Me.LastUpdated = Now.ToString("yyyyMMddHHmmss")

        Exit Sub

dBg:
        MsgBox("Error in class ClsGraphic, read(), at line " & Erl() & ": " & vbCrLf &
             Err.Number & " - " & Err.Description _
           & vbCrLf & "Line: " & Erl(), vbOKOnly + vbCritical, "Error")

    End Sub






    Public Function Write(Optional strFileName As String = vbNullString, Optional blnOverwrite As Boolean = True) As Integer



        On Error GoTo dBug
        'Debug.Print("... Start writing graphic at " & Now.ToString("HH:mm:ss"))

        Dim opHexGraphic As New List(Of String)

1:
        If strFileName <> vbNullString Then
            Me.FileName = strFileName
        End If

2:
        ' 20190815: Set default .pal filename, even if it doesn't exist (for when 'write' occurs)
        If Me.ColorPalette.FileName = vbNullString Then
            Me.ColorPalette.FileName = Me.FileName & ".pal"
        End If


5:

        If File.Exists(strFileName) = True And blnOverwrite = False Then
            MsgBox("Error: could not create a ZT1 Graphic." & vbCrLf &
                "There is already a file at this location: " & vbCrLf &
                "'" & strFileName & "'", vbOKOnly + vbCritical, "Failed to create ZT1 Graphic")

            Return 0
        End If




10:
        ' === Currently we only support basic files. ===
        ' We simply write out our frames etc.
        ' set path to use '/' instead of '\'
        Debug.Print("... Graphic: start writing.")

        Dim palName As String = Me.ColorPalette.FileName
        palName = Strings.Replace(palName, "\", "/")
        palName = Strings.Replace(palName, Strings.Replace(Cfg_path_Root, "\", "/") & "/", "", , , CompareMethod.Text)
        'Debug.Print(".... Palette: " & palName & vbCrLf & Strings.Replace(Cfg_path_Root, "\", "/"))


        With opHexGraphic

            ' === Always ZTAF? Or extra frame ===
            If (Me.ExtraFrame = 1 Or Cfg_export_ZT1_AlwaysAddZTAFBytes = 1) Then

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
                If Me.ExtraFrame = 1 Then
                    .Add("01", False)
                Else
                    .Add("00", False)
                End If
            End If

            ' === Animation speed ===
            .AddRange(Strings.Split((Me.AnimationSpeed).ToString("X8").ReverseHEX(), " "), False)


            ' === Palette file name length ===
            .AddRange(Strings.Split((palName.Length + 1).ToString("X8").ReverseHEX(), " "), False)

            ' === Palette file name ===
            Dim Sb As New System.Text.StringBuilder
            For Each c As Char In palName
                .Add(Convert.ToString(Convert.ToInt32(c), 16), False)
            Next c
            .Add("00", False)

            ' === Number of frames ====
            .AddRange(Strings.Split((Me.Frames.Count - Me.ExtraFrame).ToString("X8").ReverseHEX(), " "), False)

            ' === We need the total length. This could be a lot. I haven't calculated the actual limit. ===

            ' === Now, for each frame ===
            Dim hexSub As New List(Of String)
            Dim hexFrame As New List(Of String)

            'Debug.Print("... Start writing frames at " & Now.ToString("HH:mm:ss"))

            For Each ztFrame As ClsFrame In Me.Frames

                ' Here we get the HEX *inside* each frame.
                ' However, the ZT1 Graphic format also wants us to tell how many bytes there are in the frame.
                ' That should come first, so we're currently putting this into a variable
                ' so we can add the number of bytes first. (4 bytes).

                ' We also need to make sure that our render does not include the BG Frame!
                ztFrame.LastUpdated = vbNullString
                hexFrame = ztFrame.CoreImageHex


                ' Specify number of bytes of this frame.
                hexSub.AddRange(Strings.Split(hexFrame.Count.ToString("X8").ReverseHEX(), " "), False)

                ' Add frame bytes.
                hexSub.AddRange(hexFrame, False)


            Next

            'Debug.Print("... End writing frames at " & Now.ToString("HH:mm:ss"))

800:
            ' We have prepared everything, so it's easy to calculate how many bytes we'll have left.
            ' The difficulty: is it 4 bytes or 8? We'll support the latter.

            ' contrary to what we thought, it seems like this is for every single frame:
            '   .Add(hexSub.Count.ToString("X8").Substring(6, 2))
            '   .Add(hexSub.Count.ToString("X8").Substring(4, 2))
            '   .Add(hexSub.Count.ToString("X8").Substring(2, 2))
            '   .Add(hexSub.Count.ToString("X8").Substring(0, 2))
            .AddRange(hexSub, False)


        End With


        'Debug.Print(Strings.Join(opHex.ToArray(), " "))


1000:
        ' Working around a possible bug?
        File.Delete(Me.FileName)

        Dim fs As New FileStream(Me.FileName, FileMode.CreateNew, FileAccess.Write)

1001:

        For Each s As String In opHexGraphic
1002:
            fs.WriteByte(CByte("&H" & s))
        Next

1003:
        fs.Close()
        fs.Dispose()



        ' Let's not forget:
1100:
        If Me.ColorPalette.FileName = Me.FileName & ".pal" Then
            'Debug.Print("... Writing color palette (not shared).")
            Me.ColorPalette.WritePal(Me.ColorPalette.FileName, True)
        End If


1200:

        'Debug.Print("... End writing graphic at " & Now.ToString("HH:mm:ss"))
        Return 1

        Exit Function

dBug:
        MsgBox("Error while creating a ZT1 Graphic." & vbCrLf &
               strFileName & vbCrLf &
            "Line " & Erl() & vbCrLf &
            Err.Number & " - " & Err.Description,
            vbOKOnly + vbCritical, "Error while creating a ZT1 Graphic.")





    End Function


    Sub RenderFrames()

        ' This will render all frames
        For Each ztFrame As ClsFrame In Me.Frames
            ztFrame.GetImage()
        Next

    End Sub


    Function GetDefiningRectangle() As Rectangle

        Dim rect As New Rectangle

        Dim CoordA As New Point(Cfg_grid_numPixels * 2, Cfg_grid_numPixels * 2)
        Dim CoordB As New Point(-Cfg_grid_numPixels * 2, -Cfg_grid_numPixels * 2)

        Me.RenderFrames()


        For Each ztFrame As ClsFrame In Me.Frames

            ' One way to do it, is to gather the offsets and width/height
            If ztFrame.OffsetX < coordA.X Then coordA.X = ztFrame.OffsetX
            If ztFrame.OffsetY < coordA.Y Then coordA.Y = ztFrame.OffsetY

            ' Now, the width.
            If (ztFrame.OffsetX + ztFrame.CoreImageBitmap.Width) > coordB.X Then coordB.X = (ztFrame.OffsetX + ztFrame.CoreImageBitmap.Width)
            If (ztFrame.OffsetY + ztFrame.CoreImageBitmap.Height) > coordB.Y Then coordB.Y = (ztFrame.OffsetY + ztFrame.CoreImageBitmap.Height)

        Next

        rect.X = coordA.X
        rect.Y = coordA.Y

        rect.Width = coordB.X - coordA.Y
        rect.Height = coordB.Y - coordA.Y

        Return rect

    End Function



    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    Private Sub NotifyPropertyChanged(ByVal info As String)

        ' no need to change our lastUpdated ... ?

        If info = "coreImageBitmap" Then Exit Sub
        If info = "coreImageHex" Then Exit Sub
        If info = "fileName" Then Exit Sub ' no purpose (yet) to trigger a refresh of info


        ' This will trigger a refresh.
        MdlZTStudioUI.UpdateInfo("Property of graphic changed: " & info)

        'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub




    Public Sub New()

        Me.ColorPalette.Parent = Me


    End Sub
End Class
