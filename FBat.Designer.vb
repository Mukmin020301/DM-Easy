<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FBat
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FBat))
        Me.ButCont = New System.Windows.Forms.Button()
        Me.BatDiagTB = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ButCont
        '
        Me.ButCont.Location = New System.Drawing.Point(199, 65)
        Me.ButCont.Name = "ButCont"
        Me.ButCont.Size = New System.Drawing.Size(75, 23)
        Me.ButCont.TabIndex = 0
        Me.ButCont.Text = "N/A"
        Me.ButCont.UseVisualStyleBackColor = True
        '
        'BatDiagTB
        '
        Me.BatDiagTB.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BatDiagTB.Location = New System.Drawing.Point(16, 27)
        Me.BatDiagTB.Name = "BatDiagTB"
        Me.BatDiagTB.Size = New System.Drawing.Size(443, 29)
        Me.BatDiagTB.TabIndex = 1
        Me.BatDiagTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'FBat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(471, 100)
        Me.ControlBox = False
        Me.Controls.Add(Me.BatDiagTB)
        Me.Controls.Add(Me.ButCont)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FBat"
        Me.Text = "Batch Program Operator Instructions"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButCont As System.Windows.Forms.Button
    Friend WithEvents BatDiagTB As System.Windows.Forms.TextBox
End Class
