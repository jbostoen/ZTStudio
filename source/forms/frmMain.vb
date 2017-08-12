Option Explicit On


Imports System.IO



Public Class frmMain




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
         
         

        Debug.Flush()
        'mdlSettings.DoubleBuffered(dgvPaletteMain, True)
        dgvPaletteMain.GetType.InvokeMember("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.SetProperty, Nothing, dgvPaletteMain, New Object() {True})


        ' Always start with one frame
        editorFrame = New clsFrame2(editorGraphic)
        editorGraphic.frames.Add(editorFrame)


        ' Output
        bm = New Bitmap(cfg_grid_numPixels * 2, cfg_grid_numPixels * 2)
        With picBox
            .Width = cfg_grid_numPixels * 2
            .Height = cfg_grid_numPixels * 2
        End With

        ' Background color
        picBox.BackColor = cfg_grid_BackGroundColor


        ' Grid
        tsbFrame_fpX.Text = CStr(cfg_grid_footPrintX)
        tsbFrame_fpY.Text = CStr(cfg_grid_footPrintY)

        'clsTasks.update_Info()

        ' ZT1 Default color palettes
        ' strPathBuildingColorPals

        Dim di As New IO.DirectoryInfo(cfg_path_ColorPals8)
        Dim diar1 As IO.FileInfo() = di.GetFiles()
        Dim dra As IO.FileInfo

        'list the names of all files in the specified directory
        For Each dra In diar1
            tsbOpenPalBldg8.DropDownItems.Add(dra.Name)
        Next

        di = New IO.DirectoryInfo(cfg_path_ColorPals16)
        diar1 = di.GetFiles()

        'list the names of all files in the specified directory
        For Each dra In diar1
            tsbOpenPalBldg16.DropDownItems.Add(dra.Name)
        Next


    End Sub


    Private Sub picBox_MouseEnter(sender As Object, e As EventArgs) Handles picBox.MouseEnter

    End Sub

    Private Sub picBox_MouseMove(sender As Object, e As MouseEventArgs) Handles picBox.MouseMove

        On Error GoTo dBug

        ' If we have something in our canvas / if we have a cached frame rendered, 
        ' we will show color info on mouseover.
1:
        If IsNothing(picBox.Image) Then Exit Sub

2:
        ' frame might have been just initiated
        If IsNothing(editorFrame.coreImageBitmap) And IsNothing(editorFrame.coreImageHex) Then Exit Sub



3:
        Dim bmTmp As Bitmap
        bmTmp = picBox.Image ' editorFrame.getImage()
        Application.DoEvents()

4:
        If IsNothing(bmTmp) Then Exit Sub


20:
        Dim eX As Integer = (picBox.Width - bmtmp.Width) / 2
        Dim eY As Integer = (picBox.Height - bmtmp.Height) / 2

100:
        If e.X - eX >= 0 And e.X - eX < bmtmp.Width And e.Y - eY >= 0 And e.Y - eY < bmtmp.Height Then

101:
            Dim c As System.Drawing.Color = bmtmp.GetPixel(e.X - eX, e.Y - eY)


            If c.A <> 0 Then

102:
                lblColor.BackColor = c '.ToString()
                lblColorDetails.Text = "Coordinates: x: " & e.X - eX & " , y: " & e.Y - eY & vbCrLf &
                    "RGB: " & c.R & "," & c.G & "," & c.B & vbCrLf & _
                    "Index in .pal file: # " & editorGraphic.colorPalette.colors.IndexOf(c)



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

        MsgBox("Error in frmMain.picBox.MouseMove() " & vbCrLf & _
            "Line " & Erl() & vbCrLf & _
            Err.Number & " " & Err.Description, vbOKOnly + vbCritical + vbApplicationModal, "Error determining pixel color")


    End Sub

    Private Sub picBox_MouseWheel(sender As Object, e As MouseEventArgs) Handles picBox.MouseWheel

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


    Private Sub tbFrames_ValueChanged(sender As Object, e As EventArgs) Handles tbFrames.ValueChanged

        clsTasks.update_preview(tbFrames.Value - 1)

    End Sub

    Private Sub tmrAnimation_Tick(sender As Object, e As EventArgs) Handles tmrAnimation.Tick


        Debug.Print(tmrAnimation.Interval)


        If (tbFrames.Value = tbFrames.Maximum) Then
            tbFrames.Value = 1
        Else
            tbFrames.Value += 1
        End If

        clsTasks.update_preview(tbFrames.Value - 1)

    End Sub



    Private Sub tsbGridBG_Click(sender As Object, e As EventArgs) Handles tsbGridBG.Click

        With dlgColor
            .Color = cfg_grid_BackGroundColor
            .ShowDialog()

            cfg_grid_BackGroundColor = .Color


        End With


        clsTasks.config_write()
        clsTasks.update_Info("Background color changed.")


    End Sub

    Private Sub tsbZT1Open_Click(sender As Object, e As EventArgs) Handles tsbZT1Open.Click

         


        With dlgOpen
            .Title = "Pick a ZT1 Graphic"
            .DefaultExt = ""
            .Filter = "All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentZT1)
             

            If dlgOpen.InitialDirectory = vbNullString Or System.IO.Directory.Exists(dlgOpen.InitialDirectory) = False Then
                If System.IO.Directory.Exists(cfg_path_Root) Then
                    .InitialDirectory = cfg_path_Root
                ElseIf System.IO.Directory.Exists("C:\Program Files\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files\Microsoft Games\Zoo Tycoon"
                ElseIf System.IO.Directory.Exists("C:\Program Files (x86)\Microsoft Games\Zoo Tycoon") Then
                    .InitialDirectory = "C:\Program Files (x86)\Microsoft Games\Zoo Tycoon"

                End If
            End If

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                If System.IO.File.Exists(dlgOpen.FileName) = True Then

                    If Path.GetExtension(dlgOpen.FileName) <> vbNullString Then
                        MsgBox("You selected a file with the extension '" & Path.GetExtension(dlgOpen.FileName) & "'." & vbCrLf & _
                               "With ZT1 graphic, we mean a ZT1 Graphic file without extension.", _
                               vbOKOnly + vbCritical, "Invalid file")

                        Exit Sub


                    ElseIf dlgOpen.FileName.ToLower().Contains(cfg_path_Root.ToLower()) = False Then

                        MsgBox("Only select a file in the root directory, which you can change in Settings:" & vbCrLf & _
                               cfg_path_Root, vbOKOnly + vbCritical, "Invalid path")
                        Exit Sub


                    Else

                        ' Reset any previous info.
                        editorGraphic = New clsGraphic2

                        ' OK
                        editorGraphic.read(dlgOpen.FileName)


                        ' Keep filename
                        ssFileName.Text = dlgOpen.FileName

                        ' Draw first frame 
                        clsTasks.update_preview(0)

                        ' Add time indication
                        lblAnimTime.Text = ((editorGraphic.frames.Count - editorGraphic.extraFrame) * editorGraphic.animationSpeed) & " ms "
                        lblFrames.Text = (editorGraphic.frames.Count - editorGraphic.extraFrame) & " frames. "

                        ' Show default palette
                        editorGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)

                        ' Set editorframe
                        editorFrame = editorGraphic.frames(0)
                        tbFrames.Value = 1



                    End If
                Else
                    MsgBox("File does not exist.", vbOKOnly + vbCritical, "Invalid file")
                End If

                ' Remember
                cfg_path_recentZT1 = System.IO.Path.GetFullPath(dlgOpen.FileName) 
                clsTasks.config_write()

                ' What has been opened, might need to be saved.
                dlgSave.FileName = dlgOpen.FileName

            End If ' End Cancel check


        End With
    End Sub

    Private Sub tbFrames_ValueChanged1(sender As Object, e As EventArgs) Handles tbFrames.ValueChanged

        clsTasks.update_preview(tbFrames.Value - 1)

        Debug.Print("Value changed.")

        editorFrame = editorGraphic.frames(tbFrames.Value - 1)

    End Sub

    Private Sub chkPlayAnimation_CheckedChanged_1(sender As Object, e As EventArgs) Handles chkPlayAnimation.CheckedChanged

        If chkPlayAnimation.Checked = True Then

            tmrAnimation.Interval = editorGraphic.animationSpeed
            tmrAnimation.Enabled = True

        Else

            tmrAnimation.Enabled = False
        End If

    End Sub

    Private Sub tsbAbout_Click(sender As Object, e As EventArgs) Handles tsbAbout.Click

        MsgBox("About " & Application.ProductName & " " & Application.ProductVersion & vbCrLf & _
            "___________________________" & vbCrLf & _
            "© since 2015 by Jeffrey Bostoen" & vbCrLf & _
            "https://github.com/jbostoen/ZTStudio" & vbCrLf & _
            vbCrLf & vbCrLf & _
            "Bugs? " & vbCrLf & _
            "-------------------" & vbCrLf & _
            "- You can report them at GitHub or Zoo Tek Phoenix. " & vbCrLf & _
            "- Support not guaranteed. " & vbCrLf & _
            "- Include the graphic files which are causing the problem. " & vbCrLf & vbCrLf & _
            "Credits? " & vbCrLf & _
            "-------------------" & vbCrLf & _
            "- Blue Fang for creating Zoo Tycoon 1 (and maybe the graphic format)." & vbCrLf & _
            "- Microsoft for publishing the game." & vbCrLf & _
            "- Rapan Studios for the animal designs." & vbCrLf & _
            "- MadScientist and Jay for explaining the file format." & vbCrLf & _
            "- Vondell for providing new PNG graphics to experiment with." & vbCrLf & _
            "- HENDRIX for some contributions to the source code.", _
            vbOKOnly + vbInformation, "About ZT Studio")


    End Sub

    Private Sub tsbFrame_ExportPNG_Click(sender As Object, e As EventArgs) Handles tsbFrame_ExportPNG.Click


        With dlgSave
            .Title = "Save single frame as .PNG"
            .DefaultExt = ".png"
            .AddExtension = True
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentPNG)
            .Filter = "PNG files (*.png)|*.png|All files|*.*"


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                ' was bminput


                ' bminput.cachedFrame.Save(dlgSave.FileName, System.Drawing.Imaging.ImageFormat.Png)

                editorFrame.savePNG(dlgSave.FileName)


                ' Remember
                cfg_path_recentPNG = System.IO.Path.GetFullPath(dlgSave.FileName)
                clsTasks.config_write()


            End If

        End With



    End Sub


    Private Sub tsbZT1_OpenPal_Click(sender As Object, e As EventArgs) Handles tsbZT1_OpenPal.Click


        With dlgOpen

            .Title = "Pick a ZT1 Color Palette"
            .DefaultExt = ".pal"
            .Filter = "ZT1 Color Palette files (*.pal)|*.pal|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                pal_open(dlgOpen.FileName)

            End If ' cancel check


        End With

    End Sub





    Private Sub tsbSettings_Click(sender As Object, e As EventArgs) Handles tsbSettings.Click

        frmSettings.ShowDialog()

    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsFrame.ItemClicked

    End Sub

    Private Sub tsbOpenPalBldg8_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsbOpenPalBldg8.DropDownItemClicked

        pal_open(cfg_path_ColorPals8 & "\" & e.ClickedItem.Text)

    End Sub
    Private Sub tsbOpenPalBldg16_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsbOpenPalBldg16.DropDownItemClicked

        pal_open(cfg_path_ColorPals16 & "\" & e.ClickedItem.Text)

    End Sub

    Private Sub tsbBatchConversion_Click(sender As Object, e As EventArgs) Handles tsbBatchConversion.Click

        frmBatchConversion.ShowDialog(Me)

    End Sub

    Private Sub tsbPreview_BGGraphic_Click(sender As Object, e As EventArgs) Handles tsbPreview_BGGraphic.Click


        With dlgOpen
            .Title = "Pick a ZT1 Graphic"
            .DefaultExt = ""
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentZT1)
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
                        MsgBox("You selected a file with the extension '" & Path.GetExtension(dlgOpen.FileName) & "'." & vbCrLf & _
                               "With ZT1 graphic, we mean a ZT1 graphics file without extension.", _
                               vbOKOnly + vbCritical, "Invalid file")
                    Else

                        ' OK
                        editorBgGraphic.read(dlgOpen.FileName)




                        ' Keep filename
                        ssFileName.Text = dlgOpen.FileName



                        ' reDraw current frame 
                        clsTasks.update_preview()

                        ' Show default palette
                        'editorBgGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)

                        ' Remember
                        cfg_path_recentZT1 = System.IO.Path.GetFullPath(dlgOpen.FileName)
                        clsTasks.config_write()

                    End If
                Else
                    MsgBox("File does not exist.", vbOKOnly + vbCritical, "Invalid file")
                End If

            End If ' End Cancel check


        End With
    End Sub




    Private Sub tsbZT1Write_Click(sender As Object, e As EventArgs) Handles tsbZT1Write.Click


        If editorGraphic.frames.Count = 0 Then
            MsgBox("You can't create a ZT1 Graphic without adding a frame first.", vbOKOnly + vbCritical, "No frames")
            Exit Sub
        End If


        ' Where shall we save this ZT1 Graphic?
        With dlgSave
            .Title = "Save ZT1 Graphic"
            .DefaultExt = ""
            .AddExtension = True
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentZT1)
            .Filter = "ZT1 Graphics|*"


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                If Path.GetExtension(dlgSave.FileName).ToLower() <> "" Then
                    MsgBox("A ZT1 Graphic file does not have a file extension.", vbOKOnly + vbCritical, "Invalid filename")
                    Exit Sub

                End If

                ' 20150624. We have <filename>.pal here. 
                ' We do this to avoid issues with shared color palettes, if users are NOT familiar with them.
                ' We are assuming pro users will only tweak and use the batch conversion.
                With editorGraphic
                    .fileName = dlgSave.FileName
                    .colorPalette.fileName = editorGraphic.fileName & ".pal"
                    .write(dlgSave.FileName, True)
                End With

