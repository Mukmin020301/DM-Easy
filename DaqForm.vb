Public Class DaqForm
    Public Event closeQW()
    Private Sub ButAnaClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButAnaClose.Click
        RaiseEvent closeQW()
    End Sub

   
    Private Sub DaqForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub
End Class