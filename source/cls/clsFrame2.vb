Option Explicit On

Imports System.ComponentModel

Public Class clsFrame2

    Implements INotifyPropertyChanged


    Private fr_Hex As New List(Of String)

    'Private fr_width As Integer = -1
    'Private fr_height As Integer = -1

    ' With coreImage, we mean the actual frame's content. No background canvas, no grid, no 'extra frame'.
    ' The bitmap also implictly contains the width and height of this 'core' image. 
    Private fr_coreImageBitmap As Bitmap = Nothing
    Private fr_coreImageHex As New List(Of String)


    Private fr_offsetX As Integer = -9999
    Private fr_offsetY As Integer = -9999

    Private fr_parent As New clsGraphic2

    Private fr_MysteryHEX As New List(Of String)
     
    Private fr_lastUpdated As String = Now.ToString("yyyyMMddHHmmss")                ' for caching purposes.




    ' === Regular

    Public Sub New(myParent As clsGraphic2)
        Me.parent = myParent
    End Sub
    Public Property coreImageHex As List(Of String)
        ' What is the hex code for the core image?
        Get
            Return fr_coreImageHex
        End Get
        Set(value As List(Of String))
            fr_coreImageHex = value
            NotifyPropertyChanged("coreImageHex")
        End Set
    End Property
    Public Property coreImageBitmap As Bitmap
        ' What is the core image bitmap?
        Get
            Return fr_coreImageBitmap
        End Get
        Set(value As Bitmap)
            fr_coreImageBitmap = value
            NotifyPropertyChanged("coreImageBitmap")
        End Set
    End Property

    Public Property parent As clsGraphic2
        ' What is the parent object (clsGraphic2) of our frame? 
        ' Or in other words: which ZT1 Graphic does this frame belong to?
        Get
            Return fr_parent
        End Get
        Set(value As clsGraphic2)
            fr_parent = value
            NotifyPropertyChanged("parent")
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
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    Private Sub NotifyPropertyChanged(ByVal info As String)

        If info <> "cachedFrame" Then Me.lastUpdated = Me.parent.lastUpdated '   Now.ToString("yyyyMMddHHmmss")

        'clsTasks.update_Info("Property clsFrame." & info & " changed.") -> for debugging
        'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))

        ' Important events to detect are:
        ' if coreImageHex changed (which happens when a ZT1 Graphic is read), 
        ' it should trigger an update of coreImageBitmap too - but without causing that one to trigger a new unwanted change.
        ' if coreImageBitmap changed (after a .PNG is loaded), coreImageHex should be updated without triggering another property change.


    End Sub


    Function getCoreImageBitmap() As Bitmap

        ' This is a supporting function. It's possible the core image is not available yet.
        ' If we have coreImageHex, we will have to render it. 
        ' If not, this function shouldn't be called.

        On Error GoTo dBug

11:
        If IsNothing(Me.coreImageBitmap) = True Then
12:
            ' Rendering the frame from hex will store it in the coreImageBitmap.
            Return Me.renderCoreImageFromHex()

        Else
13:
            Return Me.coreImageBitmap
        End If

21:

        Exit Function

