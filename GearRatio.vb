Public Class GearRatio
    Public Event setgr()
    Public grs As Double
    Public speed1, speedC2, speedco As Integer
    Sub main()

        TBRPM.Text = Str(Int(Speed1 * Val(TBFact.Text)))                    'Show online while adjusting ratio
        If Speed1 > 1 Then
            If RadioButton1.Checked Then TBRat.Text = Str(Int(100000 / speed1) / 100)
            If RadioButton2.Checked Then TBRat.Text = Str(Int(200000 / speed1) / 100)
            If RadioButton3.Checked Then TBRat.Text = Str(Int(400000 / speed1) / 100)
            If RadioButton4.Checked Then TBRat.Text = Str(Int(600000 / speed1) / 100)
            If RadioButton5.Checked Then TBRat.Text = Str(Int(800000 / speed1) / 100)
        End If
        If speedC2 <> 0 Then
            TBRPM2.Text = speedC2
            If Speed1 <> 0 Then TBF2.Text = Int(speedC2 * 100 / Speed1) / 100
        End If
        If speedCo <> 0 Then
            TbRPMo.Text = speedCo
            If speed1 <> 0 Then TBFo.Text = Int(speedCo * 100 / speed1) / 100
        End If
    End Sub
    Public Sub inkey(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Console.WriteLine("Key = " + Str(Asc(e.KeyChar)))                   'Must turn "Form.Keypreview = Ture" to use this

        If Asc(e.KeyChar) = 27 Then
            e.Handled = True
            Me.Close()
        End If

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            Call GRSet(Nothing, Nothing)
        End If
    End Sub
    Private Sub GRSet(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButSet.Click
        If speed1 < 1 Then speed1 = 1
        If RadioButton1.Checked Then gr = 1000 / speed1
        If RadioButton2.Checked Then gr = 2000 / speed1
        If RadioButton3.Checked Then gr = 4000 / speed1
        If RadioButton4.Checked Then gr = 6000 / speed1
        If RadioButton5.Checked Then gr = 8000 / speed1
        If RadioButton6.Checked Then gr = Val(TBFact.Text)
        If RadioButton7.Checked Then gr = Val(TBF2.Text)
        If RadioButton8.Checked Then gr = Val(TBFo.Text)
        grs = gr
        RaiseEvent setgr()
        'MsgBox("Setting to " + Str(gr))


        Me.Hide()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButExit.Click ' Cancel
        Me.Hide()
    End Sub
End Class