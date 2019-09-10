''' <summary>
''' <para>ClsDrawingInstr is used to manage a drawing instruction.</para>
''' <para>The instruction specifies which colors to draw, from left to right.</para>
''' <para>This typically consists of an offset (how many transparent pixels are there before drawing starts? Could be 0) followed by one or more references to colors.</para>
''' <para>The references to colors are made using their index number in the color palette.</para>
''' </summary>
Public Class ClsDrawingInstr

    ' A drawing instruction block.
    ' This consists of a simple pattern:
    ' [1 byte offset] [1 byte number of colors] [if present: indexes of colors]

    Dim di_offset As Integer = 0 ' only one byte. This is actually: 'skip X pixels in this line'. Max 255 at once.
    Dim di_lstColors As New List(Of Integer)  ' refers to the index of the color in a palette. Num colors = 0-255.

    ''' <summary>
    ''' Offset. This determines how many pixels to skip horizontally (from left to right) before actually starting to draw colored pixels.
    ''' </summary>
    ''' <returns>Integer</returns>
    Public Property Offset As Integer
        ' How many transparent pixels are there first?
        Get
            Return di_offset
        End Get
        Set(value As Integer)
            di_offset = value
        End Set
    End Property

    ''' <summary>
    ''' A list of color references. These colors will be drawn horizontally (from left to right), after the offset has been applied.
    ''' The colors are referenced by their index number in the palette.
    ''' </summary>
    ''' <returns>List(Of Integer) - color references</returns>
    Public Property PixelColors As List(Of Integer)
        ' Contains the pixels which will be drawn horizontally (row) and their color.
        Get
            Return di_lstColors
        End Get
        Set(value As List(Of Integer))
            di_lstColors = value
        End Set
    End Property

    ''' <summary>
    ''' Returns the hex form of this drawing instruction.
    ''' It consist of the offset (X2), the number of colored pixels (X2) and finally the index numbers of each color (X2 per color).
    ''' </summary>
    ''' <returns>List(Of String)</returns>
    Public Function GetHex() As List(Of String)

        ' Returns the hex code for this drawing block.
        On Error GoTo dBg

0:
        Dim opHex As New List(Of String)

1:
        ' Offset.
        opHex.Add(Me.Offset.ToString("X2"), False)

2:
        ' Number of colors. 0 - 255
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
        MdlZTStudio.UnhandledError(Me.GetType().FullName, "GetHex", Information.Err)

    End Function

End Class
