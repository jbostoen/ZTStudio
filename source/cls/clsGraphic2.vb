Imports System.IO
Imports System.ComponentModel



Public Class clsGraphic2

    Implements INotifyPropertyChanged

    ' This class handles ZT1 graphic files, e.g. "N". 
    ' All handling of palette files is done by a different class


    Private editorGraphic_FileName As String                ' File name of graphic
    Private editorGraphic_Palette As New clsPalette         ' Main color palette
 
    Private editorGraphic_animationSpeed As Integer = 125     ' Speed in milliseconds for this animation


    'Private editorGraphic_frames() As String
    ' Strings containing HEX info about each frame.
    ' Each HEX string contains info about height, width, X/Y offsets, 
    ' and obviously how the image is rendered.


    Private editorGraphic_Byte9 As Byte = 0                 ' Basic files, FATZ-files with byte 9 = 0: 0. Byte 9 = 1: 1. Extra byte.

    Private editorGraphic_frames As New List(Of clsFrame)
    Private editorGraphic_lastUpdated As String = Now.ToString("yyyyMMddHHmmss")            ' For caching purposes for larger frames.


    Public Property fileName As String
        Get
            Return editorGraphic_FileName
        End Get
        Set(value As String)
            editorGraphic_FileName = value.ToLower()

            ' 20150624: If the .pal filename is not set yet:
            If Me.colorPalette.fileName = "" Then
                Me.colorPalette.fileName = Me.fileName & ".pal"
            End If

            NotifyPropertyChanged("fileName")
        End Set
    End Property

    Public Property colorPalette As clsPalette
        Get
            Return editorGraphic_Palette
        End Get
        Set(value As clsPalette)
            editorGraphic_Palette = value
            NotifyPropertyChanged("colorPalette")
        End Set
    End Property
      
    Public Property frames As List(Of clsFrame)


        Get
            Return editorGraphic_frames
        End Get
        Set(value As List(Of clsFrame))
            editorGraphic_frames = value
            NotifyPropertyChanged("frames")
        End Set
    End Property

    Public Property animationSpeed As Integer
        Get
            Return editorGraphic_animationSpeed
        End Get
        Set(value As Integer)
            editorGraphic_animationSpeed = value
            NotifyPropertyChanged("animationSpeed")
        End Set
    End Property


    Public Property extraFrame As Byte
        Get
            Return editorGraphic_Byte9
        End Get
        Set(value As Byte)
            editorGraphic_Byte9 = value
            NotifyPropertyChanged("extraFrame")
        End Set
    End Property

    Public Property lastUpdated As String
        Get
            Return editorGraphic_lastUpdated
        End Get
        Set(value As String)
            editorGraphic_lastUpdated = value
            NotifyPropertyChanged("lastUpdated")
        End Set
    End Property


    Public Function read(Optional strFileName As String = vbNullString) As Integer

        'On Error GoTo dBg


1:

        If strFileName <> vbNullString Then editorGraphic_FileName = strFileName

        Dim X As Integer = 0
        Dim curByte As Integer = 0
        Dim intTemplength As Integer = 0

        Dim editorGraphic_numFrames As Integer = 0          ' Number of frames for this animation (at least 1)


        Debug.Print("Graphic: reset defaults.")
        ' Read graphics file.
        ' Derive our main color palette.
        ' Get details about frames etc. 
        ' Resets all defaults.

        ' Reset palettes
        editorGraphic_Palette = New clsPalette 

        ' Reset other info 
        editorGraphic_animationSpeed = 0
        editorGraphic_Byte9 = 0


5:

        ' === Read file contents ===

        Debug.Print("   : file: " & editorGraphic_FileName)
        Debug.Print("   : read file contents...")

        ' Read full file.
        Dim bytes As Byte() = IO.File.ReadAllBytes(editorGraphic_FileName)
        Dim hex As String() = Array.ConvertAll(bytes, Function(b) b.ToString("X2"))

        'Debug.Print(Strings.Join(hex, " "))

10:

        If hex(0) = "46" And hex(1) = "41" And hex(2) = "54" And hex(3) = "5A" Then
            Debug.Print("   : confirmed a FATZ-file (ZT Anim File?).")
            ' 46 41 54 5a 00 00 00 00 01
            ' what's the 01?
            editorGraphic_Byte9 = hex(8)
            hex = hex.Skip(9).ToArray()
        Else
            Debug.Print("   : likely a basic ZT1 graphics file.")
        End If


        Debug.Print("   : process basic information...")

