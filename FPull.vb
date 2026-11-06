Public Class FPull
    Public Event ex()
    Public Event fact()
    Public Event clr()
    Public Event goauto()
    Public Event pprint()
    Public Event psave()
    Public Event Pcomp()
    Dim CW As Integer = Me.Width ' Current Width
    Dim CH As Integer = Me.Height ' Current Height
    Dim IW As Integer = Me.Width ' Initial Width
    Dim IH As Integer = Me.Height ' Initial Height


    Private Sub Fpull_Resize(ByVal sender As Object, _
            ByVal e As System.EventArgs) Handles Me.Resize

        Dim RW As Double = (Me.Width - CW) / CW ' Ratio change of width
        Dim RH As Double = (Me.Height - CH) / CH ' Ratio change of height

        For Each Ctrl As Control In Controls
            Ctrl.Width += CInt(Ctrl.Width * RW)
            Ctrl.Height += CInt(Ctrl.Height * RH)
            Ctrl.Left += CInt(Ctrl.Left * RW)
            Ctrl.Top += CInt(Ctrl.Top * RH)
        Next
        '     Panel1.Width += CInt(Panel1.Width * RW)
        '     Panel1.Height += CInt(Panel1.Height * RH)
        '     Panel1.Left += CInt(Panel1.Left * RW)
        '     Panel1.Top += CInt(Panel1.Top * RH)

        '  Chart1.Width += CInt(Chart1.Width * RW)
        '  Chart1.Height += CInt(Chart1.Height * RH)
        '  Chart1.Left += CInt(Chart1.Left * RW)
        '  Chart1.Top += CInt(Chart1.Top * RH)

        '        Chart2.Width += CInt(Chart2.Width * RW)
        '        Chart2.Height += CInt(Chart2.Height * RH)
        '        Chart2.Left += CInt(Chart2.Left * RW)
        '        Chart2.Top += CInt(Chart2.Top * RH)

        'Me.MSChart1.Move 0, 0, ScaleWidth, ScaleHeight
        CW = Me.Width
        CH = Me.Height

    End Sub

    Private Sub Fpull_Load(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles MyBase.Load

        IW = Me.Width
        IH = Me.Height
    End Sub
    Private Sub BExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BExit.Click
        RaiseEvent ex()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        RaiseEvent clr()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButRPMFact.Click
        RaiseEvent fact()
    End Sub

    Private Sub ButStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButStart.Click
        RaiseEvent goauto()
    End Sub
    Private Sub ButLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButComp.Click
        RaiseEvent Pcomp()
    End Sub

    Private Sub butPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPrint.Click
        RaiseEvent PPrint()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        RaiseEvent PSave()
    End Sub

    Private Sub Chart1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart1.Click

    End Sub

    Private Sub ltmaxr4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ltmaxr4.Click

    End Sub

    Private Sub ltmaxr3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ltmaxr3.Click

    End Sub
End Class