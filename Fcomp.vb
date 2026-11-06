Imports System.Windows.Forms.DataVisualization.Charting
'Imports System.Windows.Forms.Form

Public Class FComp

    Friend WithEvents prntDoc As New Printing.PrintDocument
    Private WithEvents GrafSet As New GrafSet

    Dim calfile, filab(5, 30) As String                                         'File Labels
    Public FileDat(5, 2002, 30), Datmax(5, 30), datmin(5, 30) As Double         'Pull Data , Maximums
    Dim gmx(5, 5) As Double                                                     'Data in graf for point picking and max (which run, point, X y1 y2 y3) 
    Dim indmax(5, 30), ind(5, 5) As Integer                                     'Index of Maximum and Index of data series for plotting
    Dim ldindx, i, j, k, r, frun, lastdat, i1, i2 As Integer
    Dim ftime As String
    Dim TBM(30), PBM(30) As Label
    Dim ready, on1, on2 As Boolean                                              'Ready to plot? Graf 1 ON?, Graf 2? 
    Dim prtfoto As Bitmap
    Public first, firstgraf2 As Boolean                                         '1st time run graf?
    Dim x1, x2, x3, x4, y1, y2, y3, y4 As Double                                'Graph Averaging Points
    Dim afrx(2002), afry(2002), afry2(2002), afrscale, afrscale2 As Single      'Data for re-grafing of AFR on combined graf with rescale, Xmark is position of stats line (0-100%)
    Public xmark As Single                                                      'X position marker on graf
    Dim GXint As Single                                                         'X Graf Interval
    Dim Ymx1, Ymn1, Ymx2, ymn2 As Single                                        'Max and Min Y's for re-scaling Analogs on Chart3
    Dim st As String                                                            'String Temp
    'Me.ChartControl1.PrimaryXAxis.AutoSize = False                             'Might be able to use these to set the spacing around the graf
    'Me.ChartControl1.PrimaryXAxis.Size = New Size(50, 20)
  

    Sub MyForm_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.VisibleChanged
        Panel1.Parent = Chart1
        'prtfoto.Width = 1100 : prtfoto.Height = 590
        If Me.Visible Then

            LAMxY11.Text = ""
            LAMxy21.Text = ""
            LAMnY11.Text = ""
            LAMNY21.Text = ""

            LAMxY12.Text = ""
            LAMxY22.Text = ""
            LAMNY12.Text = ""
            LAMNY22.Text = ""
            xmark = 0.5                                 'Start the stats marker in the middle                             
            on1 = False
            on2 = False
            ready = False                             'Not ready yet!
            firstgraf2 = True
            Panel2.Hide() : LBigMax2.Hide() : Panel3.Hide() 'Hide the non primary labels and stuff till we load 'em
            ' MsgBox("Second")
            If DynoMonitor.GraFirst = True Then

                DynoMonitor.GraFirst = False                      'We're running so it's not the 1st time anymore
                '=============
                ' MsgBox("Third")
                Chart1.Invalidate()                 'Chart 1 is Torque and Power on top
                Chart1.Series.Clear()
                Chart2.Invalidate()                 'chart 2 is Analog graf (single) at bottom
                Chart2.Series.Clear()
                Chart3.Invalidate()                 'Chart 3 is Combined BIG graf
                Chart3.Series.Clear()

                For i = 1 To 5                        '4 grafs x 2 analogs + 2x Target (series 8 and 9)
                    st = "AFR" + Str(i) + "-1" : st = st.Replace(" ", "")
                    '  Console.WriteLine("::: Adding Series " + st)
                    Chart2.Series.Add(st)
                    Chart2.Series((i - 1) * 2).IsVisibleInLegend = False
                    Chart2.Series((i - 1) * 2).ToolTip = "#VAL{0.0} at #VALX{0}"
                    Chart2.Series((i - 1) * 2).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                    st = "AFR" + Str(i) + "-2" : st = st.Replace(" ", "")
                    Chart2.Series.Add(st)
                    Chart2.Series((i - 1) * 2 + 1).IsVisibleInLegend = False
                    Chart2.Series((i - 1) * 2 + 1).ToolTip = "#VAL{0.0} at #VALX{0}"
                    Chart2.Series((i - 1) * 2 + 1).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                    Chart2.Series((i - 1) * 2 + 1).YAxisType = DataVisualization.Charting.AxisType.Secondary
                Next

                Chart1.Series.Add("Torque1")

                Chart1.Series(0).IsVisibleInLegend = False
                Chart1.Series(0).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart1.Series.Add("Power1")

                Chart1.Series(1).YAxisType = DataVisualization.Charting.AxisType.Secondary
                Chart1.Series(1).IsVisibleInLegend = False
                Chart1.Series(1).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.FastLine


                Chart1.Series.Add("Torque2")
                Chart1.Series(2).BorderWidth = 1
                Chart1.Series(2).IsVisibleInLegend = False
                Chart1.Series(2).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(2).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart1.Series.Add("Power2")
                Chart1.Series(3).BorderWidth = 1
                Chart1.Series(3).YAxisType = DataVisualization.Charting.AxisType.Secondary
                Chart1.Series(3).IsVisibleInLegend = False
                Chart1.Series(3).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(3).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart1.Series.Add("Torque3")
                Chart1.Series(4).BorderWidth = 1
                Chart1.Series(4).IsVisibleInLegend = False
                Chart1.Series(4).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(4).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart1.Series.Add("Power3")
                Chart1.Series(5).BorderWidth = 1
                Chart1.Series(5).YAxisType = DataVisualization.Charting.AxisType.Secondary
                Chart1.Series(5).IsVisibleInLegend = False
                Chart1.Series(5).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(5).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart1.Series.Add("Torque4")
                Chart1.Series(6).BorderWidth = 1
                Chart1.Series(6).IsVisibleInLegend = False
                Chart1.Series(6).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(6).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart1.Series.Add("Power4")
                Chart1.Series(7).BorderWidth = 1
                Chart1.Series(7).YAxisType = DataVisualization.Charting.AxisType.Secondary
                Chart1.Series(7).IsVisibleInLegend = False
                Chart1.Series(7).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(7).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                For i = 1 To 4                          'Step through each pull (series 0-3, 4-7, 8-11, 12-15)
                    j = (i - 1) * 4
                    st = "Torque" + Str(i) : st = st.Replace(" ", "") : Chart3.Series.Add(st)
                    Chart3.Series(0 + j).IsVisibleInLegend = False
                    Chart3.Series(0 + j).ToolTip = "#VAL{0.0} at #VALX{0}"
                    Chart3.Series(0 + j).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                    st = "Power" + Str(i) : st = st.Replace(" ", "") : Chart3.Series.Add(st)
                    Chart3.Series(1 + j).YAxisType = DataVisualization.Charting.AxisType.Secondary
                    Chart3.Series(1 + j).IsVisibleInLegend = False
                    Chart3.Series(1 + j).ToolTip = "#VAL{0.0} at #VALX{0}"
                    Chart3.Series(1 + j).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                    st = "AFR" + Str(i) + "-1" : st = st.Replace(" ", "") : Chart3.Series.Add(st)

                    Chart3.Series(2 + j).YAxisType = DataVisualization.Charting.AxisType.Secondary
                    Chart3.Series(2 + j).IsVisibleInLegend = False
                    Chart3.Series(2 + j).ToolTip = "#VAL{0.0} at #VALX{0}"
                    Chart3.Series(2 + j).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                    st = "AFR" + Str(i) + "-2" : st = st.Replace(" ", "") : Chart3.Series.Add(st)

                    Chart3.Series(3 + j).YAxisType = DataVisualization.Charting.AxisType.Secondary
                    Chart3.Series(3 + j).IsVisibleInLegend = False
                    Chart3.Series(3 + j).ToolTip = "#VAL{0.0} at #VALX{0}"
                    Chart3.Series(3 + j).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Next


                Chart3.Series.Add("TARGET")
                Chart3.Series(16).BorderWidth = 1
                Chart3.Series(16).YAxisType = DataVisualization.Charting.AxisType.Secondary
                Chart3.Series(16).IsVisibleInLegend = False
                Chart3.Series(16).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Call setcolors()

                '=============
            End If
            Chart1.Series(1).BorderWidth = DynoMonitor.GLineW           'These are outside as we wanna be able to change "on the fly"
            Chart1.Series(0).BorderWidth = DynoMonitor.GLineW           'Sets up the 1st graf (newest) as designated line thickness
            Chart2.Series(0).BorderWidth = DynoMonitor.GLineW
            Chart2.Series(1).BorderWidth = DynoMonitor.GLineW

            Chart3.Series(0).BorderWidth = DynoMonitor.GLineW
            Chart3.Series(1).BorderWidth = DynoMonitor.GLineW
            Chart3.Series(2).BorderWidth = DynoMonitor.GLineW
            Chart3.Series(3).BorderWidth = DynoMonitor.GLineW

            Tbtit.Text = DynoMonitor.LTagline.Text
            TBM(1) = LM1 : PBM(1) = LY1

            GrafSet.LB1.Items.Clear()
            GrafSet.LB1.Items.Add("Time")                   'Gotta reset labels each time as they may have changed
            GrafSet.LB2.Items.Clear()
            GrafSet.LB2.Items.Add("Time")
            GrafSet.LB3.Items.Clear()
            GrafSet.LB3.Items.Add("Time")
            GrafSet.LB4.Items.Clear()
            GrafSet.LB4.Items.Add("Time")
            GrafSet.LB5.Items.Clear()
            GrafSet.LB5.Items.Add("Time")

            GrafSet.LB6.Items.Clear()
            GrafSet.LB6.Items.Add("Time")                   'Gotta reset labels each time as they may have changed
            GrafSet.LB7.Items.Clear()
            GrafSet.LB7.Items.Add("Time")
            GrafSet.LB8.Items.Clear()
            GrafSet.LB8.Items.Add("Time")
            GrafSet.LB9.Items.Clear()
            GrafSet.LB9.Items.Add("Time")
            GrafSet.LB10.Items.Clear()
            GrafSet.LB10.Items.Add("Time")
            ' MsgBox("Five")
            filab(1, 1) = "Time"
            For i = 1 To DynoMonitor.nch                            'Populate Selection Boxws with Channels
                Datmax(1, i + 1) = 0                                'Clear Maximums
                indmax(1, i + 1) = 0                                'And Index of max's
                filab(1, i + 1) = DynoMonitor.Analab(i)             'And assign rest of labels
                GrafSet.LB1.Items.Add(DynoMonitor.Analab(i)) : GrafSet.LB6.Items.Add(DynoMonitor.Analab(i))
                GrafSet.LB2.Items.Add(DynoMonitor.Analab(i)) : GrafSet.LB7.Items.Add(DynoMonitor.Analab(i))
                GrafSet.LB3.Items.Add(DynoMonitor.Analab(i)) : GrafSet.LB8.Items.Add(DynoMonitor.Analab(i))
                GrafSet.LB4.Items.Add(DynoMonitor.Analab(i)) : GrafSet.LB9.Items.Add(DynoMonitor.Analab(i))
                GrafSet.LB5.Items.Add(DynoMonitor.Analab(i)) : GrafSet.LB10.Items.Add(DynoMonitor.Analab(i))
            Next
            For i = 1 To 20
                If DynoMonitor.OBDchon(i) = True Then
                    GrafSet.LB1.Items.Add(DynoMonitor.OBDisLab(i)) : GrafSet.LB6.Items.Add(DynoMonitor.OBDisLab(i))
                    GrafSet.LB2.Items.Add(DynoMonitor.OBDisLab(i)) : GrafSet.LB7.Items.Add(DynoMonitor.OBDisLab(i))
                    GrafSet.LB3.Items.Add(DynoMonitor.OBDisLab(i)) : GrafSet.LB8.Items.Add(DynoMonitor.OBDisLab(i))
                    GrafSet.LB4.Items.Add(DynoMonitor.OBDisLab(i)) : GrafSet.LB9.Items.Add(DynoMonitor.OBDisLab(i))
                    GrafSet.LB5.Items.Add(DynoMonitor.OBDisLab(i)) : GrafSet.LB10.Items.Add(DynoMonitor.OBDisLab(i))
                End If
            Next
            'MsgBox("six")
            'Undex Items are:
            '1      2       3       4       5       6       7       8       9       10      11      12      13      14   15   16
            'Time   RPM1    RPM1    RPMe    Trqd    TrqT    PowkW   PowHp   Load    Ttl     A1      A2      A3      A4   Dist kph
            ind(1, 1) = DynoMonitor.Xgraf : GrafSet.LB1.SelectedIndex = ind(1, 1) - 1 : GrafSet.LB6.SelectedIndex = ind(1, 1) - 1    'RPM
            ind(1, 2) = DynoMonitor.Y1Graf : GrafSet.LB2.SelectedIndex = ind(1, 2) - 1 : GrafSet.LB7.SelectedIndex = ind(1, 2) - 1   'Torq
            ind(1, 3) = DynoMonitor.Y2graf : GrafSet.LB3.SelectedIndex = ind(1, 3) - 1 : GrafSet.LB8.SelectedIndex = ind(1, 3) - 1    'Power
            ind(1, 4) = DynoMonitor.Y3graf : GrafSet.LB4.SelectedIndex = ind(1, 4) - 1 : GrafSet.LB9.SelectedIndex = ind(1, 4) - 1   'AFR
            ind(1, 5) = DynoMonitor.Y4Graf : GrafSet.LB5.SelectedIndex = ind(1, 5) - 1 : GrafSet.LB10.SelectedIndex = ind(1, 5) - 1   'AFR2

            first = False                                   'Only used for name shown on file buttons
            ' MsgBox("Seven")
            For i = 1 To 30
                Datmax(1, i) = 0                            'Reset Max's
                datmin(1, i) = 1000000000.0
            Next
            If DynoMonitor.points > 2 Then
                For i = 1 To 2000
                    For j = 1 To 30
                        FileDat(1, i, j) = DynoMonitor.pulldata(DynoMonitor.dic, i, j)
                    Next
                Next
                For i = 1 To DynoMonitor.points                                        'Moved to post-Averaging
                    For j = 1 To DynoMonitor.nch + 1                               'Get Maximums and Index of Max's
                        If FileDat(1, i, j) > Datmax(1, j) Then Datmax(1, j) = FileDat(1, i, j) : indmax(1, j) = i
                        If FileDat(1, i, j) < datmin(1, j) Then datmin(1, j) = FileDat(1, i, j)
                        'Add data fot the selected graphs
                    Next
                    'Check to see that not zreos
                Next
            End If
            'MsgBox("Eight")


            Chart1.ChartAreas(0).AxisX.LabelStyle.Format = "N0"
            Chart1.ChartAreas(0).AxisX.Minimum = 0
            Chart1.ChartAreas(0).AxisX.Maximum = 1
            Chart1.ChartAreas(0).AxisY.Minimum = 0
            Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
            Chart1.ChartAreas(0).AxisY2.Minimum = 0
            Chart1.ChartAreas(0).AxisY2.Maximum = Double.NaN

            Chart2.ChartAreas(0).AxisX.LabelStyle.Format = "N0"
            Chart2.ChartAreas(0).AxisX.Minimum = 0
            Chart2.ChartAreas(0).AxisX.Maximum = 1
            Chart2.ChartAreas(0).AxisY.Minimum = DynoMonitor.anamin(DynoMonitor.Y3graf)     'AFR Graph Min
            Chart2.ChartAreas(0).AxisY.Maximum = DynoMonitor.anamax(DynoMonitor.Y3graf)     'Double.NaN
            Chart2.ChartAreas(0).AxisY2.Minimum = DynoMonitor.anamin(DynoMonitor.Y4Graf)     'AFR Graph Min
            Chart2.ChartAreas(0).AxisY2.Maximum = DynoMonitor.anamax(DynoMonitor.Y4Graf)
            If DynoMonitor.AutoAnaY1 = False Then
                Chart2.ChartAreas(0).AxisY.Minimum = DynoMonitor.GRA1Y1
                Chart2.ChartAreas(0).AxisY.Maximum = DynoMonitor.GRA1Y2
            End If
            If DynoMonitor.AutoAnaY2 = False Then
                Chart2.ChartAreas(0).AxisY2.Minimum = DynoMonitor.GRA2Y1
                Chart2.ChartAreas(0).AxisY2.Maximum = DynoMonitor.GRA2Y2
            End If

            Chart3.ChartAreas(0).AxisY.Maximum = Double.NaN
            Chart3.ChartAreas(0).AxisX.LabelStyle.Format = "N0"
            Chart3.ChartAreas(0).AxisX.Maximum = 1
            'Chart3.ChartAreas(0).AxisY.Maximum = 1
            'Chart3.ChartAreas(0).AxisY2.Maximum = 1
            Chart3.ChartAreas(0).AxisY.Minimum = 0
            Chart3.ChartAreas(0).AxisX.Minimum = 0
            Chart3.ChartAreas(0).AxisY2.Minimum = 0

            If DynoMonitor.points > 3 Then on1 = True 'Qualify that we got a trace 
            If on1 = True Then Call plot(1)
            ready = True
        End If

    End Sub
    Sub plot(ByVal ch As Integer)
        Dim xmax, xmin As Integer
        Dim n As Integer   'Limits on points inc. smoothing
        Dim s As String
        ' MsgBox("PLoting. CH: " + Str(ch) + ", FIrst: " + Str(first) + ", On1:" + Str(on1) + ", On2:" + Str(on2) + ", i:" + Str(i) + ", Points:" + Str(DynoMonitor.points))
        Chart1.Series((ch - 1) * 2).Points.Clear()
        Chart1.Series((ch - 1) * 2 + 1).Points.Clear()

        Chart2.Series((ch - 1) * 2).Points.Clear()
        Chart2.Series((ch - 1) * 2 + 1).Points.Clear()
        Chart2.Series(8).Points.Clear()                     'Target

        Chart3.Series((ch - 1) * 4).Points.Clear()
        Chart3.Series((ch - 1) * 4 + 1).Points.Clear()
        Chart3.Series((ch - 1) * 4 + 2).Points.Clear()
        Chart3.Series((ch - 1) * 4 + 3).Points.Clear()
        Chart3.Series(15).Points.Clear()                    'Target

        For i = 1 To 5
            gmx(ch, i) = 0                                  'Clear Max's
        Next i

        If DynoMonitor.GAxSame = True Then
            For i = 1 To 5 : ind(2, i) = ind(1, i) : Next               'Set for SAME axis on both grafs
        End If

        Lax1.Text = DynoMonitor.Anaunit(ind(1, 1) - 1)                  'Set up axis Labels
        LAX13.Text = DynoMonitor.Anaunit(ind(1, 1) - 1)
        LaxY11.Text = DynoMonitor.Anaunit(ind(1, 2) - 1)
        LaxY21.Text = DynoMonitor.Anaunit(ind(1, 3) - 1)
        Lana1.Text = DynoMonitor.Anaunit(ind(1, 4) - 1)
        Lana12.Text = DynoMonitor.Anaunit(ind(1, 5) - 1)
        If (ch = 2) Then
            LAX2.Text = DynoMonitor.Anaunit(ind(2, 1) - 1)              'Set up axis Labels
            LAX23.Text = DynoMonitor.Anaunit(ind(2, 1) - 1)
            LAXY12.Text = DynoMonitor.Anaunit(ind(2, 2) - 1)
            LAXY22.Text = DynoMonitor.Anaunit(ind(2, 3) - 1)
            Lana2.Text = DynoMonitor.Anaunit(ind(1, 4) - 1)
            Lana22.Text = DynoMonitor.Anaunit(ind(1, 5) - 1)
        End If

        If DynoMonitor.G2Last = True And firstgraf2 = True Then
            ButFile.Text = "Previous Pull Data"
            If Lrun1.Text <> "-" Then Lrun2.Text = Str(Val(Lrun1.Text) - 1)
        End If

        If DynoMonitor.grafgrid = True Then                             'Set up for corse/fine grid
            Chart1.ChartAreas(0).AxisX.MinorGrid.Enabled = True
            Chart1.ChartAreas(0).AxisX.MajorGrid.LineWidth = 2
            Chart1.ChartAreas(0).AxisY.MajorGrid.LineWidth = 2
            Chart1.ChartAreas(0).AxisY.MinorGrid.Enabled = True
            Chart1.ChartAreas(0).AxisY2.MinorGrid.Enabled = False       '

            Chart2.ChartAreas(0).AxisX.MinorGrid.Enabled = True
            Chart2.ChartAreas(0).AxisX.MajorGrid.LineWidth = 2
            Chart2.ChartAreas(0).AxisY.MajorGrid.LineWidth = 2
            Chart2.ChartAreas(0).AxisY.MinorGrid.Enabled = True
            Chart3.ChartAreas(0).AxisX.MinorGrid.Enabled = True
            Chart3.ChartAreas(0).AxisY.MinorGrid.Enabled = True
            Chart3.ChartAreas(0).AxisX.MajorGrid.LineWidth = 2
            Chart3.ChartAreas(0).AxisY.MajorGrid.LineWidth = 2
            'Chart1.ChartAreas(0).AxisX.MinorGrid.Interval = Chart1.ChartAreas(0).AxisX.MajorTickMark.Interval / 5
            'Chart1.ChartAreas(0).AxisY.MinorTickMark.Interval = Chart1.ChartAreas(0).AxisY.MajorTickMark.Interval / 5
        Else
            Chart1.ChartAreas(0).AxisX.MinorGrid.Enabled = False
            Chart1.ChartAreas(0).AxisY.MinorGrid.Enabled = False
            Chart2.ChartAreas(0).AxisX.MinorGrid.Enabled = False
            Chart2.ChartAreas(0).AxisY.MinorGrid.Enabled = False
            Chart3.ChartAreas(0).AxisX.MinorGrid.Enabled = False
            Chart3.ChartAreas(0).AxisY.MinorGrid.Enabled = False
            Chart1.ChartAreas(0).AxisX.MajorGrid.LineWidth = 1
            Chart1.ChartAreas(0).AxisY.MajorGrid.LineWidth = 1
            Chart2.ChartAreas(0).AxisX.MajorGrid.LineWidth = 1
            Chart2.ChartAreas(0).AxisY.MajorGrid.LineWidth = 1
            Chart3.ChartAreas(0).AxisX.MajorGrid.LineWidth = 1
            Chart3.ChartAreas(0).AxisY.MajorGrid.LineWidth = 1
        End If
        If DynoMonitor.bigsum = True Then
            LBigMax1.Show()
            Panel1.Hide()
        Else
            LBigMax1.Hide()
            Panel1.Show()
        End If
        If DynoMonitor.AnaGrTop = True Then         'Hide whatcha don wanna see
            Chart1.Hide() : Lax1.Hide() : LAX2.Hide()
            Chart2.Hide() : Panel2.Hide()
            Chart3.Show() : LAX13.Show()
            If ch = 2 And on2 = True Then
                If DynoMonitor.bigsum = True Then
                    LBigMax2.Show()
                    Panel3.Hide()
                Else
                    LBigMax2.Hide()
                    Panel3.Show()
                End If
                LAX23.Show()
            End If
        Else
            Chart1.Show() : Lax1.Show()
            Chart2.Show()
            If ch = 2 And on2 = True Then

                LAX2.Show()
                If DynoMonitor.bigsum = True Then
                    LBigMax2.Show()
                    Panel2.Hide()
                Else
                    LBigMax2.Hide()
                    Panel2.Show()
                End If
            End If
            Chart3.Hide() : Panel3.Hide() : LAX13.Hide() : LAX23.Hide()
        End If

        For i = 1 To 2000                                   'Find last data point
            If FileDat(ch, i, ind(ch, 1)) = 0 Then
                lastdat = i - 1
                Exit For
            End If
        Next
        If lastdat >= 2000 Then lastdat = 2000

        For i = 1 To lastdat 'Ictmin To ictmax        'Should do a check here to make sure Last Dat > GraFilt
            ' If FileDat(ch, i, ind(ch, 1)) <> 0 Then         'x=FileDat(ch, i, ind(ch, 1)), y=FileDat(ch, i, ind(ch, 2)) unless filtering, then x=(for j=i-5 to i+5)...
            i1 = i - DynoMonitor.GraFilt
            i2 = i + DynoMonitor.GraFilt
            If i1 < 1 Then i1 = 1
            If i2 > lastdat Then i2 = lastdat 'Set bounds of averaging
            x1 = 0 : y1 = 0 : y2 = 0 : y3 = 0 : y4 = 0 : n = 0
            For j = i1 To i2
                x1 = x1 + FileDat(ch, j, ind(ch, 1))
                y1 = y1 + FileDat(ch, j, ind(ch, 2))
                y2 = y2 + FileDat(ch, j, ind(ch, 3))
                y3 = y3 + FileDat(ch, j, ind(ch, 4))
                y4 = y4 + FileDat(ch, j, ind(ch, 5))
                n = n + 1
            Next
            '   x1 = FileDat(ch, i, ind(ch, 1)) : y1 = FileDat(ch, i, ind(ch, 2))           'These are the non-averaged numbers
            '   x2 = FileDat(ch, i, ind(ch, 1)) : y2 = FileDat(ch, i, ind(ch, 3))
            '   x3 = FileDat(ch, i, ind(ch, 1)) : y3 = FileDat(ch, i, ind(ch, 4))
            x1 = x1 / n
            y1 = y1 / n
            y2 = y2 / n '(i2 - i1 + 1)
            y3 = y3 / n '(i2 - i1 + 1)
            y4 = y4 / n '(i2 - i1 + 1)
            x2 = x1
            x3 = x1
            x4 = x1
            afrx(i) = x1
            afry(i) = y3
            afry2(i) = y4

            'If i < 10 Then Console.WriteLine("Point " + i.ToString + " X: " + x1.ToString + " n= " + n.ToString + " Y3: " + y3.ToString)

            'If y1 > Datmax(ch, ind(ch, 2)) Then Datmax(ch, ind(ch, 2)) = y1 : indmax(ch, ind(ch, 2)) = i
            'If y3 > Datmax(ch, ind(ch, 3)) Then Datmax(ch, ind(ch, 3)) = y3 : indmax(ch, ind(ch, 3)) = i
            'If FileDat(1, i, j) < datmin(1, j) Then datmin(1, j) = FileDat(1, i, j)

            If x1 > gmx(ch, 1) Then gmx(ch, 1) = x1 'Assign maxes' for box   1111
            If y1 > gmx(ch, 2) Then gmx(ch, 2) = y1
            If y2 > gmx(ch, 3) Then gmx(ch, 3) = y2
            If y3 > gmx(ch, 4) Then gmx(ch, 4) = y3
            If y4 > gmx(ch, 5) Then gmx(ch, 5) = y4

            Chart1.Series((ch - 1) * 2).Points.AddXY(x1, y1)
            Chart1.Series((ch - 1) * 2 + 1).Points.AddXY(x2, y2)
            Chart2.Series((ch - 1) * 2).Points.AddXY(x3, y3)                          'Ana 1
            Chart2.Series((ch - 1) * 2 + 1).Points.AddXY(x4, y4)                      'Ana 2
            'Console.WriteLine("Adding Ana2 Point: " + Str(x4) + ", " + Str(y4))
            Chart2.Series(8).Points.AddXY(x3, DynoMonitor.GrAnaTarg)                  'AFR Target

            Chart3.Series((ch - 1) * 4).Points.AddXY(x1, y1)
            Chart3.Series((ch - 1) * 4 + 1).Points.AddXY(x2, y2)
            Chart3.Series((ch - 1) * 4 + 2).Points.AddXY(x3, y3)             'AFR
            Chart3.Series((ch - 1) * 4 + 3).Points.AddXY(x4, y4)             'AFR
            Chart3.Series(15).Points.AddXY(x3, DynoMonitor.GrAnaTarg)       'AFR Targ
        Next
        xmin = 10000000.0 : xmax = 0


        If on1 = True Then
            xmax = Datmax(1, ind(1, 1))
            xmin = datmin(1, ind(1, 1))
        End If
        If on2 = True Then
            xmax = Datmax(2, ind(2, 1))
            xmin = datmin(2, ind(2, 1))
        End If

        If on1 = True And on2 = True Then
            If Datmax(1, ind(1, 1)) > xmax Then xmax = Datmax(1, ind(1, 1))
            If datmin(1, ind(1, 1)) < xmin Then xmin = datmin(1, ind(1, 1))
        End If
        'MsgBox("1: " + Str(datmin(1, ind(1, 1))) + " : " + Str(Datmax(1, ind(1, 1))) + Str(on1) + Chr(13) + "2: " + Str(datmin(2, ind(2, 1))) + " : " + Str(Datmax(2, ind(2, 1))) + Str(on2) + Chr(13) + "Plot " + Str(ch) + "  Min:" + Str(xmin) + "   Max:" + Str(xmax))

        If on1 Or on2 Then
            If xmax > 20 Then
                xmax = Int((xmax + 10) / 10) * 10               'Prev was Xmax was at least 100, but not sutable for Time axis
            Else
                xmax = Int(xmax + 1)                            'This sutable for Time axis
            End If

            If DynoMonitor.GManX = True Then            'Set Manual Axis limits if selected
                'xmin = DynoMonitor.GRx1
                'xmax = DynoMonitor.GRx2
            End If
            Chart1.ChartAreas(0).AxisX.Minimum = Int(xmin)
            Chart1.ChartAreas(0).AxisX.Maximum = Int(xmax)
            If Chart1.ChartAreas(0).AxisX.Maximum = Chart1.ChartAreas(0).AxisX.Minimum Then Chart1.ChartAreas(0).AxisX.Maximum += 1
            Chart2.ChartAreas(0).AxisX.Minimum = Int(xmin)
            Chart2.ChartAreas(0).AxisX.Maximum = Int(xmax)
            If Chart2.ChartAreas(0).AxisX.Maximum = Chart2.ChartAreas(0).AxisX.Minimum Then Chart2.ChartAreas(0).AxisX.Maximum += 1
            Chart3.ChartAreas(0).AxisX.Minimum = Int(xmin)
            Chart3.ChartAreas(0).AxisX.Maximum = Int(xmax)
            If Chart3.ChartAreas(0).AxisX.Maximum = Chart3.ChartAreas(0).AxisX.Minimum Then Chart3.ChartAreas(0).AxisX.Maximum += 1
            If DynoMonitor.GManX = False Then
                'Chart1.ChartAreas(0).AxisX.Interval = (Int(xmax) - Int(xmin)) / 10                  'Set 10 major axis interval
                'Chart2.ChartAreas(0).AxisX.Interval = (Int(xmax) - Int(xmin)) / 10
                'Chart3.ChartAreas(0).AxisX.Interval = (Int(xmax) - Int(xmin)) / 10
                xmin = Int(xmin / 10) * 10
                If xmax <= xmin Then xmax = xmin + 1
                GXint = Int((xmax - xmin) / 100 + 1) * 10
                Chart1.ChartAreas(0).AxisX.Minimum = GXint * Int(xmin / GXint)
                Chart1.ChartAreas(0).AxisX.Maximum = GXint * (Int(xmax / GXint) + 1)
                Chart2.ChartAreas(0).AxisX.Minimum = GXint * Int(xmin / GXint)
                Chart2.ChartAreas(0).AxisX.Maximum = GXint * (Int(xmax / GXint) + 1)
                Chart3.ChartAreas(0).AxisX.Minimum = GXint * Int(xmin / GXint)
                Chart3.ChartAreas(0).AxisX.Maximum = GXint * (Int(xmax / GXint) + 1)
                Console.WriteLine("Auto Set Graph Interval: " + Str(GXint))
                Chart1.ChartAreas(0).AxisX.Interval = GXint                  'Set 10 major axis interval
                Chart2.ChartAreas(0).AxisX.Interval = GXint
                Chart3.ChartAreas(0).AxisX.Interval = GXint
            Else

                Chart1.ChartAreas(0).AxisX.Minimum = DynoMonitor.GRx1 * Int(xmin / DynoMonitor.GRx1)
                Chart1.ChartAreas(0).AxisX.Maximum = DynoMonitor.GRx1 * (Int(xmax / DynoMonitor.GRx1) + 1)
                Chart2.ChartAreas(0).AxisX.Minimum = DynoMonitor.GRx1 * Int(xmin / DynoMonitor.GRx1)
                Chart2.ChartAreas(0).AxisX.Maximum = DynoMonitor.GRx1 * (Int(xmax / DynoMonitor.GRx1) + 1)
                Chart3.ChartAreas(0).AxisX.Minimum = DynoMonitor.GRx1 * Int(xmin / DynoMonitor.GRx1)
                Chart3.ChartAreas(0).AxisX.Maximum = DynoMonitor.GRx1 * (Int(xmax / DynoMonitor.GRx1) + 1)
                Chart1.ChartAreas(0).AxisX.Interval = DynoMonitor.GRx1                  'Set 10 major axis interval
                Chart2.ChartAreas(0).AxisX.Interval = DynoMonitor.GRx1
                Chart3.ChartAreas(0).AxisX.Interval = DynoMonitor.GRx1
            End If

            If Chart1.ChartAreas(0).AxisX.Maximum = Chart1.ChartAreas(0).AxisX.Minimum Then Chart1.ChartAreas(0).AxisX.Maximum += 1
            If Chart2.ChartAreas(0).AxisX.Maximum = Chart2.ChartAreas(0).AxisX.Minimum Then Chart2.ChartAreas(0).AxisX.Maximum += 1
            If Chart3.ChartAreas(0).AxisX.Maximum = Chart3.ChartAreas(0).AxisX.Minimum Then Chart3.ChartAreas(0).AxisX.Maximum += 1

            If DynoMonitor.AutoAnaY1 = True Then
                Chart2.ChartAreas(0).AxisY.Minimum = DynoMonitor.anamin(DynoMonitor.Y3graf - 1)        'AFR Graph Min and Max from Channel Cals
                Chart2.ChartAreas(0).AxisY.Maximum = DynoMonitor.anamax(DynoMonitor.Y3graf - 1)
            End If
            If DynoMonitor.AutoAnaY2 = True Then
                Chart2.ChartAreas(0).AxisY2.Minimum = DynoMonitor.anamin(DynoMonitor.Y4Graf - 1)
                Chart2.ChartAreas(0).AxisY2.Maximum = DynoMonitor.anamax(DynoMonitor.Y4Graf - 1)
            End If
      


            If DynoMonitor.Y3graf = 1 Then                  'I think these lines still PUKE (at axis) some times!!!!!                                        'Time, gotta correct MAX as not in Analogs
                Chart2.ChartAreas(0).AxisY.Minimum = 0
                Chart2.ChartAreas(0).AxisY.Maximum = FileDat(ch, lastdat, DynoMonitor.Y3graf)
            End If
            If DynoMonitor.Y4Graf = 1 Then                                                          'Time, gotta correct MAX as not in Analogs
                Chart2.ChartAreas(0).AxisY2.Minimum = 0
                Chart2.ChartAreas(0).AxisY2.Maximum = FileDat(ch, lastdat, DynoMonitor.Y4Graf)
            End If


            If DynoMonitor.AutoAnaY1 = False Then           'Set Manually
                Chart2.ChartAreas(0).AxisY.Minimum = DynoMonitor.GRA1Y1
                Chart2.ChartAreas(0).AxisY.Maximum = DynoMonitor.GRA1Y2
            End If
            If DynoMonitor.AutoAnaY2 = False Then
                Chart2.ChartAreas(0).AxisY2.Minimum = DynoMonitor.GRA2Y1
                Chart2.ChartAreas(0).AxisY2.Maximum = DynoMonitor.GRA2Y2
            End If

            'Next line just checks for MIN=MAX and corrects
            If Chart2.ChartAreas(0).AxisY.Maximum = Chart2.ChartAreas(0).AxisY.Minimum Then Chart2.ChartAreas(0).AxisY.Maximum = Chart2.ChartAreas(0).AxisY.Maximum + 1
            If Chart2.ChartAreas(0).AxisY2.Maximum = Chart2.ChartAreas(0).AxisY2.Minimum Then Chart2.ChartAreas(0).AxisY2.Maximum = Chart2.ChartAreas(0).AxisY2.Maximum + 1


            If DynoMonitor.GManY = True Then
                Chart1.ChartAreas(0).AxisY.Maximum = DynoMonitor.GRy2
                Chart3.ChartAreas(0).AxisY.Maximum = DynoMonitor.GRy2
            Else
                Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
                Chart3.ChartAreas(0).AxisY.Maximum = Double.NaN
            End If
            If DynoMonitor.GMan2Y = True Then
                Chart1.ChartAreas(0).AxisY2.Maximum = DynoMonitor.GR2Y2
                Chart3.ChartAreas(0).AxisY2.Maximum = DynoMonitor.GR2Y2
            Else
                Chart1.ChartAreas(0).AxisY2.Maximum = Double.NaN
                Chart3.ChartAreas(0).AxisY2.Maximum = Double.NaN
            End If
            Chart1.ResetAutoValues()
            Chart2.ResetAutoValues()
            Chart3.ResetAutoValues()


            '  Chart2.ChartAreas(0).AxisX.Interval = (Int(xmax) - Int(xmin)) / 10
            '  Chart3.ChartAreas(0).AxisX.Interval = (Int(xmax) - Int(xmin)) / 10
            Ymx1 = DynoMonitor.anamax(DynoMonitor.Y3graf - 1)
            Ymn1 = DynoMonitor.anamin(DynoMonitor.Y3graf - 1)
            Ymx2 = DynoMonitor.anamax(DynoMonitor.Y4Graf - 1)
            ymn2 = DynoMonitor.anamin(DynoMonitor.Y4Graf - 1)

            If DynoMonitor.AutoAnaY1 = False Then
                Ymx1 = DynoMonitor.GRA1Y2
                Ymn1 = DynoMonitor.GRA1Y1
            End If
            If DynoMonitor.AutoAnaY2 = False Then
                Ymx2 = DynoMonitor.GRA2Y2
                ymn2 = DynoMonitor.GRA2Y1
            End If


            If DynoMonitor.AnaGrTop = True Then
                Chart3.Series((ch - 1) * 4 + 2).Points.Clear()                                      'Clear the AFR data from combined graf
                Chart3.Series((ch - 1) * 4 + 3).Points.Clear()                                      'Clear the AFR data from combined graf
                Chart3.Update()                                                                     'Gotta do this to get the MAX number to be acessable
                afrscale = Chart3.ChartAreas(0).AxisY.Maximum / (Ymx1 - Ymn1)
                afrscale2 = Chart3.ChartAreas(0).AxisY.Maximum / (Ymx2 - ymn2)
                If ch = 1 Then
                    LAMxY11.Text = Ymx1.ToString
                    LAMxy21.Text = Ymx2.ToString
                    LAMnY11.Text = Ymn1.ToString
                    LAMNY21.Text = ymn2.ToString
                Else
                    LAMxY12.Text = Ymx1.ToString
                    LAMxY22.Text = Ymx2.ToString
                    LAMNY12.Text = Ymn1.ToString
                    LAMNY22.Text = ymn2.ToString

                End If
                For i = 1 To lastdat                                                                'Redo AFR data to scale to 0-Y1 Max for Combined plot
                    Chart3.Series((ch - 1) * 4 + 2).Points.AddXY(afrx(i), (afry(i) - Ymn1) * afrscale) 'Points(i).YValues = Chart3.Series(2).Points(i).YValues * 2
                    Chart3.Series((ch - 1) * 4 + 3).Points.AddXY(afrx(i), (afry2(i) - ymn2) * afrscale2) 'Points(i).YValues = Chart3.Series(2).Points(i).YValues * 2
                Next
            Else
                Chart1.Update()                                                                     'Gotta do this to get the MAX number to be acessable
            End If

            If ch = 1 Then
                LLM1.Text = filab(1, ind(1, 2))             'Str(ind(1, 2)) + ":" + filab(1, ind(1, 2))
                LLM2.Text = filab(1, ind(1, 3))             'Str(ind(1, 3)) + ":" + filab(1, ind(1, 3))

                LM1.Text = gmx(1, 2).ToString(DynoMonitor.anadec(ind(ch, 2) - 1))
                LM2.Text = gmx(1, 3).ToString(DynoMonitor.anadec(ind(ch, 3) - 1))

                'LM1.Text = Datmax(1, ind(1, 2)).ToString(DynoMonitor.anadec(ind(ch, 2) - 1))   '1111
                'LM2.Text = Datmax(1, ind(1, 3)).ToString(DynoMonitor.anadec(ind(ch, 3) - 1))

                LY1.Text = FileDat(1, indmax(1, ind(ch, 2)), ind(1, 1)).ToString(DynoMonitor.anadec(ind(1, 1)))
                LY2.Text = FileDat(1, indmax(1, ind(ch, 3)), ind(1, 1)).ToString(DynoMonitor.anadec(ind(1, 1)))
                'Console.WriteLine("Channel " + Str(ind(ch, 1) - 1) + " Dec: " + DynoMonitor.anadec(ind(ch, 1)))
                If DynoMonitor.bigsum = True Then
                    '8888 Set from GrafDat
                    LBigMax1.Show()
                    'LBigMax1.Text = "Max " + Datmax(1, ind(1, 2)).ToString(DynoMonitor.anadec(ind(ch, 2) - 1)) + " " + filab(1, ind(1, 2))         '1111
                    LBigMax1.Text = "Max " + gmx(1, 2).ToString(DynoMonitor.anadec(ind(ch, 2))) + " " + filab(1, ind(1, 2))

                    If DynoMonitor.AnaGrTop = True Then
                        LBigMax1.Parent = Chart3
                    Else
                        LBigMax1.Parent = Chart1
                    End If
                Else
                    LBigMax1.Hide()
                    LBigMax2.Hide()
                End If
            Else
                LLM12.Text = filab(ch, ind(2, 2))           'Str(ind(ch, 2)) + ":" + filab(ch, ind(1, 2))       '*** This section is redundent!!! Use Chart1stats call
                LLM22.Text = filab(ch, ind(2, 3))           'Str(ind(ch, 3)) + ":" + filab(ch, ind(1, 3))
                llm123.Text = filab(ch, ind(2, 2))
                llm223.Text = filab(ch, ind(2, 3))
                LM12.Text = gmx(2, 2).ToString(DynoMonitor.anadec(ind(ch, 2)))
                LM22.Text = gmx(2, 3).ToString(DynoMonitor.anadec(ind(ch, 3)))
                'LM12.Text = Datmax(ch, ind(ch, 2)).ToString(DynoMonitor.anadec(ind(ch, 2) - 1))
                'LM22.Text = Datmax(ch, ind(ch, 3)).ToString(DynoMonitor.anadec(ind(ch, 3) - 1))
                lm123.Text = Datmax(ch, ind(ch, 2)).ToString(DynoMonitor.anadec(ind(ch, 2)))
                lm223.Text = Datmax(ch, ind(ch, 3)).ToString(DynoMonitor.anadec(ind(ch, 3)))
                LY12.Text = FileDat(ch, indmax(ch, ind(ch, 2)), ind(ch, 1)).ToString(DynoMonitor.anadec(ind(ch, 1)))
                LY22.Text = FileDat(ch, indmax(ch, ind(ch, 3)), ind(ch, 1)).ToString(DynoMonitor.anadec(ind(ch, 1)))
                ly123.Text = FileDat(ch, indmax(ch, ind(ch, 2)), ind(ch, 1)).ToString(DynoMonitor.anadec(ind(ch, 1)))
                ly223.Text = FileDat(ch, indmax(ch, ind(ch, 3)), ind(ch, 1)).ToString(DynoMonitor.anadec(ind(ch, 1)))
                If DynoMonitor.bigsum = True Then
                    LBigMax1.Show()
                    'LBigMax2.Text = "Max " + Datmax(ch, ind(ch, 2)).ToString(DynoMonitor.anadec(ind(ch, 2) - 1)) + " " + filab(ch, ind(2, 2))          'Show the "raw" Max (will result in bigger number than shown on graf)
                    LBigMax2.Text = "Max " + gmx(ch, 2).ToString(DynoMonitor.anadec(ind(ch, 2) - 1)) + " " + filab(ch, ind(2, 2))       'SHow the "smoothed" Max
                    If DynoMonitor.AnaGrTop = True Then
                        LBigMax2.Parent = Chart3
                    Else
                        LBigMax2.Parent = Chart1
                    End If
                Else
                    LBigMax1.Hide()
                    LBigMax2.Hide()
                End If
            End If
        End If
        Chart1.ChartAreas(0).RecalculateAxesScale()
        Chart3.ChartAreas(0).RecalculateAxesScale() 'Dies here... need TRY

        Chart1.ChartAreas(0).AxisY.Interval = (Chart1.ChartAreas(0).AxisY.Maximum - Chart1.ChartAreas(0).AxisY.Minimum) / 10
        Chart1.ChartAreas(0).AxisY2.Interval = (Chart1.ChartAreas(0).AxisY2.Maximum - Chart1.ChartAreas(0).AxisY2.Minimum) / 10

        Console.WriteLine("Setting G2Y1 Interval between: " + Chart2.ChartAreas(0).AxisY.Minimum.ToString + " to " + Chart2.ChartAreas(0).AxisY.Maximum.ToString)
        Chart2.ChartAreas(0).AxisY.Interval = (Chart2.ChartAreas(0).AxisY.Maximum - Chart2.ChartAreas(0).AxisY.Minimum) / 5
        Console.WriteLine("Setting G2Y2 Interval between: " + Chart2.ChartAreas(0).AxisY2.Minimum.ToString + " to " + Chart2.ChartAreas(0).AxisY2.Maximum.ToString)
        Chart2.ChartAreas(0).AxisY2.Interval = (Chart2.ChartAreas(0).AxisY2.Maximum - Chart2.ChartAreas(0).AxisY2.Minimum) / 5
        Chart2.ChartAreas(0).AxisY.LabelStyle.Format = DynoMonitor.anadec(ind(ch, 3) - 1)
        Console.WriteLine("Setting G3Y Interval Between: " + Chart3.ChartAreas(0).AxisY.Minimum.ToString + " to " + Chart3.ChartAreas(0).AxisY.Maximum.ToString)
        Chart3.ChartAreas(0).AxisY.Interval = (Chart3.ChartAreas(0).AxisY.Maximum - Chart3.ChartAreas(0).AxisY.Minimum) / 10
        Console.WriteLine("Setting G3Y2 Interval Between: " + Chart3.ChartAreas(0).AxisY2.Minimum.ToString + " to " + Chart3.ChartAreas(0).AxisY2.Maximum.ToString)
        Chart3.ChartAreas(0).AxisY2.Interval = (Chart3.ChartAreas(0).AxisY2.Maximum - Chart3.ChartAreas(0).AxisY2.Minimum) / 10

    End Sub
    Private Sub ButExit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButGrafExit.Click
        Call exitgraf()
        Console.WriteLine("GRAF Exit: Button")
    End Sub
    Sub exitgraf()
        ready = False
        If DynoMonitor.G2Last = False Then
            'Chart1.Series.Clear() 'Torque Curve Graph
            'Chart2.Series.Clear()
            'Chart3.Series.Clear()
        Else
            Chart1.Series(2).Points.Clear()
            Chart1.Series(3).Points.Clear()
            Chart2.Series(1).Points.Clear()
            Chart3.Series(2).Points.Clear()
            Chart3.Series(3).Points.Clear()
            Chart3.Series(5).Points.Clear()
        End If


        LabX.Text = "-" : LabY1.Text = "-" : Laby2.Text = "-" : LaY3.Text = "-" : LAY4.Text = "-"
        LX1.Text = "-" : Ly11.Text = "-" : Lz1.Text = "-" : Lw1.Text = "-" : LX2.Text = "-"
        Ly21.Text = "-" : Lz2.Text = "-" : Lw2.Text = "-" : LV1.Text = "-" : LV2.Text = "-"
        DynoMonitor.points = 0                        'Need to reset # of points (for latest pull) upon exiting I think
        DynoMonitor.graftop = Me.Top
        DynoMonitor.grafLeft = Me.Left
        DynoMonitor.grafwidth = Me.Width
        DynoMonitor.grafheight = Me.Height
        Me.Hide()
    End Sub

    Private Sub Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True         'This cancels the closing, then we just close as normally
        Call exitgraf()
        Console.WriteLine("GRAF Exit: X")
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButFile.Click
        Dim LoadFileBox As New OpenFileDialog
        With LoadFileBox
            .FileName = calfile
            .DefaultExt = "CSV"
            .Filter = "CSV|*.csv"
            .Title = "Pull Data File"
            '  .InitialDirectory = applicationSettingsDirectory
            Dim result = .ShowDialog()
            If result = DialogResult.OK Then
                calfile = .FileName
                ButFile.Text = calfile
                on2 = True
                firstgraf2 = False
                Call loadpulls(2)
            End If
        End With
    End Sub
    Private Sub loadpulls(ByVal ch As Integer)

        Dim s, sa(20) As String
        ready = False

        If ch = 1 Then
            GrafSet.LB1.Items.Clear()
            GrafSet.LB2.Items.Clear()
            GrafSet.LB3.Items.Clear()
            GrafSet.LB4.Items.Clear()
            GrafSet.LB5.Items.Clear()
        Else
            GrafSet.LB9.Items.Clear()
            GrafSet.LB8.Items.Clear()
            GrafSet.LB7.Items.Clear()
            GrafSet.LB6.Items.Clear()
            GrafSet.LB10.Items.Clear()
        End If

        For i = 1 To DynoMonitor.nch + 1
            Datmax(ch, i) = 0 : indmax(ch, i) = 0
        Next
        'Load Index is 4, 8, or 12
        Try
            Using FileRead As New System.IO.StreamReader(calfile)
                s = FileRead.ReadLine()                    'Read Headers Line 1
                If ch = 1 Then                              'Show Date of pull and Sec in the labels at top
                    s = s + "  "
                    If InStr(s, ":") > 0 Then s = Mid(s, InStr(s, ":") + 1) + "   "
                    If InStr(s, ":") > 0 Then s = Mid(s, InStr(s, ":") + 2)
                    If Len(s) > 0 Then Ltime.Text = s.TrimEnd
                End If
                s = FileRead.ReadLine()                    'Read Headers Line 2
                sa = s.Split(",")
                If ch = 1 Then                              'Show Date of pull and Sec in the labels at top
                    s = s + "  "
                    If InStr(s, ":") > 0 Then s = Mid(s, InStr(s, ":") + 1) + "   "
                    If InStr(s, ":") > 0 Then s = Mid(s, InStr(s, ":") + 1) + "   "
                    If InStr(s, ":") > 0 Then s = Mid(s, InStr(s, ":") + 1) + "   "
                    If InStr(s, ":") > 0 Then s = Mid(s, InStr(s, ":") + 1) + "   "
                    If InStr(s, ":") > 0 Then s = Mid(s, InStr(s, ":") + 2)
                    If Len(s) > 0 Then Lsec.Text = s.TrimEnd + " sec"
                End If

                sa(1) = System.Text.RegularExpressions.Regex.Replace(sa(1), "[^0-9\.]", "") 'Replace non numberics with "empty"
                frun = Val(sa(1))
                ftime = sa(3)
                If ch = 1 Then : Lrun1.Text = sa(1) : Else : Lrun2.Text = sa(1) : End If
                For i = 3 To 4
                    s = FileRead.ReadLine()                    'Read Headers
                Next                                            '4th line is Labels
                s = s + ", , , , , , , , , , , , , , , , , ,"       'Pad just in case prev version of pull with fewer columns
                sa = s.Split(",")

                For i = 1 To DynoMonitor.nch + 1
                    filab(ch, i) = sa(i - 1)
                    'MsgBox("Col:" + Str(i) + filab(ch, i))
                    If ch = 1 Then                              'Add labels as used in the individual files
                        GrafSet.LB1.Items.Add(filab(ch, i))
                        GrafSet.LB2.Items.Add(filab(ch, i))
                        GrafSet.LB3.Items.Add(filab(ch, i))
                        GrafSet.LB4.Items.Add(filab(ch, i))
                        GrafSet.LB5.Items.Add(filab(ch, i))
                    Else
                        GrafSet.LB6.Items.Add(filab(ch, i))
                        GrafSet.LB7.Items.Add(filab(ch, i))
                        GrafSet.LB8.Items.Add(filab(ch, i))
                        GrafSet.LB9.Items.Add(filab(ch, i))
                        GrafSet.LB10.Items.Add(filab(ch, i))
                    End If
                    Datmax(ch, i) = 0
                    datmin(ch, i) = 1000000000.0
                Next

                If ch = 1 Then
                    GrafSet.LB1.SelectedIndex = ind(1, 1) - 1 : GrafSet.LU1.Text = DynoMonitor.Anaunit(ind(1, 1) - 1)
                    GrafSet.LB2.SelectedIndex = ind(1, 2) - 1 : GrafSet.LU2.Text = DynoMonitor.Anaunit(ind(1, 2) - 1)
                    GrafSet.LB3.SelectedIndex = ind(1, 3) - 1 : GrafSet.LU3.Text = DynoMonitor.Anaunit(ind(1, 3) - 1)
                    GrafSet.LB4.SelectedIndex = ind(1, 4) - 1 : GrafSet.LU4.Text = DynoMonitor.Anaunit(ind(1, 4) - 1)
                    GrafSet.LB5.SelectedIndex = ind(1, 5) - 1 : GrafSet.LU5.Text = DynoMonitor.Anaunit(ind(1, 5) - 1)
                Else
                    If ind(ch, 1) = 0 Or ch = 3 Or ch = 4 Then
                        ind(ch, 1) = ind(1, 1) : GrafSet.LU6.Text = DynoMonitor.Anaunit(ind(1, 1))
                        ind(ch, 2) = ind(1, 2) : GrafSet.LU7.Text = DynoMonitor.Anaunit(ind(1, 2))
                        ind(ch, 3) = ind(1, 3) : GrafSet.LU8.Text = DynoMonitor.Anaunit(ind(1, 3))
                        ind(ch, 4) = ind(1, 4) : GrafSet.LU9.Text = DynoMonitor.Anaunit(ind(1, 4))
                        ind(ch, 5) = ind(1, 5) : GrafSet.LU10.Text = DynoMonitor.Anaunit(ind(1, 5))
                    End If
                    GrafSet.LB6.SelectedIndex = ind(ch, 1) - 1 : GrafSet.LU6.Text = DynoMonitor.Anaunit(ind(ch, 1) - 1)
                    GrafSet.LB7.SelectedIndex = ind(ch, 2) - 1 : GrafSet.LU7.Text = DynoMonitor.Anaunit(ind(ch, 2) - 1)
                    GrafSet.LB8.SelectedIndex = ind(ch, 3) - 1 : GrafSet.LU8.Text = DynoMonitor.Anaunit(ind(ch, 3) - 1)
                    GrafSet.LB9.SelectedIndex = ind(ch, 4) - 1 : GrafSet.LU9.Text = DynoMonitor.Anaunit(ind(ch, 4) - 1)
                    GrafSet.LB10.SelectedIndex = ind(ch, 5) - 1 : GrafSet.LU10.Text = DynoMonitor.Anaunit(ind(ch, 5) - 1)
                End If

                i = 0
                Do While (s <> "END")
                    i = i + 1
                    s = FileRead.ReadLine()
                    If i >= 2000 Then s = "END" 'Force an end to this madness
                    If Len(s) = 0 Then s = "END"
                    If Mid(s, 1, 3) = "END" Then s = "END" 'Just in case XL edited it and added commas
                    If s <> "END" Then
                        s = s + "0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,"       'Pad just in case prev version of pull with fewer columns
                        sa = s.Split(",")
                        'Runs
                        For j = 1 To DynoMonitor.nch + 1
                            FileDat(ch, i, j) = sa(j - 1)
                            If FileDat(ch, i, j) > Datmax(ch, j) Then Datmax(ch, j) = FileDat(ch, i, j) : indmax(ch, j) = i '1111 moved to post averaging
                            If FileDat(ch, i, j) < datmin(ch, j) Then datmin(ch, j) = FileDat(ch, i, j)
                        Next
                    End If
                Loop
                i = i - 1
                For i = i + 1 To 2000
                    For j = 1 To DynoMonitor.nch + 1                'Erase any old data (This is lazy... I should have a "last point" variable, and only plot to the last point)
                        FileDat(ch, i, j) = 0
                    Next
                Next

                FileRead.Close()            'Gotta use these 2 lines to close the file after reading
                FileRead.Dispose()
                If ch = 1 Then on1 = True
                If ch = 2 Then on2 = True
            End Using
            ready = True
            plot(ch)
        Catch ex As Exception
            MsgBox("File Read Fail at Cannel:" + Str(ch) + ", Line:" + Str(i) + ", Col:" + Str(j) + Chr(13) + ", Ready:" + Str(ready) + ", Last read:" + s)
            MsgBox(calfile + " Corrupted or missing.")            'Use Defaults and create DynoCal file
            ready = True
        End Try
        'FileClose(1)
        'MsgBox("Line 4:" + Str(pcdat(1, 1, 4)) + Str(pcdat(1, 2, 4)) + Str(pcdat(1, 3, 4)) + Str(pcdat(1, 4, 4)))
    End Sub

    

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LData1.Click
        Dim LoadFileBox As New OpenFileDialog
        With LoadFileBox
            .FileName = calfile
            .DefaultExt = "CSV"
            .Filter = "CSV|*.csv"
            .Title = "Pull Data File"
            '  .InitialDirectory = applicationSettingsDirectory
            Dim result = .ShowDialog()
            If result = DialogResult.OK Then
                calfile = .FileName
                LData1.Text = calfile
                on1 = True
                Call loadpulls(1)
            End If
        End With
    End Sub

    Public Sub Chrt1Stats(ByVal e)                      'Sends the statitisctics of the "cursor" to the text boxes
        Dim xpos, min(3), xmindx(3) As Single           'This is UPPER Pow/Torque, Lower AFT grafs (chart 1 and 2) 
        Dim mark1, mark2 As Point
        Dim CiC As Integer          'Chart Index Counter
        Dim lx1t, ly11T, lz1t, lw1t, lx2t, ly21t, lz2t, lw2t, lv1t, lv2t As String
        For j = 1 To 3
            min(j) = 999999999
        Next
        Chart1.Refresh()
        Chart3.Refresh()
        xpos = Chart1.ChartAreas(0).AxisX.Minimum + xmark * (Chart1.ChartAreas(0).AxisX.Maximum - Chart1.ChartAreas(0).AxisX.Minimum)
        'LXY.Text = "Click X: " + xpos.ToString("N0")
        For i = 1 To 2000
            For j = 1 To 3
                If Math.Abs(FileDat(j, i, ind(j, 1)) - xpos) < min(j) Then
                    min(j) = Math.Abs(FileDat(j, i, ind(j, 1)) - xpos)
                    xmindx(j) = i
                End If
            Next
        Next
        If Chart1.Series(0).Points.Count > 3 Then
            With Me.Chart1.Series(0)            'Series 0, 1 Trq, Pow , Chart 2 Series 0 is AFR on lower graf
                For CiC = 0 To Chart1.Series(0).Points.Count - 2
                    If FileDat(1, xmindx(1), ind(1, 1)) >= .Points(CiC).XValue And FileDat(1, xmindx(1), ind(1, 1)) < .Points(CiC + 1).XValue Then
                        Try
                            lx1t = .Points(CiC).XValue.ToString(DynoMonitor.anadec(ind(1, 1) - 1))
                            ly11T = .Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(1, 2) - 1))
                            lz1t = Chart1.Series(1).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(1, 3) - 1))
                            lw1t = Chart2.Series(0).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(1, 4) - 1))
                            lv1t = Chart2.Series(1).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(1, 5) - 1))
                        Catch ex As Exception
                            Beep()                                                                                  'Nothing to do...they just clicked out of range
                        End Try

                    End If
                Next
            End With
        End If



        If Chart1.Series(2).Points.Count > 3 Then
            With Me.Chart1.Series(2)                    'Series 3, 4, 5 are Trq, Pow and AFR "Old graf"
                For CiC = 0 To Chart1.Series(2).Points.Count - 2
                    If FileDat(2, xmindx(2), ind(2, 1)) >= .Points(CiC).XValue And FileDat(2, xmindx(2), ind(2, 1)) < .Points(CiC + 1).XValue Then
                        Try
                            lx2t = Chart1.Series(2).Points(CiC).XValue.ToString(DynoMonitor.anadec(ind(2, 1) - 1))
                            ly21t = Chart1.Series(2).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(2, 2) - 1))
                            lz2t = Chart1.Series(3).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(2, 3) - 1))
                            lw2t = Chart2.Series(2).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(1, 4) - 1))
                            lv2t = Chart2.Series(3).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(1, 5) - 1))
                        Catch ex As Exception
                            Beep()

                        End Try

                    End If
                Next
            End With
        End If
        LabX.Text = DynoMonitor.Analab(ind(1, 1) - 1) : LabY1.Text = DynoMonitor.Analab(ind(1, 2) - 1) : Laby2.Text = DynoMonitor.Analab(ind(1, 3) - 1)
        LaY3.Text = DynoMonitor.Analab(ind(1, 4) - 1) : LAY4.Text = DynoMonitor.Analab(ind(1, 5) - 1)
        LX1.Text = lx1t
        Ly11.Text = ly11T
        Lz1.Text = lz1t
        Lw1.Text = lw1t
        LV1.Text = lv1t
        LX2.Text = lx2t
        Ly21.Text = ly21t
        Lz2.Text = lz2t
        Lw2.Text = lw2t
        LV2.Text = lv2t

        '=== Draw LINE indicating the Point where stats are clicked
        If IsNothing(e) Then
            mark1.X = (0.072 + xmark * 0.825) * Chart1.Width + Chart1.Left
        Else
            mark1.X = e.X - Chart1.Left
        End If
        mark2.X = mark1.X
        mark1.Y = Chart1.Top + Chart1.Height * 0.03
        mark2.Y = Chart1.Top + Chart1.Height * 0.86
        Me.Chart1.CreateGraphics.DrawLine(Pens.Red, mark1, mark2)
        '===
        If IsNothing(e) Then                'Put line on Graf 3(combined)
            mark1.X = (0.072 + xmark * 0.825) * Chart3.Width + Chart3.Left
        Else
            mark1.X = e.X - Chart3.Left
        End If
        mark2.X = mark1.X
        mark1.Y = Chart3.Top + Chart3.Height * 0.03
        mark2.Y = Chart3.Top + Chart3.Height * 0.89
        Me.Chart3.CreateGraphics.DrawLine(Pens.Red, mark1, mark2)

    End Sub
    Private Sub Chart1_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Chart1.MouseDown
        Dim xpos, min(3), xmindx(3) As Single
        xmark = ((e.X - Chart1.Left) / Chart1.Width - 0.072) * 1.21 '% of chart min-max
        If xmark < 0.01 Then xmark = 0.01
        If xmark > 0.99 Then xmark = 0.99
        ' Console.WriteLine("X: " + (100 * (e.X - Chart1.Left) / Chart1.Width).ToString("N1") + "   Mark: " + (100 * xmark).ToString("N1"))
        Call Chrt1stats(e)
    End Sub
    Public Sub inkey(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        ' If e.KeyChar = Convert.ToChar(13) Then
        ' MsgBox("Key Pressed: " + Str(Asc(e.KeyChar)))
        'Console.WriteLine("Key = " + Str(Asc(e.KeyChar)))
        e.Handled = True                'Prevent key strike from making it into a text box or whatever
        If Asc(e.KeyChar) = 27 Then Me.Close()
        If Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 61 Then xmark = xmark + 0.005
        If Asc(e.KeyChar) = 44 Or Asc(e.KeyChar) = 45 Then xmark = xmark - 0.005
        If xmark < 0.01 Then xmark = 0.01
        If xmark > 0.99 Then xmark = 0.99
        Call Chrt1Stats(Nothing)
        ' End If
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean

        ' MsgBox("Cntl Key Pressed: " + Str(keyData)) 'handle your keys here
        'Prevent key strike from making it into a text box or whatever
        If keyData = 39 Then xmark = xmark + 0.005
        If keyData = 37 Then xmark = xmark - 0.005
        If xmark < 0.01 Then xmark = 0.01
        If xmark > 0.99 Then xmark = 0.99
        ' If DynoMonitor.AnaGrTop = True Then
        'Call chrt3stats(Nothing)
        'Else
        ' Call Chrt1Stats(Nothing)
        'End If
        Call Chrt1Stats(Nothing)
    End Function
    Private Sub Chart3_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Chart3.MouseDown
        Dim xpos, min(3), xmindx(3) As Single
        xmark = ((e.X - Chart3.Left) / Chart3.Width - 0.072) * 1.21 '% of chart min-max
        If xmark < 0.01 Then xmark = 0.01
        If xmark > 0.99 Then xmark = 0.99
        ' Console.WriteLine("X: " + (100 * (e.X - Chart1.Left) / Chart1.Width).ToString("N1") + "   Mark: " + (100 * xmark).ToString("N1"))
        'Call chrt3stats(e)
        Call Chrt1Stats(e)
    End Sub
    Private Sub chrt3stats(ByVal e)                 'Graf 3 is the "combined" plot ** No longer used... all moved to chart 1 ststs (it does both)
        'Dim xpos, min(3), xmindx(3) As Single
        'Dim mark1, mark2 As Point
        'Dim ly11T, lz1t, lw1t, lx1t, lx2t, ly21t, lz2t, lw2t As String
        'Dim CiC As Integer
        'For j = 1 To 3
        '    min(j) = 999999999
        'Next
        'Chart3.Refresh()
        '' xpos = ((e.X - Chart3.Left) / Chart3.Width - 0.08) * 1.25 '% of chart min-max
        'xpos = Chart3.ChartAreas(0).AxisX.Minimum + xmark * (Chart3.ChartAreas(0).AxisX.Maximum - Chart3.ChartAreas(0).AxisX.Minimum)
        ''LXY.Text = "Click X: " + xpos.ToString("N0")
        'For i = 1 To 2000
        '    For j = 1 To 3
        '        If Math.Abs(FileDat(j, i, ind(j, 1)) - xpos) < min(j) Then
        '            min(j) = Math.Abs(FileDat(j, i, ind(j, 1)) - xpos)
        '            xmindx(j) = i
        '        End If
        '    Next
        'Next
        'With Me.Chart3.Series(0)                                'This pulls the data from the chart
        '    For CiC = 0 To Chart3.Series(0).Points.Count - 2 'First Run
        '        If FileDat(1, xmindx(1), ind(1, 1)) >= .Points(CiC).XValue And FileDat(1, xmindx(1), ind(1, 1)) < .Points(CiC + 1).XValue Then
        '            lx1t = .Points(CiC).XValue.ToString(DynoMonitor.anadec(ind(1, 1)))
        '            ly11T = .Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(1, 2)))
        '            lz1t = Chart3.Series(1).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(1, 3)))
        '            lw1t = Chart3.Series(2).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(1, 4)))  'GOTTA MULTIPLY BY THE SCALE FACTOR!!!
        '        End If
        '    Next
        '    For CiC = 0 To Chart3.Series(3).Points.Count - 2 'Second Run
        '        If FileDat(2, xmindx(2), ind(2, 1)) >= .Points(CiC).XValue And FileDat(2, xmindx(2), ind(2, 1)) < .Points(CiC + 1).XValue Then
        '            lx2t = .Points(CiC).XValue.ToString(DynoMonitor.anadec(ind(2, 1)))
        '            ly21t = .Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(2, 2)))
        '            lz2t = Chart3.Series(4).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(2, 3)))
        '            lw2t = Chart3.Series(5).Points(CiC).YValues(0).ToString(DynoMonitor.anadec(ind(2, 4)))  'GOTTA MULTIPLY BY THE SCALE FACTOR!!!
        '        End If
        '    Next
        'End With
        'LabX.Text = DynoMonitor.Analab(ind(1, 1) - 1) : LabY1.Text = DynoMonitor.Analab(ind(1, 2) - 1) : Laby2.Text = DynoMonitor.Analab(ind(1, 3) - 1) : LaY3.Text = DynoMonitor.Analab(ind(1, 4) - 1)

        ''LX1.Text = FileDat(1, xmindx(1), ind(1, 1)).ToString(DynoMonitor.anadec(ind(1, 1))) + ":" + lx1t

        'LX1.Text = "{" + lx1t
        'Ly11.Text = "{" + ly11T
        'Lz1.Text = "{" + lz1t
        'Lw1.Text = "{" + lw1t
        ''Ly11.Text = FileDat(1, xmindx(1), ind(1, 2)).ToString(DynoMonitor.anadec(ind(1, 2)))
        ''Lz1.Text = FileDat(1, xmindx(1), ind(1, 3)).ToString(DynoMonitor.anadec(ind(1, 3)))
        ''Lw1.Text = FileDat(1, xmindx(1), ind(1, 4)).ToString(DynoMonitor.anadec(ind(1, 4)))
        'LX2.Text = FileDat(2, xmindx(2), ind(2, 1)).ToString(DynoMonitor.anadec(ind(2, 1))) + "{" + lx2t
        'Ly21.Text = FileDat(2, xmindx(2), ind(2, 2)).ToString(DynoMonitor.anadec(ind(2, 2))) + "{" + ly21t
        'Lz2.Text = FileDat(2, xmindx(2), ind(2, 3)).ToString(DynoMonitor.anadec(ind(2, 3))) + "{" + lz2t
        'Lw2.Text = FileDat(2, xmindx(2), ind(2, 4)).ToString(DynoMonitor.anadec(ind(2, 4))) + "{" + lw2t
        ''MsgBox("1X:" + FileDat(1, 3, ind(1, 1)).ToString + Chr(13) + "1Y:" + FileDat(1, 3, ind(1, 2)).ToString + Chr(13) + "1Z:" + FileDat(1, 3, ind(1, 3)).ToString)
        ''=== Draw LINE indicating the Point where stats are clicked

        'If IsNothing(e) Then
        '    mark1.X = (0.072 + xmark * 0.825) * Chart3.Width + Chart3.Left
        'Else
        '    mark1.X = e.X - Chart3.Left
        'End If
        'mark2.X = mark1.X
        'mark1.Y = Chart3.Top + Chart3.Height * 0.03
        'mark2.Y = Chart3.Top + Chart3.Height * 0.89
        'Me.Chart3.CreateGraphics.DrawLine(Pens.Red, mark1, mark2)
        ''===
    End Sub

    Private Sub ButRep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButRep.Click
        Dim foto As Graphics = Nothing

        Dim bmp As New Bitmap(Me.Width, Me.Height)
        Dim fmloc As System.Drawing.Point = Me.Bounds.Location
        Dim ik, ij, ipts As Integer
        Dim xlApp As Object = Nothing
        Try
            xlApp = CreateObject("Excel.Application")
        Catch
        End Try

        foto = Graphics.FromImage(bmp)
        ' Panel2.DrawToBitmap(bmp, Panel2.DisplayRectangle)
        foto.CopyFromScreen(fmloc.X, fmloc.Y, 0, 0, bmp.Size)          '5, 20 adjust from edge of form 1 to edge of pannel2
        ' Clipboard.SetDataObject(foto, False)
        Clipboard.SetImage(bmp)
        Try
            Dim excelBook As Object = xlApp.Workbooks.Add
            Dim excelWorksheet As Object = excelBook.Worksheets(1)
            xlApp.Visible = True
            With excelWorksheet
                .Cells.Select()
                .Cells.Delete()
                .Cells(1, 1).value = "Focus Applied Technologies EZDYNO Dynamometer Pull Report"
                .Cells(2, 1).value = DynoMonitor.LTagline.Text + "   " + LID.Text
                .Cells(3, 1).value = "SAE Comp:"
                .Cells(3, 2).value = DynoMonitor.SAEcf
                .Range("A4").Select()
                .PasteSpecial(Format:="Bitmap")
                For ik = 1 To 17
                    .Cells(38, ik).Value = filab(1, ik)          'Labels
                Next
                ipts = DynoMonitor.points
                If DynoMonitor.points < 5 Then
                    For ij = 2 To 2000
                        If FileDat(1, ij, 2) <> 0 Then ipts = ij
                    Next
                End If
                For ij = 1 To ipts                       'Rows
                    For ik = 1 To DynoMonitor.nch + 1
                        .Cells(ij + 38, ik).value = FileDat(1, ij, ik)
                    Next ik
                Next ij
                .Rows("1:1").Font.FontStyle = "Bold"
                .Rows("1:1").Font.Size = 10
                .Cells.Select()
                .Cells(1, 1).Select()
            End With
            '  excelBook.SaveAs(LID.Text + ".xlsx")           'Saving was a pain in the ass, cause held up program with a confirmation msgbox
        Catch ex As Exception
            MsgBox("Export Excel Error " & ex.Message)
        Finally
            'RELEASE ALLOACTED RESOURCES
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            xlApp = Nothing
        End Try
    End Sub

    Private Sub SetGrafParams() Handles GrafSet.SetGraf                     'This resets the axis after a "Setup" call
        Dim temp As Single
        ind(1, 1) = GrafSet.LB1.SelectedIndex + 1 : DynoMonitor.Xgraf = ind(1, 1)
        ind(1, 2) = GrafSet.LB2.SelectedIndex + 1 : DynoMonitor.Y1Graf = ind(1, 2)
        ind(1, 3) = GrafSet.LB3.SelectedIndex + 1 : DynoMonitor.Y2graf = ind(1, 3)
        ind(1, 4) = GrafSet.LB4.SelectedIndex + 1 : DynoMonitor.Y3graf = ind(1, 4)
        ind(1, 5) = GrafSet.LB5.SelectedIndex + 1 : DynoMonitor.Y4Graf = ind(1, 5)

        ind(2, 1) = GrafSet.LB6.SelectedIndex + 1
        ind(2, 2) = GrafSet.LB7.SelectedIndex + 1
        ind(2, 3) = GrafSet.LB8.SelectedIndex + 1
        ind(2, 4) = GrafSet.LB9.SelectedIndex + 1
        ind(2, 5) = GrafSet.LB10.SelectedIndex + 1

        DynoMonitor.GraFilt = Val(GrafSet.TBAnaTarg.Text)
        If DynoMonitor.GraFilt < 0 Then DynoMonitor.GraFilt = 0
        If DynoMonitor.GraFilt > 10 Then DynoMonitor.GraFilt = 10
        If DynoMonitor.GManX = True Then
            DynoMonitor.GRx1 = Val(GrafSet.TBX1.Text)
            'DynoMonitor.GRx2 = Val(GrafSet.TBX2.Text)
            If DynoMonitor.GRx1 >= DynoMonitor.GRx2 Then DynoMonitor.GRx2 = DynoMonitor.GRx1 + 1
        End If
        If DynoMonitor.GManY = True Then DynoMonitor.GRy2 = Val(GrafSet.TBY2.Text)
        If DynoMonitor.GMan2Y = True Then DynoMonitor.GR2Y2 = Val(GrafSet.TB2Y2.Text)

        'If DynoMonitor.GLineW <> Val(GrafSet.TBLineThk.Text) Then DynoMonitor.GraFirst = True
        DynoMonitor.GLineW = Val(GrafSet.TBLineThk.Text)
        If DynoMonitor.GLineW > 5 Then DynoMonitor.GLineW = 5
        If DynoMonitor.GLineW < 1 Then DynoMonitor.GLineW = 1
        DynoMonitor.GrAnaTarg = Val(GrafSet.TBAnaT.Text)
        DynoMonitor.GRA1Y1 = Val(GrafSet.TBA1Y1.Text)
        DynoMonitor.GRA1Y2 = Val(GrafSet.TBA1Y2.Text)
        DynoMonitor.GRA2Y1 = Val(GrafSet.TBA2Y1.Text)
        DynoMonitor.GRA2Y2 = Val(GrafSet.TBA2Y2.Text)
        If DynoMonitor.GRA1Y1 > DynoMonitor.GRA1Y2 Then     'If backwards then reverse
            temp = DynoMonitor.GRA1Y2
            DynoMonitor.GRA1Y2 = DynoMonitor.GRA1Y1
            DynoMonitor.GRA1Y1 = temp
        End If
        If DynoMonitor.GRA2Y1 > DynoMonitor.GRA2Y2 Then     'If backwards then reverse
            temp = DynoMonitor.GRA2Y2
            DynoMonitor.GRA2Y2 = DynoMonitor.GRA2Y1
            DynoMonitor.GRA2Y1 = temp
        End If
        If DynoMonitor.GRA1Y1 = DynoMonitor.GRA1Y2 Then DynoMonitor.GRA1Y2 += 1
        If DynoMonitor.GRA2Y1 = DynoMonitor.GRA2Y2 Then DynoMonitor.GRA2Y2 += 1


        'RESET GRAF COLORS

        Call setcolors()


        If ready = True Then Call plot(1) 'Just redo whole plot
        If ready = True And on2 = True Then Call plot(2)
        GrafSet.Hide()
    End Sub
    Sub setcolors()

        'LabX.Text = "-"  
        LabY1.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(1)) ' Stats Labels
        Laby2.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(2))
        LaY3.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(3))
        LAY4.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(4))
        'LX1.Text = "-" 
        Ly11.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(1)) 'Graf 1
        Lz1.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(2))
        Lw1.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(3))
        LV1.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(4))
        'LX2.Text = "-"
        Ly21.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(5)) 'Graf 2
        Lz2.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(6))
        Lw2.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(7))
        LV2.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(8))

        LAMxY11.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(3)) : LAMnY11.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(3))
        LAMxy21.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(4)) : LAMNY21.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(4))

        LAMxY12.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(7)) : LAMNY12.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(7))
        LAMxY22.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(8)) : LAMNY22.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(8))



        Lax1.ForeColor = Color.Black                 'Set up axis Labels
        LAX13.ForeColor = Color.Black
        LaxY11.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(1))
        LaxY21.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(2))
        Lana1.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(3))
        Lana12.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(4))

        LAX2.ForeColor = Color.Black              'Set up axis Labels
        LAX23.ForeColor = Color.Black
        LAXY12.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(5))
        LAXY22.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(6))
        Lana2.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(7))
        Lana22.ForeColor = DynoMonitor.Col(DynoMonitor.Tcol(8))


        Chart1.Series(0).Color = DynoMonitor.Col(DynoMonitor.Tcol(1)) : Chart1.Series(1).Color = DynoMonitor.Col(DynoMonitor.Tcol(2)) ' T1, P1
        Chart2.Series(0).Color = DynoMonitor.Col(DynoMonitor.Tcol(3)) : Chart2.Series(1).Color = DynoMonitor.Col(DynoMonitor.Tcol(4)) ' A1-1, A1-2
        Chart1.Series(2).Color = DynoMonitor.Col(DynoMonitor.Tcol(5)) : Chart1.Series(3).Color = DynoMonitor.Col(DynoMonitor.Tcol(6)) ' T2, P2
        Chart2.Series(2).Color = DynoMonitor.Col(DynoMonitor.Tcol(7)) : Chart2.Series(3).Color = DynoMonitor.Col(DynoMonitor.Tcol(8)) ' A2-1, A2-2


        Chart1.Series(4).Color = Color.Navy : Chart1.Series(5).Color = Color.Purple
        Chart1.Series(6).Color = Color.DodgerBlue : Chart1.Series(7).Color = Color.IndianRed

        Chart3.Series(0).Color = DynoMonitor.Col(DynoMonitor.Tcol(1)) : Chart3.Series(1).Color = DynoMonitor.Col(DynoMonitor.Tcol(2)) ' T1, P1
        Chart3.Series(2).Color = DynoMonitor.Col(DynoMonitor.Tcol(3)) : Chart3.Series(3).Color = DynoMonitor.Col(DynoMonitor.Tcol(4)) ' A1-1, A1-2
        Chart3.Series(4).Color = DynoMonitor.Col(DynoMonitor.Tcol(5)) : Chart3.Series(5).Color = DynoMonitor.Col(DynoMonitor.Tcol(6)) ' T2, P2
        Chart3.Series(6).Color = DynoMonitor.Col(DynoMonitor.Tcol(7)) : Chart3.Series(7).Color = DynoMonitor.Col(DynoMonitor.Tcol(8)) ' A2-1, A2-2
        Chart3.Series(8).Color = Color.DarkGreen
        Chart3.Series(9).Color = Color.DodgerBlue : Chart3.Series(10).Color = Color.IndianRed : Chart3.Series(11).Color = Color.Lime

        Chart2.Series(4).Color = Color.DarkGoldenrod : Chart3.Series(12).Color = Color.DarkGoldenrod                             'AFR Target Values

        Chart1.ChartAreas(0).BackColor = DynoMonitor.ColGrafBak(DynoMonitor.GBaCol)
        Chart2.ChartAreas(0).BackColor = DynoMonitor.ColGrafBak(DynoMonitor.GBaCol)
        Chart3.ChartAreas(0).BackColor = DynoMonitor.ColGrafBak(DynoMonitor.GBaCol)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButJPG.Click

        Dim foto As Graphics = Nothing
        Dim BMPW As Integer = Me.Width * DynoMonitor.MAG
        Dim BMPH As Integer = Me.Height * DynoMonitor.MAG
        Dim bmp As New Bitmap(BMPW, BMPH)
        Dim fmloc As System.Drawing.Point = Me.Bounds.Location
        Dim Sav As New SaveFileDialog
        'Dim scrnsize As Size
        'Dim fmloc As System.Drawing.Point = Me.PointToScreen(Me.Panel1.Location)
        foto = Graphics.FromImage(bmp)
        ' Panel2.DrawToBitmap(bmp, Panel2.DisplayRectangle)
        'scrnsize = bmp.Size
        'scrnsize.Width = scrnsize.Width * 0.78
        foto.CopyFromScreen(fmloc.X * DynoMonitor.MAG, fmloc.Y * DynoMonitor.MAG, 0, 0, bmp.Size)          '5, 20 adjust from edge of form 1 to edge of pannel2
        Sav.Filter = "JPG Image|*.jpg"
        Sav.Title = ("Save Screen Shot")
        Sav.FileName = LID.Text + ".JPG"
        If Sav.ShowDialog() = DialogResult.OK Then
            If Sav.FileName <> "" Then bmp.Save(Sav.FileName, System.Drawing.Imaging.ImageFormat.Jpeg)

        End If

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtPrint.Click
        Dim foto As Graphics = Nothing
        Dim BMPW As Integer = Me.Width * DynoMonitor.MAG
        Dim BMPH As Integer = Me.Height * DynoMonitor.MAG
        Dim bmp As New Bitmap(BMPW, BMPH)
        Dim fmloc As System.Drawing.Point = Me.Bounds.Location

        foto = Graphics.FromImage(bmp)
        foto.CopyFromScreen(fmloc.X * DynoMonitor.MAG, fmloc.Y * DynoMonitor.MAG, 0, 0, bmp.Size)
        'foto.CopyFromScreen(fmloc.X, fmloc.Y, 0, 0, bmp.Size)          '5, 20 adjust from edge of form 1 to edge of pannel2

        Dim prnDialog As New PrintDialog()
        prtfoto = New Bitmap(bmp, bmp.Width, bmp.Height)      'Make smaller to fit A4 paper
        prnDialog.Document = prntDoc
        ' Optional Dialog:   
        Dim r As DialogResult = prnDialog.ShowDialog
        prntDoc.DefaultPageSettings.Landscape = True
        If r = DialogResult.OK Then prntDoc.Print()
    End Sub
    Private Sub prntDoc_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles prntDoc.PrintPage
        e.Graphics.DrawImage(prtfoto, e.MarginBounds)         'This does the actual printing
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSetup.Click
        If GrafSet.Visible Then
            GrafSet.Hide()
            DynoMonitor.GraFilt = Val(GrafSet.TBAnaTarg.Text)
            If DynoMonitor.GraFilt < 0 Then DynoMonitor.GraFilt = 0
            If DynoMonitor.GraFilt > 10 Then DynoMonitor.GraFilt = 10
            ' Console.WriteLine("Set Filt :" + GrafSet.TBAnaTarg.Text)
            If DynoMonitor.GManX = True Then
                DynoMonitor.GRx1 = Val(GrafSet.TBX1.Text)
                'DynoMonitor.GRx2 = Val(GrafSet.TBX2.Text)
            End If
        Else
            GrafSet.Show()
            GrafSet.BACGraf.Text = "YES"
            If DynoMonitor.AutoCloseGraf = False Then GrafSet.BACGraf.Text = "NO"
            GrafSet.TBAnaT.Text = Str(DynoMonitor.GrAnaTarg)
            GrafSet.ButBaCol.BackColor = DynoMonitor.ColGrafBak(DynoMonitor.GBaCol)
            GrafSet.TBLineThk.Text = Str(DynoMonitor.GLineW)
            If DynoMonitor.GAxSame = True Then
                GrafSet.ButSame.Text = "SAME"
                GrafSet.LB6.BackColor = Color.LightGray
                GrafSet.LB7.BackColor = Color.LightGray
                GrafSet.LB8.BackColor = Color.LightGray
                GrafSet.LB9.BackColor = Color.LightGray
                GrafSet.LB10.BackColor = Color.LightGray
            Else
                GrafSet.LB6.BackColor = Color.White
                GrafSet.LB7.BackColor = Color.White
                GrafSet.LB8.BackColor = Color.White
                GrafSet.LB9.BackColor = Color.White
                GrafSet.LB10.BackColor = Color.White
                GrafSet.ButSame.Text = "DIFFERENT"
            End If
            GrafSet.TBAnaTarg.Text = Str(DynoMonitor.GraFilt)
            If DynoMonitor.grafgrid = True Then
                GrafSet.Bgraf.Text = "FINE"
            Else
                GrafSet.Bgraf.Text = "CORSE"
            End If
            If DynoMonitor.AnaGrTop = True Then
                GrafSet.ButAnaGraf.Text = "COMBINED"
            Else
                GrafSet.ButAnaGraf.Text = "SEPERATE"
            End If
            If DynoMonitor.bigsum = True Then
                GrafSet.ButBS.Text = "ON"
            Else
                GrafSet.ButBS.Text = "OFF"
            End If
            If DynoMonitor.G2Last = True Then
                GrafSet.But2Last.Text = "YES"
            Else
                GrafSet.But2Last.Text = "NO"
            End If

            GrafSet.TBX1.Text = DynoMonitor.GRx1        'Set up X-Axis
            If DynoMonitor.GManX = True Then
                GrafSet.LAX1.Text = "MANUAL"
                GrafSet.TBX1.BackColor = Color.White
            Else
                GrafSet.LAX1.Text = "AUTO"
                GrafSet.TBX1.BackColor = Color.LightGray
            End If

            GrafSet.TBY2.Text = DynoMonitor.GRy2        'Setup Y1 Axis
            If DynoMonitor.GManY = True Then
                GrafSet.LAY1.Text = "MANUAL"
                GrafSet.TBY2.BackColor = Color.White
            Else
                GrafSet.LAY1.Text = "AUTO"
                GrafSet.TBY2.BackColor = Color.LightGray
            End If

            GrafSet.TB2Y2.Text = DynoMonitor.GR2Y2      'Setup Y2 Axis
            If DynoMonitor.GMan2Y = True Then
                GrafSet.LAY2.Text = "MANUAL"
                GrafSet.TB2Y2.BackColor = Color.White
            Else
                GrafSet.LAY2.Text = "AUTO"
                GrafSet.TB2Y2.BackColor = Color.LightGray
            End If

            GrafSet.TBA1Y1.Text = DynoMonitor.GRA1Y1
            GrafSet.TBA1Y2.Text = DynoMonitor.GRA1Y2
            GrafSet.TBA2Y1.Text = DynoMonitor.GRA2Y1
            GrafSet.TBA2Y2.Text = DynoMonitor.GRA2Y2
            If DynoMonitor.AutoAnaY1 = True Then
                GrafSet.TBA1Y1.BackColor = Color.LightGray
                GrafSet.TBA1Y2.BackColor = Color.LightGray
            End If
            If DynoMonitor.AutoAnaY2 = True Then
                GrafSet.TBA2Y1.BackColor = Color.LightGray
                GrafSet.TBA2Y2.BackColor = Color.LightGray
            End If
            'Populate Lists and etc. 
        End If
    End Sub


    Private Sub ButG3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButG3.Click
        Dim LoadFileBox As New OpenFileDialog
        With LoadFileBox
            .FileName = calfile
            .DefaultExt = "CSV"
            .Filter = "CSV|*.csv"
            .Title = "Pull Data File"
            '  .InitialDirectory = applicationSettingsDirectory
            Dim result = .ShowDialog()
            If result = DialogResult.OK Then
                calfile = .FileName
                ButFile.Text = calfile
                on2 = True
                Call loadpulls(3)
            Else                                'Else remove Graf 3 from graf
                Console.WriteLine("Clearing Graf 3")

                Chart1.Series((3 - 1) * 2).Points.Clear()
                Chart1.Series((3 - 1) * 2 + 1).Points.Clear()
                Chart2.Series((3 - 1) * 2).Points.Clear()
                Chart2.Series((3 - 1) * 2 + 1).Points.Clear()
                Chart3.Series((3 - 1) * 4).Points.Clear()
                Chart3.Series((3 - 1) * 4 + 1).Points.Clear()
                Chart3.Series((3 - 1) * 4 + 2).Points.Clear()
                Chart3.Series((3 - 1) * 4 + 2).Points.Clear()
            End If
        End With
    End Sub

    Private Sub ButG4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButG4.Click
        Dim LoadFileBox As New OpenFileDialog
        With LoadFileBox
            .FileName = calfile
            .DefaultExt = "CSV"
            .Filter = "CSV|*.csv"
            .Title = "Pull Data File"
            '  .InitialDirectory = applicationSettingsDirectory
            Dim result = .ShowDialog()
            If result = DialogResult.OK Then
                calfile = .FileName
                ButFile.Text = calfile
                on2 = True
                Call loadpulls(4)
            Else                            'Else remove Graf 4
                Console.WriteLine("Clearing Graf 4")

                Chart1.Series((4 - 1) * 2).Points.Clear()
                Chart1.Series((4 - 1) * 2 + 1).Points.Clear()
                Chart2.Series((4 - 1) * 2).Points.Clear()
                Chart2.Series((4 - 1) * 2 + 1).Points.Clear()
                Chart3.Series((4 - 1) * 4).Points.Clear()
                Chart3.Series((4 - 1) * 4 + 1).Points.Clear()
                Chart3.Series((4 - 1) * 4 + 2).Points.Clear()
                Chart3.Series((4 - 1) * 4 + 2).Points.Clear()
            End If
        End With
    End Sub

    Private Sub Button1_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim j As Integer
        For j = 2 To 4
            Chart1.Series((j - 1) * 2).Points.Clear()
            Chart1.Series((j - 1) * 2 + 1).Points.Clear()
            Chart2.Series((j - 1) * 2).Points.Clear()
            Chart2.Series((j - 1) * 2 + 1).Points.Clear()
            Chart3.Series((j - 1) * 4).Points.Clear()
            Chart3.Series((j - 1) * 4 + 1).Points.Clear()
            Chart3.Series((j - 1) * 4 + 2).Points.Clear()
            Chart3.Series((j - 1) * 4 + 3).Points.Clear()
        Next
        ButFile.Text = "Click to load file"
        Panel2.Hide()
    End Sub
    
End Class