Option Explicit On

Imports System.IO
Imports System.Text.RegularExpressions

''' <summary>
''' Main user interface
''' </summary>
Public Class FrmMain

    ''' <summary>
    ''' Sets some info on loading
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Done to increase performance of adding colors to this palette.
        DgvPaletteMain.GetType.InvokeMember("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.SetProperty, Nothing, DgvPaletteMain, New Object() {True})

        ' Always start with a new ZT1 Graphic with one frame
        EditorFrame = New ClsFrame(EditorGraphic)
        EditorGraphic.Frames.Add(EditorFrame)

        ' Starting with an empty canvas
        MdlSettings.BMEmpty = New Bitmap(Cfg_grid_numPixels * 2, Cfg_grid_numPixels * 2)
        With PicBox
            .Width = Cfg_grid_numPixels * 2
            .Height = Cfg_grid_numPixels * 2
        End With

        ' Background color derived from settings (based on previously configured settings)
        PicBox.BackColor = Cfg_grid_BackGroundColor

        ' Set grid size (based on previously configured settings)
        TsbFrame_fpX.Text = CStr(Cfg_grid_footPrintX)
        TsbFrame_fpY.Text = CStr(Cfg_grid_footPrintY)
        TsbFrame_OffsetAll.Checked = (Cfg_editor_rotFix_individualFrame * -1)

        ' ZT1 Default color palettes
        ' strPathBuildingColorPals

        Dim LstColorpalettes As IO.FileInfo()
        LstColorpalettes = New IO.DirectoryInfo(Cfg_path_ColorPals8).GetFiles()
        Dim ObjFileInfo As IO.FileInfo

        ' List all files found in the directory with 8-color palettes
        For Each ObjFileInfo In LstColorpalettes
            TsbOpenPalBldg8.DropDownItems.Add(ObjFileInfo.Name)
        Next

        LstColorpalettes = New IO.DirectoryInfo(Cfg_path_ColorPals16).GetFiles()

        ' List all files found in the directory with 16-color palettes
        For Each ObjFileInfo In LstColorpalettes
            TsbOpenPalBldg16.DropDownItems.Add(ObjFileInfo.Name)
        Next

        ' Update Explorer Panel to show folder structure of root folder
        MdlZTStudioUI.UpdateExplorerPane()

        ' If exists, load ZT1 Graphic. Won't decrease performance a lot and might be helpful while working on a project
        If File.Exists(Cfg_path_recentZT1) = True Then
            MdlZTStudioUI.LoadGraphic(Cfg_path_recentZT1)
        End If

        ' Make sure everything is finished. Needed?
        Application.DoEvents()

    End Sub

    ''' <summary>
    ''' Handles mouse movements. Shows
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub PicBox_MouseMove(sender As Object, e As MouseEventArgs) Handles PicBox.MouseMove

        On Error GoTo dBug

1:
        ' Canvas is still entirely empty
        If IsNothing(PicBox.Image) = True Then
            MdlZTStudio.Trace(Me.GetType().FullName, "MouseMove", "Picture box empty")
            Exit Sub
        End If

2:
        ' Frame might have been just initiated
        If IsNothing(EditorFrame.CoreImageBitmap) And IsNothing(EditorFrame.CoreImageHex) Then
            MdlZTStudio.Trace(Me.GetType().FullName, "MouseMove", "EditorFrame has no CoreImageBitmap or CoreImageHex")
            Exit Sub
        End If
3:
        ' This is a bit of a dilemma. 
        ' If using PicBox.image, it also shows the grid color on hovering. Ignoring the grid color is a bad idea since the color may be present in the image too.
        ' If just using editorFrame.GetImage(), it won't show colors of any background graphic (toy for Orang Utan).
        ' Todo: combining them makes more sense but is more intensive. Should be cached somewhere.
        ' Images_Combine(editorFrame.GetImage(), editorBgGraphic.getimage())
        Dim BmTmp As Bitmap
        BmTmp = PicBox.Image ' Used to be EditorFrame.GetImage()


20:
        ' Find out which pixel area matters within the PicBox
        ' Offset compared to PicBox Left/Top
        ' Keep in mind: BmTmp is NOT necessarily the entire PicBox (adjusts to window)
        Dim IntOffsetX As Integer = (PicBox.Width - BmTmp.Width) / 2 ' Left = positive; right = negative
        Dim IntOffsetY As Integer = (PicBox.Height - BmTmp.Height) / 2 ' Top = positive; bottom = negative

100:

        MdlZTStudio.Trace(Me.GetType().FullName, "MouseMove", "e.X = " & e.X & ", Y = " & e.Y)
        MdlZTStudio.Trace(Me.GetType().FullName, "MouseMove", "Offset X = " & IntOffsetX & ", Y = " & IntOffsetY)
        MdlZTStudio.Trace(Me.GetType().FullName, "MouseMove", "Bmp width = " & BmTmp.Width & ", Height = " & BmTmp.Height)

        If e.X > IntOffsetX And e.X < (BmTmp.Width + IntOffsetX) And e.Y > IntOffsetY And e.Y < (BmTmp.Height + IntOffsetY) Then

101:
            ' Image might be smaller, while PicBox appears larger due to background color.
            Dim ObjColor As System.Drawing.Color = BmTmp.GetPixel(e.X - IntOffsetX, e.Y - IntOffsetY)

            ' Alpha channel is not set to 0 (transparent). This check is still from when using BmpTmp = EditorFrame.GetImage()
            ' Display color info
            If ObjColor.A <> 0 Then

102:
                LblColor.BackColor = ObjColor
                LblColorDetails.Text = "" &
                    "Coordinates: x: " & e.X - IntOffsetX & " , y: " & e.Y - IntOffsetY & vbCrLf &
                    "RGB: " & ObjColor.R & "," & ObjColor.G & "," & ObjColor.B & vbCrLf &
                    "Index in .pal file: # " & EditorGraphic.ColorPalette.Colors.IndexOf(ObjColor) & vbCrLf &
                    "VB.Net: " & ObjColor.ToArgb()

            Else

112:
                ' Definitely just the background color.
                LblColor.BackColor = PicBox.BackColor
                LblColorDetails.Text = vbNullString

            End If

        Else

122:
            LblColor.BackColor = PicBox.BackColor
            LblColorDetails.Text = vbNullString

        End If

        Exit Sub

dBug:
        MdlZTStudio.UnhandledError(Me.GetType().FullName, "PicBox_MouseMove", Information.Err)

    End Sub

    ''' <summary>
    ''' This is unfinished, but it was meant to be a zoom function.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub PicBox_MouseWheel(sender As Object, e As MouseEventArgs) Handles PicBox.MouseWheel

        Debug.Print("Picbox Wheel")
        Exit Sub

        ' it should be the image, not the picbox!
        If e.Delta <> 0 Then
            If e.Delta <= 0 Then
                If PicBox.Width < 500 Then
                    Exit Sub 'minimum 500?
                End If
            Else
                If PicBox.Width > 2000 Then
                    Exit Sub 'maximum 2000?
                End If
            End If

            PicBox.Width += CInt(PicBox.Width * e.Delta / 1000)
            PicBox.Height += CInt(PicBox.Height * e.Delta / 1000)
        End If

    End Sub

    ''' <summary>
    ''' Handles value changes in frame slider control.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TTbFrames_ValueChanged(sender As Object, e As EventArgs)

        ' Update canvas. 
        ' UI: update frame info; don't update button states
        MdlZTStudioUI.UpdatePreview(True, False, TbFrames.Value - 1)

    End Sub

    ''' <summary>
    ''' If "play animation" has been checked, the timer updates the preview. Timer interval = graphic animation speed.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TmrAnimation_Tick(sender As Object, e As EventArgs) Handles TmrAnimation.Tick

        MdlZTStudio.Trace("TmrAnimation", "Tick", "Interval = " & TmrAnimation.Interval.ToString())

        ' Reset if maximum value has already been reached
        ' Else, show next frame
        If (TbFrames.Value = TbFrames.Maximum) Then
            TbFrames.Value = 1
        Else
            TbFrames.Value += 1
        End If

        ' Update canvas.
        ' UI: update frame info; don't update button states
        MdlZTStudioUI.UpdatePreview(True, False, TbFrames.Value - 1)

    End Sub


    ''' <summary>
    ''' Handles toolbar button click to change background color
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbCanvasBG_Click(sender As Object, e As EventArgs) Handles TsbCanvasBG.Click

        With DlgColor
            .Color = Cfg_Grid_BackGroundColor
            .ShowDialog()

            ' Remember this color
            Cfg_Grid_BackGroundColor = .Color


        End With

        ' Save this change
        MdlConfig.Write()

        ' Update UI in preview
        MdlZTStudioUI.UpdatePreview(False, False)

    End Sub

    ''' <summary>
    ''' Handles toolbar button click to open ZT1 Graphic
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbZT1Open_Click(sender As Object, e As EventArgs) Handles TsbZT1Open.Click

        MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Open ZT1 file dialog.")
        MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Last used file: " & Cfg_Path_RecentZT1)

        ' Show dialog to open a ZT1 Graphic
        With DlgOpen
            .Title = "Pick a ZT1 Graphic"
            .DefaultExt = ""
            .Filter = "All files|*.*"

            ' Default to path of last used graphic
            .InitialDirectory = New FileInfo(Cfg_Path_RecentZT1).Directory.FullName

            ' If that path doesn't exist: attempt fallback to default game locations on x86 and x64 systems
            If DlgOpen.InitialDirectory = vbNullString Or System.IO.Directory.Exists(DlgOpen.InitialDirectory) = False Then
                If System.IO.Directory.Exists(Cfg_Path_Root) Then
                    MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Open ZT1 file dialog. Fallback to root: " & Cfg_Path_Root)
                    .InitialDirectory = Cfg_Path_Root
                ElseIf System.IO.Directory.Exists("C:\Program Files (x86)\Microsoft Games\Zoo Tycoon") Then
                    MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Open ZT1 file dialog. Fallback to Program Files (x86)")
                    .InitialDirectory = "C:\Program Files (x86)\Microsoft Games\Zoo Tycoon"
                ElseIf System.IO.Directory.Exists("C:\Program Files\Microsoft Games\Zoo Tycoon") Then
                    MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Open ZT1 file dialog. Fallback to Program Files")
                    .InitialDirectory = "C:\Program Files\Microsoft Games\Zoo Tycoon"
                End If
            End If

            ' User did NOT cancel
            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Load ZT1 Graphic
                MdlZTStudioUI.LoadGraphic(DlgOpen.FileName)

            End If

        End With
    End Sub

    ''' <summary>
    ''' Handles value change of trackbar (slider) and updates preview accordingly
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TbFrames_ValueChanged(sender As Object, e As EventArgs) Handles TbFrames.ValueChanged

        ' Update preview in UI
        MdlZTStudioUI.UpdatePreview(True, False, TbFrames.Value - 1)

        ' Update current editor frame value
        EditorFrame = EditorGraphic.Frames(TbFrames.Value - 1)

    End Sub

    ''' <summary>
    ''' Handles toolbar button click to show about info (credits, version info)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbAbout_Click(sender As Object, e As EventArgs) Handles TsbAbout.Click

        MsgBox("About " & Application.ProductName & " " & Application.ProductVersion & vbCrLf &
            Strings.StrDup(50, "_") & vbCrLf &
            "© since 2015 by Jeffrey Bostoen" & vbCrLf &
            Cfg_GitHub_URL & vbCrLf &
            vbCrLf & vbCrLf &
            "" &
            "Bugs? " & vbCrLf &
            Strings.StrDup(25, "-") & vbCrLf &
            "- You can report them at GitHub or Zoo Tek Phoenix. " & vbCrLf &
            "- Support not guaranteed. " & vbCrLf &
            "- Include the graphic files which are causing the problem. " & vbCrLf & vbCrLf &
            "" &
            "Credits? " & vbCrLf &
            Strings.StrDup(25, "-") & vbCrLf &
            "- Blue Fang for creating Zoo Tycoon 1 (and maybe the graphic format)." & vbCrLf &
            "- Microsoft for publishing the game." & vbCrLf &
            "- Rapan Studios for the animal designs." & vbCrLf &
            "- MadScientist and Jay for explaining the file format." & vbCrLf &
            "- Vondell for providing new PNG graphics to experiment with." & vbCrLf &
            "- HENDRIX for some contributions to the source code.",
            MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "About ZT Studio")


    End Sub

    ''' <summary>
    ''' Handles toolbar button click to export frame as PNG
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbFrame_ExportPNG_Click(sender As Object, e As EventArgs) Handles TsbFrame_ExportPNG.Click

        ' Show save dialog
        With DlgSave
            .Title = "Save current frame as .PNG"
            .DefaultExt = ".png"
            .AddExtension = True
            .Filter = "PNG files (*.png)|*.png|All files|*.*"

            ' Suggest path based on most recently saved PNG
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_Path_RecentPNG)

            ' User did not cancel? Then save.
            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Save current frame as PNG
                EditorFrame.SavePNG(DlgSave.FileName)

                ' Remember most recent PNG path
                Cfg_Path_RecentPNG = New System.IO.FileInfo(DlgSave.FileName).Directory.FullName
                MdlConfig.Write()

            End If

        End With


    End Sub


    ''' <summary>
    ''' Handles toolbar button click to open ZT1 Color palette (.pal file)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbZT1_OpenPal_Click(sender As Object, e As EventArgs) Handles TsbZT1_OpenPal.Click

        With DlgOpen

            .Title = "Pick a ZT1 Color Palette"
            .DefaultExt = ".pal"
            .Filter = "ZT1 Color Palette files (*.pal)|*.pal|All files|*.*"

            ' Set directory by default to where a ZT1 Graphic was last opened
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_Path_RecentZT1)

            ' If the user didn't press cancel, load palette.
            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                MdlColorPalette.LoadPalette(DlgOpen.FileName)

            End If

        End With

    End Sub

    ''' <summary>
    ''' Handles toolbar button click to modify Settings
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbSettings_Click(sender As Object, e As EventArgs) Handles TsbSettings.Click

        ' Show Settings window
        FrmSettings.ShowDialog()

    End Sub

    ''' <summary>
    ''' Handles clicking a menu item in the list of 8-color palettes
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">ToolStripItemClickedEventArgs</param>
    Private Sub TsbOpenPalBldg8_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles TsbOpenPalBldg8.DropDownItemClicked

        ' Load color palette (in its own window)
        MdlColorPalette.LoadPalette(Cfg_Path_ColorPals8 & "\" & e.ClickedItem.Text)

    End Sub

    ''' <summary>
    ''' Handles clicking a menu item in the list of 16-color palettes
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">TooLStripItemClickedEventArgs</param>
    Private Sub TsbOpenPalBldg16_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles TsbOpenPalBldg16.DropDownItemClicked

        ' Load color palette (in its own window)
        MdlColorPalette.LoadPalette(Cfg_Path_ColorPals16 & "\" & e.ClickedItem.Text)

    End Sub

    ''' <summary>
    ''' Handles toolbar button click to start batch conversion (ZT1 Graphic - PNG)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbBatchConversion_Click(sender As Object, e As EventArgs) Handles TsbBatchConversion.Click

        ' Show window to start batch conversion
        FrmBatchConversion.ShowDialog(Me)

    End Sub

    ''' <summary>
    ''' Handles toolbar button click to pick a ZT1 Graphic to use as background
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbPreview_BGGraphic_Click(sender As Object, e As EventArgs) Handles TsbPreview_BGGraphic.Click

        ' Show dialog
        With DlgOpen
            .Title = "Pick a ZT1 Graphic"
            .DefaultExt = ""
            .Filter = "All files|*.*"

            If Directory.Exists(New System.IO.FileInfo(Cfg_Path_RecentZT1).Directory.FullName) = False Then

                ' If this directory does not exist, try default game directory on x86 and x64 systems
                If System.IO.Directory.Exists("C:\Program Files\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files\Microsoft Games\Zoo Tycoon"
                ElseIf System.IO.Directory.Exists("C:\Program Files (x86)\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files (x86)\Microsoft Games\Zoo Tycoon"
                End If

            Else

                ' As for initial directory, use the one from the last picked ZT1 Graphic
                .InitialDirectory = New System.IO.FileInfo(Cfg_Path_RecentZT1).Directory.FullName

            End If

            ' If user didn't cancel, load background graphic.
            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                If System.IO.File.Exists(DlgOpen.FileName) = True Then

                    If Path.GetExtension(DlgOpen.FileName) <> vbNullString Then

                        ' The file path has an extension.
                        ' So it's not a ZT1 Graphic
                        Dim StrMessage As String =
                               "You selected a file with the extension '" & Path.GetExtension(DlgOpen.FileName) & "'." & vbCrLf &
                               "With ZT1 graphic, we mean a ZT1 graphics file without extension."

                        MdlZTStudio.HandledError(Me.GetType().FullName, "TsbPreview_BGGraphic_Click", StrMessage, False)

                    Else

                        ' Add background graphic
                        EditorBgGraphic.Read(DlgOpen.FileName)

                        ' Update preview. No need to update frame info, button states (GUI)
                        MdlZTStudioUI.UpdatePreview(False, False)

                        ' Remember this graphic as last selected
                        Cfg_Path_RecentZT1 = DlgOpen.FileName
                        MdlConfig.Write()

                    End If
                Else

                    ' File does not exist (for some reason)
                    MdlZTStudio.HandledError(Me.GetType().FullName, "TsbPreview_BGGraphic_Click", "File does not exist.", False)

                End If

            End If


        End With
    End Sub


    ''' <summary>
    ''' Handles toolbar button click to add a new empty frame. (or on right click: to immediately add PNG as new frame)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbFrame_Add_Click(sender As Object, e As EventArgs) Handles TsbFrame_Add.Click

        On Error GoTo dBug

0:
        ' New ClsFrame
        Dim ObjFrame As New ClsFrame(EditorGraphic)
2:

10:
        ' Add it to the list of frames (after the currently displayed one)
        EditorGraphic.Frames.Insert(TbFrames.Value, ObjFrame)


        ' Update preview. Update frame info and other GUI elements (button states, offsets, ...)
        MdlZTStudioUI.UpdatePreview(True, True, TbFrames.Value)

        Exit Sub

dBug:
        MdlZTStudio.UnhandledError(Me.GetType().FullName, "TsbFrame_Add_Click", Information.Err)



    End Sub


    ''' <summary>
    ''' Handles toolbar button click to delete an existing frame
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbFrame_Delete_Click(sender As Object, e As EventArgs) Handles TsbFrame_Delete.Click

        ' Remove the frame
        EditorGraphic.Frames.RemoveAt(TbFrames.Value - 1)

        ' Update preview. Update frame info and other GUI elements (button states, offsets, ...)
        MdlZTStudioUI.UpdatePreview(True, True, TbFrames.Value - 1)


    End Sub

    ''' <summary>
    ''' Handles toolbar button click to show the next frame
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbFrame_IndexIncrease_Click(sender As Object, e As EventArgs) Handles TsbFrame_IndexIncrease.Click

        ' Change handled in slider control
        TbFrames.Value += 1

        MdlZTStudioUI.UpdateGUI("TsbFrame_IndexIncrease_Click")

    End Sub


    ''' <summary>
    ''' Handles toolbar button click to move the frame contents up
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub TsbFrame_OffsetUp_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_OffsetUp.MouseDown

        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            EditorFrame.UpdateOffsets(New Point(0, 16))
        Else
            EditorFrame.UpdateOffsets(New Point(0, 1))
        End If

        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub

    ''' <summary>
    ''' Handles toolbar button click to move the frame contents down
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub TsbFrame_OffsetDown_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_OffsetDown.MouseDown


        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            EditorFrame.UpdateOffsets(New Point(0, -16))
        Else
            EditorFrame.UpdateOffsets(New Point(0, -1))
        End If

        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub

    ''' <summary>
    ''' Handles toolbar button click to move the frame contents to the left
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub TsbFrame_OffsetLeft_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_OffsetLeft.MouseDown



        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            EditorFrame.UpdateOffsets(New Point(16, 0))
        Else
            EditorFrame.UpdateOffsets(New Point(1, 0))
        End If

        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub

    ''' <summary>
    ''' Handles toolbar button click to move the frame contents to the right
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub TsbFrame_OffsetRight_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_OffsetRight.MouseDown


        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            EditorFrame.UpdateOffsets(New Point(-16, 0))
        Else
            EditorFrame.UpdateOffsets(New Point(-1, 0))
        End If

        MdlZTStudioUI.UpdatePreview(True, False)



    End Sub

    ''' <summary>
    ''' Handles toolbar button click to indicate whether this graphic has an extra frame or not
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbGraphic_ExtraFrame_Click(sender As Object, e As EventArgs) Handles TsbGraphic_ExtraFrame.Click

        EditorGraphic.HasBackgroundFrame = Math.Abs(EditorGraphic.HasBackgroundFrame - 1)

        ' Quick fix: on change, revert to frame 1.
        Dim IntFrameNumber As Integer = EditorGraphic.Frames.IndexOf(EditorFrame)

        ' Was displaying last frame?
        If (IntFrameNumber = EditorGraphic.Frames.Count - 1) Then
            ' Show first one instead
            MdlZTStudioUI.UpdatePreview(True, True, 0)
        Else
            ' Update current frame
            MdlZTStudioUI.UpdatePreview(True, True)
        End If



    End Sub

    ''' <summary>
    ''' Handles toolbar button click to show the previous frame
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbFrame_IndexDecrease_Click(sender As Object, e As EventArgs) Handles TsbFrame_IndexDecrease.Click

        ' Change handled in slider control
        TbFrames.Value -= 1

        ' Update preview
        MdlZTStudioUI.UpdateGUI("TsbFrame_IndexDecrease_Click")

    End Sub


    ''' <summary>
    ''' Handles toolbar button click to delete PNG files in the root folder
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbDelete_PNG_Click(sender As Object, e As EventArgs) Handles TsbDelete_PNG.Click

        ' Clean up files is generic, hence the specific messagebox needs to be shown here
        MdlTasks.CleanUp_Files(Cfg_Path_Root, ".png")
        MdlZTStudio.InfoBox("MdlTasks", "CleanUp_Files", "Deleted all .PNG files in the root folder.")

    End Sub

    ''' <summary>
    ''' Handles toolbar button click to delete ZT1 Graphic files and color palettes in the root folder
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbDelete_ZT1Files_Click(sender As Object, e As EventArgs) Handles TsbDelete_ZT1Files.Click

        ' Cleanup ZT1 Graphics and color palettes
        MdlTasks.CleanUp_Files(Cfg_Path_Root, "")
        MdlTasks.CleanUp_Files(Cfg_Path_Root, ".pal")
        MdlTasks.CleanUp_Files(Cfg_Path_Root, ".ani")
        MdlZTStudio.InfoBox("MdlTasks", "CleanUp_Files", "Deleted all ZT1 Graphic files (including .ani and .pal) in the root folder.")

    End Sub



    ''' <summary>
    ''' Handles footprint (X) selection change
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbFrame_fpX_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TsbFrame_fpX.SelectedIndexChanged

        ' Update setting and remember
        Cfg_grid_footPrintX = TsbFrame_fpX.Text
        MdlConfig.Write()

        ' Update preview. Update frame info, not GUI (button states etc)
        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub

    ''' <summary>
    ''' Handles footprint (Y) selection change
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbFrame_fpY_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TsbFrame_fpY.SelectedIndexChanged

        ' Update setting and remember
        Cfg_grid_footPrintY = TsbFrame_fpY.Text
        MdlConfig.Write()

        ' Update preview. Update frame info, not GUI (button states etc)
        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub


    ''' <summary>
    ''' Handles double clicking a color in the datagridview (color palette on the right)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">DataGridViewCellEventArgs</param>
    Private Sub DgvPaletteMain_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvPaletteMain.CellDoubleClick

        ' Actual row rather than header. Avoid crash.
        If e.RowIndex > -1 Then

            ' Replace colors
            MdlColorPalette.ReplaceColor(e.RowIndex)

        End If

    End Sub


    ''' <summary>
    ''' Handles clicking a color in the datagridview (color palette on the right), triggers menu
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">DataGridViewCellEventArgs</param>
    Private Sub DgvPaletteMain_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DgvPaletteMain.CellMouseClick

        If e.RowIndex <> -1 Then

            ' Set selected
            DgvPaletteMain.Rows(e.RowIndex).Selected = True

        End If


        If e.Button = Windows.Forms.MouseButtons.Right Then

            ' Which options to move the color up, down or to the end are enabled?
            mnuPal_MoveDown.Visible = (e.RowIndex <> DgvPaletteMain.Rows.Count - 1)
            mnuPal_MoveEnd.Visible = (e.RowIndex <> DgvPaletteMain.Rows.Count - 1)
            mnuPal_MoveUp.Visible = (e.RowIndex <> 0)

            ' Display menu
            MnuPal.Show(Cursor.Position)

        End If

    End Sub

    ''' <summary>
    ''' Handles clicking 'move color up' in the palette menu.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub MnuPal_MoveUp_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveUp.Click

        MdlColorPalette.MoveColor(DgvPaletteMain.SelectedRows(0).Index, DgvPaletteMain.SelectedRows(0).Index - 1)

    End Sub

    ''' <summary>
    ''' Handles clicking 'move color down' in the palette menu.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub MnuPal_MoveDown_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveDown.Click

        MdlColorPalette.MoveColor(DgvPaletteMain.SelectedRows(0).Index, DgvPaletteMain.SelectedRows(0).Index + 1)

    End Sub


    ''' <summary>
    ''' Handles clicking 'replace color' in the palette menu.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub MnuPal_Replace_Click(sender As Object, e As EventArgs) Handles mnuPal_Replace.Click

        MdlColorPalette.ReplaceColor(DgvPaletteMain.SelectedRows(0).Index)


    End Sub

    ''' <summary>
    ''' Handles clicking 'move color to the end' in the palette menu.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub MnuPal_MoveEnd_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveEnd.Click

        MdlColorPalette.MoveColor(DgvPaletteMain.SelectedRows(0).Index, EditorGraphic.ColorPalette.Colors.Count - 1)


    End Sub

    ''' <summary>
    ''' Handles clicking 'add color' in the palette menu.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub MnuPal_Add_Click(sender As Object, e As EventArgs) Handles mnuPal_Add.Click

        ' Add after this entry
        MdlColorPalette.AddColor(DgvPaletteMain.SelectedRows(0).Index)

    End Sub


    ''' <summary>
    ''' Handles toolbar button click to import a PNG image into the current frame (or a new one if right click)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub TsbFrame_ImportPNG_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_ImportPNG.MouseDown

        ' Shortcut to create a new frame first, then import the PNG to it.

        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            ' Add frame
0:
            Dim ObjFrame As New ClsFrame(EditorGraphic)

10:
            ' Add the frame after the existing one(s)
            EditorGraphic.Frames.Insert(TbFrames.Value, ObjFrame)

15:
            ' not sure if this is right if an extra frame is applied?
            TbFrames.Maximum = EditorGraphic.Frames.Count - EditorGraphic.HasBackgroundFrame

16:
            TbFrames.Value += 1

            ' Update current preview. Update frame info (offsets), GUI (button states, counts etc)
            MdlZTStudioUI.UpdatePreview(True, True, TbFrames.Value - 1)

        End If



100:

        With DlgOpen
            .Title = "Pick a .PNG file"
            .DefaultExt = ""
            .Filter = "PNG files|*.png"

            ' Suggest directory of most recently opened PNG
            .InitialDirectory = New System.IO.FileInfo(Cfg_Path_RecentPNG).Directory.FullName


            ' If most recent directory does not exist anymore:
            If DlgOpen.InitialDirectory = vbNullString Or System.IO.Directory.Exists(DlgOpen.InitialDirectory) = False Then
                .InitialDirectory = Cfg_Path_Root

            End If

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                If System.IO.File.Exists(DlgOpen.FileName) = True Then

                    If Path.GetExtension(DlgOpen.FileName).ToLower() <> ".png" Then

                        Dim StrMessage As String = "" &
                            "You selected a file with the extension '" & Path.GetExtension(DlgOpen.FileName) & "'." & vbCrLf &
                            "You need a file with a .PNG extension."

                        MdlZTStudio.HandledError(Me.GetType().FullName, "TsbFrame_ImportPNG_MouseDown", StrMessage)

                    Else

                        ' OK
                        EditorFrame.LoadPNG(DlgOpen.FileName)

                        ' Draw first frame 
                        MdlZTStudioUI.UpdatePreview(True, True)

                        ' Show main color palette
                        EditorFrame.Parent.ColorPalette.FillPaletteGrid(DgvPaletteMain)

                        ' Not sure why we had this. It's the color palette of the background graphic.
                        'editorBgGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)

                        ' Remember
                        Cfg_Path_RecentPNG = New System.IO.FileInfo(DlgOpen.FileName).Directory.FullName
                        MdlConfig.Write()


                    End If
                Else
                    MdlZTStudio.HandledError(Me.GetType().FullName, "TsbFrame_ImportPNG_MouseDown", "File does not exist.", False)

                End If

            End If


        End With


    End Sub




    ''' <summary>
    ''' Handles toolbar button click to import a PNG image into the current frame (or a new one if right click)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub MnuPal_ExportPNG_Click(sender As Object, e As EventArgs) Handles mnuPal_ExportPNG.Click

        With DlgSave

            .Title = "Save as a PNG Color Palette"
            .DefaultExt = ".png"
            .Filter = "PNG Color Palette files (*.png)|*.png|All files|*.*"

            ' By default same directory as most recently picked ZT1 Graphic
            .InitialDirectory = New System.IO.FileInfo(Cfg_Path_RecentZT1).Directory.FullName

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Export the frame to a PNG image
                EditorGraphic.ColorPalette.ExportToPNG(DlgSave.FileName)

            End If

        End With

    End Sub



    ''' <summary>
    ''' Handles menu click to import a PNG image as a color palette
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub MnuPal_ImportPNG_Click(sender As Object, e As EventArgs) Handles mnuPal_ImportPNG.Click



        With DlgOpen

            .Title = "Pick a PNG Color Palette"
            .DefaultExt = ".png"
            .Filter = "PNG Color Palette files (*.png)|*.png|All files|*.*"

            ' By default, specify directory of last chosen PNG file
            .InitialDirectory = New System.IO.FileInfo(Cfg_Path_RecentPNG).Directory.FullName


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Replace palette file (should trigger a re-draw AFTERWARDS )
                ' Forcefully add colors (some might be the same, after a recolor)
                EditorGraphic.ColorPalette.ImportFromPNG(DlgOpen.FileName)

                ' Update color list on the right
                EditorGraphic.ColorPalette.FillPaletteGrid(DgvPaletteMain)


                ' Now after the color palette has been replaced, our preview must be updated
                MdlZTStudioUI.UpdatePreview(True, True)

            End If

        End With

    End Sub


    ''' <summary>
    ''' Handles menu click to save as a ZT1 Color palette
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub MnuPal_SavePAL_Click(sender As Object, e As EventArgs) Handles mnuPal_SavePAL.Click

        With DlgSave

            .Title = "Save as a ZT1 Color Palette"
            .DefaultExt = ".pal"
            .Filter = "ZT1 Color Palette files (*.pal)|*.pal|All files|*.*"
            .InitialDirectory = New System.IO.FileInfo(Cfg_Path_RecentZT1).Directory.FullName

            ' If user didn't cancel, create ZT1 Color palette
            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                EditorGraphic.ColorPalette.WritePal(DlgSave.FileName, True)

            End If


        End With

    End Sub

    ''' <summary>
    ''' Handles menu click to import a GIMP Color Palette
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub MnuPal_ImportGimpPalette_Click(sender As Object, e As EventArgs) Handles mnuPal_ImportGimpPalette.Click

        With DlgOpen

            .Title = "Pick a GIMP Color Palette"
            .DefaultExt = ".gpl"
            .Filter = "GIMP Color Palette (*.gpl)|*.gpl|All files|*.*"

            ' Uses most recent ZT1 Graphic path
            .InitialDirectory = New System.IO.FileInfo(Cfg_Path_RecentZT1).Directory.FullName

            ' If user didn't cancel, import GIMP Palette
            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Replace palette file (should trigger a redraw of coreImageBitmap)
                ' Forcefully add colors (some might be the same, after a recolor)
                EditorGraphic.ColorPalette.ImportFromGIMPPalette(DlgOpen.FileName)

                ' Update color list on the right
                EditorGraphic.ColorPalette.FillPaletteGrid(DgvPaletteMain)

            End If

        End With

    End Sub


    ''' <summary>
    ''' Handles toolbar button click to batch rotation fix a set of graphics
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbBatchRotFix_Click(sender As Object, e As EventArgs) Handles TsbBatchRotFix.Click
        FrmBatchOffsetFix.ShowDialog(Me)

    End Sub


    ''' <summary>
    ''' Handles toolbar button click to create a new ZT1 Graphic from scratch
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>

    Private Sub TsbZT1New_Click(sender As Object, e As EventArgs) Handles TsbZT1New.Click

        ' New ZT1 Graphic
        EditorGraphic = New ClsGraphic(Nothing)

        ' Always start with one frame
        EditorFrame = New ClsFrame(EditorGraphic)
        EditorGraphic.Frames.Add(EditorFrame)

        ' Update/reset color palette
        EditorGraphic.ColorPalette.FillPaletteGrid(DgvPaletteMain)

        ' This is the only (or one of the few) cases where frame reset happens:
        TbFrames.Value = 1

        ' Update preview
        MdlZTStudioUI.UpdatePreview(True, True, 0)

    End Sub


    ''' <summary>
    ''' Handles toolbar button click to save as a ZT1 Graphic
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub TsbZT1Write_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbZT1Write.MouseDown

        If EditorGraphic.Frames.Count = 0 Then
            MdlZTStudio.HandledError(Me.GetType().FullName, "TsbZT1Write_MouseDown", "You can't create a ZT1 Graphic without adding a frame first.")
            Exit Sub
        End If

        If (e.Button = Windows.Forms.MouseButtons.Right) Then

            ' Shortcut to saving directly
            If File.Exists(EditorGraphic.FileName) = True Then

                ' Save graphic (existing graphic, overwrite)
                MdlTasks.Save_Graphic(EditorGraphic.FileName)
                MdlConfig.Write()

                'No need to continue
                Exit Sub

            End If

        End If

        ' Shortcut above failed, go over entire saving process

        ' Where shall we save this ZT1 Graphic?
        With DlgSave
            .Title = "Save ZT1 Graphic"
            .DefaultExt = ""
            .AddExtension = True
            .FileName = Cfg_Path_RecentZT1
            .Filter = "ZT1 Graphics|*"

            .InitialDirectory = New FileInfo(Cfg_Path_RecentZT1).Directory.FullName

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                If Path.GetExtension(DlgSave.FileName).ToLower() <> "" Then
                    MdlZTStudio.HandledError(Me.GetType().FullName, "TsbZT1Write_MouseDown", "A ZT1 Graphic file does not have a file extension.")
                    Exit Sub

                End If

51:
                MdlTasks.Save_Graphic(DlgSave.FileName)

60:
                ' Remember
                Cfg_Path_RecentZT1 = DlgSave.FileName
                MdlConfig.Write()

                ' What has been opened, might need to be saved.
                DlgOpen.FileName = DlgSave.FileName

65:
                ' Might be a new file, so update root folder and select this.
                MdlZTStudioUI.UpdateExplorerPane()

            End If

        End With

    End Sub



    ''' <summary>
    ''' Handles check change to start/stop playing animation
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub ChkPlayAnimation_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPlayAnimation.CheckedChanged

        ' Set timer to the specified graphic's animationspeed
        ' Enable/disable timer
        TmrAnimation.Interval = EditorGraphic.AnimationSpeed
        TmrAnimation.Enabled = ChkPlayAnimation.Checked

    End Sub



    ''' <summary>
    ''' Handles selection of a graphic in the Explorer Pane
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">TreeViewEventArgs</param>
    Private Sub TVExplorer_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TVExplorer.AfterSelect

        MdlZTStudio.Trace(Me.GetType().FullName, "TVExplorer_AfterSelect", "Selected node " & e.Node.Text & " -> " & e.Node.Name)

        ' If the selected item is a ZT1 Graphic file, load?
        If Regex.IsMatch(e.Node.Text, "[0-9A-z]") = True And e.Node.ImageIndex = 0 Then

            ' Same handling as ZT1 open graphic button (but don't do loop: selection also happens on form load)
            If Strings.LCase(EditorGraphic.FileName) <> Strings.LCase(Cfg_Path_Root & "\" & e.Node.Name) Then
                MdlZTStudioUI.LoadGraphic(Cfg_Path_Root & "\" & e.Node.Name)
            End If

        End If

    End Sub



    ''' <summary>
    ''' Handles Leave event of toolstrip textbox animation speed. Resets animation speed textbox.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TstZT1_AnimSpeed_Leave(sender As Object, e As EventArgs) Handles TstZT1_AnimSpeed.Leave

        ' If nothing has been confirmed ([Enter]), reset original value
        TstZT1_AnimSpeed.Text = EditorGraphic.AnimationSpeed


    End Sub

    ''' <summary>
    ''' Handles confirmation of new offset (on Enter)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">KeyEventArgs</param>
    Private Sub TstOffsetX_KeyDown(sender As Object, e As KeyEventArgs) Handles TstOffsetX.KeyDown

        ' On enter
        If e.KeyCode <> 13 Then
            Exit Sub
        End If

        If TstOffsetX.Text = "" Then
            ' Suspend checking, user is most likely still busy changing this value
            Exit Sub
        ElseIf IsNumeric(TstOffsetX.Text) = False Then
            MdlZTStudio.HandledError(Me.GetType().FullName, "TstOffsetX_TextChanged", "Offset should be a numerical value between -32767 and 32767")
            Exit Sub
        ElseIf CInt(TstOffsetX.Text) < -32767 Or CInt(TstOffsetX.Text) > 32767 Then
            MdlZTStudio.HandledError(Me.GetType().FullName, "TstOffsetX_TextChanged", "Offset should be a numerical value between -32767 and 32767")
            Exit Sub
        End If


        ' UpdateOffsets() takes changes, not final coordinates
        ' Get the difference
        Dim IntDifference = CInt(TstOffsetX.Text) - EditorFrame.OffsetX

        EditorFrame.UpdateOffsets(New Point(IntDifference, 0))

        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub


    ''' <summary>
    ''' Handles confirmation of new offset (on Enter)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">KeyEventArgs</param>
    Private Sub TstOffsetY_KeyDown(sender As Object, e As KeyEventArgs) Handles TstOffsetY.KeyDown

        ' On enter
        If e.KeyCode <> 13 Then
            Exit Sub
        End If

        If TstOffsetY.Text = "" Then
            ' Suspend checking, user is most likely still busy changing this value
            Exit Sub
        ElseIf IsNumeric(TstOffsetY.Text) = False Then
            MdlZTStudio.HandledError(Me.GetType().FullName, "TstOffsetY_TextChanged", "Offset should be a numerical value between -32767 and 32767")
            Exit Sub
        ElseIf CInt(TstOffsetY.Text) < -32767 Or CInt(TstOffsetY.Text) > 32767 Then
            MdlZTStudio.HandledError(Me.GetType().FullName, "TstOffsetY_TextChanged", "Offset should be a numerical value between -32767 and 32767")
            Exit Sub
        End If


        ' UpdateOffsets() takes changes, not final coordinates
        ' Get the difference
        Dim IntDifference = CInt(TstOffsetY.Text) - EditorFrame.OffsetY

        EditorFrame.UpdateOffsets(New Point(0, IntDifference))

        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub


    ''' <summary>
    ''' Handles Leave event of toolstrip textbox offset X. Resets offset value in textbox.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TstOffsetX_Leave(sender As Object, e As EventArgs) Handles TstOffsetX.Leave

        ' If nothing has been confirmed ([Enter]), reset original value
        TstOffsetX.Text = EditorFrame.OffsetX

    End Sub



    ''' <summary>
    ''' Handles Leave event of toolstrip textbox offset Y. Resets offset
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TstOffsetY_Leave(sender As Object, e As EventArgs) Handles TstOffsetY.Leave

        ' If nothing has been confirmed ([Enter]), reset original value
        TstOffsetY.Text = EditorFrame.OffsetY

    End Sub


    ''' <summary>
    ''' Handles confirmation of new animation speed (on Enter)
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">KeyEventArgs</param>
    Private Sub TstZT1_AnimSpeed_KeyDown(sender As Object, e As KeyEventArgs) Handles TstZT1_AnimSpeed.KeyDown

        If e.KeyCode <> 13 Then
            ' Not confirming by pressing [Enter]
        End If


        If TstZT1_AnimSpeed.Text = "" Then
            ' User is just changing value, don't be too strict on empty values.
            Exit Sub

        ElseIf IsNumeric(TstZT1_AnimSpeed.Text) = False Then

            ' Not numeric = invalid
            MdlZTStudio.HandledError(Me.GetType().FullName, "TstZT1_AnimSpeed_TextChanged", "The animation speed should be a number of milliseconds.")
            Exit Sub

        ElseIf CInt(TstZT1_AnimSpeed.Text) < 1 Or (CInt(TstZT1_AnimSpeed.Text) > 1000) Then

            ' Not in a valid range. Theoretically, the interval could be much higher.
            ' In practical ways, it should never be.
            MdlZTStudio.HandledError(Me.GetType().FullName, "TstZT1_AnimSpeed_TextChanged", "Invalid value for animation speed. Expecting a value between 1 and 1000 milliseconds.")
            Exit Sub

        End If


        ' Seems to be okay, numeric and within range.
        EditorGraphic.AnimationSpeed = CInt(TstZT1_AnimSpeed.Text)


    End Sub

    ''' <summary>
    ''' Toggle option to enforce offsets on each frame in the loaded ZT1 Graphic
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TsbFrame_OffsetAll_Click(sender As Object, e As EventArgs) Handles TsbFrame_OffsetAll.Click

        TsbFrame_OffsetAll.Checked = Not TsbFrame_OffsetAll.Checked ' Change toggle
        Cfg_editor_rotFix_individualFrame = CByte(TsbFrame_OffsetAll.Checked * -1)
        MdlConfig.Write()


    End Sub

    ''' <summary>
    ''' Doubleclick on Explorer Pane may open .pal file
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TVExplorer_DoubleClick(sender As Object, e As EventArgs) Handles TVExplorer.DoubleClick

        ' Open .pal file
        If Regex.IsMatch(TVExplorer.SelectedNode.Name, ".*\.pal$", RegexOptions.IgnoreCase) = True Then
            MdlColorPalette.LoadPalette(Cfg_path_Root & "\" & TVExplorer.SelectedNode.Name)
        End If

    End Sub

End Class
