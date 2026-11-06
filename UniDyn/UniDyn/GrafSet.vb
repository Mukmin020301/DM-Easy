Public Class GrafSet

    Public Event SetGraf()
    Sub MyForm_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible Then Console.WriteLine("Graf Setup Started.")
        LBY1.ForeColor = UniDyn.Col(UniDyn.Tcol(1))
        LBY2.ForeColor = UniDyn.Col(UniDyn.Tcol(2))
        LBy3.ForeColor = UniDyn.Col(UniDyn.Tcol(3))
        LBy4.ForeColor = UniDyn.Col(UniDyn.Tcol(11))


        LBZ1.ForeColor = UniDyn.Col(UniDyn.Tcol(4))
        LBZ2.ForeColor = UniDyn.Col(UniDyn.Tcol(5))
        LBZ3.ForeColor = UniDyn.Col(UniDyn.Tcol(6))
        LBZ4.ForeColor = UniDyn.Col(UniDyn.Tcol(12))
        Label5.ForeColor = UniDyn.Col(UniDyn.Tcol(7))
        Label10.ForeColor = UniDyn.Col(UniDyn.Tcol(8))
        Label12.ForeColor = UniDyn.Col(UniDyn.Tcol(9))
        Label11.ForeColor = UniDyn.Col(UniDyn.Tcol(10))

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        RaiseEvent SetGraf()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Hide()
    End Sub

    '   Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bgraf.Click
    '       DynoMonitor.grafgrid = Not (DynoMonitor.grafgrid)
    '       If DynoMonitor.grafgrid = True Then
    '           Bgraf.Text = "FINE"
    '       Else
    '           Bgraf.Text = "CORSE"
    '       End If
    '
    '    End Sub

    '    Private Sub ButAnaGraf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButAnaGraf.Click
    '        DynoMonitor.AnaGrTop = Not (DynoMonitor.AnaGrTop)
    '        If DynoMonitor.AnaGrTop = True Then
    '            ButAnaGraf.Text = "COMBINED"
    '        Else
    '            ButAnaGraf.Text = "SEPERATE"
    '        End If
    '    End Sub

    '    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButBS.Click
    '        DynoMonitor.BigSum = Not (DynoMonitor.BigSum)
    '        If DynoMonitor.BigSum = True Then
    '            ButBS.Text = "ON"
    '        Else
    '            ButBS.Text = "OFF"
    '        End If
    '    End Sub

    '    Private Sub ButSame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButSame.Click
    '        DynoMonitor.GAxSame = Not (DynoMonitor.GAxSame)
    '        If DynoMonitor.GAxSame = True Then
    '            ButSame.Text = "SAME"
    '            LB5.BackColor = Color.LightGray
    '            LB6.BackColor = Color.LightGray
    '            LB7.BackColor = Color.LightGray
    '            LB8.BackColor = Color.LightGray
    '        Else
    '            ButSame.Text = "DIFFERENT"
    '            LB5.BackColor = Color.White
    '            LB6.BackColor = Color.White
    '            LB7.BackColor = Color.White
    '            LB8.BackColor = Color.White
    '        End If
    '    End Sub

    '   Private Sub But2Last_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles But2Last.Click
    '       DynoMonitor.G2Last = Not (DynoMonitor.G2Last)
    '       If DynoMonitor.G2Last = True Then
    '           But2Last.Text = "YES"
    '       Else
    '           But2Last.Text = "NO"
    '       End If
    '   End Sub

    Private Sub ButXA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButXA.Click
        UniDyn.GManX = Not (UniDyn.GManX)
        If UniDyn.GManX = True Then
            TBX1.Text = UniDyn.GrX1

        Else
            TBX1.Text = "AUTO"

        End If
    End Sub

    Private Sub ButYA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButYA.Click
        UniDyn.GmanY = Not (UniDyn.GmanY)
        If UniDyn.GmanY = True Then
            TBY2.Text = UniDyn.GrY2
        Else
            TBY2.Text = "AUTO"
        End If
    End Sub

    Private Sub ButY2A_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButY2A.Click
        UniDyn.Gman2Y = Not (UniDyn.Gman2Y)
        If UniDyn.Gman2Y = True Then
            TB2Y2.Text = UniDyn.Gr2Y2
        Else
            TB2Y2.Text = "AUTO"
        End If
    End Sub

    '    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButBaCol.Click
    '        DynoMonitor.GBaCol += 1
    '        If DynoMonitor.GBaCol >= 5 Then DynoMonitor.GBaCol = 0
    '        ButBaCol.BackColor = DynoMonitor.ColGrafBak(DynoMonitor.GBaCol)
    '    End Sub

    Private Sub GrafSet_Load(ByVal sender As System.Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True         'This cancels the closing
        Me.Hide()
    End Sub

    Private Sub LBX1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBX1.Click  ' X Axis so no change of colors
        'UniDyn.Tcol(1) += 1
        'If UniDyn.Tcol(1) > 18 Then UniDyn.Tcol(1) = 1
        'LBX1.ForeColor = UniDyn.Col(UniDyn.Tcol(1))
    End Sub

    Private Sub LBY1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBY1.Click
        UniDyn.Tcol(1) += 1
        If UniDyn.Tcol(1) > 18 Then UniDyn.Tcol(1) = 1
        LBY1.ForeColor = UniDyn.Col(UniDyn.Tcol(1))
    End Sub

    Private Sub LBY2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBY2.Click
        UniDyn.Tcol(2) += 1
        If UniDyn.Tcol(2) > 18 Then UniDyn.Tcol(2) = 1
        LBY2.ForeColor = UniDyn.Col(UniDyn.Tcol(2))
    End Sub

    Private Sub LBy3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBy3.Click
        UniDyn.Tcol(3) += 1
        If UniDyn.Tcol(3) > 18 Then UniDyn.Tcol(3) = 1
        LBy3.ForeColor = UniDyn.Col(UniDyn.Tcol(3))
    End Sub

    Private Sub LBX2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBX2.Click
        '  UniDyn.Tcol(4) += 1
        ' If UniDyn.Tcol(4) > 18 Then UniDyn.Tcol(4) = 1
        'LBX2.ForeColor = UniDyn.Col(UniDyn.Tcol(4))
    End Sub

    Private Sub LBZ1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBZ1.Click
        UniDyn.Tcol(4) += 1
        If UniDyn.Tcol(4) > 18 Then UniDyn.Tcol(4) = 1
        LBZ1.ForeColor = UniDyn.Col(UniDyn.Tcol(4))
    End Sub

    Private Sub LBZ2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBZ2.Click
        UniDyn.Tcol(5) += 1
        If UniDyn.Tcol(5) > 18 Then UniDyn.Tcol(5) = 1
        LBZ2.ForeColor = UniDyn.Col(UniDyn.Tcol(5))
    End Sub

    Private Sub LBZ3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBZ3.Click
        UniDyn.Tcol(6) += 1
        If UniDyn.Tcol(6) > 18 Then UniDyn.Tcol(6) = 1
        LBZ3.ForeColor = UniDyn.Col(UniDyn.Tcol(6))
    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click
        UniDyn.Tcol(7) += 1
        If UniDyn.Tcol(7) > 18 Then UniDyn.Tcol(7) = 1
        Label5.ForeColor = UniDyn.Col(UniDyn.Tcol(7))
    End Sub

    Private Sub Label10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label10.Click
        UniDyn.Tcol(8) += 1
        If UniDyn.Tcol(8) > 18 Then UniDyn.Tcol(8) = 1
        Label10.ForeColor = UniDyn.Col(UniDyn.Tcol(8))
    End Sub

    Private Sub Label12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label12.Click
        UniDyn.Tcol(9) += 1
        If UniDyn.Tcol(9) > 18 Then UniDyn.Tcol(9) = 1
        Label12.ForeColor = UniDyn.Col(UniDyn.Tcol(9))
    End Sub

    Private Sub Label11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label11.Click
        UniDyn.Tcol(10) += 1
        If UniDyn.Tcol(10) > 18 Then UniDyn.Tcol(10) = 1
        Label11.ForeColor = UniDyn.Col(UniDyn.Tcol(10))
    End Sub

    Private Sub LBy4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBy4.Click
        UniDyn.Tcol(11) += 1
        If UniDyn.Tcol(11) > 18 Then UniDyn.Tcol(11) = 1
        LBy4.ForeColor = UniDyn.Col(UniDyn.Tcol(11))
    End Sub

    Private Sub LBZ4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBZ4.Click
        UniDyn.Tcol(12) += 1
        If UniDyn.Tcol(12) > 18 Then UniDyn.Tcol(12) = 1
        LBZ4.ForeColor = UniDyn.Col(UniDyn.Tcol(12))
    End Sub
End Class