Public Class Fgraf
    Public Event QuitInst()
    Private Sub Fgraf_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Me.Size = New System.Drawing.Size(50, 671)
        Dim x As Integer
        Dim y As Integer
        ' x = 0 ' Screen.PrimaryScreen.WorkingArea.Width - 50
        ' y = Screen.PrimaryScreen.WorkingArea.Height - Me.Height                 'Put at lower Left of screen
        ' Me.Location = New Point(x, y)
    End Sub

    Private Sub Chart1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart1.Click
        RaiseEvent quitinst()
    End Sub

    Private Sub Chart2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart2.Click
        RaiseEvent QuitInst()
    End Sub
End Class