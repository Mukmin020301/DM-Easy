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
        Me.Bclr = New System.Windows.Forms.Button()
        Me.Bchk = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LPorts = New System.Windows.Forms.Label()
        Me.Lout = New System.Windows.Forms.Label()
        Me.Lverified = New System.Windows.Forms.Label()
        Me.Bsave = New System.Windows.Forms.Button()
        Me.Bcansel = New System.Windows.Forms.Button()
        Me.CB12 = New System.Windows.Forms.CheckBox()
        Me.CB11 = New System.Windows.Forms.CheckBox()
        Me.CB10 = New System.Windows.Forms.CheckBox()
        Me.CB9 = New System.Windows.Forms.CheckBox()
        Me.CB8 = New System.Windows.Forms.CheckBox()
        Me.CB7 = New System.Windows.Forms.CheckBox()
        Me.CB6 = New System.Windows.Forms.CheckBox()
        Me.CB5 = New System.Windows.Forms.CheckBox()
        Me.CB4 = New System.Windows.Forms.CheckBox()
        Me.CB3 = New System.Windows.Forms.CheckBox()
        Me.CB2 = New System.Windows.Forms.CheckBox()
        Me.CB1 = New System.Windows.Forms.CheckBox()
        Me.TBInitT = New System.Windows.Forms.TextBox()
        Me.TBport = New System.Windows.Forms.TextBox()
        Me.TBBaud = New System.Windows.Forms.TextBox()
        Me.Bopen = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TBPngB = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TBSend = New System.Windows.Forms.TextBox()
        Me.TBBytes = New System.Windows.Forms.TextBox()
        Me.CBRep = New System.Windows.Forms.CheckBox()
        Me.RBOBD = New System.Windows.Forms.RadioButton()
        Me.RBECU = New System.Windows.Forms.RadioButton()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CBSlow = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TBinitB = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TBPngT = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.TBMaxPR = New System.Windows.Forms.TextBox()
        Me.Bpar = New System.Windows.Forms.Button()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Bclr
        '
        Me.Bclr.Location = New System.Drawing.Point(74, 19)
        Me.Bclr.Name = "Bclr"
        Me.Bclr.Size = New System.Drawing.Size(60, 23)
        Me.Bclr.TabIndex = 58
        Me.Bclr.Text = "CLEAR"
        Me.Bclr.UseVisualStyleBackColor = True
        '
        'Bchk
        '
        Me.Bchk.Location = New System.Drawing.Point(5, 19)
        Me.Bchk.Name = "Bchk"
        Me.Bchk.Size = New System.Drawing.Size(60, 23)
        Me.Bchk.TabIndex = 57
        Me.Bchk.Text = "CHECK"
        Me.Bchk.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(8, 3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(127, 13)
        Me.Label6.TabIndex = 56
        Me.Label6.Text = "OBD ERROR CODES"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 504)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(277, 26)
        Me.Label5.TabIndex = 55
        Me.Label5.Text = "  NOTE: Not all vehicles provide all channels." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Don't select inactive channels as" & _
            " it may ""hang"""
        '
        'LPorts
        '
        Me.LPorts.AutoSize = True
        Me.LPorts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LPorts.Location = New System.Drawing.Point(5, 7)
        Me.LPorts.Name = "LPorts"
        Me.LPorts.Size = New System.Drawing.Size(96, 13)
        Me.LPorts.TabIndex = 54
        Me.LPorts.Text = "Available Ports:"
        '
        'Lout
        '
        Me.Lout.AutoSize = True
        Me.Lout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lout.Location = New System.Drawing.Point(2, 152)
        Me.Lout.Name = "Lout"
        Me.Lout.Size = New System.Drawing.Size(77, 13)
        Me.Lout.TabIndex = 53
        Me.Lout.Text = "OUTGOING:"
        '
        'Lverified
        '
        Me.Lverified.AutoSize = True
        Me.Lverified.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lverified.Location = New System.Drawing.Point(2, 168)
        Me.Lverified.Name = "Lverified"
        Me.Lverified.Size = New System.Drawing.Size(201, 13)
        Me.Lverified.TabIndex = 52
        Me.Lverified.Text = "COMUNICATIONS NOT VERIFIED"
        '
        'Bsave
        '
        Me.Bsave.Location = New System.Drawing.Point(5, 552)
        Me.Bsave.Name = "Bsave"
        Me.Bsave.Size = New System.Drawing.Size(50, 23)
        Me.Bsave.TabIndex = 51
        Me.Bsave.Text = "SET"
        Me.Bsave.UseVisualStyleBackColor = True
        '
        'Bcansel
        '
        Me.Bcansel.Location = New System.Drawing.Point(242, 552)
        Me.Bcansel.Name = "Bcansel"
        Me.Bcansel.Size = New System.Drawing.Size(58, 23)
        Me.Bcansel.TabIndex = 50
        Me.Bcansel.Text = "EXIT"
        Me.Bcansel.UseVisualStyleBackColor = True
        '
        'CB12
        '
        Me.CB12.AutoSize = True
        Me.CB12.Location = New System.Drawing.Point(155, 480)
        Me.CB12.Name = "CB12"
        Me.CB12.Size = New System.Drawing.Size(131, 17)
        Me.CB12.TabIndex = 49
        Me.CB12.Text = "Fuel Pres (WRT MAP)"
        Me.CB12.UseVisualStyleBackColor = True
        '
        'CB11
        '
        Me.CB11.AutoSize = True
        Me.CB11.Location = New System.Drawing.Point(155, 461)
        Me.CB11.Name = "CB11"
        Me.CB11.Size = New System.Drawing.Size(85, 17)
        Me.CB11.TabIndex = 48
        Me.CB11.Text = "O2 Sensor 2"
        Me.CB11.UseVisualStyleBackColor = True
        '
        'CB10
        '
        Me.CB10.AutoSize = True
        Me.CB10.Location = New System.Drawing.Point(155, 442)
        Me.CB10.Name = "CB10"
        Me.CB10.Size = New System.Drawing.Size(85, 17)
        Me.CB10.TabIndex = 47
        Me.CB10.Text = "O2 Sensor 1"
        Me.CB10.UseVisualStyleBackColor = True
        '
        'CB9
        '
        Me.CB9.AutoSize = True
        Me.CB9.Location = New System.Drawing.Point(155, 423)
        Me.CB9.Name = "CB9"
        Me.CB9.Size = New System.Drawing.Size(102, 17)
        Me.CB9.TabIndex = 46
        Me.CB9.Text = "Throttle Position"
        Me.CB9.UseVisualStyleBackColor = True
        '
        'CB8
        '
        Me.CB8.AutoSize = True
        Me.CB8.Location = New System.Drawing.Point(155, 404)
        Me.CB8.Name = "CB8"
        Me.CB8.Size = New System.Drawing.Size(91, 17)
        Me.CB8.TabIndex = 45
        Me.CB8.Text = "Mass Air Flow"
        Me.CB8.UseVisualStyleBackColor = True
        '
        'CB7
        '
        Me.CB7.AutoSize = True
        Me.CB7.Location = New System.Drawing.Point(155, 385)
        Me.CB7.Name = "CB7"
        Me.CB7.Size = New System.Drawing.Size(101, 17)
        Me.CB7.TabIndex = 44
        Me.CB7.Text = "Intake Air Temp"
        Me.CB7.UseVisualStyleBackColor = True
        '
        'CB6
        '
        Me.CB6.AutoSize = True
        Me.CB6.Location = New System.Drawing.Point(13, 480)
        Me.CB6.Name = "CB6"
        Me.CB6.Size = New System.Drawing.Size(100, 17)
        Me.CB6.TabIndex = 43
        Me.CB6.Text = "Spark Advance"
        Me.CB6.UseVisualStyleBackColor = True
        '
        'CB5
        '
        Me.CB5.AutoSize = True
        Me.CB5.Location = New System.Drawing.Point(13, 461)
        Me.CB5.Name = "CB5"
        Me.CB5.Size = New System.Drawing.Size(95, 17)
        Me.CB5.TabIndex = 42
        Me.CB5.Text = "Vehicle Speed"
        Me.CB5.UseVisualStyleBackColor = True
        '
        'CB4
        '
        Me.CB4.AutoSize = True
        Me.CB4.Location = New System.Drawing.Point(13, 442)
        Me.CB4.Name = "CB4"
        Me.CB4.Size = New System.Drawing.Size(93, 17)
        Me.CB4.TabIndex = 41
        Me.CB4.Text = "Engine Speed"
        Me.CB4.UseVisualStyleBackColor = True
        '
        'CB3
        '
        Me.CB3.AutoSize = True
        Me.CB3.Location = New System.Drawing.Point(13, 423)
        Me.CB3.Name = "CB3"
        Me.CB3.Size = New System.Drawing.Size(107, 17)
        Me.CB3.TabIndex = 40
        Me.CB3.Text = "Intake Man. Pres"
        Me.CB3.UseVisualStyleBackColor = True
        '
        'CB2
        '
        Me.CB2.AutoSize = True
        Me.CB2.Location = New System.Drawing.Point(13, 404)
        Me.CB2.Name = "CB2"
        Me.CB2.Size = New System.Drawing.Size(92, 17)
        Me.CB2.TabIndex = 39
        Me.CB2.Text = "Coolant Temp"
        Me.CB2.UseVisualStyleBackColor = True
        '
        'CB1
        '
        Me.CB1.AutoSize = True
        Me.CB1.Location = New System.Drawing.Point(13, 385)
        Me.CB1.Name = "CB1"
        Me.CB1.Size = New System.Drawing.Size(86, 17)
        Me.CB1.TabIndex = 38
        Me.CB1.Text = "Engine Load"
        Me.CB1.UseVisualStyleBackColor = True
        '
        'TBInitT
        '
        Me.TBInitT.Location = New System.Drawing.Point(59, 75)
        Me.TBInitT.Name = "TBInitT"
        Me.TBInitT.Size = New System.Drawing.Size(236, 20)
        Me.TBInitT.TabIndex = 37
        Me.TBInitT.Text = "ATSP0, ATE0 are always sent"
        Me.ToolTip1.SetToolTip(Me.TBInitT, "Enter alphanumeric sequence to send. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ATZ, ATE0, ATH0 ATSP0 always sent for OBD-" & _
                "II")
        '
        'TBport
        '
        Me.TBport.Location = New System.Drawing.Point(69, 23)
        Me.TBport.Name = "TBport"
        Me.TBport.Size = New System.Drawing.Size(35, 20)
        Me.TBport.TabIndex = 36
        '
        'TBBaud
        '
        Me.TBBaud.Location = New System.Drawing.Point(156, 23)
        Me.TBBaud.Name = "TBBaud"
        Me.TBBaud.Size = New System.Drawing.Size(58, 20)
        Me.TBBaud.TabIndex = 35
        '
        'Bopen
        '
        Me.Bopen.Location = New System.Drawing.Point(201, 48)
        Me.Bopen.Name = "Bopen"
        Me.Bopen.Size = New System.Drawing.Size(92, 23)
        Me.Bopen.TabIndex = 34
        Me.Bopen.Text = "OPEN PORT"
        Me.Bopen.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 369)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(270, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "SELECT UP TO 6 OBD CHANNELS TO READ:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(5, 79)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 13)
        Me.Label3.TabIndex = 32
        Me.Label3.Text = "TEXT:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(112, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "BAUD:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "COM Port:"
        '
        'TBPngB
        '
        Me.TBPngB.Location = New System.Drawing.Point(59, 83)
        Me.TBPngB.Name = "TBPngB"
        Me.TBPngB.Size = New System.Drawing.Size(235, 20)
        Me.TBPngB.TabIndex = 60
        Me.ToolTip1.SetToolTip(Me.TBPngB, "Enter PING bytes (0-255) seperated with spaces." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "eg: 105 64 86 169 5 for RedLeo E" & _
                "CU")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(6, 86)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 13)
        Me.Label7.TabIndex = 59
        Me.Label7.Text = "BYTES:"
        '
        'TBSend
        '
        Me.TBSend.Location = New System.Drawing.Point(94, 6)
        Me.TBSend.Name = "TBSend"
        Me.TBSend.Size = New System.Drawing.Size(143, 20)
        Me.TBSend.TabIndex = 66
        Me.ToolTip1.SetToolTip(Me.TBSend, "Enter text to send, then click SEND")
        '
        'TBBytes
        '
        Me.TBBytes.Location = New System.Drawing.Point(250, 108)
        Me.TBBytes.Name = "TBBytes"
        Me.TBBytes.Size = New System.Drawing.Size(41, 20)
        Me.TBBytes.TabIndex = 68
        Me.ToolTip1.SetToolTip(Me.TBBytes, "Enter if there is a KNOWN number of data bytes," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "otherwise leave as 0.")
        '
        'CBRep
        '
        Me.CBRep.AutoSize = True
        Me.CBRep.Location = New System.Drawing.Point(162, 36)
        Me.CBRep.Name = "CBRep"
        Me.CBRep.Size = New System.Drawing.Size(96, 17)
        Me.CBRep.TabIndex = 61
        Me.CBRep.Text = "Repeate PING"
        Me.CBRep.UseVisualStyleBackColor = True
        '
        'RBOBD
        '
        Me.RBOBD.AutoSize = True
        Me.RBOBD.Location = New System.Drawing.Point(29, 124)
        Me.RBOBD.Name = "RBOBD"
        Me.RBOBD.Size = New System.Drawing.Size(48, 17)
        Me.RBOBD.TabIndex = 62
        Me.RBOBD.TabStop = True
        Me.RBOBD.Text = "OBD"
        Me.RBOBD.UseVisualStyleBackColor = True
        '
        'RBECU
        '
        Me.RBECU.AutoSize = True
        Me.RBECU.Location = New System.Drawing.Point(118, 124)
        Me.RBECU.Name = "RBECU"
        Me.RBECU.Size = New System.Drawing.Size(47, 17)
        Me.RBECU.TabIndex = 63
        Me.RBECU.TabStop = True
        Me.RBECU.Text = "ECU"
        Me.RBECU.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(243, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(51, 23)
        Me.Button1.TabIndex = 64
        Me.Button1.Text = "SEND"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'CBSlow
        '
        Me.CBSlow.AutoSize = True
        Me.CBSlow.Location = New System.Drawing.Point(15, 136)
        Me.CBSlow.Name = "CBSlow"
        Me.CBSlow.Size = New System.Drawing.Size(263, 17)
        Me.CBSlow.TabIndex = 67
        Me.CBSlow.Text = "SLOW OBD: Send ONE Chanel Request at a time"
        Me.CBSlow.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(159, 112)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(91, 13)
        Me.Label8.TabIndex = 69
        Me.Label8.Text = "Bytes to Recieve:"
        '
        'TBinitB
        '
        Me.TBinitB.Location = New System.Drawing.Point(59, 100)
        Me.TBinitB.Name = "TBinitB"
        Me.TBinitB.Size = New System.Drawing.Size(236, 20)
        Me.TBinitB.TabIndex = 71
        Me.ToolTip1.SetToolTip(Me.TBinitB, "Enter bytes (0-255) seperated by spaces")
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(4, 103)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 13)
        Me.Label9.TabIndex = 70
        Me.Label9.Text = "BYTES:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(4, 55)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(172, 13)
        Me.Label10.TabIndex = 72
        Me.Label10.Text = "INITIALIZATION SEQUENCE"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.Bpar)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.TBinitB)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.LPorts)
        Me.Panel1.Controls.Add(Me.TBInitT)
        Me.Panel1.Controls.Add(Me.TBport)
        Me.Panel1.Controls.Add(Me.TBBaud)
        Me.Panel1.Controls.Add(Me.Bopen)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.RBECU)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.RBOBD)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(0, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(303, 146)
        Me.Panel1.TabIndex = 73
        '
        'TBPngT
        '
        Me.TBPngT.Location = New System.Drawing.Point(59, 56)
        Me.TBPngT.Name = "TBPngT"
        Me.TBPngT.Size = New System.Drawing.Size(233, 20)
        Me.TBPngT.TabIndex = 75
        Me.ToolTip1.SetToolTip(Me.TBPngT, "Enter PING text." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Eg. some systems respond to: A")
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(6, 59)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(43, 13)
        Me.Label11.TabIndex = 74
        Me.Label11.Text = "TEXT:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(5, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(83, 13)
        Me.Label12.TabIndex = 76
        Me.Label12.Text = "Instantanious"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.Label14)
        Me.Panel2.Controls.Add(Me.TBMaxPR)
        Me.Panel2.Controls.Add(Me.Label13)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.TBPngT)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.TBBytes)
        Me.Panel2.Controls.Add(Me.CBSlow)
        Me.Panel2.Controls.Add(Me.TBSend)
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.CBRep)
        Me.Panel2.Controls.Add(Me.TBPngB)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Location = New System.Drawing.Point(2, 204)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(299, 160)
        Me.Panel2.TabIndex = 77
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(8, 37)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(137, 13)
        Me.Label13.TabIndex = 73
        Me.Label13.Text = "DATA REQUEST PING"
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.Bclr)
        Me.Panel3.Controls.Add(Me.Bchk)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Location = New System.Drawing.Point(76, 531)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(140, 45)
        Me.Panel3.TabIndex = 78
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(5, 113)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(102, 13)
        Me.Label14.TabIndex = 78
        Me.Label14.Text = "MAX Ping Rate (ms)"
        '
        'TBMaxPR
        '
        Me.TBMaxPR.Location = New System.Drawing.Point(110, 110)
        Me.TBMaxPR.Name = "TBMaxPR"
        Me.TBMaxPR.Size = New System.Drawing.Size(41, 20)
        Me.TBMaxPR.TabIndex = 77
        Me.ToolTip1.SetToolTip(Me.TBMaxPR, "DO NOT send Pings faster then this")
        '
        'Bpar
        '
        Me.Bpar.Location = New System.Drawing.Point(270, 21)
        Me.Bpar.Name = "Bpar"
        Me.Bpar.Size = New System.Drawing.Size(24, 23)
        Me.Bpar.TabIndex = 73
        Me.Bpar.Text = "N"
        Me.ToolTip1.SetToolTip(Me.Bpar, "Normally ""N""" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & """E"" for RedLeo ECU")
        Me.Bpar.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(224, 26)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(43, 13)
        Me.Label15.TabIndex = 74
        Me.Label15.Text = "Parity:"
        '
        'FOBD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(305, 576)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label5)
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
        Me.Controls.Add(Me.Label4)
        Me.Name = "FOBD"
        Me.Text = "ECU - OBD Comunications"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Bclr As System.Windows.Forms.Button
    Friend WithEvents Bchk As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents LPorts As System.Windows.Forms.Label
    Friend WithEvents Lout As System.Windows.Forms.Label
    Friend WithEvents Lverified As System.Windows.Forms.Label
    Friend WithEvents Bsave As System.Windows.Forms.Button
    Friend WithEvents Bcansel As System.Windows.Forms.Button
    Friend WithEvents CB12 As System.Windows.Forms.CheckBox
    Friend WithEvents CB11 As System.Windows.Forms.CheckBox
    Friend WithEvents CB10 As System.Windows.Forms.CheckBox
    Friend WithEvents CB9 As System.Windows.Forms.CheckBox
    Friend WithEvents CB8 As System.Windows.Forms.CheckBox
    Friend WithEvents CB7 As System.Windows.Forms.CheckBox
    Friend WithEvents CB6 As System.Windows.Forms.CheckBox
    Friend WithEvents CB5 As System.Windows.Forms.CheckBox
    Friend WithEvents CB4 As System.Windows.Forms.CheckBox
    Friend WithEvents CB3 As System.Windows.Forms.CheckBox
    Friend WithEvents CB2 As System.Windows.Forms.CheckBox
    Friend WithEvents CB1 As System.Windows.Forms.CheckBox
    Friend WithEvents TBInitT As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents TBport As System.Windows.Forms.TextBox
    Friend WithEvents TBBaud As System.Windows.Forms.TextBox
    Friend WithEvents Bopen As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TBPngB As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents CBRep As System.Windows.Forms.CheckBox
    Friend WithEvents RBOBD As System.Windows.Forms.RadioButton
    Friend WithEvents RBECU As System.Windows.Forms.RadioButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TBSend As System.Windows.Forms.TextBox
    Friend WithEvents CBSlow As System.Windows.Forms.CheckBox
    Friend WithEvents TBBytes As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TBinitB As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TBPngT As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TBMaxPR As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Bpar As System.Windows.Forms.Button
End Class
