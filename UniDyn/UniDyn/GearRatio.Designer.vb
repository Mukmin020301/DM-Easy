<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GearRatio
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GearRatio))
        Me.TBFact = New System.Windows.Forms.TextBox()
        Me.TBRPM = New System.Windows.Forms.TextBox()
        Me.TBRat = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadioButton6 = New System.Windows.Forms.RadioButton()
        Me.RadioButton5 = New System.Windows.Forms.RadioButton()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.ButExit = New System.Windows.Forms.Button()
        Me.ButSet = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.RadioButton7 = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TBRPM2 = New System.Windows.Forms.TextBox()
        Me.TBF2 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.RadioButton8 = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'TBFact
        '
        Me.TBFact.Location = New System.Drawing.Point(58, 167)
        Me.TBFact.Name = "TBFact"
        Me.TBFact.Size = New System.Drawing.Size(43, 20)
        Me.TBFact.TabIndex = 28
        '
        'TBRPM
        '
        Me.TBRPM.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBRPM.Location = New System.Drawing.Point(132, 163)
        Me.TBRPM.Name = "TBRPM"
        Me.TBRPM.ReadOnly = True
        Me.TBRPM.Size = New System.Drawing.Size(82, 31)
        Me.TBRPM.TabIndex = 27
        '
        'TBRat
        '
        Me.TBRat.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBRat.Location = New System.Drawing.Point(145, 62)
        Me.TBRat.Name = "TBRat"
        Me.TBRat.ReadOnly = True
        Me.TBRat.Size = New System.Drawing.Size(69, 31)
        Me.TBRat.TabIndex = 26
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(0, 256)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(226, 26)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Select target engine speed, or target gear ratio" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Click SET when stable to accept" & _
            " set ratio."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 146)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(180, 13)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "User Defined Gear Ratio           RPM"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(151, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "Ratio"
        '
        'RadioButton6
        '
        Me.RadioButton6.AutoSize = True
        Me.RadioButton6.Location = New System.Drawing.Point(38, 171)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton6.TabIndex = 22
        Me.RadioButton6.TabStop = True
        Me.RadioButton6.UseVisualStyleBackColor = True
        '
        'RadioButton5
        '
        Me.RadioButton5.AutoSize = True
        Me.RadioButton5.Location = New System.Drawing.Point(38, 100)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton5.TabIndex = 21
        Me.RadioButton5.TabStop = True
        Me.RadioButton5.Text = "6000"
        Me.RadioButton5.UseVisualStyleBackColor = True
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(38, 81)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton4.TabIndex = 20
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Text = "4000"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(38, 62)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton3.TabIndex = 19
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "2000"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(38, 43)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton2.TabIndex = 18
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "1000"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(38, 24)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(43, 17)
        Me.RadioButton1.TabIndex = 17
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "500"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'ButExit
        '
        Me.ButExit.Location = New System.Drawing.Point(158, 288)
        Me.ButExit.Name = "ButExit"
        Me.ButExit.Size = New System.Drawing.Size(56, 23)
        Me.ButExit.TabIndex = 16
        Me.ButExit.Text = "Cancel"
        Me.ButExit.UseVisualStyleBackColor = True
        '
        'ButSet
        '
        Me.ButSet.Location = New System.Drawing.Point(11, 288)
        Me.ButSet.Name = "ButSet"
        Me.ButSet.Size = New System.Drawing.Size(41, 23)
        Me.ButSet.TabIndex = 15
        Me.ButSet.Text = "SET"
        Me.ButSet.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 72.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(77, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 108)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "}"
        '
        'RadioButton7
        '
        Me.RadioButton7.AutoSize = True
        Me.RadioButton7.Location = New System.Drawing.Point(38, 230)
        Me.RadioButton7.Name = "RadioButton7"
        Me.RadioButton7.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton7.TabIndex = 30
        Me.RadioButton7.TabStop = True
        Me.RadioButton7.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(65, 207)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(107, 13)
        Me.Label5.TabIndex = 31
        Me.Label5.Text = "Set to current RPM2:"
        '
        'TBRPM2
        '
        Me.TBRPM2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBRPM2.Location = New System.Drawing.Point(132, 223)
        Me.TBRPM2.Name = "TBRPM2"
        Me.TBRPM2.ReadOnly = True
        Me.TBRPM2.Size = New System.Drawing.Size(82, 31)
        Me.TBRPM2.TabIndex = 32
        '
        'TBF2
        '
        Me.TBF2.Location = New System.Drawing.Point(58, 226)
        Me.TBF2.Name = "TBF2"
        Me.TBF2.ReadOnly = True
        Me.TBF2.Size = New System.Drawing.Size(43, 20)
        Me.TBF2.TabIndex = 33
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(23, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 34
        Me.Label6.Text = "Engine RPM"
        '
        'RadioButton8
        '
        Me.RadioButton8.AutoSize = True
        Me.RadioButton8.Location = New System.Drawing.Point(38, 119)
        Me.RadioButton8.Name = "RadioButton8"
        Me.RadioButton8.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton8.TabIndex = 35
        Me.RadioButton8.TabStop = True
        Me.RadioButton8.Text = "8000"
        Me.RadioButton8.UseVisualStyleBackColor = True
        '
        'GearRatio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(226, 312)
        Me.ControlBox = False
        Me.Controls.Add(Me.RadioButton8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TBF2)
        Me.Controls.Add(Me.TBRPM2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.RadioButton7)
        Me.Controls.Add(Me.TBFact)
        Me.Controls.Add(Me.TBRPM)
        Me.Controls.Add(Me.TBRat)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.RadioButton6)
        Me.Controls.Add(Me.RadioButton5)
        Me.Controls.Add(Me.RadioButton4)
        Me.Controls.Add(Me.RadioButton3)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.RadioButton1)
        Me.Controls.Add(Me.ButExit)
        Me.Controls.Add(Me.ButSet)
        Me.Controls.Add(Me.Label4)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "GearRatio"
        Me.Text = "Set Gear Ratio"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TBFact As System.Windows.Forms.TextBox
    Friend WithEvents TBRPM As System.Windows.Forms.TextBox
    Friend WithEvents TBRat As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents ButExit As System.Windows.Forms.Button
    Friend WithEvents ButSet As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents RadioButton7 As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TBRPM2 As System.Windows.Forms.TextBox
    Friend WithEvents TBF2 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents RadioButton8 As System.Windows.Forms.RadioButton
End Class
