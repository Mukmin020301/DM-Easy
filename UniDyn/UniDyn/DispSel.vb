Public Class DispSel
    Public Event setsel()

    Private Sub Bset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bset.Click
        RaiseEvent setsel()
    End Sub

    Private Sub Bcan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bcan.Click
        Hide()
    End Sub
End Class