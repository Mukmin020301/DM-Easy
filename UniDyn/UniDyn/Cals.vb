Public Class Cals
    Public Event setcal()
    Public Event calchanged()
    Public Event EIchanged()
    Public Event opencc()
    Public Event autofrict()
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()               'Just close the window
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        RaiseEvent setcal()             'Let main program take care of setting the parameters
    End Sub


    Private Sub TBEngInert_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBEngInert.TextChanged
        RaiseEvent EIchanged()
    End Sub

    Private Sub TBgr_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBgr.TextChanged
        RaiseEvent EIchanged()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        RaiseEvent OpenCC()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        RaiseEvent calchanged()         'Every time we switch, let main form know to save old channel cals, and upload new channel cals
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        RaiseEvent autofrict()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        UniDyn.Cskin += 1
        If UniDyn.Cskin = 5 Then UniDyn.Cskin = 0
        UniDyn.BackColor = UniDyn.ColSkin(UniDyn.Cskin)
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        UniDyn.Cback += 1
        If UniDyn.Cback = 5 Then UniDyn.Cback = 0
        UniDyn.Panel2.BackColor = UniDyn.ColBack(UniDyn.Cback)
    End Sub
End Class