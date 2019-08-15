Imports System.IO
Imports System.ComponentModel



Public Class ClsGraphic2

    Implements INotifyPropertyChanged

    ' This class handles ZT1 graphic files, e.g. "N". 
    ' All handling of palette files is done by a different class


    Private clsGraphic_FileName As String = vbNullString                 ' File name of graphic
    Private clsGraphic_Palette As New clsPalette(Me)         ' Main color palette

    Private clsGraphic_animationSpeed As Integer = 125     ' Speed in milliseconds for this animation


    ' Private clsGraphic_frames() As String
    ' Strings containing HEX info about each frame.
    ' Each HEX string contains info about height, width, X/Y offsets, 
    ' and obviously how the image is rendered.


    Private clsGraphic_Byte9 As Byte = 0                 ' Basic files, FATZ-files with byte 9 = 0: 0. Byte 9 = 1: 1. Extra byte.

    Private clsGraphic_frames As New List(Of ClsFrame2)
    Private clsGraphic_lastUpdated As String = Now.ToString("yyyyMMddHHmmss")            ' For caching purposes for larger frames.


    Public Property FileName As String
        Get
            Return clsGraphic_FileName
        End Get
        Set(value As String)
            clsGraphic_FileName = value.ToLower()

            NotifyPropertyChanged("fileName")
        End Set
    End Property

    Public Property ColorPalette As clsPalette
        Get
            Return clsGraphic_Palette
        End Get
        Set(value As clsPalette)
            clsGraphic_Palette = value
            NotifyPropertyChanged("colorPalette")
        End Set
    End Property

    Public Property Frames As List(Of ClsFrame2)
        Get
            Return clsGraphic_frames
        End Get
        Set(value As List(Of ClsFrame2))
            clsGraphic_frames = value
            NotifyPropertyChanged("frames")
        End Set
    End Property

    Public Property AnimationSpeed As Integer
        Get
            Return clsGraphic_animationSpeed
        End Get
        Set(value As Integer)
            clsGraphic_animationSpeed = value
            NotifyPropertyChanged("animationSpeed")
        End Set
    End Property


    Public Property ExtraFrame As Byte
        Get
            Return clsGraphic_Byte9
        End Get
        Set(value As Byte)
            clsGraphic_Byte9 = value
            NotifyPropertyChanged("extraFrame")
        End Set
    End Property

    Public Property LastUpdated As String
        Get
            Return clsGraphic_lastUpdated
        End Get
        Set(value As String)
            clsGraphic_lastUpdated = value
            NotifyPropertyChanged("lastUpdated")
        End Set
    End Property


    Public Sub Read(Optional strFileName As String = vbNullString)

        'On Error GoTo dBg


1:
        ' 20190815 Before this just set the filename; but not the (assumed) .pal file
        If strFileName <> vbNullString Then Me.FileName = strFileName

        Dim X As Integer = 0
        Dim curByte As Integer = 0
        Dim intTemplength As Integer = 0

        Dim clsGraphic_numFrames As Integer = 0          ' Number of frames for this animation (at least 1)


        Debug.Print("Graphic: reset defaults.")
        ' Read graphics file.
        ' Derive our main color palette.
        ' Get details about frames etc. 
        ' Resets all defaults.

        ' Reset palettes
        Me.ColorPalette = New clsPalette(Me)

        ' Reset other info 
        clsGraphic_animationSpeed = 0
        clsGraphic_Byte9 = 0


5:

        ' === Read file contents ===

        Debug.Print("   : file: " & clsGraphic_FileName)
        Debug.Print("   : read file contents...")

        ' Read full file.
        Dim bytes As Byte() = IO.File.ReadAllBytes(clsGraphic_FileName)
        Dim tHex As String() = Array.ConvertAll(bytes, Function(b) b.ToString("X2"))
        Dim hexBytes As New List(Of String)
        hexBytes.AddRange(tHex)



        'Debug.Print(Strings.Join(hex, " "))

10:

        If hexBytes(0) = "46" And hexBytes(1) = "41" And hexBytes(2) = "54" And hexBytes(3) = "5A" Then
            Debug.Print("   : confirmed a FATZ-file (ZT Anim File?).")
            ' 46 41 54 5a 00 00 00 00 01
            ' what's the 01?
            clsGraphic_Byte9 = hexBytes(8)
            hexBytes.Skip(9)
        Else
            Debug.Print("   : likely a basic ZT1 graphics file.")
        End If


        Debug.Print("   : process basic information...")

15:
        ' === ANIMATION SPEED ===
        clsGraphic_animationSpeed = CInt("&H" & hexBytes(3) & hexBytes(2) & hexBytes(1) & hexBytes(0))
        Debug.Print("   : animation speed = " & clsGraphic_animationSpeed)

20:
        ' === FILENAME ===
        ' How many bytes is the palette file name?
        Debug.Print("&H" & hexBytes(7) & hexBytes(6) & hexBytes(5) & hexBytes(4))
        intTemplength = CInt("&H" & hexBytes(7) & hexBytes(6) & hexBytes(5) & hexBytes(4)) - 1

