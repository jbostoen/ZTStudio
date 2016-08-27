<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPal
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
        Me.sBar = New System.Windows.Forms.StatusStrip()
        Me.ssFileName = New System.Windows.Forms.ToolStripStatusLabel()
        Me.dgvPal = New System.Windows.Forms.DataGridView()
        Me.colColor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnUseInMainPal = New System.Windows.Forms.Button()
        Me.sBar.SuspendLayout()
        CType(Me.dgvPal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'sBar
        '
        Me.sBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ssFileName})
        Me.sBar.Location = New System.Drawing.Point(0, 260)
        Me.sBar.Name = "sBar"
        Me.sBar.Size = New System.Drawing.Size(264, 22)
        Me.sBar.TabIndex = 0
        Me.sBar.Text = "StatusStrip1"
        '
        'ssFileName
        '
        Me.ssFileName.Name = "ssFileName"
        Me.ssFileName.Size = New System.Drawing.Size(58, 17)
        Me.ssFileName.Text = "Filename."
        '
        'dgvPal
        '
        Me.dgvPal.AllowUserToAddRows = False
        Me.dgvPal.AllowUserToDeleteRows = False
        Me.dgvPal.AllowUserToResizeColumns = False
        Me.dgvPal.AllowUserToResizeRows = False
        Me.dgvPal.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells
        Me.dgvPal.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colColor})
        Me.dgvPal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPal.Location = New System.Drawing.Point(0, 0)
        Me.dgvPal.Name = "dgvPal"
        Me.dgvPal.RowHeadersWidth = 75
        Me.dgvPal.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvPal.RowTemplate.Height = 20
        Me.dgvPal.Size = New System.Drawing.Size(264, 215)
        Me.dgvPal.TabIndex = 21
        '
        'colColor
        '
        Me.colColor.HeaderText = "Color"
        Me.colColor.Name = "colColor"
        '
        'btnUseInMainPal
        '
        Me.btnUseInMainPal.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnUseInMainPal.Location = New System.Drawing.Point(0, 215)
        Me.btnUseInMainPal.Name = "btnUseInMainPal"
        Me.btnUseInMainPal.Size = New System.Drawing.Size(264, 45)
        Me.btnUseInMainPal.TabIndex = 22
        Me.btnUseInMainPal.Text = "Replace colors in main color palette" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "with the colors above (transparent is ignor" & _
    "ed)"
        Me.btnUseInMainPal.UseVisualStyleBackColor = True
        '
        'frmPal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(264, 282)
        Me.Controls.Add(Me.dgvPal)
        Me.Controls.Add(Me.btnUseInMainPal)
        Me.Controls.Add(Me.sBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(280, 280)
        Me.Name = "frmPal"
        Me.Text = "Color Palette"
        Me.TopMost = True
        Me.sBar.ResumeLayout(False)
        Me.sBar.PerformLayout()
        CType(Me.dgvPal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sBar As System.Windows.Forms.StatusStrip
    Friend WithEvents ssFileName As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents dgvPal As System.Windows.Forms.DataGridView
    Friend WithEvents colColor As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnUseInMainPal As System.Windows.Forms.Button
End Class
