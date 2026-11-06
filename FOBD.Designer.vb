<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FOBD
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
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Bopen = New System.Windows.Forms.Button()
        Me.TBBaud = New System.Windows.Forms.TextBox()
        Me.TBport = New System.Windows.Forms.TextBox()
        Me.TBInit = New System.Windows.Forms.TextBox()
        Me.CB1 = New System.Windows.Forms.CheckBox()
        Me.CB2 = New System.Windows.Forms.CheckBox()
        Me.CB4 = New System.Windows.Forms.CheckBox()
        Me.CB3 = New System.Windows.Forms.CheckBox()
        Me.CB6 = New System.Windows.Forms.CheckBox()
        Me.CB5 = New System.Windows.Forms.CheckBox()
        Me.CB12 = New System.Windows.Forms.CheckBox()
        Me.CB11 = New System.Windows.Forms.CheckBox()
        Me.CB10 = New System.Windows.Forms.CheckBox()
        Me.CB9 = New System.Windows.Forms.CheckBox()
        Me.CB8 = New System.Windows.Forms.CheckBox()
        Me.CB7 = New System.Windows.Forms.CheckBox()
        Me.Bcansel = New System.Windows.Forms.Button()
        Me.Bsave = New System.Windows.Forms.Button()
        Me.Lverified = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Lout = New System.Windows.Forms.Label()
        Me.LPorts = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Bchk = New System.Windows.Forms.Button()
        Me.Bclr = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Comunications Port:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(179, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "BAUD:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 51)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(133, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Initialization comands:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 161)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(277, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "SELECT UP TO 6 DATA CHANNELS TO READ:"
        '
        'Bopen
        '
        Me.Bopen.Location = New System.Drawing.Point(115, 75)
        Me.Bopen.Name = "Bopen"
        Me.Bopen.Size = New System.Drawing.Size(92, 23)
        Me.Bopen.TabIndex = 4
        Me.Bopen.Text = "OPEN PORT"
        Me.Bopen.UseVisualStyleBackColor = True
        '
        'TBBaud
        '
        Me.TBBaud.Location = New System.Drawing.Point(226, 25)
        Me.TBBaud.Name = "TBBaud"
        Me.TBBaud.Size = New System.Drawing.Size(58, 20)
        Me.TBBaud.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.TBBaud, "Usually 38400, ocasionally 9600")
        '
        'TBport
        '
        Me.TBport.Location = New System.Drawing.Point(129, 23)
        Me.TBport.Name = "TBport"
        Me.TBport.Size = New System.Drawing.Size(35, 20)
        Me.TBport.TabIndex = 6
        '
        'TBInit
        '
        Me.TBInit.Location = New System.Drawing.Point(151, 50)
        Me.TBInit.Name = "TBInit"
        Me.TBInit.Size = New System.Drawing.Size(133, 20)
        Me.TBInit.TabIndex = 7
        Me.TBInit.Text = "ATSP0, ATE0 are always sent"
        '
        'CB1
        '
        Me.CB1.AutoSize = True
        Me.CB1.Location = New System.Drawing.Point(9, 183)
        Me.CB1.Name = "CB1"
        Me.CB1.Size = New System.Drawing.Size(86, 17)
        Me.CB1.TabIndex = 8
        Me.CB1.Text = "Engine Load"
        Me.CB1.UseVisualStyleBackColor = True
        '
        'CB2
        '
        Me.CB2.AutoSize = True
        Me.CB2.Location = New System.Drawing.Point(9, 206)
        Me.CB2.Name = "CB2"
        Me.CB2.Size = New System.Drawing.Size(92, 17)
        Me.CB2.TabIndex = 9
        Me.CB2.Text = "Coolant Temp"
        Me.CB2.UseVisualStyleBackColor = True
        '
        'CB4
        '
        Me.CB4.AutoSize = True
        Me.CB4.Location = New System.Drawing.Point(9, 252)
        Me.CB4.Name = "CB4"
        Me.CB4.Size = New System.Drawing.Size(93, 17)
        Me.CB4.TabIndex = 11
        Me.CB4.Text = "Engine Speed"
        Me.CB4.UseVisualStyleBackColor = True
        '
        'CB3
        '
        Me.CB3.AutoSize = True
        Me.CB3.Location = New System.Drawing.Point(9, 229)
        Me.CB3.Name = "CB3"
        Me.CB3.Size = New System.Drawing.Size(107, 17)
        Me.CB3.TabIndex = 10
        Me.CB3.Text = "Intake Man. Pres"
        Me.CB3.UseVisualStyleBackColor = True
        '
        'CB6
        '
        Me.CB6.AutoSize = True
        Me.CB6.Location = New System.Drawing.Point(9, 298)
        Me.CB6.Name = "CB6"
        Me.CB6.Size = New System.Drawing.Size(100, 17)
        Me.CB6.TabIndex = 13
        Me.CB6.Text = "Spark Advance"
        Me.CB6.UseVisualStyleBackColor = True
        '
        'CB5
        '
        Me.CB5.AutoSize = True
        Me.CB5.Location = New System.Drawing.Point(9, 275)
        Me.CB5.Name = "CB5"
        Me.CB5.Size = New System.Drawing.Size(95, 17)
        Me.CB5.TabIndex = 12
        Me.CB5.Text = "Vehicle Speed"
        Me.CB5.UseVisualStyleBackColor = True
        '
        'CB12
        '
        Me.CB12.AutoSize = True
        Me.CB12.Location = New System.Drawing.Point(151, 298)
        Me.CB12.Name = "CB12"
        Me.CB12.Size = New System.Drawing.Size(131, 17)
        Me.CB12.TabIndex = 19
        Me.CB12.Text = "Fuel Pres (WRT MAP)"
        Me.CB12.UseVisualStyleBackColor = True
        '
        'CB11
        '
        Me.CB11.AutoSize = True
        Me.CB11.Location = New System.Drawing.Point(151, 275)
        Me.CB11.Name = "CB11"
        Me.CB11.Size = New System.Drawing.Size(85, 17)
        Me.CB11.TabIndex = 18
        Me.CB11.Text = "O2 Sensor 2"
        Me.CB11.UseVisualStyleBackColor = True
        '
        'CB10
        '
        Me.CB10.AutoSize = True
        Me.CB10.Location = New System.Drawing.Point(151, 252)
        Me.CB10.Name = "CB10"
        Me.CB10.Size = New System.Drawing.Size(85, 17)
        Me.CB10.TabIndex = 17
        Me.CB10.Text = "O2 Sensor 1"
        Me.CB10.UseVisualStyleBackColor = True
        '
        'CB9
        '
        Me.CB9.AutoSize = True
        Me.CB9.Location = New System.Drawing.Point(151, 229)
        Me.CB9.Name = "CB9"
        Me.CB9.Size = New System.Drawing.Size(102, 17)
        Me.CB9.TabIndex = 16
        Me.CB9.Text = "Throttle Position"
        Me.CB9.UseVisualStyleBackColor = True
        '
        'CB8
        '
        Me.CB8.AutoSize = True
        Me.CB8.Location = New System.Drawing.Point(151, 206)
        Me.CB8.Name = "CB8"
        Me.CB8.Size = New System.Drawing.Size(91, 17)
        Me.CB8.TabIndex = 15
        Me.CB8.Text = "Mass Air Flow"
        Me.CB8.UseVisualStyleBackColor = True
        '
        'CB7
        '
        Me.CB7.AutoSize = True
        Me.CB7.Location = New System.Drawing.Point(151, 183)
        Me.CB7.Name = "CB7"
        Me.CB7.Size = New System.Drawing.Size(101, 17)
        Me.CB7.TabIndex = 14
        Me.CB7.Text = "Intake Air Temp"
        Me.CB7.UseVisualStyleBackColor = True
        '
        'Bcansel
        '
        Me.Bcansel.Location = New System.Drawing.Point(227, 370)
        Me.Bcansel.Name = "Bcansel"
        Me.Bcansel.Size = New System.Drawing.Size(58, 23)
        Me.Bcansel.TabIndex = 20
        Me.Bcansel.Text = "EXIT"
        Me.ToolTip1.SetToolTip(Me.Bcansel, "Exits this screen WITHOUT SETing")
        Me.Bcansel.UseVisualStyleBackColor = True
        '
        'Bsave
        '
        Me.Bsave.Location = New System.Drawing.Point(6, 370)
        Me.Bsave.Name = "Bsave"
        Me.Bsave.Size = New System.Drawing.Size(50, 23)
        Me.Bsave.TabIndex = 21
        Me.Bsave.Text = "SET"
        Me.Bsave.UseVisualStyleBackColor = True
        '
        'Lverified
        '
        Me.Lverified.AutoSize = True
        Me.Lverified.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lverified.Location = New System.Drawing.Point(6, 117)
        Me.Lverified.Name = "Lverified"
        Me.Lverified.Size = New System.Drawing.Size(201, 13)
        Me.Lverified.TabIndex = 22
        Me.Lverified.Text = "COMUNICATIONS NOT VERIFIED"
        '
        'Lout
        '
        Me.Lout.AutoSize = True
        Me.Lout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lout.Location = New System.Drawing.Point(6, 96)
        Me.Lout.Name = "Lout"
        Me.Lout.Size = New System.Drawing.Size(77, 13)
        Me.Lout.TabIndex = 24
        Me.Lout.Text = "OUTGOING:"
        '
        'LPorts
        '
        Me.LPorts.AutoSize = True
        Me.LPorts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LPorts.Location = New System.Drawing.Point(6, 6)
        Me.LPorts.Name = "LPorts"
        Me.LPorts.Size = New System.Drawing.Size(96, 13)
        Me.LPorts.TabIndex = 25
        Me.LPorts.Text = "Available Ports:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 322)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(277, 26)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "  NOTE: Not all vehicles provide all channels." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Don't select inactive channels as" & _
            " it may ""hang"""
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(96, 355)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(97, 13)
        Me.Label6.TabIndex = 27
        Me.Label6.Text = "ERROR CODES"
        '
        'Bchk
        '
        Me.Bchk.Location = New System.Drawing.Point(76, 370)
        Me.Bchk.Name = "Bchk"
        Me.Bchk.Size = New System.Drawing.Size(60, 23)
        Me.Bchk.TabIndex = 28
        Me.Bchk.Text = "CHECK"
        Me.Bchk.UseVisualStyleBackColor = True
        '
        'Bclr
        '
        Me.Bclr.Location = New System.Drawing.Point(147, 370)
        Me.Bclr.Name = "Bclr"
        Me.Bclr.Size = New System.Drawing.Size(60, 23)
        Me.Bclr.TabIndex = 29
        Me.Bclr.Text = "CLEAR"
        Me.Bclr.UseVisualStyleBackColor = True
        '
        'FOBD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 395)
        Me.ControlBox = False
        Me.Controls.Add(Me.Bclr)
        Me.Controls.Add(Me.Bchk)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.LPorts)
        Me.Controls.Add(Me.Lout)
        Me.Controls.Add(Me.Lverified)
        Me.Controls.Add(Me.Bsave)
        Me.Controls.Add(Me.Bcansel)
        Me.Controls.Add(Me.CB12)
        Me.Controls.Add(Me.CB11)
        Me.Controls.Add(Me.CB10)
        Me.Controls.Add(Me.CB9)
        Me.Controls.Add(Me.CB8)
        Me.Controls.Add(Me.CB7)
        Me.Controls.Add(Me.CB6)
        Me.Controls.Add(Me.CB5)
        Me.Controls.Add(Me.CB4)
        Me.Controls.Add(Me.CB3)
        Me.Controls.Add(Me.CB2)
        Me.Controls.Add(Me.CB1)
        Me.Controls.Add(Me.TBInit)
        Me.Controls.Add(Me.TBport)
        Me.Controls.Add(Me.TBBaud)
        Me.Controls.Add(Me.Bopen)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "FOBD"
        Me.Text = "On Board Diagnostics (OBD-II) "
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Bopen As System.Windows.Forms.Button
    Friend WithEvents TBBaud As System.Windows.Forms.TextBox
    Friend WithEvents TBport As System.Windows.Forms.TextBox
    Friend WithEvents TBInit As System.Windows.Forms.TextBox
    Friend WithEvents CB1 As System.Windows.Forms.CheckBox
    Friend WithEvents CB2 As System.Windows.Forms.CheckBox
    Friend WithEvents CB4 As System.Windows.Forms.CheckBox
    Friend WithEvents CB3 As System.Windows.Forms.CheckBox
    Friend WithEvents CB6 As System.Windows.Forms.CheckBox
    Friend WithEvents CB5 As System.Windows.Forms.CheckBox
    Friend WithEvents CB12 As System.Windows.Forms.CheckBox
    Friend WithEvents CB11 As System.Windows.Forms.CheckBox
    Friend WithEvents CB10 As System.Windows.Forms.CheckBox
    Friend WithEvents CB9 As System.Windows.Forms.CheckBox
    Friend WithEvents CB8 As System.Windows.Forms.CheckBox
    Friend WithEvents CB7 As System.Windows.Forms.CheckBox
    Friend WithEvents Bcansel As System.Windows.Forms.Button
    Friend WithEvents Bsave As System.Windows.Forms.Button
    Friend WithEvents Lverified As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Lout As System.Windows.Forms.Label
    Friend WithEvents LPorts As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Bchk As System.Windows.Forms.Button
    Friend WithEvents Bclr As System.Windows.Forms.Button
End Class
