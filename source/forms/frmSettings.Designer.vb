<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmSettings
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
        Me.dlgBrowseFolder = New System.Windows.Forms.FolderBrowserDialog()
        Me.tpPalette = New System.Windows.Forms.TabPage()
        Me.ChkPalImportPNGForceAddAll = New System.Windows.Forms.CheckBox()
        Me.tpWritePNG = New System.Windows.Forms.TabPage()
        Me.ChkPNGTransparentBG = New System.Windows.Forms.CheckBox()
        Me.LblHowToExportPNG = New System.Windows.Forms.Label()
        Me.CboPNGExport_Crop = New System.Windows.Forms.ComboBox()
        Me.tpWriteZT1 = New System.Windows.Forms.TabPage()
        Me.ChkExportZT1_Ani = New System.Windows.Forms.CheckBox()
        Me.ChkExportZT1_AddZTAFBytes = New System.Windows.Forms.CheckBox()
        Me.tpRenderingFrames = New System.Windows.Forms.TabPage()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.numFrameDefaultAnimSpeed = New System.Windows.Forms.NumericUpDown()
        Me.LblDefaultAnimSpeed = New System.Windows.Forms.Label()
        Me.ChkRenderFrame_BGGraphic = New System.Windows.Forms.CheckBox()
        Me.tpConversions = New System.Windows.Forms.TabPage()
        Me.TxtConvert_fileNameDelimiter = New System.Windows.Forms.TextBox()
        Me.LblConvert_fileNameDelimiter = New System.Windows.Forms.Label()
        Me.ChkConvert_SharedColorPalette = New System.Windows.Forms.CheckBox()
        Me.ChkConvert_Overwrite = New System.Windows.Forms.CheckBox()
        Me.ChkConvert_DeleteOriginal = New System.Windows.Forms.CheckBox()
        Me.LblExportPNG_Index = New System.Windows.Forms.Label()
        Me.NumConvert_PNGStartIndex = New System.Windows.Forms.NumericUpDown()
        Me.tpFolders = New System.Windows.Forms.TabPage()
        Me.BtnBrowsePal16 = New System.Windows.Forms.Button()
        Me.BtnBrowsePal8 = New System.Windows.Forms.Button()
        Me.BtnBrowse = New System.Windows.Forms.Button()
        Me.txtFolderPal16 = New System.Windows.Forms.TextBox()
        Me.txtFolderPal8 = New System.Windows.Forms.TextBox()
        Me.txtRootFolder = New System.Windows.Forms.TextBox()
        Me.LblColorPal16 = New System.Windows.Forms.Label()
        Me.LblColorPal8 = New System.Windows.Forms.Label()
        Me.LblRootFolder = New System.Windows.Forms.Label()
        Me.TCSettings = New System.Windows.Forms.TabControl()
        Me.LblHelp = New System.Windows.Forms.Label()
        Me.LblHelpTopic = New System.Windows.Forms.Label()
        Me.tpPalette.SuspendLayout
        Me.tpWritePNG.SuspendLayout
        Me.tpWriteZT1.SuspendLayout
        Me.tpRenderingFrames.SuspendLayout
        CType(Me.numFrameDefaultAnimSpeed, System.ComponentModel.ISupportInitialize).BeginInit
        Me.tpConversions.SuspendLayout
        CType(Me.NumConvert_PNGStartIndex, System.ComponentModel.ISupportInitialize).BeginInit
        Me.tpFolders.SuspendLayout
        Me.TCSettings.SuspendLayout
        Me.SuspendLayout
        '
        'tpPalette
        '
        Me.tpPalette.Controls.Add(Me.ChkPalImportPNGForceAddAll)
        Me.tpPalette.Location = New System.Drawing.Point(4, 22)
        Me.tpPalette.Name = "tpPalette"
        Me.tpPalette.Size = New System.Drawing.Size(710, 248)
        Me.tpPalette.TabIndex = 6
        Me.tpPalette.Text = "Color palettes"
        Me.tpPalette.UseVisualStyleBackColor = True
        '
        'ChkPalImportPNGForceAddAll
        '
        Me.ChkPalImportPNGForceAddAll.AutoSize = True
        Me.ChkPalImportPNGForceAddAll.Location = New System.Drawing.Point(24, 15)
        Me.ChkPalImportPNGForceAddAll.Name = "ChkPalImportPNGForceAddAll"
        Me.ChkPalImportPNGForceAddAll.Size = New System.Drawing.Size(344, 17)
        Me.ChkPalImportPNGForceAddAll.TabIndex = 23
        Me.ChkPalImportPNGForceAddAll.Text = "Add all colors (even identical) when importing from .PNG files "
        Me.ChkPalImportPNGForceAddAll.UseVisualStyleBackColor = True
        '
        'tpWritePNG
        '
        Me.tpWritePNG.Controls.Add(Me.ChkPNGTransparentBG)
        Me.tpWritePNG.Controls.Add(Me.LblHowToExportPNG)
        Me.tpWritePNG.Controls.Add(Me.CboPNGExport_Crop)
        Me.tpWritePNG.Location = New System.Drawing.Point(4, 22)
        Me.tpWritePNG.Name = "tpWritePNG"
        Me.tpWritePNG.Padding = New System.Windows.Forms.Padding(3)
        Me.tpWritePNG.Size = New System.Drawing.Size(710, 248)
        Me.tpWritePNG.TabIndex = 4
        Me.tpWritePNG.Text = "Export as PNG"
        Me.tpWritePNG.UseVisualStyleBackColor = True
        '
        'ChkPNGTransparentBG
        '
        Me.ChkPNGTransparentBG.AutoSize = True
        Me.ChkPNGTransparentBG.Location = New System.Drawing.Point(24, 118)
        Me.ChkPNGTransparentBG.Name = "ChkPNGTransparentBG"
        Me.ChkPNGTransparentBG.Size = New System.Drawing.Size(481, 17)
        Me.ChkPNGTransparentBG.TabIndex = 15
        Me.ChkPNGTransparentBG.Text = "Use transparent background with alphachannel (instead of ZT Studio background col" &
    "or)"
        Me.ChkPNGTransparentBG.UseVisualStyleBackColor = True
        '
        'LblHowToExportPNG
        '
        Me.LblHowToExportPNG.AutoSize = True
        Me.LblHowToExportPNG.Location = New System.Drawing.Point(23, 39)
        Me.LblHowToExportPNG.Name = "LblHowToExportPNG"
        Me.LblHowToExportPNG.Size = New System.Drawing.Size(108, 13)
        Me.LblHowToExportPNG.TabIndex = 14
        Me.LblHowToExportPNG.Text = "PNG Export method"
        '
        'CboPNGExport_Crop
        '
        Me.CboPNGExport_Crop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboPNGExport_Crop.FormattingEnabled = True
        Me.CboPNGExport_Crop.Location = New System.Drawing.Point(26, 55)
        Me.CboPNGExport_Crop.Name = "CboPNGExport_Crop"
        Me.CboPNGExport_Crop.Size = New System.Drawing.Size(520, 21)
        Me.CboPNGExport_Crop.TabIndex = 13
        '
        'tpWriteZT1
        '
        Me.tpWriteZT1.Controls.Add(Me.ChkExportZT1_Ani)
        Me.tpWriteZT1.Controls.Add(Me.ChkExportZT1_AddZTAFBytes)
        Me.tpWriteZT1.Location = New System.Drawing.Point(4, 22)
        Me.tpWriteZT1.Name = "tpWriteZT1"
        Me.tpWriteZT1.Padding = New System.Windows.Forms.Padding(3)
        Me.tpWriteZT1.Size = New System.Drawing.Size(710, 248)
        Me.tpWriteZT1.TabIndex = 3
        Me.tpWriteZT1.Text = "Saving as ZT1 Graphic"
        Me.tpWriteZT1.UseVisualStyleBackColor = True
        '
        'ChkExportZT1_Ani
        '
        Me.ChkExportZT1_Ani.AutoSize = True
        Me.ChkExportZT1_Ani.Checked = True
        Me.ChkExportZT1_Ani.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkExportZT1_Ani.Location = New System.Drawing.Point(24, 38)
        Me.ChkExportZT1_Ani.Name = "ChkExportZT1_Ani"
        Me.ChkExportZT1_Ani.Size = New System.Drawing.Size(142, 17)
        Me.ChkExportZT1_Ani.TabIndex = 29
        Me.ChkExportZT1_Ani.Text = "Create/update .ani-file"
        Me.ChkExportZT1_Ani.UseVisualStyleBackColor = True
        '
        'ChkExportZT1_AddZTAFBytes
        '
        Me.ChkExportZT1_AddZTAFBytes.AutoSize = True
        Me.ChkExportZT1_AddZTAFBytes.Location = New System.Drawing.Point(24, 15)
        Me.ChkExportZT1_AddZTAFBytes.Name = "ChkExportZT1_AddZTAFBytes"
        Me.ChkExportZT1_AddZTAFBytes.Size = New System.Drawing.Size(178, 17)
        Me.ChkExportZT1_AddZTAFBytes.TabIndex = 28
        Me.ChkExportZT1_AddZTAFBytes.Text = "Add ""ZT Animation File""-bytes"
        Me.ChkExportZT1_AddZTAFBytes.UseVisualStyleBackColor = True
        '
        'tpRenderingFrames
        '
        Me.tpRenderingFrames.Controls.Add(Me.Label3)
        Me.tpRenderingFrames.Controls.Add(Me.numFrameDefaultAnimSpeed)
        Me.tpRenderingFrames.Controls.Add(Me.LblDefaultAnimSpeed)
        Me.tpRenderingFrames.Controls.Add(Me.ChkRenderFrame_BGGraphic)
        Me.tpRenderingFrames.Location = New System.Drawing.Point(4, 22)
        Me.tpRenderingFrames.Name = "tpRenderingFrames"
        Me.tpRenderingFrames.Padding = New System.Windows.Forms.Padding(3)
        Me.tpRenderingFrames.Size = New System.Drawing.Size(710, 248)
        Me.tpRenderingFrames.TabIndex = 2
        Me.tpRenderingFrames.Text = "Rendering frames"
        Me.tpRenderingFrames.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(24, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(558, 13)
        Me.Label3.TabIndex = 37
        Me.Label3.Text = "These settings are applied in both the UI (main window) and when exporting graphi" &
    "cs to different formats."
        '
        'numFrameDefaultAnimSpeed
        '
        Me.numFrameDefaultAnimSpeed.Location = New System.Drawing.Point(24, 153)
        Me.numFrameDefaultAnimSpeed.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.numFrameDefaultAnimSpeed.Name = "numFrameDefaultAnimSpeed"
        Me.numFrameDefaultAnimSpeed.Size = New System.Drawing.Size(120, 22)
        Me.numFrameDefaultAnimSpeed.TabIndex = 36
        '
        'LblDefaultAnimSpeed
        '
        Me.LblDefaultAnimSpeed.AutoSize = True
        Me.LblDefaultAnimSpeed.Location = New System.Drawing.Point(21, 137)
        Me.LblDefaultAnimSpeed.Name = "LblDefaultAnimSpeed"
        Me.LblDefaultAnimSpeed.Size = New System.Drawing.Size(137, 13)
        Me.LblDefaultAnimSpeed.TabIndex = 34
        Me.LblDefaultAnimSpeed.Text = "Default animation speed:"
        '
        'ChkRenderFrame_BGGraphic
        '
        Me.ChkRenderFrame_BGGraphic.AutoSize = True
        Me.ChkRenderFrame_BGGraphic.Checked = True
        Me.ChkRenderFrame_BGGraphic.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkRenderFrame_BGGraphic.Location = New System.Drawing.Point(24, 79)
        Me.ChkRenderFrame_BGGraphic.Name = "ChkRenderFrame_BGGraphic"
        Me.ChkRenderFrame_BGGraphic.Size = New System.Drawing.Size(234, 17)
        Me.ChkRenderFrame_BGGraphic.TabIndex = 17
        Me.ChkRenderFrame_BGGraphic.Text = "Use selected ZT1 Graphic as background"
        Me.ChkRenderFrame_BGGraphic.UseVisualStyleBackColor = True
        '
        'tpConversions
        '
        Me.tpConversions.Controls.Add(Me.TxtConvert_fileNameDelimiter)
        Me.tpConversions.Controls.Add(Me.LblConvert_fileNameDelimiter)
        Me.tpConversions.Controls.Add(Me.ChkConvert_SharedColorPalette)
        Me.tpConversions.Controls.Add(Me.ChkConvert_Overwrite)
        Me.tpConversions.Controls.Add(Me.ChkConvert_DeleteOriginal)
        Me.tpConversions.Controls.Add(Me.LblExportPNG_Index)
        Me.tpConversions.Controls.Add(Me.NumConvert_PNGStartIndex)
        Me.tpConversions.Location = New System.Drawing.Point(4, 22)
        Me.tpConversions.Name = "tpConversions"
        Me.tpConversions.Padding = New System.Windows.Forms.Padding(3)
        Me.tpConversions.Size = New System.Drawing.Size(710, 248)
        Me.tpConversions.TabIndex = 0
        Me.tpConversions.Text = "Batch convert ZT1 <> PNG"
        Me.tpConversions.UseVisualStyleBackColor = True
        '
        'TxtConvert_fileNameDelimiter
        '
        Me.TxtConvert_fileNameDelimiter.Location = New System.Drawing.Point(21, 104)
        Me.TxtConvert_fileNameDelimiter.Name = "TxtConvert_fileNameDelimiter"
        Me.TxtConvert_fileNameDelimiter.Size = New System.Drawing.Size(61, 22)
        Me.TxtConvert_fileNameDelimiter.TabIndex = 33
        '
        'LblConvert_fileNameDelimiter
        '
        Me.LblConvert_fileNameDelimiter.AutoSize = True
        Me.LblConvert_fileNameDelimiter.Location = New System.Drawing.Point(18, 88)
        Me.LblConvert_fileNameDelimiter.Name = "LblConvert_fileNameDelimiter"
        Me.LblConvert_fileNameDelimiter.Size = New System.Drawing.Size(107, 13)
        Me.LblConvert_fileNameDelimiter.TabIndex = 32
        Me.LblConvert_fileNameDelimiter.Text = "File name delimiter:"
        '
        'ChkConvert_SharedColorPalette
        '
        Me.ChkConvert_SharedColorPalette.AutoSize = True
        Me.ChkConvert_SharedColorPalette.Checked = True
        Me.ChkConvert_SharedColorPalette.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkConvert_SharedColorPalette.Location = New System.Drawing.Point(21, 183)
        Me.ChkConvert_SharedColorPalette.Name = "ChkConvert_SharedColorPalette"
        Me.ChkConvert_SharedColorPalette.Size = New System.Drawing.Size(341, 17)
        Me.ChkConvert_SharedColorPalette.TabIndex = 30
        Me.ChkConvert_SharedColorPalette.Text = "Use shared color palette for each graphic's animations/views."
        Me.ChkConvert_SharedColorPalette.UseVisualStyleBackColor = True
        '
        'ChkConvert_Overwrite
        '
        Me.ChkConvert_Overwrite.AutoSize = True
        Me.ChkConvert_Overwrite.Checked = True
        Me.ChkConvert_Overwrite.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkConvert_Overwrite.Enabled = False
        Me.ChkConvert_Overwrite.Location = New System.Drawing.Point(21, 206)
        Me.ChkConvert_Overwrite.Name = "ChkConvert_Overwrite"
        Me.ChkConvert_Overwrite.Size = New System.Drawing.Size(100, 17)
        Me.ChkConvert_Overwrite.TabIndex = 29
        Me.ChkConvert_Overwrite.Text = "Overwrite files"
        Me.ChkConvert_Overwrite.UseVisualStyleBackColor = True
        Me.ChkConvert_Overwrite.Visible = False
        '
        'ChkConvert_DeleteOriginal
        '
        Me.ChkConvert_DeleteOriginal.AutoSize = True
        Me.ChkConvert_DeleteOriginal.Checked = True
        Me.ChkConvert_DeleteOriginal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkConvert_DeleteOriginal.Location = New System.Drawing.Point(21, 160)
        Me.ChkConvert_DeleteOriginal.Name = "ChkConvert_DeleteOriginal"
        Me.ChkConvert_DeleteOriginal.Size = New System.Drawing.Size(224, 17)
        Me.ChkConvert_DeleteOriginal.TabIndex = 28
        Me.ChkConvert_DeleteOriginal.Text = "Delete the source file after conversion."
        Me.ChkConvert_DeleteOriginal.UseVisualStyleBackColor = True
        '
        'LblExportPNG_Index
        '
        Me.LblExportPNG_Index.AutoSize = True
        Me.LblExportPNG_Index.Location = New System.Drawing.Point(75, 33)
        Me.LblExportPNG_Index.Name = "LblExportPNG_Index"
        Me.LblExportPNG_Index.Size = New System.Drawing.Size(354, 13)
        Me.LblExportPNG_Index.TabIndex = 27
        Me.LblExportPNG_Index.Text = "Start numbering of .PNG-file series at either 0 (index) or 1 (frame #1)"
        '
        'NumConvert_PNGStartIndex
        '
        Me.NumConvert_PNGStartIndex.Location = New System.Drawing.Point(21, 31)
        Me.NumConvert_PNGStartIndex.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumConvert_PNGStartIndex.Name = "NumConvert_PNGStartIndex"
        Me.NumConvert_PNGStartIndex.Size = New System.Drawing.Size(48, 22)
        Me.NumConvert_PNGStartIndex.TabIndex = 26
        '
        'tpFolders
        '
        Me.tpFolders.Controls.Add(Me.BtnBrowsePal16)
        Me.tpFolders.Controls.Add(Me.BtnBrowsePal8)
        Me.tpFolders.Controls.Add(Me.BtnBrowse)
        Me.tpFolders.Controls.Add(Me.txtFolderPal16)
        Me.tpFolders.Controls.Add(Me.txtFolderPal8)
        Me.tpFolders.Controls.Add(Me.txtRootFolder)
        Me.tpFolders.Controls.Add(Me.LblColorPal16)
        Me.tpFolders.Controls.Add(Me.LblColorPal8)
        Me.tpFolders.Controls.Add(Me.LblRootFolder)
        Me.tpFolders.Location = New System.Drawing.Point(4, 22)
        Me.tpFolders.Name = "tpFolders"
        Me.tpFolders.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFolders.Size = New System.Drawing.Size(710, 248)
        Me.tpFolders.TabIndex = 1
        Me.tpFolders.Text = "Folders"
        Me.tpFolders.UseVisualStyleBackColor = True
        '
        'BtnBrowsePal16
        '
        Me.BtnBrowsePal16.Location = New System.Drawing.Point(539, 152)
        Me.BtnBrowsePal16.Name = "BtnBrowsePal16"
        Me.BtnBrowsePal16.Size = New System.Drawing.Size(78, 20)
        Me.BtnBrowsePal16.TabIndex = 31
        Me.BtnBrowsePal16.Text = "Browse..."
        Me.BtnBrowsePal16.UseVisualStyleBackColor = True
        Me.BtnBrowsePal16.Visible = False
        '
        'BtnBrowsePal8
        '
        Me.BtnBrowsePal8.Location = New System.Drawing.Point(541, 104)
        Me.BtnBrowsePal8.Name = "BtnBrowsePal8"
        Me.BtnBrowsePal8.Size = New System.Drawing.Size(78, 20)
        Me.BtnBrowsePal8.TabIndex = 30
        Me.BtnBrowsePal8.Text = "Browse..."
        Me.BtnBrowsePal8.UseVisualStyleBackColor = True
        Me.BtnBrowsePal8.Visible = False
        '
        'BtnBrowse
        '
        Me.BtnBrowse.Location = New System.Drawing.Point(539, 30)
        Me.BtnBrowse.Name = "BtnBrowse"
        Me.BtnBrowse.Size = New System.Drawing.Size(78, 20)
        Me.BtnBrowse.TabIndex = 29
        Me.BtnBrowse.Text = "Browse..."
        Me.BtnBrowse.UseVisualStyleBackColor = True
        '
        'txtFolderPal16
        '
        Me.txtFolderPal16.Enabled = False
        Me.txtFolderPal16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFolderPal16.Location = New System.Drawing.Point(27, 153)
        Me.txtFolderPal16.Name = "txtFolderPal16"
        Me.txtFolderPal16.Size = New System.Drawing.Size(506, 20)
        Me.txtFolderPal16.TabIndex = 28
        Me.txtFolderPal16.Visible = False
        '
        'txtFolderPal8
        '
        Me.txtFolderPal8.Enabled = False
        Me.txtFolderPal8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFolderPal8.Location = New System.Drawing.Point(27, 105)
        Me.txtFolderPal8.Name = "txtFolderPal8"
        Me.txtFolderPal8.Size = New System.Drawing.Size(506, 20)
        Me.txtFolderPal8.TabIndex = 26
        Me.txtFolderPal8.Visible = False
        '
        'txtRootFolder
        '
        Me.txtRootFolder.Enabled = False
        Me.txtRootFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRootFolder.Location = New System.Drawing.Point(27, 31)
        Me.txtRootFolder.Name = "txtRootFolder"
        Me.txtRootFolder.Size = New System.Drawing.Size(506, 20)
        Me.txtRootFolder.TabIndex = 24
        '
        'LblColorPal16
        '
        Me.LblColorPal16.AutoSize = True
        Me.LblColorPal16.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.LblColorPal16.Location = New System.Drawing.Point(24, 137)
        Me.LblColorPal16.Name = "LblColorPal16"
        Me.LblColorPal16.Size = New System.Drawing.Size(158, 13)
        Me.LblColorPal16.TabIndex = 27
        Me.LblColorPal16.Text = "Folder with 16-color palettes:"
        Me.LblColorPal16.Visible = False
        '
        'LblColorPal8
        '
        Me.LblColorPal8.AutoSize = True
        Me.LblColorPal8.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.LblColorPal8.Location = New System.Drawing.Point(24, 88)
        Me.LblColorPal8.Name = "LblColorPal8"
        Me.LblColorPal8.Size = New System.Drawing.Size(152, 13)
        Me.LblColorPal8.TabIndex = 25
        Me.LblColorPal8.Text = "Folder with 8-color palettes:"
        Me.LblColorPal8.Visible = False
        '
        'LblRootFolder
        '
        Me.LblRootFolder.AutoSize = True
        Me.LblRootFolder.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRootFolder.Location = New System.Drawing.Point(24, 15)
        Me.LblRootFolder.Name = "LblRootFolder"
        Me.LblRootFolder.Size = New System.Drawing.Size(72, 13)
        Me.LblRootFolder.TabIndex = 23
        Me.LblRootFolder.Text = "Root folder: "
        '
        'TCSettings
        '
        Me.TCSettings.Controls.Add(Me.tpFolders)
        Me.TCSettings.Controls.Add(Me.tpRenderingFrames)
        Me.TCSettings.Controls.Add(Me.tpConversions)
        Me.TCSettings.Controls.Add(Me.tpWriteZT1)
        Me.TCSettings.Controls.Add(Me.tpWritePNG)
        Me.TCSettings.Controls.Add(Me.tpPalette)
        Me.TCSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.TCSettings.Location = New System.Drawing.Point(0, 0)
        Me.TCSettings.Name = "TCSettings"
        Me.TCSettings.SelectedIndex = 0
        Me.TCSettings.Size = New System.Drawing.Size(718, 274)
        Me.TCSettings.TabIndex = 29
        '
        'LblHelp
        '
        Me.LblHelp.AutoSize = True
        Me.LblHelp.Location = New System.Drawing.Point(12, 307)
        Me.LblHelp.Name = "LblHelp"
        Me.LblHelp.Size = New System.Drawing.Size(205, 13)
        Me.LblHelp.TabIndex = 30
        Me.LblHelp.Text = "Move over options to see related help."
        '
        'LblHelpTopic
        '
        Me.LblHelpTopic.AutoSize = True
        Me.LblHelpTopic.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHelpTopic.Location = New System.Drawing.Point(12, 294)
        Me.LblHelpTopic.Name = "LblHelpTopic"
        Me.LblHelpTopic.Size = New System.Drawing.Size(66, 13)
        Me.LblHelpTopic.TabIndex = 31
        Me.LblHelpTopic.Text = "Need help?"
        '
        'FrmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(718, 416)
        Me.Controls.Add(Me.LblHelpTopic)
        Me.Controls.Add(Me.LblHelp)
        Me.Controls.Add(Me.TCSettings)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmSettings"
        Me.Text = "Settings"
        Me.tpPalette.ResumeLayout(False)
        Me.tpPalette.PerformLayout
        Me.tpWritePNG.ResumeLayout(False)
        Me.tpWritePNG.PerformLayout
        Me.tpWriteZT1.ResumeLayout(False)
        Me.tpWriteZT1.PerformLayout
        Me.tpRenderingFrames.ResumeLayout(False)
        Me.tpRenderingFrames.PerformLayout
        CType(Me.numFrameDefaultAnimSpeed, System.ComponentModel.ISupportInitialize).EndInit
        Me.tpConversions.ResumeLayout(False)
        Me.tpConversions.PerformLayout
        CType(Me.NumConvert_PNGStartIndex, System.ComponentModel.ISupportInitialize).EndInit
        Me.tpFolders.ResumeLayout(False)
        Me.tpFolders.PerformLayout
        Me.TCSettings.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout

    End Sub
    Friend WithEvents dlgBrowseFolder As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents tpPalette As TabPage
    Friend WithEvents ChkPalImportPNGForceAddAll As CheckBox
    Friend WithEvents tpWritePNG As TabPage
    Friend WithEvents ChkPNGTransparentBG As CheckBox
    Friend WithEvents LblHowToExportPNG As Label
    Friend WithEvents CboPNGExport_Crop As ComboBox
    Friend WithEvents tpWriteZT1 As TabPage
    Friend WithEvents ChkExportZT1_Ani As CheckBox
    Friend WithEvents ChkExportZT1_AddZTAFBytes As CheckBox
    Friend WithEvents tpRenderingFrames As TabPage
    Friend WithEvents numFrameDefaultAnimSpeed As NumericUpDown
    Friend WithEvents LblDefaultAnimSpeed As Label
    Friend WithEvents ChkRenderFrame_BGGraphic As CheckBox
    Friend WithEvents tpConversions As TabPage
    Friend WithEvents TxtConvert_fileNameDelimiter As TextBox
    Friend WithEvents LblConvert_fileNameDelimiter As Label
    Friend WithEvents ChkConvert_SharedColorPalette As CheckBox
    Friend WithEvents ChkConvert_Overwrite As CheckBox
    Friend WithEvents ChkConvert_DeleteOriginal As CheckBox
    Friend WithEvents LblExportPNG_Index As Label
    Friend WithEvents NumConvert_PNGStartIndex As NumericUpDown
    Friend WithEvents tpFolders As TabPage
    Friend WithEvents BtnBrowsePal16 As Button
    Friend WithEvents BtnBrowsePal8 As Button
    Friend WithEvents BtnBrowse As Button
    Friend WithEvents txtFolderPal16 As TextBox
    Friend WithEvents txtFolderPal8 As TextBox
    Friend WithEvents txtRootFolder As TextBox
    Friend WithEvents LblColorPal16 As Label
    Friend WithEvents LblColorPal8 As Label
    Friend WithEvents LblRootFolder As Label
    Friend WithEvents TCSettings As TabControl
    Friend WithEvents Label3 As Label
    Friend WithEvents LblHelp As Label
    Friend WithEvents LblHelpTopic As Label
End Class