15:
        ' === ANIMATION SPEED ===
        editorGraphic_animationSpeed = CInt("&H" & hex(3) & hex(2) & hex(1) & hex(0))
        Debug.Print("   : animation speed = " & editorGraphic_animationSpeed)

20:
        ' === FILENAME ===
        ' How many bytes is the palette file name?
        Debug.Print("&H" & hex(7) & hex(6) & hex(5) & hex(4))
        intTemplength = CInt("&H" & hex(7) & hex(6) & hex(5) & hex(4)) - 1

30:

        X = 0
        While X < intTemplength
            editorGraphic_Palette.fileName &= Chr(CInt("&H" & hex(8 + X)))
            X += 1
        End While
        Debug.Print("   : palette name = '" & editorGraphic_Palette.fileName & "' (length: " & intTemplength & ")")


        ' remove all previous bytes.
        hex = hex.Skip(8 + intTemplength + 1).ToArray()


40:
        ' === READ COLOR PALETTE ===
        Debug.Print("Graphics: read palette: '" & cfg_path_Root & "/" & editorGraphic_Palette.fileName & "'...")
        If (editorGraphic_Palette.readPal(cfg_path_Root & "/" & editorGraphic_Palette.fileName) = 0) Then Exit Function


        'Debug.Print(Strings.Join(hex, " "))


50:
        ' === NUM OF FRAMES ===
        ' we might need more bytes for this.
        'Debug.Print("Graphics: Determining number of frames... ")
        editorGraphic_numFrames = CInt("&H" & hex(curByte + 3) & hex(curByte + 2) & hex(curByte + 1) & hex(curByte))
        Debug.Print("Graphics: number of frames = " & editorGraphic_numFrames)

        ' remove all these bytes.
        hex = hex.Skip(4).ToArray()




100:
        ' ==================================== FOR EACH FRAME... ===================================
        Dim intCurrentFrame As Integer = 0
        Dim intFrameBytes As Integer = 0
        Dim intFrameBytesCurrent As Integer = 0

        Dim ztFrames As New List(Of clsFrame)  ' Strings of HEX will be stored here, for each frame

        While hex.Length > 0

101:

            ' Now, the next 4 bytes determine the length of bytes to follow for this particular animation
            intFrameBytes = CInt("&H" & hex(curByte + 3) & hex(curByte + 2) & hex(curByte + 1) & hex(curByte))

            ' remove all these bytes.
            hex = hex.Skip(4).ToArray()

102:

            Dim ztFrame As New clsFrame
            Dim strFrame As String = ""

            ' Build our hex string first.
            For intFrameBytesCurrent = 0 To (intFrameBytes - 1)
                strFrame &= " " & hex(intFrameBytesCurrent)
            Next

103:

            ' Write our hex string to our frame
            With ztFrame
                '.index = intCurrentFrame
                .parent = Me
                .hexString = strFrame
            End With
            ztFrames.Add(ztFrame, False)

104:
            ' Remove those frame bytes
            hex = hex.Skip(intFrameBytes).ToArray()

105:
            'Debug.Print("Graphics: total bytes of frame " & (ztFrames.Count).ToString("00") & "/" & (editorGraphic_numFrames) & " = " & (Strings.Replace(ztFrame.hexString, " ", "").Length / 2) & vbTab & "Bytes left: " & hex.Length)


            intCurrentFrame += 1

        End While


200:
        ' if editorGraphic_Byte9 = 1, then animated, last frame = still
        editorGraphic_frames = ztFrames



        'Debug.Print("End processing?")

        ' Find and split animations.


        'renderFrame(Strings.Trim(strFrames(0)), bm)
205:
        Me.lastUpdated = Now.ToString("yyyyMMddHHmmss")

        Exit Function

dBg:
        MsgBox("Error in class clsGraphic2, read(), at line " & Erl() & ": " & vbCrLf & _
             Err.Number & " - " & Err.Description _
           & vbCrLf & "Line: " & Erl(), vbOKOnly + vbCritical, "Error")

    End Function






    Public Function write(Optional strFileName As String = vbNullString, Optional blnOverwrite As Boolean = True) As Integer



        On Error GoTo dBug

        Dim opHexG As New List(Of String)

1:
        If strFileName <> vbNullString Then
            Me.fileName = strFileName
        End If

5:

        If File.Exists(strFileName) = True And blnOverwrite = False Then
            MsgBox("Error: could not create a ZT1 Graphic." & vbCrLf & _
                "There is already a file at this location: " & vbCrLf & _
                "'" & strFileName & "'", vbOKOnly + vbCritical, "Failed to create ZT1 Graphic")

            Return 0
        End If




