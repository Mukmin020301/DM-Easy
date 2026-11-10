Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Effects
Imports System.Windows.Shapes

Public Class DynoDashboard
    Inherits Window

        Private _targetRpm As Integer = 7000
        Private _playbackTimer As System.Windows.Threading.DispatcherTimer
        Private _csvDataLines As New System.Collections.Generic.List(Of String)
        Private _playbackIndex As Integer = 0
        Private _isPlaying As Boolean = False
        Private _fanState As Boolean = False

        Public Sub New()
            ' This call is required by the designer.
            InitializeComponent()

            ' Draw Dial scales and initialize needles
            AddHandler Loaded, AddressOf DynoDashboard_Loaded
        End Sub

        Private Sub DynoDashboard_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            BuildRpmDial()
            BuildSpeedDial()
            BuildChartScale()

            ' Update dynamic chart grid on window resize
            AddHandler ChartGrid.SizeChanged, AddressOf ChartGrid_SizeChanged

            ' Set initial value
            UpdateTelemetry(7546)
        End Sub

        Private Sub ChartGrid_SizeChanged(ByVal sender As Object, ByVal e As SizeChangedEventArgs)
            BuildChartScale()
        End Sub

        ''' <summary>
        ''' Melukis senggatan dan nombor untuk tolok Tachometer RPM Utama (0 hingga 15,000 RPM)
        ''' </summary>
        Private Sub BuildRpmDial()
            CanvasRpmDial.Children.Clear()

            Dim cx As Double = 165
            Dim cy As Double = 165
            Dim rTicks As Double = 140
            Dim rNums As Double = 112
            Dim startAngle As Double = 135  ' Sudut permulaan (pukul 7:30)
            Dim sweepAngle As Double = 270  ' Jumlah pusingan 270 darjah
            Dim maxRpm As Integer = 15

            ' Lukis senggatan 0 hingga 15
            For i As Integer = 0 To maxRpm
                Dim frac As Double = i / maxRpm
                Dim deg As Double = startAngle + (frac * sweepAngle)
                Dim rad As Double = (deg * Math.PI) / 180.0

                Dim isRedline As Boolean = (i >= 10)
                Dim tickColor As Brush = If(isRedline, New SolidColorBrush(Color.FromRgb(255, 30, 39)), Brushes.White)

                ' Garisan senggatan utama (Major Tick)
                Dim x1 As Double = cx + (rTicks * Math.Cos(rad))
                Dim y1 As Double = cy + (rTicks * Math.Sin(rad))
                Dim x2 As Double = cx + ((rTicks - 11) * Math.Cos(rad))
                Dim y2 As Double = cy + ((rTicks - 11) * Math.Sin(rad))

                Dim line As New Line() With {
                    .X1 = x1,
                    .Y1 = y1,
                    .X2 = x2,
                    .Y2 = y2,
                    .Stroke = tickColor,
                    .StrokeThickness = If(i Mod 2 = 0, 3.5, 2.4)
                }
                CanvasRpmDial.Children.Add(line)

                ' Senggatan kecil (Minor Subdivisions)
                If i < maxRpm Then
                    For m As Integer = 1 To 4
                        Dim subFrac As Double = (i + (m * 0.2)) / maxRpm
                        Dim subDeg As Double = startAngle + (subFrac * sweepAngle)
                        Dim subRad As Double = (subDeg * Math.PI) / 180.0

                        Dim sx1 As Double = cx + (rTicks * Math.Cos(subRad))
                        Dim sy1 As Double = cy + (rTicks * Math.Sin(subRad))
                        Dim sx2 As Double = cx + ((rTicks - 6) * Math.Cos(subRad))
                        Dim sy2 As Double = cy + ((rTicks - 6) * Math.Sin(subRad))

                        Dim subLine As New Line() With {
                            .X1 = sx1,
                            .Y1 = sy1,
                            .X2 = sx2,
                            .Y2 = sy2,
                            .Stroke = If(isRedline, New SolidColorBrush(Color.FromRgb(255, 42, 42)), New SolidColorBrush(Color.FromArgb(180, 0, 240, 255))),
                            .StrokeThickness = 1.2
                        }
                        CanvasRpmDial.Children.Add(subLine)
                    Next
                End If

                ' Nombor Skala (0, 1, 2 ... 15)
                Dim nx As Double = cx + (rNums * Math.Cos(rad)) - 10
                Dim ny As Double = cy + (rNums * Math.Sin(rad)) - 10

                Dim txt As New TextBlock() With {
                    .Text = i.ToString(),
                    .Foreground = tickColor,
                    .FontWeight = FontWeights.Black,
                    .FontSize = 14,
                    .Width = 20,
                    .TextAlignment = TextAlignment.Center
                }
                Canvas.SetLeft(txt, nx)
                Canvas.SetTop(txt, ny)
                CanvasRpmDial.Children.Add(txt)
            Next
        End Sub

        ''' <summary>
        ''' Melukis tolok Speedometer Kompak (0 hingga 240 KM/H)
        ''' </summary>
        Private Sub BuildSpeedDial()
            CanvasSpeedDial.Children.Clear()

            Dim cx As Double = 120
            Dim cy As Double = 120
            Dim rTicks As Double = 100
            Dim rNums As Double = 80
            Dim startAngle As Double = 135
            Dim sweepAngle As Double = 270
            Dim maxSpeed As Integer = 240
            Dim stepVal As Integer = 30 ' 0, 30, 60, 90, 120, 150, 180, 210, 240

            For s As Integer = 0 To maxSpeed Step stepVal
                Dim frac As Double = s / CDbl(maxSpeed)
                Dim deg As Double = startAngle + (frac * sweepAngle)
                Dim rad As Double = (deg * Math.PI) / 180.0

                Dim isRedline As Boolean = (s >= 180)
                Dim tickColor As Brush = If(isRedline, New SolidColorBrush(Color.FromRgb(255, 30, 39)), Brushes.White)

                ' Garisan senggatan utama
                Dim x1 As Double = cx + (rTicks * Math.Cos(rad))
                Dim y1 As Double = cy + (rTicks * Math.Sin(rad))
                Dim x2 As Double = cx + ((rTicks - 9) * Math.Cos(rad))
                Dim y2 As Double = cy + ((rTicks - 9) * Math.Sin(rad))

                Dim line As New Line() With {
                    .X1 = x1,
                    .Y1 = y1,
                    .X2 = x2,
                    .Y2 = y2,
                    .Stroke = tickColor,
                    .StrokeThickness = 2.8
                }
                CanvasSpeedDial.Children.Add(line)

                ' Senggatan kecil (10 & 20 KM/H antara senggatan utama)
                If s < maxSpeed Then
                    For m As Integer = 1 To 2
                        Dim subFrac As Double = (s + (m * 10)) / CDbl(maxSpeed)
                        Dim subDeg As Double = startAngle + (subFrac * sweepAngle)
                        Dim subRad As Double = (subDeg * Math.PI) / 180.0

                        Dim sx1 As Double = cx + (rTicks * Math.Cos(subRad))
                        Dim sy1 As Double = cy + (rTicks * Math.Sin(subRad))
                        Dim sx2 As Double = cx + ((rTicks - 5) * Math.Cos(subRad))
                        Dim sy2 As Double = cy + ((rTicks - 5) * Math.Sin(subRad))

                        Dim subLine As New Line() With {
                            .X1 = sx1,
                            .Y1 = sy1,
                            .X2 = sx2,
                            .Y2 = sy2,
                            .Stroke = If(isRedline, New SolidColorBrush(Color.FromRgb(255, 42, 42)), New SolidColorBrush(Color.FromArgb(180, 0, 240, 255))),
                            .StrokeThickness = 1.2
                        }
                        CanvasSpeedDial.Children.Add(subLine)
                    Next
                End If

                ' Nombor Skala
                Dim nx As Double = cx + (rNums * Math.Cos(rad)) - 14
                Dim ny As Double = cy + (rNums * Math.Sin(rad)) - 8

                Dim txt As New TextBlock() With {
                    .Text = s.ToString(),
                    .Foreground = tickColor,
                    .FontWeight = FontWeights.Bold,
                    .FontSize = 10,
                    .Width = 28,
                    .TextAlignment = TextAlignment.Center
                }
                Canvas.SetLeft(txt, nx)
                Canvas.SetTop(txt, ny)
                CanvasSpeedDial.Children.Add(txt)
            Next
        End Sub

        ''' <summary>
        ''' Melukis grid profesional & senggatan paksi (HP kiri, Torque kanan, RPM bawah) pada kotak graf dyno
        ''' </summary>
        Private Sub BuildChartScale()
            If CanvasChartScale Is Nothing Then Return
            CanvasChartScale.Children.Clear()

            Dim w As Double = If(ChartGrid.ActualWidth > 50, ChartGrid.ActualWidth, 620.0)
            Dim h As Double = If(ChartGrid.ActualHeight > 50, ChartGrid.ActualHeight, 220.0)

            Dim leftMargin As Double = 36.0
            Dim rightMargin As Double = 36.0
            Dim topMargin As Double = 24.0
            Dim bottomMargin As Double = 22.0

            Dim plotW As Double = w - leftMargin - rightMargin
            Dim plotH As Double = h - topMargin - bottomMargin
            If plotW <= 0 OrElse plotH <= 0 Then Return

            Dim gridBrush As New SolidColorBrush(Color.FromRgb(14, 34, 44))
            Dim hpBrush As New SolidColorBrush(Color.FromRgb(0, 240, 255))
            Dim torqBrush As New SolidColorBrush(Color.FromRgb(255, 60, 60))
            Dim rpmBrush As New SolidColorBrush(Color.FromRgb(100, 160, 180))

            ' 1. Garisan grid mendatar & Skala HP / Torque (4 langkah: 0, 25%, 50%, 75%, 100%)
            For stepIdx As Integer = 0 To 4
                Dim frac As Double = stepIdx / 4.0
                Dim y As Double = (h - bottomMargin) - (frac * plotH)

                Dim gLine As New Line() With {
                    .X1 = leftMargin,
                    .Y1 = y,
                    .X2 = w - rightMargin,
                    .Y2 = y,
                    .Stroke = gridBrush,
                    .StrokeThickness = 1.0
                }
                CanvasChartScale.Children.Add(gLine)

                ' Nilai Paksi HP (Kiri) - 0 hingga 60 HP
                Dim valHp As Integer = CInt(frac * 60)
                Dim txtHp As New TextBlock() With {
                    .Text = valHp.ToString(),
                    .Foreground = hpBrush,
                    .FontSize = 9.5,
                    .FontWeight = FontWeights.Bold,
                    .Width = 30,
                    .TextAlignment = TextAlignment.Right
                }
                Canvas.SetLeft(txtHp, leftMargin - 33)
                Canvas.SetTop(txtHp, y - 7)
                CanvasChartScale.Children.Add(txtHp)

                ' Nilai Paksi Torque Nm (Kanan) - 0 hingga 50 Nm
                Dim valTorq As Integer = CInt(frac * 50)
                Dim txtTorq As New TextBlock() With {
                    .Text = valTorq.ToString(),
                    .Foreground = torqBrush,
                    .FontSize = 9.5,
                    .FontWeight = FontWeights.Bold,
                    .Width = 30,
                    .TextAlignment = TextAlignment.Left
                }
                Canvas.SetLeft(txtTorq, w - rightMargin + 5)
                Canvas.SetTop(txtTorq, y - 7)
                CanvasChartScale.Children.Add(txtTorq)
            Next

            ' 2. Garisan grid menegak & Skala RPM (2k hingga 14k RPM, selang 2,000 RPM)
            Dim rpmSteps As Integer() = {2000, 4000, 6000, 8000, 10000, 12000, 14000}
            For Each r As Integer In rpmSteps
                Dim frac As Double = (r - 2000.0) / (14000.0 - 2000.0)
                Dim x As Double = leftMargin + (frac * plotW)

                Dim gLine As New Line() With {
                    .X1 = x,
                    .Y1 = topMargin,
                    .X2 = x,
                    .Y2 = h - bottomMargin,
                    .Stroke = gridBrush,
                    .StrokeThickness = 1.0
                }
                CanvasChartScale.Children.Add(gLine)

                ' Label RPM di bawah
                Dim label As String = (r \ 1000).ToString() & "k"
                Dim txtRpm As New TextBlock() With {
                    .Text = label,
                    .Foreground = rpmBrush,
                    .FontSize = 9.5,
                    .FontWeight = FontWeights.SemiBold,
                    .Width = 30,
                    .TextAlignment = TextAlignment.Center
                }
                Canvas.SetLeft(txtRpm, x - 15)
                Canvas.SetTop(txtRpm, h - bottomMargin + 3)
                CanvasChartScale.Children.Add(txtRpm)
            Next
        End Sub

        ''' <summary>
        ''' Fungsi kemas kini pusingan jarum dan nombor digital (Menggunakan kiraan sudut yang tepat)
        ''' </summary>
        Public Sub UpdateTelemetry(ByVal rpm As Double)
            ' 1. Sudut jarum Tachometer (0 RPM = -135 darjah, 15000 RPM = +135 darjah)
            Dim rpmFrac As Double = Math.Min(Math.Max(rpm / 15000.0, 0.0), 1.0)
            Dim rpmAngle As Double = -135.0 + (rpmFrac * 270.0)
            RpmNeedleTransform.Angle = rpmAngle
            TxtDigitalRpm.Text = Math.Round(rpm).ToString()

            ' 2. Sudut jarum Speedometer sepadan
            Dim speedKmh As Double = (rpm / 15000.0) * 215.0
            Dim speedFrac As Double = Math.Min(Math.Max(speedKmh / 240.0, 0.0), 1.0)
            Dim speedAngle As Double = -135.0 + (speedFrac * 270.0)
            SpeedNeedleTransform.Angle = speedAngle
            TxtDigitalSpeed.Text = Math.Round(speedKmh).ToString()

            ' 3. Telemetri asas
            Dim hp As Double = (rpm / 15000.0) * 48.0
            TxtTopHp.Text = Math.Round(hp, 1).ToString("F1")
            TxtTorq.Text = Math.Round(30.0 + (Math.Sin(rpm / 2000.0) * 10.0), 1).ToString("F1")
            TxtTps.Text = Math.Round((rpm / 15000.0) * 100.0).ToString()
            TxtTemp.Text = Math.Round(38.0 + ((rpm / 15000.0) * 15.0)).ToString()
        End Sub

        ' Event Slider Simulasi
        Private Sub SliderThrottle_ValueChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Double))
            If TxtSliderVal IsNot Nothing Then
                TxtSliderVal.Text = Math.Round(e.NewValue).ToString() & " RPM"
                UpdateTelemetry(e.NewValue)
            End If
        End Sub

        ' Target RPM Steppers
        Private Sub BtnRpmMinus_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            _targetRpm = Math.Max(1000, _targetRpm - 100)
            TxtTargetRpm.Text = _targetRpm.ToString()
        End Sub

        Private Sub BtnRpmPlus_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            _targetRpm = Math.Min(15000, _targetRpm + 100)
            TxtTargetRpm.Text = _targetRpm.ToString()
        End Sub

        ' Quick TARE button
        Private Sub BtnTare_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles BtnTare.Click
            TxtTorq.Text = "0.0"
            TxtTopHp.Text = "0.0"
            TxtLoad.Text = "0"
            BtnTare.Content = "TARED ✓"
            Dim t As New Threading.Timer(Sub()
                                             Dispatcher.Invoke(Sub() BtnTare.Content = "TARE ⚖")
                                         End Sub, Nothing, 1200, Threading.Timeout.Infinite)
        End Sub

        ' Fan Toggle Button
        Private Sub BtnFan_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles BtnFan.Click
            _fanState = Not _fanState
            If _fanState Then
                BtnFan.Content = "FAN: ON"
                BtnFan.Background = New SolidColorBrush(Color.FromRgb(0, 80, 40))
            Else
                BtnFan.Content = "FAN 🌀"
                BtnFan.Background = New SolidColorBrush(Color.FromRgb(0, 31, 14))
            End If
        End Sub

        ''' <summary>
        ''' Menerima data sebenar dari kabel USB/Serial mesin Dyno (daripada Sub main di UniDyn.vb)
        ''' </summary>
        Public Sub UpdateFromRealDyno(ByVal rpm As Double, ByVal speedKmh As Double, ByVal hp As Double, ByVal torq As Double, ByVal loadPct As Double, Optional ByVal tps As Double = 0, Optional ByVal temp As Double = 0, Optional ByVal afr1 As Double = 0, Optional ByVal afr2 As Double = 0)
            ' 1. Needle RPM & Digital Readout
            Dim rpmFrac As Double = Math.Min(Math.Max(rpm / 15000.0, 0.0), 1.0)
            Dim rpmAngle As Double = -135.0 + (rpmFrac * 270.0)
            RpmNeedleTransform.Angle = rpmAngle
            TxtDigitalRpm.Text = Math.Round(rpm).ToString()
            TxtTopRpmDyno.Text = Math.Round(rpm).ToString()

            ' 2. Needle Speedometer & Digital Readout
            Dim speedFrac As Double = Math.Min(Math.Max(speedKmh / 240.0, 0.0), 1.0)
            Dim speedAngle As Double = -135.0 + (speedFrac * 270.0)
            SpeedNeedleTransform.Angle = speedAngle
            TxtDigitalSpeed.Text = Math.Round(speedKmh).ToString()

            ' 3. Telemetry HP, Torque, Load
            TxtTopHp.Text = Math.Round(hp, 1).ToString("F1")
            TxtTorq.Text = Math.Round(torq, 1).ToString("F1")
            TxtLoad.Text = Math.Round(loadPct).ToString()

            ' Max hold update
            Dim curMaxHp As Double = 0
            Double.TryParse(TxtMaxHp.Text, curMaxHp)
            If hp > curMaxHp Then TxtMaxHp.Text = Math.Round(hp, 1).ToString("F1")

            Dim curMaxNm As Double = 0
            Double.TryParse(TxtMaxNm.Text, curMaxNm)
            If torq > curMaxNm Then TxtMaxNm.Text = Math.Round(torq, 1).ToString("F1")

            ' Aux Sensors
            If tps > 0 Then TxtTps.Text = Math.Round(tps).ToString()
            If temp > 0 Then TxtTemp.Text = Math.Round(temp).ToString()

            ' Dynamic AFR Warning
            If afr1 > 0 AndAlso TxtAfr1 IsNot Nothing Then
                TxtAfr1.Text = Math.Round(afr1, 1).ToString("F1")
                If afr1 > 13.5 Then
                    TxtAfr1.Foreground = New SolidColorBrush(Color.FromRgb(255, 40, 40)) ' Lean warning!
                ElseIf afr1 >= 12.2 AndAlso afr1 <= 13.2 Then
                    TxtAfr1.Foreground = New SolidColorBrush(Color.FromRgb(0, 255, 136)) ' Safe power green
                Else
                    TxtAfr1.Foreground = New SolidColorBrush(Color.FromRgb(0, 240, 255)) ' Rich cyan
                End If
            End If

            If afr2 > 0 AndAlso TxtAfr2 IsNot Nothing Then
                TxtAfr2.Text = Math.Round(afr2, 1).ToString("F1")
                If afr2 > 13.5 Then
                    TxtAfr2.Foreground = New SolidColorBrush(Color.FromRgb(255, 40, 40))
                ElseIf afr2 >= 12.2 AndAlso afr2 <= 13.2 Then
                    TxtAfr2.Foreground = New SolidColorBrush(Color.FromRgb(0, 255, 136))
                Else
                    TxtAfr2.Foreground = New SolidColorBrush(Color.FromRgb(0, 240, 255))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Butang RUN FILE / RUN PULL: Memilih fail run CSV dan memainkan simulasi tarikan Dyno secara langsung (Playback Replay)
        ''' </summary>
        Private Sub BtnRunFile_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If _isPlaying Then
                StopPlayback()
                Return
            End If

            Dim targetPath As String = TxtFilePath.Text
            Dim ofd As New Microsoft.Win32.OpenFileDialog()
            ofd.Filter = "Dyno Run CSV (*.csv)|*.csv|All Files (*.*)|*.*"
            ofd.Title = "Pilih Fail Ujian Dyno (CSV)"

            If IO.File.Exists(targetPath) Then
                ofd.InitialDirectory = IO.Path.GetDirectoryName(targetPath)
                ofd.FileName = IO.Path.GetFileName(targetPath)
            ElseIf IO.Directory.Exists("C:\Dynomas") Then
                ofd.InitialDirectory = "C:\Dynomas"
            End If

            If ofd.ShowDialog() = True Then
                TxtFilePath.Text = ofd.FileName
                StartPlayback(ofd.FileName)
            End If
        End Sub

        ''' <summary>
        ''' Memulakan proses pembacaan baris demi baris CSV dengan selang masa 50ms (20Hz)
        ''' </summary>
        Public Sub StartPlayback(ByVal filePath As String)
            If Not IO.File.Exists(filePath) Then
                MessageBox.Show("Fail tidak ditemui: " & filePath, "Ralat", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return
            End If

            Try
                Dim lines As String() = IO.File.ReadAllLines(filePath)
                _csvDataLines.Clear()

                Dim headerFound As Boolean = False
                For Each line As String In lines
                    Dim trimmed As String = line.Trim()
                    If String.IsNullOrWhiteSpace(trimmed) Then Continue For

                    If Not headerFound Then
                        If trimmed.StartsWith("Time,", StringComparison.OrdinalIgnoreCase) Then
                            headerFound = True
                        End If
                    Else
                        _csvDataLines.Add(trimmed)
                    End If
                Next

                If _csvDataLines.Count = 0 Then
                    MessageBox.Show("Tiada baris data ditemui dalam fail CSV ini.", "Makluman", MessageBoxButton.OK, MessageBoxImage.Information)
                    Return
                End If

                ' Kosongkan garisan graf terdahulu dan reset rekod bacaan tertinggi
                PolylineHp.Points.Clear()
                PolylineTorque.Points.Clear()
                TxtMaxHp.Text = "0"
                TxtMaxNm.Text = "0"

                _playbackIndex = 0
                _isPlaying = True
                BtnRunFile.Content = "⏹ STOP"

                If _playbackTimer Is Nothing Then
                    _playbackTimer = New System.Windows.Threading.DispatcherTimer()
                    _playbackTimer.Interval = TimeSpan.FromMilliseconds(50) ' 20 Hz = 50ms
                    AddHandler _playbackTimer.Tick, AddressOf PlaybackTimer_Tick
                End If

                _playbackTimer.Start()
            Catch ex As Exception
                MessageBox.Show("Ralat membuka fail: " & ex.Message, "Ralat", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

        Public Sub StopPlayback()
            If _playbackTimer IsNot Nothing Then
                _playbackTimer.Stop()
            End If
            _isPlaying = False
            BtnRunFile.Content = "▶ RUN PULL"
        End Sub

        Private Sub PlaybackTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
            If _playbackIndex >= _csvDataLines.Count Then
                StopPlayback()
                Return
            End If

            Dim line As String = _csvDataLines(_playbackIndex)
            _playbackIndex += 1

            Dim cols As String() = line.Split(","c)
            If cols.Length >= 7 Then
                Dim ci As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture
                Dim rpm As Double = 0
                Dim torq As Double = 0
                Dim hp As Double = 0
                Dim load As Double = 0
                Dim speedKmh As Double = 0
                Dim tps As Double = 100
                Dim temp As Double = 40
                Dim afr1 As Double = 12.8
                Dim afr2 As Double = 12.8

                ' Time[0], Speed Dyno[1], Speed Engine[2], Speed Eng Calc[3], Raw Torq[4], Total Torq[5], Engine Torq[6], Pow KW[7], Pow hp[8], Load[9], AFR 1[10], AFR 2[11], TPS[12], Temp[13], Dist[14], Speed KMH[15]
                Double.TryParse(cols(2).Trim(), System.Globalization.NumberStyles.Any, ci, rpm)
                If rpm <= 0 AndAlso cols.Length > 1 Then
                    Double.TryParse(cols(1).Trim(), System.Globalization.NumberStyles.Any, ci, rpm)
                End If

                If cols.Length > 6 Then Double.TryParse(cols(6).Trim(), System.Globalization.NumberStyles.Any, ci, torq)
                If cols.Length > 8 Then Double.TryParse(cols(8).Trim(), System.Globalization.NumberStyles.Any, ci, hp)
                If cols.Length > 9 Then Double.TryParse(cols(9).Trim(), System.Globalization.NumberStyles.Any, ci, load)
                If cols.Length > 10 Then Double.TryParse(cols(10).Trim(), System.Globalization.NumberStyles.Any, ci, afr1)
                If cols.Length > 11 Then Double.TryParse(cols(11).Trim(), System.Globalization.NumberStyles.Any, ci, afr2)
                If cols.Length > 12 Then Double.TryParse(cols(12).Trim(), System.Globalization.NumberStyles.Any, ci, tps)
                If cols.Length > 13 Then Double.TryParse(cols(13).Trim(), System.Globalization.NumberStyles.Any, ci, temp)
                If cols.Length > 15 Then Double.TryParse(cols(15).Trim(), System.Globalization.NumberStyles.Any, ci, speedKmh)

                UpdateFromRealDyno(rpm, speedKmh, hp, torq, load, tps, temp, afr1, afr2)

                ' Lukis titik masa nyata pada kotak graf (Expanded Dyno Chart)
                Dim w As Double = If(ChartGrid.ActualWidth > 50, ChartGrid.ActualWidth, 620.0)
                Dim h As Double = If(ChartGrid.ActualHeight > 50, ChartGrid.ActualHeight, 220.0)

                Dim leftMargin As Double = 36.0
                Dim rightMargin As Double = 36.0
                Dim topMargin As Double = 24.0
                Dim bottomMargin As Double = 22.0

                Dim plotW As Double = w - leftMargin - rightMargin
                Dim plotH As Double = h - topMargin - bottomMargin

                ' X: 2,000 to 14,000 RPM
                Dim minRpm As Double = 2000.0
                Dim maxRpm As Double = 14000.0
                Dim rpmNorm As Double = Math.Max(0.0, Math.Min(1.0, (rpm - minRpm) / (maxRpm - minRpm)))
                Dim xPos As Double = leftMargin + (rpmNorm * plotW)

                ' Y HP: 0 to 60 HP
                Dim maxChartHp As Double = 60.0
                Dim hpNorm As Double = Math.Max(0.0, Math.Min(1.0, hp / maxChartHp))
                Dim yHpPos As Double = (h - bottomMargin) - (hpNorm * plotH)

                ' Y Torque: 0 to 50 Nm
                Dim maxChartNm As Double = 50.0
                Dim torqNorm As Double = Math.Max(0.0, Math.Min(1.0, torq / maxChartNm))
                Dim yTorqPos As Double = (h - bottomMargin) - (torqNorm * plotH)

                PolylineHp.Points.Add(New Windows.Point(xPos, yHpPos))
                PolylineTorque.Points.Add(New Windows.Point(xPos, yTorqPos))
            End If
        End Sub

End Class