<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGIMPRecolor
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
        Me.tbRed = New System.Windows.Forms.TrackBar()
        Me.tbGreen = New System.Windows.Forms.TrackBar()
        Me.tbBlue = New System.Windows.Forms.TrackBar()
        Me.lblRed = New System.Windows.Forms.Label()
        Me.lblGreen = New System.Windows.Forms.Label()
        Me.lblBlue = New System.Windows.Forms.Label()
        Me.lblTones = New System.Windows.Forms.Label()
        Me.cboTones = New System.Windows.Forms.ComboBox()
        Me.lblContrast = New System.Windows.Forms.Label()
        Me.lblBrightness = New System.Windows.Forms.Label()
        Me.tbContrast = New System.Windows.Forms.TrackBar()
        Me.tbBrightness = New System.Windows.Forms.TrackBar()
        Me.btnApply = New System.Windows.Forms.Button()
        CType(Me.tbRed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbGreen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbBlue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbContrast, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbBrightness, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbRed
        '
        Me.tbRed.Location = New System.Drawing.Point(149, 28)
        Me.tbRed.Maximum = 100
        Me.tbRed.Minimum = -100
        Me.tbRed.Name = "tbRed"
        Me.tbRed.Size = New System.Drawing.Size(511, 45)
        Me.tbRed.TabIndex = 0
        Me.tbRed.TickFrequency = 5
        '
        'tbGreen
        '
        Me.tbGreen.Location = New System.Drawing.Point(149, 79)
        Me.tbGreen.Maximum = 100
        Me.tbGreen.Minimum = -100
        Me.tbGreen.Name = "tbGreen"
        Me.tbGreen.Size = New System.Drawing.Size(511, 45)
        Me.tbGreen.TabIndex = 1
        Me.tbGreen.TickFrequency = 5
        '
        'tbBlue
        '
        Me.tbBlue.Location = New System.Drawing.Point(149, 130)
        Me.tbBlue.Maximum = 100
        Me.tbBlue.Minimum = -100
        Me.tbBlue.Name = "tbBlue"
        Me.tbBlue.Size = New System.Drawing.Size(511, 45)
        Me.tbBlue.TabIndex = 2
        Me.tbBlue.TickFrequency = 5
        '
        'lblRed
        '
        Me.lblRed.AutoSize = True
        Me.lblRed.Location = New System.Drawing.Point(32, 28)
        Me.lblRed.Name = "lblRed"
        Me.lblRed.Size = New System.Drawing.Size(27, 13)
        Me.lblRed.TabIndex = 3
        Me.lblRed.Text = "Red"
        '
        'lblGreen
        '
        Me.lblGreen.AutoSize = True
        Me.lblGreen.Location = New System.Drawing.Point(32, 79)
        Me.lblGreen.Name = "lblGreen"
        Me.lblGreen.Size = New System.Drawing.Size(36, 13)
        Me.lblGreen.TabIndex = 4
        Me.lblGreen.Text = "Green"
        '
        'lblBlue
        '
        Me.lblBlue.AutoSize = True
        Me.lblBlue.Location = New System.Drawing.Point(32, 130)
        Me.lblBlue.Name = "lblBlue"
        Me.lblBlue.Size = New System.Drawing.Size(28, 13)
        Me.lblBlue.TabIndex = 5
        Me.lblBlue.Text = "Blue"
        '
        'lblTones
        '
        Me.lblTones.AutoSize = True
        Me.lblTones.Location = New System.Drawing.Point(32, 178)
        Me.lblTones.Name = "lblTones"
        Me.lblTones.Size = New System.Drawing.Size(37, 13)
        Me.lblTones.TabIndex = 6
        Me.lblTones.Text = "Tones"
        '
        'cboTones
        '
        Me.cboTones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTones.FormattingEnabled = True
        Me.cboTones.Items.AddRange(New Object() {"Shadows", "Midtones", "Highlights"})
        Me.cboTones.Location = New System.Drawing.Point(152, 178)
        Me.cboTones.Name = "cboTones"
        Me.cboTones.Size = New System.Drawing.Size(174, 21)
        Me.cboTones.TabIndex = 7
        '
        'lblContrast
        '
        Me.lblContrast.AutoSize = True
        Me.lblContrast.Location = New System.Drawing.Point(32, 285)
        Me.lblContrast.Name = "lblContrast"
        Me.lblContrast.Size = New System.Drawing.Size(46, 13)
        Me.lblContrast.TabIndex = 11
        Me.lblContrast.Text = "Contrast"
        '
        'lblBrightness
        '
        Me.lblBrightness.AutoSize = True
        Me.lblBrightness.Location = New System.Drawing.Point(32, 234)
        Me.lblBrightness.Name = "lblBrightness"
        Me.lblBrightness.Size = New System.Drawing.Size(56, 13)
        Me.lblBrightness.TabIndex = 10
        Me.lblBrightness.Text = "Brightness"
        '
        'tbContrast
        '
        Me.tbContrast.Location = New System.Drawing.Point(149, 285)
        Me.tbContrast.Maximum = 127
        Me.tbContrast.Minimum = -127
        Me.tbContrast.Name = "tbContrast"
        Me.tbContrast.Size = New System.Drawing.Size(511, 45)
        Me.tbContrast.TabIndex = 9
        Me.tbContrast.TickFrequency = 5
        '
        'tbBrightness
        '
        Me.tbBrightness.Location = New System.Drawing.Point(149, 234)
        Me.tbBrightness.Maximum = 127
        Me.tbBrightness.Minimum = -127
        Me.tbBrightness.Name = "tbBrightness"
        Me.tbBrightness.Size = New System.Drawing.Size(511, 45)
        Me.tbBrightness.TabIndex = 8
        Me.tbBrightness.TickFrequency = 5
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(412, 365)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(248, 29)
        Me.btnApply.TabIndex = 12
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'frmGIMPRecolor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(702, 416)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.lblContrast)
        Me.Controls.Add(Me.lblBrightness)
        Me.Controls.Add(Me.tbContrast)
        Me.Controls.Add(Me.tbBrightness)
        Me.Controls.Add(Me.cboTones)
        Me.Controls.Add(Me.lblTones)
        Me.Controls.Add(Me.lblBlue)
        Me.Controls.Add(Me.lblGreen)
        Me.Controls.Add(Me.lblRed)
        Me.Controls.Add(Me.tbBlue)
        Me.Controls.Add(Me.tbGreen)
        Me.Controls.Add(Me.tbRed)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmGIMPRecolor"
        Me.Text = "Recolor options"
        CType(Me.tbRed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbGreen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbBlue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbContrast, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbBrightness, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbRed As System.Windows.Forms.TrackBar
    Friend WithEvents tbGreen As System.Windows.Forms.TrackBar
    Friend WithEvents tbBlue As System.Windows.Forms.TrackBar
    Friend WithEvents lblRed As System.Windows.Forms.Label
    Friend WithEvents lblGreen As System.Windows.Forms.Label
    Friend WithEvents lblBlue As System.Windows.Forms.Label
    Friend WithEvents lblTones As System.Windows.Forms.Label
    Friend WithEvents cboTones As System.Windows.Forms.ComboBox
    Friend WithEvents lblContrast As System.Windows.Forms.Label
    Friend WithEvents lblBrightness As System.Windows.Forms.Label
    Friend WithEvents tbContrast As System.Windows.Forms.TrackBar
    Friend WithEvents tbBrightness As System.Windows.Forms.TrackBar
    Friend WithEvents btnApply As System.Windows.Forms.Button
End Class
