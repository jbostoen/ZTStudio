Imports System.IO
Imports System.Text.RegularExpressions



''' <summary>
''' Methods related to ZT Studio UI
''' </summary>

Module MdlZTStudioUI

    ''' <summary>
    ''' Load graphic and show
    ''' </summary>
    ''' <param name="StrFileName">Source file name</param>
    Sub LoadGraphic(StrFileName As String)

        MdlZTStudio.Trace("MdlZTStudioUI", "LoadGraphic", "Loading " & StrFileName)

        If System.IO.File.Exists(StrFileName) = False Then

            Dim StrMessage As String = "File does not exist."
            MdlZTStudio.HandledError("MdlZTStudioUI", "LoadGraphic", StrMessage)
            Exit Sub

        Else

            If Path.GetExtension(StrFileName) <> vbNullString Then

                Dim StrErrorMessage As String = "" &
                    "You selected a file with the extension '" & Path.GetExtension(StrFileName) & "'." & vbCrLf &
                    "ZT Studio expects you to select a ZT1 Graphic file, which shouldn't have a file extension."

                MdlZTStudio.HandledError("MdlZTStudioUI", "LoadGraphic", StrErrorMessage)

                Exit Sub


            ElseIf StrFileName.ToLower().Contains(Cfg_path_Root.ToLower()) = False Then

                Dim StrErrorMessage As String = "" &
                    "Only select a file in the root directory, which is currently:" & vbCrLf &
                    Cfg_Path_Root & vbCrLf & vbCrLf &
                    "Would you like to change the root directory?"

                If MsgBox(StrErrorMessage, MsgBoxStyle.YesNo + MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, "ZT1 Graphic not within root folder") = MsgBoxResult.Yes Then

                    ' Allow user to quickly change settings -> root directory
                    FrmSettings.Show()

                End If

                Exit Sub


            Else



                ' Reset any previous info.
                Dim StrFileNamePalette_Original As String = EditorGraphic.ColorPalette.FileName

                EditorGraphic = New ClsGraphic(EditorGraphic.ColorPalette)

                ' OK
                EditorGraphic.Read(StrFileName)

                ' Keep filename
                FrmMain.ssFileName.Text = Now.ToString("yyyy-MM-dd HH:mm:ss") & ": opened " & StrFileName

                ' Show default palette
                If StrFileNamePalette_Original <> EditorGraphic.ColorPalette.FileName Then
                    EditorGraphic.ColorPalette.FillPaletteGrid(FrmMain.DgvPaletteMain)
                End If


                ' Set editorframe
                EditorFrame = EditorGraphic.Frames(0)
                    FrmMain.TbFrames.Value = 1

                    ' Draw first frame. Must be done after setting EditorFrame!
                    MdlZTStudioUI.UpdatePreview(True, True, 0)

                    ' Remember
                    Cfg_Path_RecentZT1 = StrFileName
                    MdlConfig.Write()


                End If

            End If

        ' Select in Explorer Pane (if not the case yet)
        Dim ObjNodeSet As TreeNode() = FrmMain.TVExplorer.Nodes.Find(Strings.LCase(Regex.Replace(Cfg_Path_RecentZT1, "^" & Regex.Escape(Cfg_Path_Root) & "\\", "")), True)
        If ObjNodeSet.Count = 1 Then
            FrmMain.TVExplorer.SelectedNode = ObjNodeSet(0)
        End If

    End Sub


    ''' <summary>
    ''' <para>
    '''     Updates info in main window.
    ''' </para>
    ''' <para>
    '''     Updates shown info such as animation speed, number of frames, current frame, ...
    ''' </para>
    ''' <para>
    '''     Enables/disables certain controls (for example, button to render background frame)
    ''' </para>
    ''' </summary>
    ''' <param name="StrReason"></param>
    Sub UpdateGUI(StrReason As String)

        ' Displays updated info.
        ' 20190816: note: before today, it relied on .indexOf(), which might return incorrect results if there are similar frames. Now intFrameIndex is added and required.

        MdlZTStudio.Trace("MdlZTStudioUI", "UpdateGUI", "Reason: " & StrReason & ". Non-background frames: " & (EditorGraphic.Frames.Count - EditorGraphic.HasBackgroundFrame) & " - background frame: " & EditorGraphic.HasBackgroundFrame.ToString())

        Dim IntFrameIndex As Integer = FrmMain.TbFrames.Value - 1


        With FrmMain

            .TstZT1_AnimSpeed.Text = EditorGraphic.AnimationSpeed

            ' == Graphic
            .TsbGraphic_ExtraFrame.Enabled = (EditorGraphic.Frames.Count > 1) ' Background frame can only be enabled if there's more than one frame
            .TsbGraphic_ExtraFrame.Checked = (EditorGraphic.HasBackgroundFrame = 1) ' Is background frame enabled for this graphic? Then toggle button.

            ' == Frame
            .TsbFrame_Delete.Enabled = (EditorGraphic.Frames.Count > 1)
            .TsbFrame_ExportPNG.Enabled = False

            '(IsNothing(editorGraphic.frames(0).cachedFrame) = False)

            If IsNothing(EditorFrame) = False Then
                If EditorFrame.CoreImageHex.Count > 0 Then
                    .TsbFrame_ExportPNG.Enabled = True
                End If
            End If

            .TsbFrame_ImportPNG.Enabled = (EditorGraphic.Frames.Count > 0)

            .TsbFrame_OffsetDown.Enabled = (EditorGraphic.Frames.Count > 0)
            .TsbFrame_OffsetUp.Enabled = (EditorGraphic.Frames.Count > 0)
            .TsbFrame_OffsetLeft.Enabled = (EditorGraphic.Frames.Count > 0)
            .TsbFrame_OffsetRight.Enabled = (EditorGraphic.Frames.Count > 0)

            .TsbFrame_IndexIncrease.Enabled = (EditorGraphic.Frames.Count > 1 And IntFrameIndex < (EditorGraphic.Frames.Count - 1 - EditorGraphic.HasBackgroundFrame))
            .TsbFrame_IndexDecrease.Enabled = (EditorGraphic.Frames.Count > 1 And IntFrameIndex > 0)

            .PicBox.BackColor = Cfg_grid_BackGroundColor


        End With


105:

        ' Add time indication
        FrmMain.LblAnimTime.Text = ((EditorGraphic.Frames.Count - EditorGraphic.HasBackgroundFrame) * EditorGraphic.AnimationSpeed) & " ms "
        FrmMain.LblFrames.Text = (EditorGraphic.Frames.Count - EditorGraphic.HasBackgroundFrame) & " frames. "

205:
        If EditorGraphic.FileName <> vbNullString Then

            ' Get path
            Dim ObjFileInfo As New System.IO.FileInfo(EditorGraphic.FileName)
            Dim StrDirectoryName As String = ObjFileInfo.Directory.FullName
            MdlZTStudio.Trace("MdlZTStudioUI", "UpdateInfo", "Path of graphic is " & StrDirectoryName)

        End If


    End Sub

    ''' <summary>
    ''' Updates explorer pane
    ''' </summary>
    Sub UpdateExplorerPane()

        MdlZTStudio.Trace("MdlZTStudio", "UpdateExplorerPane", "Updating Explorer pane")

        Dim TVExplorer As TreeView = FrmMain.TVExplorer
        Dim StackDirectories As New Stack(Of String)

        StackDirectories.Push(Cfg_path_Root)

        Dim ObjImageList = New ImageList
        Dim ObjNodeCollection As TreeNodeCollection = TVExplorer.Nodes
        ObjImageList.Images.Add(My.Resources.icon_ZT1_Graphic)
        ObjImageList.Images.Add(My.Resources.icon_folder)
        ObjImageList.Images.Add(My.Resources.icon_file)
        ObjImageList.Images.Add(My.Resources.icon_ZT1_palette)
        TVExplorer.ImageList = ObjImageList


        TVExplorer.BeginUpdate()
        TVExplorer.Nodes.Clear()


        ' Continue processing for each stacked directory
        Do While (StackDirectories.Count > 0)

            ' Get top directory string
            Dim ObjNode As New TreeNode
            Dim StrDirectoryName As String = StackDirectories.Pop()


            Debug.Print(StrDirectoryName)


            If StrDirectoryName <> Cfg_path_Root Then

                ObjNode.Name = Regex.Replace(StrDirectoryName, "^" & Regex.Escape(Cfg_path_Root) & "\\", "")
                ObjNode.Text = Regex.Match(ObjNode.Name, "(?=[^\\]*$).*$").Value
                ObjNode.ImageIndex = 1
                ObjNode.SelectedImageIndex = 1

                ' Parent node?
                Dim StrParentDirectory = Regex.Replace(ObjNode.Name, "\\(?=[^\\]*$).*$", "")
                Dim ObjParentNode() As TreeNode = ObjNodeCollection.Find(StrParentDirectory, True)

                If ObjParentNode.Count = 1 Then
                    ObjParentNode(0).Nodes.Add(ObjNode)
                Else
                    ObjNodeCollection.Add(ObjNode)
                End If

            End If

            ' Loop through all subdirectories and add them to the stack.
            Dim StrSubDirectoryName As String
            For Each StrSubDirectoryName In Directory.GetDirectories(StrDirectoryName).Reverse()

                ' Subdirectories will be processed later. But as for current dir...
                StackDirectories.Push(StrSubDirectoryName)
            Next

            ' Loop through all files and add them to the node
            Dim StrSubFileName As String
            For Each StrSubFileName In Directory.GetFiles(StrDirectoryName)
                Dim ObjFileNode As New TreeNode
                ObjFileNode.Name = Regex.Replace(StrSubFileName, "^" & Regex.Escape(Cfg_path_Root) & "\\", "")
                ObjFileNode.Text = Regex.Match(ObjFileNode.Name, "(?=[^\\]*$).*$").Value

                ' Guess if it's a graphic or not
                If Regex.IsMatch(ObjFileNode.Text, "^[0-9A-z]{1,}$", RegexOptions.Singleline) Then
                    ObjFileNode.ImageIndex = 0
                    ObjFileNode.SelectedImageIndex = 0
                ElseIf Regex.IsMatch(ObjFileNode.Text, "^.*\.pal$", RegexOptions.Singleline) Then
                    ObjFileNode.ImageIndex = 3
                    ObjFileNode.SelectedImageIndex = 3
                Else
                    ObjFileNode.ImageIndex = 2
                    ObjFileNode.SelectedImageIndex = 2
                End If

                ObjNode.Nodes.Add(ObjFileNode)
            Next


            ' Make sure everything is finished. Needed?
            Application.DoEvents()

        Loop


        TVExplorer.EndUpdate()

    End Sub

    ''' <summary>
    ''' Updates shown info such as number of frames, current frame, ...
    ''' </summary>
    ''' <param name="StrReason"></param>
    Sub UpdateFrameInfo(StrReason As String)

        MdlZTStudio.Trace("MdlZTStudioUI", "UpdateFrameInfo", "Reason: " & StrReason & ". Non-background frames: " & (EditorGraphic.Frames.Count - EditorGraphic.HasBackgroundFrame) & " - background frame: " & EditorGraphic.HasBackgroundFrame.ToString())

        Dim IntFrameIndex As Integer = FrmMain.TbFrames.Value - 1

        With FrmMain

            ' NOT using a 0-based frame index visual indication, to avoid confusing
            .TslFrame_Index.Text = IIf(EditorGraphic.Frames.Count = 0, "-", (IntFrameIndex + 1) & " / " & (EditorGraphic.Frames.Count - EditorGraphic.HasBackgroundFrame))

            With .TbFrames
                .Minimum = 1
                .Maximum = (EditorGraphic.Frames.Count - EditorGraphic.HasBackgroundFrame) ' for actually generated files: - editorGraphic.extraFrame)

                If .Maximum < 1 Then
                    .Minimum = 1
                    .Maximum = 1
                End If
                If .Value < .Minimum Then
                    .Value = .Minimum
                End If

            End With

            If IsNothing(EditorFrame) Then
                .TstOffsetX.Text = "0"
                .TstOffsetY.Text = "0"

            Else
                '.tbFrames.Value = editorGraphic.frames.IndexOf(editorFrame) + 1
                .TstOffsetX.Text = EditorFrame.OffsetX
                .TstOffsetY.Text = EditorFrame.OffsetY
            End If

        End With


    End Sub

    ''' <summary>
    ''' Updates all sort of info.
    ''' </summary>
    ''' <param name="BlnUpdateFrameInfo">Boolean. Update frame info.</param>
    ''' <param name="BlnUpdateUI">Boolean. Update UI (buttons), animation speed, file list...).</param>
    ''' <param name="IntIndexFrameNumber">Optional frame index number. Defaults to value of slider in main window.</param>
    Public Sub UpdatePreview(BlnUpdateFrameInfo As Boolean, BlnUpdateUI As Boolean, Optional IntIndexFrameNumber As Integer = -1)



