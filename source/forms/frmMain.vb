Option Explicit On


Imports System.IO



Public Class FrmMain




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        Debug.Flush()
        'mdlSettings.DoubleBuffered(dgvPaletteMain, True)
        dgvPaletteMain.GetType.InvokeMember("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.SetProperty, Nothing, dgvPaletteMain, New Object() {True})


        ' Always start with one frame
        editorFrame = New ClsFrame(editorGraphic)
        editorGraphic.Frames.Add(editorFrame)


        ' Output
        BM = New Bitmap(Cfg_grid_numPixels * 2, Cfg_grid_numPixels * 2)
        With picBox
            .Width = Cfg_grid_numPixels * 2
            .Height = Cfg_grid_numPixels * 2
        End With

        ' Background color
        picBox.BackColor = Cfg_grid_BackGroundColor


        ' Grid
        tsbFrame_fpX.Text = CStr(Cfg_grid_footPrintX)
        tsbFrame_fpY.Text = CStr(Cfg_grid_footPrintY)

        ' ZT1 Default color palettes
        ' strPathBuildingColorPals

        Dim di As New IO.DirectoryInfo(Cfg_path_ColorPals8)
        Dim diar1 As IO.FileInfo() = di.GetFiles()
        Dim dra As IO.FileInfo

        'list the names of all files in the specified directory
        For Each dra In diar1
            tsbOpenPalBldg8.DropDownItems.Add(dra.Name)
        Next

        di = New IO.DirectoryInfo(Cfg_path_ColorPals16)
        diar1 = di.GetFiles()

        'list the names of all files in the specified directory
        For Each dra In diar1
            tsbOpenPalBldg16.DropDownItems.Add(dra.Name)
        Next


    End Sub


    Private Sub PicBox_MouseEnter(sender As Object, e As EventArgs) Handles picBox.MouseEnter

    End Sub

    Private Sub PicBox_MouseMove(sender As Object, e As MouseEventArgs) Handles picBox.MouseMove

        On Error GoTo dBug

        ' If we have something in our canvas / if we have a cached frame rendered, 
        ' we will show color info on mouseover.
1:
        If IsNothing(picBox.Image) Then Exit Sub

2:
        ' frame might have been just initiated
        If IsNothing(editorFrame.CoreImageBitmap) And IsNothing(editorFrame.CoreImageHex) Then Exit Sub



3:
        Dim bmTmp As Bitmap
        bmTmp = picBox.Image ' editorFrame.getImage()
        Application.DoEvents()

4:
        If IsNothing(bmTmp) Then Exit Sub


20:
        Dim eX As Integer = (picBox.Width - bmTmp.Width) / 2
        Dim eY As Integer = (picBox.Height - bmTmp.Height) / 2

100:
        If e.X - eX >= 0 And e.X - eX < bmTmp.Width And e.Y - eY >= 0 And e.Y - eY < bmTmp.Height Then

101:
            Dim c As System.Drawing.Color = bmTmp.GetPixel(e.X - eX, e.Y - eY)


            If c.A <> 0 Then

102:
                lblColor.BackColor = c '.ToString()
                lblColorDetails.Text = "Coordinates: x: " & e.X - eX & " , y: " & e.Y - eY & vbCrLf &
                    "RGB: " & c.R & "," & c.G & "," & c.B & vbCrLf &
                    "Index in .pal file: # " & editorGraphic.ColorPalette.Colors.IndexOf(c)



            Else

112:
                ' Alpha, transparent.
                lblColor.BackColor = picBox.BackColor
                lblColorDetails.Text = vbNullString

            End If

        Else

122:
            lblColor.BackColor = picBox.BackColor
            lblColorDetails.Text = vbNullString

        End If

        Exit Sub

dBug:

        MsgBox("Error in frmMain.picBox.MouseMove() " & vbCrLf &
            "Line " & Erl() & vbCrLf &
            Err.Number & " " & Err.Description, vbOKOnly + vbCritical + vbApplicationModal, "Error determining pixel color")


    End Sub

    Private Sub PicBox_MouseWheel(sender As Object, e As MouseEventArgs) Handles picBox.MouseWheel

        Debug.Print("Picbox Wheel")
        Exit Sub

        ' it should be the image, not the picbox!
        If e.Delta <> 0 Then
            If e.Delta <= 0 Then
                If picBox.Width < 500 Then Exit Sub 'minimum 500?
            Else
                If picBox.Width > 2000 Then Exit Sub 'maximum 2000?
            End If

            picBox.Width += CInt(picBox.Width * e.Delta / 1000)
            picBox.Height += CInt(picBox.Height * e.Delta / 1000)
        End If

    End Sub


    Private Sub TTbFrames_ValueChanged(sender As Object, e As EventArgs) Handles TbFrames.ValueChanged

        clsTasks.Update_preview(TbFrames.Value - 1)

    End Sub

    Private Sub TmrAnimation_Tick(sender As Object, e As EventArgs) Handles tmrAnimation.Tick


        Debug.Print(tmrAnimation.Interval)


        If (TbFrames.Value = TbFrames.Maximum) Then
            TbFrames.Value = 1
        Else
            TbFrames.Value += 1
        End If

        clsTasks.Update_preview(TbFrames.Value - 1)

    End Sub



    Private Sub TsbGridBG_Click(sender As Object, e As EventArgs) Handles tsbGridBG.Click

        With dlgColor
            .Color = Cfg_grid_BackGroundColor
            .ShowDialog()

            Cfg_grid_BackGroundColor = .Color


        End With


        clsTasks.Config_write()
        clsTasks.Update_Info("Background color changed.")


    End Sub

    Private Sub TsbZT1Open_Click(sender As Object, e As EventArgs) Handles tsbZT1Open.Click

        With dlgOpen
            .Title = "Pick a ZT1 Graphic"
            .DefaultExt = ""
            .Filter = "All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)

            If dlgOpen.InitialDirectory = vbNullString Or System.IO.Directory.Exists(dlgOpen.InitialDirectory) = False Then
                If System.IO.Directory.Exists(Cfg_path_Root) Then
                    .InitialDirectory = Cfg_path_Root
                ElseIf System.IO.Directory.Exists("C:\Program Files\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files\Microsoft Games\Zoo Tycoon"
                ElseIf System.IO.Directory.Exists("C:\Program Files (x86)\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files (x86)\Microsoft Games\Zoo Tycoon"
                End If
            End If

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                If System.IO.File.Exists(dlgOpen.FileName) = True Then

                    If Path.GetExtension(dlgOpen.FileName) <> vbNullString Then
                        MsgBox("You selected a file with the extension '" & Path.GetExtension(dlgOpen.FileName) & "'." & vbCrLf &
                               "ZT Studio expects you to select a ZT1 Graphic file, which shouldn't have a file extension.",
                               MsgBoxStyle.OkOnly + MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, "Invalid file")

                        Exit Sub


                    ElseIf dlgOpen.FileName.ToLower().Contains(Cfg_path_Root.ToLower()) = False Then

                        If MsgBox("Only select a file in the root directory, which is currently:" & vbCrLf &
                               Cfg_path_Root & vbCrLf & vbCrLf &
                               "Would you like to change the root directory?",
                               MsgBoxStyle.YesNo + MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, "ZT1 Graphic not within root folder") = MsgBoxResult.Yes Then

                            ' Allow user to quickly change settings -> root directory
                            FrmSettings.Show()

                        End If

                        Exit Sub


                    Else

                        ' Reset any previous info.
                        editorGraphic = New ClsGraphic

                        ' OK
                        editorGraphic.Read(dlgOpen.FileName)

                        ' Keep filename
                        ssFileName.Text = Now.ToString("yyyy-MM-dd HH:mm:ss") & ": opened " & dlgOpen.FileName

                        ' Draw first frame 
                        clsTasks.Update_preview(0)

                        ' Add time indication
                        LblAnimTime.Text = ((editorGraphic.Frames.Count - editorGraphic.ExtraFrame) * editorGraphic.AnimationSpeed) & " ms "
                        LblFrames.Text = (editorGraphic.Frames.Count - editorGraphic.ExtraFrame) & " frames. "

                        ' Show default palette
                        editorGraphic.ColorPalette.FillPaletteGrid(dgvPaletteMain)

                        ' Set editorframe
                        editorFrame = editorGraphic.Frames(0)
                        TbFrames.Value = 1



                    End If
                Else
                    MsgBox("File does not exist.", vbOKOnly + vbCritical, "Invalid file")
                End If

                ' Remember
                Cfg_path_recentZT1 = System.IO.Path.GetFullPath(dlgOpen.FileName)
                clsTasks.Config_write()

                ' What has been opened, might need to be saved.
                dlgSave.FileName = dlgOpen.FileName

            End If ' End Cancel check


        End With
    End Sub

    Private Sub TbFrames_ValueChanged1(sender As Object, e As EventArgs) Handles TbFrames.ValueChanged

        clsTasks.Update_preview(TbFrames.Value - 1)

        Debug.Print("Value changed.")

        editorFrame = editorGraphic.Frames(TbFrames.Value - 1)

    End Sub

    Private Sub ChkPlayAnimation_CheckedChanged_1(sender As Object, e As EventArgs) Handles ChkPlayAnimation.CheckedChanged

        If ChkPlayAnimation.Checked = True Then

            tmrAnimation.Interval = editorGraphic.AnimationSpeed
            tmrAnimation.Enabled = True

        Else

            tmrAnimation.Enabled = False
        End If

    End Sub

    Private Sub TsbAbout_Click(sender As Object, e As EventArgs) Handles tsbAbout.Click

        MsgBox("About " & Application.ProductName & " " & Application.ProductVersion & vbCrLf &
            "___________________________" & vbCrLf &
            "© since 2015 by Jeffrey Bostoen" & vbCrLf &
            "https://github.com/jbostoen/ZTStudio" & vbCrLf &
            vbCrLf & vbCrLf &
            "Bugs? " & vbCrLf &
            "-------------------" & vbCrLf &
            "- You can report them at GitHub or Zoo Tek Phoenix. " & vbCrLf &
            "- Support not guaranteed. " & vbCrLf &
            "- Include the graphic files which are causing the problem. " & vbCrLf & vbCrLf &
            "Credits? " & vbCrLf &
            "-------------------" & vbCrLf &
            "- Blue Fang for creating Zoo Tycoon 1 (and maybe the graphic format)." & vbCrLf &
            "- Microsoft for publishing the game." & vbCrLf &
            "- Rapan Studios for the animal designs." & vbCrLf &
            "- MadScientist and Jay for explaining the file format." & vbCrLf &
            "- Vondell for providing new PNG graphics to experiment with." & vbCrLf &
            "- HENDRIX for some contributions to the source code.",
            vbOKOnly + vbInformation, "About ZT Studio")


    End Sub

    Private Sub TsbFrame_ExportPNG_Click(sender As Object, e As EventArgs) Handles tsbFrame_ExportPNG.Click


        With dlgSave
            .Title = "Save single frame as .PNG"
            .DefaultExt = ".png"
            .AddExtension = True
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentPNG)
            .Filter = "PNG files (*.png)|*.png|All files|*.*"


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                ' was bminput


                ' bminput.cachedFrame.Save(dlgSave.FileName, System.Drawing.Imaging.ImageFormat.Png)

                editorFrame.SavePNG(dlgSave.FileName)


                ' Remember
                Cfg_path_recentPNG = System.IO.Path.GetFullPath(dlgSave.FileName)
                clsTasks.Config_write()


            End If

        End With



    End Sub


    Private Sub TsbZT1_OpenPal_Click(sender As Object, e As EventArgs) Handles tsbZT1_OpenPal.Click


        With dlgOpen

            .Title = "Pick a ZT1 Color Palette"
            .DefaultExt = ".pal"
            .Filter = "ZT1 Color Palette files (*.pal)|*.pal|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                Pal_Open(dlgOpen.FileName)

            End If ' cancel check


        End With

    End Sub





    Private Sub TsbSettings_Click(sender As Object, e As EventArgs) Handles tsbSettings.Click

        FrmSettings.ShowDialog()

    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles TsFrame.ItemClicked

    End Sub

    Private Sub TsbOpenPalBldg8_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsbOpenPalBldg8.DropDownItemClicked

        Pal_Open(Cfg_path_ColorPals8 & "\" & e.ClickedItem.Text)

    End Sub
    Private Sub TsbOpenPalBldg16_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsbOpenPalBldg16.DropDownItemClicked

        Pal_Open(Cfg_path_ColorPals16 & "\" & e.ClickedItem.Text)

    End Sub

    Private Sub TsbBatchConversion_Click(sender As Object, e As EventArgs) Handles tsbBatchConversion.Click

        frmBatchConversion.ShowDialog(Me)

    End Sub

    Private Sub TsbPreview_BGGraphic_Click(sender As Object, e As EventArgs) Handles tsbPreview_BGGraphic.Click


        With dlgOpen
            .Title = "Pick a ZT1 Graphic"
            .DefaultExt = ""
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)
            .Filter = "All files|*.*"

            If dlgOpen.InitialDirectory = vbNullString Then
                If System.IO.Directory.Exists("C:\Program Files\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files\Microsoft Games\Zoo Tycoon"
                ElseIf System.IO.Directory.Exists("C:\Program Files (x86)\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files (x86)\Microsoft Games\Zoo Tycoon"

                End If
            End If

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                If System.IO.File.Exists(dlgOpen.FileName) = True Then

                    If Path.GetExtension(dlgOpen.FileName) <> vbNullString Then
                        MsgBox("You selected a file with the extension '" & Path.GetExtension(dlgOpen.FileName) & "'." & vbCrLf &
                               "With ZT1 graphic, we mean a ZT1 graphics file without extension.",
                               vbOKOnly + vbCritical, "Invalid file")
                    Else

                        ' OK
                        editorBgGraphic.Read(dlgOpen.FileName)

                        ' reDraw current frame 
                        clsTasks.Update_preview()

                        ' Show default palette
                        'editorBgGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)

                        ' Remember
                        Cfg_path_recentZT1 = System.IO.Path.GetFullPath(dlgOpen.FileName)
                        clsTasks.Config_write()

                    End If
                Else
                    MsgBox("File does not exist.", vbOKOnly + vbCritical, "Invalid file")
                End If

            End If ' End Cancel check


        End With
    End Sub




    Private Sub TsbZT1Write_Click(sender As Object, e As EventArgs) Handles tsbZT1Write.Click



    End Sub

    Private Sub TstZT1_AnimSpeed_Click(sender As Object, e As EventArgs) Handles tstZT1_AnimSpeed.Click

    End Sub

    Private Sub TstZT1_AnimSpeed_TextChanged(sender As Object, e As EventArgs) Handles tstZT1_AnimSpeed.TextChanged



        If IsNumeric(tstZT1_AnimSpeed.Text) = False Then
            MsgBox("Invalid value." & vbCrLf & "The animation speed should be a number of milliseconds.",
                   vbOKOnly + vbCritical, "Invalid value for animation speed.")
            tstZT1_AnimSpeed.Text = "1000"
            Exit Sub

        Else

            If CInt(tstZT1_AnimSpeed.Text) < 1 And (CInt(tstZT1_AnimSpeed.Text) > 1000) Then
                MsgBox("Invalid value." & vbCrLf & "ZT Studio currently expects a value between 1 and 1000 milliseconds.",
                       vbOKOnly + vbCritical, "Invalid value for animation speed.")
                tstZT1_AnimSpeed.Text = "1000"
                Exit Sub

            Else

                editorGraphic.AnimationSpeed = CInt(tstZT1_AnimSpeed.Text)


            End If
        End If

    End Sub


    Private Sub TsbFrame_Add_Click(sender As Object, e As EventArgs) Handles tsbFrame_Add.Click

        On Error GoTo dBug




0:
        Dim ztFrame As New ClsFrame(editorGraphic)
2:

10:
        editorGraphic.Frames.Insert(TbFrames.Value, ztFrame) ' add after

15:
        ' not sure if this is right if an extra frame is applied?
        TbFrames.Maximum = editorGraphic.Frames.Count - editorGraphic.ExtraFrame

16:
        TbFrames.Value += 1

        clsTasks.Update_preview(TbFrames.Value - 1)

        Exit Sub

dBug:
        MsgBox("Error at line " & Erl() & " in tsbFrame_Add: " & Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error")

    End Sub

    Private Sub TsbFrame_Delete_Click(sender As Object, e As EventArgs) Handles tsbFrame_Delete.Click

        editorGraphic.Frames.RemoveAt(TbFrames.Value - 1)

        clsTasks.Update_preview(TbFrames.Value - 1)


    End Sub

    Private Sub PicBox_Click(sender As Object, e As EventArgs) Handles picBox.Click

    End Sub

    Private Sub TsbFrame_IndexIncrease_Click(sender As Object, e As EventArgs) Handles tsbFrame_IndexIncrease.Click

        ' Change handled in slider control
        TbFrames.Value += 1


    End Sub


    Private Sub TsbFrame_OffsetUp_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_OffsetUp.MouseDown

        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            editorFrame.UpdateOffsets(New Point(0, 16))
        Else
            editorFrame.UpdateOffsets(New Point(0, 1))
        End If

        clsTasks.Update_preview()

    End Sub

    Private Sub TsbFrame_OffsetDown_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_OffsetDown.MouseDown


        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            editorFrame.UpdateOffsets(New Point(0, -16))
        Else
            editorFrame.UpdateOffsets(New Point(0, -1))
        End If

        clsTasks.Update_preview()

    End Sub

    Private Sub TsbFrame_OffsetLeft_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_OffsetLeft.MouseDown



        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            editorFrame.UpdateOffsets(New Point(16, 0))
        Else
            editorFrame.UpdateOffsets(New Point(1, 0))
        End If

        clsTasks.Update_preview()

    End Sub

    Private Sub TsbFrame_OffsetRight_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_OffsetRight.MouseDown


        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            editorFrame.UpdateOffsets(New Point(-16, 0))
        Else
            editorFrame.UpdateOffsets(New Point(-1, 0))
        End If

        clsTasks.Update_preview()



    End Sub

    Private Sub TsbGraphic_ExtraFrame_Click(sender As Object, e As EventArgs) Handles tsbGraphic_ExtraFrame.Click

        If editorGraphic.ExtraFrame = 0 Then
            editorGraphic.ExtraFrame = 1
        Else
            editorGraphic.ExtraFrame = 0
        End If


        ' Quick fix: on change, revert to frame 1.
        Dim intFrameNumber As Integer = editorGraphic.Frames.IndexOf(editorFrame)

        ' Was displaying last frame?
        If (intFrameNumber = editorGraphic.Frames.Count - 1) Then
            ' Show first one instead
            clsTasks.Update_Preview(0)
        Else
            ' Update current frame
            clsTasks.Update_Preview()
        End If



    End Sub

    Private Sub TsbFrame_IndexDecrease_Click(sender As Object, e As EventArgs) Handles tsbFrame_IndexDecrease.Click

        ' Change handled in slider control
        TbFrames.Value -= 1




    End Sub

    Private Sub Tstools_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsTools.ItemClicked

    End Sub

    Private Sub TsbDelete_PNG_Click(sender As Object, e As EventArgs) Handles tsbDelete_PNG.Click

        clsTasks.CleanUp_files(Cfg_path_Root, ".png")
        MsgBox("Finished clean up.", vbOKOnly + vbInformation, "Finished clean up.")

    End Sub

    Private Sub TsbDelete_ZT1Files_Click(sender As Object, e As EventArgs) Handles tsbDelete_ZT1Files.Click

        ' Cleanup ZT1 Graphics and color palettes
        clsTasks.CleanUp_files(Cfg_path_Root, "")
        clsTasks.CleanUp_files(Cfg_path_Root, ".pal")
        MsgBox("Finished clean up.", vbOKOnly + vbInformation, "Finished clean up.")

    End Sub



    Private Sub TsbFrame_fpX_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tsbFrame_fpX.SelectedIndexChanged

        Cfg_grid_footPrintX = tsbFrame_fpX.Text
        clsTasks.Config_write()
        clsTasks.Update_preview()

    End Sub

    Private Sub TsbFrame_fpY_ForeColorChanged(sender As Object, e As EventArgs) Handles tsbFrame_fpY.ForeColorChanged

    End Sub
    Private Sub TsbFrame_fpY_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tsbFrame_fpY.SelectedIndexChanged


        Cfg_grid_footPrintY = tsbFrame_fpY.Text
        clsTasks.Config_write()
        clsTasks.Update_preview()

    End Sub


    Private Sub DgvPaletteMain_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPaletteMain.CellDoubleClick

        ' Actual row rather than header. Avoid crash.
        If e.RowIndex > -1 Then

            ' Replace colors
            clsTasks.Pal_ReplaceColor(e.RowIndex)

        End If


    End Sub

    Private Sub DgvPaletteMain_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvPaletteMain.CellMouseClick

        If e.RowIndex <> -1 Then
            dgvPaletteMain.Rows(e.RowIndex).Selected = True

        End If


        If e.Button = Windows.Forms.MouseButtons.Right Then

            ' visible?
            mnuPal_MoveDown.Visible = (e.RowIndex <> dgvPaletteMain.Rows.Count - 1)
            mnuPal_MoveEnd.Visible = (e.RowIndex <> dgvPaletteMain.Rows.Count - 1)
            mnuPal_MoveUp.Visible = (e.RowIndex <> 0)

            'mnuPal.Show(Me.dgvPaletteMain, e.Location)
            mnuPal.Show(Cursor.Position)

        End If

    End Sub

    Private Sub MnuPal_MoveUp_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveUp.Click

        clsTasks.Pal_MoveColor(dgvPaletteMain.SelectedRows(0).Index, dgvPaletteMain.SelectedRows(0).Index - 1)

    End Sub
    Private Sub MnuPal_MoveDown_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveDown.Click

        clsTasks.Pal_MoveColor(dgvPaletteMain.SelectedRows(0).Index, dgvPaletteMain.SelectedRows(0).Index + 1)

    End Sub


    Private Sub MnuPal_Replace_Click(sender As Object, e As EventArgs) Handles mnuPal_Replace.Click

        clsTasks.Pal_ReplaceColor(dgvPaletteMain.SelectedRows(0).Index)


    End Sub

    Private Sub MnuPal_MoveEnd_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveEnd.Click

        clsTasks.Pal_MoveColor(dgvPaletteMain.SelectedRows(0).Index, editorGraphic.ColorPalette.Colors.Count - 1)


    End Sub

    Private Sub MnuPal_Add_Click(sender As Object, e As EventArgs) Handles mnuPal_Add.Click

        ' Add after this entry
        Pal_AddColor(dgvPaletteMain.SelectedRows(0).Index)

    End Sub


    Private Sub TsbFrame_ImportPNG_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_ImportPNG.MouseDown

        ' Shortcut to create a new frame first, then import the PNG to it.

        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            ' Add frame


0:
            Dim ztFrame As New ClsFrame(editorGraphic)
2:

10:
            ' Add the frame after the existing one(s)
            editorGraphic.Frames.Insert(TbFrames.Value, ztFrame)

15:
            ' not sure if this is right if an extra frame is applied?
            TbFrames.Maximum = editorGraphic.Frames.Count - editorGraphic.ExtraFrame

16:
            TbFrames.Value += 1

            clsTasks.Update_preview(TbFrames.Value - 1)

        End If



100:

        With dlgOpen
            .Title = "Pick a .PNG file"
            .DefaultExt = ""
            .Filter = "PNG files|*.png"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentPNG)

            ' If most recent directory does not exist anymore:
            If dlgOpen.InitialDirectory = vbNullString Or System.IO.Directory.Exists(dlgOpen.InitialDirectory) = False Then
                .InitialDirectory = Cfg_path_Root

            End If

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                If System.IO.File.Exists(dlgOpen.FileName) = True Then

                    If Path.GetExtension(dlgOpen.FileName).ToLower() <> ".png" Then
                        MsgBox("You selected a file with the extension '" & Path.GetExtension(dlgOpen.FileName) & "'." & vbCrLf &
                               "You need a file with a .PNG extension.",
                               vbOKOnly + vbCritical, "Invalid file")
                    Else

                        ' OK
                        editorFrame.LoadPNG(dlgOpen.FileName)


                        ' Draw first frame 
                        clsTasks.Update_preview()

                        ' Show main color palette
                        editorFrame.Parent.ColorPalette.FillPaletteGrid(dgvPaletteMain)

                        ' Not sure why we had this. It's the color palette of the background graphic.
                        'editorBgGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)

                        ' Remember
                        Cfg_path_recentPNG = System.IO.Path.GetFullPath(dlgOpen.FileName)
                        clsTasks.Config_write()


                    End If
                Else
                    MsgBox("File does not exist.", vbOKOnly + vbCritical, "Invalid file")
                End If

            End If ' End Cancel check


        End With


    End Sub



    Private Sub MnuPal_ExportPNG_Click(sender As Object, e As EventArgs) Handles mnuPal_ExportPNG.Click

        With dlgSave

            .Title = "Save as a PNG Color Palette"
            .DefaultExt = ".png"
            .Filter = "PNG Color Palette files (*.png)|*.png|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                editorGraphic.ColorPalette.Export_to_PNG(dlgSave.FileName)

            End If ' cancel check


        End With



    End Sub

    Private Sub MnuPal_ImportPNG_Click(sender As Object, e As EventArgs) Handles mnuPal_ImportPNG.Click



        With dlgOpen

            .Title = "Pick a PNG Color Palette"
            .DefaultExt = ".png"
            .Filter = "PNG Color Palette files (*.png)|*.png|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Replace palette file (should trigger a re-draw AFTERWARDS )
                ' Forcefully add colors (some might be the same, after a recolor)
                editorGraphic.ColorPalette.Import_from_PNG(dlgOpen.FileName)

                ' Update color list on the right
                editorGraphic.ColorPalette.FillPaletteGrid(dgvPaletteMain)


                ' Now after the color palette has been replaced, our preview must be updated
                clsTasks.Update_preview()

            End If ' cancel check


        End With


    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub MnuPal_SavePAL_Click(sender As Object, e As EventArgs) Handles mnuPal_SavePAL.Click

        With dlgSave

            .Title = "Save as a ZT1 Color Palette"
            .DefaultExt = ".pal"
            .Filter = "ZT1 Color Palette files (*.pal)|*.pal|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                editorGraphic.ColorPalette.WritePal(dlgSave.FileName, True)

            End If ' cancel check


        End With

    End Sub

    ''' <summary>
    ''' Button triggers an action to import a GIMP Color Palette
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MnuPal_ImportGimpPalette_Click(sender As Object, e As EventArgs) Handles mnuPal_ImportGimpPalette.Click

        With dlgOpen

            .Title = "Pick a GIMP Color Palette"
            .DefaultExt = ".gpl"
            .Filter = "GIMP Color Palette (*.gpl)|*.gpl|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Replace palette file (should trigger a redraw of coreImageBitmap)
                ' Forcefully add colors (some might be the same, after a recolor)
                editorGraphic.ColorPalette.Import_from_GimpPalette(dlgOpen.FileName)

                ' Update color list on the right
                editorGraphic.ColorPalette.FillPaletteGrid(dgvPaletteMain)

            End If ' cancel check

        End With

    End Sub

    Private Sub TsbFrame_OffsetLeft_Click(sender As Object, e As EventArgs) Handles tsbFrame_OffsetLeft.Click

    End Sub

    Private Sub TsbBatchRotFix_Click(sender As Object, e As EventArgs) Handles tsbBatchRotFix.Click
        FrmBatchRotationFix.ShowDialog(Me)

    End Sub

    Private Sub TsbFrame_OffsetRight_Click(sender As Object, e As EventArgs) Handles tsbFrame_OffsetRight.Click

    End Sub

    Private Sub TsbFrame_OffsetUp_Click(sender As Object, e As EventArgs) Handles tsbFrame_OffsetUp.Click

    End Sub


    Private Sub Button1_Click_2(sender As Object, e As EventArgs)


        'picBox.Image = editorGraphic.frames(0).renderCoreImageFromHex()
        picBox.Image = editorGraphic.Frames(0).GetImage(True)



    End Sub

    Private Sub TsbZT1New_Click(sender As Object, e As EventArgs) Handles tsbZT1New.Click


        editorGraphic = New ClsGraphic


        ' Always start with one frame
        editorFrame = New ClsFrame(editorGraphic)
        editorGraphic.Frames.Add(editorFrame)

        ' Update/reset color palette
        editorGraphic.ColorPalette.FillPaletteGrid(dgvPaletteMain)

        ' Update frame 
        picBox.Image = Nothing



    End Sub


    Private Sub TsbZT1Write_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbZT1Write.MouseDown

        If editorGraphic.Frames.Count = 0 Then
            MsgBox("You can't create a ZT1 Graphic without adding a frame first.", vbOKOnly + vbCritical, "No frames")
            Exit Sub
        End If

        If (e.Button = Windows.Forms.MouseButtons.Right) Then

            ' Shortcut to saving directly
            If File.Exists(editorGraphic.FileName) = True Then

                clsTasks.Save_Graphic(editorGraphic.FileName)
                clsTasks.Config_write()

                'No need to continue
                Exit Sub

            End If

        End If

        ' Shortcut above failed, go over entire saving process

        ' Where shall we save this ZT1 Graphic?
        With dlgSave
            .Title = "Save ZT1 Graphic"
            .DefaultExt = ""
            .AddExtension = True
            .InitialDirectory = System.IO.Path.GetDirectoryName(Cfg_path_recentZT1)
            .Filter = "ZT1 Graphics|*"


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                If Path.GetExtension(dlgSave.FileName).ToLower() <> "" Then
                    MsgBox("A ZT1 Graphic file does not have a file extension.", vbOKOnly + vbCritical, "Invalid filename")
                    Exit Sub

                End If

51:
                clsTasks.Save_Graphic(dlgSave.FileName)


60:
                ' Remember
                Cfg_path_recentZT1 = System.IO.Path.GetFullPath(dlgSave.FileName)
                clsTasks.Config_write()

                ' What has been opened, might need to be saved.
                dlgOpen.FileName = dlgSave.FileName

            End If


        End With



    End Sub

    Private Sub SsBar_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ssBar.ItemClicked

    End Sub

    Private Sub MnuPal_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mnuPal.Opening

    End Sub
End Class
