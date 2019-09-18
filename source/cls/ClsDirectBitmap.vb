Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public Class ClsDirectBitmap
    Implements IDisposable

    Private DirectBitMap_BitsHandle As GCHandle
    Private DirectBitMap_Bits As Integer()
    Private DirectBitmap_BitMap As Bitmap
    Private DirectBitmap_Height As Integer
    Private DirectBitMap_Width As Integer
    Private DirectBitMap_Disposed As Boolean
    Public Property Bitmap As Bitmap
        Get
            Return DirectBitmap_BitMap
        End Get
        Set(value As Bitmap)
            DirectBitmap_BitMap = value
        End Set
    End Property

    Public Property Bits As Integer()
        Get
            Return DirectBitMap_Bits
        End Get
        Set(value As Integer())
            DirectBitMap_Bits = value
        End Set
    End Property

    Public Property Disposed As Boolean
        Get
            Return DirectBitMap_Disposed
        End Get
        Set(value As Boolean)
            DirectBitMap_Disposed = value
        End Set
    End Property

    Public Property Height As Integer
        Get
            Return DirectBitmap_Height
        End Get
        Set(value As Integer)
            DirectBitmap_Height = value
        End Set
    End Property

    Public Property Width As Integer
        Get
            Return DirectBitMap_Width

        End Get
        Set(value As Integer)
            DirectBitMap_Width = value
        End Set
    End Property

    Protected Property BitsHandle As GCHandle
        Get
            Return DirectBitMap_BitsHandle
        End Get
        Set(value As GCHandle)
            DirectBitMap_BitsHandle = value
        End Set
    End Property

    Public Sub New(ByVal width As Integer, ByVal height As Integer)
        MyBase.New
        Me.Width = width
        Me.Height = height
        Me.Bits = New Int32(((width * height)) - 1) {}
        Me.BitsHandle = GCHandle.Alloc(Me.Bits, GCHandleType.Pinned)
        Me.Bitmap = New Bitmap(width, height, (width * 4), PixelFormat.Format32bppPArgb, Me.BitsHandle.AddrOfPinnedObject)
    End Sub

    Public Sub SetPixel(ByVal x As Integer, ByVal y As Integer, ByVal colour As Color)
        Dim index As Integer = (x _
                    + (y * Me.Width))
        Dim col As Integer = colour.ToArgb
        Me.Bits(index) = col
    End Sub

    Public Function GetPixel(ByVal x As Integer, ByVal y As Integer) As Color
        Dim index As Integer = (x _
                    + (y * Me.Width))
        Dim col As Integer = Me.Bits(index)
        Dim result As Color = Color.FromArgb(col)
        Return result
    End Function

    Sub Dispose() Implements IDisposable.Dispose

        If Me.Disposed = True Then
            Return
        End If

        Me.Disposed = True
        Me.Bitmap.Dispose()
        Me.BitsHandle.Free()
    End Sub
End Class