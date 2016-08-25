Imports System.IO

Public Class clsAniFile

    ' The .ani file is mostly used for icons in ZT1.
    ' Every graphic, with 1 or multiple views (N, NE, NW, SE, SW, S, E, ...) has one .ani-file.
    ' It contains a header, [Animation]
    ' It contains a dir line for each directory in the path (eg objects/bamboo/idle => objects, bamboo, idle )
    ' It contains an animation line for each view for this graphic ( N, NE, NW, SE, SW, S, E ...)
    ' How the order is defined, is unclear for most part.
    ' It contains x0, y0 coordinates and x1, y1 coordinates.
    ' Those seem to define the upper left and bottom right pixels, defining the maximum canvas size, 
    ' for all graphics belonging to this 'object' (consider anything: guests, staff, animals, objects, paths ... )


    Private ani_x0 As Integer = 0 ' canvas: top left
    Private ani_y0 As Integer = 0 ' canvas: top left
    Private ani_x1 As Integer = 0 ' canvas: bottom right
    Private ani_y1 As Integer = 0 ' canvas: bottom right

    Private ani_dirs As New List(Of String) ' lists the dirs in the relative location to this file
    Private ani_animations As New List(Of String) ' lists the animations in this file

    Private ani_fileName As String = "" ' filename of .ani-file

    Public Property x0 As Integer
        Get
            Return ani_x0
        End Get
        Set(value As Integer)
            ani_x0 = value
        End Set
    End Property

    Public Property x1 As Integer
        Get
            Return ani_x1

        End Get
        Set(value As Integer)
            ani_x1 = value
        End Set
    End Property

    Public Property y0 As Integer
        Get
            Return ani_y0
        End Get
        Set(value As Integer)
            ani_y0 = value
        End Set
    End Property
    Public Property y1 As Integer
        Get
            Return ani_y1
        End Get
        Set(value As Integer)
            ani_y1 = value
        End Set
    End Property


    Public Property dirs As List(Of String)
        Get
            Return ani_dirs
        End Get
        Set(value As List(Of String))
            ani_dirs = value
        End Set
    End Property

    Public Property animations As List(Of String)
        Get
            Return ani_animations
        End Get
        Set(value As List(Of String))
            ani_animations = value
        End Set
    End Property

    Public Property fileName As String
        Get
            Return ani_fileName
        End Get
        Set(value As String)
            ani_fileName = value
        End Set
    End Property

    ' Functions. 

    Public Function write(Optional sFile As String = Nothing)


        If IsNothing(sFile) = False Then Me.fileName = sFile

        ' This function will write out everything.


        On Error GoTo dBug

        Dim strAni As String = "[Animation]" & vbCrLf


1:
        ' If file exists: delete.
        If File.Exists(sFile) = True Then
            File.Delete(sFile)
        End If

2:
        ' Write out dirs
        For Each s As String In Me.dirs
            strAni = strAni & "dir = " & s & vbCrLf
        Next

3:
        ' Write out animations
        For Each s As String In Me.animations
            strAni = strAni & "animation = " & s & vbCrLf
        Next

4:
        ' Now, the coordinates
        strAni = strAni & _
            "x0 = " & Me.x0 & vbCrLf & _
            "y0 = " & Me.y0 & vbCrLf & _
            "x1 = " & Me.x1 & vbCrLf & _
            "y1 = " & Me.y1 & vbCrLf

10:
        ' Write.
        If Me.animations.Count > 0 And Me.dirs.Count > 0 Then
            If File.Exists(Me.fileName) = True Then
                File.Delete(Me.fileName)
            End If
            Using outfile As New StreamWriter(Me.fileName)
                outfile.Write(strAni.ToString())
            End Using
        End If


        Exit Function

