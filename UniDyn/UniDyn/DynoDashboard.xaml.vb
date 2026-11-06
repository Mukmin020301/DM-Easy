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

        Public Sub New()
            ' This call is required by the designer.
            InitializeComponent()

            ' Draw Dial scales and initialize needles
            AddHandler Loaded, AddressOf DynoDashboard_Loaded
        End Sub

        Private Sub DynoDashboard_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            BuildRpmDial()
            BuildSpeedDial()

            ' Set initial value
            UpdateTelemetry(7546)
        End Sub

        ''' <summary>
        ''' Melukis senggatan dan nombor untuk tolok RPM (0 hingga 15)
        ''' </summary>
        Private Sub BuildRpmDial()
            CanvasRpmDial.Children.Clear()

            Dim cx As Double = 175
            Dim cy As Double = 175
            Dim rTicks As Double = 148
            Dim rNums As Double = 118
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
                    .StrokeThickness = If(i Mod 2 = 0, 3.5, 2.5)
                }
                CanvasRpmDial.Children.Add(line)

                ' Senggatan kecil (Minor Subdivisions)
                If i < maxRpm Then
                    For m As Integer = 1 To 4
                        Dim subFrac As Double = (i + (m * 0.2)) / maxRpm
                        Dim subDeg As Double = startAngle + (subFrac * sweepAngle)
                        Dim subRad As Double = (subDeg * Math.PI) / 180.0

                        Dim sx1 As Double = cx + (rTicks * Math.Cos(subRad))
                        Dim sy1 As Double = cy + (rTicks * Math.sin(subRad))
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
        ''' Melukis senggatan dan nombor untuk tolok Speedometer (0 hingga 240 KM/H)
        ''' </summary>
        Private Sub BuildSpeedDial()
            CanvasSpeedDial.Children.Clear()

            Dim cx As Double = 175
            Dim cy As Double = 175
            Dim rTicks As Double = 148
            Dim rNums As Double = 118
            Dim startAngle As Double = 135
            Dim sweepAngle As Double = 270
            Dim maxSpeed As Integer = 240
            Dim stepVal As Integer = 20

            For s As Integer = 0 To maxSpeed Step stepVal
                Dim frac As Double = s / CDbl(maxSpeed)
                Dim deg As Double = startAngle + (frac * sweepAngle)
                Dim rad As Double = (deg * Math.PI) / 180.0

                Dim isRedline As Boolean = (s >= 180)
                Dim tickColor As Brush = If(isRedline, New SolidColorBrush(Color.FromRgb(255, 30, 39)), Brushes.White)

                ' Garisan senggatan utama
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
                    .StrokeThickness = 3.2
                }
                CanvasSpeedDial.Children.Add(line)

                ' Senggatan kecil
                If s < maxSpeed Then
                    Dim subFrac As Double = (s + 10) / CDbl(maxSpeed)
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
                        .StrokeThickness = 1.5
                    }
                    CanvasSpeedDial.Children.Add(subLine)
                End If

                ' Nombor Skala (0, 20, 40 ... 240)
                Dim nx As Double = cx + (rNums * Math.Cos(rad)) - 14
                Dim ny As Double = cy + (rNums * Math.Sin(rad)) - 9

                Dim txt As New TextBlock() With {
                    .Text = s.ToString(),
                    .Foreground = tickColor,
                    .FontWeight = FontWeights.Black,
                    .FontSize = 12,
                    .Width = 28,
                    .TextAlignment = TextAlignment.Center
                }
                Canvas.SetLeft(txt, nx)
                Canvas.SetTop(txt, ny)
                CanvasSpeedDial.Children.Add(txt)
            Next
        End Sub

        ''' <summary>
        ''' Fungsi kemas kini pusingan jarum dan nombor digital (Menggunakan kiraan sudut yang tepat)
        ''' </summary>
        Public Sub UpdateTelemetry(ByVal rpm As Double)
            ' 1. Kira sudut pusingan Jarum RPM (0 RPM = -135 darjah, 15000 RPM = +135 darjah)
            Dim rpmFrac As Double = Math.Min(Math.Max(rpm / 15000.0, 0.0), 1.0)
            Dim rpmAngle As Double = -135.0 + (rpmFrac * 270.0)
            RpmNeedleTransform.Angle = rpmAngle
            TxtDigitalRpm.Text = Math.Round(rpm).ToString()

            ' 2. Kira kelajuan sepadan untuk Speedometer
            Dim speedKmh As Double = (rpm / 15000.0) * 215.0
            Dim speedFrac As Double = Math.Min(Math.Max(speedKmh / 240.0, 0.0), 1.0)
            Dim speedAngle As Double = -135.0 + (speedFrac * 270.0)
            SpeedNeedleTransform.Angle = speedAngle
            TxtDigitalSpeed.Text = Math.Round(speedKmh).ToString()

            ' 3. Kemas kini data kotak telemetri
            Dim hp As Double = (rpm / 15000.0) * 48.0
            TxtTopHp.Text = Math.Round(hp).ToString()
            TxtTorq.Text = Math.Round(30.0 + (Math.Sin(rpm / 2000.0) * 10.0)).ToString()
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

            ' Sensors
            If tps > 0 Then TxtTps.Text = Math.Round(tps).ToString()
            If temp > 0 Then TxtTemp.Text = Math.Round(temp).ToString()
            If afr1 > 0 AndAlso TxtAfr1 IsNot Nothing Then TxtAfr1.Text = Math.Round(afr1, 1).ToString("F1")
            If afr2 > 0 AndAlso TxtAfr2 IsNot Nothing Then TxtAfr2.Text = Math.Round(afr2, 1).ToString("F1")
        End Sub

        ''' <summary>
        ''' Butang RUN FILE: Memilih fail run CSV dan memainkan simulasi tarikan Dyno secara langsung (Playback Replay)
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
                BtnRunFile.Content = "STOP"

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
            BtnRunFile.Content = "RUN FILE"
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

                ' Susunan lajur fail CSV DynoMas:
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

                ' Lukis titik masa nyata pada kotak graf (Live Dyno Graph Plotting)
                Dim chartW As Double = If(ChartGrid.ActualWidth > 50, ChartGrid.ActualWidth, 420.0)
                Dim chartH As Double = If(ChartGrid.ActualHeight > 50, ChartGrid.ActualHeight, 140.0)

                Dim minRpm As Double = 2000.0
                Dim maxRpm As Double = 14000.0
                Dim rpmNorm As Double = Math.Max(0.0, Math.Min(1.0, (rpm - minRpm) / (maxRpm - minRpm)))
                Dim xPos As Double = 10.0 + (rpmNorm * (chartW - 20.0))

                Dim maxChartHp As Double = 60.0
                Dim hpNorm As Double = Math.Max(0.0, Math.Min(1.0, hp / maxChartHp))
                Dim yHpPos As Double = (chartH - 12.0) - (hpNorm * (chartH - 24.0))

                Dim maxChartNm As Double = 50.0
                Dim torqNorm As Double = Math.Max(0.0, Math.Min(1.0, torq / maxChartNm))
                Dim yTorqPos As Double = (chartH - 12.0) - (torqNorm * (chartH - 24.0))

                PolylineHp.Points.Add(New Windows.Point(xPos, yHpPos))
                PolylineTorque.Points.Add(New Windows.Point(xPos, yTorqPos))
            End If
        End Sub

    End Class
