<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
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
        Me.dlgBrowseFolder = New System.Windows.Forms.FolderBrowserDialog()
        Me.tpEditGraphics = New System.Windows.Forms.TabPage()
        Me.chkEditor_Frame_Offsets_SingleFrame = New System.Windows.Forms.CheckBox()
        Me.tpWritePNG = New System.Windows.Forms.TabPage()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboPNGExport_Crop = New System.Windows.Forms.ComboBox()
        Me.tpWriteZT1 = New System.Windows.Forms.TabPage()
        Me.chkExportZT1_Ani = New System.Windows.Forms.CheckBox()
        Me.chkExportZT1_AddZTAFBytes = New System.Windows.Forms.CheckBox()
        Me.tpRenderingFrames = New System.Windows.Forms.TabPage()
        Me.chkRenderFrame_BGGraphic = New System.Windows.Forms.CheckBox()
        Me.chkRenderFrame_RenderExtraFrame = New System.Windows.Forms.CheckBox()
        Me.tpFolders = New System.Windows.Forms.TabPage()
        Me.btnBrowsePal16 = New System.Windows.Forms.Button()
        Me.btnBrowsePal8 = New System.Windows.Forms.Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtFolderPal16 = New System.Windows.Forms.TextBox()
        Me.txtFolderPal8 = New System.Windows.Forms.TextBox()
        Me.txtRootFolder = New System.Windows.Forms.TextBox()
        Me.lblColorPal16 = New System.Windows.Forms.Label()
        Me.lblPalette8 = New System.Windows.Forms.Label()
        Me.lblWarnings = New System.Windows.Forms.Label()
        Me.tpConversions = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkConvert_SharedColorPalette = New System.Windows.Forms.CheckBox()
        Me.chkConvert_Overwrite = New System.Windows.Forms.CheckBox()
        Me.chkConvert_DeleteOriginal = New System.Windows.Forms.CheckBox()
        Me.lblExportPNG_Index = New System.Windows.Forms.Label()
        Me.numConvert_PNGStartIndex = New System.Windows.Forms.NumericUpDown()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.lblConvert_FileNamedelimiter = New System.Windows.Forms.Label()
        Me.txtConvert_FileNamedelimiter = New System.Windows.Forms.TextBox()
        Me.tpEditGraphics.SuspendLayout()
        Me.tpWritePNG.SuspendLayout()
        Me.tpWriteZT1.SuspendLayout()
        Me.tpRenderingFrames.SuspendLayout()
        Me.tpFolders.SuspendLayout()
        Me.tpConversions.SuspendLayout()
        CType(Me.numConvert_PNGStartIndex, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tpEditGraphics
        '
        Me.tpEditGraphics.Controls.Add(Me.chkEditor_Frame_Offsets_SingleFrame)
        Me.tpEditGraphics.Location = New System.Drawing.Point(4, 22)
        Me.tpEditGraphics.Name = "tpEditGraphics"
        Me.tpEditGraphics.Padding = New System.Windows.Forms.Padding(3)
        Me.tpEditGraphics.Size = New System.Drawing.Size(710, 267)
        Me.tpEditGraphics.TabIndex = 5
        Me.tpEditGraphics.Text = "Editing graphics"
        Me.tpEditGraphics.UseVisualStyleBackColor = True
        '
        'chkEditor_Frame_Offsets_SingleFrame
        '
        Me.chkEditor_Frame_Offsets_SingleFrame.AutoSize = True
        Me.chkEditor_Frame_Offsets_SingleFrame.Location = New System.Drawing.Point(26, 6)
        Me.chkEditor_Frame_Offsets_SingleFrame.Name = "chkEditor_Frame_Offsets_SingleFrame"
        Me.chkEditor_Frame_Offsets_SingleFrame.Size = New System.Drawing.Size(491, 17)
        Me.chkEditor_Frame_Offsets_SingleFrame.TabIndex = 22
        Me.chkEditor_Frame_Offsets_SingleFrame.Text = "Adjust offsets of a single frame (instead of all frames in the graphic) when ""rot" & _
    "ation fixing"""
        Me.chkEditor_Frame_Offsets_SingleFrame.UseVisualStyleBackColor = True
        '
        'tpWritePNG
        '
        Me.tpWritePNG.Controls.Add(Me.Label1)
        Me.tpWritePNG.Controls.Add(Me.cboPNGExport_Crop)
        Me.tpWritePNG.Location = New System.Drawing.Point(4, 22)
        Me.tpWritePNG.Name = "tpWritePNG"
        Me.tpWritePNG.Padding = New System.Windows.Forms.Padding(3)
        Me.tpWritePNG.Size = New System.Drawing.Size(710, 267)
        Me.tpWritePNG.TabIndex = 4
        Me.tpWritePNG.Text = "Writing PNG Graphics"
        Me.tpWritePNG.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(211, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Recommended option: keep canvas size"
        '
        'cboPNGExport_Crop
        '
        Me.cboPNGExport_Crop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPNGExport_Crop.FormattingEnabled = True
        Me.cboPNGExport_Crop.Location = New System.Drawing.Point(26, 6)
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
        Me.tpWriteZT1.Size = New System.Drawing.Size(710, 267)
        Me.tpWriteZT1.TabIndex = 3
        Me.tpWriteZT1.Text = "Writing ZT1 Graphics"
        Me.tpWriteZT1.UseVisualStyleBackColor = True
        '
        'chkExportZT1_Ani
        '
        Me.chkExportZT1_Ani.AutoSize = True
        Me.chkExportZT1_Ani.Checked = True
        Me.chkExportZT1_Ani.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkExportZT1_Ani.Location = New System.Drawing.Point(26, 29)
        Me.chkExportZT1_Ani.Name = "chkExportZT1_Ani"
        Me.chkExportZT1_Ani.Size = New System.Drawing.Size(142, 17)
        Me.chkExportZT1_Ani.TabIndex = 29
        Me.chkExportZT1_Ani.Text = "Create/update .ani-file"
        Me.chkExportZT1_Ani.UseVisualStyleBackColor = True
        '
        'chkExportZT1_AddZTAFBytes
        '
        Me.chkExportZT1_AddZTAFBytes.AutoSize = True
        Me.chkExportZT1_AddZTAFBytes.Location = New System.Drawing.Point(26, 6)
        Me.chkExportZT1_AddZTAFBytes.Name = "chkExportZT1_AddZTAFBytes"
        Me.chkExportZT1_AddZTAFBytes.Size = New System.Drawing.Size(337, 17)
        Me.chkExportZT1_AddZTAFBytes.TabIndex = 28
        Me.chkExportZT1_AddZTAFBytes.Text = "Add ""ZT Animation File""-bytes even if there's no ""extra frame"""
        Me.chkExportZT1_AddZTAFBytes.UseVisualStyleBackColor = True
        '
        'tpRenderingFrames
        '
        Me.tpRenderingFrames.Controls.Add(Me.chkRenderFrame_BGGraphic)
        Me.tpRenderingFrames.Controls.Add(Me.chkRenderFrame_RenderExtraFrame)
        Me.tpRenderingFrames.Location = New System.Drawing.Point(4, 22)
        Me.tpRenderingFrames.Name = "tpRenderingFrames"
        Me.tpRenderingFrames.Padding = New System.Windows.Forms.Padding(3)
        Me.tpRenderingFrames.Size = New System.Drawing.Size(710, 267)
        Me.tpRenderingFrames.TabIndex = 2
        Me.tpRenderingFrames.Text = "Rendering frames"
        Me.tpRenderingFrames.UseVisualStyleBackColor = True
        '
        'chkRenderFrame_BGGraphic
        '
        Me.chkRenderFrame_BGGraphic.AutoSize = True
        Me.chkRenderFrame_BGGraphic.Checked = True
        Me.chkRenderFrame_BGGraphic.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRenderFrame_BGGraphic.Location = New System.Drawing.Point(26, 29)
        Me.chkRenderFrame_BGGraphic.Name = "chkRenderFrame_BGGraphic"
        Me.chkRenderFrame_BGGraphic.Size = New System.Drawing.Size(607, 17)
        Me.chkRenderFrame_BGGraphic.TabIndex = 17
        Me.chkRenderFrame_BGGraphic.Text = "Render chosen background ZT1 Graphic (e.g. main graphic = Orang utan swinging, ba" & _
    "ckground: rope swing toy)"
        Me.chkRenderFrame_BGGraphic.UseVisualStyleBackColor = True
        '
        'chkRenderFrame_RenderExtraFrame
        '
        Me.chkRenderFrame_RenderExtraFrame.AutoSize = True
        Me.chkRenderFrame_RenderExtraFrame.Checked = True
        Me.chkRenderFrame_RenderExtraFrame.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRenderFrame_RenderExtraFrame.Location = New System.Drawing.Point(26, 6)
        Me.chkRenderFrame_RenderExtraFrame.Name = "chkRenderFrame_RenderExtraFrame"
        Me.chkRenderFrame_RenderExtraFrame.Size = New System.Drawing.Size(630, 17)
        Me.chkRenderFrame_RenderExtraFrame.TabIndex = 16
        Me.chkRenderFrame_RenderExtraFrame.Text = "Render the extra frame in a graphic in all other frames (in case of a ZTAF-file w" & _
    "ith a background frame, e.g. restaurant)"
        Me.chkRenderFrame_RenderExtraFrame.UseVisualStyleBackColor = True
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
        Me.tpFolders.Controls.Add(Me.lblWarnings)
        Me.tpFolders.Location = New System.Drawing.Point(4, 22)
        Me.tpFolders.Name = "tpFolders"
        Me.tpFolders.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFolders.Size = New System.Drawing.Size(710, 267)
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
        Me.btnBrowse.Location = New System.Drawing.Point(541, 40)
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
        Me.txtFolderPal16.Location = New System.Drawing.Point(29, 168)
        Me.txtFolderPal16.Name = "txtFolderPal16"
        Me.txtFolderPal16.Size = New System.Drawing.Size(506, 20)
        Me.txtFolderPal16.TabIndex = 28
        Me.txtFolderPal16.Visible = False
        '
        'txtFolderPal8
        '
        Me.txtFolderPal8.Enabled = False
        Me.txtFolderPal8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFolderPal8.Location = New System.Drawing.Point(29, 104)
        Me.txtFolderPal8.Name = "txtFolderPal8"
        Me.txtFolderPal8.Size = New System.Drawing.Size(506, 20)
        Me.txtFolderPal8.TabIndex = 26
        Me.txtFolderPal8.Visible = False
        '
        'txtRootFolder
        '
        Me.txtRootFolder.Enabled = False
        Me.txtRootFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRootFolder.Location = New System.Drawing.Point(29, 40)
        Me.txtRootFolder.Name = "txtRootFolder"
        Me.txtRootFolder.Size = New System.Drawing.Size(506, 20)
        Me.txtRootFolder.TabIndex = 24
        '
        'lblColorPal16
        '
        Me.lblColorPal16.AutoSize = True
        Me.lblColorPal16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblColorPal16.Location = New System.Drawing.Point(26, 152)
        Me.lblColorPal16.Name = "lblColorPal16"
        Me.lblColorPal16.Size = New System.Drawing.Size(142, 13)
        Me.lblColorPal16.TabIndex = 27
        Me.lblColorPal16.Text = "Folder with 16-color palettes:"
        Me.lblColorPal16.Visible = False
        '
        'lblPalette8
        '
        Me.lblPalette8.AutoSize = True
        Me.lblPalette8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPalette8.Location = New System.Drawing.Point(26, 88)
        Me.lblPalette8.Name = "lblPalette8"
        Me.lblPalette8.Size = New System.Drawing.Size(136, 13)
        Me.lblPalette8.TabIndex = 25
        Me.lblPalette8.Text = "Folder with 8-color palettes:"
        Me.lblPalette8.Visible = False
        '
        'lblWarnings
        '
        Me.lblWarnings.AutoSize = True
        Me.lblWarnings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWarnings.Location = New System.Drawing.Point(26, 24)
        Me.lblWarnings.Name = "lblWarnings"
        Me.lblWarnings.Size = New System.Drawing.Size(65, 13)
        Me.lblWarnings.TabIndex = 23
        Me.lblWarnings.Text = "Root folder: "
        '
        'tpConversions
        '
        Me.tpConversions.Controls.Add(Me.txtConvert_FileNamedelimiter)
        Me.tpConversions.Controls.Add(Me.lblConvert_FileNamedelimiter)
        Me.tpConversions.Controls.Add(Me.Label2)
        Me.tpConversions.Controls.Add(Me.chkConvert_SharedColorPalette)
        Me.tpConversions.Controls.Add(Me.chkConvert_Overwrite)
        Me.tpConversions.Controls.Add(Me.chkConvert_DeleteOriginal)
        Me.tpConversions.Controls.Add(Me.lblExportPNG_Index)
        Me.tpConversions.Controls.Add(Me.numConvert_PNGStartIndex)
        Me.tpConversions.Location = New System.Drawing.Point(4, 22)
        Me.tpConversions.Name = "tpConversions"
        Me.tpConversions.Padding = New System.Windows.Forms.Padding(3)
        Me.tpConversions.Size = New System.Drawing.Size(710, 267)
        Me.tpConversions.TabIndex = 0
        Me.tpConversions.Text = "Conversions"
        Me.tpConversions.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(23, 70)
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
        Me.chkConvert_SharedColorPalette.Location = New System.Drawing.Point(26, 163)
        Me.chkConvert_SharedColorPalette.Name = "chkConvert_SharedColorPalette"
        Me.chkConvert_SharedColorPalette.Size = New System.Drawing.Size(440, 30)
        Me.chkConvert_SharedColorPalette.TabIndex = 30
        Me.chkConvert_SharedColorPalette.Text = "Use one shared color palette for each graphic's animations/views - except icons. " & _
    "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Caution: max 255 colors shared among all frames!)"
        Me.chkConvert_SharedColorPalette.UseVisualStyleBackColor = True
        '
        'chkConvert_Overwrite
        '
        Me.chkConvert_Overwrite.AutoSize = True
        Me.chkConvert_Overwrite.Checked = True
        Me.chkConvert_Overwrite.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkConvert_Overwrite.Enabled = False
        Me.chkConvert_Overwrite.Location = New System.Drawing.Point(26, 29)
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
        Me.chkConvert_DeleteOriginal.Location = New System.Drawing.Point(26, 127)
        Me.chkConvert_DeleteOriginal.Name = "chkConvert_DeleteOriginal"
        Me.chkConvert_DeleteOriginal.Size = New System.Drawing.Size(357, 30)
        Me.chkConvert_DeleteOriginal.TabIndex = 28
        Me.chkConvert_DeleteOriginal.Text = "Delete the source file after conversion. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(e.g. converting from .PNG-files to ZT" & _
    "1: .PNG files will be deleted)"
        Me.chkConvert_DeleteOriginal.UseVisualStyleBackColor = True
        '
        'lblExportPNG_Index
        '
        Me.lblExportPNG_Index.AutoSize = True
        Me.lblExportPNG_Index.Location = New System.Drawing.Point(80, 88)
        Me.lblExportPNG_Index.Name = "lblExportPNG_Index"
        Me.lblExportPNG_Index.Size = New System.Drawing.Size(354, 13)
        Me.lblExportPNG_Index.TabIndex = 27
        Me.lblExportPNG_Index.Text = "Start numbering of .PNG-file series at either 0 (index) or 1 (frame #1)"
        '
        'numConvert_PNGStartIndex
        '
        Me.numConvert_PNGStartIndex.Location = New System.Drawing.Point(26, 86)
        Me.numConvert_PNGStartIndex.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numConvert_PNGStartIndex.Name = "numConvert_PNGStartIndex"
        Me.numConvert_PNGStartIndex.Size = New System.Drawing.Size(48, 22)
        Me.numConvert_PNGStartIndex.TabIndex = 26
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpFolders)
        Me.TabControl1.Controls.Add(Me.tpConversions)
        Me.TabControl1.Controls.Add(Me.tpRenderingFrames)
        Me.TabControl1.Controls.Add(Me.tpWriteZT1)
        Me.TabControl1.Controls.Add(Me.tpWritePNG)
        Me.TabControl1.Controls.Add(Me.tpEditGraphics)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(718, 293)
        Me.TabControl1.TabIndex = 29
        '
        'lblConvert_FileNamedelimiter
        '
        Me.lblConvert_FileNamedelimiter.AutoSize = True
        Me.lblConvert_FileNamedelimiter.Location = New System.Drawing.Point(27, 213)
        Me.lblConvert_FileNamedelimiter.Name = "lblConvert_FileNamedelimiter"
        Me.lblConvert_FileNamedelimiter.Size = New System.Drawing.Size(110, 13)
        Me.lblConvert_FileNamedelimiter.TabIndex = 32
        Me.lblConvert_FileNamedelimiter.Text = "File name delimiter:"
        '
        'txtConvert_FileNamedelimiter
        '
        Me.txtConvert_FileNamedelimiter.Location = New System.Drawing.Point(29, 231)
        Me.txtConvert_FileNamedelimiter.Name = "txtConvert_FileNamedelimiter"
        Me.txtConvert_FileNamedelimiter.Size = New System.Drawing.Size(205, 22)
        Me.txtConvert_FileNamedelimiter.TabIndex = 33
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(718, 291)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmSettings"
        Me.Text = "Settings"
        Me.tpEditGraphics.ResumeLayout(False)
        Me.tpEditGraphics.PerformLayout()
        Me.tpWritePNG.ResumeLayout(False)
        Me.tpWritePNG.PerformLayout()
        Me.tpWriteZT1.ResumeLayout(False)
        Me.tpWriteZT1.PerformLayout()
        Me.tpRenderingFrames.ResumeLayout(False)
        Me.tpRenderingFrames.PerformLayout()
        Me.tpFolders.ResumeLayout(False)
        Me.tpFolders.PerformLayout()
        Me.tpConversions.ResumeLayout(False)
        Me.tpConversions.PerformLayout()
        CType(Me.numConvert_PNGStartIndex, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dlgBrowseFolder As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents tpEditGraphics As System.Windows.Forms.TabPage
    Friend WithEvents chkEditor_Frame_Offsets_SingleFrame As System.Windows.Forms.CheckBox
    Friend WithEvents tpWritePNG As System.Windows.Forms.TabPage
    Friend WithEvents cboPNGExport_Crop As System.Windows.Forms.ComboBox
    Friend WithEvents tpWriteZT1 As System.Windows.Forms.TabPage
    Friend WithEvents chkExportZT1_Ani As System.Windows.Forms.CheckBox
    Friend WithEvents chkExportZT1_AddZTAFBytes As System.Windows.Forms.CheckBox
    Friend WithEvents tpRenderingFrames As System.Windows.Forms.TabPage
    Friend WithEvents chkRenderFrame_BGGraphic As System.Windows.Forms.CheckBox
    Friend WithEvents chkRenderFrame_RenderExtraFrame As System.Windows.Forms.CheckBox
    Friend WithEvents tpFolders As System.Windows.Forms.TabPage
    Friend WithEvents btnBrowsePal16 As System.Windows.Forms.Button
    Friend WithEvents btnBrowsePal8 As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtFolderPal16 As System.Windows.Forms.TextBox
    Friend WithEvents txtFolderPal8 As System.Windows.Forms.TextBox
    Friend WithEvents txtRootFolder As System.Windows.Forms.TextBox
    Friend WithEvents lblColorPal16 As System.Windows.Forms.Label
    Friend WithEvents lblPalette8 As System.Windows.Forms.Label
    Friend WithEvents lblWarnings As System.Windows.Forms.Label
    Friend WithEvents tpConversions As System.Windows.Forms.TabPage
    Friend WithEvents chkConvert_Overwrite As System.Windows.Forms.CheckBox
    Friend WithEvents chkConvert_DeleteOriginal As System.Windows.Forms.CheckBox
    Friend WithEvents lblExportPNG_Index As System.Windows.Forms.Label
    Friend WithEvents numConvert_PNGStartIndex As System.Windows.Forms.NumericUpDown
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkConvert_SharedColorPalette As System.Windows.Forms.CheckBox
    Friend WithEvents txtConvert_FileNamedelimiter As System.Windows.Forms.TextBox
    Friend WithEvents lblConvert_FileNamedelimiter As System.Windows.Forms.Label
End Class
