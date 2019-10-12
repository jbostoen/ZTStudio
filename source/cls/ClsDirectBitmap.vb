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

        ' Stupid workaround for now; can this be done more efficiently?
        ' Only for loading PNGs. Better method to use this here is greatly appreciated! (contribute a pull request)
        Dim IntX As Integer
        Dim IntY As Integer

        For IntY = 0 To (ObjBitMap.Height - 1)
            IntX = 0 'reset every loop
            For IntX = 0 To (ObjBitMap.Width - 1)
                Me.SetPixel(IntX, IntY, ObjBitMap.GetPixel(IntX, IntY))
            Next intx
        Next inty

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