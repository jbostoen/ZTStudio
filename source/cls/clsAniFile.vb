Imports System.IO

''' <summary>
''' ClsAniFile manages information about the .ani file.
''' This file contains info about offsets.
''' </summary>
Public Class ClsAniFile

    ' The .ani file is mostly used for icons in ZT1.
    ' Every graphic, with 1 or multiple views (N, NE, NW, SE, SW, S, E, ...) has one .ani-file.
    ' It contains a header, [Animation]
    ' It contains a dir line for each directory in the path (eg objects/bamboo/idle => objects, bamboo, idle )
    ' It contains an animation line for each view for this graphic ( N, NE, NW, SE, SW, S, E ...)
    ' How the order is defined, is unclear for most part - for now.
    ' It contains x0, y0 coordinates and x1, y1 coordinates.
    ' Those seem to define the upper left and bottom right pixels, defining the maximum canvas size, 
    ' for all graphics belonging to this 'object' (consider anything: guests, staff, animals, objects, paths ... )


    Private Ani_X0 As Integer = 0 ' canvas: top left
    Private Ani_Y0 As Integer = 0 ' canvas: top left
    Private Ani_X1 As Integer = 0 ' canvas: bottom right
    Private Ani_Y1 As Integer = 0 ' canvas: bottom right

    Private Ani_RelativeDirectories As New List(Of String) ' lists the directories in the relative location to this file
    Private Ani_Views As New List(Of String) ' lists the Views in this file

    Private ani_FileName As String = "" ' filename of .ani-file

    ''' <summary>
    ''' Offset (X) of top left pixel
    ''' </summary>
    ''' <returns>Integer</returns>
    Public Property X0 As Integer
        Get
            Return ani_x0
        End Get
        Set(value As Integer)
            ani_x0 = value
        End Set
    End Property

    ''' <summary>
    ''' Offset (X) of bottom right pixel
    ''' </summary>
    ''' <returns>Integer</returns>
    Public Property X1 As Integer
        Get
            Return ani_x1

        End Get
        Set(value As Integer)
            ani_x1 = value
        End Set
    End Property

    ''' <summary>
    ''' Offset (Y) of top left pixel
    ''' </summary>
    ''' <returns>Integer</returns>
    Public Property Y0 As Integer
        Get
            Return ani_y0
        End Get
        Set(value As Integer)
            ani_y0 = value
        End Set
    End Property

    ''' <summary>
    ''' Offset (Y) of bottom right pixel
    ''' </summary>
    ''' <returns>Integer</returns>
    Public Property Y1 As Integer
        Get
            Return ani_y1
        End Get
        Set(value As Integer)
            ani_y1 = value
        End Set
    End Property

    ''' <summary>
    ''' List of directories (tree structure) relative to root
    ''' </summary>
    ''' <returns></returns>
    Public Property RelativeDirectories As List(Of String)
        Get
            Return Ani_RelativeDirectories
        End Get
        Set(value As List(Of String))
            Ani_RelativeDirectories = value
        End Set
    End Property

    ''' <summary>
    ''' List of all views 
    ''' </summary>
    ''' <returns></returns>
    Public Property Views As List(Of String)
        Get
            Return ani_views
        End Get
        Set(value As List(Of String))
            ani_views = value
        End Set
    End Property

    ''' <summary>
    ''' Filename of the .ani file
    ''' </summary>
    ''' <returns>String</returns>
    Public Property FileName As String
        Get
            Return ani_fileName
        End Get
        Set(value As String)
            ani_fileName = value
        End Set
    End Property

    ' Functions. 

    ''' <summary>
    ''' Writes a .ani file, based on the info in this object
    ''' </summary>
    ''' <param name="strFileName">Destination filename</param>
    ''' <returns></returns>
    Public Function Write(Optional strFileName As String = Nothing)


        If IsNothing(strFileName) = False Then
            Me.FileName = strFileName
        End If

        ' This function will write out the .ani-file.


        On Error GoTo dBug

        Dim StrAni As String = "[animation]" & vbCrLf

1:
        ' If there's a .ani-file present, delete it first.
        If File.Exists(Me.FileName) = True Then
            File.Delete(Me.FileName)
        End If

2:
        ' Write out dirs
        For Each s As String In Me.RelativeDirectories
            StrAni = StrAni & "dir" & Me.RelativeDirectories.IndexOf(s) & " = " & s & vbCrLf
        Next

3:
        ' Write out views
        For Each s As String In Me.Views
            StrAni = StrAni & "animation = " & s & vbCrLf
        Next

4:
        ' Now, the coordinates
        StrAni = StrAni &
            "x0 = " & Me.X0 & vbCrLf &
            "y0 = " & Me.Y0 & vbCrLf &
            "x1 = " & Me.X1 & vbCrLf &
            "y1 = " & Me.Y1 & vbCrLf

10:
        ' Write.
        If Me.Views.Count > 0 And Me.RelativeDirectories.Count > 0 Then
            If File.Exists(Me.FileName) = True Then
                File.Delete(Me.FileName)
            End If
            Using outfile As New StreamWriter(Me.FileName)
                outfile.Write(StrAni.ToString())
            End Using
        End If

        Return 0


        Exit Function

dBug:
        MdlZTStudio.UnhandledError(Me.GetType().FullName, "Write", Information.Err, False)


    End Function


    ''' <summary>
    ''' This sub tries to create a .ani-file. It does so based on the offsets of graphics it detects.
    ''' This is experimental, but it should work for the majority of graphics.
    ''' </summary>
    ''' <param name="StrFileName">Destination filename</param>
    Public Sub CreateAniConfig(Optional StrFileName As String = Nothing)

        ' This function needs a filename for the .ani-file, since it derives its directory from it.
        ' It will take note of the 'dirs'
        ' It will try to find out whether it is dealing with one of these 4 types:
        '
        ' N                 icons
        ' NE/NW/SE/SW       objects
        ' N/NE/E/SE/S       animals, guests, staff...
        ' 1-20              paths

        If IsNothing(StrFileName) = False Then
            Me.FileName = StrFileName.Replace("/", "\")
        End If


1:
        If Me.FileName = "" Then

            ' Is there any path which leads up to this error?
            MdlZTStudio.HandledError(Me.GetType().FullName, "CreateAniConfig", "Unexpected error: filename for .ani file is empty?", True, Information.Err)

        Else

2:

            ' This is the full path and the relative path of the .ani file
            Dim StrPath As String = Path.GetDirectoryName(Me.FileName)
            Dim StrPathRelative As String
            StrPathRelative = System.Text.RegularExpressions.Regex.Replace(StrPath, System.Text.RegularExpressions.Regex.Escape(Cfg_Path_Root) & "(\\|)", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            Dim ObjGraphic As New ClsGraphic(Nothing)

            MdlZTStudio.Trace(Me.GetType().FullName, "CreateAniConfig", "Root path: * " & Cfg_Path_Root)
            MdlZTStudio.Trace(Me.GetType().FullName, "CreateAniConfig", "Ani path: * " & StrPath & " -> " & StrPathRelative)

            ' Set dirs. If this function is called multiple times, it won't do any harm.
            Me.RelativeDirectories.Clear(False)
            Me.RelativeDirectories.AddRange(Strings.Split(StrPathRelative, "\"), False)

10:
            ' Set views.
            Me.Views.Clear(False)

11:
            If File.Exists(StrPath & "\N") = True And
                File.Exists(StrPath & "\NE") = True And
                File.Exists(StrPath & "\E") = True And
                File.Exists(StrPath & "\SE") = True And
                File.Exists(StrPath & "\S") = True Then


                ' This is typical for animals, guests, staff...
                With Me.Views
                    .Add("N", False)
                    .Add("NE", False)
                    .Add("E", False)
                    .Add("SE", False)
                    .Add("S", False)
                End With

                MdlZTStudio.Trace(Me.GetType().FullName, "CreateAniConfig", "Determination: 'animals', 'guests', 'staff', ...")

12:
            ElseIf File.Exists(StrPath & "\NE") = True And
                File.Exists(StrPath & "\SE") = True And
                File.Exists(StrPath & "\SW") = True And
                File.Exists(StrPath & "\NW") = True Then

                ' This is typical for objects
                With Me.Views
                    .Add("NE", False)
                    .Add("SE", False)
                    .Add("SW", False)
                    .Add("NW", False)
                End With

                MdlZTStudio.Trace(Me.GetType().FullName, "CreateAniConfig", "Determination: 'object'")


13:

            ElseIf File.Exists(StrPath & "\N") = True Then

                ' This is typical for icons
                With Me.Views
                    .Add("N", False)
                End With

                MdlZTStudio.Trace(Me.GetType().FullName, "CreateAniConfig", "Determination: 'icon'")


14:

            ElseIf File.Exists(StrPath & "\1") = True And
                File.Exists(StrPath & "\2") = True And
                File.Exists(StrPath & "\3") = True And
                File.Exists(StrPath & "\4") = True And
                File.Exists(StrPath & "\5") = True And
                File.Exists(StrPath & "\6") = True And
                File.Exists(StrPath & "\7") = True And
                File.Exists(StrPath & "\8") = True And
                File.Exists(StrPath & "\9") = True And
                File.Exists(StrPath & "\10") = True And
                File.Exists(StrPath & "\11") = True And
                File.Exists(StrPath & "\12") = True And
                File.Exists(StrPath & "\13") = True And
                File.Exists(StrPath & "\14") = True And
                File.Exists(StrPath & "\15") = True And
                File.Exists(StrPath & "\16") = True And
                File.Exists(StrPath & "\17") = True And
                File.Exists(StrPath & "\18") = True And
                File.Exists(StrPath & "\19") = True And
                File.Exists(StrPath & "\20") = True Then

                ' This is typical for paths
                With Me.Views
                    Dim IntX As Integer = 1
                    While IntX <= 20
                        .Add(IntX.ToString("0"), False)
                        IntX += 1
                    End While
                End With

                MdlZTStudio.Trace(Me.GetType().FullName, "CreateAniConfig", "Determination: 'path'")


15:

            ElseIf File.Exists(StrPath & "\N") = True And
                File.Exists(StrPath & "\H") = True And
                File.Exists(StrPath & "\S") = True And
                File.Exists(StrPath & "\G") = True Then

                ' This is typical for objects
                With Me.Views
                    .Add("N", False)
                    .Add("H", False)
                    .Add("S", False)
                    .Add("G", False)
                End With

                MdlZTStudio.Trace(Me.GetType().FullName, "CreateAniConfig", "Determination: 'ui button'")

99:
            Else

                MdlZTStudio.Trace(Me.GetType().FullName, "CreateAniConfig", "Determination: unable to determine type of graphic in " & Path.GetDirectoryName(Me.FileName))

            End If

100:
            ' Will only do something if views were detected, in a similar fashion to what's known.
            ' For instance, if one graphic (SE) is used for 4 sides, ZTStudio will NOT recognize it and do nothing.
            If Me.Views.Count > 0 Then
                For Each StrAni In Me.Views

                    ' We need to read every view for this graphic in the folder.
                    ObjGraphic.Read(StrPath.Replace("\", "/") & "/" & StrAni)

                    For Each ObjFrame As ClsFrame In ObjGraphic.Frames

                        ' Get original hex
                        ObjFrame.RenderCoreImageFromHex()

                        ' Passes the bamboo.ani-test
                        Me.X0 = Math.Min(Me.X0, -ObjFrame.OffsetX)
                        Me.Y0 = Math.Min(Me.Y0, -ObjFrame.OffsetY)
                        Me.X1 = Math.Max(Me.X1, -ObjFrame.OffsetX + ObjFrame.CoreImageBitmap.Width)
                        Me.Y1 = Math.Max(Me.Y1, -ObjFrame.OffsetY + ObjFrame.CoreImageBitmap.Height)

                    Next
                Next
            End If


        End If


50:

        Me.Write()


        Exit Sub

dBug:
        MdlZTStudio.UnhandledError(Me.GetType().FullName, "CreateAniConfig", Information.Err, True)

    End Sub


    Public Sub New(myFileName As String)
        Me.fileName = myFileName
    End Sub
End Class
