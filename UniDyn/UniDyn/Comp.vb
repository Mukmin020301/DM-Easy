Public Class Comp
    Private WithEvents GrafSet As New GrafSet
    Friend WithEvents prntDoc As New Printing.PrintDocument

    Dim prtfoto As Bitmap
    Dim calfile, filab(5, 30) As String                        'File Labels
    Dim FileDat(5, 3000, 30), Datmax(5, 30), datmin(5, 30) As Double                      'Pull Data , Maximums
    Dim indmax(10, 30), ind(10, 5) As Integer                     'Index of Maximum and Index of data series for plotting
    Dim ldindx, i, j, k, r, frun, chmax As Integer
    Dim lastdat, i1, i2 As Integer
    Dim x1, x2, x3, y1, y2, y3, y4 As Double
    Dim ftime As String
    Dim TBM(30), PBM(30) As Label
    Dim gmin, gmax As Single                                                'Graf axis min/max
    Dim gmx(5, 10) As Double
    Dim ready, on1, on2 As Boolean                                        'Readt to plot? Graf 1 ON?, Graf 2? 
    Public Shared first As Boolean                                        'First time drawing screen?
    Public xmark, Gxint As Single                                                     'X position marker on graf


    Sub MyForm_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.VisibleChanged
        Console.WriteLine("SUB: Vis Changed")
        'EnableCloseButton(Me.hWnd, False)
        If Me.Visible Then
            xmark = 0.5                                 'Start the stats marker in the middle 
            on1 = False
            on2 = False
            ready = False
            Tbtit.Text = UniDyn.LTagline.Text
            ' TBM(1) = LM1 : PBM(1) = LY1

            Me.Left = UniDyn.GRAFx           'Default Graf position, height
            Me.Top = UniDyn.GRAFy
            Me.Height = UniDyn.GRAFh
            Me.Width = UniDyn.GRAFw

            'Chart1.BackImage = "ssd" 'UniDyn.grafback

            GrafSet.LB1.Items.Clear()
            GrafSet.LB1.Items.Add("Time")                   'Gotta reset labels each time as they may have changed
            GrafSet.LB2.Items.Clear()
            GrafSet.LB2.Items.Add("Time")
            GrafSet.LB3.Items.Clear()
            GrafSet.LB3.Items.Add("Time")
            GrafSet.LB4.Items.Clear()
            GrafSet.LB4.Items.Add("Time")
            GrafSet.LB15.Items.Clear()
            GrafSet.LB15.Items.Add("Time")

            GrafSet.LB5.Items.Clear()
            GrafSet.LB5.Items.Add("Time")                   'Gotta reset labels each time as they may have changed
            GrafSet.LB6.Items.Clear()
            GrafSet.LB6.Items.Add("Time")
            GrafSet.LB7.Items.Clear()
            GrafSet.LB7.Items.Add("Time")
            GrafSet.LB8.Items.Clear()
            GrafSet.LB8.Items.Add("Time")
            GrafSet.LB19.Items.Clear()
            GrafSet.LB19.Items.Add("Time")
            ' MsgBox("Five")
            filab(1, 1) = "Time"
            For i = 1 To 27 'UniDyn.nch                            'Populate Selection Boxws with Channels
                Datmax(1, i + 1) = 0                                'Clear Maximums
                indmax(1, i + 1) = 0                                'And Index of max's
                filab(1, i + 1) = UniDyn.alab(i)             'And assign rest of labels
                GrafSet.LB1.Items.Add(UniDyn.alab(i)) : GrafSet.LB5.Items.Add(UniDyn.alab(i))
                GrafSet.LB2.Items.Add(UniDyn.alab(i)) : GrafSet.LB6.Items.Add(UniDyn.alab(i))
                GrafSet.LB3.Items.Add(UniDyn.alab(i)) : GrafSet.LB7.Items.Add(UniDyn.alab(i))
                GrafSet.LB4.Items.Add(UniDyn.alab(i)) : GrafSet.LB8.Items.Add(UniDyn.alab(i))
                GrafSet.LB15.Items.Add(UniDyn.alab(i)) : GrafSet.LB19.Items.Add(UniDyn.alab(i))
            Next
            'MsgBox("six")
            'Undex Items are:
            '1      2       3       4       5       6       7       8       9       10      11      12      13      14   15   16
            'Time   RPM1    RPM1    RPMe    Trqd    TrqT    PowkW   PowHp   Load    Ttl     A1      A2      A3      A4   Dist kph
            ind(1, 1) = UniDyn.xgraf : GrafSet.LB1.SelectedIndex = ind(1, 1) - 1 : GrafSet.LB5.SelectedIndex = ind(1, 1) - 1    'RPM
            ind(1, 2) = UniDyn.y1graf : GrafSet.LB2.SelectedIndex = ind(1, 2) - 1 : GrafSet.LB6.SelectedIndex = ind(1, 2) - 1   'Torq
            ind(1, 3) = UniDyn.y2graf : GrafSet.LB3.SelectedIndex = ind(1, 3) - 1 : GrafSet.LB7.SelectedIndex = ind(1, 3) - 1    'Power
            ind(1, 4) = UniDyn.y3graf : GrafSet.LB4.SelectedIndex = ind(1, 4) - 1 : GrafSet.LB8.SelectedIndex = ind(1, 4) - 1   'AFR
            ind(1, 5) = UniDyn.y4graf : GrafSet.LB15.SelectedIndex = ind(1, 5) - 1 : GrafSet.LB19.SelectedIndex = ind(1, 5) - 1   'AFR2 or Boost
            ind(4, 1) = ind(1, 1) : ind(5, 1) = ind(1, 1) 'Where's ind(2,x) and Ind(3,x)? These must be unused... beter to delete
            ind(4, 2) = ind(1, 2) : ind(5, 2) = ind(1, 2)
            ind(4, 3) = ind(1, 3) : ind(5, 3) = ind(1, 3)
            ind(2, 1) = ind(1, 1)
            ind(2, 2) = ind(1, 2)
            ind(2, 3) = ind(1, 3)
            ind(2, 4) = ind(1, 4)
            ind(2, 5) = ind(1, 5)
            For i = 1 To 2000
                For j = 1 To 20
                    FileDat(1, i, j) = UniDyn.pulldata(UniDyn.dic, i, j)
                Next
            Next
            filab(1, 1) = "Time"
            For i = 1 To 27                                 'All of the labels get added...
                Datmax(1, i + 1) = 0
                indmax(1, i + 1) = 0
                filab(1, i + 1) = UniDyn.alab(i)
                For j = 1 To 8                              '... to each Combobox 
                    UniDyn.ls(j).Items.Add(UniDyn.alab(i))
                Next
            Next
            'Undex Items are:
            '1      2       3       4       5       6       7       8       9       10      11      12      13      14     15
            'Time   RPM1    RPM1    RPMe    Trqd    TrqT    PowkW   PowHp   Load    Ttl     A1      A2      A3      A4     Dist (m)

            If first = True Then
                first = False
                Chart1.Invalidate()
                Chart1.Series.Clear() 'Torque Curve Graph
                Chart2.Invalidate()
                Chart2.Series.Clear()
                'Chart1.BackImage = Image.FromFile(My.Resources.KZR)
                ' Chart1.BackImage = My.Resources.krz


                Chart2.Series.Add("Analog1")
                Chart2.Series(0).BorderWidth = 3
                Chart2.Series(0).IsVisibleInLegend = False
                Chart2.Series(0).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart2.Series.Add("Analog1-2")
                Chart2.Series(1).BorderWidth = 1
                Chart2.Series(1).IsVisibleInLegend = False
                Chart2.Series(1).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart2.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart2.Series.Add("Analog2-1")
                Chart2.Series(2).BorderWidth = 1
                Chart2.Series(2).IsVisibleInLegend = False
                Chart2.Series(2).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart2.Series(2).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Chart2.Series(2).Color = Color.Aqua
                LW3.ForeColor = Color.Aqua

                Chart2.Series.Add("Analog2-2")
                Chart2.Series(3).BorderWidth = 1
                Chart2.Series(3).IsVisibleInLegend = False
                Chart2.Series(3).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart2.Series(3).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Chart2.Series(3).Color = Color.Turquoise
                LW4.ForeColor = Color.Turquoise

                Chart2.Series.Add("Analog3-1")
                Chart2.Series(4).BorderWidth = 1
                Chart2.Series(4).IsVisibleInLegend = False
                Chart2.Series(4).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Chart2.Series.Add("Analog3-2")
                Chart2.Series(5).BorderWidth = 1
                Chart2.Series(5).IsVisibleInLegend = False
                Chart2.Series(5).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Chart2.Series.Add("Analog4-1")
                Chart2.Series(6).BorderWidth = 1
                Chart2.Series(6).IsVisibleInLegend = False
                Chart2.Series(6).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Chart2.Series.Add("Analog4-2")
                Chart2.Series(7).BorderWidth = 1
                Chart2.Series(7).IsVisibleInLegend = False
                Chart2.Series(7).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart2.Series.Add("AnalogRef")
                Chart2.Series(8).BorderWidth = 1
                Chart2.Series(8).IsVisibleInLegend = False
                Chart2.Series(8).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Chart2.Series(8).Color = Color.Orange
                Chart2.Series(8).BorderDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

                Chart1.Series.Add("Torque1")
                Chart1.Series(0).BorderWidth = 4
                Chart1.Series(0).IsVisibleInLegend = False
                Chart1.Series(0).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart1.Series.Add("Power1")
                Chart1.Series(1).BorderWidth = 4
                Chart1.Series(1).YAxisType = DataVisualization.Charting.AxisType.Secondary
                Chart1.Series(1).IsVisibleInLegend = False
                Chart1.Series(1).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart1.Series.Add("Torque2")
                Chart1.Series(2).BorderWidth = 3
                Chart1.Series(2).IsVisibleInLegend = False
                Chart1.Series(2).ToolTip = "#VAL{0.0} at #VALX{0}"
                Chart1.Series(2).ChartType = DataVisualization.Charting.SeriesChartType.FastLine

                Chart1.Series.Add("Power2")
                Chart1.Series(3).BorderWidth = 3
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

                'Colors replaced with Set Colot routine
                'Chart1.Series(0).Color = Color.DarkBlue : Chart1.Series(1).Color = Color.DarkRed : Chart2.Series(0).Color = Color.Black
                'Chart1.Series(2).Color = Color.DarkBlue : Chart1.Series(3).Color = Color.DarkRed : Chart2.Series(1).Color = Color.Black
                'Chart1.Series(4).Color = Color.Blue : Chart1.Series(5).Color = Color.Red : Chart2.Series(2).Color = Color.DarkGray
                'Chart1.Series(6).Color = Color.LightBlue : Chart1.Series(7).Color = Color.LightCoral : Chart2.Series(3).Color = Color.Gray
            End If

            Chart1.Series(4).Points.Clear()                  'Erase old graf #3 and 4
            Chart1.Series(5).Points.Clear()
            Chart1.Series(6).Points.Clear()
            Chart1.Series(7).Points.Clear()
            Chart2.Series(4).Points.Clear()             'Graf 3 Analog 1
            Chart2.Series(5).Points.Clear()

            Chart2.Series(6).Points.Clear()             'Missed these before?
            Chart2.Series(7).Points.Clear()             'Dito?
            Chart2.Series(8).Points.Clear()
            For i = 1 To 20
                Datmax(1, i) = 0                            'Reset Max's
                datmin(1, i) = 1000000000.0
            Next
            For i = 1 To UniDyn.points
                For j = 1 To 16                                'Get Maximums and Index of Max's
                    If FileDat(1, i, j) > Datmax(1, j) Then Datmax(1, j) = FileDat(1, i, j) : indmax(1, j) = i
                    If FileDat(1, i, j) < datmin(1, j) Then datmin(1, j) = FileDat(1, i, j)
                    'Add data fot the selected graphs
                Next
                'Check to see that not zreos
            Next

            Try
                Chart1.ChartAreas(0).AxisX.LabelStyle.Format = "N0"
                Chart1.ChartAreas(0).AxisX.Maximum = 1
                Chart1.ChartAreas(0).AxisX.Minimum = 0
                Chart1.ChartAreas(0).AxisY.Maximum = 1
                Chart1.ChartAreas(0).AxisY.Minimum = 0
                Chart1.ChartAreas(0).AxisY2.Maximum = 1
                Chart1.ChartAreas(0).AxisY2.Minimum = 0

                Chart2.ChartAreas(0).AxisX.LabelStyle.Format = "N0"
                Chart2.ChartAreas(0).AxisX.Maximum = 1
                Chart2.ChartAreas(0).AxisX.Minimum = 0
                Chart2.ChartAreas(0).AxisY.Maximum = 1
                Chart2.ChartAreas(0).AxisY.Minimum = 0

                gmin = UniDyn.amin(ind(1, 4) - 1) ' Ind(1,4)        'AFR Graph Min
                Chart2.ChartAreas(0).AxisY.Minimum = gmin
                ' Console.WriteLine("Gr1 AFR Channel: " + Str(ind(1, 4) - 1) + " Min: " + Str(UniDyn.amin(ind(1, 4) - 1)) + "   Max: " + Str(UniDyn.amax(ind(1, 4) - 1)))
                gmax = UniDyn.amax(ind(1, 4) - 1)
                If gmax = gmin Then gmax = gmin + 1
                If gmax > Chart2.ChartAreas(0).AxisX.Minimum Then
                    Chart2.ChartAreas(0).AxisY.Maximum = gmax
                Else
                    Chart2.ChartAreas(0).AxisY.Maximum = gmax + 1
                End If
                Chart1.ChartAreas(0).AxisY.MaximumAutoSize = 100
            Catch exmm As Exception
                MsgBox("Graf Min/Max Error.")
            End Try
        Else                                            'Form is CLOSING
            UniDyn.GRAFx = Me.Left          'Default Graf position, height
            UniDyn.GRAFy = Me.Top
            UniDyn.GRAFh = Me.Height
            UniDyn.GRAFw = Me.Width
        End If
        If UniDyn.points > 5 Then on1 = True 'Qualify that we got a trace 
        If on1 = True Then Call plot(1)
        setcolors()
        ready = True
    End Sub
    Sub plot(ByVal ch As Integer)
        Dim xmax, xmin, FCH As Integer          'Fake CG (use 1 indicies for CH 3, 4)
        Dim s As String
        Console.WriteLine("SUB: Plot")
        Chart1.Series((ch - 1) * 2).Points.Clear()
        Chart1.Series((ch - 1) * 2 + 1).Points.Clear()
        Chart2.Series((ch - 1) * 2).Points.Clear()
        Chart2.Series((ch - 1) * 2 + 1).Points.Clear()
        Try
            Chart2.ChartAreas(0).AxisY.Minimum = UniDyn.amin(ind(1, 4) - 1)         'AFR Graph Min
            Chart2.ChartAreas(0).AxisY.Maximum = UniDyn.amin(ind(1, 4) - 1) + 1
            If UniDyn.amax(ind(1, 4) - 1) > Chart2.ChartAreas(0).AxisY.Minimum Then
                Chart2.ChartAreas(0).AxisY.Maximum = UniDyn.amax(ind(1, 4) - 1)
            Else
                Chart2.ChartAreas(0).AxisY.Maximum = Chart2.ChartAreas(0).AxisY.Minimum + 1
            End If
            If ind(1, 4) = 1 Then                                   ' Special min/max for TIME 8888
                Chart2.ChartAreas(0).AxisY.Minimum = 0
                Chart2.ChartAreas(0).AxisY.Maximum = 20
                If UniDyn.timelaps > 0 Then Chart2.ChartAreas(0).AxisY.Maximum = UniDyn.timelaps
                Console.WriteLine("TIME!")
            End If
        Catch ex As Exception
            Chart2.ChartAreas(0).AxisY.Minimum = 0
            Chart2.ChartAreas(0).AxisY.Maximum = 1
        End Try
        


        For i = 1 To 5
            gmx(ch, i) = 0                                  'Clear Max's
        Next i
        'Max set for AFR channel selected

        'For i = 1 To 2000
        '    If FileDat(ch, i, ind(ch, 1)) <> 0 Then
        '        Chart1.Series((ch - 1) * 2).Points.AddXY(FileDat(ch, i, ind(ch, 1)), FileDat(ch, i, ind(ch, 2)))
        '        Chart1.Series((ch - 1) * 2 + 1).Points.AddXY(FileDat(ch, i, ind(ch, 1)), FileDat(ch, i, ind(ch, 3)))
        '        Chart2.Series(ch - 1).Points.AddXY(FileDat(ch, i, ind(ch, 1)), FileDat(ch, i, ind(ch, 4)))
        '    End If

        'Next
        'BEGIN AVERAGED TRACE PLOTTING========================================================================================================
        ' FCH = ch
        ' If ch > 2 Then FCH = 1
        For i = 1 To 2000                                   'Find last data point
            If FileDat(ch, i, ind(ch, 1)) = 0 Then
                lastdat = i - 1
                Exit For
            End If
        Next
        If lastdat >= 2000 Then lastdat = 2000

        For i = 1 To lastdat
            ' If FileDat(ch, i, ind(ch, 1)) <> 0 Then         'x=FileDat(ch, i, ind(ch, 1)), y=FileDat(ch, i, ind(ch, 2)) unless filtering, then x=(for j=i-5 to i+5)...
            i1 = i - UniDyn.GraFilt
            i2 = i + UniDyn.GraFilt
            If i1 < 1 Then i1 = 1
            If i2 > lastdat Then i2 = lastdat 'Set bounds of averaging
            x1 = 0 : y1 = 0 : y2 = 0 : y3 = 0 : y4 = 0
            For j = i1 To i2
                x1 = x1 + FileDat(ch, j, ind(ch, 1))
                y1 = y1 + FileDat(ch, j, ind(ch, 2))
                y2 = y2 + FileDat(ch, j, ind(ch, 3))
                y3 = y3 + FileDat(ch, j, ind(ch, 4))
                y4 = y4 + FileDat(ch, j, ind(ch, 5))
            Next

            '   x1 = FileDat(ch, i, ind(ch, 1)) : y1 = FileDat(ch, i, ind(ch, 2))           'These are the non-averaged numbers
            '   x2 = FileDat(ch, i, ind(ch, 1)) : y2 = FileDat(ch, i, ind(ch, 3))
            '   x3 = FileDat(ch, i, ind(ch, 1)) : y3 = FileDat(ch, i, ind(ch, 4))
            x1 = x1 / (i2 - i1 + 1)
            y1 = y1 / (i2 - i1 + 1)
            y2 = y2 / (i2 - i1 + 1)
            y3 = y3 / (i2 - i1 + 1)
            y4 = y4 / (i2 - i1 + 1)
            x2 = x1
            x3 = x1
            ' afrx(i) = x1
            ' afry(i) = y3
            If x1 > gmx(ch, 1) Then gmx(ch, 1) = x1 'Assign maxes' for box   1111
            If y1 > gmx(ch, 2) Then gmx(ch, 2) = y1
            If y2 > gmx(ch, 3) Then gmx(ch, 3) = y2
            If y3 > gmx(ch, 4) Then gmx(ch, 4) = y3
            If y4 > gmx(ch, 5) Then gmx(ch, 5) = y4

            Chart1.Series((ch - 1) * 2).Points.AddXY(x1, y1)
            Chart1.Series((ch - 1) * 2 + 1).Points.AddXY(x2, y2)
            Chart2.Series((ch - 1) * 2).Points.AddXY(x3, y3)                      'AFR
            Chart2.Series((ch - 1) * 2 + 1).Points.AddXY(x3, y4)                'Analog #2

        Next    'END AVERAGING ============================================================================================================================

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
        ' MsgBox("1: " + Str(datmin(1, ind(1, 1))) + " : " + Str(Datmax(1, ind(1, 1))) + Str(on1) + Chr(13) + "2: " + Str(datmin(2, ind(2, 1))) + " : " + Str(Datmax(2, ind(2, 1))) + Str(on2) + Chr(13) + "Plot " + Str(ch) + "  Min:" + Str(xmin) + "   Max:" + Str(xmax))

        If on1 Or on2 Then
            If xmax - xmin > 500 Then
                xmax = Int((xmax + 100) / 10) * 10              'Rounds X axis to nearest 100 (ie. for RPM's in the 1000's)
                xmin = Int(xmin / 100) * 100
            Else
                xmax = Int((xmax + 1))                          'This just rounds to nearest "1" for small X values (ie. time)
                xmin = Int(xmin)
            End If

            If xmax <= xmin Then xmax = xmin + 1 'This is to catch "partial" pulls where there is no real data
            Debug.WriteLine("Xmin:" + Str(xmin) + " Xmax:" + Str(xmax))
            Chart1.ChartAreas(0).AxisX.Minimum = Int(xmin)
            Chart1.ChartAreas(0).AxisX.Maximum = Int(xmax)
            Chart2.ChartAreas(0).AxisX.Minimum = Int(xmin)
            Chart2.ChartAreas(0).AxisX.Maximum = Int(xmax)

            Console.WriteLine("XInt:" + Str((Int(xmax) - Int(xmin)) / 10))
            Chart1.ChartAreas(0).AxisX.Interval = (Int(xmax) - Int(xmin)) / 10                  'Set 10 major axis interval
            Chart2.ChartAreas(0).AxisX.Interval = (Int(xmax) - Int(xmin)) / 10

            Chart1.ResetAutoValues()
            Chart1.ChartAreas(0).RecalculateAxesScale()
            Chart1.ChartAreas(0).AxisY.Maximum = Int((Datmax(1, ind(1, 2)) * 1.1))
            Chart1.ChartAreas(0).AxisY2.Maximum = Int((Datmax(1, ind(1, 3)) * 1.1))
            Chart1.ChartAreas(0).AxisY.MajorTickMark.Interval = Int((Chart1.ChartAreas(0).AxisY.Minimum - Chart1.ChartAreas(0).AxisY.Minimum) / 5)
            Chart1.ChartAreas(0).AxisY.MajorTickMark.Interval = Int((Chart1.ChartAreas(0).AxisY2.Minimum - Chart1.ChartAreas(0).AxisY2.Minimum) / 5)
            Console.WriteLine("Y1 Grid: " + Str(Int((Datmax(1, ind(1, 2)) - Chart1.ChartAreas(0).AxisY.Minimum) / 5)))
            Console.WriteLine("Y2 Grid: " + Str(Int((Datmax(1, ind(1, 3)) - Chart1.ChartAreas(0).AxisY2.Minimum) / 5)))
            Chart2.ResetAutoValues()
            If ch = 1 Then
                lmaxPull1L.Text = LData1.Text '9999
                ' LMaxPull1.Text = "   Max:" + Datmax(1, ind(1, 2)).ToString(UniDyn.adec(ind(ch, 2))) + " " + UniDyn.auni(ind(1, 2) - 1) + " at " + FileDat(1, indmax(1, ind(ch, 2)), ind(1, 1)).ToString(UniDyn.adec(ind(ch, 1)))

                LMaxPull1.Text = "   Max:" + gmx(1, 2).ToString(UniDyn.adec(ind(ch, 2))) + " " + UniDyn.auni(ind(1, 2) - 1) + " at " + FileDat(1, indmax(1, ind(ch, 2)), ind(1, 1)).ToString(UniDyn.adec(ind(ch, 1)))
                LMaxPull1.Text = LMaxPull1.Text + ", " + gmx(1, 3).ToString(UniDyn.adec(ind(ch, 3))) + " " + UniDyn.auni(ind(1, 3) - 1) + " at " + FileDat(1, indmax(1, ind(ch, 3)), ind(1, 1)).ToString(UniDyn.adec(ind(ch, 1)))
                LMaxPull1.Text = LMaxPull1.Text + "," + ftime

                LAY1.Text = UniDyn.auni(ind(1, 2) - 1)
                LAY2.Text = UniDyn.auni(ind(1, 3) - 1)
                LAYU.Text = UniDyn.auni(ind(1, 4) - 1)               'Label Analog Axis
            Else
                If ch = 2 Then                                      'To avoid loading for CH3, 4
                    LmaxPull2L.Text = ButFile.Text
                    Lmaxpull2.Text = "   Max:" + gmx(ch, 2).ToString(UniDyn.adec(ind(ch, 2))) + " " + UniDyn.auni(ind(ch, 2) - 1) + " at " + FileDat(ch, indmax(ch, ind(ch, 2)), ind(ch, 1)).ToString(UniDyn.adec(ind(ch, 1)))
                    Lmaxpull2.Text = Lmaxpull2.Text + ", " + gmx(ch, 3).ToString(UniDyn.adec(ind(ch, 3))) + " " + UniDyn.auni(ind(2, 3) - 1) + " at " + FileDat(ch, indmax(ch, ind(ch, 3)), ind(ch, 1)).ToString(UniDyn.adec(ind(ch, 1)))
                    Lmaxpull2.Text = Lmaxpull2.Text + "," + ftime
                End If
            End If
        End If



        If UniDyn.GManX = False Then
            xmin = Int(xmin / 10) * 10
            If xmax <= xmin Then xmax = xmin + 1
            GXint = Int((xmax - xmin) / 100 + 1) * 10
            Chart1.ChartAreas(0).AxisX.Minimum = GXint * Int(xmin / GXint)
            Chart1.ChartAreas(0).AxisX.Maximum = GXint * (Int(xmax / GXint) + 1)
            Chart2.ChartAreas(0).AxisX.Minimum = GXint * Int(xmin / GXint)
            Chart2.ChartAreas(0).AxisX.Maximum = GXint * (Int(xmax / GXint) + 1)
   
            Console.WriteLine("Auto Set Graph Interval: " + Str(Gxint))
            Console.WriteLine("Auto Set Graph X Min: " + Str(Chart1.ChartAreas(0).AxisX.Minimum))
            Chart1.ChartAreas(0).AxisX.Interval = GXint                  'Set 10 major axis interval
            Chart2.ChartAreas(0).AxisX.Interval = GXint
        Else

            Chart1.ChartAreas(0).AxisX.Minimum = UniDyn.GrX1 * Int(xmin / UniDyn.GrX1)
            Chart1.ChartAreas(0).AxisX.Maximum = UniDyn.GrX1 * (Int(xmax / UniDyn.GrX1) + 1)
            Chart2.ChartAreas(0).AxisX.Minimum = UniDyn.GrX1 * Int(xmin / UniDyn.GrX1)
            Chart2.ChartAreas(0).AxisX.Maximum = UniDyn.GrX1 * (Int(xmax / UniDyn.GrX1) + 1)
            Chart1.ChartAreas(0).AxisX.Interval = UniDyn.GrX1                  'Set 10 major axis interval
            Chart2.ChartAreas(0).AxisX.Interval = UniDyn.GrX1
            Console.WriteLine("UD GR: " + Str(UniDyn.GrX1) + " xmin: " + Str(xmin))
            Console.WriteLine("MANUAL Set Graph X Min: " + Str(Chart1.ChartAreas(0).AxisX.Minimum))
        End If
        If UniDyn.GmanY = True Then
            Chart1.ChartAreas(0).AxisY.Maximum = UniDyn.GrY2
        Else
            Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
        End If
        If UniDyn.Gman2Y = True Then
            Chart1.ChartAreas(0).AxisY2.Maximum = UniDyn.Gr2Y2
        Else
            Chart1.ChartAreas(0).AxisY2.Maximum = Double.NaN
        End If
        Chart1.ChartAreas(0).RecalculateAxesScale()

        If Chart1.ChartAreas(0).AxisY.Maximum = Chart1.ChartAreas(0).AxisY.Minimum Then Chart1.ChartAreas(0).AxisY.Maximum = 1 + Chart1.ChartAreas(0).AxisY.Minimum
        If Chart1.ChartAreas(0).AxisY2.Maximum = Chart1.ChartAreas(0).AxisY2.Minimum Then Chart1.ChartAreas(0).AxisY2.Maximum = 1 + Chart1.ChartAreas(0).AxisY2.Minimum

        Chart1.ChartAreas(0).AxisY.Interval = (Chart1.ChartAreas(0).AxisY.Maximum - Chart1.ChartAreas(0).AxisY.Minimum) / 10
        Chart1.ChartAreas(0).AxisY2.Interval = (Chart1.ChartAreas(0).AxisY2.Maximum - Chart1.ChartAreas(0).AxisY2.Minimum) / 10
        Console.WriteLine("Set Y1 Int: " + Str(Chart1.ChartAreas(0).AxisY.Interval))
        Console.WriteLine("Set Y2 Int: " + Str(Chart1.ChartAreas(0).AxisY2.Interval))
        'Chart2.ChartAreas(0).AxisY.Interval = (Chart2.ChartAreas(0).AxisY.Maximum - Chart2.ChartAreas(0).AxisY.Minimum) / 3
        'Chart2.ChartAreas(0).AxisY2.Interval = (Chart2.ChartAreas(0).AxisY2.Maximum - Chart2.ChartAreas(0).AxisY2.Minimum) / 3
        Chart2.ChartAreas(0).AxisX.Maximum = Chart1.ChartAreas(0).AxisX.Maximum
        Chart2.ChartAreas(0).AxisX.Minimum = Chart1.ChartAreas(0).AxisX.Minimum
        Console.WriteLine("Done setting axis!")

        If Chart2.Series(8).Points.Count < 1 Then           'Ie. we dont have the ref graf on yet
            Try
                'Chart2.Series(8).Points.AddXY(0.Minimum, UniDyn.AnaRef)
                If Chart2.ChartAreas(0).AxisY.Minimum > UniDyn.AnaRef Or Chart2.ChartAreas(0).AxisY.Maximum < UniDyn.AnaRef Then
                    Chart2.ChartAreas(0).AxisY.Minimum = Double.NaN
                    Chart2.ChartAreas(0).AxisY.Maximum = Double.NaN
                    Chart2.ChartAreas(0).AxisX.Minimum = Double.NaN
                    Chart2.ChartAreas(0).AxisX.Maximum = Double.NaN
                    Chart2.ChartAreas(0).AxisY.Minimum = UniDyn.AnaRef - 1
                    Chart2.ChartAreas(0).AxisY.Maximum = UniDyn.AnaRef + 1
                    Chart2.ChartAreas(0).AxisX.Minimum = 0
                    Chart2.ChartAreas(0).AxisX.Maximum = 1
                End If

                ' If Chart2.ChartAreas(0).AxisY.Maximum < UniDyn.AnaRef Then 
                Chart2.Series(8).Points.AddXY(Chart2.ChartAreas(0).AxisX.Minimum, UniDyn.AnaRef)
                Chart2.Series(8).Points.AddXY(Chart2.ChartAreas(0).AxisX.Maximum, UniDyn.AnaRef)
            Catch ex As Exception
                Beep()
            End Try
        End If
        Console.WriteLine("Done with Analog Ref!")

        Console.WriteLine("Still Done with Analog Ref!")
    End Sub
    Private Sub ButExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BGrafEx.Click
        ready = False
        Me.Hide()
        Console.WriteLine("Click on EXIT")
        ' CB1.Checked = False
    End Sub
    Private Sub Bfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButFile.Click
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
                Call loadpulls(2)
            End If
        End With
    End Sub
    Private Sub loadpulls(ByVal ch As Integer)

        Dim s, sa(20) As String
        Console.WriteLine("SUB: Load Pulls")
        ready = False

        If ch = 1 Then
            GrafSet.LB1.Items.Clear()
            GrafSet.LB2.Items.Clear()
            GrafSet.LB3.Items.Clear()
            GrafSet.LB4.Items.Clear()
            GrafSet.LB15.Items.Clear()
        Else
            GrafSet.LB8.Items.Clear()
            GrafSet.LB7.Items.Clear()
            GrafSet.LB6.Items.Clear()
            GrafSet.LB5.Items.Clear()
            GrafSet.LB19.Items.Clear()
        End If

        For i = 1 To 27
            Datmax(ch, i) = 0 : indmax(ch, i) = 0
        Next

        'Load Index is 4, 8, or 12
        Try
            Using FileRead As New System.IO.StreamReader(calfile)
                s = FileRead.ReadLine()                    'Read Headers Line 1
                s = FileRead.ReadLine()                    'Read Headers Line 2
                sa = s.Split(",")
                sa(1) = System.Text.RegularExpressions.Regex.Replace(sa(1), "[^0-9\.]", "") 'Replace non numberics with "empty"
                frun = Val(sa(1))
                ftime = sa(3)
                Lsec.Text = ftime
                If ch = 1 Then
                    Lrun1.Text = sa(1)
                Else
                    Lrun2.Text = sa(1)
                End If

                For i = 3 To 4
                    s = FileRead.ReadLine()                    'Read Headers
                Next                                            '4th line is Labels
                s = s + ", , , , , , , , , , , , , , , , , , , , ,"       'Pad just in case prev version of pull with fewer columns
                sa = s.Split(",")

                For i = 1 To 27                                 'Read Labels
                    If sa(i - 1) = "" Or sa(i - 1) = " " Then sa(i - 1) = Str(i) + " N/A"
                    filab(ch, i) = sa(i - 1)
                    ' MsgBox("Col " + Str(i) + " Label: " + filab(ch, i))

                    If ch = 1 Then                              'Add labels as used in the individual files
                        GrafSet.LB1.Items.Add(filab(ch, i))
                        GrafSet.LB2.Items.Add(filab(ch, i))
                        GrafSet.LB3.Items.Add(filab(ch, i))
                        GrafSet.LB4.Items.Add(filab(ch, i))
                        GrafSet.LB15.Items.Add(filab(ch, i))
                    Else
                        GrafSet.LB5.Items.Add(filab(ch, i))
                        GrafSet.LB6.Items.Add(filab(ch, i))
                        GrafSet.LB7.Items.Add(filab(ch, i))
                        GrafSet.LB8.Items.Add(filab(ch, i))
                        GrafSet.LB19.Items.Add(filab(ch, i))
                    End If
                    Datmax(ch, i) = 0
                    datmin(ch, i) = 1000000000.0
                Next
                'MsgBox("Done Labels")
                If ch = 1 Then
                    'MsgBox("Inds: " + Str(ind(1, 1) - 1) + " " + Str(ind(1, 2) - 1) + " " + Str(ind(1, 3) - 1) + " " + Str(ind(1, 4) - 1) + " " + Str(ind(1, 5) - 1))
                    GrafSet.LB1.SelectedIndex = ind(1, 1) - 1 : GrafSet.LU1.Text = UniDyn.auni(ind(1, 1) - 1)
                    GrafSet.LB2.SelectedIndex = ind(1, 2) - 1 : GrafSet.LU2.Text = UniDyn.auni(ind(1, 2) - 1)
                    GrafSet.LB3.SelectedIndex = ind(1, 3) - 1 : GrafSet.LU3.Text = UniDyn.auni(ind(1, 3) - 1)
                    GrafSet.LB4.SelectedIndex = ind(1, 4) - 1 : GrafSet.LU4.Text = UniDyn.auni(ind(1, 4) - 1)
                    GrafSet.LB15.SelectedIndex = ind(1, 5) - 1 : GrafSet.LU15.Text = UniDyn.auni(ind(1, 5) - 1)
                Else
                    If ind(ch, 1) = 0 Or ch = 3 Or ch = 4 Then
                        ind(ch, 1) = ind(1, 1) : GrafSet.LU5.Text = UniDyn.auni(ind(1, 1))
                        ind(ch, 2) = ind(1, 2) : GrafSet.LU6.Text = UniDyn.auni(ind(1, 2))
                        ind(ch, 3) = ind(1, 3) : GrafSet.LU7.Text = UniDyn.auni(ind(1, 3))
                        ind(ch, 4) = ind(1, 4) : GrafSet.LU8.Text = UniDyn.auni(ind(1, 4))
                        ind(ch, 5) = ind(1, 5) : GrafSet.LU19.Text = UniDyn.auni(ind(1, 5))
                    End If
                    GrafSet.LB5.SelectedIndex = ind(ch, 1) - 1 : GrafSet.LU5.Text = UniDyn.auni(ind(ch, 1) - 1)
                    GrafSet.LB6.SelectedIndex = ind(ch, 2) - 1 : GrafSet.LU6.Text = UniDyn.auni(ind(ch, 2) - 1)
                    GrafSet.LB7.SelectedIndex = ind(ch, 3) - 1 : GrafSet.LU7.Text = UniDyn.auni(ind(ch, 3) - 1)
                    GrafSet.LB8.SelectedIndex = ind(ch, 4) - 1 : GrafSet.LU8.Text = UniDyn.auni(ind(ch, 4) - 1)
                    GrafSet.LB19.SelectedIndex = ind(ch, 5) - 1 : GrafSet.LU19.Text = UniDyn.auni(ind(ch, 5) - 1)
                End If
                'MsgBox("Done Setups")
                i = 0
                Do While (s <> "END")
                    i = i + 1                       'Line Number

                    s = FileRead.ReadLine()

                    If Len(s) = 0 Then s = "END"
                    If Mid(s, 1, 3) = "END" Then s = "END" 'Just in case XL edited it and added commas
                    If s <> "END" Then
                        s = s + "0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,"       'Pad just in case prev version of pull with fewer columns
                        sa = s.Split(",")
                        'Runs
                        For j = 1 To 27
                            FileDat(ch, i, j) = sa(j - 1)
                            If FileDat(ch, i, j) > Datmax(ch, j) Then Datmax(ch, j) = FileDat(ch, i, j) : indmax(ch, j) = i
                            If FileDat(ch, i, j) < datmin(ch, j) Then datmin(ch, j) = FileDat(ch, i, j)
                        Next
                    End If
                Loop
                i = i - 1
                For i = i + 1 To 2000
                    For j = 1 To 27                 'Erase any old data (This is lazy... I should have a "last point" variable, and only plot to the last point)
                        FileDat(ch, i, j) = 0
                    Next
                Next

                FileRead.Close()            'Gotta use these 2 lines to close the file after reading
                FileRead.Dispose()
                If ch = 1 Or ch = 3 Or ch = 4 Then on1 = True
                If ch = 2 Then on2 = True
            End Using
            ready = True
            plot(ch)
        Catch ex As Exception
            MsgBox("File Read Fail at Channel:" + Str(ch) + "  Line:" + Str(i) + "  Column:" + Str(j) + Chr(13) + s)
            MsgBox(calfile + " Corrupted or missing.")            'Use Defaults and create DynoCal file
            ready = True
        End Try
        If Chart2.Series(8).Points.Count < 1 Then
            Try
                'Chart2.Series(8).Points.AddXY(Chart2.ChartAreas(0).AxisX.Minimum, UniDyn.AnaRef)
                Chart2.Series(8).Points.AddXY(0, UniDyn.AnaRef)
                Chart2.Series(8).Points.AddXY(Chart2.ChartAreas(0).AxisX.Maximum, UniDyn.AnaRef)
            Catch ex As Exception
                Beep()
            End Try

        End If
        'FileClose(1)
        'MsgBox("Line 4:" + Str(pcdat(1, 1, 4)) + Str(pcdat(1, 2, 4)) + Str(pcdat(1, 3, 4)) + Str(pcdat(1, 4, 4)))
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButPrint.Click

        Dim foto As Graphics = Nothing
        Dim BMPW As Integer = Me.Panel1.Width * UniDyn.MAG
        Dim BMPH As Integer = Me.Panel1.Height * UniDyn.MAG
        Dim bmp As New Bitmap(BMPW, BMPH)         '* 0.756
        Dim Sav As New SaveFileDialog
        'Dim scrnsize As Size
        Dim fmloc As System.Drawing.Point = Me.PointToScreen(Me.Panel1.Location)
        foto = Graphics.FromImage(bmp)
        ' Panel2.DrawToBitmap(bmp, Panel2.DisplayRectangle)
        'scrnsize = bmp.Size
        'scrnsize.Width = scrnsize.Width * 0.78
        foto.CopyFromScreen(fmloc.X * UniDyn.MAG, fmloc.Y * UniDyn.MAG, 0, 0, bmp.Size)          '5, 20 adjust from edge of form 1 to edge of pannel2
        Sav.Filter = "JPG Image|*.jpg"
        Sav.Title = ("Save Screen Shot")
        Sav.FileName = LID.Text + ".JPG"
        If Sav.ShowDialog() = DialogResult.OK Then
            If Sav.FileName <> "" Then bmp.Save(Sav.FileName, System.Drawing.Imaging.ImageFormat.Jpeg)

        End If
        'jfile = InputBox("Enter Filename for Screen Shot", DefaultResponse:=LID.Text + ".JPG")
        'bmp = foto.DrawImage


    End Sub
    'Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ind(1, 1) = GrafSet.LB1.SelectedIndex + 1
    '    GrafSet.LU1.Text = UniDyn.auni(ind(1, 1) - 1)
    '    UniDyn.xgraf = ind(1, 1)
    '    Chart1.ChartAreas(0).AxisX.Maximum = Datmax(1, ind(1, 1))
    '    Chart1.ChartAreas(0).AxisX.Minimum = datmin(1, ind(1, 1))
    '    If ready = True Then Call plot(1) 'Just redo whole plot
    'End Sub
    'Private Sub ListBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ind(1, 2) = GrafSet.LB2.SelectedIndex + 1
    '    GrafSet.LU2.Text = UniDyn.auni(ind(1, 2) - 1)
    '    UniDyn.y1graf = ind(1, 2)
    '    If ready = True Then Call plot(1) 'Just redo whole plot
    'End Sub
    'Private Sub ListBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ind(1, 3) = GrafSet.LB3.SelectedIndex + 1
    '    GrafSet.LU3.Text = UniDyn.auni(ind(1, 3) - 1)
    '    UniDyn.y2graf = ind(1, 3)
    '    If ready = True Then Call plot(1) 'Just redo whole plot
    'End Sub
    'Private Sub ListBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ind(1, 4) = GrafSet.LB4.SelectedIndex + 1
    '    GrafSet.LU4.Text = UniDyn.auni(ind(1, 4) - 1)
    '    UniDyn.y3graf = ind(1, 4)

    '    If ready = True Then Call plot(1) 'Just redo whole plot
    'End Sub
    'Private Sub ListBox8_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ind(2, 1) = GrafSet.LB5.SelectedIndex + 1
    '    GrafSet.LU5.Text = UniDyn.auni(ind(2, 1) - 1)
    '    If ready = True Then Call plot(2)
    'End Sub
    'Private Sub ListBox7_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ind(2, 2) = GrafSet.LB6.SelectedIndex + 1
    '    GrafSet.LU6.Text = UniDyn.auni(ind(2, 2) - 1)
    '    If ready = True Then Call plot(2)
    'End Sub
    'Private Sub ListBox6_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ind(2, 3) = LB7.SelectedIndex + 1
    '    LU7.Text = UniDyn.auni(ind(2, 3) - 1)
    '    If ready = True Then Call plot(2)
    'End Sub
    'Private Sub ListBox5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ind(2, 4) = LB8.SelectedIndex + 1
    '    LU8.Text = UniDyn.auni(ind(2, 4) - 1)

    '    If ready = True Then Call plot(2)
    'End Sub
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
    Private Sub Form1_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        'Me.WindowState = FormWindowState.Minimized
        ready = False
        Me.Hide()
        e.Cancel = True
        Console.WriteLine("Click on X")
    End Sub

    Private Sub Chart1_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Chart1.MouseDown
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
        Call Chrt1stats(Nothing)
        'Return()
    End Function
    Public Sub Chrt1stats(ByVal e)
        Dim xpos, min(9), xmindx(9) As Single
        Dim mark1, mark2 As Point
        Dim lx1t, ly11T, lz1t, lw1t, lx2t, ly21t, lz2t, lw2t As String
        Dim lx3t, ly31T, lz3t, lw3t, lx4t, ly41t, lz4t, lw4t As String
        Dim lv1t, lv2t, lv3t, lv4t As String
        Console.WriteLine("SUB: CH1 Stats")
        For j = 1 To 5
            min(j) = 999999999                                                  'Stuff min with a big number for all grafs
        Next
        For i = 1 To 5
            If ind(1, i) = 0 Then ind(1, i) = 1 'Stuff with real # just incase not initialized (was causing crashes)
            If ind(2, i) = 0 Then ind(2, i) = 1
        Next


        Chart1.Refresh()
        xpos = Chart1.ChartAreas(0).AxisX.Minimum + xmark * (Chart1.ChartAreas(0).AxisX.Maximum - Chart1.ChartAreas(0).AxisX.Minimum)

        For i = 1 To 2000
            For j = 1 To 5
                Try
                    If Math.Abs(FileDat(j, i, ind(j, 1)) - xpos) < min(j) Then
                        min(j) = Math.Abs(FileDat(j, i, ind(j, 1)) - xpos)              'Find the closest point to the current X Position
                        xmindx(j) = i
                        'Get the X Index of this point
                    End If
                Catch ex As Exception
                    min(j) = 0
                    xmindx(j) = i
                End Try

            Next
        Next

        For i = 1 To 5
            Console.WriteLine(Str(i) + "   Min:" + Str(min(i)) + "   Xmind:" + Str(xmindx(i)))
        Next

        If Chart1.Series(0).Points.Count > 3 Then
            With Me.Chart1.Series(0)            'Series 0, 1 Trq, Pow , Chart 2 Series 0, 1 is AFR 1, 2 on lower graf
                For CiC = 0 To Chart1.Series(0).Points.Count - 2
                    If FileDat(1, xmindx(1), ind(1, 1)) >= .Points(CiC).XValue And FileDat(1, xmindx(1), ind(1, 1)) < .Points(CiC + 1).XValue Then
                        Try
                            lx1t = .Points(CiC).XValue.ToString(UniDyn.adec(ind(1, 1) - 1))
                            ly11T = .Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 2) - 1))
                            lz1t = Chart1.Series(1).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 3) - 1))
                            lw1t = Chart2.Series(0).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 4) - 1))
                            lv1t = Chart2.Series(1).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 5) - 1))
                        Catch ex As Exception
                            Beep()                          'Clicked outta bounds
                            lx1t = "" : ly11T = "" : lz1t = "" : lw1t = "" : lv1t = ""
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
                            lx2t = Chart1.Series(2).Points(CiC).XValue.ToString(UniDyn.adec(ind(2, 1) - 1))
                            ly21t = Chart1.Series(2).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(2, 2) - 1))
                            lz2t = Chart1.Series(3).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(2, 3) - 1))
                            lw2t = Chart2.Series(2).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(2, 4) - 1))
                            lv2t = Chart2.Series(3).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(2, 5) - 1))
                        Catch ex As Exception
                            Beep()
                            lx2t = "" : ly21t = "" : lz2t = "" : lw2t = "" : lv2t = ""
                        End Try
                    
                    End If
                Next
            End With
        End If

        If Chart1.Series(4).Points.Count > 3 Then
            With Me.Chart1.Series(4)                    'Series 4, 5 Trq, Pow of File #3
                For CiC = 0 To Chart1.Series(4).Points.Count - 2
                    If FileDat(3, xmindx(3), ind(3, 1)) >= .Points(CiC).XValue And FileDat(3, xmindx(3), ind(3, 1)) < .Points(CiC + 1).XValue Then   'Only show if Between the two X values
                        Try
                            lx3t = Chart1.Series(4).Points(CiC).XValue.ToString(UniDyn.adec(ind(1, 1) - 1))                     'Ind is the channel value for each line.. just use same as graf1
                            ly31T = Chart1.Series(4).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 2) - 1))
                            lz3t = Chart1.Series(5).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 3) - 1))
                            lw3t = Chart2.Series(4).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 4) - 1))
                            lv3t = Chart2.Series(5).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 5) - 1))
                        Catch ex As Exception
                            Beep()
                            lx3t = "" : ly31T = "" : lz3t = "" : lw3t = "" : lv3t = ""
                        End Try

                    End If
                Next
            End With
        End If

        If Chart1.Series(6).Points.Count > 3 Then
            With Me.Chart1.Series(6)                    'Series 6, 7 Trq, Pow of File #3
                For CiC = 0 To Chart1.Series(6).Points.Count - 2
                    If FileDat(4, xmindx(4), ind(4, 1)) >= .Points(CiC).XValue And FileDat(4, xmindx(4), ind(4, 1)) < .Points(CiC + 1).XValue Then   'Only show if Between the two X values
                        Try
                            lx4t = Chart1.Series(6).Points(CiC).XValue.ToString(UniDyn.adec(ind(1, 1) - 1))                     'Ind is the channel value for each line.. just use same as graf1
                            ly41t = Chart1.Series(6).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 2) - 1))
                            lz4t = Chart1.Series(7).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 3) - 1))
                            lw4t = Chart2.Series(6).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 4) - 1))
                            lv4t = Chart2.Series(7).Points(CiC).YValues(0).ToString(UniDyn.adec(ind(1, 5) - 1))
                        Catch ex As Exception
                            Beep()
                            lx4t = "" : ly41t = "" : lz4t = "" : lw4t = "" : lv4t = ""
                        End Try

                    End If
                Next
            End With
        End If

        LabX.Text = UniDyn.alab(ind(1, 1) - 1) : LabY1.Text = UniDyn.alab(ind(1, 2) - 1) : Laby2.Text = UniDyn.alab(ind(1, 3) - 1) : LaY3.Text = UniDyn.alab(ind(1, 4) - 1) : LAY4.Text = UniDyn.alab(ind(1, 5) - 1)

        LX1.Text = lx1t : LX2.Text = lx2t : LX3.Text = lx3t : LX4.Text = lx4t              'Update Stats at screen
        Ly11.Text = ly11T : Ly21.Text = ly21t : LY31.Text = ly31T : LY41.Text = ly41t
        Lz1.Text = lz1t : Lz2.Text = lz2t : LZ3.Text = lz3t : LZ4.Text = lz4t
        Lw1.Text = lw1t : Lw2.Text = lw2t : LW3.Text = lw3t : LW4.Text = lw4t
        LV1.Text = lv1t : LV2.Text = lv2t : LV3.Text = lv3t : LV4.Text = lv4t


        If IsNothing(e) Then
            mark1.X = (0.072 + xmark * 0.825) * Chart1.Width + Chart1.Left
        Else
            mark1.X = e.X - Chart1.Left
        End If

        mark2.X = mark1.X
        mark1.Y = Chart1.Top + Chart1.Height * 0.03
        mark2.Y = Chart1.Top + Chart1.Height * 0.86
        Me.Chart1.CreateGraphics.DrawLine(Pens.Red, mark1, mark2)
    End Sub

    Private Sub Report(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButRep.Click

        '8888
        Dim foto As Graphics = Nothing
        Dim BMPW As Integer = Me.Panel1.Width * UniDyn.MAG
        Dim BMPH As Integer = Me.Panel1.Height * UniDyn.MAG
        Dim bmp As New Bitmap(BMPW, BMPH)
        Dim fmloc As System.Drawing.Point = Me.PointToScreen(Me.Panel1.Location)
        Dim ik, ij, repoints As Integer
        Dim xlApp As Object = Nothing
        Try
            xlApp = CreateObject("Excel.Application")
        Catch
        End Try

        foto = Graphics.FromImage(bmp)

        foto.CopyFromScreen(fmloc.X * UniDyn.MAG, fmloc.Y * UniDyn.MAG, 0, 0, bmp.Size)          '5, 20 adjust from edge of form 1 to edge of pannel2
        'foto.CopyFromScreen(fmloc.X, fmloc.Y, 0, 0, bmp.Size)          '5, 20 adjust from edge of form 1 to edge of pannel2
        ' Clipboard.SetDataObject(foto, False)
        Clipboard.SetImage(bmp)
        Try
            Dim excelBook As Object = xlApp.Workbooks.Add
            Dim excelWorksheet As Object = excelBook.Worksheets(1)
            xlApp.Visible = True
            With excelWorksheet
                .Cells.Select()
                .Cells.Delete()
                .Cells(1, 1).value = "Focus Applied Technologies DynoMAS Dynamometer Pull Report"
                .Cells(2, 1).value = UniDyn.LTagline.Text + "   " + LID.Text
                .Range("A3").Select()
                .PasteSpecial(Format:="Bitmap")
                For ik = 1 To 17
                    .Cells(40, ik).Value = filab(1, ik)          'Labels
                Next
                repoints = UniDyn.points
                If repoints = 0 Then
                    For ij = 1 To 2000
                        If FileDat(1, ij, 1) > 1 Then repoints = ij
                    Next
                End If
                For ij = 1 To repoints                         'Rows
                    For ik = 1 To 16
                        .Cells(ij + 40, ik).value = FileDat(1, ij, ik)
                    Next ik
                Next ij
                .Rows("1:1").Font.FontStyle = "Bold"
                .Rows("1:1").Font.Size = 10
                .Cells.Select()
                .Cells(1, 1).Select()
            End With
            excelBook.SaveAs(LID.Text + ".xlsx")

        Catch ex As Exception
            MsgBox("Export Excel Error " & ex.Message)
        Finally
            'RELEASE ALLOACTED RESOURCES
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            xlApp = Nothing
        End Try

    End Sub

  
    Private Sub ButSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButSetup.Click

        If GrafSet.Visible Then
            GrafSet.Hide()
            UniDyn.GraFilt = Val(GrafSet.TBAvg.Text)
            If UniDyn.GraFilt < 0 Then UniDyn.GraFilt = 0
            If UniDyn.GraFilt > 10 Then UniDyn.GraFilt = 10
            ' Console.WriteLine("Set Filt :" + GrafSet.TBAnaTarg.Text)
            'If DynoMonitor.GManX = True Then
            '    DynoMonitor.GRx1 = Val(GrafSet.TBX1.Text)
            '    DynoMonitor.GRx2 = Val(GrafSet.TBX2.Text)
            'End If
        Else
            GrafSet.Show()

            GrafSet.LB5.SelectedIndex = GrafSet.LB1.SelectedIndex
            GrafSet.LB6.SelectedIndex = GrafSet.LB2.SelectedIndex
            GrafSet.LB7.SelectedIndex = GrafSet.LB3.SelectedIndex
            GrafSet.LB8.SelectedIndex = GrafSet.LB4.SelectedIndex
            GrafSet.LB19.SelectedIndex = GrafSet.LB15.SelectedIndex


            'GrafSet.TBAnaT.Text = Str(DynoMonitor.GrAnaTarg)
            'GrafSet.ButBaCol.BackColor = DynoMonitor.ColGrafBak(DynoMonitor.GBaCol)
            'GrafSet.TBLineThk.Text = Str(DynoMonitor.GLineW)
            'If DynoMonitor.GAxSame = True Then
            '    GrafSet.ButSame.Text = "SAME"
            '    GrafSet.LB5.BackColor = Color.LightGray
            '    GrafSet.LB6.BackColor = Color.LightGray
            '    GrafSet.LB7.BackColor = Color.LightGray
            '    GrafSet.LB8.BackColor = Color.LightGray
            'Else
            '    GrafSet.LB5.BackColor = Color.White
            '    GrafSet.LB6.BackColor = Color.White
            '    GrafSet.LB7.BackColor = Color.White
            '    GrafSet.LB8.BackColor = Color.White
            '    GrafSet.ButSame.Text = "DIFFERENT"
            'End If
            GrafSet.TBAvg.Text = Str(UniDyn.GraFilt)
            'If DynoMonitor.grafgrid = True Then
            '    GrafSet.Bgraf.Text = "FINE"
            'Else
            '    GrafSet.Bgraf.Text = "CORSE"
            'End If
            'If DynoMonitor.AnaGrTop = True Then
            '    GrafSet.ButAnaGraf.Text = "COMBINED"
            'Else
            '    GrafSet.ButAnaGraf.Text = "SEPERATE"
            'End If
            'If DynoMonitor.bigsum = True Then
            '    GrafSet.ButBS.Text = "ON"
            'Else
            '    GrafSet.ButBS.Text = "OFF"
            'End If
            'If DynoMonitor.G2Last = True Then
            '    GrafSet.But2Last.Text = "YES"
            'Else
            '    GrafSet.But2Last.Text = "NO"
            'End If
            If UniDyn.GManX = True Then
                GrafSet.TBX1.Text = UniDyn.GrX1
            Else
                GrafSet.TBX1.Text = "AUTO"
            End If
            If UniDyn.GmanY = True Then
                GrafSet.TBY2.Text = UniDyn.GrY2
            Else
                GrafSet.TBY2.Text = "AUTO"
            End If
            If UniDyn.Gman2Y = True Then
                GrafSet.TB2Y2.Text = UniDyn.Gr2Y2
            Else
                GrafSet.TB2Y2.Text = "AUTO"
            End If
            GrafSet.TBAnaRef.Text = Str(UniDyn.AnaRef)
            'Populate Lists and etc. 
        End If

    End Sub
    Private Sub SetGrafParams() Handles GrafSet.SetGraf                     'This resets the axis after a "Setup" call 8888
        ind(1, 1) = GrafSet.LB1.SelectedIndex + 1
        '  Lax1.Text = DynoMonitor.Anaunit(ind(1, 1) - 1)
        UniDyn.xgraf = ind(1, 1)
        ind(1, 2) = GrafSet.LB2.SelectedIndex + 1
        ' LaxY11.Text = DynoMonitor.Anaunit(ind(1, 2) - 1)
        UniDyn.y1graf = ind(1, 2)
        ind(1, 3) = GrafSet.LB3.SelectedIndex + 1
        'LaxY21.Text = DynoMonitor.Anaunit(ind(1, 3) - 1)
        UniDyn.y2graf = ind(1, 3)
        ind(1, 4) = GrafSet.LB4.SelectedIndex + 1
        'LU4.Text = DynoMonitor.Anaunit(ind(1, 4) -1)
        UniDyn.y3graf = ind(1, 4)
        ind(1, 5) = GrafSet.LB15.SelectedIndex + 1
        UniDyn.y4graf = ind(1, 5)
        UniDyn.GraFilt = Val(GrafSet.TBAvg.Text)
        If UniDyn.GraFilt < 0 Then UniDyn.GraFilt = 0
        If UniDyn.GraFilt > 10 Then UniDyn.GraFilt = 10
        UniDyn.AnaRef = Val(GrafSet.TBAnaRef.Text)
        If UniDyn.GManX = True Then
            UniDyn.GrX1 = Val(GrafSet.TBX1.Text)
            '    UniDyn.GRx2 = Val(GrafSet.TBX2.Text)
            '    If UniDyn.GRx1 >= UniDyn.GRx2 Then UniDyn.GRx2 = UniDyn.GRx1 + 1
        End If
        If UniDyn.GmanY = True Then UniDyn.GrY2 = Val(GrafSet.TBY2.Text)
        If UniDyn.Gman2Y = True Then UniDyn.Gr2Y2 = Val(GrafSet.TB2Y2.Text)

        'If DynoMonitor.GLineW <> Val(GrafSet.TBLineThk.Text) Then DynoMonitor.GraFirst = True
        'UniDyn.GLineW = Val(GrafSet.TBLineThk.Text)
        'If UniDyn.GLineW > 5 Then UniDyn.GLineW = 5
        'If UniDyn.GLineW < 1 Then UniDyn.GLineW = 1
        'UniDyn.GrAnaTarg = Val(GrafSet.TBAnaT.Text)
        setcolors()
        GrafSet.Hide()
        ind(4, 1) = ind(1, 1) : ind(5, 1) = ind(1, 1)
        ind(4, 2) = ind(1, 2) : ind(5, 2) = ind(1, 2)
        ind(4, 3) = ind(1, 3) : ind(5, 3) = ind(1, 3)
        ind(4, 4) = ind(1, 4) : ind(5, 4) = ind(1, 4)
        ind(2, 1) = ind(1, 1)
        ind(2, 2) = ind(1, 2)
        ind(2, 3) = ind(1, 3)
        ind(2, 4) = ind(1, 4)
        ind(2, 5) = ind(1, 5)
        'MsgBox("B4")
        If Chart1.Series(0).Points.Count > 3 Then
            If ready = True Then Call plot(1) 'Just redo whole plot
            If ready = True And on2 = True Then Call plot(2)
        End If
        'MsgBox("AFTER")
    End Sub
    Sub setcolors()
        Console.WriteLine("SUB: SetColor")
        'LabX.Text = "-"  
        LabY1.ForeColor = UniDyn.Col(UniDyn.Tcol(1)) ' Stats Labels
        Laby2.ForeColor = UniDyn.Col(UniDyn.Tcol(2))
        LaY3.ForeColor = UniDyn.Col(UniDyn.Tcol(3))
        ' LAY4.ForeColor = UniDyn.Col(UniDyn.Tcol(4))
        'LX1.Text = "-" 
        Ly11.ForeColor = UniDyn.Col(UniDyn.Tcol(1)) 'Graf 1
        Lz1.ForeColor = UniDyn.Col(UniDyn.Tcol(2))
        Lw1.ForeColor = UniDyn.Col(UniDyn.Tcol(3)) ' Graf 1 Analog
        'LV1.ForeColor = UniDyn.Col(UniDyn.Tcol(4))
        'LX2.Text = "-"
        Ly21.ForeColor = UniDyn.Col(UniDyn.Tcol(4)) 'Graf 2
        Lz2.ForeColor = UniDyn.Col(UniDyn.Tcol(5))
        Lw2.ForeColor = UniDyn.Col(UniDyn.Tcol(6)) ' Graf3 Analog

        LY31.ForeColor = UniDyn.Col(UniDyn.Tcol(7))
        LZ3.ForeColor = UniDyn.Col(UniDyn.Tcol(8))
        LY41.ForeColor = UniDyn.Col(UniDyn.Tcol(9))
        LZ4.ForeColor = UniDyn.Col(UniDyn.Tcol(10))

        ButGr3.ForeColor = UniDyn.Col(UniDyn.Tcol(7))
        ButGR4.ForeColor = UniDyn.Col(UniDyn.Tcol(9))
        'Lax1.ForeColor = Color.Black                 'Set up axis Labels
        'LAX13.ForeColor = Color.Black
        'LaxY11.ForeColor = UniDyn.Col(UniDyn.Tcol(1))
        'LaxY21.ForeColor = UniDyn.Col(UniDyn.Tcol(2))
        'Lana1.ForeColor = UniDyn.Col(UniDyn.Tcol(3))
        'Lana12.ForeColor = UniDyn.Col(UniDyn.Tcol(4))

        'LAX2.ForeColor = Color.Black              'Set up axis Labels
        'LAX23.ForeColor = Color.Black
        'LAXY12.ForeColor = UniDyn.Col(UniDyn.Tcol(5))
        'LAXY22.ForeColor = UniDyn.Col(UniDyn.Tcol(6))
        'Lana2.ForeColor = UniDyn.Col(UniDyn.Tcol(7))
        'Lana22.ForeColor = UniDyn.Col(UniDyn.Tcol(8))


        Chart1.Series(0).Color = UniDyn.Col(UniDyn.Tcol(1)) : Chart1.Series(1).Color = UniDyn.Col(UniDyn.Tcol(2)) ' T1, P1
        Chart2.Series(0).Color = UniDyn.Col(UniDyn.Tcol(3)) : Chart2.Series(1).Color = UniDyn.Col(UniDyn.Tcol(11)) ' A1-1, A1-2
        Chart1.Series(2).Color = UniDyn.Col(UniDyn.Tcol(4)) : Chart1.Series(3).Color = UniDyn.Col(UniDyn.Tcol(5)) ' T2, P2
        Chart2.Series(2).Color = UniDyn.Col(UniDyn.Tcol(6)) : Chart2.Series(3).Color = UniDyn.Col(UniDyn.Tcol(12)) ' A2-1, A2-2

        Chart1.Series(4).Color = UniDyn.Col(UniDyn.Tcol(7)) : Chart1.Series(5).Color = UniDyn.Col(UniDyn.Tcol(8))
        Chart1.Series(6).Color = UniDyn.Col(UniDyn.Tcol(9)) : Chart1.Series(7).Color = UniDyn.Col(UniDyn.Tcol(10))
        ' Chart1.Series(4).Color = Color.Navy : Chart1.Series(5).Color = Color.Purple
        'Chart1.Series(6).Color = Color.DodgerBlue : Chart1.Series(7).Color = Color.IndianRed

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButGr3.Click
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
            End If
        End With
    End Sub

    Private Sub ButGR4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButGR4.Click
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
            End If
        End With
    End Sub

    Private Sub Button1_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' Chart1.Series(0).Points.Clear()                         'Clears Graf Chart Series and the Stats + labels
        'Chart1.Series(1).Points.Clear()
        Chart1.Series(2).Points.Clear()
        Chart1.Series(3).Points.Clear()
        Chart1.Series(4).Points.Clear()
        Chart1.Series(5).Points.Clear()
        Chart1.Series(6).Points.Clear()
        Chart1.Series(7).Points.Clear()

        'Chart2.Series(0).Points.Clear()
        Chart2.Series(1).Points.Clear()
        Chart2.Series(2).Points.Clear()
        Chart2.Series(3).Points.Clear()
        'LX1.Text = ""
        'Ly11.Text = ""
        'Lz1.Text = ""
        'Lw1.Text = ""
        LX2.Text = ""
        Ly21.Text = ""
        Lz2.Text = ""
        Lw2.Text = ""
        'LMaxPull1.Text = ""
        'lmaxPull1L.Text = ""
        Lmaxpull2.Text = ""
        LmaxPull2L.Text = ""
        'LabX.Text = ""
        'LaY3.Text = ""
        'LabY1.Text = ""
        'Laby2.Text = ""
        'Ltime.Text = ""
        'Lsec.Text = ""
        'LID.Text = ""
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btprint.Click
        Dim foto As Graphics = Nothing
        Dim BMPW As Integer = Me.Panel1.Width * UniDyn.MAG
        Dim BMPH As Integer = Me.Panel1.Height * UniDyn.MAG
        Dim bmp As New Bitmap(BMPW, BMPH)
        Dim fmloc As System.Drawing.Point = Me.PointToScreen(Me.Panel1.Location)
        foto = Graphics.FromImage(bmp)
        foto.CopyFromScreen(fmloc.X * UniDyn.MAG, fmloc.Y * UniDyn.MAG, 0, 0, bmp.Size)          '5, 20 adjust from edge of form 1 to edge of pannel2
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

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click, btprint.Click

    End Sub
End Class