Class DrvCycle
    Public Event closedW()
    Private Sub ButAnaClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButAnaClose.Click
        RaiseEvent closedW()
    End Sub
End Class