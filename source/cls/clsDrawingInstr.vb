Public Class clsDrawingInstr

    ' A drawing instruction block.
    ' This consists of a simple pattern:
    ' [1 byte offset] [1 byte number of colors] [if present: indexes of colors]

    Dim di_offset As Integer = 0 ' only one byte. This is actually: 'skip X pixels in this line'. Max 255 at once.
    Dim di_lstColors As New List(Of Integer)  ' refers to the index of the color in a palette. Num colors = 0-255.

    Public Property offset As Integer
        ' How many transparent pixels do we have before we start drawing this block?
        Get
            Return di_offset
        End Get
        Set(value As Integer)
            di_offset = value
        End Set
    End Property

    Public Property pixelColors As List(Of Integer)
        ' Contains the pixels which will be drawn horizontally (row) and their color.
        Get
            Return di_lstColors
        End Get
        Set(value As List(Of Integer))
            di_lstColors = value
        End Set
    End Property

    Public Function getHex() As List(Of String)

        ' Returns the hex code for this drawing block.
        On Error GoTo dBg

0:
        Dim opHex As New List(Of String)

1:
        ' Offset.
        opHex.Add(Me.offset.ToString("X2"), False)

2:
        ' Num colors. 0 - 255
        opHex.Add(Me.di_lstColors.Count.ToString("X2"), False)

3:
        ' Indexes of colors (~ colorpalette)
        For Each c As Integer In di_lstColors
            opHex.Add(c.ToString("X2"), False)
        Next

5:
        Return opHex

        Exit Function

dBg:
        MsgBox("Error in class clsDrawingInstr, getHex(), line " & Erl() & vbCrLf & _
            Err.Number & " " & Err.Description, vbOKOnly + vbCritical, "Error while generating frame HEX")

    End Function

End Class
