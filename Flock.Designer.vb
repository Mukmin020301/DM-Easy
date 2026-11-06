<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Flock
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Flock))
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Lstat = New System.Windows.Forms.Label()
        Me.Lhours = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Lcode = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TBScode = New System.Windows.Forms.TextBox()
        Me.Bsend = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Lcounter = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(316, 245)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 193
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 20)
        Me.Label1.TabIndex = 194
        Me.Label1.Text = "STATUS:"
        '
        'Lstat
        '
        Me.Lstat.AutoSize = True
        Me.Lstat.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lstat.Location = New System.Drawing.Point(101, 20)
        Me.Lstat.Name = "Lstat"
        Me.Lstat.Size = New System.Drawing.Size(45, 20)
        Me.Lstat.TabIndex = 195
        Me.Lstat.Text = "UNK"
        '
        'Lhours
        '
        Me.Lhours.AutoSize = True
        Me.Lhours.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lhours.Location = New System.Drawing.Point(101, 52)
        Me.Lhours.Name = "Lhours"
        Me.Lhours.Size = New System.Drawing.Size(45, 20)
        Me.Lhours.TabIndex = 197
        Me.Lhours.Text = "UNK"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(14, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 20)
        Me.Label3.TabIndex = 196
        Me.Label3.Text = "HOURS:"
        '
        'Lcode
        '
        Me.Lcode.AutoSize = True
        Me.Lcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lcode.Location = New System.Drawing.Point(101, 84)
        Me.Lcode.Name = "Lcode"
        Me.Lcode.Size = New System.Drawing.Size(45, 20)
        Me.Lcode.TabIndex = 199
        Me.Lcode.Text = "UNK"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 84)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 20)
        Me.Label5.TabIndex = 198
        Me.Label5.Text = "C-CODE:"
        '
        'TBScode
        '
        Me.TBScode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TBScode.Location = New System.Drawing.Point(105, 123)
        Me.TBScode.Name = "TBScode"
        Me.TBScode.Size = New System.Drawing.Size(205, 26)
        Me.TBScode.TabIndex = 200
        Me.ToolTip1.SetToolTip(Me.TBScode, "Enter your S-CODE here." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If you don't have a valid S-CODE don't enter anything!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & _
                "Too many incorrect codes, and you will be locked out!")
        '
        'Bsend
        '
        Me.Bsend.Location = New System.Drawing.Point(316, 123)
        Me.Bsend.Name = "Bsend"
        Me.Bsend.Size = New System.Drawing.Size(51, 23)
        Me.Bsend.TabIndex = 201
        Me.Bsend.Text = "SEND"
        Me.Bsend.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 165)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(393, 65)
        Me.Label6.TabIndex = 202
        Me.Label6.Text = resources.GetString("Label6.Text")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 126)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 20)
        Me.Label2.TabIndex = 203
        Me.Label2.Text = "S-CODE:"
        '
        'Lcounter
        '
        Me.Lcounter.AutoSize = True
        Me.Lcounter.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lcounter.Location = New System.Drawing.Point(58, 245)
        Me.Lcounter.Name = "Lcounter"
        Me.Lcounter.Size = New System.Drawing.Size(82, 20)
        Me.Lcounter.TabIndex = 204
        Me.Lcounter.Text = "S-CODE:"
        '
        'Flock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(398, 270)
        Me.Controls.Add(Me.Lcounter)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Bsend)
        Me.Controls.Add(Me.TBScode)
        Me.Controls.Add(Me.Lcode)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Lhours)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Lstat)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button2)
        Me.Name = "Flock"
        Me.Text = "CONTROLLER LOCK CONTROL"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Lstat As System.Windows.Forms.Label
    Friend WithEvents Lhours As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Lcode As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TBScode As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Bsend As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Lcounter As System.Windows.Forms.Label
End Class
