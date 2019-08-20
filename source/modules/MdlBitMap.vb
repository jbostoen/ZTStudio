''' <summary>
''' Contains functions related to bitmap operations
''' </summary>
Module MdlBitMap


    ''' <summary>
    ''' Combines two images into one
    ''' </summary>
    ''' <param name="ImgBack">Image background</param>
    ''' <param name="ImgFront">Image on top</param>
    ''' <returns>Image (Bitmap)</returns>
    Public Function CombineImages(ByVal ImgBack As Image, ByVal ImgFront As Image) As Image
        'this can now combine images of any size and will center them on each other

        Dim IntMaxWidth As Integer = Math.Max(ImgBack.Width, ImgFront.Width)
        Dim IntMaxHeight As Integer = Math.Max(ImgBack.Height, ImgFront.Height)

        Dim BmpOutput As New Bitmap(IntMaxWidth, IntMaxHeight)
        Dim ObjGraphic As Graphics = Graphics.FromImage(BmpOutput)

        ObjGraphic.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor ' Prevent softening
        ObjGraphic.DrawImage(ImgBack, CInt((IntMaxWidth - ImgBack.Width) / 2), CInt((IntMaxHeight - ImgBack.Height) / 2), ImgBack.Width, ImgBack.Height)
        ObjGraphic.DrawImage(ImgFront, CInt((IntMaxWidth - ImgFront.Width) / 2), CInt((IntMaxHeight - ImgFront.Height) / 2), ImgFront.Width, ImgFront.Height)
        ObjGraphic.Dispose()
        Return BmpOutput

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
        ' Find most left
        ' Find most top
        ' Find most right
        ' Find most bottom

        Dim CoordX As Integer = 0
        Dim CoordY As Integer = 0

        Dim CoordA As New Point(BmInput.Width, BmInput.Height)
        Dim CoordB As New Point(0, 0)
        Dim CurColor As System.Drawing.Color
        Dim CurTransparentColor As System.Drawing.Color = BmInput.GetPixel(0, 0)


102:
        Const px As System.Drawing.GraphicsUnit = GraphicsUnit.Pixel
        Const fmtArgb As Imaging.PixelFormat = Imaging.PixelFormat.Format32bppArgb
        Dim BoundsF As RectangleF = BmInput.GetBounds(px)
        Dim Bounds As New Rectangle(New Point(CInt(BoundsF.X), CInt(BoundsF.Y)), New Size(CInt(BoundsF.Width), CInt(BoundsF.Height)))
        Dim BmClone As Bitmap = BmInput.Clone(Bounds, fmtArgb)
        Dim BmData As System.Drawing.Imaging.BitmapData = BmClone.LockBits(Bounds, Imaging.ImageLockMode.ReadWrite, BmClone.PixelFormat)
        Dim offsetToFirstPixel As IntPtr = BmData.Scan0
        Dim ByteCount As Integer = Math.Abs(BmData.Stride) * BmClone.Height
        Dim BitmapBytes(ByteCount - 1) As Byte
        System.Runtime.InteropServices.Marshal.Copy(offsetToFirstPixel, BitmapBytes, 0, ByteCount)

110:
        ' rectangle = bounds ?
        Dim StartOffset As Integer = (0 * BmData.Stride)
        Dim EndOffset As Integer = StartOffset + ((BmInput.Height) * BmData.Stride) - 1
        Dim RectLeftOffset As Integer = (0 * 4)
        Dim RectRightOffset As Integer = (0 + (BmInput.Width) * 4) - 1
        Dim X As Integer = 0
        Dim y As Integer = 0

        Debug.Print("from " & StartOffset & " to " & EndOffset & " (total bytes: " & BitmapBytes.Length & ")")
        Debug.Print("offset left from " & RectLeftOffset & " to " & RectRightOffset)

        Dim pixelLocation As Point

251:
        For FirstOffsetInEachLine As Integer = StartOffset To EndOffset Step BmData.Stride
            X = 0
            For PixelOffset As Integer = RectLeftOffset To RectRightOffset Step 4 ' 4 because there are 4 bytes for the color: Blue, Green, Red, Alpha

                pixelLocation = New Point(X, y)
                'bitmapBytes(FirstOffsetInEachLine + PixelOffset) = FillColor.B
                'bitmapBytes(FirstOffsetInEachLine + PixelOffset + 1) = FillColor.G
                'bitmapBytes(FirstOffsetInEachLine + PixelOffset + 2) = FillColor.R
                'bitmapBytes(FirstOffsetInEachLine + PixelOffset + 3) = FillColor.A

                'If X > 510 And y > 510 Then
                '    Debug.Print(bitmapBytes(FirstOffsetInEachLine + PixelOffset + 3))
                'End If

                ' Non-transparent
                If BitmapBytes(FirstOffsetInEachLine + PixelOffset + 3) = 255 And
                    BitmapBytes(FirstOffsetInEachLine + PixelOffset) <> CurTransparentColor.B And
                    BitmapBytes(FirstOffsetInEachLine + PixelOffset + 1) <> CurTransparentColor.G And
                    BitmapBytes(FirstOffsetInEachLine + PixelOffset + 2) <> CurTransparentColor.R Then

                    ' Detected a non-transparent color
                    If X < CoordA.X Then CoordA.X = X ' Topleft: move to left
                    If y < CoordA.Y Then CoordA.Y = y ' Topleft: move to top

                    If y > CoordB.Y Then CoordB.Y = y ' Bottomright: move to bottom 
                    If X > CoordB.X Then CoordB.X = X ' Bottomright: move to right

                End If

                X += 1
            Next
            y += 1
        Next

        ' Unlock
        System.Runtime.InteropServices.Marshal.Copy(BitmapBytes, 0, offsetToFirstPixel, ByteCount)
        BmClone.UnlockBits(BmData)

901:
        ' The width/height are +1.
        CoordB.X += 1
        CoordB.Y += 1

999:
        ' 20170512 
        ' HENDRIX found out that completely transparent frames can cause issues.
        ' This is a simple fix: it seems that a 1x1 frame is valid in ZT1, even if it's transparent.
        If CoordA.X = BmInput.Width And CoordA.Y = BmInput.Height Then
            CoordA = New Point(0, 0)
            CoordB = New Point(1, 1)
        End If

        Return New Rectangle(CoordA.X, CoordA.Y, CoordB.X - CoordA.X, CoordB.Y - CoordA.Y)

        Exit Function

dBug:
        MsgBox("Unexpected error occurred in ClsTasks:Bitmap_GetDefiningRectangle() at line " & Information.Erl() & vbCrLf &
               Err.Number & " - " & Err.Description & vbCrLf &
            "Last processed: " & pixelLocation.X & " - " & pixelLocation.Y,
            vbOKOnly + vbCritical, "Critical error")

    End Function


    ''' <summary>
    ''' Deprecated! Previous version of determining the relevant rectangle.
    ''' Only left here in case a regression is spotted.
    ''' </summary>
    ''' <param name="bmInput">Bitmap image</param>
    ''' <returns>Rectangle</returns>
    Function Bitmap_GetDefiningRectangle_old(bmInput As Bitmap) As Rectangle

        On Error GoTo dBug


        ' Todo: this must be easier to go through. 
        ' It doesn't make sense to search through all pixels from left to right, top to bottom. 

101:
        ' Find most left
        ' Find most top
        ' Find most right
        ' Find most bottom

        Dim CoordX As Integer = 0
        Dim CoordY As Integer = 0

        Dim CoordA As New Point(bmInput.Width, bmInput.Height)
        Dim CoordB As New Point(0, 0)
        Dim CurColor As System.Drawing.Color
        Dim CurTransparentColor As System.Drawing.Color = bmInput.GetPixel(0, 0)

        ' Optimized by HENDRIX
        ' I like the new rectangle code, seems to be a good speedup!

        ' However, I think  "If coordX >= coordA.X And coordX < coordB.X Then" can never be true. It works for Y. If we split it into
        ' two loops, we can make use Of that condition. First Loop As it Is, getting left, top and bottom.
        ' Second loop would swap While coordX and While coordY from the first loop, and only check the area that the other loop had not
        ' covered to get the right coord.

        ' I had this idea before reading through your new code:
        ' how about we split the while loops into four ones And break out of each as soon as we find a non-transparent pixel?
        ' go over each line from the top -> get coordA.Y
        ' go over each column from the left -> get coordA.X
        ' go over each line from the bottom -> get coordB.Y
        ' go over each column from the right -> get coordB.X
        ' that would speed things up in cases where there is relatively little padding on each side, but a large defining rectangle

        ' My new idea is a hybrid of that and your new code, and it runs a tiny bit faster and produces better results

        'first, crop away stuff from top and bottom
        ' Left to right 
        While CoordX <= (bmInput.Width - 1)

            ' Top to bottom
            CoordY = 0
            While CoordY <= (bmInput.Height - 1)

                ' Get color
                CurColor = bmInput.GetPixel(CoordX, CoordY)

                If CurColor <> CurTransparentColor And CurColor.A = 255 Then
                    ' Color pixel

                    'in this iteration it makes sense to check the other three
                    If CoordX < CoordA.X Then CoordA.X = CoordX ' Topleft: move to left
                    If CoordY < CoordA.Y Then CoordA.Y = CoordY ' Topleft: move to top

                    'test is pointless, because coordX is always at least coordB.X+1
                    CoordB.X = CoordX ' Bottomright: move to right
                    If CoordY > CoordB.Y Then CoordB.Y = CoordY ' Bottomright: move to bottom

                End If

                ' If the current pixel is larger than a.Y and smaller than b.Y, we should skip.
                ' It's a bit late so I'm not thinking straight, this might be a pixel off. 
                If CoordY >= CoordA.Y And CoordY < CoordB.Y Then
                    CoordY = CoordB.Y
                Else
                    ' Default 
                    CoordY += 1
                End If

            End While

            CoordX += 1

        End While


200:
        ' MsgBox("w,h=" & coordA.X & "," & coordA.Y & " --- " & coordB.X & "," & coordB.Y)
        ' then crop away stuff from right
        ' but only the area we have not yet processed
        CoordY = CoordA.Y
        ' Top to bottom
        While CoordY <= (CoordB.Y)

            ' Right to left 
            CoordX = bmInput.Width - 1
            While CoordX > CoordB.X

                ' Get color
                CurColor = bmInput.GetPixel(CoordX, CoordY)

                If CurColor <> CurTransparentColor And CurColor.A = 255 Then
                    ' Color pixel
                    If CoordX > CoordB.X Then CoordB.X = CoordX ' Bottomright: move to right

                End If
                'I don't think we need any test here, do we?
                CoordX -= 1

            End While

            CoordY += 1

        End While

901:
        'MsgBox("w,h=" & coordA.X & "," & coordA.Y & " --- " & coordB.X & "," & coordB.Y)
        ' enabled for cropping of frames, 20150619
        CoordB.X += 1
        CoordB.Y += 1


999:
        ' 20170512 
        ' HENDRIX found out that transparent frames can cause issues.
        ' This is a more simple fix, since it seems this is valid in ZT1 after all?
        If CoordA.X = bmInput.Width And CoordA.Y = bmInput.Height Then
            CoordA = New Point(0, 0)
            CoordB = New Point(1, 1)
        End If

        'Debug.Print("x1,y1=" & coordA.X & "," & coordA.Y & " --- x2,y2 " & coordB.X & "," & coordB.Y)
        Return New Rectangle(CoordA.X, CoordA.Y, CoordB.X - CoordA.X, CoordB.Y - CoordA.Y)


        Exit Function

dBug:
        MsgBox("Error while obtaining the 'defining rectangle' of this graphic." & vbCrLf &
            "Erl " & Erl() & " - " & Err.Number & " - " & Err.Description,
            vbOKOnly + vbCritical, "Critical error")

    End Function


End Module
