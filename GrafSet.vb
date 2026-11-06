Public Class GrafSet
    Public Event SetGraf()
    Sub MyForm_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible Then Console.WriteLine("Graf Setup Started.")
        LBY1.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(1))
        LBY2.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(2))
        LBy3.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(3))
        LBY4.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(4))
        LBZ2.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(6))
        LBZ1.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(5))
        LBZ3.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(7))
        LBZ4.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(8))
    End Sub
    Public Sub inkey(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Console.WriteLine("Key = " + Str(Asc(e.KeyChar)))                   'Must turn "Form.Keypreview = Ture" to use this

        If Asc(e.KeyChar) = 27 Then
            e.Handled = True
            Me.Close()
        End If

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            RaiseEvent SetGraf()
        End If
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        RaiseEvent SetGraf()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Hide()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bgraf.Click
        DynoMonitor.grafgrid = Not (DynoMonitor.grafgrid)
        If DynoMonitor.grafgrid = True Then
            Bgraf.Text = "FINE"
        Else
            Bgraf.Text = "CORSE"
        End If
     
    End Sub

    Private Sub ButAnaGraf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButAnaGraf.Click
        DynoMonitor.AnaGrTop = Not (DynoMonitor.AnaGrTop)
        If DynoMonitor.AnaGrTop = True Then
            ButAnaGraf.Text = "COMBINED"
        Else
            ButAnaGraf.Text = "SEPERATE"
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButBS.Click
        DynoMonitor.BigSum = Not (DynoMonitor.BigSum)
        If DynoMonitor.BigSum = True Then
            ButBS.Text = "ON"
        Else
            ButBS.Text = "OFF"
        End If
    End Sub

    Private Sub ButSame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButSame.Click
        DynoMonitor.GAxSame = Not (DynoMonitor.GAxSame)
        If DynoMonitor.GAxSame = True Then
            ButSame.Text = "SAME"
            LB6.BackColor = Color.LightGray
            LB7.BackColor = Color.LightGray
            LB8.BackColor = Color.LightGray
            LB9.BackColor = Color.LightGray
            LB10.BackColor = Color.LightGray
        Else
            ButSame.Text = "DIFFERENT"
            LB6.BackColor = Color.White
            LB7.BackColor = Color.White
            LB8.BackColor = Color.White
            LB9.BackColor = Color.White
            LB10.BackColor = Color.White
        End If
    End Sub

    Private Sub But2Last_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles But2Last.Click
        DynoMonitor.G2Last = Not (DynoMonitor.G2Last)
        If DynoMonitor.G2Last = True Then
            But2Last.Text = "YES"
        Else
            But2Last.Text = "NO"
        End If
    End Sub

    Private Sub ButXA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButXA.Click
        DynoMonitor.GManX = Not (DynoMonitor.GManX)
        If DynoMonitor.GManX = True Then
            TBX1.Text = DynoMonitor.GRx1
            LAX1.Text = "MANUAL"
            TBX1.BackColor = Color.White
        Else
            LAX1.Text = "AUTO"
            TBX1.BackColor = Color.LightGray
        End If
    End Sub

    Private Sub ButYA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButYA.Click
        DynoMonitor.GManY = Not (DynoMonitor.GManY)
        If DynoMonitor.GManY = True Then
            TBY2.Text = DynoMonitor.GRy2
            LAY1.Text = "MANUAL"
            TBY2.BackColor = Color.White
        Else
            LAY1.Text = "AUTO"
            TBY2.BackColor = Color.LightGray
        End If
    End Sub

    Private Sub ButY2A_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButY2A.Click
        DynoMonitor.GMan2Y = Not (DynoMonitor.GMan2Y)
        If DynoMonitor.GMan2Y = True Then
            TB2Y2.Text = DynoMonitor.GR2Y2
            LAY2.Text = "MANUAL"
            TB2Y2.BackColor = Color.White
        Else
            LAY2.Text = "AUTO"
            TB2Y2.BackColor = Color.LightGray
        End If
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButBaCol.Click
        DynoMonitor.GBaCol += 1
        If DynoMonitor.GBaCol >= 5 Then DynoMonitor.GBaCol = 0
        ButBaCol.BackColor = DynoMonitor.ColGrafBak(DynoMonitor.GBaCol)
    End Sub

    Private Sub GrafSet_Load(ByVal sender As System.Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True         'This cancels the closing
        Me.Hide()
    End Sub

    Private Sub BACGraf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BACGraf.Click
        DynoMonitor.AutoCloseGraf = Not (DynoMonitor.AutoCloseGraf)
        If DynoMonitor.AutoCloseGraf = True Then
            BACGraf.Text = "YES"
        Else
            BACGraf.Text = "NO"
        End If
    End Sub

    Private Sub BA1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BA1.Click
        DynoMonitor.AutoAnaY1 = Not (DynoMonitor.AutoAnaY1)
        If DynoMonitor.AutoAnaY1 = True Then
            LAA1.Text = "CALS"
            TBA1Y1.BackColor = Color.LightGray
            TBA1Y2.BackColor = Color.LightGray
        Else
            LAA1.Text = "MANUAL"
            TBA1Y1.BackColor = Color.White
            TBA1Y2.BackColor = Color.White
        End If
    End Sub

    Private Sub BA2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BA2.Click
        DynoMonitor.AutoAnaY2 = Not (DynoMonitor.AutoAnaY2)
        If DynoMonitor.AutoAnaY2 = True Then
            LAA2.Text = "CALS"
            TBA2Y1.BackColor = Color.LightGray
            TBA2Y2.BackColor = Color.LightGray
        Else
            LAA2.Text = "MANUAL"
            TBA2Y1.BackColor = Color.White
            TBA2Y2.BackColor = Color.White
        End If
    End Sub

    Private Sub LBY1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBY1.Click
        DynoMonitor.Tcol(1) += 1
        If DynoMonitor.Tcol(1) > 18 Then DynoMonitor.Tcol(1) = 1
        LBY1.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(1))
    End Sub

    Private Sub LBY2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBY2.Click
        DynoMonitor.Tcol(2) += 1
        If DynoMonitor.Tcol(2) > 18 Then DynoMonitor.Tcol(2) = 1
        LBY2.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(2))
    End Sub

    Private Sub LBy3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBy3.Click
        DynoMonitor.Tcol(3) += 1
        If DynoMonitor.Tcol(3) > 18 Then DynoMonitor.Tcol(3) = 1
        LBy3.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(3))
    End Sub

    Private Sub LBY4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBY4.Click
        DynoMonitor.Tcol(4) += 1
        If DynoMonitor.Tcol(4) > 18 Then DynoMonitor.Tcol(4) = 1
        LBY4.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(4))
    
    End Sub
    Private Sub LBZ1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBZ1.Click
        DynoMonitor.Tcol(5) += 1
        If DynoMonitor.Tcol(5) > 18 Then DynoMonitor.Tcol(5) = 1
        LBZ1.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(5))
    End Sub

    Private Sub LBZ2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBZ2.Click
        DynoMonitor.Tcol(6) += 1
        If DynoMonitor.Tcol(6) > 18 Then DynoMonitor.Tcol(6) = 1
        LBZ2.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(6))
    End Sub

    Private Sub LBZ3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBZ3.Click
        DynoMonitor.Tcol(7) += 1
        If DynoMonitor.Tcol(7) > 18 Then DynoMonitor.Tcol(7) = 1
        LBZ3.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(7))
    End Sub

    Private Sub LBZ4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBZ4.Click
        DynoMonitor.Tcol(8) += 1
        If DynoMonitor.Tcol(8) > 18 Then DynoMonitor.Tcol(8) = 1
        LBZ4.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(8))
    End Sub
End Class