dBug:

        MsgBox("Error in clsFrame2.getCoreImageBitmap()" & vbCrLf & _
               "Line " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error")


    End Function
    Function getCoreImageBitmapOnTransparentCanvas() As Bitmap

        On Error GoTo dBug

1:

11:
        ' Draw on transparent canvas
        Dim bmOutput As New Bitmap(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2)
        Dim g As Graphics = Graphics.FromImage(bmOutput)


        Dim imgB = Me.getCoreImageBitmap()
21:
        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor ' prevent softening 
        g.DrawImage(imgB, cfg_grid_numPixels - Me.offsetX, cfg_grid_numPixels - Me.offsetY, imgB.Width, imgB.Height)
        g.Dispose()

31:
        Return bmOutput

        Exit Function

dBug:

        MsgBox("Error in clsFrame2.getCoreImageBitmapOnTransparentCanvas()" & vbCrLf & _
               "Line " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, _
            vbOKOnly + vbCritical, "Error")

    End Function


    Function getHex() As List(Of String)


        Dim lst As New List(Of String)

9000:
        With lst
            ' Remove previous bytes
            .Clear()

5011:
            ' Easier to build it this way. Start by writing the dimensions: height, width.
            .AddRange(Strings.Split(Me.coreImageBitmap.Height.ToString("X4").ReverseHEX(), " "), False)
            .AddRange(Strings.Split(Me.coreImageBitmap.Width.ToString("X4").ReverseHEX(), " "), False)

5015:
            If Me.offsetY >= 0 Then
                .AddRange(Strings.Split(Me.offsetY.ToString("X4").ReverseHEX(), " "), False)
            Else
                .AddRange(Strings.Split((256 * 256 + Me.offsetY).ToString("X4").ReverseHEX(), " "), False)
            End If

5016:
            If Me.offsetX >= 0 Then
                .AddRange(Strings.Split(Me.offsetX.ToString("X4").ReverseHEX(), " "), False)
            Else
                .AddRange(Strings.Split((256 * 256 + Me.offsetX).ToString("X4").ReverseHEX(), " "), False)
            End If

5017:
            ' Issue: two  unknown bytes.
            ' For bamboo, frame 1 = 1. 
            ' Always seems to be 1 in APE.
            .Add("01", False)
            .Add("00", False)

5020:
            ' Now add our drawing instructions for the frame.
            .AddRange(Me.coreImageHex, False)


        End With

        Return lst


    End Function


    Function getImage(Optional blnDrawGrid As Boolean = False) As Bitmap

        ' GetImage will return a bitmap/image.
        ' It will render the core image in this frame; and then add backgrounds.
        ' There's an option to render the image on top of a visible grid (as you have in ZT1).

        On Error GoTo dBug

1:
        ' Draw frame.
        Dim bmOutput As Bitmap = Me.getCoreImageBitmapOnTransparentCanvas()
         

11:
            ' Draw 'extra' background frame, e.g. restaurants?
            If Me.parent.extraFrame = 1 And cfg_export_PNG_RenderBGFrame = 1 Then
                bmOutput = clsTasks.images_Combine(Me.parent.frames(Me.parent.frames.Count - 1).getCoreImageBitmapOnTransparentCanvas(), bmOutput)
            End If

21:
            ' Optional background ZT1 Graphic frame, e.g. animal + toy?
            If editorBgGraphic.frames.Count > 0 And cfg_export_PNG_RenderBGZT1 = 1 Then
                bmOutput = clsTasks.images_Combine(editorBgGraphic.frames(0).getCoreImageBitmapOnTransparentCanvas(), bmOutput)
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

            MsgBox("Error in clsFrame2.getImage()" & vbCrLf & _
                   "Line " & Erl() & vbCrLf & _
                Err.Number & " - " & Err.Description, _
                vbOKOnly + vbCritical, "Error")


    End Function

    Function renderCoreImageFromHex() As Bitmap

        ' This function will read the frame's bytes (hex).
        ' It will only render the core image. For other purposes, you should use .getImage() (backgrounds, extra frame etc).
        ' It will only return a height + width.
        ' It will update our offset properties on this frame object


        On Error GoTo dBug2
        Debug.Print(Now().ToString("yyyyMMdd HHmmss") & " clsFrame2.renderCoreImageFromHex: start")

        Debug.Print("Current amount of hex in coreImageHex: " & Me.coreImageHex.Count)



10:
        ' Only one thing matters: do we actually have HEX?
        If Me.coreImageHex.Count = 0 Then

            Debug.Print("No hex to create anything from!")
            Me.coreImageBitmap = Nothing
            Return Nothing

        End If

20:

        ' Now, let's proceed.


  


        'Debug.Print("'" & strFrameHex & "'")

30:
        ' Create a copy of this frame's bytes.
        Dim frameHex As New List(Of String)
        frameHex.AddRange(Me.coreImageHex)

        Dim frameCoreImageBitmap As Bitmap                       ' Contains the core image we'll be drawing

        Dim ztPal As clsPalette = Me.parent.colorPalette

        Dim blnIsShadow As Boolean = False


         


31:
        ' Our core image bitmap's canvas. Height and width are determined in the first few bytes (reversed).
        frameCoreImageBitmap = New Bitmap( _
             CInt("&H" & frameHex(3) & frameHex(2)), _
             CInt("&H" & frameHex(1) & frameHex(0)) _
             )


32:

        ' Usually hex(1) = 00. But sometimes, it is "80". This seems to be an indicator introduced in Marine Mania.
        ' Example: dolphin's "ssurf"-animations. The frames are actually compressed. For shadows, it's only offsets and black.
        If frameHex(1) = "80" Then
            blnIsShadow = True
        End If


41:
        ' Offsets. 
        ' In case of unknown offsets 
        'If Me.offsetY = -9999 Then
        If frameHex(5) = "FF" Then
            ' Large size images. Needs some adjustment.
            Me.offsetY = ((256 * 256) - CInt("&H" & frameHex(5) & frameHex(4))) * -1
        Else
            ' Normal offsets
            Me.offsetY = CInt("&H" & frameHex(5) & frameHex(4))
        End If

        'End If

42:
        ' In case of unknown offsets and HEX = FF (large size image)
        'If Me.offsetX = -9999 Then
        If frameHex(7) = "FF" Then
            ' Large size images. Needs some adjustment.
            Me.offsetX = ((256 * 256) - CInt("&H" & frameHex(7) & frameHex(6))) * -1
        Else
            ' Normal offsets
            Me.offsetX = CInt("&H" & frameHex(7) & frameHex(6))
        End If

        'End If


45:
        ' We figured out the entire ZT1 Graphic format, EXCEPT for these 2 bytes.
        ' APE usually sets them to 00 00 (or was it 00 01? I always forget, needs verification).
        ' Anyhow, in ZT1 you will see that these mysterious bytes may vary.
        With Me.mysteryHEX
            .Clear(False)
            .Add(frameHex(8), False)
            .Add(frameHex(9), False)
        End With

46:
        ' We covered the first 10 bytes (height, width, offset Y, offset X, mystery bytes). 
        ' Remove them now to speed up further processing. We'll repeat this a few more times.
        ' It's why we made a copy of the core image's hex.
        frameHex.Skip(10)





1000:
        ' === Color instructions ===
        ' We get to the most exciting part. 
        ' We will have some variables here. It would have been better to declare these in the top of this function, 
        ' but for easier explanation/understanding, we're only declaring them here.

        Dim intX As Integer = 0 ' which 'row' of pixels is being drawn?
        Dim intY As Integer = 0 ' which 'column' of pixels is being drawn?
        Dim intNumDrawingInstructions As Integer ' How many drawing instructions are there for this 'row' of pixels?
        Dim intNumDrawingInstructions_current As Integer ' which drawing instruction is being processed?
        Dim intNumDrawingInstructions_colors As Integer ' How many pixels will we color?
        Dim intNumDrawingInstructions_colors_current As Integer ' which is the current pixel we're processing/going to color?
        Dim c As System.Drawing.Color ' this is the color we'll draw. 

1005:
        ' This 'while'-loop should spare us from APE junk bytes.
        ' Often, when analyzing graphics generated by APE, you'll notice some unneccessary bytes at the very end.

        ' Zoo Tycoon draws from left to right, row by row.

        While frameHex.Count > 0 And intY < frameCoreImageBitmap.Height

            ' First byte for each row contains the number of pixel sets.
            ' That's at least 1 block (a transparent line would give [offset] [0 colors] -
            ' Otherwise, it's what we've called a 'drawing instruction': 
            ' [offset/number of pixels to remain transparent][numColorPixels][pixels]

1100:
            ' Number of drawing instructions are there for this 'row' of pixels?
            intNumDrawingInstructions = CInt("&H" & frameHex(0))
            frameHex.Skip(1)

1120:

            ' We know how many drawing instructions we have for this row.
            ' Let's process those instructions.
            For intNumDrawingInstructions_current = 0 To (intNumDrawingInstructions - 1)



1300:
                ' Starting with color byte( [offset] ). 
                ' If this is 00, we start all the way to the left. 
                ' If this is 01, we skip 1 pixel and leave it transparent.
                ' If this is 02, we'll skip 2 pixels and leave them transparent
                ' And so on.
                intX += CInt("&H" & frameHex(0))
1301:
                ' Number of pixels we'll give a color ( [num of pixels to draw] )
                intNumDrawingInstructions_colors = CInt("&H" & frameHex(1))

1309:
                ' Remove [offset] and [num of pixels to draw] instructions.
                frameHex.Skip(2)

1400:


                ' We know how many colors are expected. 
                For intNumDrawingInstructions_colors_current = 0 To (intNumDrawingInstructions_colors - 1)


1410:
                    ' This is the color we'll be drawing. 
                    If blnIsShadow = True Then
                        ' If it's the compressed format, it's simply black.
                        c = Color.Black
                    Else
                        ' In the traditional format, we fetch it from a color palette, by its index number.
                        c = ztPal.colors(CInt("&H" & frameHex(intNumDrawingInstructions_colors_current)))
                    End If

1413:
                    ' Color the pixel.
                    ' Debug.Print("Drawing: x=" & intX & ", y=" & intY & " = " & c.ToString() & " / w=" & frameCoreImageBitmap.Width & ", h=" & frameCoreImageBitmap.Height)
                    frameCoreImageBitmap.SetPixel(intX, intY, c)

1450:
                    ' Be ready to draw next pixel.
                    intX += 1


                Next intNumDrawingInstructions_colors_current
1455:
                ' Rather than individually deleting those colors one by one from the bytes we (still) need to process, 
                ' we'll do it at once now.
                frameHex.Skip(intNumDrawingInstructions_colors_current)

1500:

            Next intNumDrawingInstructions_current


            'Debug.Print("Last pixel in line was X " & intX & ". Y was " & intY)

1550:
            intX = 0 ' Start all the way on the left again.
            intY += 1 ' Ready to process next line.

        End While


        Debug.Print("Check: " & intX & " - " & intY & " - " & frameCoreImageBitmap.Width & " - " & frameCoreImageBitmap.Height)


2100:

        ' Implemented a check for APE junk bytes and remove if any are left.
        ' Theoretically, there shouldn't be. But APE has the tendency to generate crap.
        If frameHex.Count > 0 Then
            Debug.Print("Cleaning up APE junk bytes: " & frameHex.Count)
            'Me.coreImageHex.RemoveRange(Me.coreImageHex.Count - frameHex.Count - 1, frameHex.Count)
        End If


2110:
        ' This should come in very handy. The actual bitmap won't be changed unless a .PNG is loaded.
        ' If that's the case, this frame's coreImageHex should be updated as well
        Me.coreImageBitmap = frameCoreImageBitmap





        Debug.Print("Current amount of hex in coreImageHex after rendering: " & Me.coreImageHex.Count)

        Debug.Print("Frame rendered.")


9999:
        Return frameCoreImageBitmap

        Exit Function

dBug2:

        frmMain.picBox.Image = bm

        Debug.Print("Debugging info:")
        ' Debug.Print("What's left of HEX() ? " & Strings.Join(hex.ToArray(), " "))
        ' Debug.Print("My index: " & Me.parent.frames.IndexOf(Me) & " - frame Length: " & Me.hexString.Length.ToString("X4"))

        MsgBox("Error in clsFrame2.renderCoreImageFromHex()" & vbCrLf & _
               "Line " & Erl() & vbCrLf & _
               "Width, height: " & frameCoreImageBitmap.Width & ", " & frameCoreImageBitmap.Height & vbCrLf & _
            "Offset x, y: " & Me.offsetX & ", " & Me.offsetY & vbCrLf & _
            "Colors: Currently at drawing instruction " & intNumDrawingInstructions_current & "/" & intNumDrawingInstructions & ", color " & intNumDrawingInstructions_colors_current & "/" & intNumDrawingInstructions_colors & vbCrLf & _
            "Last referenced x, y: " & intX & ", " & intY & vbCrLf & _
            "Current length of framehex: " & frameHex.Count & vbCrLf & _
            "Current length of colors: " & ztPal.colors.Count & vbCrLf & _
             vbCrLf & Err.Number & " - " & Err.Description & _
            vbCrLf & "Line: " & Erl(), vbOKOnly + vbCritical, "Error")



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
            For Each ztFrame As clsFrame2 In Me.parent.frames

                ' Change.
                ztFrame.offsetY += coordOffsetChanges.Y
                ztFrame.offsetX += coordOffsetChanges.X

            Next

        Else

            ' Correct our offsets and nothing else.
            Me.offsetY += coordOffsetChanges.Y
            Me.offsetX += coordOffsetChanges.X

        End If


21:


        Exit Function


dBug:
        MsgBox("Error occurred while updating offsets for frame." & vbCrLf & "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical + vbApplicationModal, "Error while settings offsets")


    End Function

    Public Function updateIndex(intNewIndex As Integer, Optional ztFrame As clsFrame2 = Nothing, Optional ztGraphic As clsGraphic2 = Nothing)


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






    Public Function loadPNG(sFile As String)

        On Error GoTo dBug


5: 
        Dim bmpDrawTemp As Bitmap = Bitmap.FromFile(sFile)
        Dim bmpDraw As Bitmap


10:
        ' Prevent a file lock on .PNG files.
        ' If we don't use this, they can't be automatically deleted after batch conversion.
        Using bmpDrawTemp
            bmpDraw = New Bitmap(bmpDrawTemp)
            bmpDrawTemp = Nothing
        End Using
          
         

        ' === Rewrite.

        ' Get the defining rectangle of the .PNG image.
        ' Next, generate the hex code. That should be enough for now.
        ' Do reset our imageCoreBitmap

100:
        ' Easy to start with: define our new offsets
        ' height, width is another easy one. We can calculate it by our top/left and bottom/right pixel
        ' The offset is difficult. Zoot *seemed* to handle it by setting the offset to half the height/width. 
        ' That approach at least centers your image, but it might not be what's wanted.
        ' We will follow the same approach though, as the program can't know the right offsets.
        Me.offsetX = Math.Ceiling(bmpDraw.Width / 2)
        Me.offsetY = Math.Ceiling(bmpDraw.Height / 2)

110:
        Dim rectCrop As Rectangle = bitmap_getDefiningRectangle(bmpDraw)
        Dim bmpCropped As Bitmap = clsTasks.bitmap_getCropped(bmpDraw, rectCrop)
         

200:
        ' We have our bitmap now. 'coreImageBitmap'. We won't store this right away.
        ' Reason: instead of the top left pixel, transparency could already have been determined in some (batch) processes

        'Dim opHex As New List(Of String)                            ' Main output
        Dim opHexRows As New List(Of String)                        ' Store bytes as strings for now. Actual drawing instructions.

        ' We will need this to move over every pixel.
        Dim intX As Integer = 0
        Dim intY As Integer = 0

        Dim c As System.Drawing.Color                               ' to go over every color

         

        ' Take the palette from the parent
        Dim ztPal As clsPalette = Me.parent.colorPalette

        Dim lstDrawingInstructions As New List(Of clsDrawingInstr)
        Dim tmpDrawingInstr As New clsDrawingInstr


1000:


        ' Here it gets a bit more tricky. There's some information we will process, 
        ' but we will have to switch a few things in our output.
        ' - we need to remember how many instruction blocks (offset + num colors + [colors] );
        ' - we need to remember the offset;
        ' - we need to count the colors;
        ' - we need to keep track of the color indexes (either find them in a palette, or add them. Max 255 colors, warn!)
        ' --> this will only have been changed if the color palette has been altered in some way.
        ' Output per line: #numBlocks, blocks




3000:
        'bv: coord pixel [0,0] --- w,h [1,1]

        ' APE / Zoot: top left color = transparent.
        ' Only if no colors are known in our palette, we rely on that method.
        ' Reason: it works differently in batch conversions.
        If Me.parent.colorPalette.colors.Count = 0 Then
            Me.parent.colorPalette.colors.Add(bm.GetPixel(0, 0))
        End If

3005:
         
        ' From top to bottom, from left to right
        While intY < bmpCropped.Height

            ' Restart.
            intX = 0
            tmpDrawingInstr = New clsDrawingInstr
            lstDrawingInstructions.Clear(False)

            While intX < bmpCropped.Width

3010:
                ' Read the color.
                c = bm.GetPixel(intX, intY)

                ' If the color is considered to be transparent:
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
                        lstDrawingInstructions.Add(tmpDrawingInstr, False)
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
                        lstDrawingInstructions.Add(tmpDrawingInstr, False)
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
                        lstDrawingInstructions.Add(tmpDrawingInstr, False)
                        tmpDrawingInstr = New clsDrawingInstr
                    End If


                End If

                intX += 1
            End While


            ' === END OF LINE ===

3400:
            ' We processed all blocks. We should finish this too by adding the last block, 
            ' if it never got closed (most likely case, unless we had 255 pixels)
            If tmpDrawingInstr.offset <> 0 Or tmpDrawingInstr.pixelColors.Count > 0 Then
                lstDrawingInstructions.Add(tmpDrawingInstr, False)
            End If

3405:
            ' We have all our pixel sets for this line.
            ' So to opHexRows, we can add:
            ' Number of instruction blocks [between 0 - 255]
            opHexRows.Add(lstDrawingInstructions.Count.ToString("X2"), False)

3406:
            For Each d As clsDrawingInstr In lstDrawingInstructions
                ' For each block: 
                ' - get HEX of offset, num colors, color indexes of pixels
                opHexRows.AddRange(d.getHex(), False)
            Next

3450:

            intY += 1
        End While



         


9000: 

        With Me.coreImageHex
            ' Remove previous bytes
            .Clear() 

9001:
            ' Now add our drawing instructions for the frame.
            .AddRange(opHexRows, False)

        End With

9002:
        ' Reset. Should be regenerated from our hex.
        Me.coreImageBitmap = Nothing


        Exit Function


dBug:
        MsgBox("Error occurred in clsFrame2.loadPNG()" & vbCrLf & _
               "Line: " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error while settings offsets")




    End Function


    Public Function savePNG(strFileName As String)

        On Error GoTo dBug

1:


10:

        ' 0 = canvas size
        ' 1 = relevant pixel area of graphic
        ' 2 = relevant pixel area of frame

        Dim bmRect As New Rectangle(-9999, -9999, 0, 0)
        Dim bmCropped As Bitmap

        Select Case cfg_export_PNG_CanvasSize


            Case 0
                ' Save PNG image. Complete canvas size.
21:
                Me.getCoreImageBitmapOnTransparentCanvas().Save(strFileName, System.Drawing.Imaging.ImageFormat.Png)

            Case 1
                ' Only save cropped version (relevant area of graphic) 

31:
                ' Cheap trick: combine all images into 1, then get the relevant rectangle.
                ' Some caching might be in order in the future :)

                Dim imgComb As Image
                imgComb = New Bitmap(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2)

                For Each ztFrame As clsFrame2 In Me.parent.frames
                    imgComb = clsTasks.images_Combine(imgComb, ztFrame.getCoreImageBitmapOnTransparentCanvas())
                Next

                ' Apply to this particular frame.
                bmRect = bitmap_getDefiningRectangle(imgComb)
                bmCropped = clsTasks.bitmap_getCropped(Me.getCoreImageBitmapOnTransparentCanvas, bmRect)
                bmCropped.Save(strFileName, System.Drawing.Imaging.ImageFormat.Png)

            Case 2
                ' Only save relevant area of frame

41:
                Me.getCoreImageBitmap().Save(strFileName, System.Drawing.Imaging.ImageFormat.Png)



        End Select



        Exit Function

dBug:

        MsgBox("Error in clsFrame:savePNG" & vbCrLf & _
               "Line " & Erl() & vbCrLf & _
            Err.Number & " - " & Err.Description, _
            vbApplicationModal + vbOKOnly + vbCritical, "Error while saving frame as .PNG")


    End Function

End Class
