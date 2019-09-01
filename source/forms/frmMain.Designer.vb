<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.PicBox = New System.Windows.Forms.PictureBox()
        Me.TmrAnimation = New System.Windows.Forms.Timer(Me.components)
        Me.DlgColor = New System.Windows.Forms.ColorDialog()
        Me.TsZT1Graphic = New System.Windows.Forms.ToolStrip()
        Me.TslZT1Graphic = New System.Windows.Forms.ToolStripLabel()
        Me.TsbZT1New = New System.Windows.Forms.ToolStripButton()
        Me.TsbZT1Open = New System.Windows.Forms.ToolStripButton()
        Me.TsbZT1Write = New System.Windows.Forms.ToolStripButton()
        Me.TsbZT1_OpenPal = New System.Windows.Forms.ToolStripButton()
        Me.Tss_Graphic_1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsbGraphic_ExtraFrame = New System.Windows.Forms.ToolStripButton()
        Me.TslZT1_AnimSpeed = New System.Windows.Forms.ToolStripLabel()
        Me.TstZT1_AnimSpeed = New System.Windows.Forms.ToolStripTextBox()
        Me.DlgOpen = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GBAnimation = New System.Windows.Forms.GroupBox()
        Me.LblFrames = New System.Windows.Forms.Label()
        Me.LblAnimTime = New System.Windows.Forms.Label()
        Me.ChkPlayAnimation = New System.Windows.Forms.CheckBox()
        Me.TbFrames = New System.Windows.Forms.TrackBar()
        Me.GBOtherViews = New System.Windows.Forms.GroupBox()
        Me.TVExplorer = New System.Windows.Forms.TreeView()
        Me.GBColors = New System.Windows.Forms.GroupBox()
        Me.LblColorTool = New System.Windows.Forms.Label()
        Me.LblColorDetails = New System.Windows.Forms.Label()
        Me.LblColor = New System.Windows.Forms.Label()
        Me.SsBar = New System.Windows.Forms.StatusStrip()
        Me.ssFileName = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ssColor = New System.Windows.Forms.ToolStripStatusLabel()
        Me.DgvPaletteMain = New System.Windows.Forms.DataGridView()
        Me.ColColor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DlgSave = New System.Windows.Forms.SaveFileDialog()
        Me.TsFrame = New System.Windows.Forms.ToolStrip()
        Me.TslFrame = New System.Windows.Forms.ToolStripLabel()
        Me.TsbFrame_ImportPNG = New System.Windows.Forms.ToolStripButton()
        Me.TsbFrame_ExportPNG = New System.Windows.Forms.ToolStripButton()
        Me.Tss_Frame_1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TslFrame_Index = New System.Windows.Forms.ToolStripLabel()
        Me.TsbFrame_IndexDecrease = New System.Windows.Forms.ToolStripButton()
        Me.TsbFrame_IndexIncrease = New System.Windows.Forms.ToolStripButton()
        Me.Tss_Frame_2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsbFrame_Add = New System.Windows.Forms.ToolStripButton()
        Me.TsbFrame_Delete = New System.Windows.Forms.ToolStripButton()
        Me.TsbFrame_OffsetAll = New System.Windows.Forms.ToolStripButton()
        Me.TsbFrame_OffsetUp = New System.Windows.Forms.ToolStripButton()
        Me.TsbFrame_OffsetDown = New System.Windows.Forms.ToolStripButton()
        Me.TsbFrame_OffsetLeft = New System.Windows.Forms.ToolStripButton()
        Me.TsbFrame_OffsetRight = New System.Windows.Forms.ToolStripButton()
        Me.TslFrame_Offset = New System.Windows.Forms.ToolStripLabel()
        Me.TstOffsetX = New System.Windows.Forms.ToolStripTextBox()
        Me.TstOffsetY = New System.Windows.Forms.ToolStripTextBox()
        Me.Tss_Frame_4 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsTools = New System.Windows.Forms.ToolStrip()
        Me.TslMisc = New System.Windows.Forms.ToolStripLabel()
        Me.TsbOpenPalBldg8 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.TsbOpenPalBldg16 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.TssMisc_1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsbGridBG = New System.Windows.Forms.ToolStripButton()
        Me.TsbPreview_BGGraphic = New System.Windows.Forms.ToolStripButton()
        Me.TssMisc_2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsbBatchConversion = New System.Windows.Forms.ToolStripButton()
        Me.TsbBatchRotFix = New System.Windows.Forms.ToolStripButton()
        Me.TsbDelete_ZT1Files = New System.Windows.Forms.ToolStripButton()
        Me.TsbDelete_PNG = New System.Windows.Forms.ToolStripButton()
        Me.Tss_Misc_3 = New System.Windows.Forms.ToolStripSeparator()
        Me.TslFrame_FP = New System.Windows.Forms.ToolStripLabel()
        Me.TsbFrame_fpX = New System.Windows.Forms.ToolStripComboBox()
        Me.TsbFrame_fpY = New System.Windows.Forms.ToolStripComboBox()
        Me.TssMisc_4 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsbSettings = New System.Windows.Forms.ToolStripButton()
        Me.TsbAbout = New System.Windows.Forms.ToolStripButton()
        Me.MnuPal = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPal_MoveEnd = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_MoveUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_MoveDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_Replace = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_Add = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_SavePAL = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_ExportPNG = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_ImportPNG = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPal_ImportGimpPalette = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TsZT1Graphic.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GBAnimation.SuspendLayout()
        CType(Me.TbFrames, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GBOtherViews.SuspendLayout()
        Me.GBColors.SuspendLayout()
        Me.SsBar.SuspendLayout()
        CType(Me.DgvPaletteMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TsFrame.SuspendLayout()
        Me.TsTools.SuspendLayout()
        Me.MnuPal.SuspendLayout()
        Me.SuspendLayout()
        '
        'PicBox
        '
        Me.PicBox.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.PicBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.PicBox.Location = New System.Drawing.Point(0, 117)
        Me.PicBox.Name = "PicBox"
        Me.PicBox.Size = New System.Drawing.Size(808, 512)
        Me.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PicBox.TabIndex = 6
        Me.PicBox.TabStop = False
        '
        'TmrAnimation
        '
        '
        'DlgColor
        '
        Me.DlgColor.AnyColor = True
        Me.DlgColor.SolidColorOnly = True
        '
        'TsZT1Graphic
        '
        Me.TsZT1Graphic.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TslZT1Graphic, Me.TsbZT1New, Me.TsbZT1Open, Me.TsbZT1Write, Me.TsbZT1_OpenPal, Me.Tss_Graphic_1, Me.TsbGraphic_ExtraFrame, Me.TslZT1_AnimSpeed, Me.TstZT1_AnimSpeed})
        Me.TsZT1Graphic.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.TsZT1Graphic.Location = New System.Drawing.Point(0, 0)
        Me.TsZT1Graphic.Name = "TsZT1Graphic"
        Me.TsZT1Graphic.Size = New System.Drawing.Size(1008, 39)
        Me.TsZT1Graphic.TabIndex = 12
        Me.TsZT1Graphic.Text = "ToolStrip1"
        '
        'TslZT1Graphic
        '
        Me.TslZT1Graphic.AutoSize = False
        Me.TslZT1Graphic.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TslZT1Graphic.Name = "TslZT1Graphic"
        Me.TslZT1Graphic.Size = New System.Drawing.Size(100, 22)
        Me.TslZT1Graphic.Text = "ZT1 Graphic"
        Me.TslZT1Graphic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TsbZT1New
        '
        Me.TsbZT1New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbZT1New.Image = CType(resources.GetObject("TsbZT1New.Image"), System.Drawing.Image)
        Me.TsbZT1New.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbZT1New.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbZT1New.Name = "TsbZT1New"
        Me.TsbZT1New.Size = New System.Drawing.Size(36, 36)
        Me.TsbZT1New.Text = "New graphic"
        Me.TsbZT1New.ToolTipText = "Create a new ZT1 Graphics File"
        '
        'TsbZT1Open
        '
        Me.TsbZT1Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbZT1Open.Image = CType(resources.GetObject("TsbZT1Open.Image"), System.Drawing.Image)
        Me.TsbZT1Open.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbZT1Open.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbZT1Open.Name = "TsbZT1Open"
        Me.TsbZT1Open.Size = New System.Drawing.Size(36, 36)
        Me.TsbZT1Open.Text = "Open graphic"
        Me.TsbZT1Open.ToolTipText = "Open a ZT1 Graphics File"
        '
        'TsbZT1Write
        '
        Me.TsbZT1Write.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbZT1Write.Image = CType(resources.GetObject("TsbZT1Write.Image"), System.Drawing.Image)
        Me.TsbZT1Write.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbZT1Write.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbZT1Write.Name = "TsbZT1Write"
        Me.TsbZT1Write.Size = New System.Drawing.Size(36, 36)
        Me.TsbZT1Write.Text = "Save as ZT1 Graphic"
        '
        'TsbZT1_OpenPal
        '
        Me.TsbZT1_OpenPal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbZT1_OpenPal.Image = CType(resources.GetObject("TsbZT1_OpenPal.Image"), System.Drawing.Image)
        Me.TsbZT1_OpenPal.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbZT1_OpenPal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbZT1_OpenPal.Name = "TsbZT1_OpenPal"
        Me.TsbZT1_OpenPal.Size = New System.Drawing.Size(36, 36)
        Me.TsbZT1_OpenPal.Text = "Open a ZT1 Color Palette (.pal)"
        '
        'Tss_Graphic_1
        '
        Me.Tss_Graphic_1.Name = "Tss_Graphic_1"
        Me.Tss_Graphic_1.Size = New System.Drawing.Size(6, 39)
        '
        'TsbGraphic_ExtraFrame
        '
        Me.TsbGraphic_ExtraFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbGraphic_ExtraFrame.Image = CType(resources.GetObject("TsbGraphic_ExtraFrame.Image"), System.Drawing.Image)
        Me.TsbGraphic_ExtraFrame.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbGraphic_ExtraFrame.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbGraphic_ExtraFrame.Name = "TsbGraphic_ExtraFrame"
        Me.TsbGraphic_ExtraFrame.Size = New System.Drawing.Size(36, 36)
        Me.TsbGraphic_ExtraFrame.Text = "Use last frame as background frame"
        '
        'TslZT1_AnimSpeed
        '
        Me.TslZT1_AnimSpeed.Name = "TslZT1_AnimSpeed"
        Me.TslZT1_AnimSpeed.Size = New System.Drawing.Size(100, 36)
        Me.TslZT1_AnimSpeed.Text = "Animation speed:"
        '
        'TstZT1_AnimSpeed
        '
        Me.TstZT1_AnimSpeed.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TstZT1_AnimSpeed.Name = "TstZT1_AnimSpeed"
        Me.TstZT1_AnimSpeed.Size = New System.Drawing.Size(50, 39)
        Me.TstZT1_AnimSpeed.ToolTipText = "Animation speed in milliseconds. Press [Enter] to confirm new values."
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GBAnimation)
        Me.Panel1.Controls.Add(Me.GBOtherViews)
        Me.Panel1.Controls.Add(Me.GBColors)
        Me.Panel1.Controls.Add(Me.SsBar)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(0, 629)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(808, 151)
        Me.Panel1.TabIndex = 19
        '
        'GBAnimation
        '
        Me.GBAnimation.Controls.Add(Me.LblFrames)
        Me.GBAnimation.Controls.Add(Me.LblAnimTime)
        Me.GBAnimation.Controls.Add(Me.ChkPlayAnimation)
        Me.GBAnimation.Controls.Add(Me.TbFrames)
        Me.GBAnimation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GBAnimation.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GBAnimation.Location = New System.Drawing.Point(549, 0)
        Me.GBAnimation.Name = "GBAnimation"
        Me.GBAnimation.Size = New System.Drawing.Size(259, 129)
        Me.GBAnimation.TabIndex = 37
        Me.GBAnimation.TabStop = False
        Me.GBAnimation.Text = "Animation"
        '
        'LblFrames
        '
        Me.LblFrames.AutoSize = True
        Me.LblFrames.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFrames.Location = New System.Drawing.Point(15, 90)
        Me.LblFrames.Name = "LblFrames"
        Me.LblFrames.Size = New System.Drawing.Size(50, 13)
        Me.LblFrames.TabIndex = 35
        Me.LblFrames.Text = "0 frames"
        '
        'LblAnimTime
        '
        Me.LblAnimTime.AutoSize = True
        Me.LblAnimTime.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAnimTime.Location = New System.Drawing.Point(15, 103)
        Me.LblAnimTime.Name = "LblAnimTime"
        Me.LblAnimTime.Size = New System.Drawing.Size(30, 13)
        Me.LblAnimTime.TabIndex = 34
        Me.LblAnimTime.Text = "0 ms"
        '
        'ChkPlayAnimation
        '
        Me.ChkPlayAnimation.AutoSize = True
        Me.ChkPlayAnimation.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkPlayAnimation.Location = New System.Drawing.Point(18, 69)
        Me.ChkPlayAnimation.Name = "ChkPlayAnimation"
        Me.ChkPlayAnimation.Size = New System.Drawing.Size(101, 17)
        Me.ChkPlayAnimation.TabIndex = 32
        Me.ChkPlayAnimation.Text = "Play animation"
        Me.ChkPlayAnimation.UseVisualStyleBackColor = True
        '
        'TbFrames
        '
        Me.TbFrames.Dock = System.Windows.Forms.DockStyle.Top
        Me.TbFrames.Location = New System.Drawing.Point(3, 18)
        Me.TbFrames.Maximum = 1
        Me.TbFrames.Minimum = 1
        Me.TbFrames.Name = "TbFrames"
        Me.TbFrames.Size = New System.Drawing.Size(253, 45)
        Me.TbFrames.TabIndex = 36
        Me.TbFrames.Value = 1
        '
        'GBOtherViews
        '
        Me.GBOtherViews.Controls.Add(Me.TVExplorer)
        Me.GBOtherViews.Dock = System.Windows.Forms.DockStyle.Left
        Me.GBOtherViews.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GBOtherViews.Location = New System.Drawing.Point(240, 0)
        Me.GBOtherViews.Name = "GBOtherViews"
        Me.GBOtherViews.Size = New System.Drawing.Size(309, 129)
        Me.GBOtherViews.TabIndex = 38
        Me.GBOtherViews.TabStop = False
        Me.GBOtherViews.Text = "Explorer"
        '
        'TVExplorer
        '
        Me.TVExplorer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TVExplorer.Location = New System.Drawing.Point(3, 18)
        Me.TVExplorer.Name = "TVExplorer"
        Me.TVExplorer.Size = New System.Drawing.Size(303, 108)
        Me.TVExplorer.TabIndex = 0
        '
        'GBColors
        '
        Me.GBColors.Controls.Add(Me.LblColorTool)
        Me.GBColors.Controls.Add(Me.LblColorDetails)
        Me.GBColors.Controls.Add(Me.LblColor)
        Me.GBColors.Dock = System.Windows.Forms.DockStyle.Left
        Me.GBColors.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GBColors.Location = New System.Drawing.Point(0, 0)
        Me.GBColors.Name = "GBColors"
        Me.GBColors.Size = New System.Drawing.Size(240, 129)
        Me.GBColors.TabIndex = 36
        Me.GBColors.TabStop = False
        Me.GBColors.Text = "Color details"
        '
        'LblColorTool
        '
        Me.LblColorTool.AutoSize = True
        Me.LblColorTool.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblColorTool.Location = New System.Drawing.Point(18, 18)
        Me.LblColorTool.Name = "LblColorTool"
        Me.LblColorTool.Size = New System.Drawing.Size(101, 13)
        Me.LblColorTool.TabIndex = 37
        Me.LblColorTool.Text = "Move over a color."
        '
        'LblColorDetails
        '
        Me.LblColorDetails.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblColorDetails.Location = New System.Drawing.Point(80, 33)
        Me.LblColorDetails.Name = "LblColorDetails"
        Me.LblColorDetails.Size = New System.Drawing.Size(139, 70)
        Me.LblColorDetails.TabIndex = 36
        Me.LblColorDetails.Text = "Color details"
        Me.LblColorDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblColor
        '
        Me.LblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblColor.Location = New System.Drawing.Point(15, 36)
        Me.LblColor.Name = "LblColor"
        Me.LblColor.Size = New System.Drawing.Size(59, 43)
        Me.LblColor.TabIndex = 35
        Me.LblColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SsBar
        '
        Me.SsBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ssFileName, Me.ssColor})
        Me.SsBar.Location = New System.Drawing.Point(0, 129)
        Me.SsBar.Name = "SsBar"
        Me.SsBar.Size = New System.Drawing.Size(808, 22)
        Me.SsBar.TabIndex = 27
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
        'DgvPaletteMain
        '
        Me.DgvPaletteMain.AllowUserToAddRows = False
        Me.DgvPaletteMain.AllowUserToDeleteRows = False
        Me.DgvPaletteMain.AllowUserToResizeColumns = False
        Me.DgvPaletteMain.AllowUserToResizeRows = False
        Me.DgvPaletteMain.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells
        Me.DgvPaletteMain.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColColor})
        Me.DgvPaletteMain.Dock = System.Windows.Forms.DockStyle.Right
        Me.DgvPaletteMain.Location = New System.Drawing.Point(808, 117)
        Me.DgvPaletteMain.Name = "DgvPaletteMain"
        Me.DgvPaletteMain.RowHeadersWidth = 75
        Me.DgvPaletteMain.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DgvPaletteMain.RowTemplate.Height = 20
        Me.DgvPaletteMain.Size = New System.Drawing.Size(200, 663)
        Me.DgvPaletteMain.TabIndex = 20
        '
        'ColColor
        '
        Me.ColColor.HeaderText = "Color"
        Me.ColColor.Name = "ColColor"
        '
        'TsFrame
        '
        Me.TsFrame.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TslFrame, Me.TsbFrame_ImportPNG, Me.TsbFrame_ExportPNG, Me.Tss_Frame_1, Me.TsbFrame_Add, Me.TsbFrame_Delete, Me.Tss_Frame_2, Me.TsbFrame_OffsetAll, Me.TsbFrame_OffsetUp, Me.TsbFrame_OffsetDown, Me.TsbFrame_OffsetLeft, Me.TsbFrame_OffsetRight, Me.TslFrame_Offset, Me.TstOffsetX, Me.TstOffsetY, Me.Tss_Frame_4, Me.TsbFrame_IndexDecrease, Me.TsbFrame_IndexIncrease, Me.TslFrame_Index})
        Me.TsFrame.Location = New System.Drawing.Point(0, 39)
        Me.TsFrame.Name = "TsFrame"
        Me.TsFrame.Size = New System.Drawing.Size(1008, 39)
        Me.TsFrame.TabIndex = 21
        Me.TsFrame.Text = "ToolStrip1"
        '
        'TslFrame
        '
        Me.TslFrame.AutoSize = False
        Me.TslFrame.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.TslFrame.Name = "TslFrame"
        Me.TslFrame.Size = New System.Drawing.Size(100, 22)
        Me.TslFrame.Text = "Frame"
        Me.TslFrame.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TsbFrame_ImportPNG
        '
        Me.TsbFrame_ImportPNG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_ImportPNG.Image = CType(resources.GetObject("TsbFrame_ImportPNG.Image"), System.Drawing.Image)
        Me.TsbFrame_ImportPNG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_ImportPNG.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_ImportPNG.Name = "TsbFrame_ImportPNG"
        Me.TsbFrame_ImportPNG.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_ImportPNG.Text = "Open .PNG to use in this frame. Right-click to import a .PNG as a new frame."
        '
        'TsbFrame_ExportPNG
        '
        Me.TsbFrame_ExportPNG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_ExportPNG.Image = CType(resources.GetObject("TsbFrame_ExportPNG.Image"), System.Drawing.Image)
        Me.TsbFrame_ExportPNG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_ExportPNG.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_ExportPNG.Name = "TsbFrame_ExportPNG"
        Me.TsbFrame_ExportPNG.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_ExportPNG.Text = "Save frame as .PNG"
        '
        'Tss_Frame_1
        '
        Me.Tss_Frame_1.Name = "Tss_Frame_1"
        Me.Tss_Frame_1.Size = New System.Drawing.Size(6, 39)
        '
        'TslFrame_Index
        '
        Me.TslFrame_Index.Name = "TslFrame_Index"
        Me.TslFrame_Index.Size = New System.Drawing.Size(65, 36)
        Me.TslFrame_Index.Text = "Index: 0 / 0"
        '
        'TsbFrame_IndexDecrease
        '
        Me.TsbFrame_IndexDecrease.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_IndexDecrease.Image = CType(resources.GetObject("TsbFrame_IndexDecrease.Image"), System.Drawing.Image)
        Me.TsbFrame_IndexDecrease.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_IndexDecrease.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_IndexDecrease.Name = "TsbFrame_IndexDecrease"
        Me.TsbFrame_IndexDecrease.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_IndexDecrease.Text = "Show frame earlier in animation"
        '
        'TsbFrame_IndexIncrease
        '
        Me.TsbFrame_IndexIncrease.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_IndexIncrease.Image = CType(resources.GetObject("TsbFrame_IndexIncrease.Image"), System.Drawing.Image)
        Me.TsbFrame_IndexIncrease.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_IndexIncrease.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_IndexIncrease.Name = "TsbFrame_IndexIncrease"
        Me.TsbFrame_IndexIncrease.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_IndexIncrease.Text = "Show frame later in animation"
        '
        'Tss_Frame_2
        '
        Me.Tss_Frame_2.Name = "Tss_Frame_2"
        Me.Tss_Frame_2.Size = New System.Drawing.Size(6, 39)
        '
        'TsbFrame_Add
        '
        Me.TsbFrame_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_Add.Image = CType(resources.GetObject("TsbFrame_Add.Image"), System.Drawing.Image)
        Me.TsbFrame_Add.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_Add.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_Add.Name = "TsbFrame_Add"
        Me.TsbFrame_Add.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_Add.Text = "Add frame"
        '
        'TsbFrame_Delete
        '
        Me.TsbFrame_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_Delete.Image = CType(resources.GetObject("TsbFrame_Delete.Image"), System.Drawing.Image)
        Me.TsbFrame_Delete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_Delete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_Delete.Name = "TsbFrame_Delete"
        Me.TsbFrame_Delete.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_Delete.Text = "Delete frame"
        '
        'TsbFrame_OffsetAll
        '
        Me.TsbFrame_OffsetAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_OffsetAll.Image = CType(resources.GetObject("TsbFrame_OffsetAll.Image"), System.Drawing.Image)
        Me.TsbFrame_OffsetAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_OffsetAll.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_OffsetAll.Name = "TsbFrame_OffsetAll"
        Me.TsbFrame_OffsetAll.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_OffsetAll.Text = "Force offset adjustments on all frames"
        '
        'TsbFrame_OffsetUp
        '
        Me.TsbFrame_OffsetUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_OffsetUp.Image = CType(resources.GetObject("TsbFrame_OffsetUp.Image"), System.Drawing.Image)
        Me.TsbFrame_OffsetUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_OffsetUp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_OffsetUp.Name = "TsbFrame_OffsetUp"
        Me.TsbFrame_OffsetUp.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_OffsetUp.Text = "Move up. Right-click to move up 16 pixels at a time."
        '
        'TsbFrame_OffsetDown
        '
        Me.TsbFrame_OffsetDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_OffsetDown.Image = CType(resources.GetObject("TsbFrame_OffsetDown.Image"), System.Drawing.Image)
        Me.TsbFrame_OffsetDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_OffsetDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_OffsetDown.Name = "TsbFrame_OffsetDown"
        Me.TsbFrame_OffsetDown.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_OffsetDown.Text = "Move down. Right-click to move down 16 pixels at a time."
        '
        'TsbFrame_OffsetLeft
        '
        Me.TsbFrame_OffsetLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_OffsetLeft.Image = CType(resources.GetObject("TsbFrame_OffsetLeft.Image"), System.Drawing.Image)
        Me.TsbFrame_OffsetLeft.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_OffsetLeft.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_OffsetLeft.Name = "TsbFrame_OffsetLeft"
        Me.TsbFrame_OffsetLeft.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_OffsetLeft.Text = "Move left. Right-click to move left 16 pixels at a time."
        '
        'TsbFrame_OffsetRight
        '
        Me.TsbFrame_OffsetRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbFrame_OffsetRight.Image = CType(resources.GetObject("TsbFrame_OffsetRight.Image"), System.Drawing.Image)
        Me.TsbFrame_OffsetRight.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbFrame_OffsetRight.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbFrame_OffsetRight.Name = "TsbFrame_OffsetRight"
        Me.TsbFrame_OffsetRight.Size = New System.Drawing.Size(36, 36)
        Me.TsbFrame_OffsetRight.Text = "Move right. Right-click to move right 16 pixels at a time."
        '
        'TslFrame_Offset
        '
        Me.TslFrame_Offset.Name = "TslFrame_Offset"
        Me.TslFrame_Offset.Size = New System.Drawing.Size(68, 36)
        Me.TslFrame_Offset.Text = "Offset: X, Y:"
        '
        'TstOffsetX
        '
        Me.TstOffsetX.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TstOffsetX.Name = "TstOffsetX"
        Me.TstOffsetX.Size = New System.Drawing.Size(50, 39)
        Me.TstOffsetX.ToolTipText = "Offset X. Press [Enter] to confirm new values."
        '
        'TstOffsetY
        '
        Me.TstOffsetY.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TstOffsetY.Name = "TstOffsetY"
        Me.TstOffsetY.Size = New System.Drawing.Size(50, 39)
        Me.TstOffsetY.ToolTipText = "Offset Y. Press [Enter] to confirm new values."
        '
        'Tss_Frame_4
        '
        Me.Tss_Frame_4.Name = "Tss_Frame_4"
        Me.Tss_Frame_4.Size = New System.Drawing.Size(6, 39)
        '
        'TsTools
        '
        Me.TsTools.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TslMisc, Me.TsbOpenPalBldg8, Me.TsbOpenPalBldg16, Me.TssMisc_1, Me.TsbGridBG, Me.TsbPreview_BGGraphic, Me.TssMisc_2, Me.TsbBatchConversion, Me.TsbBatchRotFix, Me.TsbDelete_ZT1Files, Me.TsbDelete_PNG, Me.Tss_Misc_3, Me.TslFrame_FP, Me.TsbFrame_fpX, Me.TsbFrame_fpY, Me.TssMisc_4, Me.TsbSettings, Me.TsbAbout})
        Me.TsTools.Location = New System.Drawing.Point(0, 78)
        Me.TsTools.Name = "TsTools"
        Me.TsTools.Size = New System.Drawing.Size(1008, 39)
        Me.TsTools.TabIndex = 22
        Me.TsTools.Text = "Misc"
        '
        'TslMisc
        '
        Me.TslMisc.AutoSize = False
        Me.TslMisc.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.TslMisc.Name = "TslMisc"
        Me.TslMisc.Size = New System.Drawing.Size(100, 22)
        Me.TslMisc.Text = "Misc."
        Me.TslMisc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TsbOpenPalBldg8
        '
        Me.TsbOpenPalBldg8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbOpenPalBldg8.Image = CType(resources.GetObject("TsbOpenPalBldg8.Image"), System.Drawing.Image)
        Me.TsbOpenPalBldg8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbOpenPalBldg8.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbOpenPalBldg8.Name = "TsbOpenPalBldg8"
        Me.TsbOpenPalBldg8.Size = New System.Drawing.Size(45, 36)
        Me.TsbOpenPalBldg8.Text = "Quick access to 8-color palettes"
        '
        'TsbOpenPalBldg16
        '
        Me.TsbOpenPalBldg16.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbOpenPalBldg16.Image = CType(resources.GetObject("TsbOpenPalBldg16.Image"), System.Drawing.Image)
        Me.TsbOpenPalBldg16.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbOpenPalBldg16.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbOpenPalBldg16.Name = "TsbOpenPalBldg16"
        Me.TsbOpenPalBldg16.Size = New System.Drawing.Size(45, 36)
        Me.TsbOpenPalBldg16.Text = "Quick access to 16-color palettes"
        '
        'TssMisc_1
        '
        Me.TssMisc_1.Name = "TssMisc_1"
        Me.TssMisc_1.Size = New System.Drawing.Size(6, 39)
        '
        'TsbGridBG
        '
        Me.TsbGridBG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbGridBG.Image = CType(resources.GetObject("TsbGridBG.Image"), System.Drawing.Image)
        Me.TsbGridBG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbGridBG.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbGridBG.Name = "TsbGridBG"
        Me.TsbGridBG.Size = New System.Drawing.Size(36, 36)
        Me.TsbGridBG.Text = "Change the canvas background"
        Me.TsbGridBG.ToolTipText = "Background color of the image preview"
        '
        'TsbPreview_BGGraphic
        '
        Me.TsbPreview_BGGraphic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbPreview_BGGraphic.Image = CType(resources.GetObject("TsbPreview_BGGraphic.Image"), System.Drawing.Image)
        Me.TsbPreview_BGGraphic.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbPreview_BGGraphic.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbPreview_BGGraphic.Name = "TsbPreview_BGGraphic"
        Me.TsbPreview_BGGraphic.Size = New System.Drawing.Size(36, 36)
        Me.TsbPreview_BGGraphic.Text = "Open ZT1 Graphic and use it as background"
        '
        'TssMisc_2
        '
        Me.TssMisc_2.Name = "TssMisc_2"
        Me.TssMisc_2.Size = New System.Drawing.Size(6, 39)
        '
        'TsbBatchConversion
        '
        Me.TsbBatchConversion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbBatchConversion.Image = CType(resources.GetObject("TsbBatchConversion.Image"), System.Drawing.Image)
        Me.TsbBatchConversion.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbBatchConversion.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbBatchConversion.Name = "TsbBatchConversion"
        Me.TsbBatchConversion.Size = New System.Drawing.Size(36, 36)
        Me.TsbBatchConversion.Text = "Batch graphic onversion: ZT1 Graphic <=> .PNG"
        Me.TsbBatchConversion.ToolTipText = "Batch graphic graphic onversion: ZT1 Graphic <=> .PNG"
        '
        'TsbBatchRotFix
        '
        Me.TsbBatchRotFix.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbBatchRotFix.Image = CType(resources.GetObject("TsbBatchRotFix.Image"), System.Drawing.Image)
        Me.TsbBatchRotFix.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbBatchRotFix.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbBatchRotFix.Name = "TsbBatchRotFix"
        Me.TsbBatchRotFix.Size = New System.Drawing.Size(36, 36)
        Me.TsbBatchRotFix.Text = "Batch offset fixing"
        '
        'TsbDelete_ZT1Files
        '
        Me.TsbDelete_ZT1Files.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbDelete_ZT1Files.Image = CType(resources.GetObject("TsbDelete_ZT1Files.Image"), System.Drawing.Image)
        Me.TsbDelete_ZT1Files.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbDelete_ZT1Files.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbDelete_ZT1Files.Name = "TsbDelete_ZT1Files"
        Me.TsbDelete_ZT1Files.Size = New System.Drawing.Size(36, 36)
        Me.TsbDelete_ZT1Files.Text = "Delete all ZT1 Graphics and color palettes in the root folder"
        '
        'TsbDelete_PNG
        '
        Me.TsbDelete_PNG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbDelete_PNG.Image = CType(resources.GetObject("TsbDelete_PNG.Image"), System.Drawing.Image)
        Me.TsbDelete_PNG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbDelete_PNG.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbDelete_PNG.Name = "TsbDelete_PNG"
        Me.TsbDelete_PNG.Size = New System.Drawing.Size(36, 36)
        Me.TsbDelete_PNG.Text = "Delete all .PNG files in the root directory"
        '
        'Tss_Misc_3
        '
        Me.Tss_Misc_3.Name = "Tss_Misc_3"
        Me.Tss_Misc_3.Size = New System.Drawing.Size(6, 39)
        '
        'TslFrame_FP
        '
        Me.TslFrame_FP.Name = "TslFrame_FP"
        Me.TslFrame_FP.Size = New System.Drawing.Size(79, 36)
        Me.TslFrame_FP.Text = "Footprint X,Y:"
        '
        'TsbFrame_fpX
        '
        Me.TsbFrame_fpX.AutoSize = False
        Me.TsbFrame_fpX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TsbFrame_fpX.Items.AddRange(New Object() {"2", "4", "6", "8", "10", "12", "14", "16", "18"})
        Me.TsbFrame_fpX.Name = "TsbFrame_fpX"
        Me.TsbFrame_fpX.Size = New System.Drawing.Size(50, 23)
        '
        'TsbFrame_fpY
        '
        Me.TsbFrame_fpY.AutoSize = False
        Me.TsbFrame_fpY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TsbFrame_fpY.Items.AddRange(New Object() {"2", "4", "6", "8", "10", "12", "14", "16", "18"})
        Me.TsbFrame_fpY.Name = "TsbFrame_fpY"
        Me.TsbFrame_fpY.Size = New System.Drawing.Size(50, 23)
        '
        'TssMisc_4
        '
        Me.TssMisc_4.Name = "TssMisc_4"
        Me.TssMisc_4.Size = New System.Drawing.Size(6, 39)
        '
        'TsbSettings
        '
        Me.TsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbSettings.Image = CType(resources.GetObject("TsbSettings.Image"), System.Drawing.Image)
        Me.TsbSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbSettings.Name = "TsbSettings"
        Me.TsbSettings.Size = New System.Drawing.Size(36, 36)
        Me.TsbSettings.Text = "Settings"
        '
        'TsbAbout
        '
        Me.TsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TsbAbout.Image = CType(resources.GetObject("TsbAbout.Image"), System.Drawing.Image)
        Me.TsbAbout.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.TsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TsbAbout.Name = "TsbAbout"
        Me.TsbAbout.Size = New System.Drawing.Size(36, 36)
        Me.TsbAbout.Text = "About"
        '
        'MnuPal
        '
        Me.MnuPal.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPal_MoveEnd, Me.mnuPal_MoveUp, Me.mnuPal_MoveDown, Me.mnuPal_Replace, Me.mnuPal_Add, Me.mnuPal_SavePAL, Me.mnuPal_ExportPNG, Me.mnuPal_ImportPNG, Me.mnuPal_ImportGimpPalette})
        Me.MnuPal.Name = "MnuPal"
        Me.MnuPal.Size = New System.Drawing.Size(245, 202)
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
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 780)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PicBox)
        Me.Controls.Add(Me.DgvPaletteMain)
        Me.Controls.Add(Me.TsTools)
        Me.Controls.Add(Me.TsFrame)
        Me.Controls.Add(Me.TsZT1Graphic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmMain"
        Me.Text = "ZT Studio"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TsZT1Graphic.ResumeLayout(False)
        Me.TsZT1Graphic.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GBAnimation.ResumeLayout(False)
        Me.GBAnimation.PerformLayout()
        CType(Me.TbFrames, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GBOtherViews.ResumeLayout(False)
        Me.GBColors.ResumeLayout(False)
        Me.GBColors.PerformLayout()
        Me.SsBar.ResumeLayout(False)
        Me.SsBar.PerformLayout()
        CType(Me.DgvPaletteMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TsFrame.ResumeLayout(False)
        Me.TsFrame.PerformLayout()
        Me.TsTools.ResumeLayout(False)
        Me.TsTools.PerformLayout()
        Me.MnuPal.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PicBox As System.Windows.Forms.PictureBox
    Friend WithEvents TmrAnimation As System.Windows.Forms.Timer
    Friend WithEvents DlgColor As System.Windows.Forms.ColorDialog
    Friend WithEvents TsZT1Graphic As System.Windows.Forms.ToolStrip
    Friend WithEvents TsbZT1Open As System.Windows.Forms.ToolStripButton
    Friend WithEvents DlgOpen As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents DgvPaletteMain As System.Windows.Forms.DataGridView
    Friend WithEvents SsBar As System.Windows.Forms.StatusStrip
    Friend WithEvents ssFileName As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents DlgSave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ssColor As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents TsbZT1_OpenPal As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbZT1Write As System.Windows.Forms.ToolStripButton
    Friend WithEvents TslZT1Graphic As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TsFrame As System.Windows.Forms.ToolStrip
    Friend WithEvents TslFrame As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TsbFrame_ExportPNG As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsTools As System.Windows.Forms.ToolStrip
    Friend WithEvents TslMisc As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TsbFrame_ImportPNG As System.Windows.Forms.ToolStripButton
    Friend WithEvents Tss_Frame_2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TsbFrame_OffsetUp As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbFrame_OffsetDown As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbFrame_OffsetLeft As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbFrame_OffsetRight As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbOpenPalBldg8 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents TsbOpenPalBldg16 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents TssMisc_1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TsbGridBG As System.Windows.Forms.ToolStripButton
    Friend WithEvents TssMisc_2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TsbBatchConversion As System.Windows.Forms.ToolStripButton
    Friend WithEvents TssMisc_4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TsbSettings As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbAbout As System.Windows.Forms.ToolStripButton
    Friend WithEvents Tss_Graphic_1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TslZT1_AnimSpeed As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TstZT1_AnimSpeed As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents TslFrame_Offset As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TsbFrame_IndexDecrease As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbFrame_IndexIncrease As System.Windows.Forms.ToolStripButton
    Friend WithEvents TslFrame_Index As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TsbPreview_BGGraphic As System.Windows.Forms.ToolStripButton
    Friend WithEvents Tss_Frame_1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TsbFrame_Add As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbFrame_Delete As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbGraphic_ExtraFrame As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbDelete_PNG As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbDelete_ZT1Files As System.Windows.Forms.ToolStripButton
    Friend WithEvents Tss_Frame_4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MnuPal As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuPal_MoveEnd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_MoveUp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_MoveDown As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_Replace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_Add As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_ImportPNG As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_ExportPNG As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_SavePAL As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPal_ImportGimpPalette As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TsbBatchRotFix As System.Windows.Forms.ToolStripButton
    Friend WithEvents TsbZT1New As System.Windows.Forms.ToolStripButton
    Friend WithEvents ColColor As DataGridViewTextBoxColumn
    Friend WithEvents GBAnimation As GroupBox
    Friend WithEvents LblFrames As Label
    Friend WithEvents LblAnimTime As Label
    Friend WithEvents ChkPlayAnimation As CheckBox
    Friend WithEvents GBColors As GroupBox
    Friend WithEvents LblColorTool As Label
    Friend WithEvents LblColorDetails As Label
    Friend WithEvents LblColor As Label
    Friend WithEvents TbFrames As TrackBar
    Friend WithEvents GBOtherViews As GroupBox
    Friend WithEvents TVExplorer As TreeView
    Friend WithEvents TstOffsetX As ToolStripTextBox
    Friend WithEvents TstOffsetY As ToolStripTextBox
    Friend WithEvents Tss_Misc_3 As ToolStripSeparator
    Friend WithEvents TslFrame_FP As ToolStripLabel
    Friend WithEvents TsbFrame_fpX As ToolStripComboBox
    Friend WithEvents TsbFrame_fpY As ToolStripComboBox
    Friend WithEvents TsbFrame_OffsetAll As ToolStripButton
End Class
