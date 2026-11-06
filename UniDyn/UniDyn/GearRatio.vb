Public Class GearRatio
    Public Event setgr()
    Public grs As Double
    Public speed1, speedC2 As Integer
    Sub main()
        'TBRPM.Text = Str(Int(Speed1))
        TBRPM.Text = Str(Int(speed1 * Val(TBFact.Text)))                    'Show online while adjusting ratio
        If Speed1 > 1 Then
            If RadioButton1.Checked Then TBRat.Text = Str(Int(50000 / Speed1) / 100)
            If RadioButton2.Checked Then TBRat.Text = Str(Int(100000 / Speed1) / 100)
            If RadioButton3.Checked Then TBRat.Text = Str(Int(200000 / Speed1) / 100)
            If RadioButton4.Checked Then TBRat.Text = Str(Int(400000 / Speed1) / 100)
            If RadioButton5.Checked Then TBRat.Text = Str(Int(600000 / speed1) / 100)
            If RadioButton8.Checked Then TBRat.Text = Str(Int(800000 / speed1) / 100)
        End If
        If speedC2 <> 0 Then
            TBRPM2.Text = speedC2
            If speed1 <> 0 Then TBF2.Text = Int(speedC2 * 100 / speed1) / 100
        End If

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButSet.Click
        If Speed1 < 1 Then Speed1 = 1
        If RadioButton1.Checked Then gr = 500 / Speed1
        If RadioButton2.Checked Then gr = 1000 / Speed1
        If RadioButton3.Checked Then gr = 2000 / Speed1
        If RadioButton4.Checked Then gr = 4000 / Speed1
        If RadioButton5.Checked Then gr = 6000 / speed1
        If RadioButton8.Checked Then gr = 8000 / speed1
        If RadioButton6.Checked Then gr = Val(TBFact.Text)
        If RadioButton7.Checked Then gr = Val(TBF2.Text)
        grs = gr
        RaiseEvent setgr()
        'MsgBox("Setting to " + Str(gr))
        Me.Hide()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButExit.Click ' Cancel
        Me.Hide()
    End Sub
    Public Sub inkey(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Console.WriteLine("Key = " + Str(Asc(e.KeyChar)))                   'Must turn "Form.Keypreview = Ture" to use this

        If Asc(e.KeyChar) = 27 Then
            e.Handled = True
            Me.Hide()
        End If

        If Asc(e.KeyChar) = 13 Or Asc(e.KeyChar) = 32 Then
            e.Handled = True
            Call Button1_Click(Nothing, Nothing)
        End If
    End Sub
End Class