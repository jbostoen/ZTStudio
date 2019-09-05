<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmPal
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
        Me.sBar = New System.Windows.Forms.StatusStrip()
        Me.SsFileName = New System.Windows.Forms.ToolStripStatusLabel()
        Me.DgvPal = New System.Windows.Forms.DataGridView()
        Me.ColColor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnUseInMainPal = New System.Windows.Forms.Button()
        Me.sBar.SuspendLayout()
        CType(Me.DgvPal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'sBar
        '
        Me.sBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SsFileName})
        Me.sBar.Location = New System.Drawing.Point(0, 260)
        Me.sBar.Name = "sBar"
        Me.sBar.Size = New System.Drawing.Size(264, 22)
        Me.sBar.TabIndex = 0
        Me.sBar.Text = "StatusStrip1"
        '
        'SsFileName
        '
        Me.SsFileName.Name = "SsFileName"
        Me.SsFileName.Size = New System.Drawing.Size(55, 17)
        Me.SsFileName.Text = "Filename"
        '
        'DgvPal
        '
        Me.DgvPal.AllowUserToAddRows = False
        Me.DgvPal.AllowUserToDeleteRows = False
        Me.DgvPal.AllowUserToResizeColumns = False
        Me.DgvPal.AllowUserToResizeRows = False
        Me.DgvPal.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells
        Me.DgvPal.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColColor})
        Me.DgvPal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DgvPal.Location = New System.Drawing.Point(0, 0)
        Me.DgvPal.Name = "DgvPal"
        Me.DgvPal.RowHeadersWidth = 75
        Me.DgvPal.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DgvPal.RowTemplate.Height = 20
        Me.DgvPal.Size = New System.Drawing.Size(264, 215)
        Me.DgvPal.TabIndex = 21
        '
        'ColColor
        '
        Me.ColColor.HeaderText = "Color"
        Me.ColColor.Name = "ColColor"
        '
        'BtnUseInMainPal
        '
        Me.BtnUseInMainPal.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.BtnUseInMainPal.Location = New System.Drawing.Point(0, 215)
        Me.BtnUseInMainPal.Name = "BtnUseInMainPal"
        Me.BtnUseInMainPal.Size = New System.Drawing.Size(264, 45)
        Me.BtnUseInMainPal.TabIndex = 22
        Me.BtnUseInMainPal.Text = "Replace colors in main color palette" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "with the colors above (transparent is ignor" &
    "ed)"
        Me.BtnUseInMainPal.UseVisualStyleBackColor = True
        '
        'FrmPal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(264, 282)
        Me.Controls.Add(Me.DgvPal)
        Me.Controls.Add(Me.BtnUseInMainPal)
        Me.Controls.Add(Me.sBar)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(280, 280)
        Me.Name = "FrmPal"
        Me.Text = "Color Palette"
        Me.TopMost = True
        Me.sBar.ResumeLayout(False)
        Me.sBar.PerformLayout()
        CType(Me.DgvPal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sBar As System.Windows.Forms.StatusStrip
    Friend WithEvents SsFileName As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents DgvPal As System.Windows.Forms.DataGridView
    Friend WithEvents BtnUseInMainPal As System.Windows.Forms.Button
    Friend WithEvents ColColor As DataGridViewTextBoxColumn
End Class
