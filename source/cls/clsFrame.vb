Option Explicit On

Imports System.ComponentModel

Public Class clsFrame

    Implements INotifyPropertyChanged


    Private fr_Hex As New List(Of String)

    Private fr_width As Integer = -1
    Private fr_height As Integer = -1
    Private fr_offsetX As Integer = -9999
    Private fr_offsetY As Integer = -9999

    Private fr_parent As New clsGraphic2
    Private fr_type As Integer = 0
    ' 0 = basic
    ' 1 = ZTAF, no extra frame
    ' 2 = ZTAF, extra frame
    ' 3 = ZTAF, shadows MM

    Private fr_MysteryHEX As New List(Of String)

    Private fr_cachedFrame As Bitmap
    Private fr_lastUpdated As String = Now.ToString("yyyyMMddHHmmss")                ' for caching purposes.

    Private fr_ex_numBlocks As Integer = 0
    Private fr_ex_numPixels As Integer = 0

    Private fr_cachedFrameHexVisual As New List(Of String)

    ' === Exp

    Public Property ex_numBlocks As Integer
        Get
            Return fr_ex_numBlocks
        End Get
        Set(value As Integer)
            fr_ex_numBlocks = value
        End Set
    End Property

    Public Property ex_numPixels As Integer
        Get
            Return fr_ex_numPixels
        End Get
        Set(value As Integer)
            fr_ex_numPixels = value
        End Set
    End Property




    ' === Regular

    Public Property hexString As List(Of String)
        ' What is the hex code for this frame? (if generated)
        Get
            Return fr_Hex
        End Get
        Set(value As List(Of String))
            fr_Hex = value
            NotifyPropertyChanged("hexString")
        End Set
    End Property




    Public Property parent As clsGraphic2
        ' What is the parent of our frame? (which ZT1 Graphic does this frame belong to?)
        Get
            Return fr_parent
        End Get
        Set(value As clsGraphic2)
            fr_parent = value
            NotifyPropertyChanged("parent")
        End Set
    End Property


    Public Property height As Integer
        ' What is the height of our frame?
        Get
            Return fr_height
        End Get
        Set(value As Integer)
            fr_height = value
            NotifyPropertyChanged("height")
        End Set

    End Property
    Public Property width As Integer
        ' What is the width of our frame?
        Get
            Return fr_width
        End Get
        Set(value As Integer)
            fr_width = value
            NotifyPropertyChanged("width")
        End Set
    End Property
    Public Property offsetX As Integer
        ' The horizontal offset. How much should the image be moved to the left compared to the center of the square (based on the ZT1 cFootPrintX) ?
        Get
            Return fr_offsetX
        End Get
        Set(value As Integer)
            fr_offsetX = value
            NotifyPropertyChanged("offsetX")
        End Set
    End Property
    Public Property offsetY As Integer
        ' The vertical offset. How much should the image be moved to the top compared to the center of the square (based on the ZT1 cFootPrintX) ?
        Get
            Return fr_offsetY
        End Get
        Set(value As Integer)
            fr_offsetY = value
            NotifyPropertyChanged("offsetY")
        End Set
    End Property

    Public Property cachedFrame As Bitmap
        ' This property is used to store a cached version of a frame of a ZT1 Graphic.
        ' If nothing changed, this is a major performance boost.
        Get
            Return fr_cachedFrame
        End Get
        Set(value As Bitmap)
            fr_cachedFrame = value
            NotifyPropertyChanged("cachedFrame")
        End Set
    End Property

    Public Property cachedFrameHexVisual As List(Of String)
        ' When the visual component is read [this happens when a PNG is loaded OR when a frame is read from a ZT1 Graphic.
        ' Height/width won't change, nor will the graphic itself. Offsets MAY change though!
        ' This should only be updated/cleared if the color palette has been changed as well in the meanwhile.
        ' Minor problem: since the color palette is part of the ZT1 Graphic and not the frame, 
        ' it will have to be cleared on that level.
        Get
            Return fr_cachedFrameHexVisual
        End Get
        Set(value As List(Of String))
            fr_cachedFrameHexVisual = value

        End Set
    End Property

    Public Property lastUpdated As String
        ' Used to see if re-rendering is needed.
        Get
            Return fr_lastUpdated
        End Get
        Set(value As String)
            fr_lastUpdated = value
            'NotifyPropertyChanged("lastUpdated")
        End Set
    End Property

    ' For experiments, stats
    Public Property mysteryHEX As List(Of String)
        ' This is used to store our currently 2 unknown bytes. We call them our mystery bytes.
        Get
            Return fr_MysteryHEX
        End Get
        Set(value As List(Of String))
            fr_MysteryHEX = value
            NotifyPropertyChanged("mysteryHEX")
        End Set
    End Property


    Function getImage(Optional blnDrawGrid As Boolean = False)

        ' GetImage will return the file as a bitmap/image.
        ' It will render the frame; and then add backgrounds.
        ' There's an option to render the image on top of a visible grid (as you have in ZT1).

        On Error GoTo dBug

1:
        ' Draw frame.
        Dim bmOutput As Bitmap = Me.renderFrame()