50:
                'GoTo 60


                If cfg_export_ZT1_Ani = 1 Then
                    Debug.Print("Try .ani")
                    ' Get the folder + name of the folder + .ani
                    Dim cAni As New clsAniFile(Path.GetDirectoryName(dlgSave.FileName) & "\" & Path.GetFileName(Path.GetDirectoryName(dlgSave.FileName)) & ".ani")
                    cAni.createAniConfig()
                End If

60:


                MsgBox("ZT1 Graphic created.", vbOKOnly + vbInformation, "ZT1 Graphic created.")



                ' Remember
                cfg_path_recentZT1 = System.IO.Path.GetFullPath(dlgSave.FileName)
                clsTasks.config_write()



                ' What has been opened, might need to be saved.
                dlgOpen.FileName = dlgSave.FileName

            End If


        End With


    End Sub

    Private Sub tstZT1_AnimSpeed_Click(sender As Object, e As EventArgs) Handles tstZT1_AnimSpeed.Click

    End Sub

    Private Sub tstZT1_AnimSpeed_TextChanged(sender As Object, e As EventArgs) Handles tstZT1_AnimSpeed.TextChanged



        If IsNumeric(tstZT1_AnimSpeed.Text) = False Then
            MsgBox("Invalid value." & vbCrLf & "The animation speed should be a number of milliseconds.", _
                   vbOKOnly + vbCritical, "Invalid value for animation speed.")
            tstZT1_AnimSpeed.Text = "1000"
            Exit Sub

        Else

            If CInt(tstZT1_AnimSpeed.Text) < 1 And (CInt(tstZT1_AnimSpeed.Text) > 1000) Then
                MsgBox("Invalid value." & vbCrLf & "ZT Studio currently expects a value between 1 and 1000 milliseconds.", _
                       vbOKOnly + vbCritical, "Invalid value for animation speed.")
                tstZT1_AnimSpeed.Text = "1000"
                Exit Sub

            Else

                editorGraphic.animationSpeed = CInt(tstZT1_AnimSpeed.Text)


            End If
        End If

    End Sub

    Private Sub tbFrames_Scroll(sender As Object, e As EventArgs) Handles tbFrames.Scroll

    End Sub

    Private Sub tsbFrame_Add_Click(sender As Object, e As EventArgs) Handles tsbFrame_Add.Click

        On Error GoTo dBug




0:
        Dim ztFrame As New clsFrame2(editorGraphic)
2:

10:
        editorGraphic.frames.Insert(tbFrames.Value, ztFrame) ' add after

15:
        ' not sure if this is right if an extra frame is applied?
        tbFrames.Maximum = editorGraphic.frames.Count - editorGraphic.extraFrame

16:
        tbFrames.Value += 1

        clsTasks.update_preview(tbFrames.Value - 1)

        Exit Sub

dBug:
        MsgBox("Error at line " & Erl() & " in tsbFrame_Add: " & Err.Number & " - " & Err.Description, vbOKOnly + vbCritical, "Error")

    End Sub

    Private Sub tsbFrame_Delete_Click(sender As Object, e As EventArgs) Handles tsbFrame_Delete.Click

        editorGraphic.frames.RemoveAt(tbFrames.Value - 1)

        clsTasks.update_preview(tbFrames.Value - 1)


    End Sub

    Private Sub picBox_Click(sender As Object, e As EventArgs) Handles picBox.Click

    End Sub

    Private Sub tsbFrame_IndexIncrease_Click(sender As Object, e As EventArgs) Handles tsbFrame_IndexIncrease.Click

        ' Change handled in slider control
        tbFrames.Value += 1


    End Sub


    Private Sub tsbFrame_OffsetUp_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_OffsetUp.MouseDown

        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            editorFrame.updateOffsets(New Point(0, 16))
        Else
            editorFrame.updateOffsets(New Point(0, 1))
        End If

        clsTasks.update_preview()

    End Sub

    Private Sub tsbFrame_OffsetDown_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_OffsetDown.MouseDown


        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            editorFrame.updateOffsets(New Point(0, -16))
        Else
            editorFrame.updateOffsets(New Point(0, -1))
        End If

        clsTasks.update_preview()

    End Sub

    Private Sub tsbFrame_OffsetLeft_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_OffsetLeft.MouseDown



        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            editorFrame.updateOffsets(New Point(16, 0))
        Else
            editorFrame.updateOffsets(New Point(1, 0))
        End If

        clsTasks.update_preview()

    End Sub

    Private Sub tsbFrame_OffsetRight_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_OffsetRight.MouseDown


        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            editorFrame.updateOffsets(New Point(-16, 0))
        Else
            editorFrame.updateOffsets(New Point(-1, 0))
        End If

        clsTasks.update_preview()



    End Sub

    Private Sub tsbGraphic_ExtraFrame_Click(sender As Object, e As EventArgs) Handles tsbGraphic_ExtraFrame.Click

        If editorGraphic.extraFrame = 0 Then
            editorGraphic.extraFrame = 1
        Else
            editorGraphic.extraFrame = 0
        End If


        ' Quick fix: on change, revert to frame 1.
        clsTasks.update_preview(0)






    End Sub

    Private Sub tsbFrame_IndexDecrease_Click(sender As Object, e As EventArgs) Handles tsbFrame_IndexDecrease.Click

        ' Change handled in slider control
        tbFrames.Value -= 1




    End Sub

    Private Sub tsTools_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsTools.ItemClicked

    End Sub

    Private Sub tsbDelete_PNG_Click(sender As Object, e As EventArgs) Handles tsbDelete_PNG.Click

        clsTasks.cleanUp_files(cfg_path_Root, ".png")
        MsgBox("Finished clean up.", vbOKOnly + vbInformation, "Finished clean up.")

    End Sub

    Private Sub tsbDelete_ZT1Files_Click(sender As Object, e As EventArgs) Handles tsbDelete_ZT1Files.Click

        ' Cleanup ZT1 Graphics and color palettes
        clsTasks.cleanUp_files(cfg_path_Root, "")
        clsTasks.cleanUp_files(cfg_path_Root, ".pal")
        MsgBox("Finished clean up.", vbOKOnly + vbInformation, "Finished clean up.")

    End Sub



    Private Sub tsbFrame_fpX_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tsbFrame_fpX.SelectedIndexChanged

        cfg_grid_footPrintX = tsbFrame_fpX.Text
        clsTasks.config_write()
        clsTasks.update_preview()

    End Sub

    Private Sub tsbFrame_fpY_ForeColorChanged(sender As Object, e As EventArgs) Handles tsbFrame_fpY.ForeColorChanged

    End Sub
    Private Sub tsbFrame_fpY_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tsbFrame_fpY.SelectedIndexChanged


        cfg_grid_footPrintY = tsbFrame_fpY.Text
        clsTasks.config_write()
        clsTasks.update_preview()

    End Sub


    Private Sub dgvPaletteMain_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPaletteMain.CellDoubleClick

        ' Actual row rather than header. Avoid crash.
        If e.RowIndex > -1 Then

            ' Replace colors
            clsTasks.pal_replaceColor(e.RowIndex)

        End If


    End Sub

    Private Sub dgvPaletteMain_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvPaletteMain.CellMouseClick

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

    Private Sub mnuPal_MoveUp_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveUp.Click

        clsTasks.pal_moveColor(dgvPaletteMain.SelectedRows(0).Index, dgvPaletteMain.SelectedRows(0).Index - 1)

    End Sub
    Private Sub mnuPal_MoveDown_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveDown.Click

        clsTasks.pal_moveColor(dgvPaletteMain.SelectedRows(0).Index, dgvPaletteMain.SelectedRows(0).Index + 1)

    End Sub


    Private Sub mnuPal_Replace_Click(sender As Object, e As EventArgs) Handles mnuPal_Replace.Click

        clsTasks.pal_replaceColor(dgvPaletteMain.SelectedRows(0).Index)


    End Sub

    Private Sub mnuPal_MoveEnd_Click(sender As Object, e As EventArgs) Handles mnuPal_MoveEnd.Click

        clsTasks.pal_moveColor(dgvPaletteMain.SelectedRows(0).Index, editorGraphic.colorPalette.colors.Count - 1)


    End Sub

    Private Sub mnuPal_Add_Click(sender As Object, e As EventArgs) Handles mnuPal_Add.Click

        ' Add after this entry
        pal_addColor(dgvPaletteMain.SelectedRows(0).Index)

    End Sub


    Private Sub tsbFrame_ImportPNG_MouseDown(sender As Object, e As MouseEventArgs) Handles tsbFrame_ImportPNG.MouseDown

        ' Shortcut to create a new frame first, then import the PNG to it.

        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            ' Add frame


0:
            Dim ztFrame As New clsFrame2(editorGraphic)
2:

10:
            ' Add the frame after the existing one(s)
            editorGraphic.frames.Insert(tbFrames.Value, ztFrame)

15:
            ' not sure if this is right if an extra frame is applied?
            tbFrames.Maximum = editorGraphic.frames.Count - editorGraphic.extraFrame

16:
            tbFrames.Value += 1

            clsTasks.update_preview(tbFrames.Value - 1)

        End If



100:

        With dlgOpen
            .Title = "Pick a .PNG file"
            .DefaultExt = ""
            .Filter = "PNG files|*.png"
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentPNG)

            ' If most recent directory does not exist anymore:
            If dlgOpen.InitialDirectory = vbNullString Or System.IO.Directory.Exists(dlgOpen.InitialDirectory) = False Then
                .InitialDirectory = cfg_path_Root

            End If

            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then


                If System.IO.File.Exists(dlgOpen.FileName) = True Then

                    If Path.GetExtension(dlgOpen.FileName).ToLower() <> ".png" Then
                        MsgBox("You selected a file with the extension '" & Path.GetExtension(dlgOpen.FileName) & "'." & vbCrLf & _
                               "You need a file with a .PNG extension.", _
                               vbOKOnly + vbCritical, "Invalid file")
                    Else

                        ' OK
                        editorFrame.loadPNG(dlgOpen.FileName)


                        ' Draw first frame 
                        clsTasks.update_preview()

                        ' Show main color palette
                        editorFrame.parent.colorPalette.fillPaletteGrid(dgvPaletteMain)

                        ' Not sure why we had this. It's the color palette of the background graphic.
                        'editorBgGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)

                        ' Remember
                        cfg_path_recentPNG = System.IO.Path.GetFullPath(dlgOpen.FileName)
                        clsTasks.config_write()


                    End If
                Else
                    MsgBox("File does not exist.", vbOKOnly + vbCritical, "Invalid file")
                End If

            End If ' End Cancel check


        End With


    End Sub

    Private Sub tsbFrame_fpX_Click(sender As Object, e As EventArgs) Handles tsbFrame_fpX.Click

    End Sub

    Private Sub tslFrame_Index_Click(sender As Object, e As EventArgs) Handles tslFrame_Index.Click

    End Sub

    Private Sub tsbOpenPalBldg16_Click(sender As Object, e As EventArgs) Handles tsbOpenPalBldg16.Click

    End Sub


    Private Sub tsbFrame_fpY_Click(sender As Object, e As EventArgs) Handles tsbFrame_fpY.Click

    End Sub

    Private Sub tsbFrame_ImportPNG_Click(sender As Object, e As EventArgs) Handles tsbFrame_ImportPNG.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

        For Each col In editorFrame.parent.colorPalette.colors
            Debug.Print(col.ToString())

        Next
    End Sub


    Private Sub mnuPal_ExportPNG_Click(sender As Object, e As EventArgs) Handles mnuPal_ExportPNG.Click

        With dlgSave

            .Title = "Save as a PNG Color Palette"
            .DefaultExt = ".png"
            .Filter = "PNG Color Palette files (*.png)|*.png|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                editorGraphic.colorPalette.export_to_PNG(dlgSave.FileName)

            End If ' cancel check


        End With



    End Sub

    Private Sub mnuPal_ImportPNG_Click(sender As Object, e As EventArgs) Handles mnuPal_ImportPNG.Click



        With dlgOpen

            .Title = "Pick a PNG Color Palette"
            .DefaultExt = ".png"
            .Filter = "PNG Color Palette files (*.png)|*.png|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Replace palette file (should trigger a re-draw AFTERWARDS )
                ' Forcefully add colors (some might be the same, after a recolor)
                editorGraphic.colorPalette.import_from_PNG(dlgOpen.FileName)

                ' Update color list on the right
                editorGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)


                ' Now after the color palette has been replaced, our preview must be updated
                clsTasks.update_preview()

            End If ' cancel check


        End With


    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub mnuPal_SavePAL_Click(sender As Object, e As EventArgs) Handles mnuPal_SavePAL.Click

        With dlgSave

            .Title = "Save as a ZT1 Color Palette"
            .DefaultExt = ".pal"
            .Filter = "ZT1 Color Palette files (*.pal)|*.pal|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                editorGraphic.colorPalette.writePal(dlgSave.FileName, True)

            End If ' cancel check


        End With

    End Sub

    Private Sub mnuPal_ImportGimpPalette_Click(sender As Object, e As EventArgs) Handles mnuPal_ImportGimpPalette.Click


        With dlgOpen

            .Title = "Pick a GIMP Color Palette"
            .DefaultExt = ".gpl"
            .Filter = "GIMP Color Palette (*.gpl)|*.gpl|All files|*.*"
            .InitialDirectory = System.IO.Path.GetDirectoryName(cfg_path_recentZT1)


            If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then

                ' Replace palette file (should trigger a redraw of coreImageBitmap)
                ' Forcefully add colors (some might be the same, after a recolor)
                editorGraphic.colorPalette.import_from_GimpPalette(dlgOpen.FileName, True)

                ' Update color list on the right
                editorGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)


            End If ' cancel check


        End With

    End Sub

    Private Sub tsbFrame_OffsetLeft_Click(sender As Object, e As EventArgs) Handles tsbFrame_OffsetLeft.Click

    End Sub

    Private Sub tsbBatchRotFix_Click(sender As Object, e As EventArgs) Handles tsbBatchRotFix.Click
        frmBatchRotationFix.ShowDialog(Me)

    End Sub

    Private Sub tsbFrame_OffsetRight_Click(sender As Object, e As EventArgs) Handles tsbFrame_OffsetRight.Click

    End Sub

    Private Sub tsbFrame_OffsetUp_Click(sender As Object, e As EventArgs) Handles tsbFrame_OffsetUp.Click

    End Sub


    Private Sub Button1_Click_2(sender As Object, e As EventArgs)


        'picBox.Image = editorGraphic.frames(0).renderCoreImageFromHex()
        picBox.Image = editorGraphic.frames(0).getImage(True)



    End Sub

    Private Sub tsbZT1New_Click(sender As Object, e As EventArgs) Handles tsbZT1New.Click


        editorGraphic = New clsGraphic2


        ' Always start with one frame
        editorFrame = New clsFrame2(editorGraphic)
        editorGraphic.frames.Add(editorFrame)

        ' Update/reset color palette
        editorGraphic.colorPalette.fillPaletteGrid(dgvPaletteMain)

        ' Update frame 
        picBox.Image = Nothing



    End Sub

    Private Sub dgvPaletteMain_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPaletteMain.CellContentClick

    End Sub
End Class