30:

        X = 0
        While X < intTemplength
            Me.ColorPalette.fileName &= Chr(CInt("&H" & hexBytes(8 + X)))
            X += 1
        End While
        Debug.Print("   : palette name = '" & Me.ColorPalette.fileName & "' (length: " & intTemplength & ")")


        ' remove all previous bytes.
        hexBytes.Skip(8 + intTemplength + 1)


40:
        ' === READ COLOR PALETTE ===
        Debug.Print("Graphics: read palette: '" & cfg_path_Root & "/" & Me.ColorPalette.fileName & "'...")
        If (Me.ColorPalette.readPal(cfg_path_Root & "/" & Me.ColorPalette.fileName) = 0) Then Exit Sub


        'Debug.Print(Strings.Join(hex, " "))


50:
        ' === NUM OF FRAMES ===
        ' we might need more bytes for this.
        'Debug.Print("Graphics: Determining number of frames... ")
        clsGraphic_numFrames = CInt("&H" & hexBytes(curByte + 3) & hexBytes(curByte + 2) & hexBytes(curByte + 1) & hexBytes(curByte))
        Debug.Print("Graphics: number of frames = " & clsGraphic_numFrames)

        ' remove all these bytes.
        hexBytes.Skip(4)





100:
        ' ==================================== FOR EACH FRAME... ===================================
        Dim intCurrentFrame As Integer = 0
        Dim intFrameBytes As Integer = 0
        Dim intFrameBytesCurrent As Integer = 0

        Dim ztFrames As New List(Of ClsFrame2)  ' Strings of HEX will be stored here, for each frame

        While hexBytes.Count > 0

101:

            ' Now, the next 4 bytes determine the length of bytes to follow for this particular animation
            intFrameBytes = CInt("&H" & hexBytes(curByte + 3) & hexBytes(curByte + 2) & hexBytes(curByte + 1) & hexBytes(curByte))

            ' remove all these bytes.
            hexBytes.Skip(4)

            Debug.Print("   Bytes to follow for the entire frame: " & intFrameBytes)
102:

            Dim ztFrame As New ClsFrame2(Me)
            Dim frameEntireHex As New List(Of String)

            ' Build our hex string first.
            For intFrameBytesCurrent = 0 To (intFrameBytes - 1)
                frameEntireHex.Add(hexBytes(intFrameBytesCurrent))
            Next

103:



            ' Write our hex string to our frame
            ztFrame.CoreImageHex = frameEntireHex

104:
            ' It's best to render the bitmap. This also sets offsets etc.
            ztFrame.RenderCoreImageFromHex()


            ztFrames.Add(ztFrame, False)



110:
            ' Remove those frame bytes
            hexBytes.Skip(intFrameBytes)

155:
            'Debug.Print("Graphics: total bytes of frame " & (ztFrames.Count).ToString("00") & "/" & (clsGraphic_numFrames) & " = " & (Strings.Replace(ztFrame.hexString, " ", "").Length / 2) & vbTab & "Bytes left: " & hex.Length)


            intCurrentFrame += 1

        End While


200:
        ' if clsGraphic_Byte9 = 1, then animated, last frame = still
        clsGraphic_frames = ztFrames

201:
        ' pre-render last image.
        Me.Frames(Me.Frames.Count - 1).RenderCoreImageFromHex()



        ' Find and split animations.

205:
        Me.LastUpdated = Now.ToString("yyyyMMddHHmmss")

        Exit Sub

dBg:
        MsgBox("Error in class clsGraphic2, read(), at line " & Erl() & ": " & vbCrLf &
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
        If Me.ColorPalette.fileName = vbNullString Then
            Me.ColorPalette.fileName = Me.FileName & ".pal"
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

        Dim palName As String = Me.ColorPalette.fileName
        palName = Strings.Replace(palName, "\", "/")
        palName = Strings.Replace(palName, Strings.Replace(cfg_path_Root, "\", "/") & "/", "", , , CompareMethod.Text)
        'Debug.Print(".... Palette: " & palName & vbCrLf & Strings.Replace(cfg_path_Root, "\", "/"))


        With opHexGraphic

            ' === Always ZTAF? Or extra frame ===
            If (Me.ExtraFrame = 1 Or cfg_export_ZT1_AlwaysAddZTAFBytes = 1) Then

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
            Dim sb As New System.Text.StringBuilder
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

            For Each ztFrame As ClsFrame2 In Me.Frames

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
        If Me.ColorPalette.fileName = Me.FileName & ".pal" Then
            'Debug.Print("... Writing color palette (not shared).")
            Me.ColorPalette.writePal(Me.ColorPalette.fileName, True)
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

        ' This   will render all frames
        For Each ztFrame As ClsFrame2 In Me.Frames
            ztFrame.GetImage()

        Next

    End Sub


    Function GetDefiningRectangle() As Rectangle

        Dim rect As New Rectangle

        Dim coordA As New Point(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2)
        Dim coordB As New Point(-cfg_grid_numPixels * 2, -cfg_grid_numPixels * 2)

        Me.RenderFrames()


        For Each ztFrame As ClsFrame2 In Me.Frames

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
        clsTasks.update_info("Property of graphic changed: " & info)

        'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub




    Public Sub New()

        Me.colorPalette.parent = Me


    End Sub
End Class
