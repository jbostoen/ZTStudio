<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmBatchConversion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmBatchConversion))
        Me.gbType = New System.Windows.Forms.GroupBox()
        Me.RbPNG_to_ZT1 = New System.Windows.Forms.RadioButton()
        Me.RbZT1_to_PNG = New System.Windows.Forms.RadioButton()
        Me.BtnConvert = New System.Windows.Forms.Button()
        Me.LblInfo = New System.Windows.Forms.Label()
        Me.BtnSettings = New System.Windows.Forms.Button()
        Me.PBBatchProgress = New System.Windows.Forms.ProgressBar()
        Me.gbType.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbType
        '
        Me.gbType.Controls.Add(Me.RbPNG_to_ZT1)
        Me.gbType.Controls.Add(Me.RbZT1_to_PNG)
        Me.gbType.Location = New System.Drawing.Point(47, 47)
        Me.gbType.Name = "gbType"
        Me.gbType.Size = New System.Drawing.Size(498, 78)
        Me.gbType.TabIndex = 11
        Me.gbType.TabStop = False
        Me.gbType.Text = "Action to perform:"
        '
        'RbPNG_to_ZT1
        '
        Me.RbPNG_to_ZT1.AutoSize = True
        Me.RbPNG_to_ZT1.Checked = True
        Me.RbPNG_to_ZT1.Location = New System.Drawing.Point(6, 28)
        Me.RbPNG_to_ZT1.Name = "RbPNG_to_ZT1"
        Me.RbPNG_to_ZT1.Size = New System.Drawing.Size(220, 17)
        Me.RbPNG_to_ZT1.TabIndex = 9
        Me.RbPNG_to_ZT1.TabStop = True
        Me.RbPNG_to_ZT1.Text = "Convert all .PNG-files to a ZT1-graphic"
        Me.RbPNG_to_ZT1.UseVisualStyleBackColor = True
        '
        'RbZT1_to_PNG
        '
        Me.RbZT1_to_PNG.AutoSize = True
        Me.RbZT1_to_PNG.Location = New System.Drawing.Point(6, 51)
        Me.RbZT1_to_PNG.Name = "RbZT1_to_PNG"
        Me.RbZT1_to_PNG.Size = New System.Drawing.Size(233, 17)
        Me.RbZT1_to_PNG.TabIndex = 8
        Me.RbZT1_to_PNG.Text = "Convert all ZT1 graphic files to .PNG files"
        Me.RbZT1_to_PNG.UseVisualStyleBackColor = True
        '
        'BtnConvert
        '
        Me.BtnConvert.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnConvert.Location = New System.Drawing.Point(47, 294)
        Me.BtnConvert.Name = "BtnConvert"
        Me.BtnConvert.Size = New System.Drawing.Size(498, 32)
        Me.BtnConvert.TabIndex = 15
        Me.BtnConvert.Text = "Start batch conversion"
        Me.BtnConvert.UseVisualStyleBackColor = True
        '
        'LblInfo
        '
        Me.LblInfo.AutoSize = True
        Me.LblInfo.Location = New System.Drawing.Point(50, 156)
        Me.LblInfo.Name = "LblInfo"
        Me.LblInfo.Size = New System.Drawing.Size(527, 78)
        Me.LblInfo.TabIndex = 19
        Me.LblInfo.Text = resources.GetString("LblInfo.Text")
        '
        'BtnSettings
        '
        Me.BtnSettings.Location = New System.Drawing.Point(47, 258)
        Me.BtnSettings.Name = "BtnSettings"
        Me.BtnSettings.Size = New System.Drawing.Size(498, 30)
        Me.BtnSettings.TabIndex = 22
        Me.BtnSettings.Text = "Change settings"
        Me.BtnSettings.UseVisualStyleBackColor = True
        '
        'PBBatchProgress
        '
        Me.PBBatchProgress.Location = New System.Drawing.Point(47, 332)
        Me.PBBatchProgress.Name = "PBBatchProgress"
        Me.PBBatchProgress.Size = New System.Drawing.Size(498, 22)
        Me.PBBatchProgress.TabIndex = 23
        '
        'FrmBatchConversion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(604, 384)
        Me.Controls.Add(Me.PBBatchProgress)
        Me.Controls.Add(Me.BtnSettings)
        Me.Controls.Add(Me.LblInfo)
        Me.Controls.Add(Me.BtnConvert)
        Me.Controls.Add(Me.gbType)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmBatchConversion"
        Me.Text = "Batch graphic conversion"
        Me.gbType.ResumeLayout(False)
        Me.gbType.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbType As System.Windows.Forms.GroupBox
    Friend WithEvents RbPNG_to_ZT1 As System.Windows.Forms.RadioButton
    Friend WithEvents RbZT1_to_PNG As System.Windows.Forms.RadioButton
    Friend WithEvents BtnConvert As System.Windows.Forms.Button
    Friend WithEvents LblInfo As System.Windows.Forms.Label
    Friend WithEvents BtnSettings As System.Windows.Forms.Button
    Friend WithEvents PBBatchProgress As System.Windows.Forms.ProgressBar
End Class
