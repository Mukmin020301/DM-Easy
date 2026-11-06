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
        Me.ButSet = New System.Windows.Forms.Button()
        Me.ButExit = New System.Windows.Forms.Button()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton5 = New System.Windows.Forms.RadioButton()
        Me.RadioButton6 = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TBRat = New System.Windows.Forms.TextBox()
        Me.TBRPM = New System.Windows.Forms.TextBox()
        Me.TBFact = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TBF2 = New System.Windows.Forms.TextBox()
        Me.TBRPM2 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.RadioButton7 = New System.Windows.Forms.RadioButton()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TBFo = New System.Windows.Forms.TextBox()
        Me.TbRPMo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RadioButton8 = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'ButSet
        '
        Me.ButSet.Location = New System.Drawing.Point(6, 337)
        Me.ButSet.Name = "ButSet"
        Me.ButSet.Size = New System.Drawing.Size(41, 23)
        Me.ButSet.TabIndex = 0
        Me.ButSet.Text = "SET"
        Me.ButSet.UseVisualStyleBackColor = True
        '
        'ButExit
        '
        Me.ButExit.Location = New System.Drawing.Point(166, 337)
        Me.ButExit.Name = "ButExit"
        Me.ButExit.Size = New System.Drawing.Size(56, 23)
        Me.ButExit.TabIndex = 1
        Me.ButExit.Text = "Cancel"
        Me.ButExit.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(31, 24)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton1.TabIndex = 2
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "1000"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(31, 47)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton2.TabIndex = 3
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "2000"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(31, 70)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton3.TabIndex = 4
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "4000"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(31, 93)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton4.TabIndex = 5
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Text = "6000"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton5
        '
        Me.RadioButton5.AutoSize = True
        Me.RadioButton5.Location = New System.Drawing.Point(31, 117)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton5.TabIndex = 6
        Me.RadioButton5.TabStop = True
        Me.RadioButton5.Text = "8000"
        Me.RadioButton5.UseVisualStyleBackColor = True
        '
        'RadioButton6
        '
        Me.RadioButton6.AutoSize = True
        Me.RadioButton6.Location = New System.Drawing.Point(31, 171)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton6.TabIndex = 7
        Me.RadioButton6.TabStop = True
        Me.RadioButton6.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(144, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Ratio"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 305)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(226, 26)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Select target engine speed, or target gear ratio" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Click SET when stable to accept" & _
            " set ratio."
        '
        'TBRat
        '
        Me.TBRat.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBRat.Location = New System.Drawing.Point(138, 62)
        Me.TBRat.Name = "TBRat"
        Me.TBRat.ReadOnly = True
        Me.TBRat.Size = New System.Drawing.Size(69, 31)
        Me.TBRat.TabIndex = 11
        '
        'TBRPM
        '
        Me.TBRPM.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBRPM.Location = New System.Drawing.Point(125, 163)
        Me.TBRPM.Name = "TBRPM"
        Me.TBRPM.ReadOnly = True
        Me.TBRPM.Size = New System.Drawing.Size(82, 31)
        Me.TBRPM.TabIndex = 12
        '
        'TBFact
        '
        Me.TBFact.Location = New System.Drawing.Point(51, 167)
        Me.TBFact.Name = "TBFact"
        Me.TBFact.Size = New System.Drawing.Size(43, 20)
        Me.TBFact.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 72.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(75, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 108)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "}"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(29, 5)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 40
        Me.Label6.Text = "Engine RPM"
        '
        'TBF2
        '
        Me.TBF2.Location = New System.Drawing.Point(51, 223)
        Me.TBF2.Name = "TBF2"
        Me.TBF2.ReadOnly = True
        Me.TBF2.Size = New System.Drawing.Size(43, 20)
        Me.TBF2.TabIndex = 39
        '
        'TBRPM2
        '
        Me.TBRPM2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBRPM2.Location = New System.Drawing.Point(125, 220)
        Me.TBRPM2.Name = "TBRPM2"
        Me.TBRPM2.ReadOnly = True
        Me.TBRPM2.Size = New System.Drawing.Size(82, 31)
        Me.TBRPM2.TabIndex = 38
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 204)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(218, 13)
        Me.Label5.TabIndex = 37
        Me.Label5.Text = "Set to current RPM2 (or hit G from Keyboard)"
        '
        'RadioButton7
        '
        Me.RadioButton7.AutoSize = True
        Me.RadioButton7.Location = New System.Drawing.Point(31, 227)
        Me.RadioButton7.Name = "RadioButton7"
        Me.RadioButton7.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton7.TabIndex = 36
        Me.RadioButton7.TabStop = True
        Me.RadioButton7.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 147)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(171, 13)
        Me.Label7.TabIndex = 35
        Me.Label7.Text = "User Defined Gear Ratio        RPM"
        '
        'TBFo
        '
        Me.TBFo.Location = New System.Drawing.Point(50, 276)
        Me.TBFo.Name = "TBFo"
        Me.TBFo.ReadOnly = True
        Me.TBFo.Size = New System.Drawing.Size(43, 20)
        Me.TBFo.TabIndex = 44
        '
        'TbRPMo
        '
        Me.TbRPMo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TbRPMo.Location = New System.Drawing.Point(124, 273)
        Me.TbRPMo.Name = "TbRPMo"
        Me.TbRPMo.ReadOnly = True
        Me.TbRPMo.Size = New System.Drawing.Size(82, 31)
        Me.TbRPMo.TabIndex = 43
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(70, 257)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(91, 13)
        Me.Label2.TabIndex = 42
        Me.Label2.Text = "Set to OBD RPM:"
        '
        'RadioButton8
        '
        Me.RadioButton8.AutoSize = True
        Me.RadioButton8.Location = New System.Drawing.Point(30, 280)
        Me.RadioButton8.Name = "RadioButton8"
        Me.RadioButton8.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton8.TabIndex = 41
        Me.RadioButton8.TabStop = True
        Me.RadioButton8.UseVisualStyleBackColor = True
        '
        'GearRatio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(233, 362)
        Me.ControlBox = False
        Me.Controls.Add(Me.TBFo)
        Me.Controls.Add(Me.TbRPMo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.RadioButton8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TBF2)
        Me.Controls.Add(Me.TBRPM2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.RadioButton7)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TBFact)
        Me.Controls.Add(Me.TBRPM)
        Me.Controls.Add(Me.TBRat)
        Me.Controls.Add(Me.Label3)
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
        Me.KeyPreview = True
        Me.Name = "GearRatio"
        Me.Text = "Set Gear Ratior"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButSet As System.Windows.Forms.Button
    Friend WithEvents ButExit As System.Windows.Forms.Button
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TBRat As System.Windows.Forms.TextBox
    Friend WithEvents TBRPM As System.Windows.Forms.TextBox
    Friend WithEvents TBFact As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TBF2 As System.Windows.Forms.TextBox
    Friend WithEvents TBRPM2 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents RadioButton7 As System.Windows.Forms.RadioButton
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TBFo As System.Windows.Forms.TextBox
    Friend WithEvents TbRPMo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents RadioButton8 As System.Windows.Forms.RadioButton
End Class
