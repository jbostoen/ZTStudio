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

    Private Ani_Dirs As New List(Of String) ' lists the dirs in the relative location to this file
    Private Ani_Views As New List(Of String) ' lists the Views in this file

    Private ani_fileName As String = "" ' filename of .ani-file

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
    Public Property Dirs As List(Of String)
        Get
            Return ani_dirs
        End Get
        Set(value As List(Of String))
            ani_dirs = value
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

        Dim strAni As String = "[animation]" & vbCrLf

1:
        ' If there's a .ani-file present, delete it first.
        If File.Exists(Me.FileName) = True Then
            File.Delete(Me.FileName)
        End If

2:
        ' Write out dirs
        For Each s As String In Me.Dirs
            strAni = strAni & "dir" & Me.Dirs.IndexOf(s) & " = " & s & vbCrLf
        Next

3:
        ' Write out views
        For Each s As String In Me.Views
            strAni = strAni & "animation = " & s & vbCrLf
        Next

4:
        ' Now, the coordinates
        strAni = strAni &
            "x0 = " & Me.X0 & vbCrLf &
            "y0 = " & Me.Y0 & vbCrLf &
            "x1 = " & Me.X1 & vbCrLf &
            "y1 = " & Me.Y1 & vbCrLf

10:
        ' Write.
        If Me.Views.Count > 0 And Me.Dirs.Count > 0 Then
            If File.Exists(Me.FileName) = True Then
                File.Delete(Me.FileName)
            End If
            Using outfile As New StreamWriter(Me.FileName)
                outfile.Write(strAni.ToString())
            End Using
        End If

        Return 0


        Exit Function

