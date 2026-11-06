Public Class FOBD
    Public Event OBDOpen()
    Public Event OBDSav()
    Public Event OBDclr()
    Public Event OBDchk()
    Private Sub Bopen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bopen.Click
        RaiseEvent OBDOpen()
    End Sub

    Private Sub Bcansel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bcansel.Click
        Me.Hide()
    End Sub

    Private Sub Bsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bsave.Click
        RaiseEvent OBDSav()
    End Sub
    Private Sub Bclr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bclr.Click
        RaiseEvent OBDclr()
    End Sub
    Private Sub Bchk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bchk.Click
        RaiseEvent OBDchk()
    End Sub
End Class