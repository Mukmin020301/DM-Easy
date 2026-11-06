
Public Class FTun
    Public Event closeTune()
    Private WithEvents FtuneCal As New FtuneCal           'Cal form

    Dim ver, i, j, k, xn, yn, xind, yind, OXI, OYI As Integer                     'Counters n stuff
    Dim title, xtit, ytit, ztit, dummy, s, tablefile As String         'Titles
    Dim xch, ych, zch As String                             'Channels
    Dim x, y, z, avZ, avfact As Double                      'Instantanious X, Y and AFR and Avg
    Dim xb(100), yb(100) As Double                          'Bins
    Dim t(3, 100, 100) As Double                            'Tables
    Dim accum, Ready, fuel As Boolean                               'Accumulate AFR/Correction and Ready to RUn interface? (only true after loading cals)


    Private Sub ButClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButClose.Click
        'Save
        If TuneCal = "" Then
            Dim SaveFileBox As New SaveFileDialog
            With SaveFileBox
                TuneCal = "TuneCal.CSV"
                .FileName = TuneCal
                .DefaultExt = "CSV"
                .Filter = "CSV|*.csv"
                .Title = "Pull Data File"
                Dim result = .ShowDialog()
                If result = DialogResult.OK Then
                Else
                    TuneCal = ""
                End If

            End With
        End If
        If TuneCal <> "" Then
            Using FileWrite As New System.IO.StreamWriter(TuneCal)
                FileWrite.WriteLine("Tune Boss Version," + Str(ver))                    'Header
                FileWrite.WriteLine("Title," + title)
                FileWrite.WriteLine("X Title," + xtit)
                FileWrite.WriteLine("Y Title," + ytit)
                FileWrite.WriteLine("Z Title," + ztit)
                FileWrite.WriteLine("X Channel," + xch)
                FileWrite.WriteLine("Y Channel," + ych)
                FileWrite.WriteLine("Z Channel," + zch)
                FileWrite.WriteLine("Accumulation Factor," + Str(avfact))
                FileWrite.WriteLine("Accumulate?," + Str(accum))
                FileWrite.WriteLine("Table X Cols," + Str(xn))
                FileWrite.WriteLine("Table Y Rows," + Str(yn))

                FileWrite.Write("X Bins")
                For i = 1 To xn
                    FileWrite.Write("," + xb(i).ToString)
                Next
                FileWrite.WriteLine()

                FileWrite.Write("Y Bins")
                For i = 1 To yn
                    FileWrite.Write("," + yb(i).ToString("N2"))
                Next
                FileWrite.WriteLine()

                For j = 1 To yn
                    FileWrite.Write("Targ Row")
                    For i = 1 To xn
                        FileWrite.Write("," + t(1, i, j).ToString("N2"))
                    Next
                    FileWrite.WriteLine()
                Next

                For j = 1 To yn
                    FileWrite.Write("ACT Row")
                    For i = 1 To xn
                        FileWrite.Write("," + t(2, i, j).ToString("N2"))
                    Next
                    FileWrite.WriteLine()
                Next

                For j = 1 To yn
                    FileWrite.Write("CORR Row")
                    For i = 1 To xn
                        FileWrite.Write("," + t(3, i, j).ToString)
                    Next
                    FileWrite.WriteLine()
                Next

                FileWrite.WriteLine("Fuel," + Str(fuel))
                FileWrite.WriteLine("Spare,0")
                FileWrite.WriteLine("Spare,0")
                FileWrite.WriteLine("Spare,0")
                FileWrite.WriteLine("Spare,0")
                FileWrite.WriteLine("Spare,0")
                FileWrite.Close()
                FileWrite.Dispose()
            End Using
        End If
        RaiseEvent closeTune()
    End Sub

    Sub MyForm_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.VisibleChanged
        Ready = False
        If Me.Visible Then
            If TFirst = True Then
                Dim LoadFileBox As New OpenFileDialog
                With LoadFileBox
                    .FileName = TuneCal
                    .DefaultExt = "CSV"
                    .Filter = "CSV|*.CSV"
                    .Title = "Select Tune Configuration File"
                    '  .InitialDirectory = applicationSettingsDirectory
                    Dim result = .ShowDialog()
                    If result = DialogResult.OK Then
                        TuneCal = .FileName
                        TFirst = False
                    End If

                End With
            End If
            If System.IO.File.Exists(TuneCal) = True Then
                Try
                    Using FileRead As New System.IO.StreamReader(TuneCal)
                        ' dummy = FileRead.ReadLine()                    'Header
                        s = FileRead.ReadLine()                        'Ver
                        ver = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                        s = FileRead.ReadLine()                        'Mass not used
                        title = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                        s = FileRead.ReadLine()                        'Diam
                        xtit = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                        s = FileRead.ReadLine()                        'Previously was PPR and unused
                        ytit = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                        s = FileRead.ReadLine()                        'Dyno Maximum Torque
                        ztit = Mid(s, InStr(s, ",") + 1, Len(s) - 1)             'Dyno Maximum Torque
                        s = FileRead.ReadLine()                        'diam
                        xch = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                        s = FileRead.ReadLine()                        'diam
                        ych = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                        s = FileRead.ReadLine()                        'diam
                        zch = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                        s = FileRead.ReadLine()                        'diam
                        avfact = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                        s = FileRead.ReadLine()                        'diam
                        accum = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                        s = FileRead.ReadLine()                    'LastCal
                        xn = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                        s = FileRead.ReadLine()                        'Type
                        yn = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                        s = FileRead.ReadLine()
                        For i = 1 To xn                                              'Read Analog channels Labels and Factors

                            If Len(s) > 1 Then s = Mid(s, InStr(s, ",") + 1, Len(s) - 1) 'Eliminate first element 
                            xb(i) = Val(s)
                        Next
                        s = FileRead.ReadLine()
                        For i = 1 To yn                                              'Read Analog channels Labels and Factors
                            If Len(s) > 1 Then s = Mid(s, InStr(s, ",") + 1, Len(s) - 1) 'Eliminate first element 
                            yb(i) = Val(s)
                        Next
                        For j = 1 To yn
                            s = FileRead.ReadLine()
                            For i = 1 To xn                                              'Read Analog channels Labels and Factors
                                If Len(s) > 1 Then s = Mid(s, InStr(s, ",") + 1, Len(s) - 1) 'Eliminate first element 
                                t(1, i, j) = Val(s)
                            Next
                        Next
                        For j = 1 To yn
                            s = FileRead.ReadLine()
                            For i = 1 To xn                                              'Read Analog channels Labels and Factors
                                If Len(s) > 1 Then s = Mid(s, InStr(s, ",") + 1, Len(s) - 1) 'Eliminate first element
                                t(2, i, j) = Val(s)
                            Next
                        Next
                        For j = 1 To yn
                            s = FileRead.ReadLine()
                            For i = 1 To xn                                              'Read Analog channels Labels and Factors
                                If Len(s) > 1 Then s = Mid(s, InStr(s, ",") + 1, Len(s) - 1) 'Eliminate first element 
                                t(3, i, j) = Val(s)
                            Next
                        Next
                        s = FileRead.ReadLine()                        'Fuel (vs. AFR) as error
                        fuel = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                        FileRead.Close()            'Gotta use these 2 lines to close the file after reading
                        FileRead.Dispose()
                    End Using
                Catch ex As Exception
                    MsgBox(TuneCal + " Is corrupt, or wrong version. Using Defaults.")            'Use Defaults and create DynoCal file
                    'Call Defaults()
                End Try
                TFirst = False
            Else
                MsgBox(TuneCal + " Does Not Exist. Using Defaults.")            'Use Defaults and create DynoCal file
                ver = 0
                Fuel = False
                xn = 10 : yn = 10
                title = "Tune Assist"
                xtit = "Dyno RPM"
                ytit = "Throttle"
                ztit = "Analog 1"
                xch = "RPM1"
                ych = "TPS"
                zch = "A1"
                avZ = 0
                avfact = 0.8
                accum = True
                TFirst = False
                For i = 1 To 10
                    xb(i) = 800 * i
                    yb(i) = 10 * i
                    For j = 1 To 10
                        t(1, i, j) = 14.7
                        t(2, i, j) = 13.5
                        t(3, i, j) = 0
                    Next
                Next
            End If
        End If
        CBFuel.Checked = fuel
        Ltit.Text = title
        Ly.Text = ytit
        Lx.Text = xtit
        Lz.Text = ztit
        TbAF.Text = Str(avfact)
        CBAccum.Checked = accum
        Table1.ColumnCount = xn : Table2.ColumnCount = xn : Table3.ColumnCount = xn
        Table1.RowCount = yn : Table2.RowCount = yn + 1 : Table3.RowCount = yn + 1
        'Table1.Columns(0).Width = 50 : Table2.Columns(0).Width = 50 : Table3.Columns(0).Width = 50
        Table1.AllowUserToAddRows = False : Table2.AllowUserToAddRows = False : Table3.AllowUserToAddRows = False
        Table1.AllowUserToDeleteRows = False : Table2.AllowUserToDeleteRows = False : Table3.AllowUserToDeleteRows = False
        For i = 1 To xn
            Table1.Columns(i - 1).HeaderText = Str(xb(i))
            Table2.Columns(i - 1).HeaderText = Str(xb(i))
            Table3.Columns(i - 1).HeaderText = Str(xb(i))
            Table1.Columns(i - 1).Width = 50 : Table2.Columns(i - 1).Width = 50 : Table3.Columns(i - 1).Width = 50
            For j = 1 To yn
                If i = 1 Then
                    Table1.Rows(j - 1).HeaderCell.Value = Str(yb(j))
                    Table2.Rows(j - 1).HeaderCell.Value = Str(yb(j))
                    Table3.Rows(j - 1).HeaderCell.Value = Str(yb(j))
                End If
                Table1(i - 1, j - 1).Value = t(1, i, j)
                Table2(i - 1, j - 1).Value = t(2, i, j)
                Table3(i - 1, j - 1).Value = t(3, i, j)
                Table2.Rows(j - 1).Cells(i - 1).Style.Format = "N2"
                Table3.Rows(j - 1).Cells(i - 1).Style.Format = "N2"
            Next
        Next
        'Table1.Columns.MaximumSize = 50
        Ready = True
        'MsgBox("All good")
    End Sub
    Public Sub main()      'This is the main Tune Buddy updating routine and is triggered by a loop through the main display on DM

        If Ready = True Then            'Have to skip the interface while loading cals from file
            '        Extract X- Y- and Z- Coordinates
            Select Case Mid(xch, 1, 1)
                Case "R"        'RPM1, RPM2
                    If xch = "RPM1" Then
                        x = Speed1
                    Else
                        x = speed2
                    End If
                Case "A"        'A1 .. A4
                    x = analog(9 + Val(Mid(xch, 2, 1)))
            End Select

            Select Case Mid(ych, 1, 1)
                Case "T"        'TTL
                    y = throttle / 10
                Case "L"        'Load
                    y = Dynoload
                Case "A"        'A1..A4
                    y = analog(9 + Val(Mid(ych, 2, 1)))
            End Select

            Select Case Mid(zch, 1, 1)
                Case "A"
                    z = analog(9 + Val(Mid(zch, 2, 1)))
                    Console.WriteLine("Tune ZCH: " + Str(Val(9 + Mid(zch, 2, 1))) + " Value:" + Str(analog(9 + Val(Mid(zch, 2, 1)))))
            End Select

            TBxax.Text = x.ToString("N0")
            TBYax.Text = y.ToString("N0")


            'INTERPOLATE
            OXI = xind : OYI = yind                       'Set previous values
            If OXI < 1 Then OXI = 1
            If OYI < 1 Then OYI = 1
            xind = 1 : yind = 1                     'These set the defaults (non interpolated)
            If x > xb(xn - 1) Then xind = xn
            If y > yb(yn - 1) Then yind = yn

            For i = 2 To xn - 1
                If x >= (xb(i - 1) + xb(i)) / 2 And x < (xb(i) + xb(i + 1)) / 2 Then xind = i
            Next
            For j = 2 To yn - 1
                If y >= (yb(j) + yb(j - 1)) / 2 And y < (yb(j) + yb(j + 1)) / 2 Then yind = j
            Next


            t(2, xind, yind) = t(2, xind, yind) * avfact + (1 - avfact) * z         'Assign New Average AFR
            If CBFuel.Checked = False Then
                t(3, xind, yind) = t(2, xind, yind) - t(1, xind, yind)                  'Assign Correction
            Else
                If t(1, xind, yind) <> 0 Then t(3, xind, yind) = t(2, xind, yind) / t(1, xind, yind) 'Assign Correction Factor
            End If


            Table2.Rows(OYI - 1).Cells(OXI - 1).Style.BackColor = Color.White               'Reset
            '  Table2.Rows(yind - 1).Cells(xind - 1).Style.ForeColor = Color.Red            'Color
            Table2.Rows(yind - 1).Cells(xind - 1).Style.BackColor = Color.LightBlue         'Color
            Table3.Rows(OYI - 1).Cells(OXI - 1).Style.BackColor = Color.White               'Reset... May set to different colors depending on the error (red = too lean, green = just nice)
            Table3.Rows(yind - 1).Cells(xind - 1).Style.BackColor = Color.LightBlue         'Color


            TBTarg.Text = t(1, xind, yind).ToString("N2")         'Target
            TBCurr.Text = z.ToString("N2")                      'Current
            TBCorr.Text = t(3, xind, yind).ToString("N2")       'Correction

            If xind > 0 And xind <= xn And yind > 0 And yind <= yn Then 'Update tables if not out of bound
                Table2(xind - 1, yind - 1).Value = t(2, xind, yind)
                Table3(xind - 1, yind - 1).Value = t(3, xind, yind)
            End If


            If TabCntl.SelectedTab.Name = "TabPage3" Then

            End If

        End If
    End Sub

    Private Sub Butset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButSet.Click
        If TBSet.Text <> "" Then
            For i = 1 To xn
                For j = 1 To yn
                    Table1(i - 1, j - 1).Value = Val(TBSet.Text)
                    t(1, i, j) = Val(TBSet.Text)
                Next
            Next
        End If
    End Sub

    Private Sub BCfg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCfg.Click
        'Open Config Screen
        If Ftunecal.Visible Then
            FtuneCal.Hide()
            title = FtuneCal.Titbox.Text
            xtit = FtuneCal.titboxx.Text
            ytit = FtuneCal.titboxy.Text
            ztit = FtuneCal.titboxz.Text
            If FtuneCal.RBX1.Checked Then xch = "RPM1"
            If FtuneCal.RBX2.Checked Then xch = "RPM2"
            If FtuneCal.RBX3.Checked Then xch = "A1"
            If FtuneCal.RBX4.Checked Then xch = "A2"
            If FtuneCal.RBX5.Checked Then xch = "A3"
            If FtuneCal.RBX6.Checked Then xch = "A4"
            If FtuneCal.RBY1.Checked Then ych = "Throttle"
            If FtuneCal.RBY2.Checked Then ych = "Load"
            If FtuneCal.RBY3.Checked Then ych = "A1"
            If FtuneCal.RBY4.Checked Then ych = "A2"
            If FtuneCal.RBY5.Checked Then ych = "A3"
            If FtuneCal.RBY6.Checked Then ych = "A4"
            If FtuneCal.RBZ1.Checked Then zch = "A1"
            If FtuneCal.RBZ2.Checked Then zch = "A2"
            If FtuneCal.RBZ3.Checked Then zch = "A3"
            If FtuneCal.RBZ4.Checked Then zch = "A4"
            Ltit.Text = title
            Ly.Text = ytit
            Lx.Text = xtit
            Lz.Text = ztit
            xn = Val(FtuneCal.TBXn.Text)
            yn = Val(FtuneCal.TBYn.Text)
            If xn < 1 Then xn = 10
            If xn > 20 Then xn = 20
            If yn < 1 Then yn = 1
            If yn > 20 Then yn = 20
            For i = 1 To xn
                xb(i) = i * Val(FtuneCal.Tbxs.Text)
            Next
            For i = 1 To yn
                yb(i) = i * Val(FtuneCal.tbys.Text)
            Next

            Table1.ColumnCount = xn : Table2.ColumnCount = xn : Table3.ColumnCount = xn
            Table1.RowCount = yn : Table2.RowCount = yn : Table3.RowCount = yn
            For i = 1 To xn                                             'Reassign tables based on new numbers
                Table1.Columns(i - 1).HeaderText = Str(xb(i))
                Table2.Columns(i - 1).HeaderText = Str(xb(i))
                Table3.Columns(i - 1).HeaderText = Str(xb(i))
                Table1.Columns(i - 1).Width = 50 : Table2.Columns(i - 1).Width = 50 : Table3.Columns(i - 1).Width = 50
                For j = 1 To yn
                    If i = 1 Then
                        Table1.Rows(j - 1).HeaderCell.Value = Str(yb(j))
                        Table2.Rows(j - 1).HeaderCell.Value = Str(yb(j))
                        Table3.Rows(j - 1).HeaderCell.Value = Str(yb(j))
                    End If
                    Table1(i - 1, j - 1).Value = t(1, i, j)
                    Table2(i - 1, j - 1).Value = t(2, i, j)
                    Table3(i - 1, j - 1).Value = t(3, i, j)
                    Table2.Rows(j - 1).Cells(i - 1).Style.Format = "N2"
                    Table3.Rows(j - 1).Cells(i - 1).Style.Format = "N2"
                Next
            Next

        Else
            FtuneCal.TBXn.Text = Str(xn)
            FtuneCal.TBYn.Text = Str(yn)
            FtuneCal.Tbxs.Text = Str(xb(1))
            FtuneCal.tbys.Text = Str(yb(1))
            FtuneCal.Titbox.Text = title
            FtuneCal.titboxx.Text = xtit
            FtuneCal.titboxy.Text = ytit
            FtuneCal.titboxz.Text = ztit




            Select Case Mid(xch, 1, 1)
                Case "R"        'RPM1, RPM2
                    If xch = "RPM1" Then
                        FtuneCal.RBX1.Checked = True
                    Else
                        FtuneCal.RBX2.Checked = True
                    End If
                Case "A"        'A1 .. A4
                    If Mid(xch, 2, 1) = 1 Then FtuneCal.RBX3.Checked = True
                    If Mid(xch, 2, 1) = 2 Then FtuneCal.RBX4.Checked = True
                    If Mid(xch, 2, 1) = 3 Then FtuneCal.RBX5.Checked = True
                    If Mid(xch, 2, 1) = 4 Then FtuneCal.RBX6.Checked = True
            End Select

            Select Case Mid(ych, 1, 1)
                Case "T"        'TTL
                    FtuneCal.RBY1.Checked = True
                Case "L"        'Load
                    FtuneCal.RBY2.Checked = True
                Case "A"        'A1..A4
                    If Mid(ych, 2, 1) = 1 Then FtuneCal.RBY3.Checked = True
                    If Mid(ych, 2, 1) = 2 Then FtuneCal.RBY4.Checked = True
                    If Mid(ych, 2, 1) = 3 Then FtuneCal.RBY5.Checked = True
                    If Mid(ych, 2, 1) = 4 Then FtuneCal.RBY6.Checked = True
            End Select

            Select Case Mid(zch, 1, 1)
                Case "A"
                    If Mid(zch, 2, 1) = 1 Then FtuneCal.RBZ1.Checked = True
                    If Mid(zch, 2, 1) = 2 Then FtuneCal.RBZ2.Checked = True
                    If Mid(zch, 2, 1) = 3 Then FtuneCal.RBZ3.Checked = True
                    If Mid(zch, 2, 1) = 4 Then FtuneCal.RBZ4.Checked = True
            End Select
            FtuneCal.Show()


        End If
    End Sub
    Private Sub z1() Handles FtuneCal.closeTunecal     'Close Tune Window
        BCfg.PerformClick()
    End Sub
    Private Sub z2() Handles FtuneCal.AbortTunecal     'Close Tune Window
        FtuneCal.Hide()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButZeroErr.Click
        For i = 1 To xn                                         'Zero's Correction Factors
            For j = 1 To yn
                Table3(i - 1, j - 1).Value = 0
                t(3, i, j) = 0
            Next
        Next
    End Sub

    Private Sub ButAcc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButAcc.Click
        If Val(TbAF.Text) < 0 Then TbAF.Text = "0"
        If Val(TbAF.Text) > 1 Then TbAF.Text = "1"
        avfact = Val(TbAF.Text)
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButAbort.Click
        RaiseEvent closeTune()              'Close without saving
    End Sub

    Private Sub ButExp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButExp.Click
        Dim openFileBox As New SaveFileDialog
        With openFileBox
            .FileName = tablefile
            .DefaultExt = "CSV"
            .Filter = "CSV|*.CSV"
            .Title = "Select file to save Actual and Correction Tables"
            '  .InitialDirectory = applicationSettingsDirectory
            Dim result = .ShowDialog()
            If result = DialogResult.OK Then
                tablefile = .FileName

                Using FileWrite As New System.IO.StreamWriter(tablefile)
                    FileWrite.WriteLine("Tune Boss Version," + Str(ver))                    'Header
                    FileWrite.WriteLine("Title," + title)
                    FileWrite.WriteLine("X Title," + xtit)
                    FileWrite.WriteLine("Y Title," + ytit)
                    FileWrite.WriteLine("Z Title," + ztit)

                    FileWrite.Write("Actual")
                    For i = 1 To xn
                        FileWrite.Write("," + xb(i).ToString)
                    Next
                    FileWrite.WriteLine()


                    For j = 1 To yn
                        FileWrite.Write(yb(j).ToString)
                        For i = 1 To xn
                            FileWrite.Write("," + t(2, i, j).ToString("N2"))
                        Next
                        FileWrite.WriteLine()
                    Next
                    FileWrite.WriteLine()

                    FileWrite.Write("Correction")
                    For i = 1 To xn
                        FileWrite.Write("," + xb(i).ToString)
                    Next
                    FileWrite.WriteLine()
                    For j = 1 To yn
                        FileWrite.Write(yb(j).ToString)
                        For i = 1 To xn
                            FileWrite.Write("," + t(3, i, j).ToString)
                        Next
                        FileWrite.WriteLine()
                    Next

                    FileWrite.Close()
                    FileWrite.Dispose()
                End Using



            End If
        End With

    End Sub

    Private Sub CBAccum_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBAccum.CheckedChanged

    End Sub

    Private Sub TBSet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBSet.TextChanged

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub ButImp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButImp.Click

    End Sub

    Private Sub TbAF_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TbAF.TextChanged

    End Sub

    Private Sub CBFuel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBFuel.CheckedChanged

    End Sub
End Class