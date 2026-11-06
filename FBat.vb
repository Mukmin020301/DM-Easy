Public Class FBat
    Public Event BatDiagCont()
    Private Sub ButCont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButCont.Click
        RaiseEvent BatDiagCont()
    End Sub
End Class