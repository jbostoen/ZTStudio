''' <summary>
''' Contains functions related to bitmap operations
''' </summary>
Module MdlBitMap


    ''' <summary>
    ''' Combines two images into one. Always centers the images on each other, if different size. (For instance: grid + actual graphic)
    ''' </summary>
    ''' <param name="ImgBack">Image background</param>
    ''' <param name="ImgFront">Image on top</param>
    ''' <returns>Image (Bitmap)</returns>
    Public Function CombineImages(ByVal ImgBack As Image, ByVal ImgFront As Image) As Image

        On Error GoTo dBg

1:
        Dim IntMaxWidth As Integer = Math.Max(ImgBack.Width, ImgFront.Width)
        Dim IntMaxHeight As Integer = Math.Max(ImgBack.Height, ImgFront.Height)

        MdlZTStudio.Trace("MdlBitMap", "CombineImages", "Background w1 = " & ImgBack.Width & ", h1 = " & ImgBack.Height & " | Front w2 = " & ImgFront.Width & ", " & ImgFront.Height)


2:
        Debug.Print(IntMaxWidth & " - " & IntMaxHeight)
        Dim BmpOutput As New Bitmap(IntMaxWidth, IntMaxHeight)

3:
        Dim ObjGraphic As Graphics = Graphics.FromImage(BmpOutput)

11:
        ObjGraphic.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor ' Prevent softening

21:
        ObjGraphic.DrawImage(ImgBack, CInt((IntMaxWidth - ImgBack.Width) / 2), CInt((IntMaxHeight - ImgBack.Height) / 2), ImgBack.Width, ImgBack.Height)

31:
        ObjGraphic.DrawImage(ImgFront, CInt((IntMaxWidth - ImgFront.Width) / 2), CInt((IntMaxHeight - ImgFront.Height) / 2), ImgFront.Width, ImgFront.Height)

41:
        ObjGraphic.Dispose()

        Return BmpOutput

dBg:
        MdlZTStudio.UnhandledError("MdlBitMap", "CombineImages", Information.Err)

    End Function

    ''' <summary>
    ''' Draws an isometric grid (squares).
    ''' </summary>
    ''' <param name="IntFootPrintX">Sets the amount of squares (X)</param>
    ''' <param name="IntFootPrintY">Sets the amount of squares (Y)</param>
    ''' <returns>Bitmap of a grid</returns>
    ''' <remarks>
    ''' Could be simplified: IntFootPrintX and IntFootPrintY are always simply the config parameters up till now.
    ''' However, for future use, do not change this.
    ''' </remarks>
    Function DrawGridFootPrintXY(IntFootPrintX As Integer, IntFootPrintY As Integer) As Bitmap

        ' Draws a certain amount of squares.
        ' ZT1 uses either 1/4th of a square, or complete squares from there on. 
        ' Anything else doesn't seem to be too reliable!

        ' This function calculates where to put the center of the image.
        ' View:
        ' 0 = SE
        ' 1 = SW
        ' 2 = NE
        ' 3 = NW

        ' SE, cFootPrintX = 10, cFootPrintY = 8 ---> Front view of an object, 5 squares. Side: 4 squares.

        ' Draw bitmap with squares first.
        ' To do so, calculate the top left pixel of the center of the grid.
        Dim IntWidth As Integer = (IntFootPrintX + IntFootPrintY) * 16
        Dim IntHeight As Integer = IntWidth / 2

        ' Every grid square adds this much for X and Y - consider both directions to be efficient!
        Dim x_dim As Integer = IntFootPrintX * 16 + IntFootPrintY * 16
        Dim y_dim As Integer = IntFootPrintY * 8 + IntFootPrintX * 8
        Dim BmInput As New Bitmap(x_dim * 2, y_dim * 2)

        ' Iirst point of the generated grid: intFootprintX * 32, +16px Y (center), 

        ' Find the center of the center of the generated grid bitmap.
        ' Next, align it with the center of the image.

        ' Starting info: coordinate, number of squares, even or odd (= extra row) 
        ' Keep track of how many squares are drawn
        ' Do not draw more squares than the max width

        Dim Coord As New Point((IntFootPrintX / 2) * 32, 0)

        ' Think with X=10,Y=8
        Dim IntCurFootPrintX As Integer
        Dim IntCurFootPrintY As Integer

        For IntCurFootPrintX = 2 To IntFootPrintX Step 2

            ' Starting point:
            Coord.X = x_dim - (IntWidth / 2) + (IntFootPrintX / 2 * 32)

            Coord.X = x_dim - (IntWidth / 2)  ' Move to the left
            Coord.X += ((IntFootPrintX - IntCurFootPrintX) / 2) * 32  ' What can we add?

            Coord.Y = y_dim - (IntHeight / 2) + 16 * (IntCurFootPrintX / 2)
            Coord.Y -= 16

            ' Draw the first square, which is easy.
            ' Footprints step by 2
            For IntCurFootPrintY = 2 To IntFootPrintY Step 2

                ' For each
                Coord.X += 32
                Coord.Y += 16

                MdlBitMap.DrawGridSquare(Coord, BmInput)

            Next

        Next

        Return BmInput

    End Function

    ''' <summary>
    ''' Draws a square (for a grid)
    ''' </summary>
    ''' <param name="CoordTopLeft">The top left coordinate</param>
    ''' <param name="BmInput">The bitmap to drawn on. If not specified</param>
    ''' <returns>Bitmap of a full square</returns>
    Function DrawGridSquare(CoordTopLeft As Point, Optional BmInput As Bitmap = Nothing) As Bitmap

        ' Todo: replace SetPixel()?

        If IsNothing(BmInput) = True Then
            BmInput = MdlSettings.BMEmpty
        End If

        Dim IntX As Integer
        Dim IntY As Integer = 0

        ' === Top left
        For IntX = -31 To 0

            BmInput.SetPixel(CoordTopLeft.X + IntX, CoordTopLeft.Y + IntY, Cfg_grid_ForeGroundColor)

            ' Mirror to the right
            BmInput.SetPixel(CoordTopLeft.X + 1 - IntX, CoordTopLeft.Y + IntY, Cfg_grid_ForeGroundColor)

            ' Mirror bottom part for bottom
            BmInput.SetPixel(CoordTopLeft.X + IntX, CoordTopLeft.Y - IntY + 1, Cfg_grid_ForeGroundColor)
            BmInput.SetPixel(CoordTopLeft.X + 1 - IntX, CoordTopLeft.Y - IntY + 1, Cfg_grid_ForeGroundColor)

            ' Width = 32; height = 16
            ' This means the height decreases slower
            If IntX Mod 2 = 0 Then
                IntY -= 1
            End If

        Next

        ' Center consists of 4px
        BmInput.SetPixel(CoordTopLeft.X, CoordTopLeft.Y, Cfg_grid_ForeGroundColor)
        BmInput.SetPixel(CoordTopLeft.X, CoordTopLeft.Y + 1, Cfg_grid_ForeGroundColor)
        BmInput.SetPixel(CoordTopLeft.X + 1, CoordTopLeft.Y, Cfg_grid_ForeGroundColor)
        BmInput.SetPixel(CoordTopLeft.X + 1, CoordTopLeft.Y + 1, Cfg_grid_ForeGroundColor)

        'picBox.Image = bmInput

        Return BmInput

    End Function



    ''' <summary>
    ''' Returns a cropped version of the given bitmap
    ''' </summary>
    ''' <param name="BmInput">Bitmap image</param>
    ''' <param name="RectCropArea">Rectangle used to crop the bitmap</param>
    ''' <returns>Bitmap</returns>
    Function GetCroppedVersion(BmInput As Bitmap, RectCropArea As Rectangle) As Bitmap

        Return BmInput.Clone(RectCropArea, BmInput.PixelFormat)

    End Function

    ''' <summary>
    ''' Returns the defining rectangle for this bitmap.
    ''' This means the rectangle which contains all colored (non-transparent) pixels
    ''' </summary>
    ''' <param name="BmInput">Bitmap image</param>
    ''' <returns>Rectangle - dimensions of relevant part</returns>
    Function GetDefiningRectangle(BmInput As Bitmap) As Rectangle

        On Error GoTo dBug

        ' This new method using LockBits is much faster than a previous version where GetPixel() was used. 
        ' This is a big performance boost when loading 512x512 (canvas size) images.
        ' For now, the old function still exists to make sure regressions can be detected.

101:
        ' Find most top/left
        ' Find most bottom/right

        Dim ObjCoordTopLeft As New Point(BmInput.Width, BmInput.Height)
        Dim ObjCoordBottomRight As New Point(0, 0)
        Dim ObjCurrentTransparentColor As System.Drawing.Color = BmInput.GetPixel(0, 0)


102:
        Dim BoundsF As RectangleF = BmInput.GetBounds(GraphicsUnit.Pixel)
        Dim Bounds As New Rectangle(New Point(CInt(BoundsF.X), CInt(BoundsF.Y)), New Size(CInt(BoundsF.Width), CInt(BoundsF.Height)))
        Dim ObjBitmapClone As Bitmap = BmInput.Clone(Bounds, Imaging.PixelFormat.Format32bppArgb)
        Dim ObjBitmapData As System.Drawing.Imaging.BitmapData = ObjBitmapClone.LockBits(Bounds, Imaging.ImageLockMode.ReadWrite, ObjBitmapClone.PixelFormat)
        Dim offsetToFirstPixel As IntPtr = ObjBitmapData.Scan0
        Dim IntByteCount As Integer = Math.Abs(ObjBitmapData.Stride) * ObjBitmapClone.Height
        Dim BitmapBytes(IntByteCount - 1) As Byte
        System.Runtime.InteropServices.Marshal.Copy(offsetToFirstPixel, BitmapBytes, 0, IntByteCount)

110:
        ' rectangle = bounds ?
        Dim IntStartOffset As Integer = (0 * ObjBitmapData.Stride)
        Dim IntEndOffset As Integer = IntStartOffset + ((BmInput.Height) * ObjBitmapData.Stride) - 1
        Dim IntRectLeftOffset As Integer = (0 * 4)
        Dim IntRectRightOffset As Integer = (0 + (BmInput.Width) * 4) - 1
        Dim IntX As Integer = 0
        Dim IntY As Integer = 0


251:
        For FirstOffsetInEachLine As Integer = IntStartOffset To IntEndOffset Step ObjBitmapData.Stride
            IntX = 0
            For PixelOffset As Integer = IntRectLeftOffset To IntRectRightOffset Step 4 ' 4 because there are 4 bytes for the color: Blue, Green, Red, Alpha

                ' Non-transparent
                If BitmapBytes(FirstOffsetInEachLine + PixelOffset + 3) = 255 And
                    BitmapBytes(FirstOffsetInEachLine + PixelOffset) <> ObjCurrentTransparentColor.B And
                    BitmapBytes(FirstOffsetInEachLine + PixelOffset + 1) <> ObjCurrentTransparentColor.G And
                    BitmapBytes(FirstOffsetInEachLine + PixelOffset + 2) <> ObjCurrentTransparentColor.R Then

                    ' Detected a non-transparent color
                    If IntX < ObjCoordTopLeft.X Then ObjCoordTopLeft.X = IntX ' Topleft: move to left
                    If IntY < ObjCoordTopLeft.Y Then ObjCoordTopLeft.Y = IntY ' Topleft: move to top

                    If IntY > ObjCoordBottomRight.Y Then ObjCoordBottomRight.Y = IntY ' Bottomright: move to bottom 
                    If IntX > ObjCoordBottomRight.X Then ObjCoordBottomRight.X = IntX ' Bottomright: move to right

                End If

                IntX += 1
            Next
            IntY += 1
        Next

        ' Unlock
        System.Runtime.InteropServices.Marshal.Copy(BitmapBytes, 0, offsetToFirstPixel, IntByteCount)
        ObjBitmapClone.UnlockBits(ObjBitmapData)

901:
        ' The width/height are +1.
        ObjCoordBottomRight.X += 1
        ObjCoordBottomRight.Y += 1

999:
        ' 20170512 
        ' HENDRIX found out that completely transparent frames can cause issues.
        ' This is a simple fix: it seems that a 1x1 frame is valid in ZT1, even if it's transparent.
        If ObjCoordTopLeft.X = BmInput.Width And ObjCoordTopLeft.Y = BmInput.Height Then
            ObjCoordTopLeft = New Point(0, 0)
            objCoordBottomRight = New Point(1, 1)
        End If

        Return New Rectangle(ObjCoordTopLeft.X, ObjCoordTopLeft.Y, objCoordBottomRight.X - ObjCoordTopLeft.X, objCoordBottomRight.Y - ObjCoordTopLeft.Y)

        Exit Function

dBug:
        MdlZTStudio.UnhandledError("MdlBitMap", "GetDefiningRectangle", Information.Err)

    End Function




End Module