10:
        ' === Currently we only support basic files. ===
        ' We simply write out our frames etc.
        ' set path to use '/' instead of '\'
        Debug.Print("... Graphic: start writing.")

        Dim palName As String = Me.colorPalette.fileName
        palName = Strings.Replace(palName, "\", "/")
        palName = Strings.Replace(palName, Strings.Replace(cfg_path_Root, "\", "/") & "/", "", , , CompareMethod.Text)
        Debug.Print(palName & vbCrLf & Strings.Replace(cfg_path_Root, "\", "/"))


        With opHexG

            ' === Always ZTAF? Or extra frame ===
            If (Me.extraFrame = 1 Or cfg_export_ZT1_AlwaysAddZTAFBytes = 1) Then

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
                If Me.extraFrame = 1 Then
                    .Add("01", False)
                Else
                    .Add("00", False)
                End If
            End If

            ' === Animation speed ===
            .AddRange(Strings.Split((Me.animationSpeed).ToString("X8").ReverseHEX(), " "), False)


            ' === Palette file name length ===
            .AddRange(Strings.Split((palName.Length + 1).ToString("X8").ReverseHEX(), " "), False)

            ' === Palette file name ===
            Dim sb As New System.Text.StringBuilder
            For Each c As Char In palName
                .Add(Convert.ToString(Convert.ToInt32(c), 16), False)
            Next c
            .Add("00", False)

            ' === Number of frames ====
            .AddRange(Strings.Split((Me.frames.Count - Me.extraFrame).ToString("X8").ReverseHEX(), " "), False)

            ' === We need the total length. This could be a lot. Haven't calculated the actual limit. ===


            ' === Now, for each frame ===
            Dim hexSub As New List(Of String)
            Dim hexFrame As New List(Of String)
            'MsgBox(Me.frames.Count)

            Debug.Print("... Start writing frames.")

            'GoTo 800

            For Each ztFrame As clsFrame In Me.frames

                ' Here we get the HEX *inside* the frame.
                ' However, the ZT1 Graphic format also  wants us to tell how many bytes there are in the frame.
                ' That should come first, so we're currently putting this into a variable
                ' so we can add the number of bytes first. (4 bytes).

                ' We also need to make sure that our render does not include the BG Frame!
                ztFrame.lastUpdated = vbNullString
                hexFrame = ztFrame.getHex(Nothing, False, True)

                ' Specify number of bytes of this frame.
                hexSub.AddRange(Strings.Split(hexFrame.Count.ToString("X8").ReverseHEX(), " "), False)

                ' Add frame bytes.
                hexSub.AddRange(hexFrame, False)


            Next

            Debug.Print("... End writing frames.")

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
        File.Delete(Me.fileName)

        Dim fs As New FileStream(Me.fileName, FileMode.CreateNew, FileAccess.Write)

1001:

        For Each s As String In opHexG
1002:
            fs.WriteByte(CByte("&H" & s))
        Next

1003:
        fs.Close()
        fs.Dispose()



        ' Let's not forget:
1100:
        Debug.Print("... Write color palette.")
        editorGraphic_Palette.writePal(editorGraphic_Palette.fileName, True)


        Return 1

        Exit Function

dBug:
        MsgBox("Error while creating a ZT1 Graphic." & vbCrLf &
               strFileName & vbCrLf & _
            "Line " & Erl() & vbCrLf &
            Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error while creating a ZT1 Graphic.")





    End Function


    Sub renderFrames()

        ' This   will render all frames
        For Each ztFrame As clsFrame In Me.frames
            ztFrame.renderFrame()
        Next

    End Sub


    Function getDefiningRectangle() As Rectangle

        Dim rect As New Rectangle

        Dim coordA As New Point(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2)
        Dim coordB As New Point(-cfg_grid_numPixels * 2, -cfg_grid_numPixels * 2)

        Me.renderFrames()


        For Each ztFrame As clsFrame In Me.frames

            ' One way to do it, is to gather the offsets and width/height
            If ztFrame.offsetX < coordA.X Then coordA.X = ztFrame.offsetX
            If ztFrame.offsetY < coordA.Y Then coordA.Y = ztFrame.offsetY

            ' Now, the width.
            If (ztFrame.offsetX + ztFrame.width) > coordB.X Then coordB.X = (ztFrame.offsetX + ztFrame.width)
            If (ztFrame.offsetY + ztFrame.height) > coordB.Y Then coordB.Y = (ztFrame.offsetY + ztFrame.height)

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

        clsTasks.update_Info("Property of graphic changed.")
        'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub




End Class
