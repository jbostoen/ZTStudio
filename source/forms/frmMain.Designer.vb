<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.picBox = New System.Windows.Forms.PictureBox()
        Me.tmrAnimation = New System.Windows.Forms.Timer(Me.components)
        Me.dlgColor = New System.Windows.Forms.ColorDialog()
        Me.tsZT1Graphic = New System.Windows.Forms.ToolStrip()
        Me.tslZT1Graphic = New System.Windows.Forms.ToolStripLabel()
        Me.tsbZT1Open = New System.Windows.Forms.ToolStripButton()
        Me.tsbZT1Write = New System.Windows.Forms.ToolStripButton()
        Me.tsbZT1_OpenPal = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tslZT1_AnimSpeed = New System.Windows.Forms.ToolStripLabel()
        Me.tstZT1_AnimSpeed = New System.Windows.Forms.ToolStripTextBox()
        Me.tsbGraphic_ExtraFrame = New System.Windows.Forms.ToolStripButton()
        Me.dlgOpen = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblColorDetails = New System.Windows.Forms.Label()
        Me.lblFrames = New System.Windows.Forms.Label()
        Me.lblAnimTime = New System.Windows.Forms.Label()
        Me.lblColor = New System.Windows.Forms.Label()
        Me.ssBar = New System.Windows.Forms.StatusStrip()
        Me.ssFileName = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ssColor = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblAnimSpeed = New System.Windows.Forms.Label()
        Me.chkPlayAnimation = New System.Windows.Forms.CheckBox()
        Me.tbFrames = New System.Windows.Forms.TrackBar()
        Me.dgvPaletteMain = New System.Windows.Forms.DataGridView()
        Me.colColor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dlgSave = New System.Windows.Forms.SaveFileDialog()
        Me.tsFrame = New System.Windows.Forms.ToolStrip()
        Me.tslFrame = New System.Windows.Forms.ToolStripLabel()
        Me.tsbFrame_ImportPNG = New System.Windows.Forms.ToolStripButton()
        Me.tsbFrame_ExportPNG = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbFrame_Add = New System.Windows.Forms.ToolStripButton()
        Me.tsbFrame_Delete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbFrame_OffsetUp = New System.Windows.Forms.ToolStripButton()
        Me.tsbFrame_OffsetDown = New System.Windows.Forms.ToolStripButton()
        Me.tsbFrame_OffsetLeft = New System.Windows.Forms.ToolStripButton()
        Me.tsbFrame_OffsetRight = New System.Windows.Forms.ToolStripButton()
        Me.tslFrame_Offset = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tslFrame_FP = New System.Windows.Forms.ToolStripLabel()
        Me.tsbFrame_fpX = New System.Windows.Forms.ToolStripComboBox()
        Me.tsbFrame_fpY = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbFrame_IndexDecrease = New System.Windows.Forms.ToolStripButton()
        Me.tsbFrame_IndexIncrease = New System.Windows.Forms.ToolStripButton()
        Me.tslFrame_Index = New System.Windows.Forms.ToolStripLabel()
        Me.tsTools = New System.Windows.Forms.ToolStrip()
        Me.tsMisc = New System.Windows.Forms.ToolStripLabel()
        Me.tsbOpenPalBldg8 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsbOpenPalBldg16 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbGridBG = New System.Windows.Forms.ToolStripButton()
        Me.tsbPreview_BGGraphic = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbBatchConversion = New System.Windows.Forms.ToolStripButton()
        Me.tsbBatchRotFix = New System.Windows.Forms.ToolStripButton()
        Me.tsbDelete_ZT1Files = New System.Windows.Forms.ToolStripButton()
        Me.tsbDelete_PNG = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbSettings = New System.Windows.Forms.ToolStripButton()
        Me.tsbAbout = New System.Windows.Forms.ToolStripButton()
        Me.mnuPal = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPal_MoveEnd = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_MoveUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_MoveDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_Replace = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_Add = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_SavePAL = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_ExportPNG = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_ImportPNG = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_ImportGimpPalette = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.picBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tsZT1Graphic.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ssBar.SuspendLayout()
        CType(Me.tbFrames, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvPaletteMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tsFrame.SuspendLayout()
        Me.tsTools.SuspendLayout()
        Me.mnuPal.SuspendLayout()
        Me.SuspendLayout()
        '
        'picBox
        '
        Me.picBox.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.picBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.picBox.Location = New System.Drawing.Point(0, 117)
        Me.picBox.Name = "picBox"
        Me.picBox.Size = New System.Drawing.Size(808, 512)
        Me.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picBox.TabIndex = 6
        Me.picBox.TabStop = False
        '
        'tmrAnimation
        '
        '
        'dlgColor
        '
        Me.dlgColor.AnyColor = True
        Me.dlgColor.SolidColorOnly = True
        '
        'tsZT1Graphic
        '
        Me.tsZT1Graphic.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslZT1Graphic, Me.tsbZT1Open, Me.tsbZT1Write, Me.tsbZT1_OpenPal, Me.ToolStripSeparator6, Me.tslZT1_AnimSpeed, Me.tstZT1_AnimSpeed, Me.tsbGraphic_ExtraFrame})
        Me.tsZT1Graphic.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.tsZT1Graphic.Location = New System.Drawing.Point(0, 0)
        Me.tsZT1Graphic.Name = "tsZT1Graphic"
        Me.tsZT1Graphic.Size = New System.Drawing.Size(1008, 39)
        Me.tsZT1Graphic.TabIndex = 12
        Me.tsZT1Graphic.Text = "ToolStrip1"
        '
        'tslZT1Graphic
        '
        Me.tslZT1Graphic.AutoSize = False
        Me.tslZT1Graphic.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslZT1Graphic.Name = "tslZT1Graphic"
        Me.tslZT1Graphic.Size = New System.Drawing.Size(100, 22)
        Me.tslZT1Graphic.Text = "ZT1 Graphic"
        Me.tslZT1Graphic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsbZT1Open
        '
        Me.tsbZT1Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbZT1Open.Image = CType(resources.GetObject("tsbZT1Open.Image"), System.Drawing.Image)
        Me.tsbZT1Open.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbZT1Open.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbZT1Open.Name = "tsbZT1Open"
        Me.tsbZT1Open.Size = New System.Drawing.Size(36, 36)
        Me.tsbZT1Open.Text = "Open graphic"
        Me.tsbZT1Open.ToolTipText = "Open a ZT1 Graphics File"
        '
        'tsbZT1Write
        '
        Me.tsbZT1Write.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbZT1Write.Image = CType(resources.GetObject("tsbZT1Write.Image"), System.Drawing.Image)
        Me.tsbZT1Write.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbZT1Write.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbZT1Write.Name = "tsbZT1Write"
        Me.tsbZT1Write.Size = New System.Drawing.Size(36, 36)
        Me.tsbZT1Write.Text = "Save as ZT1 Graphic"
        '
        'tsbZT1_OpenPal
        '
        Me.tsbZT1_OpenPal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbZT1_OpenPal.Image = CType(resources.GetObject("tsbZT1_OpenPal.Image"), System.Drawing.Image)
        Me.tsbZT1_OpenPal.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbZT1_OpenPal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbZT1_OpenPal.Name = "tsbZT1_OpenPal"
        Me.tsbZT1_OpenPal.Size = New System.Drawing.Size(36, 36)
        Me.tsbZT1_OpenPal.Text = "Open a ZT1 Color Palette (.pal)"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 39)
        '
        'tslZT1_AnimSpeed
        '
        Me.tslZT1_AnimSpeed.Name = "tslZT1_AnimSpeed"
        Me.tslZT1_AnimSpeed.Size = New System.Drawing.Size(180, 36)
        Me.tslZT1_AnimSpeed.Text = "Animation speed (milli seconds):"
        '
        'tstZT1_AnimSpeed
        '
        Me.tstZT1_AnimSpeed.Name = "tstZT1_AnimSpeed"
        Me.tstZT1_AnimSpeed.Size = New System.Drawing.Size(50, 39)
        '
        'tsbGraphic_ExtraFrame
        '
        Me.tsbGraphic_ExtraFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbGraphic_ExtraFrame.Image = CType(resources.GetObject("tsbGraphic_ExtraFrame.Image"), System.Drawing.Image)
        Me.tsbGraphic_ExtraFrame.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbGraphic_ExtraFrame.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbGraphic_ExtraFrame.Name = "tsbGraphic_ExtraFrame"
        Me.tsbGraphic_ExtraFrame.Size = New System.Drawing.Size(36, 36)
        Me.tsbGraphic_ExtraFrame.Text = "Use last frame as background frame"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblColorDetails)
        Me.Panel1.Controls.Add(Me.lblFrames)
        Me.Panel1.Controls.Add(Me.lblAnimTime)
        Me.Panel1.Controls.Add(Me.lblColor)
        Me.Panel1.Controls.Add(Me.ssBar)
        Me.Panel1.Controls.Add(Me.lblAnimSpeed)
        Me.Panel1.Controls.Add(Me.chkPlayAnimation)
        Me.Panel1.Controls.Add(Me.tbFrames)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 629)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(808, 151)
        Me.Panel1.TabIndex = 19
        '
        'lblColorDetails
        '
        Me.lblColorDetails.Location = New System.Drawing.Point(108, 39)
        Me.lblColorDetails.Name = "lblColorDetails"
        Me.lblColorDetails.Size = New System.Drawing.Size(224, 47)
        Me.lblColorDetails.TabIndex = 34
        Me.lblColorDetails.Text = "Color details"
        Me.lblColorDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFrames
        '
        Me.lblFrames.AutoSize = True
        Me.lblFrames.Location = New System.Drawing.Point(540, 39)
        Me.lblFrames.Name = "lblFrames"
        Me.lblFrames.Size = New System.Drawing.Size(47, 13)
        Me.lblFrames.TabIndex = 31
        Me.lblFrames.Text = "0 frames"
        '
        'lblAnimTime
        '
        Me.lblAnimTime.AutoSize = True
        Me.lblAnimTime.Location = New System.Drawing.Point(540, 52)
        Me.lblAnimTime.Name = "lblAnimTime"
        Me.lblAnimTime.Size = New System.Drawing.Size(29, 13)
        Me.lblAnimTime.TabIndex = 29
        Me.lblAnimTime.Text = "0 ms"
        '
        'lblColor
        '
        Me.lblColor.Location = New System.Drawing.Point(21, 40)
        Me.lblColor.Name = "lblColor"
        Me.lblColor.Size = New System.Drawing.Size(60, 46)
        Me.lblColor.TabIndex = 28
        Me.lblColor.Text = "Move over a color."
        Me.lblColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ssBar
        '
        Me.ssBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ssFileName, Me.ssColor})
        Me.ssBar.Location = New System.Drawing.Point(0, 129)
        Me.ssBar.Name = "ssBar"
        Me.ssBar.Size = New System.Drawing.Size(808, 22)
        Me.ssBar.TabIndex = 27
        Me.ssBar.Text = "StatusStrip1"
        '
        'ssFileName
        '
        Me.ssFileName.Name = "ssFileName"
        Me.ssFileName.Size = New System.Drawing.Size(88, 17)
        Me.ssFileName.Text = "No file opened."
        '
        'ssColor
        '
        Me.ssColor.Name = "ssColor"
        Me.ssColor.Size = New System.Drawing.Size(0, 17)
        '
        'lblAnimSpeed
        '
        Me.lblAnimSpeed.AutoSize = True
        Me.lblAnimSpeed.Location = New System.Drawing.Point(540, 26)
        Me.lblAnimSpeed.Name = "lblAnimSpeed"
        Me.lblAnimSpeed.Size = New System.Drawing.Size(85, 13)
        Me.lblAnimSpeed.TabIndex = 24
        Me.lblAnimSpeed.Text = "Animation speed"
        '
        'chkPlayAnimation
        '
        Me.chkPlayAnimation.AutoSize = True
        Me.chkPlayAnimation.Location = New System.Drawing.Point(543, 6)
        Me.chkPlayAnimation.Name = "chkPlayAnimation"
        Me.chkPlayAnimation.Size = New System.Drawing.Size(94, 17)
        Me.chkPlayAnimation.TabIndex = 20
        Me.chkPlayAnimation.Text = "Play animation"
        Me.chkPlayAnimation.UseVisualStyleBackColor = True
        '
        'tbFrames
        '
        Me.tbFrames.Location = New System.Drawing.Point(24, 3)
        Me.tbFrames.Maximum = 1
        Me.tbFrames.Minimum = 1
        Me.tbFrames.Name = "tbFrames"
        Me.tbFrames.Size = New System.Drawing.Size(488, 45)
        Me.tbFrames.TabIndex = 19
        Me.tbFrames.Value = 1
        '
        'dgvPaletteMain
        '
        Me.dgvPaletteMain.AllowUserToAddRows = False
        Me.dgvPaletteMain.AllowUserToDeleteRows = False
        Me.dgvPaletteMain.AllowUserToResizeColumns = False
        Me.dgvPaletteMain.AllowUserToResizeRows = False
        Me.dgvPaletteMain.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells
        Me.dgvPaletteMain.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colColor})
        Me.dgvPaletteMain.Dock = System.Windows.Forms.DockStyle.Right
        Me.dgvPaletteMain.Location = New System.Drawing.Point(808, 117)
        Me.dgvPaletteMain.Name = "dgvPaletteMain"
        Me.dgvPaletteMain.RowHeadersWidth = 75
        Me.dgvPaletteMain.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvPaletteMain.RowTemplate.Height = 20
        Me.dgvPaletteMain.Size = New System.Drawing.Size(200, 663)
        Me.dgvPaletteMain.TabIndex = 20
        '
        'colColor
        '
        Me.colColor.HeaderText = "Color"
        Me.colColor.Name = "colColor"
        '
        'tsFrame
        '
        Me.tsFrame.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslFrame, Me.tsbFrame_ImportPNG, Me.tsbFrame_ExportPNG, Me.ToolStripSeparator7, Me.tsbFrame_Add, Me.tsbFrame_Delete, Me.ToolStripSeparator4, Me.tsbFrame_OffsetUp, Me.tsbFrame_OffsetDown, Me.tsbFrame_OffsetLeft, Me.tsbFrame_OffsetRight, Me.tslFrame_Offset, Me.ToolStripSeparator5, Me.tslFrame_FP, Me.tsbFrame_fpX, Me.tsbFrame_fpY, Me.ToolStripSeparator8, Me.tsbFrame_IndexDecrease, Me.tsbFrame_IndexIncrease, Me.tslFrame_Index})
        Me.tsFrame.Location = New System.Drawing.Point(0, 39)
        Me.tsFrame.Name = "tsFrame"
        Me.tsFrame.Size = New System.Drawing.Size(1008, 39)
        Me.tsFrame.TabIndex = 21
        Me.tsFrame.Text = "ToolStrip1"
        '
        'tslFrame
        '
        Me.tslFrame.AutoSize = False
        Me.tslFrame.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.tslFrame.Name = "tslFrame"
        Me.tslFrame.Size = New System.Drawing.Size(100, 22)
        Me.tslFrame.Text = "Frame"
        Me.tslFrame.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsbFrame_ImportPNG
        '
        Me.tsbFrame_ImportPNG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_ImportPNG.Image = CType(resources.GetObject("tsbFrame_ImportPNG.Image"), System.Drawing.Image)
        Me.tsbFrame_ImportPNG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_ImportPNG.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_ImportPNG.Name = "tsbFrame_ImportPNG"
        Me.tsbFrame_ImportPNG.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_ImportPNG.Text = "Open graphic to use in this frame"
        '
        'tsbFrame_ExportPNG
        '
        Me.tsbFrame_ExportPNG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_ExportPNG.Image = CType(resources.GetObject("tsbFrame_ExportPNG.Image"), System.Drawing.Image)
        Me.tsbFrame_ExportPNG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_ExportPNG.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_ExportPNG.Name = "tsbFrame_ExportPNG"
        Me.tsbFrame_ExportPNG.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_ExportPNG.Text = "Save frame as PNG"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 39)
        '
        'tsbFrame_Add
        '
        Me.tsbFrame_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_Add.Image = CType(resources.GetObject("tsbFrame_Add.Image"), System.Drawing.Image)
        Me.tsbFrame_Add.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_Add.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_Add.Name = "tsbFrame_Add"
        Me.tsbFrame_Add.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_Add.Text = "Add frame"
        '
        'tsbFrame_Delete
        '
        Me.tsbFrame_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_Delete.Image = CType(resources.GetObject("tsbFrame_Delete.Image"), System.Drawing.Image)
        Me.tsbFrame_Delete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_Delete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_Delete.Name = "tsbFrame_Delete"
        Me.tsbFrame_Delete.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_Delete.Text = "Delete frame"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 39)
        '
        'tsbFrame_OffsetUp
        '
        Me.tsbFrame_OffsetUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_OffsetUp.Image = CType(resources.GetObject("tsbFrame_OffsetUp.Image"), System.Drawing.Image)
        Me.tsbFrame_OffsetUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_OffsetUp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_OffsetUp.Name = "tsbFrame_OffsetUp"
        Me.tsbFrame_OffsetUp.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_OffsetUp.Text = "Move up"
        '
        'tsbFrame_OffsetDown
        '
        Me.tsbFrame_OffsetDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_OffsetDown.Image = CType(resources.GetObject("tsbFrame_OffsetDown.Image"), System.Drawing.Image)
        Me.tsbFrame_OffsetDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_OffsetDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_OffsetDown.Name = "tsbFrame_OffsetDown"
        Me.tsbFrame_OffsetDown.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_OffsetDown.Text = "Move down"
        '
        'tsbFrame_OffsetLeft
        '
        Me.tsbFrame_OffsetLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_OffsetLeft.Image = CType(resources.GetObject("tsbFrame_OffsetLeft.Image"), System.Drawing.Image)
        Me.tsbFrame_OffsetLeft.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_OffsetLeft.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_OffsetLeft.Name = "tsbFrame_OffsetLeft"
        Me.tsbFrame_OffsetLeft.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_OffsetLeft.Text = "Move left"
        '
        'tsbFrame_OffsetRight
        '
        Me.tsbFrame_OffsetRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_OffsetRight.Image = CType(resources.GetObject("tsbFrame_OffsetRight.Image"), System.Drawing.Image)
        Me.tsbFrame_OffsetRight.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_OffsetRight.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_OffsetRight.Name = "tsbFrame_OffsetRight"
        Me.tsbFrame_OffsetRight.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_OffsetRight.Text = "Move right"
        '
        'tslFrame_Offset
        '
        Me.tslFrame_Offset.Name = "tslFrame_Offset"
        Me.tslFrame_Offset.Size = New System.Drawing.Size(66, 36)
        Me.tslFrame_Offset.Text = "Offset: 0 , 0"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 39)
        '
        'tslFrame_FP
        '
        Me.tslFrame_FP.Name = "tslFrame_FP"
        Me.tslFrame_FP.Size = New System.Drawing.Size(79, 36)
        Me.tslFrame_FP.Text = "Footprint X,Y:"
        '
        'tsbFrame_fpX
        '
        Me.tsbFrame_fpX.AutoSize = False
        Me.tsbFrame_fpX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsbFrame_fpX.Items.AddRange(New Object() {"2", "4", "6", "8", "10", "12", "14", "16", "18"})
        Me.tsbFrame_fpX.Name = "tsbFrame_fpX"
        Me.tsbFrame_fpX.Size = New System.Drawing.Size(50, 23)
        '
        'tsbFrame_fpY
        '
        Me.tsbFrame_fpY.AutoSize = False
        Me.tsbFrame_fpY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsbFrame_fpY.Items.AddRange(New Object() {"2", "4", "6", "8", "10", "12", "14", "16", "18"})
        Me.tsbFrame_fpY.Name = "tsbFrame_fpY"
        Me.tsbFrame_fpY.Size = New System.Drawing.Size(50, 23)
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 39)
        '
        'tsbFrame_IndexDecrease
        '
        Me.tsbFrame_IndexDecrease.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_IndexDecrease.Image = CType(resources.GetObject("tsbFrame_IndexDecrease.Image"), System.Drawing.Image)
        Me.tsbFrame_IndexDecrease.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_IndexDecrease.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_IndexDecrease.Name = "tsbFrame_IndexDecrease"
        Me.tsbFrame_IndexDecrease.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_IndexDecrease.Text = "Show frame earlier in animation"
        '
        'tsbFrame_IndexIncrease
        '
        Me.tsbFrame_IndexIncrease.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFrame_IndexIncrease.Image = CType(resources.GetObject("tsbFrame_IndexIncrease.Image"), System.Drawing.Image)
        Me.tsbFrame_IndexIncrease.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFrame_IndexIncrease.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFrame_IndexIncrease.Name = "tsbFrame_IndexIncrease"
        Me.tsbFrame_IndexIncrease.Size = New System.Drawing.Size(36, 36)
        Me.tsbFrame_IndexIncrease.Text = "Show frame later in animation"
        '
        'tslFrame_Index
        '
        Me.tslFrame_Index.Name = "tslFrame_Index"
        Me.tslFrame_Index.Size = New System.Drawing.Size(64, 36)
        Me.tslFrame_Index.Text = "Index: 0 / 0"
        '
        'tsTools
        '
        Me.tsTools.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsMisc, Me.tsbOpenPalBldg8, Me.tsbOpenPalBldg16, Me.ToolStripSeparator2, Me.tsbGridBG, Me.tsbPreview_BGGraphic, Me.ToolStripSeparator3, Me.tsbBatchConversion, Me.tsbBatchRotFix, Me.tsbDelete_ZT1Files, Me.tsbDelete_PNG, Me.ToolStripSeparator1, Me.tsbSettings, Me.tsbAbout})
        Me.tsTools.Location = New System.Drawing.Point(0, 78)
        Me.tsTools.Name = "tsTools"
        Me.tsTools.Size = New System.Drawing.Size(1008, 39)
        Me.tsTools.TabIndex = 22
        Me.tsTools.Text = "Misc"
        '
        'tsMisc
        '
        Me.tsMisc.AutoSize = False
        Me.tsMisc.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.tsMisc.Name = "tsMisc"
        Me.tsMisc.Size = New System.Drawing.Size(100, 22)
        Me.tsMisc.Text = "Misc."
        Me.tsMisc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsbOpenPalBldg8
        '
        Me.tsbOpenPalBldg8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbOpenPalBldg8.Image = CType(resources.GetObject("tsbOpenPalBldg8.Image"), System.Drawing.Image)
        Me.tsbOpenPalBldg8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbOpenPalBldg8.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbOpenPalBldg8.Name = "tsbOpenPalBldg8"
        Me.tsbOpenPalBldg8.Size = New System.Drawing.Size(45, 36)
        Me.tsbOpenPalBldg8.Text = "Quick access to 8-color palettes"
        '
        'tsbOpenPalBldg16
        '
        Me.tsbOpenPalBldg16.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbOpenPalBldg16.Image = CType(resources.GetObject("tsbOpenPalBldg16.Image"), System.Drawing.Image)
        Me.tsbOpenPalBldg16.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbOpenPalBldg16.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbOpenPalBldg16.Name = "tsbOpenPalBldg16"
        Me.tsbOpenPalBldg16.Size = New System.Drawing.Size(45, 36)
        Me.tsbOpenPalBldg16.Text = "Quick access to 16-color palettes"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 39)
        '
        'tsbGridBG
        '
        Me.tsbGridBG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbGridBG.Image = CType(resources.GetObject("tsbGridBG.Image"), System.Drawing.Image)
        Me.tsbGridBG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbGridBG.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbGridBG.Name = "tsbGridBG"
        Me.tsbGridBG.Size = New System.Drawing.Size(36, 36)
        Me.tsbGridBG.Text = "Change the canvas background"
        Me.tsbGridBG.ToolTipText = "Background color of the image preview"
        '
        'tsbPreview_BGGraphic
        '
        Me.tsbPreview_BGGraphic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPreview_BGGraphic.Image = CType(resources.GetObject("tsbPreview_BGGraphic.Image"), System.Drawing.Image)
        Me.tsbPreview_BGGraphic.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbPreview_BGGraphic.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPreview_BGGraphic.Name = "tsbPreview_BGGraphic"
        Me.tsbPreview_BGGraphic.Size = New System.Drawing.Size(36, 36)
        Me.tsbPreview_BGGraphic.Text = "Open ZT1 Graphic and use it as background"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 39)
        '
        'tsbBatchConversion
        '
        Me.tsbBatchConversion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBatchConversion.Image = CType(resources.GetObject("tsbBatchConversion.Image"), System.Drawing.Image)
        Me.tsbBatchConversion.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbBatchConversion.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBatchConversion.Name = "tsbBatchConversion"
        Me.tsbBatchConversion.Size = New System.Drawing.Size(36, 36)
        Me.tsbBatchConversion.Text = "Batch conversion: ZT1 Graphic <=> .PNG"
        '
        'tsbBatchRotFix
        '
        Me.tsbBatchRotFix.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBatchRotFix.Image = CType(resources.GetObject("tsbBatchRotFix.Image"), System.Drawing.Image)
        Me.tsbBatchRotFix.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbBatchRotFix.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBatchRotFix.Name = "tsbBatchRotFix"
        Me.tsbBatchRotFix.Size = New System.Drawing.Size(36, 36)
        Me.tsbBatchRotFix.Text = "Batch rotation fixing"
        '
        'tsbDelete_ZT1Files
        '
        Me.tsbDelete_ZT1Files.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelete_ZT1Files.Image = CType(resources.GetObject("tsbDelete_ZT1Files.Image"), System.Drawing.Image)
        Me.tsbDelete_ZT1Files.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbDelete_ZT1Files.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelete_ZT1Files.Name = "tsbDelete_ZT1Files"
        Me.tsbDelete_ZT1Files.Size = New System.Drawing.Size(36, 36)
        Me.tsbDelete_ZT1Files.Text = "Delete all ZT1 Graphics and color palettes in the root folder"
        '
        'tsbDelete_PNG
        '
        Me.tsbDelete_PNG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDelete_PNG.Image = CType(resources.GetObject("tsbDelete_PNG.Image"), System.Drawing.Image)
        Me.tsbDelete_PNG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbDelete_PNG.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelete_PNG.Name = "tsbDelete_PNG"
        Me.tsbDelete_PNG.Size = New System.Drawing.Size(36, 36)
        Me.tsbDelete_PNG.Text = "Delete all .PNG files in the root directory"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 39)
        '
        'tsbSettings
        '
        Me.tsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSettings.Image = CType(resources.GetObject("tsbSettings.Image"), System.Drawing.Image)
        Me.tsbSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSettings.Name = "tsbSettings"
        Me.tsbSettings.Size = New System.Drawing.Size(36, 36)
        Me.tsbSettings.Text = "Settings"
        '
        'tsbAbout
        '
        Me.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAbout.Image = CType(resources.GetObject("tsbAbout.Image"), System.Drawing.Image)
        Me.tsbAbout.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAbout.Name = "tsbAbout"
        Me.tsbAbout.Size = New System.Drawing.Size(36, 36)
        Me.tsbAbout.Text = "About"
        '
        'mnuPal
        '
        Me.mnuPal.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPal_MoveEnd, Me.mnuPal_MoveUp, Me.mnuPal_MoveDown, Me.mnuPal_Replace, Me.mnuPal_Add, Me.mnuPal_SavePAL, Me.mnuPal_ExportPNG, Me.mnuPal_ImportPNG, Me.mnuPal_ImportGimpPalette})
        Me.mnuPal.Name = "mnuPal"
        Me.mnuPal.Size = New System.Drawing.Size(245, 202)
        '
        'mnuPal_MoveEnd
        '
        Me.mnuPal_MoveEnd.Name = "mnuPal_MoveEnd"
        Me.mnuPal_MoveEnd.Size = New System.Drawing.Size(244, 22)
        Me.mnuPal_MoveEnd.Text = "Move to end"
        '
        'mnuPal_MoveUp
        '
        Me.mnuPal_MoveUp.Name = "mnuPal_MoveUp"
        Me.mnuPal_MoveUp.Size = New System.Drawing.Size(244, 22)
        Me.mnuPal_MoveUp.Text = "Move up"
        '
        'mnuPal_MoveDown
        '
        Me.mnuPal_MoveDown.Name = "mnuPal_MoveDown"
        Me.mnuPal_MoveDown.Size = New System.Drawing.Size(244, 22)
        Me.mnuPal_MoveDown.Text = "Move down"
        '
        'mnuPal_Replace
        '
        Me.mnuPal_Replace.Name = "mnuPal_Replace"
        Me.mnuPal_Replace.Size = New System.Drawing.Size(244, 22)
        Me.mnuPal_Replace.Text = "Replace color"
        '
        'mnuPal_Add
        '
        Me.mnuPal_Add.Name = "mnuPal_Add"
        Me.mnuPal_Add.Size = New System.Drawing.Size(244, 22)
        Me.mnuPal_Add.Text = "Add color entry"
        '
        'mnuPal_SavePAL
        '
        Me.mnuPal_SavePAL.Name = "mnuPal_SavePAL"
        Me.mnuPal_SavePAL.Size = New System.Drawing.Size(244, 22)
        Me.mnuPal_SavePAL.Text = "Save as .PAL"
        '
        'mnuPal_ExportPNG
        '
        Me.mnuPal_ExportPNG.Name = "mnuPal_ExportPNG"
        Me.mnuPal_ExportPNG.Size = New System.Drawing.Size(244, 22)
        Me.mnuPal_ExportPNG.Text = "Export to PNG palette"
        '
        'mnuPal_ImportPNG
        '
        Me.mnuPal_ImportPNG.Name = "mnuPal_ImportPNG"
        Me.mnuPal_ImportPNG.Size = New System.Drawing.Size(244, 22)
        Me.mnuPal_ImportPNG.Text = "Replace with PNG palette"
        '
        'mnuPal_ImportGimpPalette
        '
        Me.mnuPal_ImportGimpPalette.Name = "mnuPal_ImportGimpPalette"
        Me.mnuPal_ImportGimpPalette.Size = New System.Drawing.Size(244, 22)
        Me.mnuPal_ImportGimpPalette.Text = "Replace with GIMP Color palette"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 780)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.picBox)
        Me.Controls.Add(Me.dgvPaletteMain)
        Me.Controls.Add(Me.tsTools)
        Me.Controls.Add(Me.tsFrame)
        Me.Controls.Add(Me.tsZT1Graphic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "ZT Studio"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.picBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tsZT1Graphic.ResumeLayout(False)
        Me.tsZT1Graphic.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ssBar.ResumeLayout(False)
        Me.ssBar.PerformLayout()
        CType(Me.tbFrames, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvPaletteMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tsFrame.ResumeLayout(False)
        Me.tsFrame.PerformLayout()
        Me.tsTools.ResumeLayout(False)
        Me.tsTools.PerformLayout()
        Me.mnuPal.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picBox As System.Windows.Forms.PictureBox
    Friend WithEvents tmrAnimation As System.Windows.Forms.Timer
    Friend WithEvents dlgColor As System.Windows.Forms.ColorDialog
    Friend WithEvents tsZT1Graphic As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbZT1Open As System.Windows.Forms.ToolStripButton
    Friend WithEvents dlgOpen As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblAnimSpeed As System.Windows.Forms.Label
    Friend WithEvents chkPlayAnimation As System.Windows.Forms.CheckBox
    Friend WithEvents tbFrames As System.Windows.Forms.TrackBar
    Friend WithEvents dgvPaletteMain As System.Windows.Forms.DataGridView
    Friend WithEvents colColor As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ssBar As System.Windows.Forms.StatusStrip
    Friend WithEvents ssFileName As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents dlgSave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ssColor As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblColor As System.Windows.Forms.Label
    Friend WithEvents tsbZT1_OpenPal As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblAnimTime As System.Windows.Forms.Label
    Friend WithEvents lblFrames As System.Windows.Forms.Label
    Friend WithEvents tsbZT1Write As System.Windows.Forms.ToolStripButton
    Friend WithEvents tslZT1Graphic As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsFrame As System.Windows.Forms.ToolStrip
    Friend WithEvents tslFrame As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbFrame_ExportPNG As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsTools As System.Windows.Forms.ToolStrip
    Friend WithEvents tsMisc As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbFrame_ImportPNG As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbFrame_OffsetUp As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFrame_OffsetDown As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFrame_OffsetLeft As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFrame_OffsetRight As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbOpenPalBldg8 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsbOpenPalBldg16 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbGridBG As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbBatchConversion As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbSettings As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbAbout As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tslZT1_AnimSpeed As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstZT1_AnimSpeed As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslFrame_Offset As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbFrame_IndexDecrease As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFrame_IndexIncrease As System.Windows.Forms.ToolStripButton
    Friend WithEvents tslFrame_Index As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbPreview_BGGraphic As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbFrame_Add As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFrame_Delete As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbGraphic_ExtraFrame As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbDelete_PNG As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbDelete_ZT1Files As System.Windows.Forms.ToolStripButton
    Friend WithEvents tslFrame_FP As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbFrame_fpX As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tsbFrame_fpY As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPal As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuPal_MoveEnd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_MoveUp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_MoveDown As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_Replace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_Add As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblColorDetails As System.Windows.Forms.Label
    Friend WithEvents mnuPal_ImportPNG As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_ExportPNG As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_SavePAL As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_ImportGimpPalette As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbBatchRotFix As System.Windows.Forms.ToolStripButton

End Class