dBug:

        MsgBox("Error in clsAniFile:saveAni at line " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error while creating .ani-file")
    End Function



    ' === special functions ===

    Public Function guessConfig(Optional sFileName As String = Nothing)



        ' This function needs a filename for the .ani-file, since it derives its directory from it
        ' This function will take note of the 'dirs'
        ' This function will try to find out whether we're dealing with one of these 4 path types:
        ' N                 icons
        ' NE/NW/SE/SW       objects
        ' N/NE/E/SE/S       animals, guests, staff...
        ' 1-20              paths

        If IsNothing(sFileName) = False Then
            Me.fileName = sFileName.Replace("/", "\")
        End If


1:
        If Me.fileName = "" Then

            MsgBox("clsAniFile:guessAnimations assumes you've set the filename for the .ani-file.", _
                vbOKOnly + vbCritical, "Error while guessing animations for .ani-file")

        Else

2:

            ' This is the full path and the relative path of the .ani file
            Dim strPath As String = Path.GetDirectoryName(Me.fileName)
            Dim strPathRel As String
            strPathRel = Strings.Replace(strPath, cfg_path_Root & "\", "")
            strPathRel = Strings.Replace(strPathRel, cfg_path_Root, "")
            Dim g As New clsGraphic2

            Debug.Print("Ani path: " & vbCrLf & "* " & strPath & vbCrLf & "* " & strPathRel)

            ' Set dirs. If this function is called multiple times, it won't do any harm.
            Me.dirs.Clear(False)
            Me.dirs.AddRange(Strings.Split(strPathRel, "\"), False)

10:
            ' Set animations.
            Me.animations.Clear(False)

11:
            If File.Exists(strPath & "\N") = True And _
                File.Exists(strPath & "\NE") = True And _
                File.Exists(strPath & "\E") = True And _
                File.Exists(strPath & "\SE") = True And _
                File.Exists(strPath & "\S") = True Then


                ' animal, guest, staff...
                With Me.animations
                    .Add("N", False)
                    .Add("NE", False)
                    .Add("E", False)
                    .Add("SE", False)
                    .Add("S", False)
                End With


            End If

12:
            If File.Exists(strPath & "\NE") = True And _
                File.Exists(strPath & "\SE") = True And _
                File.Exists(strPath & "\SW") = True And _
                File.Exists(strPath & "\NW") = True Then

                ' object
                With Me.animations
                    .Add("NE", False)
                    .Add("SE", False)
                    .Add("SW", False)
                    .Add("NW", False)
                End With

                Debug.Print("... Detected: object")

            End If

13:

            If File.Exists(strPath & "\N") = True Then

                ' icon
                With Me.animations
                    .Add("N", False)
                End With


            End If


14:

            If File.Exists(strPath & "\1") = True And _
                File.Exists(strPath & "\2") = True And _
                File.Exists(strPath & "\3") = True And _
                File.Exists(strPath & "\4") = True And _
                File.Exists(strPath & "\5") = True And _
                File.Exists(strPath & "\6") = True And _
                File.Exists(strPath & "\7") = True And _
                File.Exists(strPath & "\8") = True And _
                File.Exists(strPath & "\9") = True And _
                File.Exists(strPath & "\10") = True And _
                File.Exists(strPath & "\11") = True And _
                File.Exists(strPath & "\12") = True And _
                File.Exists(strPath & "\13") = True And _
                File.Exists(strPath & "\14") = True And _
                File.Exists(strPath & "\15") = True And _
                File.Exists(strPath & "\16") = True And _
                File.Exists(strPath & "\17") = True And _
                File.Exists(strPath & "\18") = True And _
                File.Exists(strPath & "\19") = True And _
                File.Exists(strPath & "\20") = True Then

                ' paths
                ' we could do this shorter
                With Me.animations
                    Dim intX As Integer = 1
                    While intX <= 20
                        .Add(intX.ToString("0"), False)
                        intX += 1
                    End While
                End With


            End If

100:
            If Me.animations.Count > 0 Then
                For Each sAni In Me.animations

                    ' We need to read every view for this graphic in the folder.
                    g.read(strPath.Replace("\", "/") & "/" & sAni)

                    For Each ztFrame As clsFrame In g.frames

                        ' Get original hex
                        ztFrame.getHex(Nothing, False, False)

                        '  Debug.Print("ztFrame offsetX = " & ztFrame.offsetX & "," & ztFrame.offsetY & "," & _
                        '              ztFrame.height & "," & ztFrame.width)


                        ' Passes the bamboo.ani-test
                        Me.x0 = Math.Min(Me.x0, -ztFrame.offsetX)
                        Me.y0 = Math.Min(Me.y0, -ztFrame.offsetY)
                        Me.x1 = Math.Max(Me.x1, -ztFrame.offsetX + ztFrame.width)
                        Me.y1 = Math.Max(Me.y1, -ztFrame.offsetY + ztFrame.height)
                    Next
                Next
            Else
                Debug.Print("guessConfig: type not recognized.")
            End If


        End If


50:

        Me.write()



        Exit Function

dBug:
        MsgBox("Error in clsAniFile:guessConfig at line " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error while guessing config for .ani-file")

    End Function


End Class
