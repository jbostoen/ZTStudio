Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

''' <summary>
''' Custom class to perform faster bitmap operations
''' </summary>
Public Class ClsDirectBitmap
    Implements IDisposable

    Private DirectBitMap_BitsHandle As GCHandle
    Private DirectBitMap_Bits As Integer()
    Private DirectBitmap_BitMap As Bitmap
    Private DirectBitmap_Height As Integer
    Private DirectBitMap_Width As Integer
    Private DirectBitMap_Disposed As Boolean

    ''' <summary>
    ''' Bitmap object
    ''' </summary>
    ''' <returns>Bitmap</returns>
    Public Property Bitmap As Bitmap
        Get
            Return DirectBitmap_BitMap
        End Get
        Set(value As Bitmap)
            DirectBitmap_BitMap = value
        End Set
    End Property

    ''' <summary>
    ''' Bits
    ''' </summary>
    ''' <returns>Integer()</returns>
    Public Property Bits As Integer()
        Get
            Return DirectBitMap_Bits
        End Get
        Set(value As Integer())
            DirectBitMap_Bits = value
        End Set
    End Property

    ''' <summary>
    ''' Disposed
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property Disposed As Boolean
        Get
            Return DirectBitMap_Disposed
        End Get
        Set(value As Boolean)
            DirectBitMap_Disposed = value
        End Set
    End Property

    ''' <summary>
    ''' Height
    ''' </summary>
    ''' <returns>Integer</returns>
    Public Property Height As Integer
        Get
            Return DirectBitmap_Height
        End Get
        Set(value As Integer)
            DirectBitmap_Height = value
        End Set
    End Property

    ''' <summary>
    ''' Width
    ''' </summary>
    ''' <returns>Integer</returns>
    Public Property Width As Integer
        Get
            Return DirectBitMap_Width

        End Get
        Set(value As Integer)
            DirectBitMap_Width = value
        End Set
    End Property

    ''' <summary>
    ''' Bitshandle
    ''' </summary>
    ''' <returns>GCHandle</returns>
    Protected Property BitsHandle As GCHandle
        Get
            Return DirectBitMap_BitsHandle
        End Get
        Set(value As GCHandle)
            DirectBitMap_BitsHandle = value
        End Set
    End Property

    ''' <summary>
    ''' Initializes new instance
    ''' </summary>
    ''' <param name="IntWidth">Width</param>
    ''' <param name="IntHeight">Height</param>
    Public Sub New(ByVal IntWidth As Integer, ByVal IntHeight As Integer)

        MyBase.New
        Me.Width = IntWidth
        Me.Height = IntHeight

        Me.Bits = New Int32(((IntWidth * IntHeight)) - 1) {}
        Me.BitsHandle = GCHandle.Alloc(Me.Bits, GCHandleType.Pinned)
        Me.Bitmap = New Bitmap(IntWidth, IntHeight, (IntWidth * 4), PixelFormat.Format32bppPArgb, Me.BitsHandle.AddrOfPinnedObject)

    End Sub

    ''' <summary>
    ''' Initializes new instance
    ''' </summary>
    ''' <param name="ObjBitMap">Bitmap</param>
    Public Sub New(ByVal ObjBitMap As Bitmap)

        MyBase.New
        Me.Width = ObjBitMap.Width
        Me.Height = ObjBitMap.Height

        Me.Bits = New Int32(((ObjBitMap.Width * ObjBitMap.Height)) - 1) {}
        Me.BitsHandle = GCHandle.Alloc(Me.Bits, GCHandleType.Pinned)
        Me.Bitmap = New Bitmap(ObjBitMap.Width, ObjBitMap.Height, (ObjBitMap.Width * 4), PixelFormat.Format32bppPArgb, Me.BitsHandle.AddrOfPinnedObject)

        ' Bulk-copy pixels via LockBits instead of GetPixel()/SetPixel() per pixel.
        ' GetPixel() re-validates/re-locks the whole bitmap on every single call, which used to make
        ' importing larger PNGs (e.g. 512x512 canvases) drastically slower than necessary.
        ' Source pixels are normalized to 32bppArgb first, since imported PNGs can arrive in a variety
        ' of pixel formats (indexed, 24bpp, etc.) that can't be copied directly.
        Dim BlnClonedSource As Boolean = (ObjBitMap.PixelFormat <> PixelFormat.Format32bppArgb)
        Dim BmpSourceArgb As Bitmap = ObjBitMap
        If BlnClonedSource = True Then
            BmpSourceArgb = ObjBitMap.Clone(New Rectangle(0, 0, ObjBitMap.Width, ObjBitMap.Height), PixelFormat.Format32bppArgb)
        End If

        Try
            Dim BmpData As BitmapData = BmpSourceArgb.LockBits(
                New Rectangle(0, 0, BmpSourceArgb.Width, BmpSourceArgb.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb)

            Try
                If BmpData.Stride = BmpSourceArgb.Width * 4 Then
                    ' No row padding: the whole buffer is contiguous, copy it in one go.
                    Marshal.Copy(BmpData.Scan0, Me.Bits, 0, BmpSourceArgb.Width * BmpSourceArgb.Height)
                Else
                    ' Row padding present (stride > width * 4): copy row by row.
                    Dim IntY As Integer
                    For IntY = 0 To (BmpSourceArgb.Height - 1)
                        Dim PtrRow As IntPtr = IntPtr.Add(BmpData.Scan0, IntY * BmpData.Stride)
                        Marshal.Copy(PtrRow, Me.Bits, IntY * BmpSourceArgb.Width, BmpSourceArgb.Width)
                    Next

                End If
            Finally
                BmpSourceArgb.UnlockBits(BmpData)
            End Try
        Finally
            If BlnClonedSource = True Then
                BmpSourceArgb.Dispose()
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sets pixel on ClsDirectBitmap
    ''' </summary>
    ''' <param name="IntX">X Integer</param>
    ''' <param name="IntY">Y Integer</param>
    ''' <param name="ObjColor">Color</param>
    Public Sub SetPixel(ByVal IntX As Integer, ByVal IntY As Integer, ByVal ObjColor As Color)
        Dim IntIndex As Integer = (IntX + (IntY * Me.Width))
        Dim IntCol As Integer = ObjColor.ToArgb
        Me.Bits(IntIndex) = IntCol
    End Sub

    ''' <summary>
    ''' Sets pixel on ClsDirectBitmap
    ''' </summary>
    ''' <param name="IntX">X Integer</param>
    ''' <param name="IntY">Y Integer</param>
    ''' <returns>Color</returns>
    Public Function GetPixel(ByVal IntX As Integer, ByVal IntY As Integer) As Color
        Dim IntIndex As Integer = (IntX + (IntY * Me.Width))
        Dim IntCol As Integer = Me.Bits(IntIndex)
        Dim ObjColor As Color = Color.FromArgb(IntCol)
        Return ObjColor
    End Function

    ''' <summary>
    ''' Disposes object
    ''' </summary>
    Sub Dispose() Implements IDisposable.Dispose

        If Me.Disposed = True Then
            Return
        End If

        Me.Disposed = True
        Me.Bitmap.Dispose()
        Me.BitsHandle.Free()
    End Sub
End Class