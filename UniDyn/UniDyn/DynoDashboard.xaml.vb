Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Shapes
Imports System.Windows.Input

''' <summary>
''' DynoDashboard.xaml.vb
''' Dashboard Utama Dyno KZR DYNOMAS (WPF Cyberpunk Edition)
''' Dilengkapi ATPS Speed Controller (0-5V), Tolok Jarum Analog, Carta Kuasa Realtime, dan Replay Playback
''' </summary>
Partial Public Class DynoDashboard
    Inherits Window

    Private _targetRpm As Integer = 7000
    Private _playbackTimer As System.Windows.Threading.DispatcherTimer = Nothing
    Private _csvDataLines As New List(Of String)()
    Private _playbackIndex As Integer = 0
    Private _isPlaying As Boolean = False
    Private _chartMaxHp As Double = 60.0
    Private _chartMaxNm As Double = 50.0

    Public Sub New()
        InitializeComponent()
        AddHandler Loaded, AddressOf DynoDashboard_Loaded
    End Sub

    Private Sub DynoDashboard_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
        BuildRpmDial()
        BuildSpeedDial()
        BuildChartScale()

        AddHandler ChartGrid.SizeChanged, AddressOf ChartGrid_SizeChanged

        If UniDyn.Instance IsNot Nothing Then
            If Not String.IsNullOrWhiteSpace(UniDyn.Instance.LTagline.Text) Then
                TxtBannerTitle.Text = UniDyn.Instance.LTagline.Text
            ElseIf Not String.IsNullOrWhiteSpace(UniDyn.Instance.Cals.Tbtit.Text) Then
                TxtBannerTitle.Text = UniDyn.Instance.Cals.Tbtit.Text
            End If
        End If

        UpdateOnlineStatus()
        UpdateTelemetry(0)
    End Sub

    Private Sub ChartGrid_SizeChanged(ByVal sender As Object, ByVal e As SizeChangedEventArgs)
        BuildChartScale()
    End Sub

    ''' <summary>
    ''' Melukis senggatan dan nombor untuk tolok Tachometer RPM Utama (0 hingga 15,000 RPM)
    ''' </summary>
    Private Sub BuildRpmDial()
        CanvasRpmDial.Children.Clear()

        Dim cx As Double = 200.0
        Dim cy As Double = 200.0
        Dim rTicks As Double = 180.0
        Dim rNums As Double = 148.0
        Dim startAngle As Double = 135.0
        Dim sweepAngle As Double = 270.0
        Dim maxRpm As Integer = 15

        For i As Integer = 0 To maxRpm
            Dim frac As Double = i / maxRpm
            Dim deg As Double = startAngle + (frac * sweepAngle)
            Dim rad As Double = (deg * Math.PI) / 180.0

            Dim isRedline As Boolean = (i >= 10)
            Dim tickBrush As Brush = If(isRedline, New SolidColorBrush(Color.FromRgb(255, 34, 34)), New SolidColorBrush(Color.FromRgb(255, 255, 255)))
            Dim numBrush As Brush = If(isRedline, New SolidColorBrush(Color.FromRgb(255, 34, 34)), New SolidColorBrush(Color.FromRgb(255, 255, 255)))

            Dim tickLen As Double = 14.0
            Dim x1 As Double = cx + ((rTicks - tickLen) * Math.Cos(rad))
            Dim y1 As Double = cy + ((rTicks - tickLen) * Math.Sin(rad))
            Dim x2 As Double = cx + (rTicks * Math.Cos(rad))
            Dim y2 As Double = cy + (rTicks * Math.Sin(rad))

            Dim line As New Line() With {
                .X1 = x1,
                .Y1 = y1,
                .X2 = x2,
                .Y2 = y2,
                .Stroke = tickBrush,
                .StrokeThickness = 2.8
            }
            CanvasRpmDial.Children.Add(line)

            Dim nx As Double = cx + (rNums * Math.Cos(rad))
            Dim ny As Double = cy + (rNums * Math.Sin(rad))

            Dim txt As New TextBlock() With {
                .Text = i.ToString(),
                .Foreground = numBrush,
                .FontSize = 15,
                .FontWeight = FontWeights.Black,
                .FontStyle = FontStyles.Italic,
                .Width = 30,
                .Height = 24,
                .TextAlignment = TextAlignment.Center
            }
            Canvas.SetLeft(txt, nx - 15)
            Canvas.SetTop(txt, ny - 12)
            CanvasRpmDial.Children.Add(txt)

            If i < maxRpm Then
                For subTick As Integer = 1 To 4
                    Dim subFrac As Double = (i + (subTick * 0.2)) / maxRpm
                    Dim subDeg As Double = startAngle + (subFrac * sweepAngle)
                    Dim subRad As Double = (subDeg * Math.PI) / 180.0

                    Dim isSubRed As Boolean = (i >= 10)
                    Dim subBrush As Brush = If(isSubRed, New SolidColorBrush(Color.FromRgb(255, 60, 60)), New SolidColorBrush(Color.FromArgb(180, 0, 240, 255)))
                    Dim subLen As Double = If(subTick = 2, 8.5, 5.0)

                    Dim sx1 As Double = cx + ((rTicks - subLen) * Math.Cos(subRad))
                    Dim sy1 As Double = cy + ((rTicks - subLen) * Math.Sin(subRad))
                    Dim sx2 As Double = cx + (rTicks * Math.Cos(subRad))
                    Dim sy2 As Double = cy + (rTicks * Math.Sin(subRad))

                    Dim sline As New Line() With {
                        .X1 = sx1,
                        .Y1 = sy1,
                        .X2 = sx2,
                        .Y2 = sy2,
                        .Stroke = subBrush,
                        .StrokeThickness = If(subTick = 2, 1.8, 1.2)
                    }
                    CanvasRpmDial.Children.Add(sline)
                Next
            End If
        Next
    End Sub

    ''' <summary>
    ''' Melukis senggatan tolok Speedometer (0 hingga 240 KM/J)
    ''' </summary>
    Private Sub BuildSpeedDial()
        CanvasSpeedDial.Children.Clear()

        Dim cx As Double = 200.0
        Dim cy As Double = 200.0
        Dim rTicks As Double = 180.0
        Dim rNums As Double = 148.0
        Dim startAngle As Double = 135.0
        Dim sweepAngle As Double = 270.0

        ' Senggatan setiap 20 KM/J mengikut spesifikasi mockup rasmi DYNOMAS
        Dim speeds As Integer() = {0, 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240}
        Dim maxSpeed As Double = 240.0

        For Each spd As Integer In speeds
            Dim frac As Double = spd / maxSpeed
            Dim deg As Double = startAngle + (frac * sweepAngle)
            Dim rad As Double = (deg * Math.PI) / 180.0

            Dim isRedline As Boolean = (spd >= 120)
            Dim brush As Brush = If(isRedline, New SolidColorBrush(Color.FromRgb(255, 34, 34)), New SolidColorBrush(Color.FromRgb(255, 255, 255)))

            Dim tickLen As Double = 14.0
            Dim x1 As Double = cx + ((rTicks - tickLen) * Math.Cos(rad))
            Dim y1 As Double = cy + ((rTicks - tickLen) * Math.Sin(rad))
            Dim x2 As Double = cx + (rTicks * Math.Cos(rad))
            Dim y2 As Double = cy + (rTicks * Math.Sin(rad))

            Dim line As New Line() With {
                .X1 = x1,
                .Y1 = y1,
                .X2 = x2,
                .Y2 = y2,
                .Stroke = brush,
                .StrokeThickness = 2.8
            }
            CanvasSpeedDial.Children.Add(line)

            Dim nx As Double = cx + (rNums * Math.Cos(rad))
            Dim ny As Double = cy + (rNums * Math.Sin(rad))

            Dim txt As New TextBlock() With {
                .Text = spd.ToString(),
                .Foreground = brush,
                .FontSize = 13,
                .FontWeight = FontWeights.Black,
                .FontStyle = FontStyles.Italic,
                .Width = 34,
                .Height = 22,
                .TextAlignment = TextAlignment.Center
            }
            Canvas.SetLeft(txt, nx - 17)
            Canvas.SetTop(txt, ny - 11)
            CanvasSpeedDial.Children.Add(txt)

            If spd < 240 Then
                ' Sub-tick pada pertengahan (+10 KM/J)
                Dim subFrac As Double = (spd + 10.0) / maxSpeed
                Dim subDeg As Double = startAngle + (subFrac * sweepAngle)
                Dim subRad As Double = (subDeg * Math.PI) / 180.0
                Dim isSubRed As Boolean = (spd >= 120)
                Dim subBrush As Brush = If(isSubRed, New SolidColorBrush(Color.FromRgb(255, 60, 60)), New SolidColorBrush(Color.FromArgb(180, 0, 240, 255)))

                Dim sx1 As Double = cx + ((rTicks - 7.0) * Math.Cos(subRad))
                Dim sy1 As Double = cy + ((rTicks - 7.0) * Math.Sin(subRad))
                Dim sx2 As Double = cx + (rTicks * Math.Cos(subRad))
                Dim sy2 As Double = cy + (rTicks * Math.Sin(subRad))

                Dim sline As New Line() With {
                    .X1 = sx1,
                    .Y1 = sy1,
                    .X2 = sx2,
                    .Y2 = sy2,
                    .Stroke = subBrush,
                    .StrokeThickness = 1.4
                }
                CanvasSpeedDial.Children.Add(sline)
            End If
        Next
    End Sub

'' <summary>
    ''' Melukis grid dan skala carta Dyno (Horsepower kiri, Torque kanan, RPM bawah)
    ''' </summary>
    Public Sub BuildChartScale()
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

        Dim gridBrush As New SolidColorBrush(Color.FromArgb(50, 0, 240, 255))
        Dim hpBrush As New SolidColorBrush(Color.FromRgb(0, 240, 255))
        Dim torqBrush As New SolidColorBrush(Color.FromRgb(255, 60, 60))
        Dim rpmBrush As New SolidColorBrush(Color.FromRgb(140, 180, 200))

        ' 1. Garisan grid mendatar (Horizontal Grid Lines - 4 langkah = 5 garisan)
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

            ' Nilai Paksi HP (Kiri)
            Dim valHp As Integer = CInt(Math.Round(frac * _chartMaxHp))
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

            ' Nilai Paksi Torque Nm (Kanan)
            Dim valTorq As Integer = CInt(Math.Round(frac * _chartMaxNm))
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

        ' 2. Garisan grid menegak & Skala RPM (0k hingga 15k RPM)
        Dim rpmSteps As Integer() = {0, 2000, 4000, 6000, 8000, 10000, 12000, 14000, 15000}
        For Each r As Integer In rpmSteps
            Dim frac As Double = r / 15000.0
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

            Dim label As String = If(r = 0, "0", (r \ 1000).ToString() & "k")
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
    ''' Fungsi kemas kini pusingan jarum dan nombor digital asas
    ''' </summary>
    Public Sub UpdateTelemetry(ByVal rpm As Double)
        Dim rpmFrac As Double = Math.Min(Math.Max(rpm / 15000.0, 0.0), 1.0)
        Dim rpmAngle As Double = -135.0 + (rpmFrac * 270.0)
        RpmNeedleTransform.Angle = rpmAngle
        TxtDigitalRpm.Text = Math.Round(rpm).ToString()
        TxtTopRpmDyno.Text = Math.Round(rpm).ToString()

        Dim speedKmh As Double = (rpm / 15000.0) * 215.0
        Dim speedFrac As Double = Math.Min(Math.Max(speedKmh / 240.0, 0.0), 1.0)
        Dim speedAngle As Double = -135.0 + (speedFrac * 270.0)
        SpeedNeedleTransform.Angle = speedAngle
        TxtDigitalSpeed.Text = Math.Round(speedKmh).ToString()

        If rpm > 0 Then
            Dim hp As Double = (rpm / 15000.0) * 48.0
            TxtTopHp.Text = Math.Round(hp, 1).ToString("F1")
            TxtTorq.Text = Math.Round(30.0 + (Math.Sin(rpm / 2000.0) * 10.0), 1).ToString("F1")
            TxtTps.Text = Math.Round((rpm / 15000.0) * 100.0).ToString()
            TxtTemp.Text = Math.Round(38.0 + ((rpm / 15000.0) * 15.0)).ToString()
        Else
            TxtTopHp.Text = "0.0"
            TxtTorq.Text = "0.0"
            TxtTps.Text = "0"
            TxtTemp.Text = "25"
        End If
    End Sub

    ' ================= ATPS / SPEED CONTROLLER EVENT HANDLERS =================
    Private Sub SliderAtps_ValueChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Double))
        Dim pct As Double = e.NewValue
        Dim volt As Double = (pct / 100.0) * 5.0
        If TxtAtpsVal IsNot Nothing Then
            TxtAtpsVal.Text = String.Format("{0:0.0}% ({1:0.00}V)", pct, volt)
        End If
        SendAtpsToController(pct)
    End Sub

    Private Sub SliderAtps_MouseUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
        If SliderAtps IsNot Nothing Then
            SendAtpsToController(SliderAtps.Value)
        End If
    End Sub

    Private Sub BtnAtpsZero_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If SliderAtps IsNot Nothing Then
            SliderAtps.Value = 0
        End If
        SendAtpsToController(0)
    End Sub

    Private Sub BtnAtpsPreset_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim btn As Button = TryCast(sender, Button)
        If btn IsNot Nothing AndAlso btn.Tag IsNot Nothing Then
            Dim val As Double
            If Double.TryParse(btn.Tag.ToString(), val) Then
                If SliderAtps IsNot Nothing Then
                    SliderAtps.Value = val
                End If
                SendAtpsToController(val)
            End If
        End If
    End Sub

    Private Sub BtnAtpsMinus_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If SliderAtps IsNot Nothing Then
            SliderAtps.Value = Math.Max(0, SliderAtps.Value - 2.0)
        End If
    End Sub

    Private Sub BtnAtpsPlus_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If SliderAtps IsNot Nothing Then
            SliderAtps.Value = Math.Min(100, SliderAtps.Value + 2.0)
        End If
    End Sub

    Private Sub SendAtpsToController(ByVal pct As Double)
        If UniDyn.Instance IsNot Nothing Then
            UniDyn.Instance.SendAtpsOutput(pct)
        End If
    End Sub

    ' ================= ON-LINE COM STATUS =================
    Private Sub BtnOnline_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If UniDyn.Instance IsNot Nothing Then
            If Not UniDyn.Instance.CheckComConnected() Then
                UniDyn.Instance.ReconnectComPort()
            End If
        End If
        UpdateOnlineStatus()
    End Sub

    ' ================= FULLSCREEN & KEYBOARD SHORTCUTS =================
    Private Sub BtnFullscreen_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ToggleFullscreen()
    End Sub

    Private Sub DynoDashboard_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.Key = Key.F11 Then
            ToggleFullscreen()
            e.Handled = True
        ElseIf e.Key = Key.Escape AndAlso Me.WindowStyle = WindowStyle.None Then
            ' Keluar dari mod Fullscreen Arcade
            Me.WindowStyle = WindowStyle.SingleBorderWindow
            Me.WindowState = WindowState.Normal
            e.Handled = True
        End If
    End Sub

    ' ================= SETTINGS & CLEAN EXIT =================
    ' ================= MODERN CALIBRATION MODAL HANDLERS =================
    Private _channelsInitialized As Boolean = False
    Private _isSwitchingChannel As Boolean = False
    Private _currentChannelIndex As Integer = -1

    Private Sub BtnSetting_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ShowCalibrationModal()
    End Sub

    Private Sub ShowCalibrationModal()
        If UniDyn.Instance IsNot Nothing Then
            UniDyn.Instance.PrepareCalsData()

            ' 1. Tab 1: Shop & Vehicle
            TxtCalTitle.Text = UniDyn.Instance.Cals.Tbtit.Text
            TxtCalShopName.Text = UniDyn.Instance.Cals.TBShop.Text
            TxtCalContact.Text = UniDyn.Instance.Cals.TBCont.Text
            TxtCalGearRatio.Text = UniDyn.Instance.Cals.TBgr.Text
            TxtCalMag.Text = UniDyn.Instance.Cals.TBMag.Text
            TxtCalKm.Text = UniDyn.Instance.Cals.Lkm.Text

            ' 2. Tab 2: Dyno Roller & Inertia
            TxtCalRollerDiam.Text = UniDyn.Instance.Cals.TBDiam.Text
            TxtCalDynoInert.Text = UniDyn.Instance.Cals.TBInert.Text
            TxtCalEngInert.Text = UniDyn.Instance.Cals.TBEngInert.Text
            TxtCalEquivInert.Text = UniDyn.Instance.Cals.LEngInertEquiv.Text
            TxtCalDynoType.Text = UniDyn.Instance.Cals.TBType.Text
            TxtCalMaxRpm.Text = UniDyn.Instance.Cals.TBMaxrpm.Text
            TxtCalMaxTorq.Text = UniDyn.Instance.Cals.TBMaxTorq.Text
            TxtCalAcelCtrl.Text = UniDyn.Instance.Cals.TBAC.Text
            TxtCalFrictOfs.Text = UniDyn.Instance.Cals.TBFofs.Text
            TxtCalFrictFact.Text = UniDyn.Instance.Cals.TBFfact.Text
            TxtCalFrictQuad.Text = UniDyn.Instance.Cals.TBFquad.Text

            ' 3. Tab 3: SAE Weather
            ChkUseSae.IsChecked = UniDyn.Instance.Cals.CBSAEComp.Checked
            TxtSaeCf.Text = UniDyn.Instance.Cals.LSAECF.Text
            TxtSaePress.Text = UniDyn.Instance.Cals.TBSAEP.Text
            TxtSaeTemp.Text = UniDyn.Instance.Cals.TBSAET.Text
            TxtSaeRh.Text = UniDyn.Instance.Cals.TBSAER.Text
            ChkSaeAuto.IsChecked = UniDyn.Instance.Cals.CBSAEAuto.Checked

            ' 4. Tab 4: Braking & Sensor Channels
            ChkRemoteBrake.IsChecked = UniDyn.Instance.Cals.CBRemote.Checked
            TxtRemoteChannel.Text = UniDyn.Instance.Cals.TBRch.Text
            RadBrakeLoad.IsChecked = UniDyn.Instance.Cals.RBLoad.Checked
            RadBrake2Point.IsChecked = UniDyn.Instance.Cals.RB2PS.Checked
            TxtBrakeLoadVal.Text = UniDyn.Instance.Cals.TBBLoad.Text
            ChkRampDown.IsChecked = UniDyn.Instance.Cals.CBRamp.Checked
            ChkCutoffStart.IsChecked = UniDyn.Instance.Cals.CBStart.Checked

            ' Channel ComboBox Initialization
            _isSwitchingChannel = True
            If Not _channelsInitialized Then
                CmbChannels.Items.Clear()
                For Each itm In UniDyn.Instance.Cals.ComboBox1.Items
                    CmbChannels.Items.Add(itm.ToString())
                Next
                _channelsInitialized = True
            End If

            Dim selIdx As Integer = Math.Max(0, UniDyn.Instance.Cals.ComboBox1.SelectedIndex)
            CmbChannels.SelectedIndex = selIdx
            _currentChannelIndex = selIdx
            LoadCurrentChannelInputs()
            _isSwitchingChannel = False
        End If

        ModalCalOverlay.Visibility = Visibility.Visible
    End Sub

    Private Sub BtnCloseModal_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ModalCalOverlay.Visibility = Visibility.Collapsed
    End Sub

    Private Sub BtnCancelCal_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ModalCalOverlay.Visibility = Visibility.Collapsed
    End Sub

    Private Sub BtnSaveCal_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If UniDyn.Instance IsNot Nothing Then
            ' Write Tab 1: Shop & Vehicle
            UniDyn.Instance.Cals.Tbtit.Text = TxtCalTitle.Text
            UniDyn.Instance.Cals.TBShop.Text = TxtCalShopName.Text
            UniDyn.Instance.Cals.TBCont.Text = TxtCalContact.Text
            UniDyn.Instance.Cals.TBgr.Text = TxtCalGearRatio.Text
            UniDyn.Instance.Cals.TBMag.Text = TxtCalMag.Text

            ' Write Tab 2: Roller & Inertia
            UniDyn.Instance.Cals.TBDiam.Text = TxtCalRollerDiam.Text
            UniDyn.Instance.Cals.TBInert.Text = TxtCalDynoInert.Text
            UniDyn.Instance.Cals.TBEngInert.Text = TxtCalEngInert.Text
            UniDyn.Instance.Cals.TBType.Text = TxtCalDynoType.Text
            UniDyn.Instance.Cals.TBMaxrpm.Text = TxtCalMaxRpm.Text
            UniDyn.Instance.Cals.TBMaxTorq.Text = TxtCalMaxTorq.Text
            UniDyn.Instance.Cals.TBAC.Text = TxtCalAcelCtrl.Text
            UniDyn.Instance.Cals.TBFofs.Text = TxtCalFrictOfs.Text
            UniDyn.Instance.Cals.TBFfact.Text = TxtCalFrictFact.Text
            UniDyn.Instance.Cals.TBFquad.Text = TxtCalFrictQuad.Text

            ' Write Tab 3: SAE Weather
            UniDyn.Instance.Cals.CBSAEComp.Checked = (ChkUseSae.IsChecked = True)
            UniDyn.Instance.Cals.TBSAEP.Text = TxtSaePress.Text
            UniDyn.Instance.Cals.TBSAET.Text = TxtSaeTemp.Text
            UniDyn.Instance.Cals.TBSAER.Text = TxtSaeRh.Text
            UniDyn.Instance.Cals.CBSAEAuto.Checked = (ChkSaeAuto.IsChecked = True)

            ' Write Tab 4: Braking
            UniDyn.Instance.Cals.CBRemote.Checked = (ChkRemoteBrake.IsChecked = True)
            UniDyn.Instance.Cals.TBRch.Text = TxtRemoteChannel.Text
            UniDyn.Instance.Cals.RBLoad.Checked = (RadBrakeLoad.IsChecked = True)
            UniDyn.Instance.Cals.RB2PS.Checked = (RadBrake2Point.IsChecked = True)
            UniDyn.Instance.Cals.TBBLoad.Text = TxtBrakeLoadVal.Text
            UniDyn.Instance.Cals.CBRamp.Checked = (ChkRampDown.IsChecked = True)
            UniDyn.Instance.Cals.CBStart.Checked = (ChkCutoffStart.IsChecked = True)

            ' Write Active Channel Inputs
            If CmbChannels.SelectedIndex >= 0 Then
                UniDyn.Instance.Cals.ComboBox1.SelectedIndex = CmbChannels.SelectedIndex
            End If
            SaveCurrentChannelInputs()

            ' Apply & Save calibrations in UniDyn
            UniDyn.Instance.ApplyCalsFromWpf()

            ' Update Dashboard shop banner
            TxtBannerTitle.Text = If(String.IsNullOrWhiteSpace(TxtCalTitle.Text), "YOUR NAME HERE", TxtCalTitle.Text)
        End If

        ModalCalOverlay.Visibility = Visibility.Collapsed
    End Sub

    Private Sub CmbChannels_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
        If _channelsInitialized AndAlso Not _isSwitchingChannel AndAlso UniDyn.Instance IsNot Nothing AndAlso CmbChannels.SelectedIndex >= 0 Then
            _isSwitchingChannel = True
            SaveCurrentChannelInputs()
            _currentChannelIndex = CmbChannels.SelectedIndex
            UniDyn.Instance.Cals.ComboBox1.SelectedIndex = CmbChannels.SelectedIndex
            LoadCurrentChannelInputs()
            _isSwitchingChannel = False
        End If
    End Sub

    Private Sub LoadCurrentChannelInputs()
        If UniDyn.Instance IsNot Nothing Then
            TxtChName.Text = UniDyn.Instance.Cals.tNAM.Text
            TxtChUnits.Text = UniDyn.Instance.Cals.tUNI.Text
            TxtChOffset.Text = UniDyn.Instance.Cals.tOF.Text
            TxtChFactor.Text = UniDyn.Instance.Cals.tFAC.Text
            TxtChMin.Text = UniDyn.Instance.Cals.tMIN.Text
            TxtChMax.Text = UniDyn.Instance.Cals.tMAX.Text
            TxtChA1.Text = UniDyn.Instance.Cals.TA1.Text
            TxtChA2.Text = UniDyn.Instance.Cals.TA2.Text
            TxtChFilter.Text = UniDyn.Instance.Cals.tFLT.Text
            TxtChMajorTicks.Text = UniDyn.Instance.Cals.tMAJ.Text
            TxtChMinorTicks.Text = UniDyn.Instance.Cals.tMNR.Text
            TxtChDecimals.Text = UniDyn.Instance.Cals.tDEC.Text
        End If
    End Sub

    Private Sub SaveCurrentChannelInputs()
        If UniDyn.Instance IsNot Nothing Then
            UniDyn.Instance.Cals.tNAM.Text = TxtChName.Text
            UniDyn.Instance.Cals.tUNI.Text = TxtChUnits.Text
            UniDyn.Instance.Cals.tOF.Text = TxtChOffset.Text
            UniDyn.Instance.Cals.tFAC.Text = TxtChFactor.Text
            UniDyn.Instance.Cals.tMIN.Text = TxtChMin.Text
            UniDyn.Instance.Cals.tMAX.Text = TxtChMax.Text
            UniDyn.Instance.Cals.TA1.Text = TxtChA1.Text
            UniDyn.Instance.Cals.TA2.Text = TxtChA2.Text
            UniDyn.Instance.Cals.tFLT.Text = TxtChFilter.Text
            UniDyn.Instance.Cals.tMAJ.Text = TxtChMajorTicks.Text
            UniDyn.Instance.Cals.tMNR.Text = TxtChMinorTicks.Text
            UniDyn.Instance.Cals.tDEC.Text = TxtChDecimals.Text
        End If
    End Sub

    Private Sub TxtCalEngInert_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
        UpdateEquivInertia()
    End Sub

    Private Sub TxtCalGearRatio_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
        UpdateEquivInertia()
    End Sub

    Private Sub UpdateEquivInertia()
        If TxtCalEngInert Is Nothing OrElse TxtCalGearRatio Is Nothing OrElse TxtCalEquivInert Is Nothing Then Return
        Dim engInert As Double = 0.0
        Dim gr As Double = 0.0
        Double.TryParse(TxtCalEngInert.Text, engInert)
        Double.TryParse(TxtCalGearRatio.Text, gr)
        Dim equiv As Double = engInert * (gr ^ 2)
        TxtCalEquivInert.Text = (Math.Round(equiv * 1000.0) / 1000.0).ToString("F3")
    End Sub

    Private Sub TxtSae_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
        RecalculateSaeCf()
    End Sub

    Private Sub ChkUseSae_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        RecalculateSaeCf()
    End Sub

    Private Sub RecalculateSaeCf()
        If TxtSaePress Is Nothing OrElse TxtSaeTemp Is Nothing OrElse TxtSaeCf Is Nothing OrElse ChkUseSae Is Nothing Then Return
        If ChkUseSae.IsChecked <> True Then
            TxtSaeCf.Text = "100%"
            Return
        End If

        Dim p As Double = 0.0
        Dim t As Double = 0.0
        Double.TryParse(TxtSaePress.Text, p)
        Double.TryParse(TxtSaeTemp.Text, t)

        If p >= 95.0 AndAlso p <= 105.0 AndAlso t >= 15.0 AndAlso t <= 35.0 Then
            Dim cf As Double = 1.176 * ((99.0 / p) * ((t + 273.0) / 298.0) ^ 0.5) - 0.176
            TxtSaeCf.Text = (cf * 100.0).ToString("N1") & "%"
        Else
            TxtSaeCf.Text = "100%"
        End If
    End Sub

    Private Sub BtnAutoFrictWpf_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If UniDyn.Instance IsNot Nothing Then
            UniDyn.Instance.Z_AutoFrict()
            TxtCalFrictOfs.Text = UniDyn.Instance.Cals.TBFofs.Text
            TxtCalFrictFact.Text = UniDyn.Instance.Cals.TBFfact.Text
            TxtCalFrictQuad.Text = UniDyn.Instance.Cals.TBFquad.Text

            If UniDyn.Instance.FACon Then
                ModalCalOverlay.Visibility = Visibility.Collapsed
            End If
        End If
    End Sub

    Private Sub BtnContCalsWpf_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If UniDyn.Instance IsNot Nothing Then
            UniDyn.Instance.Cals.Button3.PerformClick()
            Try
                If UniDyn.Instance.contcals IsNot Nothing Then
                    UniDyn.Instance.contcals.TopMost = True
                    UniDyn.Instance.contcals.BringToFront()
                End If
            Catch
            End Try
        End If
    End Sub

    Private Sub BtnSkinColor_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If UniDyn.Instance IsNot Nothing Then
            UniDyn.Instance.Cals.Button5.PerformClick()
        End If
    End Sub

    Private Sub BtnBgColor_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If UniDyn.Instance IsNot Nothing Then
            UniDyn.Instance.Cals.Button6.PerformClick()
        End If
    End Sub

    Protected Overrides Sub OnClosed(ByVal e As EventArgs)
        MyBase.OnClosed(e)
        Try
            If UniDyn.Instance IsNot Nothing Then
                UniDyn.Instance.Close()
            End If
        Catch
        End Try
        Environment.Exit(0)
    End Sub

    Private Sub ToggleFullscreen()
        If Me.WindowStyle = WindowStyle.None Then
            Me.WindowStyle = WindowStyle.SingleBorderWindow
            Me.WindowState = WindowState.Maximized
        Else
            Me.WindowStyle = WindowStyle.None
            Me.WindowState = WindowState.Maximized
        End If
    End Sub

    Public Sub UpdateOnlineStatus()
        If BtnOnline Is Nothing Then Return
        Dim isOnline As Boolean = False
        If UniDyn.Instance IsNot Nothing Then
            isOnline = UniDyn.Instance.CheckComConnected()
        End If
        If isOnline Then
            BtnOnline.Content = "ONLINE (COM4)"
            BtnOnline.Background = New SolidColorBrush(Color.FromRgb(10, 40, 25))
            BtnOnline.Foreground = New SolidColorBrush(Color.FromRgb(0, 255, 136))
            BtnOnline.BorderBrush = New SolidColorBrush(Color.FromRgb(0, 255, 136))
        Else
            BtnOnline.Content = "OFFLINE"
            BtnOnline.Background = New SolidColorBrush(Color.FromRgb(35, 10, 10))
            BtnOnline.Foreground = New SolidColorBrush(Color.FromRgb(255, 70, 70))
            BtnOnline.BorderBrush = New SolidColorBrush(Color.FromRgb(255, 70, 70))
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
    Private Sub BtnTare_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        TxtTorq.Text = "0.0"
        TxtTopHp.Text = "0.0"
        TxtLoad.Text = "0"
        If PolylineHp IsNot Nothing Then PolylineHp.Points.Clear()
        If PolylineTorque IsNot Nothing Then PolylineTorque.Points.Clear()
        If UniDyn.Instance IsNot Nothing Then
            UniDyn.Instance.TriggerTare()
        End If
        BtnTare.Content = "TARED ✓"
        Dim t As New System.Threading.Timer(Sub()
                                                Dispatcher.Invoke(Sub() BtnTare.Content = "TARE ⚖")
                                            End Sub, Nothing, 1200, System.Threading.Timeout.Infinite)
    End Sub

    ' Fan Toggle Button
    Private Sub BtnFan_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If BtnFan.Content.ToString().Contains("ON") Then
            BtnFan.Content = "FAN 🌀"
            BtnFan.Background = New SolidColorBrush(Color.FromRgb(5, 8, 12))
        Else
            BtnFan.Content = "FAN ON 🌀"
            BtnFan.Background = New SolidColorBrush(Color.FromRgb(0, 255, 136))
            BtnFan.Foreground = New SolidColorBrush(Color.FromRgb(0, 0, 0))
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
                TxtAfr1.Foreground = New SolidColorBrush(Color.FromRgb(255, 40, 40))
            ElseIf afr1 >= 12.2 AndAlso afr1 <= 13.2 Then
                TxtAfr1.Foreground = New SolidColorBrush(Color.FromRgb(0, 255, 136))
            Else
                TxtAfr1.Foreground = New SolidColorBrush(Color.FromRgb(0, 240, 255))
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
        Try
            If Not IO.File.Exists(filePath) Then
                MessageBox.Show("Fail CSV tidak dijumpai: " & filePath, "Ralat", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return
            End If

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

            ' Imbas bacaan tertinggi untuk skala paksi automatik
            Dim maxHpFound As Double = 0
            Dim maxTorqFound As Double = 0
            Dim ci As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture
            For Each l As String In _csvDataLines
                Dim parts As String() = l.Split(","c)
                If parts.Length > 8 Then
                    Dim hVal As Double = 0
                    Dim tVal As Double = 0
                    Double.TryParse(parts(8).Trim(), System.Globalization.NumberStyles.Any, ci, hVal)
                    Double.TryParse(parts(6).Trim(), System.Globalization.NumberStyles.Any, ci, tVal)
                    If hVal > maxHpFound Then maxHpFound = hVal
                    If tVal > maxTorqFound Then maxTorqFound = tVal
                End If
            Next

            If maxHpFound > 120 Then
                _chartMaxHp = 180.0
            ElseIf maxHpFound > 60 Then
                _chartMaxHp = 120.0
            Else
                _chartMaxHp = 60.0
            End If

            If maxTorqFound > 100 Then
                _chartMaxNm = 150.0
            ElseIf maxTorqFound > 50 Then
                _chartMaxNm = 100.0
            Else
                _chartMaxNm = 50.0
            End If

            BuildChartScale()

            ' Kosongkan garisan graf terdahulu dan reset rekod bacaan tertinggi
            PolylineHp.Points.Clear()
            PolylineTorque.Points.Clear()
            TxtMaxHp.Text = "0.0"
            TxtMaxNm.Text = "0.0"

            _playbackIndex = 0
            _isPlaying = True
            BtnRunFile.Content = "⏹ STOP"

            If _playbackTimer Is Nothing Then
                _playbackTimer = New System.Windows.Threading.DispatcherTimer()
                _playbackTimer.Interval = TimeSpan.FromMilliseconds(50)
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

            ' Pemetaan Lajur CSV DYNOMAS:
            ' cols(0): Time
            ' cols(1): Dyno RPM
            ' cols(2): Clamp RPM
            ' cols(3): Engine RPM
            ' cols(6): Engine Torque (ENG TORK)
            ' cols(8): Horsepower (HP)
            ' cols(9): Load
            ' cols(10): AFR 1
            ' cols(11): AFR 2
            ' cols(12): TPS
            ' cols(13): Temp
            ' cols(15): Speed (KM/H)

            If cols.Length > 3 Then Double.TryParse(cols(3).Trim(), System.Globalization.NumberStyles.Any, ci, rpm)
            If rpm <= 0 AndAlso cols.Length > 2 Then Double.TryParse(cols(2).Trim(), System.Globalization.NumberStyles.Any, ci, rpm)
            If rpm <= 0 AndAlso cols.Length > 1 Then Double.TryParse(cols(1).Trim(), System.Globalization.NumberStyles.Any, ci, rpm)

            If cols.Length > 6 Then Double.TryParse(cols(6).Trim(), System.Globalization.NumberStyles.Any, ci, torq)
            If cols.Length > 8 Then Double.TryParse(cols(8).Trim(), System.Globalization.NumberStyles.Any, ci, hp)
            If cols.Length > 9 Then Double.TryParse(cols(9).Trim(), System.Globalization.NumberStyles.Any, ci, load)
            If cols.Length > 10 Then Double.TryParse(cols(10).Trim(), System.Globalization.NumberStyles.Any, ci, afr1)
            If cols.Length > 11 Then Double.TryParse(cols(11).Trim(), System.Globalization.NumberStyles.Any, ci, afr2)
            If cols.Length > 12 Then Double.TryParse(cols(12).Trim(), System.Globalization.NumberStyles.Any, ci, tps)
            If cols.Length > 13 Then Double.TryParse(cols(13).Trim(), System.Globalization.NumberStyles.Any, ci, temp)
            If cols.Length > 15 Then Double.TryParse(cols(15).Trim(), System.Globalization.NumberStyles.Any, ci, speedKmh)

            UpdateFromRealDyno(rpm, speedKmh, hp, torq, load, tps, temp, afr1, afr2)

            ' Lukis titik pada kotak graf
            Dim w As Double = If(ChartGrid.ActualWidth > 50, ChartGrid.ActualWidth, 620.0)
            Dim h As Double = If(ChartGrid.ActualHeight > 50, ChartGrid.ActualHeight, 220.0)

            Dim leftMargin As Double = 36.0
            Dim rightMargin As Double = 36.0
            Dim topMargin As Double = 24.0
            Dim bottomMargin As Double = 22.0

            Dim plotW As Double = w - leftMargin - rightMargin
            Dim plotH As Double = h - topMargin - bottomMargin

            ' Hanya lukis titik bacaan pull yang sah (RPM >= 500 dan HP > 0 atau Torque > 0)
            If rpm >= 500 AndAlso rpm <= 15500 AndAlso (hp > 0 OrElse torq > 0) Then
                Dim rpmNorm As Double = Math.Max(0.0, Math.Min(1.0, rpm / 15000.0))
                Dim xPos As Double = leftMargin + (rpmNorm * plotW)

                Dim hpNorm As Double = Math.Max(0.0, Math.Min(1.0, hp / _chartMaxHp))
                Dim yHpPos As Double = (h - bottomMargin) - (hpNorm * plotH)

                Dim torqNorm As Double = Math.Max(0.0, Math.Min(1.0, torq / _chartMaxNm))
                Dim yTorqPos As Double = (h - bottomMargin) - (torqNorm * plotH)

                PolylineHp.Points.Add(New Windows.Point(xPos, yHpPos))
                PolylineTorque.Points.Add(New Windows.Point(xPos, yTorqPos))
            End If
        End If
    End Sub

End Class