dBug:
        MdlZTStudio.UnexpectedError("ClsTasks", "Write", Information.Erl(), Information.Err())


    End Function


    ''' <summary>
    ''' This sub tries to create a .ani-file. It does so based on the offsets of graphics it detects.
    ''' This is experimental, but it should work for the majority of graphics.
    ''' </summary>
    ''' <param name="strFileName">Destination filename</param>
    Public Sub CreateAniConfig(Optional strFileName As String = Nothing)

        ' This function needs a filename for the .ani-file, since it derives its directory from it.
        ' It will take note of the 'dirs'
        ' It will try to find out whether it is dealing with one of these 4 types:
        '
        ' N                 icons
        ' NE/NW/SE/SW       objects
        ' N/NE/E/SE/S       animals, guests, staff...
        ' 1-20              paths

        If IsNothing(strFileName) = False Then
            Me.FileName = strFileName.Replace("/", "\")
        End If


1:
        If Me.FileName = "" Then

            ' Is there any path which leads up to this error?
            MsgBox("ClsAniFile::createAniConfig() assumes you've set the filename for the .ani-file.",
                vbOKOnly + vbCritical, "Error while guessing views for .ani-file")

        Else

2:

            ' This is the full path and the relative path of the .ani file
            Dim strPath As String = Path.GetDirectoryName(Me.FileName)
            Dim strPathRel As String
            strPathRel = Strings.Replace(strPath, Cfg_path_Root & "\", "")
            strPathRel = Strings.Replace(strPathRel, Cfg_path_Root, "")
            Dim Graphic As New ClsGraphic

            MdlZTStudio.Trace("ClsAniFile", "CreateAniConfig", "Ani path: * " & strPath & " -> " & strPathRel)

            ' Set dirs. If this function is called multiple times, it won't do any harm.
            Me.Dirs.Clear(False)
            Me.Dirs.AddRange(Strings.Split(strPathRel, "\"), False)

10:
            ' Set views.
            Me.Views.Clear(False)

11:
            If File.Exists(strPath & "\N") = True And
                File.Exists(strPath & "\NE") = True And
                File.Exists(strPath & "\E") = True And
                File.Exists(strPath & "\SE") = True And
                File.Exists(strPath & "\S") = True Then


                ' This is typical for animals, guests, staff...
                With Me.Views
                    .Add("N", False)
                    .Add("NE", False)
                    .Add("E", False)
                    .Add("SE", False)
                    .Add("S", False)
                End With

                MdlZTStudio.Trace("ClsAniFile", "CreateAniConfig", "Determination: 'animals', 'guests', 'staff', ...")

12:
            ElseIf File.Exists(strPath & "\NE") = True And
                File.Exists(strPath & "\SE") = True And
                File.Exists(strPath & "\SW") = True And
                File.Exists(strPath & "\NW") = True Then

                ' This is typical for objects
                With Me.Views
                    .Add("NE", False)
                    .Add("SE", False)
                    .Add("SW", False)
                    .Add("NW", False)
                End With

                MdlZTStudio.Trace("ClsAniFile", "CreateAniConfig", "Determination: 'object'")


13:

            ElseIf File.Exists(strPath & "\N") = True Then

                ' This is typical for icons
                With Me.Views
                    .Add("N", False)
                End With

                MdlZTStudio.Trace("ClsAniFile", "CreateAniConfig", "Determination: 'icon'")


14:

            ElseIf File.Exists(strPath & "\1") = True And
                File.Exists(strPath & "\2") = True And
                File.Exists(strPath & "\3") = True And
                File.Exists(strPath & "\4") = True And
                File.Exists(strPath & "\5") = True And
                File.Exists(strPath & "\6") = True And
                File.Exists(strPath & "\7") = True And
                File.Exists(strPath & "\8") = True And
                File.Exists(strPath & "\9") = True And
                File.Exists(strPath & "\10") = True And
                File.Exists(strPath & "\11") = True And
                File.Exists(strPath & "\12") = True And
                File.Exists(strPath & "\13") = True And
                File.Exists(strPath & "\14") = True And
                File.Exists(strPath & "\15") = True And
                File.Exists(strPath & "\16") = True And
                File.Exists(strPath & "\17") = True And
                File.Exists(strPath & "\18") = True And
                File.Exists(strPath & "\19") = True And
                File.Exists(strPath & "\20") = True Then

                ' This is typical for paths
                With Me.Views
                    Dim intX As Integer = 1
                    While intX <= 20
                        .Add(intX.ToString("0"), False)
                        intX += 1
                    End While
                End With

                MdlZTStudio.Trace("ClsAniFile", "CreateAniConfig", "Determination: 'path'")

15:
            Else

                MdlZTStudio.Trace("ClsAniFile", "CreateAniConfig", "Determination: unable to determine type of graphic in " & Path.GetDirectoryName(Me.FileName))

            End If

100:
            ' Will only do something if views were detected, in a similar fashion to what's known.
            ' For instance, if one graphic (SE) is used for 4 sides, ZTStudio will NOT recognize it and do nothing.
            If Me.Views.Count > 0 Then
                For Each sAni In Me.Views

                    ' We need to read every view for this graphic in the folder.
                    Graphic.Read(strPath.Replace("\", "/") & "/" & sAni)

                    For Each ztFrame As ClsFrame In Graphic.Frames

                        ' Get original hex
                        ztFrame.RenderCoreImageFromHex()

                        ' Passes the bamboo.ani-test
                        Me.X0 = Math.Min(Me.X0, -ztFrame.OffsetX)
                        Me.Y0 = Math.Min(Me.Y0, -ztFrame.OffsetY)
                        Me.X1 = Math.Max(Me.X1, -ztFrame.OffsetX + ztFrame.CoreImageBitmap.Width)
                        Me.Y1 = Math.Max(Me.Y1, -ztFrame.OffsetY + ztFrame.CoreImageBitmap.Height)

                    Next
                Next
            End If


        End If


50:

        Me.Write()


        Exit Sub

dBug:
        MdlZTStudio.UnexpectedError("ClsAniFile", "CreateAniConfig", Information.Erl(), Information.Err())

    End Sub


    Public Sub New(myFileName As String)
        Me.fileName = myFileName
    End Sub
End Class
