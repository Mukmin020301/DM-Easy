Public Class FtuneCal
    Public Event closeTunecal()
    Public Event aborttunecal()
    Private Sub ButClCfg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButClCfg.Click
        RaiseEvent closeTunecal()
    End Sub

    Private Sub ButAbortCfg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButAbortCfg.Click
        RaiseEvent aborttunecal()
    End Sub
End Class