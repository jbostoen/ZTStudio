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
        Me.components = New System.ComponentModel.Container()
        Me.dlgBrowseFolder = New System.Windows.Forms.FolderBrowserDialog()
        Me.tpPalette = New System.Windows.Forms.TabPage()
        Me.chkPalImportPNGForceAddAll = New System.Windows.Forms.CheckBox()
        Me.tpWritePNG = New System.Windows.Forms.TabPage()
        Me.chkPNGTransparentBG = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboPNGExport_Crop = New System.Windows.Forms.ComboBox()
        Me.tpWriteZT1 = New System.Windows.Forms.TabPage()
        Me.chkExportZT1_Ani = New System.Windows.Forms.CheckBox()
        Me.chkExportZT1_AddZTAFBytes = New System.Windows.Forms.CheckBox()
        Me.tpRenderingFrames = New System.Windows.Forms.TabPage()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.numFrameDefaultAnimSpeed = New System.Windows.Forms.NumericUpDown()
        Me.lblDefaultAnimSpeed = New System.Windows.Forms.Label()
        Me.chkRenderFrame_BGGraphic = New System.Windows.Forms.CheckBox()
        Me.chkRenderFrame_RenderExtraFrame = New System.Windows.Forms.CheckBox()
        Me.tpConversions = New System.Windows.Forms.TabPage()
        Me.txtConvert_fileNameDelimiter = New System.Windows.Forms.TextBox()
        Me.lblConvert_fileNameDelimiter = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkConvert_SharedColorPalette = New System.Windows.Forms.CheckBox()
        Me.chkConvert_Overwrite = New System.Windows.Forms.CheckBox()
        Me.chkConvert_DeleteOriginal = New System.Windows.Forms.CheckBox()
        Me.lblExportPNG_Index = New System.Windows.Forms.Label()
        Me.numConvert_PNGStartIndex = New System.Windows.Forms.NumericUpDown()
        Me.tpFolders = New System.Windows.Forms.TabPage()
        Me.btnBrowsePal16 = New System.Windows.Forms.Button()
        Me.btnBrowsePal8 = New System.Windows.Forms.Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtFolderPal16 = New System.Windows.Forms.TextBox()
        Me.txtFolderPal8 = New System.Windows.Forms.TextBox()
        Me.txtRootFolder = New System.Windows.Forms.TextBox()
        Me.lblColorPal16 = New System.Windows.Forms.Label()
        Me.lblPalette8 = New System.Windows.Forms.Label()
        Me.LblRootFolder = New System.Windows.Forms.Label()
        Me.TCSettings = New System.Windows.Forms.TabControl()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tpPalette.SuspendLayout()
        Me.tpWritePNG.SuspendLayout()
        Me.tpWriteZT1.SuspendLayout()
        Me.tpRenderingFrames.SuspendLayout()
        CType(Me.numFrameDefaultAnimSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpConversions.SuspendLayout()
        CType(Me.numConvert_PNGStartIndex, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpFolders.SuspendLayout()
        Me.TCSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'tpPalette
        '
        Me.tpPalette.Controls.Add(Me.chkPalImportPNGForceAddAll)
        Me.tpPalette.Location = New System.Drawing.Point(4, 22)
        Me.tpPalette.Name = "tpPalette"
        Me.tpPalette.Size = New System.Drawing.Size(710, 325)
        Me.tpPalette.TabIndex = 6
        Me.tpPalette.Text = "Color palettes"
        Me.tpPalette.UseVisualStyleBackColor = True
        '
        'chkPalImportPNGForceAddAll
        '
        Me.chkPalImportPNGForceAddAll.AutoSize = True
        Me.chkPalImportPNGForceAddAll.Location = New System.Drawing.Point(24, 15)
        Me.chkPalImportPNGForceAddAll.Name = "chkPalImportPNGForceAddAll"
        Me.chkPalImportPNGForceAddAll.Size = New System.Drawing.Size(344, 30)
        Me.chkPalImportPNGForceAddAll.TabIndex = 23
        Me.chkPalImportPNGForceAddAll.Text = "Add all colors (even identical) when importing from .PNG files " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "( Recommended af" &
    "ter recolors )"
        Me.chkPalImportPNGForceAddAll.UseVisualStyleBackColor = True
        '
        'tpWritePNG
        '
        Me.tpWritePNG.Controls.Add(Me.chkPNGTransparentBG)
        Me.tpWritePNG.Controls.Add(Me.Label1)
        Me.tpWritePNG.Controls.Add(Me.cboPNGExport_Crop)
        Me.tpWritePNG.Location = New System.Drawing.Point(4, 22)
        Me.tpWritePNG.Name = "tpWritePNG"
        Me.tpWritePNG.Padding = New System.Windows.Forms.Padding(3)
        Me.tpWritePNG.Size = New System.Drawing.Size(710, 325)
        Me.tpWritePNG.TabIndex = 4
        Me.tpWritePNG.Text = "Export as PNG"
        Me.tpWritePNG.UseVisualStyleBackColor = True
        '
        'chkPNGTransparentBG
        '
        Me.chkPNGTransparentBG.AutoSize = True
        Me.chkPNGTransparentBG.Location = New System.Drawing.Point(24, 118)
        Me.chkPNGTransparentBG.Name = "chkPNGTransparentBG"
        Me.chkPNGTransparentBG.Size = New System.Drawing.Size(481, 17)
        Me.chkPNGTransparentBG.TabIndex = 15
        Me.chkPNGTransparentBG.Text = "Use transparent background with alphachannel (instead of ZT Studio background col" &
    "or)"
        Me.chkPNGTransparentBG.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(211, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Recommended option: keep canvas size"
        '
        'cboPNGExport_Crop
        '
        Me.cboPNGExport_Crop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPNGExport_Crop.FormattingEnabled = True
        Me.cboPNGExport_Crop.Location = New System.Drawing.Point(24, 15)
        Me.cboPNGExport_Crop.Name = "cboPNGExport_Crop"
        Me.cboPNGExport_Crop.Size = New System.Drawing.Size(520, 21)
        Me.cboPNGExport_Crop.TabIndex = 13
        '
        'tpWriteZT1
        '
        Me.tpWriteZT1.Controls.Add(Me.chkExportZT1_Ani)
        Me.tpWriteZT1.Controls.Add(Me.chkExportZT1_AddZTAFBytes)
        Me.tpWriteZT1.Location = New System.Drawing.Point(4, 22)
        Me.tpWriteZT1.Name = "tpWriteZT1"
        Me.tpWriteZT1.Padding = New System.Windows.Forms.Padding(3)
        Me.tpWriteZT1.Size = New System.Drawing.Size(710, 325)
        Me.tpWriteZT1.TabIndex = 3
        Me.tpWriteZT1.Text = "Saving as ZT1 Graphic"
        Me.tpWriteZT1.UseVisualStyleBackColor = True
        '
        'chkExportZT1_Ani
        '
        Me.chkExportZT1_Ani.AutoSize = True
        Me.chkExportZT1_Ani.Checked = True
        Me.chkExportZT1_Ani.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkExportZT1_Ani.Location = New System.Drawing.Point(24, 38)
        Me.chkExportZT1_Ani.Name = "chkExportZT1_Ani"
        Me.chkExportZT1_Ani.Size = New System.Drawing.Size(142, 17)
        Me.chkExportZT1_Ani.TabIndex = 29
        Me.chkExportZT1_Ani.Text = "Create/update .ani-file"
        Me.chkExportZT1_Ani.UseVisualStyleBackColor = True
        '
        'chkExportZT1_AddZTAFBytes
        '
        Me.chkExportZT1_AddZTAFBytes.AutoSize = True
        Me.chkExportZT1_AddZTAFBytes.Location = New System.Drawing.Point(24, 15)
        Me.chkExportZT1_AddZTAFBytes.Name = "chkExportZT1_AddZTAFBytes"
        Me.chkExportZT1_AddZTAFBytes.Size = New System.Drawing.Size(338, 17)
        Me.chkExportZT1_AddZTAFBytes.TabIndex = 28
        Me.chkExportZT1_AddZTAFBytes.Text = "Add ""ZT Animation File""-bytes even if there's no ""extra frame"""
        Me.chkExportZT1_AddZTAFBytes.UseVisualStyleBackColor = True
        '
        'tpRenderingFrames
        '
        Me.tpRenderingFrames.Controls.Add(Me.Label3)
        Me.tpRenderingFrames.Controls.Add(Me.numFrameDefaultAnimSpeed)
        Me.tpRenderingFrames.Controls.Add(Me.lblDefaultAnimSpeed)
        Me.tpRenderingFrames.Controls.Add(Me.chkRenderFrame_BGGraphic)
        Me.tpRenderingFrames.Controls.Add(Me.chkRenderFrame_RenderExtraFrame)
        Me.tpRenderingFrames.Location = New System.Drawing.Point(4, 22)
        Me.tpRenderingFrames.Name = "tpRenderingFrames"
        Me.tpRenderingFrames.Padding = New System.Windows.Forms.Padding(3)
        Me.tpRenderingFrames.Size = New System.Drawing.Size(710, 325)
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
        'lblDefaultAnimSpeed
        '
        Me.lblDefaultAnimSpeed.AutoSize = True
        Me.lblDefaultAnimSpeed.Location = New System.Drawing.Point(21, 137)
        Me.lblDefaultAnimSpeed.Name = "lblDefaultAnimSpeed"
        Me.lblDefaultAnimSpeed.Size = New System.Drawing.Size(137, 13)
        Me.lblDefaultAnimSpeed.TabIndex = 34
        Me.lblDefaultAnimSpeed.Text = "Default animation speed:"
        '
        'chkRenderFrame_BGGraphic
        '
        Me.chkRenderFrame_BGGraphic.AutoSize = True
        Me.chkRenderFrame_BGGraphic.Checked = True
        Me.chkRenderFrame_BGGraphic.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRenderFrame_BGGraphic.Location = New System.Drawing.Point(24, 79)
        Me.chkRenderFrame_BGGraphic.Name = "chkRenderFrame_BGGraphic"
        Me.chkRenderFrame_BGGraphic.Size = New System.Drawing.Size(608, 17)
        Me.chkRenderFrame_BGGraphic.TabIndex = 17
        Me.chkRenderFrame_BGGraphic.Text = "Render chosen background ZT1 Graphic (e.g. main graphic = Orang utan swinging, ba" &
    "ckground: rope swing toy)"
        Me.chkRenderFrame_BGGraphic.UseVisualStyleBackColor = True
        '
        'chkRenderFrame_RenderExtraFrame
        '
        Me.chkRenderFrame_RenderExtraFrame.AutoSize = True
        Me.chkRenderFrame_RenderExtraFrame.Checked = True
        Me.chkRenderFrame_RenderExtraFrame.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRenderFrame_RenderExtraFrame.Location = New System.Drawing.Point(24, 56)
        Me.chkRenderFrame_RenderExtraFrame.Name = "chkRenderFrame_RenderExtraFrame"
        Me.chkRenderFrame_RenderExtraFrame.Size = New System.Drawing.Size(631, 17)
        Me.chkRenderFrame_RenderExtraFrame.TabIndex = 16
        Me.chkRenderFrame_RenderExtraFrame.Text = "Render the extra frame in a graphic in all other frames (in case of a ZTAF-file w" &
    "ith a background frame, e.g. restaurant)"
        Me.chkRenderFrame_RenderExtraFrame.UseVisualStyleBackColor = True
        '
        'tpConversions
        '
        Me.tpConversions.Controls.Add(Me.txtConvert_fileNameDelimiter)
        Me.tpConversions.Controls.Add(Me.lblConvert_fileNameDelimiter)
        Me.tpConversions.Controls.Add(Me.Label2)
        Me.tpConversions.Controls.Add(Me.chkConvert_SharedColorPalette)
        Me.tpConversions.Controls.Add(Me.chkConvert_Overwrite)
        Me.tpConversions.Controls.Add(Me.chkConvert_DeleteOriginal)
        Me.tpConversions.Controls.Add(Me.lblExportPNG_Index)
        Me.tpConversions.Controls.Add(Me.numConvert_PNGStartIndex)
        Me.tpConversions.Location = New System.Drawing.Point(4, 22)
        Me.tpConversions.Name = "tpConversions"
        Me.tpConversions.Padding = New System.Windows.Forms.Padding(3)
        Me.tpConversions.Size = New System.Drawing.Size(710, 325)
        Me.tpConversions.TabIndex = 0
        Me.tpConversions.Text = "Converting ZT1 <> PNG"
        Me.tpConversions.UseVisualStyleBackColor = True
        '
        'txtConvert_fileNameDelimiter
        '
        Me.txtConvert_fileNameDelimiter.Location = New System.Drawing.Point(21, 200)
        Me.txtConvert_fileNameDelimiter.Name = "txtConvert_fileNameDelimiter"
        Me.txtConvert_fileNameDelimiter.Size = New System.Drawing.Size(205, 22)
        Me.txtConvert_fileNameDelimiter.TabIndex = 33
        '
        'lblConvert_fileNameDelimiter
        '
        Me.lblConvert_fileNameDelimiter.AutoSize = True
        Me.lblConvert_fileNameDelimiter.Location = New System.Drawing.Point(18, 158)
        Me.lblConvert_fileNameDelimiter.Name = "lblConvert_fileNameDelimiter"
        Me.lblConvert_fileNameDelimiter.Size = New System.Drawing.Size(458, 39)
        Me.lblConvert_fileNameDelimiter.TabIndex = 32
        Me.lblConvert_fileNameDelimiter.Text = "File name delimiter:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This is the character used in filenames, between the name o" &
    "f the graphic and the frame." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "For example, _ is the delimiter in NE_0000.png "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(18, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(116, 13)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Batch conversions:"
        '
        'chkConvert_SharedColorPalette
        '
        Me.chkConvert_SharedColorPalette.AutoSize = True
        Me.chkConvert_SharedColorPalette.Checked = True
        Me.chkConvert_SharedColorPalette.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkConvert_SharedColorPalette.Location = New System.Drawing.Point(21, 106)
        Me.chkConvert_SharedColorPalette.Name = "chkConvert_SharedColorPalette"
        Me.chkConvert_SharedColorPalette.Size = New System.Drawing.Size(440, 30)
        Me.chkConvert_SharedColorPalette.TabIndex = 30
        Me.chkConvert_SharedColorPalette.Text = "Use one shared color palette for each graphic's animations/views - except icons. " &
    "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Caution: max 255 colors shared among all frames!)"
        Me.chkConvert_SharedColorPalette.UseVisualStyleBackColor = True
        '
        'chkConvert_Overwrite
        '
        Me.chkConvert_Overwrite.AutoSize = True
        Me.chkConvert_Overwrite.Checked = True
        Me.chkConvert_Overwrite.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkConvert_Overwrite.Enabled = False
        Me.chkConvert_Overwrite.Location = New System.Drawing.Point(21, 264)
        Me.chkConvert_Overwrite.Name = "chkConvert_Overwrite"
        Me.chkConvert_Overwrite.Size = New System.Drawing.Size(100, 17)
        Me.chkConvert_Overwrite.TabIndex = 29
        Me.chkConvert_Overwrite.Text = "Overwrite files"
        Me.chkConvert_Overwrite.UseVisualStyleBackColor = True
        Me.chkConvert_Overwrite.Visible = False
        '
        'chkConvert_DeleteOriginal
        '
        Me.chkConvert_DeleteOriginal.AutoSize = True
        Me.chkConvert_DeleteOriginal.Checked = True
        Me.chkConvert_DeleteOriginal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkConvert_DeleteOriginal.Location = New System.Drawing.Point(21, 70)
        Me.chkConvert_DeleteOriginal.Name = "chkConvert_DeleteOriginal"
        Me.chkConvert_DeleteOriginal.Size = New System.Drawing.Size(358, 30)
        Me.chkConvert_DeleteOriginal.TabIndex = 28
        Me.chkConvert_DeleteOriginal.Text = "Delete the source file after conversion. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(e.g. converting from .PNG-files to ZT" &
    "1: .PNG files will be deleted)"
        Me.chkConvert_DeleteOriginal.UseVisualStyleBackColor = True
        '
        'lblExportPNG_Index
        '
        Me.lblExportPNG_Index.AutoSize = True
        Me.lblExportPNG_Index.Location = New System.Drawing.Point(75, 33)
        Me.lblExportPNG_Index.Name = "lblExportPNG_Index"
        Me.lblExportPNG_Index.Size = New System.Drawing.Size(354, 13)
        Me.lblExportPNG_Index.TabIndex = 27
        Me.lblExportPNG_Index.Text = "Start numbering of .PNG-file series at either 0 (index) or 1 (frame #1)"
        '
        'numConvert_PNGStartIndex
        '
        Me.numConvert_PNGStartIndex.Location = New System.Drawing.Point(21, 31)
        Me.numConvert_PNGStartIndex.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numConvert_PNGStartIndex.Name = "numConvert_PNGStartIndex"
        Me.numConvert_PNGStartIndex.Size = New System.Drawing.Size(48, 22)
        Me.numConvert_PNGStartIndex.TabIndex = 26
        '
        'tpFolders
        '
        Me.tpFolders.Controls.Add(Me.btnBrowsePal16)
        Me.tpFolders.Controls.Add(Me.btnBrowsePal8)
        Me.tpFolders.Controls.Add(Me.btnBrowse)
        Me.tpFolders.Controls.Add(Me.txtFolderPal16)
        Me.tpFolders.Controls.Add(Me.txtFolderPal8)
        Me.tpFolders.Controls.Add(Me.txtRootFolder)
        Me.tpFolders.Controls.Add(Me.lblColorPal16)
        Me.tpFolders.Controls.Add(Me.lblPalette8)
        Me.tpFolders.Controls.Add(Me.LblRootFolder)
        Me.tpFolders.Location = New System.Drawing.Point(4, 22)
        Me.tpFolders.Name = "tpFolders"
        Me.tpFolders.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFolders.Size = New System.Drawing.Size(710, 325)
        Me.tpFolders.TabIndex = 1
        Me.tpFolders.Text = "Folders"
        Me.tpFolders.UseVisualStyleBackColor = True
        '
        'btnBrowsePal16
        '
        Me.btnBrowsePal16.Location = New System.Drawing.Point(541, 168)
        Me.btnBrowsePal16.Name = "btnBrowsePal16"
        Me.btnBrowsePal16.Size = New System.Drawing.Size(78, 20)
        Me.btnBrowsePal16.TabIndex = 31
        Me.btnBrowsePal16.Text = "Browse..."
        Me.btnBrowsePal16.UseVisualStyleBackColor = True
        Me.btnBrowsePal16.Visible = False
        '
        'btnBrowsePal8
        '
        Me.btnBrowsePal8.Location = New System.Drawing.Point(541, 104)
        Me.btnBrowsePal8.Name = "btnBrowsePal8"
        Me.btnBrowsePal8.Size = New System.Drawing.Size(78, 20)
        Me.btnBrowsePal8.TabIndex = 30
        Me.btnBrowsePal8.Text = "Browse..."
        Me.btnBrowsePal8.UseVisualStyleBackColor = True
        Me.btnBrowsePal8.Visible = False
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(539, 30)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(78, 20)
        Me.btnBrowse.TabIndex = 29
        Me.btnBrowse.Text = "Browse..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtFolderPal16
        '
        Me.txtFolderPal16.Enabled = False
        Me.txtFolderPal16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFolderPal16.Location = New System.Drawing.Point(27, 169)
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
        'lblColorPal16
        '
        Me.lblColorPal16.AutoSize = True
        Me.lblColorPal16.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblColorPal16.Location = New System.Drawing.Point(24, 152)
        Me.lblColorPal16.Name = "lblColorPal16"
        Me.lblColorPal16.Size = New System.Drawing.Size(158, 13)
        Me.lblColorPal16.TabIndex = 27
        Me.lblColorPal16.Text = "Folder with 16-color palettes:"
        Me.lblColorPal16.Visible = False
        '
        'lblPalette8
        '
        Me.lblPalette8.AutoSize = True
        Me.lblPalette8.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblPalette8.Location = New System.Drawing.Point(24, 88)
        Me.lblPalette8.Name = "lblPalette8"
        Me.lblPalette8.Size = New System.Drawing.Size(152, 13)
        Me.lblPalette8.TabIndex = 25
        Me.lblPalette8.Text = "Folder with 8-color palettes:"
        Me.lblPalette8.Visible = False
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
        Me.TCSettings.Size = New System.Drawing.Size(718, 351)
        Me.TCSettings.TabIndex = 29
        '
        'FrmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(718, 348)
        Me.Controls.Add(Me.TCSettings)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmSettings"
        Me.Text = "Settings"
        Me.tpPalette.ResumeLayout(False)
        Me.tpPalette.PerformLayout()
        Me.tpWritePNG.ResumeLayout(False)
        Me.tpWritePNG.PerformLayout()
        Me.tpWriteZT1.ResumeLayout(False)
        Me.tpWriteZT1.PerformLayout()
        Me.tpRenderingFrames.ResumeLayout(False)
        Me.tpRenderingFrames.PerformLayout()
        CType(Me.numFrameDefaultAnimSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpConversions.ResumeLayout(False)
        Me.tpConversions.PerformLayout()
        CType(Me.numConvert_PNGStartIndex, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpFolders.ResumeLayout(False)
        Me.tpFolders.PerformLayout()
        Me.TCSettings.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dlgBrowseFolder As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents tpPalette As TabPage
    Friend WithEvents chkPalImportPNGForceAddAll As CheckBox
    Friend WithEvents tpWritePNG As TabPage
    Friend WithEvents chkPNGTransparentBG As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cboPNGExport_Crop As ComboBox
    Friend WithEvents tpWriteZT1 As TabPage
    Friend WithEvents chkExportZT1_Ani As CheckBox
    Friend WithEvents chkExportZT1_AddZTAFBytes As CheckBox
    Friend WithEvents tpRenderingFrames As TabPage
    Friend WithEvents numFrameDefaultAnimSpeed As NumericUpDown
    Friend WithEvents lblDefaultAnimSpeed As Label
    Friend WithEvents chkRenderFrame_BGGraphic As CheckBox
    Friend WithEvents chkRenderFrame_RenderExtraFrame As CheckBox
    Friend WithEvents tpConversions As TabPage
    Friend WithEvents txtConvert_fileNameDelimiter As TextBox
    Friend WithEvents lblConvert_fileNameDelimiter As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents chkConvert_SharedColorPalette As CheckBox
    Friend WithEvents chkConvert_Overwrite As CheckBox
    Friend WithEvents chkConvert_DeleteOriginal As CheckBox
    Friend WithEvents lblExportPNG_Index As Label
    Friend WithEvents numConvert_PNGStartIndex As NumericUpDown
    Friend WithEvents tpFolders As TabPage
    Friend WithEvents btnBrowsePal16 As Button
    Friend WithEvents btnBrowsePal8 As Button
    Friend WithEvents btnBrowse As Button
    Friend WithEvents txtFolderPal16 As TextBox
    Friend WithEvents txtFolderPal8 As TextBox
    Friend WithEvents txtRootFolder As TextBox
    Friend WithEvents lblColorPal16 As Label
    Friend WithEvents lblPalette8 As Label
    Friend WithEvents LblRootFolder As Label
    Friend WithEvents TCSettings As TabControl
    Friend WithEvents Label3 As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