11:
        ' Draw 'extra' background frame, e.g. restaurants?
        If Me.parent.extraFrame = 1 And cfg_export_PNG_RenderBGFrame = 1 Then
            bmOutput = clsTasks.images_Combine(Me.parent.frames(Me.parent.frames.Count - 1).renderFrame(), bmOutput)
        End If

21: 
        ' Optional background ZT1 Graphic frame, e.g. animal + toy?
        If editorBgGraphic.frames.Count > 0 And cfg_export_PNG_RenderBGZT1 = 1 Then 
            bmOutput = clsTasks.images_Combine(editorBgGraphic.frames(0).renderFrame(), bmOutput)
        End If

31:
        ' Draw grid?
        If blnDrawGrid = True Then
            bmOutput = clsTasks.images_Combine(clsTasks.grid_drawFootPrintXY(cfg_grid_footPrintX, cfg_grid_footPrintY, 0), bmOutput)
        End If



41:
        Return bmOutput


        Exit Function

dBug:

        MsgBox("Error in displayFrame at line " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error")


    End Function

    Function renderFrame(Optional bmOutput As Bitmap = Nothing, Optional ztPal As clsPalette = Nothing, Optional cacheLoad As Boolean = True, Optional cacheStore As Boolean = True) As Bitmap

        Debug.Print("clsFrame: Render frame. " & Me.parent.frames.IndexOf(Me))

        ' Cached? 
        If IsNothing(Me.parent) = False Then
            If Me.lastUpdated = Me.parent.lastUpdated And IsNothing(Me.cachedFrame) = False And cacheLoad = True Then
                ' We already rendered this frame. It hasn't been changed, and no re-render is forced.
                ' Return a cached version.
                Return Me.cachedFrame
            Else
                ' Either the frame has been updated or a re-render is forced.
                ' Do not return a cached version, render from scratch.
            End If
        Else
            ' Cached version of the image is not available. 
            ' We will need to render the image from scratch.
        End If

        'Debug.Print("clsFrame: Not relying on a cached frame.")


        ' Color palette
        ' No (optional) color palette has been specified. Use the one from our parent (ZT1 Graphic).
        If IsNothing(ztPal) = True Then
            ztPal = Me.parent.colorPalette
        End If

        ' Bitmap 
        If IsNothing(bmOutput) = True Then

            ' We are creating a new bitmap
            bmOutput = New Bitmap(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2)

            ' This should fill our canvas
            'Dim g As Graphics = Graphics.FromImage(bmOutput)
            'g.Clear(cfg_grid_BackGroundColor)
            ' we miss our grid here.

        Else
            ' We are drawing on top of an existing bitmap
        End If



        ' There's only 1 color (=transparent) or none at all.
        ' Don't bother rendering. Return the input background image or an empty canvas.
        If (Me.parent.colorPalette.colors.Count <= 1) Then
            Debug.Print("clsFrame.renderFrame: <= 1 color in the color palette.")
            Return bmOutput
        End If



        ' There's no cached image, no hex previously read from a ZT1 Graphic
        If IsNothing(Me.cachedFrame) = True And Me.hexString.Count = 0 Then
            Debug.Print("clsFrame.renderFrame: no cached frame (" & IsNothing(Me.cachedFrame) & ") nor hex available.")
            Return bmOutput
        End If


        'Debug.Print("... renderFrame - render [again].| w x h = " & bmOutput.Width & " x " & bmOutput.Height & " | " & ztPal.fileName)


        Dim intX As Integer = 0
        Dim intY As Integer = 0

        Dim coord As Point


        Dim num_colorsExpected As Integer = 0
        Dim num_currentColor As Integer = 0

        Dim num_pixelSets As Integer = 0
        Dim num_currentPixelSet As Integer = 0

        Dim c As System.Drawing.Color


        On Error GoTo dBug
        'Debug.Print("'" & strFrameHex & "'")

1:
        Dim hex As List(Of String) = Me.hexString
        Dim curByte As Integer = 0

        'Debug.Print(hex.Length)

2:
        ' If our view has an extra frame at the end, usually containing the biggest part of the animation:
        ' And only if this is not in fact the last part of the animation:
        'If Me.parent.extraFrame = 1 And Me.parent.frames.IndexOf(Me) <> (Me.parent.frames.Count - 1) And blnRenderBGFrame = True Then
        'Debug.Print("Extra frame, renderBG = true, cfg: " & cfg_export_PNG_RenderBGFrame)
        'bmOutput = New Bitmap(Me.parent.frames(Me.parent.frames.Count - 1).renderFrame(bmOutput, ztPal))
        'End If


11:
        If Me.height = -1 Then Me.height = CInt("&H" & hex(1) & hex(0))
        If Me.width = -1 Then Me.width = CInt("&H" & hex(3) & hex(2))

12:

        ' Special "ssurf"-animations
        If hex(1) = "80" Then
            Return renderFrame2(bmOutput, ztPal)
            Exit Function
        End If



        ' In case of unknown offsets and HEX = FF (large size image)
        If Me.offsetY = -9999 And hex(5) = "FF" Then
            Me.offsetY = ((256 * 256) - CInt("&H" & hex(5) & hex(4))) * -1
            'debug.Print("Offset: change because offsetY is nothing and hex = FF. Changed to " & Me.offsetY)

        End If

        ' In case of unknown offsets and HEX = FF (large size image)
        If Me.offsetX = -9999 And hex(7) = "FF" Then
            Me.offsetX = ((256 * 256) - CInt("&H" & hex(7) & hex(6))) * -1
            'Debug.Print("Offset: change because offsetX is nothing and hex = FF. Changed to " & Me.offsetX)
        End If

        ' In case of unknown offsets and normal HEX
        If Me.offsetY = -9999 Then
            Me.offsetY = CInt("&H" & hex(5) & hex(4))
            'Debug.Print("Offset: change because offsetY is nothing. Changed to " & Me.offsetY & " -> " & hex(5) & " " & hex(4))

        End If
        If Me.offsetX = -9999 Then
            Me.offsetX = CInt("&H" & hex(7) & hex(6))
            'Debug.Print("Offset: change because offsetX is nothing. Changed to " & Me.offsetX & " -> " & hex(7) & " " & hex(6))
        End If


        ' MsgBox("offset is now " & Me.offsetX & " , " & Me.offsetY)



20:


21:
        With fr_MysteryHEX
            .Clear(False)
            .Add(hex(8), False)
            .Add(hex(9), False)
        End With

        ' Remove first 10 bytes. 2 height, 2 width, 2 offset Y, 2 offset X, 2 mystery bites
22:
        hex.Skip(10)
         

        'Debug.Print("Frame length = " & Strings.Join(hex.ToArray(), "").Length / 2)
        'Debug.Print(Strings.Join(hex.ToArray(), " "))
1000:

1001:
        ' === Color instructions ===
        ' What's left, is the image part (without width, height, offset X, offsetY, mystery byte).
        Me.cachedFrameHexVisual = hex


1005:
        ' This condition should spare us from APE junk bytes...

        While hex.Count > 0 And (intY) < Me.height  'And intY < 255 'temp fix to render the flag

            ' Show what's left.
            'Debug.Print(Strings.Join(hex.ToArray(), " "))


            ' First byte for each row contains the number of pixel sets.
            ' That's at least 1 block (a transparent line would give [offset] [0 colors] -
            ' Otherwise, it's a series of pixel sets: [offset][numColorPixels][pixels]

1100:
            num_pixelSets = CInt("&H" & hex(0))    ' number of pixel sets in this line.
            hex.Skip(1)

1120:


            ' We know how many pixel sets we have in this line.
            ' Let's process them.

            For num_currentPixelSet = 0 To (num_pixelSets - 1)

                'Debug.Print("Pixel set, num " & (num_currentPixelSet + 1) & " out of " & (num_pixelSets))

1300:
                ' Starting with color byte.
                intX += CInt("&H" & hex(0))                  ' Offset
1301:
                num_colorsExpected = CInt("&H" & hex(1))     ' Number of instruction blocks

                'Debug.Print("   # colors: " & num_colorsExpected)

1310:
                ' Remove [offset] and [num of pixels to draw] instructions.
                hex.Skip(2)

1400:
                ' We know how many colors are expected.
                For num_currentColor = 0 To (num_colorsExpected - 1)

1409:
                    ' Get color index etc, draw pixel
                    'Debug.Print(num_currentColor & "/" & hex.Length & " --- " & ztPal.colors.Count & " --- " & (num_colorsExpected - 1))

1410:
                    c = ztPal.colors(CInt("&H" & hex(num_currentColor)))

1412:
                    ' Jump to the right offset.
                    coord.X = (cfg_grid_numPixels - Me.offsetX) + intX
                    coord.Y = (cfg_grid_numPixels - Me.offsetY) + intY

1413:
                    ' Color the pixel.
                    bmOutput.SetPixel(coord.X, coord.Y, c)

                    Me.ex_numPixels += 1
                    intX += 1


                Next num_currentColor
                hex.Skip(num_currentColor)  'Remove all colors above at once now.

1415:

            Next num_currentPixelSet
            'Debug.Print("Last pixel in line was X " & intX & ". Y was " & intY)

1450:

            ' Entire line (left to right) has been processed.
            ' Next, please.
            intX = 0
            intY += 1

        End While


1455:

        ' Implemented a check for APE junk bytes and remove if any are left.
        If hex.Count > 0 Then
            Debug.Print("Cleaning up APE junk bytes: " & hex.Count)
            Me.cachedFrameHexVisual.RemoveRange(Me.cachedFrameHexVisual.Count - hex.Count, hex.Count)
        End If




1500:

        If IsNothing(Me.parent) = False Then
            Me.lastUpdated = Me.parent.lastUpdated ' A bit of cheating. We need it to be identical
        End If

        'Debug.Print("Processed frame.")

        If cacheStore = True Then
            Me.cachedFrame = bmOutput
        End If


        Return bmOutput

        Debug.Print("Frame rendered.")
        Exit Function

dBug:

        frmMain.picBox.Image = bm

        Debug.Print("Debugging info:")
        ' Debug.Print("What's left of HEX() ? " & Strings.Join(hex.ToArray(), " "))
        ' Debug.Print("My index: " & Me.parent.frames.IndexOf(Me) & " - frame Length: " & Me.hexString.Length.ToString("X4"))

        MsgBox("An error occured while trying to render a frame. Line " & Erl() & vbCrLf & _
               "Width, height: " & Me.width & ", " & Me.height & vbCrLf & _
            "Offset x, y: " & Me.offsetX & ", " & Me.offsetY & vbCrLf & _
            "Colors: Currently at pixel set " & num_currentPixelSet & "/" & num_pixelSets & ", color " & num_currentColor & "/" & num_colorsExpected & vbCrLf & _
            "Last referenced x, y: " & coord.X & ", " & coord.Y & vbCrLf & _
            "Current length of hex(): " & hex.Count & vbCrLf & _
            "Current length of colors: " & Me.parent.colorPalette.colors.Count & vbCrLf & _
             vbCrLf & Err.Number & " - " & Err.Description & _
            vbCrLf & "Line: " & Erl(), vbOKOnly + vbCritical, "Error")


        On Error Resume Next

        If hex.Count > 0 Then

            Dim xDebug As Integer = 0
            Dim strDebug As Integer
            For xDebug = 0 To 15
                strDebug &= hex(xDebug) & " "
            Next
            Debug.Print("First 8 bytes left: " & strDebug)
        End If



    End Function



    Function renderFrame2(bm As Bitmap, ztPal As clsPalette, Optional blnRenderBG As Boolean = True) As Bitmap

        ' Similar, but slightly different
        ' The big mystery - unless the only reason is to be more similar to other files: why bother to even have a .PAL-file?
        ' We're only either drawing black pixels or nothing at all...


        Dim intX As Integer = 0
        Dim intY As Integer = 0 
        Dim coord As Point

        Dim intInstructionBlocksNum As Integer = 0
        Dim intInstructionBlocksCurrent As Integer = 0
        Dim intInstructionBlackPixelsNum As Integer = 0
        Dim intinstructionBlackPixelsCurrent As Integer = 0

        Dim c As System.Drawing.Color


        On Error GoTo dBug 

1:
        Dim hex As List(Of String) = Me.hexString
        Dim curByte As Integer = 0

        Debug.Print(hex.Count)

2:
        ' If this is not the last frame, and the animation relies on a last frame:
        ' (unlikely in the case of shadows, but who knows.)
        If Me.parent.extraFrame = 1 And Me.parent.frames.IndexOf(Me) <> (Me.parent.frames.Count - 1) And blnRenderBG = True Then
            bm = Me.parent.frames(Me.parent.frames.Count - 1).renderFrame(bm, ztPal)
        End If


11:
        Me.height = CInt("&H" & hex(0))
        Me.width = CInt("&H" & hex(2))

12:



        ' FF F7   = -9,?
        ' 255 247 = -9 ?
        If Me.offsetY = -9999 And hex(5) = "FF" Then Me.offsetY = ((256 * 256) - CInt("&H" & hex(5) & hex(4))) * -1
        If Me.offsetX = -9999 And hex(7) = "FF" Then Me.offsetX = ((256 * 256) - CInt("&H" & hex(7) & hex(6))) * -1

        If Me.offsetY = -9999 Then Me.offsetY = CInt("&H" & hex(5) & hex(4))
        If Me.offsetX = -9999 Then Me.offsetX = CInt("&H" & hex(7) & hex(6))

      

20:

        ' Remove first 10 bytes.
        ' 2 x height, width, offsetY, offsetX + 2 mysterious bytes
        hex.Skip(10)


25:


1000:


        ' Rewrite the stuff below.
        ' First byte in a row signals how many blocks there are.
        ' A block consists of: [offset] [numColors] [c1] [c2] [cN] [00]
 

1001:
        ' === Color instructions ===
        ' What's left, is the image part (without width, height, offset X, offsetY, mystery byte).
        Me.cachedFrameHexVisual = hex


1005:
        ' This condition should spare us from APE junk bytes...

        While hex.Count > 0

            ' First byte of each row: how many bytes about this row?
            ' Second byte: offset for the first pixel
            ' Third byte: number of colors. This means we have a maximum width, in theory.

            c = Color.Transparent

1100:
            'Debug.Print("----------- Row " & intY & " ----------------")
            intInstructionBlocksNum = CInt("&H" & hex(0))    ' number of blocks
            hex.Skip(1)

1120:

            ' Instruction blocks. offset, pixels (black)
            For intInstructionBlocksCurrent = 0 To (intInstructionBlocksNum - 1)

1300:
                ' Starting with color byte.
                intX += CInt("&H" & hex(0))                 ' Offset
1301:
                intInstructionBlackPixelsNum = CInt("&H" & hex(1))     ' Number of colors (black)


1310:
                'hex = hex.Skip(2).ToArray()

1400:


                If intInstructionBlackPixelsNum <> 0 Then


                    For intinstructionBlackPixelsCurrent = 0 To intInstructionBlackPixelsNum

1410:
                        ' Switch between black and transparent
                        If c = Color.Transparent Then
                            ' c = Color.Black
                        Else
                            'c = Color.Transparent
                        End If
1412:
                        coord.X = (cfg_grid_numPixels - Me.offsetX) + intX   'intX
                        coord.Y = (cfg_grid_numPixels - Me.offsetY) + intY
                        'Debug.Print("       Coords: x=" & coordX & ", y=" & coordY & ", w=" & intWidth & ",h=" & intHeight & ", ox=" & offsetX & ", oy=" & offsetY)

                        bm.SetPixel(coord.X, coord.Y, Color.Black)

1413:
                        intX += 1


                    Next intinstructionBlackPixelsCurrent

                End If

                ' Remove offset, amount of pixels
                hex.Skip(2)

1415:

            Next intInstructionBlocksCurrent

1450:

            intX = 0
            intY += 1

        End While


1455:

        ' Implemented a check for APE junk bytes and remove if any are left.
        If hex.Count > 0 Then
            Debug.Print("Cleaning up APE junk bytes: " & hex.Count)
            Me.cachedFrameHexVisual.RemoveRange(Me.cachedFrameHexVisual.Count - hex.Count, hex.Count)
        End If
         



1500:

        If Not IsNothing(Me.parent) Then
            Me.lastUpdated = Me.parent.lastUpdated ' A bit of cheating. We need it to be identical
        End If

        Me.cachedFrame = bm
        Return bm


        Exit Function



dBug:
        frmMain.picBox.Image = bm

        MsgBox("An error occured while trying to render a surface-shadows frame." & vbCrLf & _
               "Width, height: " & Me.width & ", " & Me.height & vbCrLf & _
            "Offset x, y: " & Me.offsetX & ", " & Me.offsetY & vbCrLf & _
              "Last referenced x, y: " & coord.X & ", " & coord.Y & vbCrLf & _
            "Current length of hex(): " & hex.Count & vbCrLf & _
             vbCrLf & Err.Number & " - " & Err.Description & _
            vbCrLf & "Line: " & Erl(), vbOKOnly + vbCritical, "Error")


        On Error Resume Next

        If hex.Count > 0 Then

            Dim xDebug As Integer = 0
            Dim strDebug As Integer
            For xDebug = 0 To 15
                strDebug &= hex(xDebug) & " "
            Next
            Debug.Print("First 8 bytes left: " & strDebug)
        End If



    End Function






    Function getHexFromFrame(Optional bm As Bitmap = Nothing, Optional cacheLoad As Boolean = True) As List(Of String)


        ' This function will output the HEX values for a frame. 

        If IsNothing(bm) = True Then

            ' No bitmap has been specified. There's no cached frame either, or we explicitly do not want to use the cached frame.
            If IsNothing(Me.cachedFrame) Or cacheLoad = False Then

                ' Render the frame from scratch.
                bm = Me.renderFrame(New Bitmap(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2), Me.parent.colorPalette, cacheLoad).Clone()

            Else

                ' Use a cached version (performance) of this frame.
                bm = Me.cachedFrame

            End If

        Else
            ' Use the provided bitmap instead. 
        End If



        On Error GoTo dBug

        Dim opHex As New List(Of String)                            ' Main output
        Dim opHexRows As New List(Of String)                        ' Store bytes as strings for now. Actual drawing instructions.

        ' This means: height, width, offsetX, offsetY FOR EACH FRAME.
        Dim coord As Point                                          ' Our pixel reference.

        Dim c As System.Drawing.Color                               ' to go over every color


        Dim lstColorIndexes As New List(Of String)                  ' Store the HEX values, as strings, for color index in our .pal file 
        Dim lstInstrBlocks As New List(Of String)                   ' Store the HEX values of instruction blocks [offset][numColors][for each color]
        Dim intInstrBlocks As Integer = 1                           ' Keep track of the number of blocks

        ' Take the palette from the parent
        Dim ztPal As clsPalette = Me.parent.colorPalette

100:

        ' === We will read the bitmap. We need 2 rectangle defining coordinates. ===
        Dim rect As Rectangle
        'Dim rect As Rectangle = Me.parent.getDefiningRectangle()

        ' might be able to do this more efficiently?
        '  Return New Rectangle(coordA.X, coordA.Y, coordB.X - coordA.X, coordB.Y - coordA.Y)
        'If Me.height <> -1 And Me.width <> -1 And Me.offsetX <> 9999 And Me.offsetY <> 9999 Then
        ' shortcut?
        '    Debug.Print("Shortcut")
        '    rect = New Rectangle( _
        '        cfg_grid_numPixels - Me.offsetX, _
        '        cfg_grid_numPixels - Me.offsetY, _
        '         Me.width, Me.height)

        ' Else
        '    rect = clsTasks.bitmap_getDefiningRectangle(bm)
        'End If

        ' we still need to find the occasional differences between above and this.
        rect = clsTasks.bitmap_getDefiningRectangle(bm)

        ' In some cases, we are out of luck: the defining rectangle's top left pixel is NOT our transparent color.
        ' In this case, we'll need to do a minor variation.

        'Debug.Print("Known: x,y,oX,oY = " & Me.offsetX & " - " & Me.offsetY & " - " & Me.width & " - " & Me.height)
        'Debug.Print("Defining rectangle: x,y,oX,oY = " & rect.X & " - " & rect.Y & " - " & rect.Width & " - " & rect.Height)


1000:
        ' With that in mind, we might have an issue with rotation fixing.
        ' This is something we could just skip for now and implement later.
        ' For now, it's just something the user would need to take care off, and we'll have to change some bytes.
        ' Aka: write the file again.

        ' We have the coordinates. We can read a PART ("significant rectangle") again.
        ' Here it gets a bit more tricky. There's some information we will process, 
        ' but we will have to switch a few things in our output.
        ' - we need to remember how many instruction blocks (offset + num colors + [colors] );
        ' - we need to remember the offset;
        ' - we need to count the colors;
        ' - we need to keep track of the color indexes (either find them in a palette, or add them. Max 255 colors, warn!)
        ' --> this will only have been changed if the color palette has been altered in some way.
        ' Output per line: #numBlocks, blocks


        ' We know our relevant pixels.
        coord.Y = rect.Y

        Dim lstPixelSets As New List(Of clsDrawingInstr)
        Dim tmpDrawingInstr As New clsDrawingInstr


3000:
        'bv: coord pixel [0,0] --- w,h [1,1]

        ' APE / Zoot: top left color = transparent
        'Dim colTransparent As System.Drawing.Color = bm.GetPixel(rect.X, rect.Y)

        If Me.parent.colorPalette.colors.Count = 0 Then
            Me.parent.colorPalette.colors.Add(bm.GetPixel(rect.X, rect.Y))
        End If

3005:

        ' 20150619 : after adjusting getDefiningRectangle: coord.Y <=, coord.x <=  --> <
        ' From to to bottom, from left to right
        While coord.Y < (rect.Y + rect.Height)

            ' Restart.
            coord.X = rect.X
            tmpDrawingInstr = New clsDrawingInstr
            lstPixelSets.Clear(False)

            While coord.X < (rect.X + rect.Width)

3010:
                ' Read the color.
                c = bm.GetPixel(coord.X, coord.Y)

                'If c = colTransparent Then
                If Me.parent.colorPalette.getColorIndex(c) = 0 Then

3100:
                    ' We have a transparent pixel.
                    ' We can have this at the very start of the row;
                    ' We can have this after a series of color indexes.
                    If tmpDrawingInstr.offset = 0 And tmpDrawingInstr.pixelColors.Count = 0 Then
3101:
                        ' We are most likely getting this at the very start of the row. 
                        ' No action required.

                    ElseIf tmpDrawingInstr.pixelColors.Count > 0 Then
3102:
                        ' We have had stuff in this line. So we had colors, and now it's transparent.
                        ' Push the drawing instruction, then start over.
                        lstPixelSets.Add(tmpDrawingInstr, False)
                        tmpDrawingInstr = New clsDrawingInstr

                    Else

3108:
                        ' In this case, our offset is bigger than 0 and our color count is 0.
                        ' Don't do anything.

                    End If

                    ' The current pixel is transparent.
                    ' This means we have to increase our offset by 1.
3110:
                    tmpDrawingInstr.offset += 1

3115:
                    ' Exception: if our offset is now 255, we will need to push this block and create a new one.
                    If tmpDrawingInstr.offset = 255 Then
                        lstPixelSets.Add(tmpDrawingInstr, False)
                        tmpDrawingInstr = New clsDrawingInstr
                    End If

                Else
3200:
                    ' We have detected a colored pixel.
                    ' Get its index and add it to our collection.

                    Dim tmpColorIndex = Me.parent.colorPalette.getColorIndex(c, True)
                    If tmpColorIndex = -1 Then Exit Function

                    tmpDrawingInstr.pixelColors.Add(tmpColorIndex, False)



3399:
                    ' Exception: if our number of colored pixels is now 255, we will need to push this block.
                    If tmpDrawingInstr.pixelColors.Count = 255 Then
                        lstPixelSets.Add(tmpDrawingInstr, False)
                        tmpDrawingInstr = New clsDrawingInstr
                    End If


                End If

                coord.X += 1
            End While


            ' === END OF LINE ===

3400:
            ' We processed all blocks. We should finish this too by adding the last block, 
            ' if it never got closed (most likely case, unless we had 255 pixels)
            If tmpDrawingInstr.offset <> 0 Or tmpDrawingInstr.pixelColors.Count > 0 Then
                lstPixelSets.Add(tmpDrawingInstr, False)
            End If

3405:
            ' We have all our pixel sets for this line.
            ' So to opHexRows, we can add:
            ' Number of instruction blocks [between 0 - 255]
            opHexRows.Add(lstPixelSets.Count.ToString("X2"), False)

3406:
            For Each d As clsDrawingInstr In lstPixelSets
                ' For each block: 
                ' - get HEX of offset, num colors, color indexes of pixels
                opHexRows.AddRange(d.getHex(), False)
            Next

3450:

            coord.Y += 1
        End While


        ' height, width is another easy one. We can calculate it by our top/left and bottom/right pixel
        ' The offset is difficult.
        ' Zoot *seemed* to handle it by setting the offset to half the height/width. 
        ' That approach at least centers your image, but it might not be what's wanted.
        ' We will follow the same approach though, as the program can't know the right offsets.

        ' In front of all that, we will also have to add 4 bytes determining the length of this frame.
        ' "et voila!"

        ' 20150619 : after adjusting getDefiningRectangle:  not +1
5001:
        ' Our width, height and offsets are currently read and stored.
        Me.width = rect.Width '+ 1

5002:
        Me.height = rect.Height '+ 1


5003:
        ' This must be improved.
        ' For each frame, we need the relevant pixel (top left) and calculate it's offset to the center.

        ' These offsets are supposedly calculated against a canvas sized image.
        ' added offset = nothing
        If Me.offsetX = -9999 Then Me.offsetX = cfg_grid_numPixels - rect.X
        If Me.offsetY = -9999 Then Me.offsetY = cfg_grid_numPixels - rect.Y

5010:

        'Debug.Print("... Graphic " & vbCrLf & _
        '            "offset X = " & Me.offsetX & " - " & Me.offsetX.ToString("X4").ReverseHEX() & vbCrLf & _
        '            "offset Y = " & Me.offsetY & " - " & Me.offsetY.ToString("X4").ReverseHEX() & vbCrLf & _
        '            "width = " & Me.width & " - " & Me.width.ToString("X4").ReverseHEX() & vbCrLf & _
        '            "height = " & Me.height & " - " & Me.height.ToString("X4").ReverseHEX() & vbCrLf)

        With opHex

5011:
            ' Easier to build it this way. Start by writing the dimensions: height, width.
            .AddRange(Strings.Split(Me.height.ToString("X4").ReverseHEX(), " "), False)
            .AddRange(Strings.Split(Me.width.ToString("X4").ReverseHEX(), " "), False)

            If Me.offsetY >= 0 Then
                .AddRange(Strings.Split(Me.offsetY.ToString("X4").ReverseHEX(), " "), False)
            Else
                .AddRange(Strings.Split((256 * 256 + Me.offsetY).ToString("X4").ReverseHEX(), " "), False)

            End If

            If Me.offsetX >= 0 Then
                .AddRange(Strings.Split(Me.offsetX.ToString("X4").ReverseHEX(), " "), False)
            Else
                .AddRange(Strings.Split((256 * 256 + Me.offsetX).ToString("X4").ReverseHEX(), " "), False)

            End If

5015:
            ' Issue: two  unknown bytes.
            ' For bamboo, frame 1 = 1. 
            ' Always seems to be 1 in APE.
            .Add("01", False)
            .Add("00", False)

5020:
            ' Now add our drawing instructions for the frame.
            .AddRange(opHexRows, False)

        End With

5220:

        Me.hexString = opHex

        ' This frame's hex.
        ' Debug.Print(Strings.Join(Me.hexString.ToArray(), " "))






        ' Return our hex code. 

        Return opHex


        Exit Function

dBug:

        MsgBox("Error while generating HEX-values for a frame." & vbCrLf & _
            "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error while generating HEX-values of a frame")

        Return Nothing



    End Function


 
    Public Function updateOffsets(coordOffsetChanges As Point, Optional blnBatchRotFix As Boolean = False)

        ' This function is for the so called "rotation fixing", positioning fixing, correcting offsets.
        ' By default, changes are applied to all frames in the graphic rather than just this frame.




        On Error GoTo dBug

10:


12:
200:
        ' By default, this applies to all frames
        If cfg_editor_rotFix_individualFrame <> 1 Or blnBatchRotFix = True Then

            ' Just go for every frame
            For Each ztFrame As clsFrame In Me.parent.frames

                ' We need to make sure the offset properties have been set first. 
                If IsNothing(Me.cachedFrame) = True Then
                    Debug.Print("No cached frame yet. Render.")
                    Me.renderFrame() ' Render first to get offsets etc
                End If
                

                ' Now, change.
                ztFrame.offsetY += coordOffsetChanges.Y
                ztFrame.offsetX += coordOffsetChanges.X

                ' Overwrite. This forces a redraw.
                ztFrame.cachedFrame = Nothing 

            Next


        Else

        ' In case we didn't obtain the offsets yet:
        If IsNothing(Me.cachedFrame) = True Then
            Me.renderFrame() ' Render first to get offsets etc
        End If


        ' Correct our offsets and nothing else.
        Me.offsetY += coordOffsetChanges.Y
        Me.offsetX += coordOffsetChanges.X

        ' Overwrite. This forces a redraw.
        Me.cachedFrame = Nothing


            End If


21:


        Exit Function


dBug:
        MsgBox("Error occurred while updating offsets for frame." & vbCrLf & "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical + vbApplicationModal, "Error while settings offsets")


    End Function

    Public Function updateIndex(intNewIndex As Integer, Optional ztFrame As clsFrame = Nothing, Optional ztGraphic As clsGraphic2 = Nothing)


        On Error GoTo dBug

1:
        If IsNothing(ztFrame) Then ztFrame = editorFrame

2:
        If IsNothing(ztGraphic) Then ztGraphic = editorGraphic


5:
        ' Get current list, remove item, add to new
        ztGraphic.frames.Remove(ztFrame)

6:
        ' Add to wanted place
        ztGraphic.frames.Insert(intNewIndex, ztFrame)

        Exit Function


dBug:
        MsgBox("Error occurred while updating index for frame." & vbCrLf & "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error while settings offsets")


    End Function


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    Private Sub NotifyPropertyChanged(ByVal info As String)

        If info <> "cachedFrame" Then Me.lastUpdated = Me.parent.lastUpdated '   Now.ToString("yyyyMMddHHmmss")

        'clsTasks.update_Info("Property clsFrame." & info & " changed.") -> for debugging
        'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))

    End Sub




    Public Function loadPNG(sFile As String)


        Dim bmpCanvas As New Bitmap(2 * cfg_grid_numPixels, 2 * cfg_grid_numPixels)
        Dim bmpDrawTemp As Bitmap = Bitmap.FromFile(sFile)
        Dim bmpDraw As Bitmap

        ' Prevent a file lock on .PNG files.
        ' If we don't use this, they can't be automatically deleted after batch conversion.
        Using bmpDrawTemp
            bmpDraw = New Bitmap(bmpDrawTemp)
            bmpDrawTemp = Nothing
        End Using

        Dim rect As Rectangle = bitmap_getDefiningRectangle(bmpDraw)

        ' Moving from the center: minus width, minus height

        ' Round up, offsets based on PNG image size
        Dim dX As Single = (cfg_grid_numPixels - Math.Ceiling(bmpDraw.Width / 2))
        Dim dY As Single = (cfg_grid_numPixels - Math.Ceiling(bmpDraw.Height / 2))

        dX += rect.X ' defined
        dY += rect.Y ' defined


        ' We draw this on our canvas, because with getHex(), we will want to get the right offsets.
        Dim g As Graphics = Graphics.FromImage(bmpCanvas)
        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor ' prevent softening

        ' Draw the PNG on the canvas.
        g.DrawImage(bmpDraw, New Rectangle(dX, dY, rect.Width, rect.Height), New Rectangle(rect.X, rect.Y, rect.Width, rect.Height), GraphicsUnit.Pixel)
        g.Dispose()


        ' This is probably a dirty way to update the cached frame.
        Me.getHexFromFrame(bmpCanvas, False) ' store.


    End Function


    Public Function savePNG(strFileName As String)

        On Error GoTo dBug

1:

        If IsNothing(editorFrame.cachedFrame) Then


            'MsgBox("Please try to reproduce the steps you've taken to reach this error message." & vbCrLf & _
            '    "A frame has not even been rendered yet, so it can't be saved." & vbCrLf & _
            '    "Feel free to report this as a bug.", vbOKOnly + vbCritical, "Error")

            ' Force render
            Me.renderFrame()
        End If


10:

        ' 0 = canvas size
        ' 1 = relevant pixel area of graphic
        ' 2 = relevant pixel area of frame

        Dim bmRect As New Rectangle(-9999, -9999, 0, 0)
        Dim bmCropped As Bitmap

        Select Case cfg_export_PNG_CanvasSize


            Case 0 ' canvas size
21:

                Me.cachedFrame.Save(strFileName, System.Drawing.Imaging.ImageFormat.Png)

            Case 1 ' relev pixel area of graphic

                ' Several ways to do this.
                ' One is to create the canvas and write all frames;
                ' then just take the defining rectangle
31:

                Dim imgComb As Image
                imgComb = New Bitmap(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2)

                For Each ztFrame As clsFrame In Me.parent.frames

                    If IsNothing(ztFrame.cachedFrame) Then ztFrame.renderFrame()

                    imgComb = clsTasks.images_Combine(imgComb, ztFrame.cachedFrame)
                Next

                bmRect = bitmap_getDefiningRectangle(imgComb)
                bmCropped = clsTasks.bitmap_getCropped(Me.cachedFrame, bmRect)
                bmCropped.Save(strFileName, System.Drawing.Imaging.ImageFormat.Png)

            Case 2 ' relev pixel area of frame

41:
                bmRect = clsTasks.bitmap_getDefiningRectangle(Me.cachedFrame)
                bmCropped = clsTasks.bitmap_getCropped(Me.cachedFrame, bmRect)
                bmCropped.Save(strFileName, System.Drawing.Imaging.ImageFormat.Png)

                Debug.Print("Rect: " & bmRect.X & " - " & bmRect.Y & " - " & bmRect.Width & " - " & bmRect.Height)


        End Select

         

        Exit Function

dBug:

        MsgBox("Error in clsFrame:savePNG, line " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error while saving frame as .PNG")


    End Function

End Class
