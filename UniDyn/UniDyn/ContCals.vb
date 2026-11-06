Public Class ContCals
    'Especially important as we no longer have the front pannel for setup
    Public Event SetCC()
    Public Event Burn()
    Public Event CCExit()
    Public Event CTare()
    Public Event CalSet()


    Private Sub BSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSend.Click
        RaiseEvent SetCC()
    End Sub
    Private Sub BBurn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BBurn.Click
        RaiseEvent Burn()
    End Sub
    Private Sub BExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bexit.Click
        RaiseEvent CCExit()
    End Sub

    Private Sub BuTare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuTare.Click
        RaiseEvent CTare()
    End Sub

    Private Sub BuSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuSet.Click
        RaiseEvent CalSet()
    End Sub
    Private Sub Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True         'This cancels the closing
        RaiseEvent CCExit()
    End Sub

    Private Sub ContCals_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class