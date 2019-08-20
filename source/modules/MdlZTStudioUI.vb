''' <summary>
''' Methods related to ZT Studio UI
''' </summary>

Module MdlZTStudioUI


    ''' <summary>
    ''' Updates info in main screen.
    ''' Updates shown info such as animation speed, number of frames, current frame, ...
    ''' Enables/disables certain controls (for example, button to render background frame)
    ''' </summary>
    ''' <param name="StrReason"></param>
    Sub UpdateInfo(StrReason As String)

        ' Displays updated info.
        ' 20190816: note: before today, it relied on .indexOf(), which might return incorrect results if there are similar frames. Now intFrameIndex is added and required.


        Dim IntFrameIndex As Integer = FrmMain.TbFrames.Value - 1


        With FrmMain

            .tstZT1_AnimSpeed.Text = editorGraphic.AnimationSpeed

            ' NOT using a 0-based frame index visual indication, to avoid confusing
            .tslFrame_Index.Text = IIf(editorGraphic.Frames.Count = 0, "-", (intFrameIndex + 1) & " / " & (editorGraphic.Frames.Count - editorGraphic.ExtraFrame))

            MdlZTStudio.Trace("ClsTasks", "Update_Info", "Reason: " & StrReason & ". # non-background frames = " & (editorGraphic.Frames.Count - editorGraphic.ExtraFrame) & " - background frame: " & editorGraphic.ExtraFrame.ToString())

            With .TbFrames
                .Minimum = 1
                .Maximum = (editorGraphic.Frames.Count - editorGraphic.ExtraFrame) ' for actually generated files: - editorGraphic.extraFrame)

                If .Maximum < 1 Then
                    .Minimum = 1
                    .Maximum = 1
                End If
                If .Value < .Minimum Then
                    .Value = .Minimum
                End If

            End With

            ' == Graphic
            .tsbGraphic_ExtraFrame.Enabled = (editorGraphic.Frames.Count > 1) ' Background frame can only be enabled if there's more than one frame
            .tsbGraphic_ExtraFrame.Checked = (editorGraphic.ExtraFrame = 1) ' Is background frame enabled for this graphic? Then toggle button.

            ' == Frame
            .tsbFrame_Delete.Enabled = (editorGraphic.Frames.Count > 1)
            .tsbFrame_ExportPNG.Enabled = False

            '(IsNothing(editorGraphic.frames(0).cachedFrame) = False)

            If IsNothing(editorFrame) = False Then
                If editorFrame.CoreImageHex.Count > 0 Then
                    .tsbFrame_ExportPNG.Enabled = True
                End If
            End If


            .tsbFrame_ImportPNG.Enabled = (editorGraphic.Frames.Count > 0)

            .tsbFrame_OffsetDown.Enabled = (editorGraphic.Frames.Count > 0)
            .tsbFrame_OffsetUp.Enabled = (editorGraphic.Frames.Count > 0)
            .tsbFrame_OffsetLeft.Enabled = (editorGraphic.Frames.Count > 0)
            .tsbFrame_OffsetRight.Enabled = (editorGraphic.Frames.Count > 0)

            .tsbFrame_IndexIncrease.Enabled = (editorGraphic.Frames.Count > 1 And intFrameIndex < (editorGraphic.Frames.Count - 1 - editorGraphic.ExtraFrame))
            .tsbFrame_IndexDecrease.Enabled = (editorGraphic.Frames.Count > 1 And intFrameIndex > 0)

            .picBox.BackColor = Cfg_grid_BackGroundColor

            If IsNothing(editorFrame) Then
                .tslFrame_Offset.Text = "0 , 0"

            Else
                '.tbFrames.Value = editorGraphic.frames.IndexOf(editorFrame) + 1
                .tslFrame_Offset.Text = editorFrame.OffsetX & " , " & editorFrame.OffsetY
            End If

        End With

    End Sub


    ''' <summary>
    ''' Updates all sort of info.
    ''' </summary>
    ''' <param name="intIndexFrameNumber">Optional frame index number. Defaults to value of slider in main window.</param>
    Public Sub UpdatePreview(Optional IntIndexFrameNumber As Integer = -1)

1:
        ' Can't update if there are no frames.
        If editorGraphic.Frames.Count = 0 Then
            Exit Sub
        End If

2:
        ' Shortcut. If no index number for the frame was specified, assume the currently visible frame needs to be updated.
        If IntIndexFrameNumber = -1 Then
            IntIndexFrameNumber = FrmMain.TbFrames.Value - 1
        End If


25:
        ' 20190816: some aspects weren't managed properly, for instance when toggling extra frame or adding/removing frames.
        ' Previous/next frame; current And max value of progress bar, ...
        ' Update preview is called from lots of places, so this may be a bit of an overkill, but better safe.
        MdlZTStudioUI.UpdateInfo("Update Preview")

30:
        editorFrame = editorGraphic.Frames(IntIndexFrameNumber)

300:
        ' The sub gets triggered when a new frame has been added, but no .PNG has been loaded yet, so frame contains no data.
        ' However, the picbox may need to be cleared (previous frame would still be shown otherwise)
        If editorGraphic.Frames(IntIndexFrameNumber).CoreImageHex.Count = 0 Then
            FrmMain.picBox.Image = MdlTasks.Grid_DrawFootPrintXY(Cfg_grid_footPrintX, Cfg_grid_footPrintY)
            Exit Sub
        End If

320:
        FrmMain.picBox.Image = editorGraphic.Frames(IntIndexFrameNumber).GetImage(True)



    End Sub

End Module
