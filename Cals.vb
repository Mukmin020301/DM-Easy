Public Class Cals
    Public Event setcal()
    Public Event OpenCC()
    Public Event EIchanged()
    Public Event autofrict()
    Public Event ChanChange()
    Public Event Calcode()
    Public Event LockCheck()
    Public Sub inkey(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Console.WriteLine("Key = " + Str(Asc(e.KeyChar)))                   'Must turn "Form.Keypreview = Ture" to use this

        If Asc(e.KeyChar) = 27 Then     'ESC
            e.Handled = True
            Me.Close()
        End If

        If Asc(e.KeyChar) = 13 Then     'ENTER
            e.Handled = True
            RaiseEvent setcal()
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()               'Just close the window
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        RaiseEvent setcal()             'Let main program take care of setting the parameters
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        RaiseEvent OpenCC()
    End Sub
    Private Sub Block_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Block.Click
        RaiseEvent LockCheck()
    End Sub

    Private Sub TBEngInert_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBEngInert.TextChanged
        RaiseEvent EIChanged()
    End Sub

    Private Sub TBgr_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBgr.TextChanged
        RaiseEvent EIchanged()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButAF.Click
        RaiseEvent autofrict()
    End Sub
    Private Sub Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True         'This cancels the closing
        Me.Hide()
    End Sub

    Private Sub ChaName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChaName.SelectedIndexChanged
        RaiseEvent ChanChange()
    End Sub

    Private Sub ButCalEnab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButCalEnab.Click
        RaiseEvent Calcode()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButColSkin.Click
        DynoMonitor.Cskin += 1
        If DynoMonitor.Cskin = 5 Then DynoMonitor.Cskin = 0
        ButColSkin.BackColor = DynoMonitor.ColSkin(DynoMonitor.Cskin)

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButColGage.Click
        DynoMonitor.cgage += 1
        If DynoMonitor.cgage = 5 Then DynoMonitor.cgage = 0
        ButColGage.BackColor = DynoMonitor.ColGage(DynoMonitor.cgage)
    End Sub

    'Private Sub Button4_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Block.Click
    '    Dim CSign, CtrSign, Cresp, Ctmp As Integer
    '    CSign = Int(Rnd(1) * 65535)
    '    Cresp = InputBox("CODE: " + Str(CSign), "ENTER COUNTERSIGN", 0)
    '    CtrSign = CSign Xor 53955
    '    'MsgBox("Xor Resp: " + Str(CtrSign))
    '    CtrSign = CtrSign * 16
    '    'MsgBox("x8 Resp: " + Str(CtrSign))
    '    Ctmp = Int(CtrSign / 65536)
    '    CtrSign = CtrSign And 65535
    '    CtrSign = CtrSign + Ctmp
    '    CtrSign = CtrSign And 65535
    '    'MsgBox("x8+Rollover n Truck Resp: " + Str(CtrSign))

    '    CtrSign = CtrSign + 1355

    '    'Console.WriteLine("Resp: " + Str(CtrSign))
    '    'MsgBox("Final Resp: " + Str(CtrSign))
    '    If Cresp = CtrSign Then
    '        Block.BackColor = Color.Green
    '    Else
    '        Block.BackColor = Color.Red
    '    End If

    'End Sub
End Class