Public Class Ldunno
    Public Event setSel()


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButCancel.Click
        Me.Hide()
    End Sub

    Private Sub BSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSet.Click
        RaiseEvent setSel()
    End Sub
End Class