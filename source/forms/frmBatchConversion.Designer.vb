<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchConversion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatchConversion))
        Me.gbType = New System.Windows.Forms.GroupBox()
        Me.rbPNG_to_ZT1 = New System.Windows.Forms.RadioButton()
        Me.rbZT1_to_PNG = New System.Windows.Forms.RadioButton()
        Me.btnConvert = New System.Windows.Forms.Button()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.cmdSettings = New System.Windows.Forms.Button()
        Me.PB = New System.Windows.Forms.ProgressBar()
        Me.gbType.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbType
        '
        Me.gbType.Controls.Add(Me.rbPNG_to_ZT1)
        Me.gbType.Controls.Add(Me.rbZT1_to_PNG)
        Me.gbType.Location = New System.Drawing.Point(47, 47)
        Me.gbType.Name = "GbType"
        Me.gbType.Size = New System.Drawing.Size(498, 78)
        Me.gbType.TabIndex = 11
        Me.gbType.TabStop = False
        Me.gbType.Text = "Choose what needs to be converted:"
        '
        'rbPNG_to_ZT1
        '
        Me.rbPNG_to_ZT1.AutoSize = True
        Me.rbPNG_to_ZT1.Checked = True
        Me.rbPNG_to_ZT1.Location = New System.Drawing.Point(6, 28)
        Me.rbPNG_to_ZT1.Name = "RbPNG_to_ZT1"
        Me.rbPNG_to_ZT1.Size = New System.Drawing.Size(219, 17)
        Me.rbPNG_to_ZT1.TabIndex = 9
        Me.rbPNG_to_ZT1.TabStop = True
        Me.rbPNG_to_ZT1.Text = "Convert all .PNG-files to a ZT1-graphic"
        Me.rbPNG_to_ZT1.UseVisualStyleBackColor = True
        '
        'rbZT1_to_PNG
        '
        Me.rbZT1_to_PNG.AutoSize = True
        Me.rbZT1_to_PNG.Location = New System.Drawing.Point(6, 51)
        Me.rbZT1_to_PNG.Name = "RbZT1_to_PNG"
        Me.rbZT1_to_PNG.Size = New System.Drawing.Size(232, 17)
        Me.rbZT1_to_PNG.TabIndex = 8
        Me.rbZT1_to_PNG.Text = "Convert all ZT1 graphic files to .PNG files"
        Me.rbZT1_to_PNG.UseVisualStyleBackColor = True
        '
        'btnConvert
        '
        Me.btnConvert.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConvert.Location = New System.Drawing.Point(47, 294)
        Me.btnConvert.Name = "BtnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(498, 32)
        Me.btnConvert.TabIndex = 15
        Me.btnConvert.Text = "Convert"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(50, 156)
        Me.lblInfo.Name = "LblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(529, 78)
        Me.lblInfo.TabIndex = 19
        Me.lblInfo.Text = resources.GetString("lblInfo.Text")
        '
        'cmdSettings
        '
        Me.cmdSettings.Location = New System.Drawing.Point(47, 258)
        Me.cmdSettings.Name = "CmdSettings"
        Me.cmdSettings.Size = New System.Drawing.Size(498, 30)
        Me.cmdSettings.TabIndex = 22
        Me.cmdSettings.Text = "Change settings"
        Me.cmdSettings.UseVisualStyleBackColor = True
        '
        'PB
        '
        Me.PB.Location = New System.Drawing.Point(47, 332)
        Me.PB.Name = "PB"
        Me.PB.Size = New System.Drawing.Size(498, 22)
        Me.PB.TabIndex = 23
        '
        'frmBatchConversion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(604, 384)
        Me.Controls.Add(Me.PB)
        Me.Controls.Add(Me.cmdSettings)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.btnConvert)
        Me.Controls.Add(Me.gbType)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmBatchConversion"
        Me.Text = "Batch conversion"
        Me.gbType.ResumeLayout(False)
        Me.gbType.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbType As System.Windows.Forms.GroupBox
    Friend WithEvents rbPNG_to_ZT1 As System.Windows.Forms.RadioButton
    Friend WithEvents rbZT1_to_PNG As System.Windows.Forms.RadioButton
    Friend WithEvents btnConvert As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents cmdSettings As System.Windows.Forms.Button
    Friend WithEvents PB As System.Windows.Forms.ProgressBar
End Class
