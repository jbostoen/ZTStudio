<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchRotationFix
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
        Me.lblFolder = New System.Windows.Forms.Label()
        Me.numUpDown = New System.Windows.Forms.NumericUpDown()
        Me.numLeftRight = New System.Windows.Forms.NumericUpDown()
        Me.lblUpDown = New System.Windows.Forms.Label()
        Me.lblLeftRight = New System.Windows.Forms.Label()
        Me.cmdBatchFix = New System.Windows.Forms.Button()
        Me.txtFolder = New System.Windows.Forms.TextBox()
        Me.dlgBrowseFolder = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnSelect = New System.Windows.Forms.Button()
        Me.lblHint = New System.Windows.Forms.Label()
        Me.PB = New System.Windows.Forms.ProgressBar()
        CType(Me.numUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numLeftRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblFolder
        '
        Me.lblFolder.AutoSize = True
        Me.lblFolder.Location = New System.Drawing.Point(25, 65)
        Me.lblFolder.Name = "lblFolder"
        Me.lblFolder.Size = New System.Drawing.Size(80, 13)
        Me.lblFolder.TabIndex = 0
        Me.lblFolder.Text = "Specify folder:"
        '
        'numUpDown
        '
        Me.numUpDown.Location = New System.Drawing.Point(28, 148)
        Me.numUpDown.Name = "numUpDown"
        Me.numUpDown.Size = New System.Drawing.Size(69, 22)
        Me.numUpDown.TabIndex = 1
        Me.numUpDown.Value = New Decimal(New Integer() {16, 0, 0, 0})
        '
        'numLeftRight
        '
        Me.numLeftRight.Location = New System.Drawing.Point(317, 148)
        Me.numLeftRight.Name = "numLeftRight"
        Me.numLeftRight.Size = New System.Drawing.Size(69, 22)
        Me.numLeftRight.TabIndex = 2
        '
        'lblUpDown
        '
        Me.lblUpDown.AutoSize = True
        Me.lblUpDown.Location = New System.Drawing.Point(102, 148)
        Me.lblUpDown.Name = "lblUpDown"
        Me.lblUpDown.Size = New System.Drawing.Size(164, 13)
        Me.lblUpDown.TabIndex = 3
        Me.lblUpDown.Text = "Up (positive) / down (negative)"
        '
        'lblLeftRight
        '
        Me.lblLeftRight.AutoSize = True
        Me.lblLeftRight.Location = New System.Drawing.Point(392, 150)
        Me.lblLeftRight.Name = "lblLeftRight"
        Me.lblLeftRight.Size = New System.Drawing.Size(163, 13)
        Me.lblLeftRight.TabIndex = 4
        Me.lblLeftRight.Text = "Left (positive) / right (negative)"
        '
        'cmdBatchFix
        '
        Me.cmdBatchFix.Location = New System.Drawing.Point(28, 204)
        Me.cmdBatchFix.Name = "cmdBatchFix"
        Me.cmdBatchFix.Size = New System.Drawing.Size(561, 43)
        Me.cmdBatchFix.TabIndex = 5
        Me.cmdBatchFix.Text = "Process entire folder"
        Me.cmdBatchFix.UseVisualStyleBackColor = True
        '
        'txtFolder
        '
        Me.txtFolder.Location = New System.Drawing.Point(28, 81)
        Me.txtFolder.Name = "txtFolder"
        Me.txtFolder.ReadOnly = True
        Me.txtFolder.Size = New System.Drawing.Size(561, 22)
        Me.txtFolder.TabIndex = 6
        '
        'btnSelect
        '
        Me.btnSelect.Location = New System.Drawing.Point(28, 105)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(561, 20)
        Me.btnSelect.TabIndex = 7
        Me.btnSelect.Text = "Select folder"
        Me.btnSelect.UseVisualStyleBackColor = True
        '
        'lblHint
        '
        Me.lblHint.AutoSize = True
        Me.lblHint.Location = New System.Drawing.Point(29, 18)
        Me.lblHint.Name = "lblHint"
        Me.lblHint.Size = New System.Drawing.Size(407, 26)
        Me.lblHint.TabIndex = 8
        Me.lblHint.Text = "Hint: adjust one frame manually first, to know which offsets you need below. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Do" & _
    "n't save that ZT1 Graphic though!"
        '
        'PB
        '
        Me.PB.Location = New System.Drawing.Point(30, 250)
        Me.PB.Name = "PB"
        Me.PB.Size = New System.Drawing.Size(558, 29)
        Me.PB.TabIndex = 9
        '
        'frmBatchRotationFix
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(623, 301)
        Me.Controls.Add(Me.PB)
        Me.Controls.Add(Me.lblHint)
        Me.Controls.Add(Me.btnSelect)
        Me.Controls.Add(Me.txtFolder)
        Me.Controls.Add(Me.cmdBatchFix)
        Me.Controls.Add(Me.lblLeftRight)
        Me.Controls.Add(Me.lblUpDown)
        Me.Controls.Add(Me.numLeftRight)
        Me.Controls.Add(Me.numUpDown)
        Me.Controls.Add(Me.lblFolder)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmBatchRotationFix"
        Me.Text = "Batch rotation fixing"
        CType(Me.numUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numLeftRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblFolder As System.Windows.Forms.Label
    Friend WithEvents numUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents numLeftRight As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblUpDown As System.Windows.Forms.Label
    Friend WithEvents lblLeftRight As System.Windows.Forms.Label
    Friend WithEvents cmdBatchFix As System.Windows.Forms.Button
    Friend WithEvents txtFolder As System.Windows.Forms.TextBox
    Friend WithEvents dlgBrowseFolder As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnSelect As System.Windows.Forms.Button
    Friend WithEvents lblHint As System.Windows.Forms.Label
    Friend WithEvents PB As System.Windows.Forms.ProgressBar
End Class
