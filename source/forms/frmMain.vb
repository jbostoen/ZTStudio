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
        MdlZTStudio.Trace(Me.GetType().FullName, "MouseMove", "Offset X = " & intoffsetX & ", Y = " & intoffsetY )
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
        MdlZTStudio.UnexpectedError(Me.GetType().FullName, "PicBox_MouseMove", Information.Err)

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

        MdlZTStudioUI.UpdatePreview(True, False, TbFrames.Value - 1)

    End Sub

    ''' <summary>
    ''' If "play animation" has been checked, the timer updates the preview. Timer interval = graphic animation speed.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TmrAnimation_Tick(sender As Object, e As EventArgs) Handles TmrAnimation.Tick


        MdlZTStudio.Trace("TmrAnimation", "Tick", "Interval = " & TmrAnimation.Interval.ToString())


        If (TbFrames.Value = TbFrames.Maximum) Then
            TbFrames.Value = 1
        Else
            TbFrames.Value += 1
        End If

        MdlZTStudioUI.UpdatePreview(True, False, TbFrames.Value - 1)

    End Sub



    Private Sub TsbGridBG_Click(sender As Object, e As EventArgs) Handles TsbGridBG.Click

        With DlgColor
            .Color = Cfg_grid_BackGroundColor
            .ShowDialog()

            Cfg_grid_BackGroundColor = .Color


        End With

        MdlConfig.Write()
        MdlZTStudioUI.UpdatePreview(False, False, Me.GetType().FullName & "::TsbGridBG_Click(): Background color changed.")

    End Sub

    Private Sub TsbZT1Open_Click(sender As Object, e As EventArgs) Handles TsbZT1Open.Click

        MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Open ZT1 file dialog.")
        MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Last used file: " & Cfg_path_recentZT1)

        With DlgOpen
            .Title = "Pick a ZT1 Graphic"
            .DefaultExt = ""
            .Filter = "All files|*.*"
            .InitialDirectory = New FileInfo(Cfg_path_recentZT1).Directory.FullName
            Debug.Print("last used = " & New FileInfo(Cfg_path_recentZT1).Directory.FullName)


            If DlgOpen.InitialDirectory = vbNullString Or System.IO.Directory.Exists(DlgOpen.InitialDirectory) = False Then
                If System.IO.Directory.Exists(Cfg_path_Root) Then
                    MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Open ZT1 file dialog. Fallback to root: " & Cfg_path_Root)
                    .InitialDirectory = Cfg_path_Root
                ElseIf System.IO.Directory.Exists("C:\Program Files (x86)\Microsoft Games\Zoo Tycoon") Then
                    MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Open ZT1 file dialog. Fallback to Program Files (x86)")
                    .InitialDirectory = "C:\Program Files (x86)\Microsoft Games\Zoo Tycoon"
                ElseIf System.IO.Directory.Exists("C:\Program Files\Microsoft Games\Zoo Tycoon") Then
                    MdlZTStudio.Trace(Me.GetType().FullName, "TsbZT1Open_Click", "Open ZT1 file dialog. Fallback to Program Files")
                    .InitialDirectory = "C:\Program Files\Microsoft Games\Zoo Tycoon"
                End If
            End If

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                MdlZTStudioUI.LoadGraphic(DlgOpen.FileName)
                ' What has been opened, might need to be saved.
                DlgSave.FileName = DlgOpen.FileName

            End If ' End Cancel check


        End With
    End Sub

    Private Sub TbFrames_ValueChanged1(sender As Object, e As EventArgs) Handles TbFrames.ValueChanged

        MdlZTStudioUI.UpdatePreview(True, False, TbFrames.Value - 1)

        Debug.Print("Value changed.")

        EditorFrame = EditorGraphic.Frames(TbFrames.Value - 1)

    End Sub


    Private Sub TsbAbout_Click(sender As Object, e As EventArgs) Handles TsbAbout.Click

        MsgBox("About " & Application.ProductName & " " & Application.ProductVersion & vbCrLf &
            Strings.StrDup(50, "_") & vbCrLf &
            "© since 2015 by Jeffrey Bostoen" & vbCrLf &
            "https://github.com/jbostoen/ZTStudio" & vbCrLf &
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

    Private Sub TsbFrame_ExportPNG_Click(sender As Object, e As EventArgs) Handles TsbFrame_ExportPNG.Click


        With DlgSave
            .Title = "Save single frame as .PNG"
            .DefaultExt = ".png"
            .AddExtension = True
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentPNG)
            .Filter = "PNG files (*.png)|*.png|All files|*.*"


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                ' was bminput


                ' bminput.cachedFrame.Save(dlgSave.FileName, System.Drawing.Imaging.ImageFormat.Png)

                EditorFrame.SavePNG(DlgSave.FileName)


                ' Remember
                Cfg_path_recentPNG = New System.IO.FileInfo(DlgSave.FileName).Directory.FullName
                MdlConfig.Write()


            End If

        End With



    End Sub


    Private Sub TsbZT1_OpenPal_Click(sender As Object, e As EventArgs) Handles TsbZT1_OpenPal.Click


        With DlgOpen

            .Title = "Pick a ZT1 Color Palette"
            .DefaultExt = ".pal"
            .Filter = "ZT1 Color Palette files (*.pal)|*.pal|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                Pal_Open(DlgOpen.FileName)

            End If ' cancel check


        End With

    End Sub





    Private Sub TsbSettings_Click(sender As Object, e As EventArgs) Handles TsbSettings.Click

        FrmSettings.ShowDialog()

    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles TsFrame.ItemClicked

    End Sub

    Private Sub TsbOpenPalBldg8_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles TsbOpenPalBldg8.DropDownItemClicked

        Pal_Open(Cfg_path_ColorPals8 & "\" & e.ClickedItem.Text)

    End Sub
    Private Sub TsbOpenPalBldg16_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles TsbOpenPalBldg16.DropDownItemClicked

        Pal_Open(Cfg_path_ColorPals16 & "\" & e.ClickedItem.Text)

    End Sub

    Private Sub TsbBatchConversion_Click(sender As Object, e As EventArgs) Handles TsbBatchConversion.Click

        FrmBatchConversion.ShowDialog(Me)

    End Sub

    Private Sub TsbPreview_BGGraphic_Click(sender As Object, e As EventArgs) Handles TsbPreview_BGGraphic.Click


        With DlgOpen
            .Title = "Pick a ZT1 Graphic"
            .DefaultExt = ""
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)
            .Filter = "All files|*.*"

            If DlgOpen.InitialDirectory = vbNullString Then
                If System.IO.Directory.Exists("C:\Program Files\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files\Microsoft Games\Zoo Tycoon"
                ElseIf System.IO.Directory.Exists("C:\Program Files (x86)\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files (x86)\Microsoft Games\Zoo Tycoon"

                End If
            End If

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                If System.IO.File.Exists(DlgOpen.FileName) = True Then

                    If Path.GetExtension(DlgOpen.FileName) <> vbNullString Then
                        MsgBox("You selected a file with the extension '" & Path.GetExtension(DlgOpen.FileName) & "'." & vbCrLf &
                               "With ZT1 graphic, we mean a ZT1 graphics file without extension.",
                               vbOKOnly + vbCritical, "Invalid file")
                    Else

                        ' OK
                        EditorBgGraphic.Read(DlgOpen.FileName)

                        ' reDraw current frame 
                        MdlZTStudioUI.UpdatePreview(True, False)

                        ' Show default palette
                        'editorBgGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)

                        ' Remember
                        Cfg_path_recentZT1 = DlgOpen.FileName
                        MdlConfig.Write()

                    End If
                Else
                    MsgBox("File does not exist.", vbOKOnly + vbCritical, "Invalid file")
                End If

            End If ' End Cancel check


        End With
    End Sub



    Private Sub TstZT1_AnimSpeed_TextChanged(sender As Object, e As EventArgs) Handles TstZT1_AnimSpeed.TextChanged

        If TstZT1_AnimSpeed.Text = "" Then
            ' User is just changing value, don't be too strict on empty values.
            EditorGraphic.AnimationSpeed = 1000

        ElseIf IsNumeric(TstZT1_AnimSpeed.Text) = False Then
            MdlZTStudio.ExpectedError(Me.GetType().FullName, "TstZT1_AnimSpeed_TextChanged", "The animation speed should be a number of milliseconds.")
            TstZT1_AnimSpeed.Text = "1000"
            EditorGraphic.AnimationSpeed = 1000
            Exit Sub

        Else

            ' Valid value?
            If CInt(TstZT1_AnimSpeed.Text) < 1 Or (CInt(TstZT1_AnimSpeed.Text) > 1000) Then

                MdlZTStudio.ExpectedError(Me.GetType().FullName, "TstZT1_AnimSpeed_TextChanged", "Invalid value for animation speed. Expecting a value between 1 and 1000 milliseconds.")
                TstZT1_AnimSpeed.Text = "1000"
                EditorGraphic.AnimationSpeed = 1000
                Exit Sub

            End If

            ' Seems to be okay, numeric and within range.
            EditorGraphic.AnimationSpeed = CInt(TstZT1_AnimSpeed.Text)

        End If


    End Sub


    Private Sub TsbFrame_Add_Click(sender As Object, e As EventArgs) Handles TsbFrame_Add.Click

        On Error GoTo dBug

0:
        Dim ZtFrame As New ClsFrame(EditorGraphic)
2:

10:
        EditorGraphic.Frames.Insert(TbFrames.Value, ZtFrame) ' add after

15:
        ' not sure if this is right if an extra frame is applied?
        TbFrames.Maximum = EditorGraphic.Frames.Count - EditorGraphic.HasBackgroundFrame

16:
        TbFrames.Value += 1

        MdlZTStudioUI.UpdatePreview(True, True, TbFrames.Value - 1)

        Exit Sub

dBug:
        MsgBox("Error at line " & Erl() & " in tsbFrame_Add: " & Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error")

    End Sub

    Private Sub TsbFrame_Delete_Click(sender As Object, e As EventArgs) Handles TsbFrame_Delete.Click

        EditorGraphic.Frames.RemoveAt(TbFrames.Value - 1)

        MdlZTStudioUI.UpdatePreview(True, True, TbFrames.Value - 1)


    End Sub

    Private Sub PicBox_Click(sender As Object, e As EventArgs) Handles PicBox.Click

    End Sub

    Private Sub TsbFrame_IndexIncrease_Click(sender As Object, e As EventArgs) Handles TsbFrame_IndexIncrease.Click

        ' Change handled in slider control
        TbFrames.Value += 1


    End Sub


    Private Sub TsbFrame_OffsetUp_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_OffsetUp.MouseDown

        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            EditorFrame.UpdateOffsets(New Point(0, 16))
        Else
            EditorFrame.UpdateOffsets(New Point(0, 1))
        End If

        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub

    Private Sub TsbFrame_OffsetDown_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_OffsetDown.MouseDown


        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            EditorFrame.UpdateOffsets(New Point(0, -16))
        Else
            EditorFrame.UpdateOffsets(New Point(0, -1))
        End If

        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub

    Private Sub TsbFrame_OffsetLeft_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_OffsetLeft.MouseDown



        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            EditorFrame.UpdateOffsets(New Point(16, 0))
        Else
            EditorFrame.UpdateOffsets(New Point(1, 0))
        End If

        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub

    Private Sub TsbFrame_OffsetRight_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_OffsetRight.MouseDown


        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            EditorFrame.UpdateOffsets(New Point(-16, 0))
        Else
            EditorFrame.UpdateOffsets(New Point(-1, 0))
        End If

        MdlZTStudioUI.UpdatePreview(True, False)



    End Sub

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

    Private Sub TsbFrame_IndexDecrease_Click(sender As Object, e As EventArgs) Handles TsbFrame_IndexDecrease.Click

        ' Change handled in slider control
        TbFrames.Value -= 1

    End Sub

    Private Sub Tstools_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles TsTools.ItemClicked

    End Sub

    Private Sub TsbDelete_PNG_Click(sender As Object, e As EventArgs) Handles TsbDelete_PNG.Click

        MdlTasks.CleanUp_files(Cfg_path_Root, ".png")
        MsgBox("Finished clean up.", vbOKOnly + vbInformation, "Finished clean up.")

    End Sub

    Private Sub TsbDelete_ZT1Files_Click(sender As Object, e As EventArgs) Handles TsbDelete_ZT1Files.Click

        ' Cleanup ZT1 Graphics and color palettes
        MdlTasks.CleanUp_files(Cfg_path_Root, "")
        MdlTasks.CleanUp_files(Cfg_path_Root, ".pal")
        MsgBox("Finished clean up.", vbOKOnly + vbInformation, "Finished clean up.")

    End Sub



    Private Sub TsbFrame_fpX_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TsbFrame_fpX.SelectedIndexChanged

        Cfg_grid_footPrintX = TsbFrame_fpX.Text
        MdlConfig.Write()
        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub

    Private Sub TsbFrame_fpY_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TsbFrame_fpY.SelectedIndexChanged


        Cfg_grid_footPrintY = TsbFrame_fpY.Text
        MdlConfig.Write()
        MdlZTStudioUI.UpdatePreview(True, False)

    End Sub


    Private Sub DgvPaletteMain_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvPaletteMain.CellDoubleClick

        ' Actual row rather than header. Avoid crash.
        If e.RowIndex > -1 Then

            ' Replace colors
            MdlTasks.Pal_ReplaceColor(e.RowIndex)

        End If


    End Sub

    Private Sub DgvPaletteMain_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DgvPaletteMain.CellMouseClick

        If e.RowIndex <> -1 Then
            DgvPaletteMain.Rows(e.RowIndex).Selected = True

        End If


        If e.Button = Windows.Forms.MouseButtons.Right Then

            ' visible?
            mnuPal_MoveDown.Visible = (e.RowIndex <> DgvPaletteMain.Rows.Count - 1)
            mnuPal_MoveEnd.Visible = (e.RowIndex <> DgvPaletteMain.Rows.Count - 1)
            mnuPal_MoveUp.Visible = (e.RowIndex <> 0)

            'mnuPal.Show(Me.dgvPaletteMain, e.Location)
            MnuPal.Show(Cursor.Position)

        End If

    End Sub

    Private Sub MnuPal_MoveUp_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveUp.Click

        MdlTasks.Pal_MoveColor(DgvPaletteMain.SelectedRows(0).Index, DgvPaletteMain.SelectedRows(0).Index - 1)

    End Sub
    Private Sub MnuPal_MoveDown_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveDown.Click

        MdlTasks.Pal_MoveColor(DgvPaletteMain.SelectedRows(0).Index, DgvPaletteMain.SelectedRows(0).Index + 1)

    End Sub


    Private Sub MnuPal_Replace_Click(sender As Object, e As EventArgs) Handles mnuPal_Replace.Click

        MdlTasks.Pal_ReplaceColor(DgvPaletteMain.SelectedRows(0).Index)


    End Sub

    Private Sub MnuPal_MoveEnd_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveEnd.Click

        MdlTasks.Pal_MoveColor(DgvPaletteMain.SelectedRows(0).Index, EditorGraphic.ColorPalette.Colors.Count - 1)


    End Sub

    Private Sub MnuPal_Add_Click(sender As Object, e As EventArgs) Handles mnuPal_Add.Click

        ' Add after this entry
        Pal_AddColor(DgvPaletteMain.SelectedRows(0).Index)

    End Sub


    Private Sub TsbFrame_ImportPNG_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbFrame_ImportPNG.MouseDown

        ' Shortcut to create a new frame first, then import the PNG to it.

        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            ' Add frame


0:
            Dim ZtFrame As New ClsFrame(EditorGraphic)
2:

10:
            ' Add the frame after the existing one(s)
            EditorGraphic.Frames.Insert(TbFrames.Value, ZtFrame)

15:
            ' not sure if this is right if an extra frame is applied?
            TbFrames.Maximum = EditorGraphic.Frames.Count - EditorGraphic.HasBackgroundFrame

16:
            TbFrames.Value += 1

            MdlZTStudioUI.UpdatePreview(True, True, TbFrames.Value - 1)

        End If



100:

        With DlgOpen
            .Title = "Pick a .PNG file"
            .DefaultExt = ""
            .Filter = "PNG files|*.png"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentPNG)

            ' If most recent directory does not exist anymore:
            If DlgOpen.InitialDirectory = vbNullString Or System.IO.Directory.Exists(DlgOpen.InitialDirectory) = False Then
                .InitialDirectory = Cfg_path_Root

            End If

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                If System.IO.File.Exists(DlgOpen.FileName) = True Then

                    If Path.GetExtension(DlgOpen.FileName).ToLower() <> ".png" Then
                        MsgBox("You selected a file with the extension '" & Path.GetExtension(DlgOpen.FileName) & "'." & vbCrLf &
                               "You need a file with a .PNG extension.",
                               vbOKOnly + vbCritical, "Invalid file")
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
                        Cfg_path_recentPNG = New System.IO.FileInfo(DlgOpen.FileName).Directory.FullName
                        MdlConfig.Write()


                    End If
                Else
                    MsgBox("File does not exist.", vbOKOnly + vbCritical, "Invalid file")
                End If

            End If ' End Cancel check


        End With


    End Sub



    Private Sub MnuPal_ExportPNG_Click(sender As Object, e As EventArgs) Handles mnuPal_ExportPNG.Click

        With DlgSave

            .Title = "Save as a PNG Color Palette"
            .DefaultExt = ".png"
            .Filter = "PNG Color Palette files (*.png)|*.png|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                EditorGraphic.ColorPalette.ExportToPNG(DlgSave.FileName)

            End If ' cancel check

        End With

    End Sub

    Private Sub MnuPal_ImportPNG_Click(sender As Object, e As EventArgs) Handles mnuPal_ImportPNG.Click



        With DlgOpen

            .Title = "Pick a PNG Color Palette"
            .DefaultExt = ".png"
            .Filter = "PNG Color Palette files (*.png)|*.png|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Replace palette file (should trigger a re-draw AFTERWARDS )
                ' Forcefully add colors (some might be the same, after a recolor)
                EditorGraphic.ColorPalette.ImportFromPNG(DlgOpen.FileName)

                ' Update color list on the right
                EditorGraphic.ColorPalette.FillPaletteGrid(DgvPaletteMain)


                ' Now after the color palette has been replaced, our preview must be updated
                MdlZTStudioUI.UpdatePreview(True, True)

            End If ' cancel check

        End With

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub MnuPal_SavePAL_Click(sender As Object, e As EventArgs) Handles mnuPal_SavePAL.Click

        With DlgSave

            .Title = "Save as a ZT1 Color Palette"
            .DefaultExt = ".pal"
            .Filter = "ZT1 Color Palette files (*.pal)|*.pal|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                EditorGraphic.ColorPalette.WritePal(DlgSave.FileName, True)

            End If ' cancel check


        End With

    End Sub

    ''' <summary>
    ''' Button triggers an action to import a GIMP Color Palette
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MnuPal_ImportGimpPalette_Click(sender As Object, e As EventArgs) Handles mnuPal_ImportGimpPalette.Click

        With DlgOpen

            .Title = "Pick a GIMP Color Palette"
            .DefaultExt = ".gpl"
            .Filter = "GIMP Color Palette (*.gpl)|*.gpl|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Replace palette file (should trigger a redraw of coreImageBitmap)
                ' Forcefully add colors (some might be the same, after a recolor)
                EditorGraphic.ColorPalette.ImportFromGIMPPalette(DlgOpen.FileName)

                ' Update color list on the right
                EditorGraphic.ColorPalette.FillPaletteGrid(DgvPaletteMain)

            End If ' cancel check

        End With

    End Sub


    Private Sub TsbBatchRotFix_Click(sender As Object, e As EventArgs) Handles TsbBatchRotFix.Click
        FrmBatchOffsetFix.ShowDialog(Me)

    End Sub



    Private Sub TsbZT1New_Click(sender As Object, e As EventArgs) Handles TsbZT1New.Click

        ' New ZT1 Graphic
        EditorGraphic = New ClsGraphic

        ' Always start with one frame
        EditorFrame = New ClsFrame(EditorGraphic)
        EditorGraphic.Frames.Add(EditorFrame)

        ' Update/reset color palette
        EditorGraphic.ColorPalette.FillPaletteGrid(DgvPaletteMain)

        ' Update preview
        MdlZTStudioUI.UpdatePreview(True, True, 0)

    End Sub


    Private Sub TsbZT1Write_MouseDown(sender As Object, e As MouseEventArgs) Handles TsbZT1Write.MouseDown

        If EditorGraphic.Frames.Count = 0 Then
            MdlZTStudio.ExpectedError(Me.GetType().FullName, "TsbZT1Write_MouseDown", "You can't create a ZT1 Graphic without adding a frame first.")
            Exit Sub
        End If

        If (e.Button = Windows.Forms.MouseButtons.Right) Then

            ' Shortcut to saving directly
            If File.Exists(EditorGraphic.FileName) = True Then

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
            .InitialDirectory = New FileInfo(Cfg_path_recentZT1).Directory.FullName
            .FileName = Cfg_path_recentZT1
            .Filter = "ZT1 Graphics|*"

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                If Path.GetExtension(DlgSave.FileName).ToLower() <> "" Then
                    MsgBox("A ZT1 Graphic file does not have a file extension.", vbOKOnly + vbCritical, "Invalid filename")
                    Exit Sub

                End If

51:
                MdlTasks.Save_Graphic(DlgSave.FileName)

60:
                ' Remember
                Cfg_path_recentZT1 = DlgSave.FileName
                MdlConfig.Write()

                ' What has been opened, might need to be saved.
                DlgOpen.FileName = DlgSave.FileName

65:
                ' Might be a new file, so update root folder and select this.
                MdlZTStudioUI.UpdateExplorerPane()

            End If

        End With

    End Sub



    Private Sub ChkPlayAnimation_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPlayAnimation.CheckedChanged

        TmrAnimation.Interval = EditorGraphic.AnimationSpeed
        TmrAnimation.Enabled = ChkPlayAnimation.Checked


    End Sub

    Private Sub LblColorDetails_Click(sender As Object, e As EventArgs) Handles LblColorDetails.Click

    End Sub

    Private Sub TbFrames_Scroll(sender As Object, e As EventArgs) Handles TbFrames.Scroll

    End Sub

    Private Sub TVExplorer_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TVExplorer.AfterSelect

        MdlZTStudio.Trace(Me.GetType().FullName, "TVExplorer_AfterSelect", "Selected node " & e.Node.Text & " -> " & e.Node.Name)


        Application.DoEvents()


        ' If the selected item is a ZT1 Graphic file, load?
        If Regex.IsMatch(e.Node.Text, "[0-9A-z]") = True And e.Node.ImageIndex = 0 Then

            ' Same handling as ZT1 open graphic button
            MdlZTStudioUI.LoadGraphic(Cfg_path_Root & "\" & e.Node.Name)

        End If

    End Sub

    ''' <summary>
    ''' Makes sure there is a value present for the animation speed
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub TstZT1_AnimSpeed_LostFocus(sender As Object, e As EventArgs) Handles TstZT1_AnimSpeed.LostFocus
        If IsNumeric(TstZT1_AnimSpeed.Text) = False Then
            TstZT1_AnimSpeed.Text = "1000"
            EditorGraphic.AnimationSpeed = 1000
        End If
    End Sub
End Class
