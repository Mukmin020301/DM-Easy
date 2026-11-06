Public Class Flock
    Public Event SendLock()
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bsend.Click
        RaiseEvent SendLock()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()               'Just close the window
    End Sub
    Private Sub Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True         'This cancels the closing
        Me.Hide()
    End Sub
End Class