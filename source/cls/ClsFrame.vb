Option Explicit On

Imports System.ComponentModel

''' <summary>
''' <para>ClsFrame is an object to handle a ZT1 Frame.</para>
''' <para>A frame is one still picture of view of an animation.</para>
''' </summary>
''' <remarks>
''' It contains some properties such as width, height, offset; as well as a series of drawing instructions.
''' In 2019, there are still two "mystery bytes" of which the function is still unknown.
''' It's assumed the purpose of all other bytes has been discovered.
''' </remarks>
Public Class ClsFrame

    Implements INotifyPropertyChanged

    ' CoreImage means the actual frame's content. No background canvas, no grid, no 'extra frame'.
    ' The bitmap also implictly contains the width and height of this 'core' image. 
    Private fr_coreImageBitmap As Bitmap = Nothing
    Private fr_coreImageHex As New List(Of String) ' contains height/width and offsets after all.

    Private fr_offsetX As Integer = -9999
    Private fr_offsetY As Integer = -9999

    Private fr_parent As New ClsGraphic

    Private fr_MysteryHEX As New List(Of String)

    Private fr_lastUpdated As String = Now.ToString("yyyyMMddHHmmss")                ' for caching purposes.




    ' === Regular

    ''' <summary>
    ''' New frame object has been created.
    ''' </summary>
    ''' <param name="myParent">ClsGraphic - the graphic (view) of which this frame is part</param>
    Public Sub New(myParent As ClsGraphic)

        ' Set parent
        Me.Parent = myParent

        ' 20170512 - consider automatically adding this frame to the parent's frame collection?

    End Sub

    ''' <summary>
    ''' Hex values for this ZT1 frame (if set)
    ''' </summary>
    ''' <returns>List(Of String)</returns>
    Public Property CoreImageHex As List(Of String)
        Get
            Return fr_coreImageHex
        End Get
        Set(value As List(Of String))
            fr_coreImageHex = value
            NotifyPropertyChanged("CoreImageHex")
        End Set
    End Property


    ''' <summary>
    ''' Image bitmap (if set)
    ''' </summary>
    ''' <returns>Bitmap</returns>
    Public Property CoreImageBitmap As Bitmap
        ' What is the core image bitmap?
        Get
            Return fr_coreImageBitmap
        End Get
        Set(value As Bitmap)
            fr_coreImageBitmap = value
            NotifyPropertyChanged("CoreImageBitmap")
        End Set
    End Property

    ''' <summary>
    ''' Parent graphic of this ZT1 frame
    ''' </summary>
    ''' <returns>ClsGraphic</returns>
    Public Property Parent As ClsGraphic
        ' What is the parent object (ClsGraphic) of our frame? 
        ' Or in other words: which ZT1 Graphic does this frame belong to?
        Get
            Return fr_parent
        End Get
        Set(value As ClsGraphic)
            fr_parent = value
            NotifyPropertyChanged("Parent")
        End Set
    End Property

    ''' <summary>
    ''' Horizontal offset (X) of this frame.
    ''' How much should the image be moved to the left (+)/right (-), compared to the center of the square? (center is based on ZT1's cFootPrintX and cFootPrintY settings)
    ''' </summary>
    ''' <returns>Integer</returns>
    Public Property OffsetX As Integer
        Get
            Return fr_offsetX
        End Get
        Set(value As Integer)
            fr_offsetX = value
            NotifyPropertyChanged("OffsetX")
        End Set
    End Property

    ''' <summary>
    ''' Vertical offset (Y) of this frame.
    ''' How much should the image be moved to the top (+)/bottom (-), compared to the center of the square? (center is based on ZT1's cFootPrintX and cFootPrintY settings)
    ''' </summary>
    ''' <returns>Integer</returns>
    Public Property OffsetY As Integer
        Get
            Return fr_offsetY
        End Get
        Set(value As Integer)
            fr_offsetY = value
            NotifyPropertyChanged("OffsetY")
        End Set
    End Property

    ''' <summary>
    ''' Timestamp of last update. Used to see if re-rendering the frame is needed.
    ''' </summary>
    ''' <returns>Timestamp</returns>
    Public Property LastUpdated As String
        ' Used to see if re-rendering is needed.
        Get
            Return fr_lastUpdated
        End Get
        Set(value As String)
            fr_lastUpdated = value
            'NotifyPropertyChanged("LastUpdated") -> would cause a loop
        End Set
    End Property

    ''' <summary>
    ''' <para>Mystery bytes/hex is the term used in this project to refer to two bytes present in the hex values of each frame.</para>
    ''' <para>Until now (16th of August 2019), the purpose of these 2 bytes is unknown. It's not been discovered yet if they have any purpose in the game.</para>
    ''' </summary>
    ''' <remarks>
    ''' Are they:
    ''' - another counter?
    ''' - another integrity check?
    ''' - a signature?
    ''' - some value used in the program used by Blue Fang/Rapan Animation Studio (Rapan LLC. Sofia) to create the graphics?
    ''' - ...
    ''' 
    ''' If anyone ever finds out, please let me know!
    ''' </remarks>
    ''' <returns>List(Of String)</returns>
    Public Property MysteryHEX As List(Of String)
        ' This is used to store our currently 2 unknown bytes. We call them our mystery bytes.
        Get
            Return fr_MysteryHEX
        End Get
        Set(value As List(Of String))
            fr_MysteryHEX = value
            NotifyPropertyChanged("MysteryHEX")
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' This sub is used to inform the object that an important property has changed.
    ''' By calling this sub, the LastUpdated property will be set.
    ''' </summary>
    ''' <param name="strSource">Which property has changed</param>
    Private Sub NotifyPropertyChanged(ByVal StrSource As String)

        Me.LastUpdated = Me.Parent.LastUpdated '   Now.ToString("yyyyMMddHHmmss")

        'clsTasks.update_Info("Property clsFrame." & info & " changed.") -> for debugging
        'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))

        ' Important events to detect are:
        ' - if CoreImageHex changed (which happens when a ZT1 Graphic is read), 
        ' --->  it should trigger an update of CoreImageBitmap too - but without causing that one to trigger a new unwanted change.
        ' - if CoreImageBitmap changed (after a .PNG is loaded), coreImageHex should be updated without triggering another property change.


    End Sub

    ''' <summary>
    ''' <para>Returns the core bitmap image.</para>
    ''' <para>If the ZT1 Graphic has been rendered (CoreImageBitmap is set), it returns this cached version.</para>
    ''' <para>If the ZT1 Graphic hasn't been rendered yet, it renders the ZT1 Graphic from the hex values.</para>
    ''' </summary>
    ''' <returns>Bitmap</returns>
    Function GetCoreImageBitmap() As Bitmap

        On Error GoTo dBug


        MdlZTStudio.Trace(Me.GetType().FullName, "GetCoreImageBitmap", "Fetching core image bitmap...")

11:
        If IsNothing(Me.CoreImageBitmap) = True Then
12:
            ' Rendering the frame from hex will store it in the coreImageBitmap.
            MdlZTStudio.Trace(Me.GetType().FullName, "GetCoreImageBitmap", "Need to render bitmap...")
            Return Me.RenderCoreImageFromHex()

        Else
13:

            MdlZTStudio.Trace(Me.GetType().FullName, "GetCoreImageBitmap", "Using cached bitmap...")
            Return Me.CoreImageBitmap
        End If

21:

        Exit Function

dBug:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "GetCoreImageBitmap", Information.Err)

    End Function

    ''' <summary>
    ''' Returns the image bitmap, on a transparent canvas.
    ''' </summary>
    ''' <remarks>
    ''' Todo: this needs more documentation. Take the time again to see how blnDrawInCenter works.
    ''' </remarks>
    ''' <param name="BlnDrawInCenter">Boolean</param>
    ''' <returns>Bitmap</returns>
    Function GetCoreImageBitmapOnTransparentCanvas(Optional BlnDrawInCenter As Boolean = False) As Bitmap

        On Error GoTo dBug
1:
        Dim IntWidth As Integer
        Dim IntHeight As Integer

        ' Retrieve the pure bitmap
        Dim BmCoreImageBitMap = Me.GetCoreImageBitmap()

        If IsNothing(Me.GetCoreImageBitmap) = True Then
            Return Nothing
        End If

        ' Determine how big the canvas will be.
        ' Continue reading below, as only the width/height are determined first, but only multiplied by 2 later!
        Select Case BlnDrawInCenter
            Case False
                ' Draw on transparent canvas (most common scenario)
                ' It's important to keep in mind that the canvas is by default 2 * Cfg_grid_numPixels, both in width and in height.
                ' So these variables will actually contain the top left pixel of the (four pixel) center.
                IntWidth = Cfg_grid_numPixels
                IntHeight = Cfg_grid_numPixels
            Case True
                ' Convert everything relative to the center. Method contributed by HENDRIX.
                ' The idea is to generate a canvas around the center. By adding some spacing to the left/right or top/bottom, 
                ' ZT Studio's import mechanisms will automatically apply the correct offsets again in ClsFrame::LoadPNG()
                ' There, the initial offset (before being corrected) is already determined by width /2 and height / 2
                ' Pick whatever is bigger: the absolute value of the  actual offset compared to the center; or of the offset (+ or -) minus the width/height
                '
                ' Some examples (based on width) make this easier to understand.
                ' Basically it tries to find which side (left or right of the center) is the largest.
                ' Left part is easy to understand; as for right part here are some examples based on abs(offset-width)
                ' Positive offset = to left of center; negative = to right of center
                ' Offset abs(0) = 0 | abs(0 - 50) = 50 ---> max is 50
                ' Offset abs(5) = 5 | abs(5-50) = 45 ---> max is 45
                ' Offset abs(5) = 5 | abs(5-4) = 1 ---> max is 5
                ' Offset abs(-5) = 5 | abs(-5-50) = 55 ---> max is 55
                ' Offset abs(-5) = 5 | abs(-5-4) = 9 ---> max is 9

                ' It seems the + and -1 are needed to avoid changing the image size
                ' Keep in mind this is multiplied by 2 further in the code!
                IntWidth = Math.Max(Math.Abs(Me.OffsetX), Math.Abs(Me.OffsetX - BmCoreImageBitMap.Width)) + 1
                IntHeight = Math.Max(Math.Abs(Me.OffsetY), Math.Abs(Me.OffsetY - BmCoreImageBitMap.Height)) - 1
        End Select

        ' Draw this retrieved bitmap on a transparent canvas
        Dim BmOutput As New Bitmap(IntWidth * 2, IntHeight * 2) ' Creating the output canvas
        Dim ObjGraphic As Graphics = Graphics.FromImage(BmOutput) ' Preparing to manipulate this empty canvas

25:
        ObjGraphic.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor ' Prevent softening: set InterpolationMode to NearestNeighbour

30:
        ' Onto this canvas, draw the CoreImageBitmap of this frame.
        Dim IntStartingPointX As Integer = IntWidth - Me.OffsetX + 1
        Dim IntStartingPointY As Integer = IntHeight - Me.OffsetY + 1
        ObjGraphic.DrawImage(BmCoreImageBitMap, IntStartingPointX, IntStartingPointY, BmCoreImageBitMap.Width, BmCoreImageBitMap.Height)
31:
        ObjGraphic.Dispose() 'Dispose as recommended. Output has been stored in BmOutput anyway.

        Return BmOutput

        Exit Function

dBug:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "GetCoreImageBitmapOnTransparentCanvas", Information.Err)

    End Function

    ''' <summary>
    ''' <para>Returns a bitmap image for this frame.</para>
    ''' <para>If a background frame within this graphic exists, it will also be rendered if enabled. (for example: Restaurant)</para>
    ''' <para>If a background graphic has been set, it will also be rendered if enabled. (for example: Orang Utan toy)</para>
    ''' </summary>
    ''' <param name="BlnDrawGrid">Add grid. Defaults to false.</param>
    ''' <param name="BlnCentered"></param>
    ''' <returns>Bitmap</returns>
    Function GetImage(Optional BlnDrawGrid As Boolean = False, Optional BlnCentered As Boolean = False) As Bitmap

        ' It will render the core image in this frame; and then add backgrounds.
        ' There's an option to render the image on top of a visible grid (as you have in ZT1).

        On Error GoTo dBug

1:
        ' Draw frame.
        Dim BmOutput As Bitmap = Me.GetCoreImageBitmapOnTransparentCanvas(BlnCentered)


11:
        ' Draw 'extra' background frame, e.g. restaurants?
        If Me.Parent.HasBackgroundFrame = 1 And Cfg_export_PNG_RenderBGFrame = 1 Then
            BmOutput = MdlBitMap.CombineImages(Me.Parent.Frames(Me.Parent.Frames.Count - 1).GetCoreImageBitmapOnTransparentCanvas(BlnCentered), BmOutput)
        End If

21:
        ' Optional background ZT1 Graphic frame, e.g. animal + toy?
        If EditorBgGraphic.Frames.Count > 0 And Cfg_export_PNG_RenderBGZT1 = 1 Then
            ' Currently it's always the 
            BmOutput = MdlBitMap.CombineImages(EditorBgGraphic.Frames(0).GetCoreImageBitmapOnTransparentCanvas(BlnCentered), BmOutput)
        End If

31:
        ' Draw grid?
        If BlnDrawGrid = True Then
            BmOutput = MdlBitMap.CombineImages(MdlBitMap.DrawGridFootPrintXY(Cfg_grid_footPrintX, Cfg_grid_footPrintY), BmOutput)
        End If

41:
        Return BmOutput


        Exit Function

dBug:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "GetImage", Information.Err)


    End Function


    ''' <summary>
    ''' <para>Renders the core image of this frame (without grid, offsets etc.) as a bitmap, based on the supplied hex (CoreImageHex).</para>
    ''' <para>Warning: while processing the hex, this method (re)sets the offsets for this frame!</para>
    ''' <para>For other renderings, use another render method.</para>
    ''' </summary>
    ''' <returns>Bitmap or Nothing</returns>
    Function RenderCoreImageFromHex() As Bitmap

        On Error GoTo dBug2

        MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Start rendering from hex...")

10:
        ' Only one thing matters: do we actually have HEX?
        If Me.CoreImageHex.Count = 0 Then

            MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "There is no hex! Returning Nothing instead of Bitmap")
            Me.CoreImageBitmap = Nothing
            Return Nothing

        End If

30:
        ' Create a copy of this frame's bytes.
        Dim LstFrameHex As New List(Of String)
        LstFrameHex.AddRange(Me.CoreImageHex)

        Dim FrameCoreImageBitmap As Bitmap                        ' Contains the core image that will be rendered

        Dim ZtPal As ClsPalette = Me.Parent.ColorPalette
        Dim BlnIsShadow As Boolean = False

        ' This case is really weird. It's for the Restaurant.
        ' The idle animation's views (eg NE) contain 10 bytes: (00 00) (00 00) (00 00) (00 00) (D0 10)
        ' The idle view supposedly uses an extraFrame
        ' Which means no height/width, no offsets, and yet some weird mystery bytes.
        ' The mystery bytes might be the same for similar graphics, which have an empty first frame and an extra frame as background?

        ' Perhaps it could be identified by mystery bytes?
        If LstFrameHex.Count = 10 Then

            MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Weird hex: " & String.Join(" ", LstFrameHex.ToArray()))

            ' Basically height = width = 0.
            If LstFrameHex(0) = 0 And LstFrameHex(1) = 0 And LstFrameHex(2) = 0 And LstFrameHex(3) = 0 Then

                ' It should actually be 0, 0 (empty); but this is a work around.
                Me.CoreImageBitmap = New Bitmap(1, 1)
                Return Me.CoreImageBitmap
            End If
        End If

32:

        ' Usually hex(1) = 00. But sometimes, it is "80". This seems to be an indicator introduced in Marine Mania.
        ' Example: dolphin's "ssurfswi"-animations. The frames are actually compressed. For shadows, it's only offsets and black.
        ' Surprisingly enough, not all shadow animations (even of the dolphin) use this format.
        ' Probably due to HEX 80 00 being -32678, which is very unlikely to happen?
        If LstFrameHex(1) = "80" Then

            MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Byte index 1 = 80 -> assuming this is the compressed shadow format (Marine Mania)")
            MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Byte index 2 = " & LstFrameHex(2) & " -> still signifies height")

            BlnIsShadow = True

            ' Of course, 80 doesn't make any sense here. Ignore byte index 1.
            ' Also ignoring byte index 3 to be consistent, although it could probably work?
            FrameCoreImageBitmap = New Bitmap(
                CInt("&H" & LstFrameHex(2)),
                CInt("&H" & LstFrameHex(0))
            )

        Else

            MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Dealing with a regular sized graphic")
            ' All normal cases
            ' Canvas size determined: height and width are specified in the first few bytes (reversed).
            FrameCoreImageBitmap = New Bitmap(
                 CInt("&H" & LstFrameHex(3) & LstFrameHex(2)),
                 CInt("&H" & LstFrameHex(1) & LstFrameHex(0))
                 )

        End If


41:
        ' Offsets. 
        ' In case of unknown offsets 
        ' If Me.offsetY = -9999 Then
        If LstFrameHex(5) = "FF" Then
            ' Large size images. Needs some adjustment.
            ' Todo: add an example image here.
            MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Byte index 5 = FF -> This graphic should be large in size (offset Y). Add this as an example in the code!")
            Me.OffsetY = ((256 * 256) - CInt("&H" & LstFrameHex(5) & LstFrameHex(4))) * -1
        Else
            ' Normal offsets
            Me.OffsetY = CInt("&H" & LstFrameHex(5) & LstFrameHex(4))
        End If

42:
        ' In case of unknown offsets and HEX = FF (large size image)
        'If Me.offsetX = -9999 Then
        If LstFrameHex(7) = "FF" Then
            ' Large size images. Needs some adjustment.
            MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Byte index 7 = FF -> This graphic should be large in size (offset X). Add this as an example in the code!")
            Me.OffsetX = ((256 * 256) - CInt("&H" & LstFrameHex(7) & LstFrameHex(6))) * -1
        Else
            ' Normal offsets
            Me.OffsetX = CInt("&H" & LstFrameHex(7) & LstFrameHex(6))
        End If


45:
        ' Entire ZT1 Graphic format is documented, EXCEPT for these 2 bytes.
        ' APE usually sets them to 00 00 (or was it 00 01? Needs verification).
        ' Anyhow, in ZT1 you will see that these mysterious bytes may vary.
        With Me.MysteryHEX
            .Clear(False)
            .Add(LstFrameHex(8), False)
            .Add(LstFrameHex(9), False)
        End With

        MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Byte index 8, 9 -> the mystery bytes are " & LstFrameHex(8) & ", " & LstFrameHex(9))


46:
        ' Above covered the first 10 bytes (height, width, offset Y, offset X, mystery bytes). 
        ' Remove them now to speed up further processing. 
        ' This is something which will be done a couple of times later.
        ' No worries, this is on a copy of the bytes.
        LstFrameHex.Skip(10)


1000:
        ' === Color instructions ===
        ' For clarity, declarations only happen here.
        ' Keep in mind that an image is rendered from left to right, from top to bottom.

        Dim IntX As Integer = 0 ' which 'row' of pixels is being drawn?
        Dim IntY As Integer = 0 ' which 'column' of pixels is being drawn?
        Dim IntNumDrawingInstructions As Integer ' How many drawing instructions are there for this 'row' of pixels?
        Dim IntNumDrawingInstructions_current As Integer ' Which drawing instruction is being processed?
        Dim IntNumDrawingInstructions_colors As Integer ' How many pixels to color?
        Dim IntNumDrawingInstructions_colors_current As Integer ' which is the current pixel being processed/colored?
        Dim ObjColor As System.Drawing.Color ' this is the color we'll draw. 

1005:
        ' This 'while'-loop should prevent any side-effects from APE junk bytes.
        ' Often, when analyzing graphics generated by APE, you'll notice some unneccessary bytes at the very end.

        While LstFrameHex.Count > 0 And IntY < FrameCoreImageBitmap.Height

            ' First byte for each row contains the number of pixel sets.
            ' That's at least 1 block (a transparent line would give [offset][0 colors] -
            ' Otherwise, it's a 'drawing instruction': 
            ' [offset/number of pixels to remain transparent][numColorPixels][pixels]

1100:
            ' Number of drawing instructions. How many are there for this 'row' of pixels?
            ' Limitation: theoretically: 0 to 255 drawing instructions per row
            IntNumDrawingInstructions = CInt("&H" & LstFrameHex(0))
            LstFrameHex.Skip(1)

1120:
            ' Process this set of drawing instructions.
            ' Todo: replace this SetPixel() part with a faster method (LockBits). First implement a way to check if output graphics are still the same.

            For IntNumDrawingInstructions_current = 0 To (IntNumDrawingInstructions - 1)

1300:
                ' Starting with color byte( [offset] ). 
                ' If this is 00, starting all the way to the left. 
                ' If this is 01, skipping 1 pixel and leave it transparent.
                ' If this is 02, skipping 2 pixels and leave them transparent
                ' And so on.
                IntX += CInt("&H" & LstFrameHex(0))
1301:
                ' Number of pixels to color ([num of pixels to draw])
                IntNumDrawingInstructions_colors = CInt("&H" & LstFrameHex(1))

1309:
                ' Remove [offset] and [num of pixels to draw] instructions.
                LstFrameHex.Skip(2)

1400:
                ' The hex code mentioned how many colors there will be
                For IntNumDrawingInstructions_colors_current = 0 To (IntNumDrawingInstructions_colors - 1)

1410:
                    If BlnIsShadow = True Then
                        ' Marine Mania's underwater shadow format (compressed ZT1 Graphic)
                        ' It does not rely on the palette.
                        ObjColor = Color.Black
                    Else
                        ' In the traditional format, the color is referenced by it's index number in the color palette. Get it.
                        Dim IntColorIndex As Byte = CInt("&H" & LstFrameHex(IntNumDrawingInstructions_colors_current))
                        ObjColor = ZtPal.Colors(IntColorIndex)
                    End If

1413:
                    ' Color the pixel.
                    FrameCoreImageBitmap.SetPixel(IntX, IntY, ObjColor)

1450:
                    ' Be ready to draw next pixel.
                    IntX += 1


                Next IntNumDrawingInstructions_colors_current
1455:
                ' Rather than individually deleting those colors one by one from the bytes that still need to be processed, do it at once now.
                If BlnIsShadow = False Then
                    LstFrameHex.Skip(IntNumDrawingInstructions_colors_current)
                End If

2040:
            Next IntNumDrawingInstructions_current

2050:
            IntX = 0 ' Start all the way on the left of the canvas again.
            IntY += 1 ' Ready to process next line.

        End While


2100:

        ' Implemented a check for APE junk bytes and remove if any are left.
        ' Theoretically, there shouldn't be. But APE has the tendency to generate crap. (Sorry Blue Fang, but I'm sure you can agree by now?)
        If LstFrameHex.Count > 0 Then
            MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Detected APE junk bytes!")
            ' In the past, this used to be cleaned up. For now, don't even bother.
            ' Me.coreImageHex.RemoveRange(Me.coreImageHex.Count - LstFrameHex.Count - 1, LstFrameHex.Count)
        Else
            MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "No APE junk bytes.")
        End If


2110:
        ' The actual bitmap won't be changed unless a .PNG is loaded.
        ' If a .PNG is loaded, this frame's CoreImageHex should be updated as well
        Me.CoreImageBitmap = FrameCoreImageBitmap

        MdlZTStudio.Trace(Me.GetType().FullName, "RenderCoreImageFromHex", "Finished frame rendering.")

9999:
        Return FrameCoreImageBitmap

        Exit Function

dBug2:
        ' Not sure if this is really useful
        FrmMain.PicBox.Image = MdlSettings.BMEmpty


        MsgBox("Error in ClsFrame.renderCoreImageFromHex()" & vbCrLf &
               "Line " & Erl() & vbCrLf &
               "Width, height: " & FrameCoreImageBitmap.Width & ", " & FrameCoreImageBitmap.Height & vbCrLf &
            "Offset x, y: " & Me.OffsetX & ", " & Me.OffsetY & vbCrLf &
            "Colors: Currently at drawing instruction " & IntNumDrawingInstructions_current & "/" & IntNumDrawingInstructions & ", color " & IntNumDrawingInstructions_colors_current & "/" & IntNumDrawingInstructions_colors & vbCrLf &
            "Last referenced x, y: " & IntX & ", " & IntY & vbCrLf &
            "Current length of LstFrameHex: " & LstFrameHex.Count & vbCrLf &
            "Current length of colors: " & ZtPal.Colors.Count & vbCrLf &
             vbCrLf & Err.Number & " - " & Err.Description &
            vbCrLf & "Line: " & Erl(), vbOKOnly + vbCritical, "Error")



    End Function

    ''' <summary>
    ''' Sets the offsets for this frame. By default, changes are applied to all frames in this graphic rather than just to one frame.
    ''' </summary>
    ''' <param name="PntCoordOffsetChanges">Contains offset values</param>
    ''' <param name="BlnBatchFix">Fix all frames at once (is batch operation)</param>
    Public Sub UpdateOffsets(PntCoordOffsetChanges As Point, Optional BlnBatchFix As Boolean = False)

        On Error GoTo dBug
200:
        ' By default, this applies to all frames (config setting)
        If Cfg_editor_rotFix_individualFrame <> 1 Or BlnBatchFix = True Then

            ' Process every frame
            For Each ztFrame As ClsFrame In Me.Parent.Frames

                ' Update hex
                If CoreImageHex.Count > 0 Then

                    ' Assuming in all cases offsets have been set.

                    ' Update offsets of this frame
                    ztFrame.OffsetY += PntCoordOffsetChanges.Y
                    ztFrame.OffsetX += PntCoordOffsetChanges.X

                    ' Valid offsets?
                    Dim StrHintOffset As String = "Problem with a frame. Valid {0} offset should (theoretically, still untested) be between -32768 and 32767."
                    If ztFrame.OffsetX < -32768 Or ztFrame.OffsetX > 32767 Then
                        MdlZTStudio.ExpectedError(Me.GetType().FullName, "ClsFrame_UpdateOffsets", String.Format(StrHintOffset, "X"), True)
                        Exit Sub
                    End If

                    If ztFrame.OffsetY < -32768 Or ztFrame.OffsetY > 32767 Then
                        MdlZTStudio.ExpectedError(Me.GetType().FullName, "ClsFrame_UpdateOffsets", String.Format(StrHintOffset, "Y"), True)
                        Exit Sub
                    End If

                    ' .CoreImageHex(4) and (5) make up offset Y (top/bottom)
                    If ztFrame.OffsetY >= 0 Then
                        ' Positive offsets work just fine.
                        ztFrame.CoreImageHex(4) = Strings.Split(ztFrame.OffsetY.ToString("X4").ReverseHex())(0)
                        ztFrame.CoreImageHex(5) = Strings.Split(ztFrame.OffsetY.ToString("X4").ReverseHex())(1)
                    Else
                        ' Negative offsets need a different approach.
                        ' 256 (FF) * 256  (FF) = 65536. Despite the plus sign below, offset is negative, so it's substracted.
                        ' There's some great explanations on the internet:
                        ' HEX FFFF (-1) -> 8000 (-32768) are negative;
                        ' HEX 0000 (0) -> 7FFF (32767) are positive
                        ' Explained at 
                        ztFrame.CoreImageHex(4) = Strings.Split((256 * 256 + ztFrame.OffsetY).ToString("X4").ReverseHex())(0)
                        ztFrame.CoreImageHex(5) = Strings.Split((256 * 256 + ztFrame.OffsetY).ToString("X4").ReverseHex())(1)
                    End If

                    ' .CoreImageHex(6) and (7) make up offset X (left/right)
                    ' Logic: see above
                    If ztFrame.OffsetX >= 0 Then
                        ztFrame.CoreImageHex(6) = Strings.Split(ztFrame.OffsetX.ToString("X4").ReverseHex())(0)
                        ztFrame.CoreImageHex(7) = Strings.Split(ztFrame.OffsetX.ToString("X4").ReverseHex())(1)
                    Else
                        ztFrame.CoreImageHex(6) = Strings.Split((256 * 256 + ztFrame.OffsetX).ToString("X4").ReverseHex())(0)
                        ztFrame.CoreImageHex(7) = Strings.Split((256 * 256 + ztFrame.OffsetX).ToString("X4").ReverseHex())(1)
                    End If

                End If

            Next

        Else

            ' Correct the offsets and nothing else.
            ' Same logic as above.

            ' Update hex
            If CoreImageHex.Count > 0 Then

                ' Change offsets of this frame 
                Me.OffsetY += PntCoordOffsetChanges.Y
                Me.OffsetX += PntCoordOffsetChanges.X

                ' Simply change the hex
                If Me.OffsetY >= 0 Then
                    Me.CoreImageHex(4) = Strings.Split(Me.OffsetY.ToString("X4").ReverseHex())(0)
                    Me.CoreImageHex(5) = Strings.Split(Me.OffsetY.ToString("X4").ReverseHex())(0)
                Else
                    Me.CoreImageHex(4) = Strings.Split((256 * 256 + Me.OffsetY).ToString("X4").ReverseHex())(0)
                    Me.CoreImageHex(5) = Strings.Split((256 * 256 + Me.OffsetY).ToString("X4").ReverseHex())(0)
                End If

                If Me.OffsetX >= 0 Then
                    Me.CoreImageHex(6) = Strings.Split(Me.OffsetX.ToString("X4").ReverseHex())(0)
                    Me.CoreImageHex(7) = Strings.Split(Me.OffsetX.ToString("X4").ReverseHex())(0)
                Else
                    Me.CoreImageHex(6) = Strings.Split((256 * 256 + Me.OffsetX).ToString("X4").ReverseHex())(0)
                    Me.CoreImageHex(7) = Strings.Split((256 * 256 + Me.OffsetX).ToString("X4").ReverseHex())(0)
                End If

            End If

        End If

21:

        Exit Sub

dBug:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "UpdateOffsets", Information.Err)


    End Sub

    ''' <summary>
    ''' Moves frame to a different position
    ''' </summary>
    ''' <remarks>
    ''' No longer in use, but still kept just in case.
    ''' </remarks>
    ''' <param name="IntNewIndex">Integer</param>
    ''' <param name="ZtFrame">ClsFrame. Defaults to editorFrame</param>
    ''' <param name="ZtGraphic">ClsGraphic. Defaults to editorGraphic</param>
    Public Sub UpdateIndex(IntNewIndex As Integer, Optional ZtFrame As ClsFrame = Nothing, Optional ZtGraphic As ClsGraphic = Nothing)

        On Error GoTo dBug

1:
        If IsNothing(ZtFrame) Then
            ZtFrame = EditorFrame
        End If

2:
        If IsNothing(ZtGraphic) Then
            ZtGraphic = EditorGraphic
        End If

5:
        ' Get current list, remove item, add to new
        ZtGraphic.Frames.Remove(ZtFrame)

6:
        ' Add to wanted place
        ZtGraphic.Frames.Insert(IntNewIndex, ZtFrame)

        Exit Sub

dBug:

        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "UpdateIndex", Information.Err)

    End Sub

    ''' <summary>
    ''' Loads a .PNG file and converts it to HEX.
    ''' </summary>
    ''' <param name="StrFileName">File name of PNG to load</param>
    Public Sub LoadPNG(StrFileName As String)

        On Error GoTo dBug

5:
        Dim BmpDrawTemp As Bitmap = Bitmap.FromFile(StrFileName)
        Dim BmpDraw As Bitmap

10:
        ' Prevent a file lock on .PNG files.
        ' If files are locked, the files can't be automatically deleted after batch conversion.
        Using BmpDrawTemp
            BmpDraw = New Bitmap(BmpDrawTemp)
            BmpDrawTemp = Nothing
        End Using

20:
        ' The offsets should be set here first!
        ' They should NOT be changed in ClsFrame::BitMapToHex(), since they might overwrite/change updated offsets!

        ' Easy to start with: 
        ' * Define the offsets
        ' * Define the dimensions (height, width). Calculate by top/left and bottom/right pixel
        ' The offset is difficult. "Zoot" (program by MadScientist) *seemed* to handle it by setting the offset to half the height/width.
        ' That approach at least centers your image, but it might still not be desired.
        ' Following the same approach though, as it's impossible to know the correct/desired offsets at this point.
        Me.OffsetX = Math.Ceiling(BmpDraw.Width / 2) + 1
        Me.OffsetY = Math.Ceiling(BmpDraw.Height / 2) + 1

21:
        ' Get defining rectangle (dimensions)
        Dim RectCrop As Rectangle = MdlBitMap.GetDefiningRectangle(BmpDraw)

22:
        ' Get cropped version of bitmap based on this rectangle
        Dim BmpCropped As Bitmap = MdlBitMap.GetCroppedVersion(BmpDraw, RectCrop)

23:
        ' Improvement: by cropping to the relevant area, the offset should in most cases be better.
        Me.OffsetX -= RectCrop.X ' Originally centered based on width. The offset was to the left (positive). Substract from offset to move it right a bit (closer to center)
        Me.OffsetY -= RectCrop.Y ' Originally centered based on height. The offset was to the top (positive). Substract from offset to  move it down a bit (closer to center)

25:
        ' Like APE and ZOOT, by default assume the top left pixel of the imported PNG determines the transparent color of the image.
        ' So it is necessary to add the color from the original image and add that as transparent color to the palette (if still empty) 
        ' This should've been avoided by setting the background color properly, but it's easily overlooked.
        '
        ' The condition below is (mostly) meant for plaques, to AVOID this from happening.
        ' Use cases: importing an icon or plaque, centered on a transparent background.
        ' Image how they're imported: usually a photo or other visual, with no transparent colors around the borders (or in case of icon, mistakenly the top left)
        ' The cropped version will be the plaque (rectangle).
        ' This means: the top left pixel Of the cropped area IS relevant (gray/black -> (0, 0)) and should NOT be transparent. 
        '
        If RectCrop.X <> 0 And RectCrop.Y <> 0 And Me.Parent.ColorPalette.Colors.Count = 0 Then
            MdlZTStudio.Trace(Me.GetType().FullName, "LoadPng", "Defining rectangle is not starting at (0,0), color palette is empty. Add top left pixel of bitmap (input PNG) as transparent color.")
            Me.Parent.ColorPalette.Colors.Add(BmpDraw.GetPixel(0, 0))
        End If

30:
        ' ZT Studio has cropped the image.
        ' Generate hex from bitmap.
        Me.BitMapToHex(BmpCropped)

        Exit Sub

dBug:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "LoadPNG", Information.Err)

    End Sub

    ''' <summary>
    ''' Converts a bitmap to hex values. Important: offsets should have been set already!
    ''' </summary>
    ''' <remarks>
    ''' In the past, an alternative method using LockBits was implemented to find the defining rectangle.
    ''' According to the comments, it was much faster than the old method.
    ''' Could this also be applied here?
    ''' </remarks>
    ''' <param name="BmImage">Bitmap. Defaults to CoreImageBitmap</param>
    ''' <returns>List(Of String) - Hex values for the ZT1 rendering engine (single frame)</returns>
    Public Function BitMapToHex(Optional BmImage As Bitmap = Nothing) As List(Of String)

        On Error GoTo dBug

1:
        Dim LstGeneratedHex As New List(Of String)

2:
        If IsNothing(BmImage) = True Then

            ' Fall back to CoreImageBitmap, if available.
            If IsNothing(Me.CoreImageBitmap) = True Then
                ' Can this happen?
                MsgBox("ClsFrame::GetHexFromBitmap(): no bitmap was given as input. A fallback to ClsFrame::CoreImageBitmap was impossible.",
                    vbOKOnly + vbCritical + vbApplicationModal,
                    "Error while generating HEX for this frame")
                Return LstGeneratedHex ' exits further processing

            Else
                BmImage = Me.CoreImageBitmap
            End If

        Else

            ' APE / ZOOT: top left color = transparent.
            ' Rely only on that method if no colors are known in the color palette yet.
            ' Reason: it works differently in batch conversions (20190820: todo: needs more explaining. Why again? Shared pelette?)
            ' 20170519: beware: this might not work properly for (cropped) plaques!
            If Me.Parent.ColorPalette.Colors.Count = 0 Then
                Me.Parent.ColorPalette.Colors.Add(BmImage.GetPixel(0, 0))
            End If

            ' 20170519. Store BmImage. The hex values are stored as well.
            ' 20190817:
            ' * Do NOT store BmImage. It may cache a bitmap with the transparent color, such as blue.
            ' * However, on changing ZT Studio's background color to green, the blue will still be shown.
            ' * Already setting the core image bitmap here may result in unwanted side-effects!
            ' Me.CoreImageBitmap = BmImage

        End If

        ' === Rewrite.

        ' Get the defining rectangle of the .PNG image.
        ' Next, generate the hex code. That should be enough for now.

200:

        ' Reason: instead of the top left pixel, transparency could already have been determined in some (batch) processes

        Dim LstHexRows As New List(Of String) ' Store bytes as strings for now. Actual drawing instructions.

        ' Use intX, intY to move over every pixel.
        Dim IntX As Integer = 0
        Dim IntY As Integer = 0

        Dim ObjColor As System.Drawing.Color ' Will be used to go over each pixel and determine the color

        ' Take the palette from the parent
        Dim ZtPal As ClsPalette = Me.Parent.ColorPalette

        Dim LstDrawingInstructions As New List(Of ClsDrawingInstr)
        Dim ObjDrawingInstr As New ClsDrawingInstr

1000:
        ' Here it gets a bit more tricky. There's some information to process, 
        ' but some information needs to be switched around in the final output.
        ' - per drawn line (lines go from left to right, they're drawn below each other): 
        '   [number of drawing instruction blocks] [drawing instruction blocks, if any (could be 0?)]
        ' - remember how many drawing instruction blocks ([offset] + [number of color indexes to follow] + [color indexes]) there are per line;
        ' - for each drawing instruction:
        ' -- remember the offset;
        ' -- count the colors;
        ' -- keep track of the color indexes: find them in a palette, or add them. Max 255 colors, warn user if there are more colors in the Bitmap (PNG)!
        ' --> this will only have been changed if the color palette has been altered in some way.

        ' Keep in mind: a one pixel image would be pixel [0,0] but width = 1 and height = 1.

        ' Todo: implement hashes to check for regressions; improve this with LockBits instead of GetPixel().

3005:

        ' From top to bottom, from left to right
        While IntY < BmImage.Height

            ' Restart.
            IntX = 0
            ObjDrawingInstr = New ClsDrawingInstr
            LstDrawingInstructions.Clear(False)

            While IntX < BmImage.Width

3010:
                ' Read the color.
                ObjColor = BmImage.GetPixel(IntX, IntY)

                ' If the index of the color is 0, assume it's a transparent pixel.
                If Me.Parent.ColorPalette.GetColorIndex(ObjColor) = 0 Then

3100:
                    ' Assuming transparent pixel.
                    ' This can happen at the very start of the row;
                    ' This can happen after a series of colored pixels.
                    If ObjDrawingInstr.Offset = 0 And ObjDrawingInstr.PixelColors.Count = 0 Then
3101:
                        ' Most likely getting this at the very start of the row. 
                        ' No action required.

                    ElseIf ObjDrawingInstr.PixelColors.Count > 0 Then
3102:
                        ' Colors were detected before.
                        ' Now processing a transparent pixel again.
                        ' Close the previous drawing instruction, start a new one.
                        LstDrawingInstructions.Add(ObjDrawingInstr, False)
                        ObjDrawingInstr = New ClsDrawingInstr

                    Else

3108:
                        ' In this case, the offset is bigger than 0 (previous pixel was also transparent) and the color count is 0.
                        ' Don't do anything.
                        ' 20190816: shouldn't this case be merged with the first one?

                    End If

                    ' The current pixel is transparent.
                    ' Increase offset by 1.
3110:
                    ObjDrawingInstr.Offset += 1

3115:
                    ' Rare exception: if the offset is now 255 (limit), end this drawing instruction and start a new one.
                    If ObjDrawingInstr.Offset = 255 Then
                        MdlZTStudio.Trace(Me.GetType().FullName, "BitMapToHex", "This graphic has an example of a drawing instruction with a large offset (255). Add in documentation.")
                        LstDrawingInstructions.Add(ObjDrawingInstr, False)
                        ObjDrawingInstr = New ClsDrawingInstr
                    End If

                Else
3200:
                    ' Detected a colored pixel.
                    ' Get the index of this color from the palette and add it to the drawing instruction.
                    Dim tmpColorIndex = Me.Parent.ColorPalette.GetColorIndex(ObjColor, True)
                    ObjDrawingInstr.PixelColors.Add(tmpColorIndex, False)

3399:
                    ' Rare exception: if the number of colored pixels is now 255 (limit), end this drawing instruction and start a new one.
                    If ObjDrawingInstr.PixelColors.Count = 255 Then
                        MdlZTStudio.Trace(Me.GetType().FullName, "BitMapToHex", "This graphic is an example has an example of a drawing instruction with a color offset (255). Add in documentation.")
                        LstDrawingInstructions.Add(ObjDrawingInstr, False)
                        ObjDrawingInstr = New ClsDrawingInstr
                    End If

                End If

                IntX += 1
            End While


            ' === END OF LINE ===

3400:
            ' All pixels have been processed.
            ' This means the last drawing instruction is likely still open (unless it was just ended after hitting one of the above limits).
            ' Check if this drawing instruction contains an offset or color.
            If ObjDrawingInstr.Offset <> 0 Or ObjDrawingInstr.PixelColors.Count > 0 Then
                LstDrawingInstructions.Add(ObjDrawingInstr, False)
            End If

3405:
            ' All drawing instructions for this line are prepared.
            ' So to LstHexRows, add:
            ' Number of instruction blocks [between 0 - 255]
            LstHexRows.Add(LstDrawingInstructions.Count.ToString("X2"), False)

3406:
            For Each diInstruction As ClsDrawingInstr In LstDrawingInstructions
                ' For each block: 
                ' - get hex: [offset][num colors][color indexes of pixels]
                LstHexRows.AddRange(diInstruction.GetHex(), False)
            Next

3450:

            IntY += 1
        End While


9000:

        With LstGeneratedHex

9001:
            ' Easier to build it this way. Start by writing the dimensions: height, width.
            .AddRange(Strings.Split(BmImage.Height.ToString("X4").ReverseHex(), " "), False)
            .AddRange(Strings.Split(BmImage.Width.ToString("X4").ReverseHex(), " "), False)

9002:
            If Me.OffsetY >= 0 Then
                .AddRange(Strings.Split(Me.OffsetY.ToString("X4").ReverseHex(), " "), False)
            Else
                .AddRange(Strings.Split((256 * 256 + Me.OffsetY).ToString("X4").ReverseHex(), " "), False)

            End If

            If Me.OffsetX >= 0 Then
                .AddRange(Strings.Split(Me.OffsetX.ToString("X4").ReverseHex(), " "), False)
            Else
                .AddRange(Strings.Split((256 * 256 + Me.OffsetX).ToString("X4").ReverseHex(), " "), False)
            End If

9003:
            ' Issue: two unknown bytes. ('mystery bytes')
            ' For bamboo, frame 1 = 1. 
            ' Always seems to be 1 in APE.
            .Add("01", False)
            .Add("00", False)

9020:
            ' Now add the drawing instructions for the frame.
            .AddRange(LstHexRows, False)

        End With

9502:
        ' Reset. Should be regenerated from the hex.
        ' Me.CoreImageBitmap = Nothing - 20170519 - what was the point again in setting this to nothing?
        Me.CoreImageHex = LstGeneratedHex

        Return Me.CoreImageHex

        Exit Function

dBug:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "BitMapToHex", Information.Err)


    End Function

    ''' <summary>
    ''' Saves the frame of a ZT1 Graphic to a .PNG file
    ''' </summary>
    ''' <param name="StrFileName">Destination filename</param>
    Public Sub SavePNG(StrFileName As String)

        On Error GoTo dBug
10:

        Dim BmRect As New Rectangle(-9999, -9999, 0, 0)
        Dim BmCropped As Bitmap

        ' 0 = canvas size
        ' 1 = relevant pixel area of graphic
        ' 2 = relevant pixel area of frame
        ' 3 = around the grid origin of frame

        Select Case Cfg_export_PNG_CanvasSize


            Case 0
                ' Save PNG image. Complete canvas size.
21:

                Dim ImgComb As Image
                ImgComb = New Bitmap(Cfg_grid_numPixels * 2, Cfg_grid_numPixels * 2)

32:
                ' Use ZT Studio's main window background color (transparent) or export with an entirely transparent background (user's choice)
                Using ObjGraphic As Graphics = Graphics.FromImage(ImgComb)
                    ObjGraphic.Clear(IIf(Cfg_export_PNG_TransparentBG = 0, Cfg_grid_BackGroundColor, Color.Transparent))
                End Using

35:
                ImgComb = MdlBitMap.CombineImages(ImgComb, Me.GetImage())
                ImgComb.Save(StrFileName, System.Drawing.Imaging.ImageFormat.Png)

            Case 1
                ' Save PNG image. Relevant pixel area of graphic
131:
                ' Cheap trick: combine all images into 1, then get the relevant rectangle.
                ' Some caching might be in order in the future :)

                Dim ImgComb As Image
                ImgComb = New Bitmap(Cfg_grid_numPixels * 2, Cfg_grid_numPixels * 2)

132:
                ' Use ZT Studio's main window background color (transparent) or export with an entirely transparent background (user's choice)
                Using ObjGraphic As Graphics = Graphics.FromImage(ImgComb)
                    ObjGraphic.Clear(IIf(Cfg_export_PNG_TransparentBG = 0, Cfg_grid_BackGroundColor, Color.Transparent))
                End Using
135:

                ' Combine all images. Basically put them all on top of each other.
                ' That way, it's easy to determine the most relevant pixel top/left and bottom/right
                For Each ztFrame As ClsFrame In Me.Parent.Frames
                    ImgComb = MdlBitMap.CombineImages(ImgComb, ztFrame.GetImage())
                Next

                ' Apply to this particular frame.
                BmRect = MdlBitMap.GetDefiningRectangle(ImgComb)
                BmCropped = MdlBitMap.GetCroppedVersion(Me.GetImage(), BmRect)
                BmCropped.Save(StrFileName, System.Drawing.Imaging.ImageFormat.Png)

            Case 2
                ' Save PNG image. Relevant area of frame.

141:
                BmRect = MdlBitMap.GetDefiningRectangle(Me.GetImage())
                BmCropped = MdlBitMap.GetCroppedVersion(Me.GetImage(), BmRect)
                BmCropped.Save(StrFileName, System.Drawing.Imaging.ImageFormat.Png)

            Case 3
                ' Center around the origin. This method has been contributed by HENDRIX
                ' This is much faster and avoids all cropping, but preserves the offset
                Me.GetImage(False, True).Save(StrFileName, System.Drawing.Imaging.ImageFormat.Png)

        End Select

        Exit Sub

dBug:
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "SavePNG", Information.Err)


    End Sub

End Class
