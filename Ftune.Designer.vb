<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FTun
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TabCntl = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Table1 = New System.Windows.Forms.DataGridView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Table2 = New System.Windows.Forms.DataGridView()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Table3 = New System.Windows.Forms.DataGridView()
        Me.Lx = New System.Windows.Forms.Label()
        Me.TBxax = New System.Windows.Forms.TextBox()
        Me.TBYax = New System.Windows.Forms.TextBox()
        Me.Ly = New System.Windows.Forms.Label()
        Me.BCfg = New System.Windows.Forms.Button()
        Me.CBAccum = New System.Windows.Forms.CheckBox()
        Me.ButSet = New System.Windows.Forms.Button()
        Me.TBSet = New System.Windows.Forms.TextBox()
        Me.ButExp = New System.Windows.Forms.Button()
        Me.ButImp = New System.Windows.Forms.Button()
        Me.ButClose = New System.Windows.Forms.Button()
        Me.TBTarg = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TBCurr = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TBCorr = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TbAF = New System.Windows.Forms.TextBox()
        Me.ButAcc = New System.Windows.Forms.Button()
        Me.ButZeroErr = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ButAbort = New System.Windows.Forms.Button()
        Me.CBFuel = New System.Windows.Forms.CheckBox()
        Me.Ltit = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Lz = New System.Windows.Forms.Label()
        Me.TabCntl.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.Table1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.Table2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.Table3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Panel1.AutoSize = True
        Me.Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel1.Location = New System.Drawing.Point(192, 487)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(0, 0)
        Me.Panel1.TabIndex = 1
        '
        'TabCntl
        '
        Me.TabCntl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabCntl.Controls.Add(Me.TabPage1)
        Me.TabCntl.Controls.Add(Me.TabPage2)
        Me.TabCntl.Controls.Add(Me.TabPage3)
        Me.TabCntl.Location = New System.Drawing.Point(181, 34)
        Me.TabCntl.Name = "TabCntl"
        Me.TabCntl.SelectedIndex = 0
        Me.TabCntl.Size = New System.Drawing.Size(716, 446)
        Me.TabCntl.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Table1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(708, 420)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Target"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Table1
        '
        Me.Table1.AllowUserToAddRows = False
        Me.Table1.AllowUserToDeleteRows = False
        Me.Table1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Table1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Table1.Location = New System.Drawing.Point(3, 3)
        Me.Table1.Name = "Table1"
        Me.Table1.RowHeadersWidth = 60
        Me.Table1.Size = New System.Drawing.Size(702, 414)
        Me.Table1.TabIndex = 1
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Table2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(708, 420)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Measured"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Table2
        '
        Me.Table2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Table2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Table2.Location = New System.Drawing.Point(3, 3)
        Me.Table2.Name = "Table2"
        Me.Table2.ReadOnly = True
        Me.Table2.RowHeadersWidth = 60
        Me.Table2.Size = New System.Drawing.Size(0, 0)
        Me.Table2.TabIndex = 1
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Table3)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(708, 420)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Correction %"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Table3
        '
        Me.Table3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Table3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Table3.Location = New System.Drawing.Point(3, 3)
        Me.Table3.Name = "Table3"
        Me.Table3.ReadOnly = True
        Me.Table3.RowHeadersWidth = 60
        Me.Table3.Size = New System.Drawing.Size(0, 0)
        Me.Table3.TabIndex = 0
        '
        'Lx
        '
        Me.Lx.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Lx.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lx.Location = New System.Drawing.Point(687, 27)
        Me.Lx.Name = "Lx"
        Me.Lx.Size = New System.Drawing.Size(116, 20)
        Me.Lx.TabIndex = 2
        Me.Lx.Text = "X-Axis"
        Me.Lx.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TBxax
        '
        Me.TBxax.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TBxax.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBxax.Location = New System.Drawing.Point(809, 24)
        Me.TBxax.Name = "TBxax"
        Me.TBxax.ReadOnly = True
        Me.TBxax.Size = New System.Drawing.Size(81, 26)
        Me.TBxax.TabIndex = 3
        Me.TBxax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TBYax
        '
        Me.TBYax.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TBYax.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBYax.Location = New System.Drawing.Point(48, 191)
        Me.TBYax.Name = "TBYax"
        Me.TBYax.ReadOnly = True
        Me.TBYax.Size = New System.Drawing.Size(82, 26)
        Me.TBYax.TabIndex = 5
        Me.TBYax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Ly
        '
        Me.Ly.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Ly.AutoSize = True
        Me.Ly.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ly.Location = New System.Drawing.Point(56, 161)
        Me.Ly.Name = "Ly"
        Me.Ly.Size = New System.Drawing.Size(60, 20)
        Me.Ly.TabIndex = 4
        Me.Ly.Text = "Y-Axis"
        '
        'BCfg
        '
        Me.BCfg.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BCfg.Location = New System.Drawing.Point(60, 458)
        Me.BCfg.Name = "BCfg"
        Me.BCfg.Size = New System.Drawing.Size(65, 22)
        Me.BCfg.TabIndex = 6
        Me.BCfg.Text = "Configure"
        Me.ToolTip1.SetToolTip(Me.BCfg, "Hit again to close Config window")
        Me.BCfg.UseVisualStyleBackColor = True
        '
        'CBAccum
        '
        Me.CBAccum.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CBAccum.AutoSize = True
        Me.CBAccum.Location = New System.Drawing.Point(48, 277)
        Me.CBAccum.Name = "CBAccum"
        Me.CBAccum.Size = New System.Drawing.Size(82, 17)
        Me.CBAccum.TabIndex = 7
        Me.CBAccum.Text = "Accumulate"
        Me.CBAccum.UseVisualStyleBackColor = True
        '
        'ButSet
        '
        Me.ButSet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButSet.Location = New System.Drawing.Point(12, 248)
        Me.ButSet.Name = "ButSet"
        Me.ButSet.Size = New System.Drawing.Size(93, 22)
        Me.ButSet.TabIndex = 8
        Me.ButSet.Text = "Set Target To: "
        Me.ButSet.UseVisualStyleBackColor = True
        '
        'TBSet
        '
        Me.TBSet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TBSet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBSet.Location = New System.Drawing.Point(111, 250)
        Me.TBSet.Name = "TBSet"
        Me.TBSet.Size = New System.Drawing.Size(64, 20)
        Me.TBSet.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.TBSet, "Allows setting of the whole Target Map")
        '
        'ButExp
        '
        Me.ButExp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButExp.Location = New System.Drawing.Point(12, 398)
        Me.ButExp.Name = "ButExp"
        Me.ButExp.Size = New System.Drawing.Size(163, 22)
        Me.ButExp.TabIndex = 10
        Me.ButExp.Text = "Export Map"
        Me.ToolTip1.SetToolTip(Me.ButExp, "Exports Measured and Correction tables to a file")
        Me.ButExp.UseVisualStyleBackColor = True
        '
        'ButImp
        '
        Me.ButImp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButImp.Location = New System.Drawing.Point(12, 426)
        Me.ButImp.Name = "ButImp"
        Me.ButImp.Size = New System.Drawing.Size(163, 22)
        Me.ButImp.TabIndex = 11
        Me.ButImp.Text = "Import Map"
        Me.ToolTip1.SetToolTip(Me.ButImp, "Not working yet.")
        Me.ButImp.UseVisualStyleBackColor = True
        '
        'ButClose
        '
        Me.ButClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButClose.Location = New System.Drawing.Point(1047, 458)
        Me.ButClose.Name = "ButClose"
        Me.ButClose.Size = New System.Drawing.Size(65, 22)
        Me.ButClose.TabIndex = 12
        Me.ButClose.Text = "Close"
        Me.ToolTip1.SetToolTip(Me.ButClose, "Saves Configuration and maps, and closes Tube Buddy")
        Me.ButClose.UseVisualStyleBackColor = True
        '
        'TBTarg
        '
        Me.TBTarg.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TBTarg.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBTarg.Location = New System.Drawing.Point(1008, 337)
        Me.TBTarg.Name = "TBTarg"
        Me.TBTarg.ReadOnly = True
        Me.TBTarg.Size = New System.Drawing.Size(103, 26)
        Me.TBTarg.TabIndex = 14
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(916, 340)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 20)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Target"
        '
        'TBCurr
        '
        Me.TBCurr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TBCurr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBCurr.Location = New System.Drawing.Point(1008, 372)
        Me.TBCurr.Name = "TBCurr"
        Me.TBCurr.ReadOnly = True
        Me.TBCurr.Size = New System.Drawing.Size(103, 26)
        Me.TBCurr.TabIndex = 16
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(916, 375)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 20)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Current"
        '
        'TBCorr
        '
        Me.TBCorr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TBCorr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBCorr.Location = New System.Drawing.Point(1008, 407)
        Me.TBCorr.Name = "TBCorr"
        Me.TBCorr.ReadOnly = True
        Me.TBCorr.Size = New System.Drawing.Size(103, 26)
        Me.TBCorr.TabIndex = 18
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(903, 410)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 20)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Correction"
        '
        'TbAF
        '
        Me.TbAF.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TbAF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TbAF.Location = New System.Drawing.Point(111, 300)
        Me.TbAF.Name = "TbAF"
        Me.TbAF.Size = New System.Drawing.Size(64, 20)
        Me.TbAF.TabIndex = 20
        Me.ToolTip1.SetToolTip(Me.TbAF, "Typically 0.8" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Range: 0 to 1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "0 = no averaging" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "0.99 = heavy averaging")
        '
        'ButAcc
        '
        Me.ButAcc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButAcc.Location = New System.Drawing.Point(12, 298)
        Me.ButAcc.Name = "ButAcc"
        Me.ButAcc.Size = New System.Drawing.Size(82, 22)
        Me.ButAcc.TabIndex = 19
        Me.ButAcc.Text = "Acc. Factor"
        Me.ButAcc.UseVisualStyleBackColor = True
        '
        'ButZeroErr
        '
        Me.ButZeroErr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButZeroErr.Location = New System.Drawing.Point(44, 370)
        Me.ButZeroErr.Name = "ButZeroErr"
        Me.ButZeroErr.Size = New System.Drawing.Size(93, 22)
        Me.ButZeroErr.TabIndex = 21
        Me.ButZeroErr.Text = "Zero Corrections"
        Me.ToolTip1.SetToolTip(Me.ButZeroErr, "Set all corrections to Zero")
        Me.ButZeroErr.UseVisualStyleBackColor = True
        '
        'ButAbort
        '
        Me.ButAbort.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButAbort.Location = New System.Drawing.Point(930, 458)
        Me.ButAbort.Name = "ButAbort"
        Me.ButAbort.Size = New System.Drawing.Size(65, 22)
        Me.ButAbort.TabIndex = 22
        Me.ButAbort.Text = "Abort"
        Me.ToolTip1.SetToolTip(Me.ButAbort, "Closes Tube Buddy without saving changes")
        Me.ButAbort.UseVisualStyleBackColor = True
        '
        'CBFuel
        '
        Me.CBFuel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CBFuel.AutoSize = True
        Me.CBFuel.Location = New System.Drawing.Point(56, 348)
        Me.CBFuel.Name = "CBFuel"
        Me.CBFuel.Size = New System.Drawing.Size(57, 17)
        Me.CBFuel.TabIndex = 24
        Me.CBFuel.Text = "% Fuel"
        Me.ToolTip1.SetToolTip(Me.CBFuel, "Click this ON to get correction in % Fuel" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ok leave it OFF for correction in AFR")
        Me.CBFuel.UseVisualStyleBackColor = True
        '
        'Ltit
        '
        Me.Ltit.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Ltit.AutoSize = True
        Me.Ltit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ltit.Location = New System.Drawing.Point(438, 3)
        Me.Ltit.Name = "Ltit"
        Me.Ltit.Size = New System.Drawing.Size(57, 20)
        Me.Ltit.TabIndex = 23
        Me.Ltit.Text = "TITLE"
        Me.Ltit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(17, 331)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(131, 13)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Correction: AFR or % Fuel:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Lz
        '
        Me.Lz.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lz.AutoSize = True
        Me.Lz.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lz.Location = New System.Drawing.Point(978, 291)
        Me.Lz.Name = "Lz"
        Me.Lz.Size = New System.Drawing.Size(61, 20)
        Me.Lz.TabIndex = 26
        Me.Lz.Text = "Target"
        '
        'FTun
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1122, 488)
        Me.ControlBox = False
        Me.Controls.Add(Me.Lz)
        Me.Controls.Add(Me.TBxax)
        Me.Controls.Add(Me.Lx)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CBFuel)
        Me.Controls.Add(Me.Ltit)
        Me.Controls.Add(Me.ButAbort)
        Me.Controls.Add(Me.TabCntl)
        Me.Controls.Add(Me.ButZeroErr)
        Me.Controls.Add(Me.TbAF)
        Me.Controls.Add(Me.ButAcc)
        Me.Controls.Add(Me.TBCorr)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TBCurr)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TBTarg)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ButClose)
        Me.Controls.Add(Me.ButImp)
        Me.Controls.Add(Me.ButExp)
        Me.Controls.Add(Me.TBSet)
        Me.Controls.Add(Me.ButSet)
        Me.Controls.Add(Me.CBAccum)
        Me.Controls.Add(Me.BCfg)
        Me.Controls.Add(Me.TBYax)
        Me.Controls.Add(Me.Ly)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "FTun"
        Me.Text = "Tune Buddy"
        Me.TabCntl.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.Table1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.Table2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        CType(Me.Table3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TabCntl As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Table3 As System.Windows.Forms.DataGridView
    Friend WithEvents Lx As System.Windows.Forms.Label
    Friend WithEvents TBxax As System.Windows.Forms.TextBox
    Friend WithEvents TBYax As System.Windows.Forms.TextBox
    Friend WithEvents Ly As System.Windows.Forms.Label
    Friend WithEvents BCfg As System.Windows.Forms.Button
    Friend WithEvents Table1 As System.Windows.Forms.DataGridView
    Friend WithEvents Table2 As System.Windows.Forms.DataGridView
    Friend WithEvents CBAccum As System.Windows.Forms.CheckBox
    Friend WithEvents ButSet As System.Windows.Forms.Button
    Friend WithEvents TBSet As System.Windows.Forms.TextBox
    Friend WithEvents ButExp As System.Windows.Forms.Button
    Friend WithEvents ButImp As System.Windows.Forms.Button
    Friend WithEvents ButClose As System.Windows.Forms.Button
    Friend WithEvents TBTarg As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TBCurr As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TBCorr As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TbAF As System.Windows.Forms.TextBox
    Friend WithEvents ButAcc As System.Windows.Forms.Button
    Friend WithEvents ButZeroErr As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ButAbort As System.Windows.Forms.Button
    Friend WithEvents Ltit As System.Windows.Forms.Label
    Friend WithEvents CBFuel As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Lz As System.Windows.Forms.Label
End Class