10:
        ' Can't update if there are no frames.
        If EditorGraphic.Frames.Count = 0 Then
            ' Add time indication
            FrmMain.LblAnimTime.Text = "0 ms"
            FrmMain.LblFrames.Text = "0 frames."
            Exit Sub
        End If

20:
        ' Shortcut. If no index number for the frame was specified, assume the currently visible frame needs to be updated.
        If IntIndexFrameNumber = -1 Then
            IntIndexFrameNumber = FrmMain.TbFrames.Value - 1
        End If


125:
        ' 20190816: some aspects weren't managed properly, for instance when toggling extra frame or adding/removing frames.
        ' Previous/next frame; current And max value of progress bar, ...
        ' Update preview is called from lots of places, so this may be a bit of an overkill, but better safe.
        If BlnUpdateFrameInfo = True Then
            MdlZTStudioUI.UpdateFrameInfo("MdlZTStudioUI_UpdatePreview()")
        End If

126:
        If BlnUpdateUI = True Then
            MdlZTStudioUI.UpdateGUI("MdlZTStudioUI_UpdatePreview()")
        End If


130:
        EditorFrame = EditorGraphic.Frames(IntIndexFrameNumber)

300:
        ' The sub gets triggered when a new frame has been added, but no .PNG has been loaded yet, so frame contains no data.
        ' However, the picbox may need to be cleared (previous frame would still be shown otherwise)
        If EditorGraphic.Frames(IntIndexFrameNumber).CoreImageHex.Count = 0 Then

310:
            FrmMain.PicBox.Image = MdlBitMap.DrawGridFootPrintXY(Cfg_grid_footPrintX, Cfg_grid_footPrintY).Bitmap

        Else

320:
            FrmMain.PicBox.Image = EditorGraphic.Frames(IntIndexFrameNumber).GetImage(True).Bitmap

        End If


    End Sub


    ''' <summary>
    ''' Shows tooltip when hovering above element
    ''' </summary>
    ''' <param name="ObjControl">Control which owns this tooltip</param>
    ''' <param name="StrMessage">Message</param>
    Sub ShowToolTip(ObjControl As Control, StrMessage As String)

        FrmSettings.LblHelpTopic.Text = ObjControl.Text
        FrmSettings.LblHelp.Text = StrMessage

    End Sub

End Module
