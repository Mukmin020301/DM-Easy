Public Class FOBD

    Public Event OBDOpen()
    Public Event OBDSav()
    Public Event OBDclr()
    Public Event OBDchk()
    Public Event OBDSndI()

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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        RaiseEvent OBDSndI()
    End Sub

    Private Sub Bpar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bpar.Click
        Select Case Bpar.Text                   'Just toggle through the options
            Case "N" : Bpar.Text = "E"
            Case "E" : Bpar.Text = "O"
            Case "O" : Bpar.Text = "M"
            Case "M" : Bpar.Text = "S"
            Case "S" : Bpar.Text = "N"
        End Select
    End Sub
End Class