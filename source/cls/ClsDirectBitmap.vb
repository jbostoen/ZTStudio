
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

''' <summary>
''' Class DirectBitmap. Offers a faster alternative to native Bitmap's GetPixel() and SetPixel()
''' </summary>
Public Class ClsDirectBitmap
    Implements IDisposable


    Public Property DirectBitmap_Bitmap As Bitmap
    Public Property DirectBitmap_Bits As Integer()
    Public Property DirectBitmap_Disposed As Boolean
    Public Property DirectBitmap_Height As Integer
    Public Property DirectBitmap_Width As Integer
    Protected Property DirectBitmap_BitsHandle As GCHandle

    Public Property Bits As Integer()
        Get
            Return DirectBitmap_Bits
        End Get
        Set(value As Integer())
            DirectBitmap_Bits = value
        End Set
    End Property

    Public Property Disposed As Boolean
        Get
            Return DirectBitmap_Disposed
        End Get
        Set(value As Boolean)
            DirectBitmap_Disposed = True
        End Set
    End Property
    Public Property BitsHandle As GCHandle
        Get
            Return DirectBitmap_BitsHandle
        End Get
        Set(value As GCHandle)
            DirectBitmap_BitsHandle = value
        End Set
    End Property
    Public Property Width As Integer
        Get
            Return DirectBitmap_Width
        End Get
        Set(value As Integer)
            DirectBitmap_Width = value
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

    Public Property Bitmap As Bitmap
        Get
            Return DirectBitmap_Bitmap
        End Get
        Set(value As Bitmap)
            DirectBitmap_Bitmap = value
        End Set
    End Property


    Public Sub New(ByVal IntWidth As Integer, ByVal IntHeight As Integer)
        Me.Width = Width
        Me.Height = Height
        Me.Bits = New Integer(IntWidth * IntHeight - 1) {}
        Me.BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned)
        Me.Bitmap = New Bitmap(IntWidth, IntHeight, IntWidth * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject())
    End Sub

    Public Sub SetPixel(ByVal x As Integer, ByVal y As Integer, ByVal ObjColor As Color)
        Dim IntIndex As Integer = x + (y * Me.Width)
        Dim IntCol As Integer = ObjColor.ToArgb()
        Me.Bits(IntIndex) = IntCol
    End Sub

    Public Function GetPixel(ByVal x As Integer, ByVal y As Integer) As Color
        Dim IntIndex As Integer = x + (y * Me.Width)
        Dim IntCol As Integer = Bits(IntIndex)
        Dim ObjColor As Color = Color.FromArgb(IntCol)
        Return ObjColor
    End Function

    Public Sub Dispose()
        If Me.Disposed = True Then
            Return
        End If

        Me.Disposed = True

        Me.Bitmap.Dispose()
        Me.BitsHandle.Free()
    End Sub

    Private Sub IDisposable_Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub
End Class
