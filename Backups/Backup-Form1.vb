Imports System                      'Dyno Monitor V-Basic for FAT Gen5 Dyno Controllers
Imports System.IO.Ports             'These "Imports" are just non-required subroutines required by this program
'Imports System.IO.FileInfo
Imports System.Threading            'Goto View "Show Code" if you can't find this. 
'Imports System.Threading.Timer      'Time based control
Imports System.Threading.Thread     'Some times gotta run twice as it gives a bogus error on 1st run (No idea why).
Imports System.Drawing.Graphics
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Drawing

'REVISION 4         STILL TO CHANGE: ==============================================================
'
'Allow setting of com port from front screen                Added List of Comports to Failure Menue
'Resetting of Rang Max of gages                             DONE Based on T/RPM curve 
'Prevent overflow of scrolling graphs by purging old data   DONE
'Convert X-Axis on scrolling graphs to Time                 DONE
'Read from Fuel Scale                                       DONE
' Give Seperate Drive Cycle Window for Drive Cycle Runs     DONE
' Read from Emissions Analyzer                              
'Prettier "gages"
'Eliminate partial DynoCal file induced crash

'Module globalvariables
'  Public sendCMD As String
'End Module
'Public Module module1
'Friend comport As New SerialPort
'End Module
Public Class DynoMonitor

    '  Private WithEvents DrvCycle As New Form

    Dim WithEvents COMport As New SerialPort
    Dim WithEvents FSport As New SerialPort
    Dim RXtxt As String
    Dim x As Integer

    Dim Rxfs As String
    Dim SaveFile As String
    Dim CalFile As String
    Dim s As String             'Following are the Cal File variables (if used) delcared in order of appearance in the file
    Dim dummy As String
    Dim drivecyc As String
    Dim ver As String
    Dim mass As Double
    Dim diam As Double
    Dim ppr As Integer
    Dim rpmpv As Double
    Dim mpspv As Double
    Dim torqoff As Double
    Dim torqfact As Double
    Dim frictoff As Double
    Dim frictfact As Double
    Dim lastcal As String
    Dim dynotype As String
    Dim rollerdiam As Double
    Dim inert As Double
    Dim compt As Integer    'Com Port Value 
    Dim comstr As String    'Dyno Com Port in String form
    Dim fscompt As Integer  'Fuel Scale Com Port
    Dim EmisCompt As Integer 'Emissions Analyzer Com Port
    Dim analabel(4) As String   'Analog Input Labels
    Dim anafact(4) As Double    'Analog input factors
    Dim anaofs(4) As Double     'Analog Offset (Units = V x Anafact + Anaofs)
    Dim anafilt(4) As Double     'Analog Filtering Factor
    Dim i As Integer            'Loop counter
    Dim rt As Integer           'Refresh timer
    Dim sendSP As Boolean       'Send Mode/Setpoint (remote mode) if send button clicked
    Dim sendttl As Boolean      'Send TTl if button clicked
    Public Shared sendcmd As String       'Remote Send Command
    Dim batchname As String
    Dim prog(200) As String                 'Batch Program
    Dim progloop(10) As Integer             'Loop back address 
    Dim proglcnt(10) As Integer             'Program Loop Copunt
    Dim pind As Integer                     'Program Index pointer
    Dim batrun As Boolean                   'Running batch or not ma?
    Dim Tnext As Double                     'Time to exacute next batch command
    Dim cmdval As Single                    'Set point value of a batch command
    Dim dwell As Integer                    'Dwell period (pause before subsaquent command) in sec of a batch command
    Dim itmp As Integer                     'Batch command temporary number
    Dim batsrvo As Boolean                  'Batch Servo Command (servos throttle position)
    Dim batsvc As Char                      'Batch Servo Channel
    Dim batsvv As Double                    'Batch Servo Channel Target Value
    Dim batsvr As Integer                   'Batch Servo Throttle Rate
    Dim scmd As String                      'Temp servo command during de-parsing
    Dim batsav As Double                    'Batch Servo Command "Actual" value of servo control parameter
    Dim tnexts As Double                    'Time of next servo update
    Dim fwt(600, 2) As Single               'Fuel Weight
    Dim FSon As Boolean                     'Fuel Scale on or not ma?
    Dim temp As Integer
    Dim find As Integer                     'Index of Fuel Measurment
    Dim fc As Single                        'Fuel Consumption (gm/min)
    Dim bsfc As Single                      'BSFC gm/kWh
    Dim km As Single                        'Kilometers Traveled
    Dim tss As Single                       'Time of Start of the drive Cycle
    Dim DCn As Integer                      'Drive Cycle Number (of cycles)
    Dim drvfile As String                   'Drive Cycle File Name
    Dim dt As Single                        'Drive Cycle Time
    Dim tcmax As Single                     'Max tie in Drive Cycle (to trigger quit)
    Dim dv As Single                        'Driver cycle Velocity
    Dim saetemp As Double                   'Temp for SAE correction Factor
    Dim saepress As Double                  'Pressure for SAE correstion Factor
    Dim saerh As Double                     '%RH for SAE Correction Factor
    Dim saecf As Double                     'SAE Correction Factor (nominally 1)
    Dim serports As String                  'Names of all serial ports available


    Dim FuelWt As Double
    'Format should be:    ZSSSS1_SSSS2_TT.TTT_LLL_Mode_SP_TTL_Alarm_A1_A2_A3_A4 [CR] 
    Dim Speed1 As Double
    Dim Speed1o As Double   'Old Speed 1 value
    Dim speed1av As Double
    Dim speed2 As Double
    Dim torque As Single	'Raw + Friction + Inertial Torque
    Dim TorqueRaw as single	'Raw torque reading from load cell	
    Dim torqueo As Single
    Dim torqueAF As Single  'Torque Averaging Factor (also applied to Acellerational Torques)
    Dim acell As Single     'Acelleration average
    Dim torqueav As Single
    Dim Dynoload As Single
    Dim mode As String
    Dim setpt As Double
    Dim throttle As Single
    Dim throttleo As Single 'Old Throttle
    Dim power As Double
    Dim alarm As Integer
    Dim analog(4) As Single
    Dim time As Double
    Dim timeo As Double
    Dim tctr As Integer     'Time Click Counter (from start of grpah scroll) for erasing old data
    Dim timeav As Double
    Dim tstart As Double
    Dim tgrafX As Single
    Dim tgrafY As Single


    Public Sub New()             ' This call is required by "the designer", whatever that means.
        InitializeComponent()    ' Add any initialization stuff in here. 

        'Read Dynocal File========================================================================
        CalFile = "C:\Focus\DynoCal.CSV"
        If System.IO.File.Exists(CalFile) = True Then
            Using FileRead As New System.IO.StreamReader(CalFile)
                dummy = FileRead.ReadLine()                    'Header
                s = FileRead.ReadLine()                        'Ver
                ver = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                s = FileRead.ReadLine()                        'Mass
                mass = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()                        'diam
                diam = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()                        'diam
                ppr = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                dummy = FileRead.ReadLine()                    'RPMPV
                s = FileRead.ReadLine()                        'diam
                torqueAF = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()                        'diam
                torqoff = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()                        'diam
                torqfact = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()                        'diam
                frictoff = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()                        'diam
                frictfact = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()                    'LastCal
                lastcal = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                s = FileRead.ReadLine()                        'Type
                dynotype = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                s = FileRead.ReadLine()
                rollerdiam = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()
                inert = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()
                compt = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()
                fscompt = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()
                EmisCompt = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                For Me.i = 1 To 4                                              'Read Analog channels Labels and Factors
                    s = FileRead.ReadLine()
                    analabel(Me.i) = Mid(s, 1, InStr(s, ",") - 1)
                    anafact(Me.i) = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                Next
                For Me.i = 1 To 4                                              'Read Analog channels Offsets
                    s = FileRead.ReadLine()
                    anaofs(Me.i) = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                Next
                For Me.i = 1 To 4                                              'Read Analog channels Filter Values
                    s = FileRead.ReadLine()
                    anafilt(Me.i) = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                Next
                s = FileRead.ReadLine()
                tgrafX = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()
                tgrafY = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))
                s = FileRead.ReadLine()
                batchname = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                s = FileRead.ReadLine()
                drivecyc = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                s = FileRead.ReadLine()
                saetemp = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                s = FileRead.ReadLine()
                saepress = Mid(s, InStr(s, ",") + 1, Len(s) - 1)
                s = FileRead.ReadLine()
                saerh = Mid(s, InStr(s, ",") + 1, Len(s) - 1)                'MsgBox(CalFile + " Version is " + ver)
                FileRead.Close()            'Gotta use these 2 lines to close the file after reading
                FileRead.Dispose()
            End Using
            'FileClose(1)
        Else
            MsgBox(CalFile + " Does Not Exist. Using Defaults.")            'Use Defaults and create DynoCal file

            ver = "Default"
            mass = 120
            diam = 0.4
            ppr = 20
            torqoff = 0
            torqfact = 1
            torqueAF = 0.5
            frictoff = 0
            frictfact = 0
            lastcal = "Not Calibrated."
            dynotype = "Unknown"
            rollerdiam = 0.3
            inert = 0
            compt = 1
            fscompt = 0
            EmisCompt = 0
            For Me.i = 1 To 4                                              'Analog Labels and Factors
                analabel(Me.i) = "V"
                anafact(Me.i) = 1
                anaofs(Me.i) = 0
            Next
            tgrafX = 10000
            tgrafY = 100
            timeav = 0.25
            drivecyc = "ECER40.DRV"
            saetemp = 25
            saepress = 1000
            saerh = 50
        End If

        'Display Calibration FactorsVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV
        TextBox18.Text = inert
        TextBox19.Text = rollerdiam
        TextBox17.Text = mass
        TextBox16.Text = diam
        BatchFile.Text = batchname                        'Show previous batch filename
        TextBox29.Text = drivecyc
        DCn = 0


        'compt = 4                                       '1 or 2 for internal, higher numbers for USB ports (like 5 or 15)
        'Check and verify Coms to things HERE
        comstr = "COM" + LTrim(Str(compt))
        '     If PortIsAvailable(comstr) Then
        ' module1.comport.PortName = comstr    'OPen Dyno Controller Com Port: NOTE Can use USB com ports just like internal ones!
        comport.PortName = comstr    'OPen Dyno Controller Com Port: NOTE Can use USB com ports just like internal ones!
        comport.BaudRate = 9600
        COMport.ReadBufferSize = 64
        ' COMport.ReceivedBytesThreshold = 32        '       MsgBox("About to try" + comstr)
        Try
            comport.Open()
        Catch ex As Exception                       'This only allows 1 retry!!!
            serports = ""
            For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                serports = serports + sp + ", "
            Next
            compt = InputBox(comstr + " Does Not Exist." + Chr(13) + "Available Ports: " + serports, "Enter New Com Port #", 1, 5, 5)
            comstr = "COM" + LTrim(Str(compt))
            '     If PortIsAvailable(comstr) Then
            COMport.PortName = comstr    'OPen Dyno Controller Com Port: NOTE Can use USB com ports just like internal ones!
            COMport.Open()
        End Try



        SaveFile = "C:\Focus\DMLog.CSV"
        FileOpen(2, SaveFile, OpenMode.Output)          'OPen Output Datalogging File 
        WriteLine(2, "Focus Applied Technologies Dyno Controller Log Version: " + ver + "    Opened: " + Now)
        WriteLine(2, "Time (s)", "RPM1", "RPM2", "Torque Raw (Nm)", "Total Torque (Nm)", "Power (W)", "Load", "Mode", "SetPoint", "Throttle", "Alarm", "1:" + analabel(1), "2:" + analabel(2), "3:" + analabel(3), "4:" + analabel(4), "Fuel (g)")
        ' WriteLine(2, "Time(s), RPM1, RPM2, Torque(Nm), Load, Mode, SetPoint, Power, Throttle, Alarm, Analog1 (V), Analog2 (V), Analog3 (V), Analog4 (V), Fuel (g) ")
        WriteLine(2, " ")
        '     WriteLine(2, time, Speed1, speed2, torque, Power, Dynoload, mode, setpt, throttle, alarm, analog(1), analog(2), analog(3), analog(4)) ' Save to file

        TextBox2.Text = SaveFile                        'Display Save File name so user doesn't forget it

        Chart2.Series.Clear()                           'This sets up a graph
        Chart2.Series.Add("Load")
        Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart2.Series(0).Color = Color.Black

        Chart2.ChartAreas(0).AxisY.Minimum = 0
        Chart2.ChartAreas(0).AxisY.Maximum = 100
        Chart2.ChartAreas(0).AxisX.Minimum = 0
        Chart2.ChartAreas(0).AxisX.Maximum = 30
        Chart2.ChartAreas(0).AxisX.MajorGrid.Interval = 5
        Chart2.ChartAreas(0).AxisX.Interval = 5
        Chart2.Series.Add("Throttle")
        Chart2.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart2.Series(1).Color = Color.Blue

        Chart2.Series.Add("Speed")
        Chart2.Series(2).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart2.Series(2).Color = Color.Red

        Chart2.Series.Add("Torque")
        Chart2.Series(3).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart2.Series(3).Color = Color.Green

        Chart3.Series.Clear()           'Analog Logging Graph
        Chart3.Series.Add("Ana1")
        Chart3.Series.Add("Ana2")
        Chart3.Series.Add("Ana3")
        Chart3.Series.Add("Ana4")
        Chart3.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart3.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart3.Series(2).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart3.Series(3).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart3.ChartAreas(0).AxisY.Minimum = 0
        Chart3.ChartAreas(0).AxisY.Maximum = 5
        Chart3.ChartAreas(0).AxisX.Minimum = 0
        Chart3.ChartAreas(0).AxisX.Maximum = 30


        TextBox21.Text = tgrafX
        TextBox22.Text = tgrafY
        Chart4.Series.Clear()           'Torque Curve Graph
        Chart4.Series.Add("Torque")
        Chart4.Series(0).Color = Color.Green
        Chart4.Series.Add("Power")
        Chart4.Series(1).Color = Color.Orange
        Chart4.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart4.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Spline
        Chart4.ChartAreas(0).AxisY.Minimum = 0
        Chart4.ChartAreas(0).AxisY.Maximum = tgrafY
        Chart4.ChartAreas(0).AxisX.Minimum = 0
        Chart4.ChartAreas(0).AxisX.Maximum = tgrafX



        TextBox20.Text = ver
        TextBox24.Text = dynotype
        Logging.Checked = True                  'Turn logging ON
        torqueav = 0
        timeav = 0.25                          'Start with difualt delta T, later update to average (in case we change update rates)
        tctr = 0                               'Strat removing graph points from 0
        tstart = Now.ToOADate                  'Start Time from now
        rt = 0                                  'Reset refresh timer
        batrun = False
        batlines.Text = "Direct"
        FSon = False                            'Start with NO Fuel Scale Display
        find = 0
        TextBox26.Text = "N/A"
        COMport.DiscardInBuffer()
    End Sub

    Private Sub Receiver(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs) Handles comport.DataReceived
        Try
            '   RXtxt = COMport.ReadTo("Z")                        'This reads the serial port and puts the results in the RxTxt string
            '  RXtxt = "Z" + RXtxt
     
            RXtxt = COMport.ReadLine
            If Len(RXtxt) < 10 Then RXtxt = ""
            Me.Invoke(New MethodInvoker(AddressOf Display)) 'If it pukes on this when running, just RE-RUN!
        Catch ex As Exception
            RXtxt = "SerCom Error"
            ' MsgBox("Serial Com Glitch.")              'Conitinue (freezez if we don't catch this error)
        End Try                                             'Don't actually need the msgbox, but gotta have the "catch"
        '                                                    to avoid "freexing" the computer. 
    End Sub
    Private Sub ReceiveF(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs) Handles FSport.DataReceived
        Try
            Rxfs = FSport.ReadLine                       'This reads the serial port and puts the results in the RxTxt string
            FuelWt = Val(Rxfs)
            Me.Invoke(New MethodInvoker(AddressOf FuelRate)) 'If it pukes on this when running, just RE-RUN!
        Catch ex As Exception
            Rxfs = "SerCom Error"
            ' MsgBox("Serial Com Glitch.")              'Conitinue (freezez if we don't catch this error)
        End Try                                             'Don't actually need the msgbox, but gotta have the "catch"
        '                                                    to avoid "freexing" the computer. 
    End Sub
    Private Sub PictureBox2_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox2.Paint ' Power neddle gage
        Dim x1 As Integer = (PictureBox2.Width / 2)             'Y is 0 at top
        Dim x2 As Integer
        Dim y1 As Integer = (PictureBox2.Height * 0.7) + 3      'Leav 20% at bottom ov gage for value
        Dim y2 As Integer
        Dim l As Integer = PictureBox2.Height * 0.45             'Length of needel

        x2 = x1 + l * Math.Cos(Math.PI * 1.125 + power * 3.14 * 1.5 / (tgrafX * tgrafY * 6.28 / 60) / 2)
        x1 = x1 * 0.6 + x2 * 0.4
        y2 = y1 + l * Math.Sin(Math.PI * 1.125 + power * 3.14 * 1.5 / (tgrafX * tgrafY * 6.28 / 60) / 2)
        y1 = y1 * 0.6 + y2 * 0.4

        e.Graphics.DrawLine(Pens.Black, x1 - 1, y1, x2, y2)                 'Draw "Triangular" Needel
        e.Graphics.DrawLine(Pens.Black, x1 + 1, y1, x2, y2)
    End Sub
    Private Sub PictureBox3_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox3.Paint ' RPM it as a neddle gage
        Dim x1 As Integer = (PictureBox3.Width / 2)             'Y is 0 at top
        Dim x2 As Integer
        Dim y1 As Integer = (PictureBox3.Height * 0.7) + 3      'Leav 20% at bottom ov gage for value
        Dim y2 As Integer
        Dim l As Integer = PictureBox3.Height * 0.5             'Length of needel
        x2 = x1 + l * Math.Cos(Math.PI * 1.125 + Speed1 / tgrafX * 3.14 * 1.5 / 2)   ' Scale to max of torque plot RPM
        x1 = x1 * 0.6 + x2 * 0.4
        y2 = y1 + l * Math.Sin(Math.PI * 1.125 + Speed1 / tgrafX * 3.14 * 1.5 / 2)
        y1 = y1 * 0.6 + y2 * 0.4

        e.Graphics.DrawLine(Pens.Black, x1 - 1, y1, x2, y2)                 'Draw "Triangular" Needel
        e.Graphics.DrawLine(Pens.Black, x1 + 1, y1, x2, y2)
    End Sub
    Private Sub PictureBox4_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox4.Paint ' Torque it as a neddle gage
        Dim x1 As Integer = (PictureBox4.Width / 2)             'Y is 0 at top
        Dim x2 As Integer
        Dim y1 As Integer = (PictureBox4.Height * 0.7) + 3      'Leav 20% at bottom ov gage for value
        Dim y2 As Integer
        Dim l As Integer = PictureBox4.Height * 0.5             'Length of needel
        x2 = x1 + l * Math.Cos(Math.PI * 1.125 + torque / tgrafY * 3.14 * 1.5 / 2)
        x1 = x1 * 0.6 + x2 * 0.4
        y2 = y1 + l * Math.Sin(Math.PI * 1.125 + torque / tgrafY * 3.14 * 1.5 / 2)
        y1 = y1 * 0.6 + y2 * 0.4

        e.Graphics.DrawLine(Pens.Black, x1 - 1, y1, x2, y2)                 'Draw "Triangular" Needel
        e.Graphics.DrawLine(Pens.Black, x1 + 1, y1, x2, y2)
    End Sub
    Private Sub PictureBox5_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox5.Paint ' Load neddle gage
        Dim x1 As Integer = (PictureBox5.Width / 2)             'Y is 0 at top
        Dim x2 As Integer
        Dim y1 As Integer = (PictureBox5.Height * 0.7) + 3      'Leav 20% at bottom ov gage for value
        Dim y2 As Integer
        Dim l As Integer = PictureBox5.Height * 0.5             'Length of needel
        x2 = x1 + l * Math.Cos(Math.PI * 1.125 + Dynoload / 100 * 3.14 * 1.5 / 2)
        x1 = x1 * 0.6 + x2 * 0.4
        y2 = y1 + l * Math.Sin(Math.PI * 1.125 + Dynoload / 100 * 3.14 * 1.5 / 2)
        y1 = y1 * 0.6 + y2 * 0.4

        e.Graphics.DrawLine(Pens.Black, x1 - 1, y1, x2, y2)                 'Draw "Triangular" Needel
        e.Graphics.DrawLine(Pens.Black, x1 + 1, y1, x2, y2)
    End Sub
    Private Sub PictureBox6_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox6.Paint ' Throttle neddle gage
        Dim x1 As Integer = (PictureBox6.Width / 2)             'Y is 0 at top
        Dim x2 As Integer
        Dim y1 As Integer = (PictureBox6.Height * 0.7) + 3      'Leav 20% at bottom ov gage for value
        Dim y2 As Integer
        Dim l As Integer = PictureBox6.Height * 0.5             'Length of needel
        x2 = x1 + l * Math.Cos(Math.PI * 1.125 + throttle / 1000 * 3.14 * 1.5 / 2)
        x1 = x1 * 0.6 + x2 * 0.4
        y2 = y1 + l * Math.Sin(Math.PI * 1.125 + throttle / 1000 * 3.14 * 1.5 / 2)
        y1 = y1 * 0.6 + y2 * 0.4

        e.Graphics.DrawLine(Pens.Black, x1 - 1, y1, x2, y2)                 'Draw "Triangular" Needel
        e.Graphics.DrawLine(Pens.Black, x1 + 1, y1, x2, y2)
    End Sub
    Private Sub FuelRate()
        '   Calculate fuel consumption (gm/sec) and update display thereof
        If FSon Then
            find = find + 1                         'Increment array index
            If find > 50 And fwt(50, 2) - fwt(1, 2) < 10 Then find = 1 ' Catch bad startup data
            If find > 60 Then                       'Got 60 samples?
                find = 60
                For i = 1 To 59                     'Yah, a pointer would be a better way of doing this, but I'm lazy
                    fwt(i, 1) = fwt(i + 1, 1)
                    fwt(i, 2) = fwt(i + 1, 2)
                Next
            End If
            If find = 59 Then fc = 60 * (fwt(48, 1) - fwt(58, 1)) / (fwt(58, 2) - fwt(48, 2)) ' Start average at reassonable point
            fwt(find, 1) = FuelWt
            fwt(find, 2) = Int(864000 * (Now.ToOADate - tstart)) / 10
            If find = 60 And fwt(60, 2) <> fwt(1, 2) Then
                fc = fc * 0.99 + 0.01 * (60 * (fwt(1, 1) - fwt(60, 1)) / (fwt(60, 2) - fwt(1, 2)))
                TextBox26.Text = Str(Int(1000 * fc) / 1000)
                If power > 0 Then
                    bsfc = 0.9 * bsfc + 0.1 * (60 * fc / (power / 1000))
                    TextBox27.Text = Int(bsfc)
                End If
            Else
                TextBox26.Text = "PEND:" + Str(60 - find)
            End If
        Else
            find = 0
        End If
        
    End Sub

    Private Sub Display()
        ' Label1.Clear()
        '

        'Format should be:    ZSSSS1_SSSS2_TT.TTT_LLL_Mode_SP_TTL_Alarm_A1_A2_A3_A4 [CR] 
        'FuelWt = Val(RXtxt)
        time = Int(864000 * (Now.ToOADate - tstart)) / 10           'Calculate time elapsed in seconds fractional
        rt = rt + 1
        If rt = 50 Then
            rt = 0
            Application.DoEvents()                                  'Refresh to avoid "seizing/Dissappearing"
            'Chart1.Invalidate()
            Chart2.Invalidate()
            Chart3.Invalidate()
            Chart4.Invalidate()

        End If

        If FSon Then
            FuelWt = Val(Rxfs)
            'TextBox25.Text = FuelWt
            TextBox25.Text = Rxfs
        Else
            TextBox25.Text = " OFF "
            '        TextBox26.Text = " Off: " + Str(find)
        End If

        '        If InStr(RXtxt, "Z") = 1 And Len(RXtxt) > 30 And InStr(RXtxt, "!") = 0 And InStr(RXtxt, "<") = 0 And InStr(RXtxt, ">") = 0 Then


        If InStr(RXtxt, "Z") = 1 And Len(RXtxt) > 30 And InStr(RXtxt, "!") = 0 And InStr(RXtxt, "<") = 0 And InStr(RXtxt, ">") = 0 Then
            TextBox1.Text = RXtxt

            RXtxt = Mid(RXtxt, 2, Len(RXtxt) - 1)
            Speed1 = Val(RXtxt) / 10
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            speed2 = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            TorqueRaw = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            Dynoload = Val(Mid(RXtxt, 1, InStr(RXtxt, " "))) / 10
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            mode = Mid(RXtxt, 1, 2)
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            setpt = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            throttle = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            alarm = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            analog(1) = analog(1) * anafilt(1) + (1 - anafilt(1)) * ((Val(Mid(RXtxt, 1, InStr(RXtxt, " "))) * 5 / 1023) * anafact(1) + anaofs(1))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            analog(2) = analog(2) * anafilt(2) + (1 - anafilt(2)) * ((Val(Mid(RXtxt, 1, InStr(RXtxt, " "))) * 5 / 1023) * anafact(2) + anaofs(2))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            analog(3) = analog(3) * anafilt(3) + (1 - anafilt(3)) * ((Val(Mid(RXtxt, 1, InStr(RXtxt, " "))) * 5 / 1023) * anafact(3) + anaofs(3))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            analog(4) = analog(4) * anafilt(4) + (1 - anafilt(4)) * ((Val(RXtxt) * 5 / 1023) * anafact(4) + anaofs(4))   'Units = V x Anafact + Anaofs
            RXtxt = ""
            'TextBox2.Text = ">" + RXtxt + "<"                   'Process RAW incomming numbers

            timeav = timeav * 0.9 + (time - timeo) * 0.1            'Delta T in windows is +/- 15ms, so average heavily
            timeo = time
            speed1av = Speed1 * 0.8 + speed1av * 0.2
            If time < 0.5 Then speed1av = Speed1 'Start Avg speed at actual when starting
            Speed1 = speed1av                                       'Smooth speed a bit
            TorqueRaw = torqoff + TorqueRaw * torqfact              'Modify with offset and slope (normally 0 and 1 respectively)
            torque = TorqueRaw + frictoff + speed1av * frictfact    'Add frictional compensation  
            If timeav <> 0 Then
                acell = acell * torqueAF + (1 - torqueAF) * ((Speed1 - Speed1o) * 2 * Math.PI / (60 * timeav))
                torque = torque + inert * acell    'Acellerational Torque from Inertia
            End If



            If time < 1 Then torque = TorqueRaw 'Avoid "infinite" acellerations on power up of spinning system 
            torqueav = torqueav * torqueAF + torque * (1 - torqueAF)
            torque = torqueav
            power = Speed1 * torque * 2 * Math.PI / 60
            saecf = 1.176 * ((900 / saepress) * ((saetemp + 273) / 298) ^ 0.5) - 0.176  'Apply Correction Factor
            If SAEBox.Checked = True And saecf > 0.9 And saecf < 1.1 Then power = power * saecf ' if it is less than a 10% change

            TextBox3.Text = Int(Speed1)             'Display Data in text boxes
            TextBox4.Text = speed2
            TextBox5.Text = Int(torque * 100) / 100
            TextBox6.Text = Dynoload
            TextBox7.Text = mode
            TextBox8.Text = setpt
            TextBox23.Text = Int(throttle) / 10
            TextBox9.Text = Int(power)
            If alarm = 1 Then
                Label11.Text = "ALARM!"
                Label11.BackColor = Color.Red

            Else
                Label11.BackColor = Color.Green
                Label11.Text = " OK "
            End If

            TextBox10.Text = Int(Speed1 * 60 * rollerdiam * Math.PI / 100) / 10       'kph
            km = km + Speed1 * rollerdiam * Math.PI * timeav / 60 / 1000                                'Update Odometer
            TextBox28.Text = Int(km * 10) / 10

            TextBox11.Text = analog(1)
            TextBox12.Text = analog(2)
            TextBox13.Text = analog(3)
            TextBox14.Text = analog(4)
            Label12.Text = "1: " + analabel(1)
            Label13.Text = "2: " + analabel(2)
            Label14.Text = "3: " + analabel(3)
            Label15.Text = "4: " + analabel(4)
            TextBox15.Text = time

            If Logging.Checked = True Then
                ' FileOpen(2, SaveFile, OpenMode.Append)
                WriteLine(2, time, Int(Speed1), speed2, TorqueRaw, torque, Int(10 * power) / 10, Dynoload, mode, setpt, throttle, alarm, analog(1), analog(2), analog(3), analog(4), FuelWt) ' Save to file
                'FileClose(2)
            End If

            'Graph
            If time > 30 Then                                                   'Should Convert to Seconds on X-Axis
                Chart2.ChartAreas(0).AxisX.Minimum = Int(time + 0.8) - 30
                Chart2.ChartAreas(0).AxisX.Maximum = Int(time + 0.8)
                Chart3.ChartAreas(0).AxisX.Minimum = Int(time + 0.8) - 30
                Chart3.ChartAreas(0).AxisX.Maximum = Int(time + 0.8)
                tctr = tctr + timeav                                                 'Next point
                Chart2.Series(0).Points.RemoveAt(0)                         'Remove old data from bottom of stack
                Chart2.Series(1).Points.RemoveAt(0)                         'Must remove old data or it'll overflow memory
                Chart2.Series(2).Points.RemoveAt(0)
                Chart2.Series(3).Points.RemoveAt(0)
                Chart3.Series(0).Points.RemoveAt(0)
                Chart3.Series(1).Points.RemoveAt(0)
                Chart3.Series(2).Points.RemoveAt(0)
                Chart3.Series(3).Points.RemoveAt(0)

            End If

            Chart2.Series(0).Points.AddXY(time, Dynoload)
            Chart2.Series(1).Points.AddXY(time, throttle / 10)
            Chart2.Series(2).Points.AddXY(time, Speed1 * 100 / tgrafX)
            Chart2.Series(3).Points.AddXY(time, torque * 100 / tgrafY)

            If anafact(1) <> 0 Then Chart3.Series(0).Points.AddXY(time, (analog(1) - anaofs(1)) / anafact(1))
            If anafact(2) <> 0 Then Chart3.Series(1).Points.AddXY(time, (analog(2) - anaofs(2)) / anafact(2))
            If anafact(3) <> 0 Then Chart3.Series(2).Points.AddXY(time, (analog(3) - anaofs(3)) / anafact(3))
            If anafact(4) <> 0 Then Chart3.Series(3).Points.AddXY(time, (analog(4) - anaofs(4)) / anafact(4))

            Chart4.Series(0).Points.AddXY(Speed1, torque)
            Chart4.Series(1).Points.AddXY(Speed1, power / (tgrafX * 6.28 / 60))
            If Chart4.Series(0).Points.Count > 500 Then
                Chart4.Series(0).Points.RemoveAt(0)
                Chart4.Series(1).Points.RemoveAt(0)
            End If

            If Clearbox.Checked Then                    'Reset the clear button
                Clearbox.Checked = False
                'Clear grpah
                Chart4.Series(0).Points.Clear()
                Chart4.Series(1).Points.Clear()
            End If
            If tgrafX <> Val(TextBox21.Text) Then
                tgrafX = Val(TextBox21.Text)
                If tgrafX = 0 Then tgrafX = 1 'Prevent Max being 0
                Chart4.ChartAreas(0).AxisX.Maximum = tgrafX
            End If
            If tgrafY <> Val(TextBox22.Text) Then
                tgrafY = Val(TextBox22.Text)
                If tgrafY = 0 Then tgrafY = 1 'Prevent Max = 0
                If tgrafY < 1 Then
                    Chart4.ChartAreas(0).AxisY.LabelStyle.Format = "{0.00}"     'This is just to prevent crazy # digits on graph axis
                Else
                    If tgrafY < 10 Then
                        Chart4.ChartAreas(0).AxisY.LabelStyle.Format = "{0.0}"
                    Else
                        Chart4.ChartAreas(0).AxisY.LabelStyle.Format = "{0}"
                    End If

                End If

                Chart4.ChartAreas(0).AxisY.Maximum = tgrafY
            End If

            If throttleo <> throttle Then PictureBox6.Invalidate() 'Either one of these lines can be used to re-draw meter
            throttleo = throttle
            PictureBox4.Invalidate()
            PictureBox5.Invalidate()
            PictureBox2.Invalidate()
            PictureBox3.Invalidate()   'Always re-draw power
            If Speed1 <> Speed1o Then PictureBox3.Invalidate() 'Either one of these lines can be used to re-draw meter
            Speed1o = Speed1
            throttleo = throttle
            rollerdiam = Val(TextBox19.Text)                    'This changes the value of the variable when entered in the text box
            inert = Val(TextBox18.Text)
            mass = Val(TextBox17.Text)
            diam = Val(TextBox16.Text)

        Else
            If RXtxt <> "" Then TextBox1.Text = "Invalid Serial Input"
            'TextBox1.Text = ">>>" + RXtxt + "<<<<"

        End If
        If CB1.Checked Then CmdUnit.Text = "K" 'Put the proper simbol by the Send Comand box for Remote Control
        If CB2.Checked Then CmdUnit.Text = "Nm"
        If CB3.Checked Then CmdUnit.Text = "rpm"
        If CB4.Checked Then CmdUnit.Text = "%"

        If DrvCycle.Visible Then               'If it's open and visable, then show time DDDDDDDRRRRRRRIIIIIIVVVVVVEEEEE!!!!
            TextBox30.Text = DCn
            DrvCycle.TextBox1.Text = drivecyc
            DrvCycle.TextBox2.Text = Int((time - tss) * 10) / 10
            '  Chart2.ChartAreas(0).AxisY.Minimum = 0
            ' Chart2.ChartAreas(0).AxisY.Maximum = 100
            If time - tss > 5 Then
                DrvCycle.Chart1.ChartAreas(0).AxisX.Minimum = Int((time - tss) * 10) / 10 - 5
                DrvCycle.Chart1.ChartAreas(0).AxisX.Maximum = Int((time - tss) * 10) / 10 + 10
            End If
            If time - tss > tcmax Then Button10_Click(Me, EventArgs.Empty)
            ' DrvCycle.Chart1.Series(0).Points.AddXY(time - tss, throttle / 10)                            ' Switch these Two
            DrvCycle.Chart1.Series(0).Points.AddXY(time - tss, Int(Speed1 * 60 * rollerdiam * Math.PI / 100) / 10)  ' Lines for Testing   
            If DrvCycle.Chart1.Series(0).Points.Count > 40 Then
                DrvCycle.Chart1.Series(0).Points.RemoveAt(0)
                'DrvCycle.Chart1.Series(1).Points.RemoveAt(0)
            End If
        End If

        If batrun = True Then       'RUN BATCH FILE 
            If batsrvo = True Then                      'Perform servo iterations
                If time > tnexts Then
                    Select Case batsvc
                        Case "T"
                            batsav = torque
                        Case "S"
                            batsav = Speed1
                        Case "1"
                            batsav = analog(1)
                        Case "2"
                            batsav = analog(2)
                        Case "3"
                            batsav = analog(3)
                        Case "4"
                            batsav = analog(4)
                    End Select
                    If batsav < batsvv Then                 'Increment Throttle Position
                        COMport.WriteLine("P" + LTrim(Str(Int(throttle) + batsvr)))
                        '  MsgBox("P" + LTrim(Str(Int(throttle * 1.023) + batsvr)))
                    Else
                        COMport.WriteLine("P" + LTrim(Str(Int(throttle) - batsvr)))
                        '   MsgBox("P" + LTrim(Str(Int(throttle * 1.023) - batsvr)))
                    End If
                    tnexts = time + 1                       'Next servo update in 1 sec
                    If time >= Tnext Then batsrvo = False
                End If
            End If
            If time >= Tnext Then
                batsrvo = False
                cmdval = 0
                dwell = 0
                If Len(prog(pind)) > 2 Then cmdval = Val(Mid(prog(pind), 2, Len(prog(pind)) - 1))
                If InStr(prog(pind), ",") > 0 Then dwell = Val(Mid(prog(pind), InStr(prog(pind), ",") + 1, Len(prog(pind)) - 1))
                'Assign time of next command exacution
                ' MsgBox(Str(pind) + ":" + Mid(prog(pind), 1, 1) + " " + Str(cmdval) + " " + Str(dwell))
                Select Case Mid(prog(pind), 1, 1)
                    Case "C"                                                            'Dynamic Throttle Control 
                        batsrvo = True
                        batsvc = Mid(prog(pind), 2, 1)                                  'Servo Channel
                        scmd = Mid(prog(pind), 4, Len(prog(pind)) - 3)
                        batsvv = Val(scmd)                                              'Set Value
                        scmd = Mid(scmd, InStr(scmd, ",") + 1, Len(scmd) - 1)
                        batsvr = Int(10.24 * Val(Mid(scmd, 1, InStr(scmd, ","))))   'Throttle Step Size 
                        Tnext = time + Val(Mid(scmd, InStr(scmd, ",") + 1, Len(scmd) - 1))
                        tnexts = time - 1
                        BatchFile.Text = "Line " + Str(pind) + ": " + "Servoing " + batsvc + ":" + batsvv
                        pind = pind + 1                                                         'Go to next command (but not yet)
                        'Start servoing now
                        ' MsgBox("Servo Cmd: " + batsvc + " to " + Str(batsvv) + " step " + Str(batsvr) + " dwell " + Mid(scmd, InStr(scmd, ",") + 1, Len(scmd) - 1))
                    Case "L"
                        '   itmp = Val(Mid(prog(pind), 2, Len(prog(pind)) - 1))
                        proglcnt(cmdval) = proglcnt(cmdval) - 1
                        If proglcnt(cmdval) = 0 Then
                            pind = pind + 1                                             'Goto next commend
                        Else
                            pind = progloop(cmdval)                                     'Jump to loop label
                        End If
                        '  Case "S", "T", "R"                                                  'Gotta "Ltrim" to eliminate leading 0 from #'s 
                        '     COMport.WriteLine(Mid(prog(pind), 1, 1) + LTrim(Str(cmdval)) + Chr(13))
                        '    BatchFile.Text = "Line " + Str(pind) + ": " + Mid(prog(pind), 1, 1) + LTrim(Str(cmdval))
                        '   Tnext = time + dwell
                        '  pind = pind + 1
                    Case "S", "T", "R", "M", "P"                                        'Gotta "Ltrim" to eliminate leading 0 from #'s 
                        COMport.WriteLine(Mid(prog(pind), 1, 1) + LTrim(Str(cmdval)))
                        'NOTE: Older Versions have to "left pad" number with '0's if # is too short (controller counts characters)
                        '  MsgBox("Sent " + Mid(prog(pind), 1, 1) + LTrim(Str(cmdval)))
                        BatchFile.Text = "Line " + Str(pind) + ": " + Mid(prog(pind), 1, 1) + LTrim(Str(cmdval))
                        Tnext = time + dwell
                        pind = pind + 1
                    Case "E"                                                             'End Batch Program
                        batrun = False
                        batlines.Text = "Direct"
                        BatchFile.Text = batchname
                    Case Else
                        pind = pind + 1
                End Select
            End If
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click 'Exit Button Routine

        CalFile = "C:\Focus\DynoCal.CSV"
        '   If System.IO.File.Exists(CalFile) Then
        '       System.IO.File.Delete(CalFile)
        '   End If
        Using FileWrite As New System.IO.StreamWriter(CalFile)
            FileWrite.WriteLine("Focus Applied Technologies Dynamometer Calibration File, DC5")                    'Header
            FileWrite.WriteLine("Version," + ver)
            FileWrite.WriteLine("Mass, " + Str(mass))
            FileWrite.WriteLine("Wheel Diameter, " + Str(diam))
            FileWrite.WriteLine("Pulses per Rev," + Str(ppr))
            FileWrite.WriteLine("RPMPV, 0")                    'RPMPV  These 2 are no-longer used
            FileWrite.WriteLine("Torque Averaging, 0.5 ")                    'Torque Averaging Factor
            FileWrite.WriteLine("Torque Offset," + Str(torqoff))
            FileWrite.WriteLine("Torque Factor," + Str(torqfact))
            FileWrite.WriteLine("Frict Offset," + Str(frictoff))
            FileWrite.WriteLine("Frict Factor," + Str(frictfact))
            FileWrite.WriteLine("Last Cal," + lastcal)
            FileWrite.WriteLine("Dyno Type," + dynotype)
            FileWrite.WriteLine("Roller Diam," + Str(rollerdiam))
            FileWrite.WriteLine("Inertia," + Str(inert))
            FileWrite.WriteLine("Controller Com Port," + Str(compt))
            FileWrite.WriteLine("Fuel Scale Com Port," + Str(fscompt))
            FileWrite.WriteLine("Emissions Com Port," + Str(EmisCompt))
            For Me.i = 1 To 4                                              'Analog Labels and Factors
                FileWrite.WriteLine(analabel(Me.i) + "," + Str(anafact(Me.i)))
            Next
            For Me.i = 1 To 4                                              'Analog Labels and Offsets
                FileWrite.WriteLine("Analog Offset" + Str(i) + "," + Str(anaofs(Me.i)))
            Next
            For Me.i = 1 To 4                                              'Analog Labels and Offsets
                FileWrite.WriteLine("Analog Filter" + Str(i) + "," + Str(anafilt(Me.i)))
            Next
            FileWrite.WriteLine("T-Graph Max X," + Str(tgrafX))
            FileWrite.WriteLine("T-Graph Max Y," + Str(tgrafY))
            FileWrite.WriteLine("Batch File," + batchname)
            FileWrite.WriteLine("Drive Cycle," + drivecyc)
            FileWrite.WriteLine("Temp," + Str(saetemp))
            FileWrite.WriteLine("Press," + Str(saepress))
            FileWrite.WriteLine("RH," + Str(saerh))
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.WriteLine("Spare,0")
            FileWrite.Close()
            FileWrite.Dispose()

        End Using
        '  FileOpen(1, CalFile, OpenMode.Output)             'Open Output Datalogging File 


        FileClose(1)
        FileClose(2)
        Me.Close()          'Exit when "Exit" button hit 
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'Adjust Analog Values
        If Form2.Visible Then               'If it's open and visable, then we must want to close, so save variable values
            analabel(1) = Form2.LB1.Text
            anafact(1) = Val(Form2.UB1.Text)
            analabel(2) = Form2.LB2.Text
            anafact(2) = Val(Form2.UB2.Text)
            analabel(3) = form2.LB3.Text
            anafact(3) = Val(form2.UB3.Text)
            analabel(4) = form2.LB4.Text
            anafact(4) = Val(form2.UB4.Text)
            anaofs(1) = Val(form2.OB1.Text)
            anaofs(2) = Val(form2.OB2.Text)
            anaofs(3) = Val(form2.OB3.Text)
            anaofs(4) = Val(form2.OB4.Text)
            anafilt(1) = val(form2.FB1.Text)
            anafilt(2) = val(form2.FB2.Text)
            anafilt(3) = val(form2.FB3.Text)
            anafilt(4) = val(form2.FB4.Text)
            If anafact(1) = 0 Then anafact(1) = 1 'To prevent divide by 0 during display unscaled
            If anafact(2) = 0 Then anafact(2) = 1
            If anafact(3) = 0 Then anafact(3) = 1
            If anafact(4) = 0 Then anafact(4) = 1
            Form2.Close()
        Else
            Form2.Show()    ' This opens the "form2" window (which is what we want to work on in this subroutine)
            Form2.LB1.Text = analabel(1)           'Display the existing values, and allow them to be manipulated
            Form2.UB1.Text = anafact(1)
            Form2.LB2.Text = analabel(2)
            Form2.UB2.Text = anafact(2)
            Form2.LB4.Text = analabel(4)
            Form2.UB4.Text = anafact(4)
            Form2.LB3.Text = analabel(3)
            form2.UB3.Text = anafact(3)
            form2.OB1.Text = anaofs(1)
            form2.OB2.Text = anaofs(2)
            form2.OB3.Text = anaofs(3)
            form2.OB4.Text = anaofs(4)
            form2.FB1.Text = anafilt(1)
            form2.FB2.Text = anafilt(2)
            form2.FB3.Text = anafilt(3)
            form2.FB4.Text = anafilt(4)
            If anafilt(1) > 0.99 Then anafilt(1) = 0.99 'To prevent general crazyness with the averaging bit
            If anafilt(2) > 0.99 Then anafilt(2) = 0.99
            If anafilt(3) > 0.99 Then anafilt(3) = 0.99
            If anafilt(4) > 0.99 Then anafilt(4) = 0.99
        End If
    End Sub


    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click ' Log File Name
        SaveFile = InputBox("Currently Logging to: " + SaveFile, "Enter New Log Fielname ", "C:\Focus\DMV5.CSV")
        FileClose(2)
        FileOpen(2, SaveFile, OpenMode.Output)
        WriteLine(2, "Focus Applied Technologies Dyno Controller Log Version: " + ver + "    Opened: " + Now)
        WriteLine(2, "Time (s)", "RPM1", "RPM2", "Torque Raw (Nm)", "Total Torque (Nm)", "Power (W)", "Load", "Mode", "SetPoint", "Throttle", "Alarm", "1:" + analabel(1), "2:" + analabel(2), "3:" + analabel(3), "4:" + analabel(4), "Fuel (g)")
        WriteLine(2, " ")
        TextBox2.Text = SaveFile
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        If Form3.Visible Then               'If it's open and visable, then we must want to close, so save variable values
            torqoff = Val(Form3.TextBox1.Text)
            torqfact = Val(Form3.TextBox2.Text)
            frictoff = Val(Form3.TextBox3.Text)
            frictfact = Val(Form3.TextBox4.Text)
            saetemp = Val(Form3.TextBox7.Text)
            torqueAF = Val(Form3.TextBox8.Text)
            If torqueAF < 0 Then torqueAF = 0 'Clip Averaging
            If torqueAF > 0.99 Then torqueAF = 0.99
            If saetemp < 15 Then saetemp = 15
            If saetemp > 35 Then saetemp = 35 'Clip to the limits of the equasion
            saepress = Val(Form3.TextBox6.Text)
            If saepress < 900 Then saepress = 900
            If saepress > 1050 Then saepress = 1050 'CLip to limits of the equasion
            saerh = Val(Form3.TextBox5.Text)
            Form3.Close()
        Else
            '  Dim form3 As New Form3
            Form3.Show()    ' This opens the "form3" window (which is what we want to work on in this subroutine)
            Form3.TextBox8.Text = torqueAF
            Form3.TextBox1.Text = torqoff           'Display the existing values, and allow them to be manipulated
            Form3.TextBox2.Text = torqfact
            Form3.TextBox3.Text = frictoff
            Form3.TextBox4.Text = frictfact
            Form3.TextBox7.Text = saetemp
            Form3.TextBox6.Text = saepress
            Form3.TextBox5.Text = saerh
        End If           'Adjust Calibrations
    End Sub
    Private Sub button5_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If CB1.Checked Then sendcmd = "R" + RemCmd.Text
        If CB2.Checked Then sendcmd = "T" + RemCmd.Text
        If CB3.Checked Then sendcmd = "S" + RemCmd.Text
        If CB4.Checked Then sendcmd = "M" + Mid(Str(Int(10 * Val(RemCmd.Text))), 2, Len(Str(Int(10 * Val(RemCmd.Text))) - 1))
        COMport.WriteLine(sendcmd)
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim done As Boolean
        If batrun = False Then
            If System.IO.File.Exists(BatchFile.Text) = True Then
                Using FileRead As New System.IO.StreamReader(BatchFile.Text)

                    batchname = BatchFile.Text
                    BatchFile.Text = "START!"
                    pind = 0
                    done = False
                    While done = False                                      'LOAD Batch File and assign loop control data
                        pind = pind + 1
                        prog(pind) = FileRead.ReadLine()
                        '  BatchFile.Text = "Line " + Str(pind) + ": " + prog(pind)
                        BatchFile.Text = prog(pind)
                        '          MsgBox(Str(pind) + ":" + prog(pind))  Regurgitate the program line-by-line
                        If prog(pind) >= "1" And prog(pind) <= "9" Then
                            progloop(Val(prog(pind))) = pind                'Assign Loob Back Address
                            'MsgBox(Str(pind) + ":" + prog(pind) + " Gives " + Str(Val(prog(pind))))
                        Else
                            If Mid(prog(pind), 1, 1) = "L" Then
                                proglcnt(Val(Mid(prog(pind), 2, Len(prog(pind)) - 2))) = Val(Mid(prog(pind), InStr(prog(pind), ",") + 1, Len(prog(pind)) - 1))               'Get the loop index, and save the loop count in ProgLcnt()
                            End If
                        End If
                        If prog(pind) = "END" Or prog(pind) = "end" Then done = 1
                    End While
                End Using
                batlines.Text = "PROGRAM"
                batrun = True
                pind = 1
                BatchFile.Text = batchname          'Return text box to file name
            End If
        Else
            batrun = False                          'Stop Batch file if hit button while running
            batlines.Text = "Direct"
            BatchFile.Text = batchname
        End If
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        sendcmd = "P" + LTrim(Str(Int(10 * Val(TTLCmd.Text))))          'Convert from 12.3 to 123
        '   MsgBox("Sent " + sendcmd)
        COMport.WriteLine(sendcmd)
    End Sub

    Private Sub TextBox29_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox29.TextChanged
        drivecyc = TextBox29.Text
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click


        If FSon = False Then        'Flip display on/off
            serports = ""
            For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                serports = serports + sp + ", "
            Next

            fscompt = InputBox("Currently Reading From: " + Str(fscompt) + Chr(13) + "Controller Port: " + Str(compt) + Chr(13) + "Available Ports: " + serports, "Enter Fuel Scale Com Port ", Str(fscompt))
            comstr = "COM" + LTrim(Str(fscompt))
            FSport.Close()
            FSport.PortName = comstr    'OPen Fuel Scale Com Port: NOTE Can use USB com ports just like internal ones!
            FSport.BaudRate = 9600
            FSport.ReceivedBytesThreshold = 11
            '   FSport.ReadBufferSize = 11
            '       MsgBox("About to try" + comstr)

            Try
                FSport.Open()              'Erase old Fuel Data from Serial input buffer
            Catch ex As Exception                       'This only allows 1 retry!!!
                serports = ""
                For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                    serports = serports + sp + ", "
                Next

                fscompt = InputBox(comstr + " Does Not Exist." + Chr(13) + "Available Ports: " + serports, "Enter Fuel Scale Com Port #", 1, 5, 5)
                comstr = "COM" + LTrim(Str(fscompt))
                '     If PortIsAvailable(comstr) Then
                FSport.PortName = comstr    'OPen Dyno Controller Com Port: NOTE Can use USB com ports just like internal ones!
                FSport.BaudRate = 9600
                FSport.ReceivedBytesThreshold = 11
                FSport.Open()
            End Try
            FSport.DiscardInBuffer()
            FSon = True
            For i = 1 To 59                     'Erase fuel data
                fwt(i, 1) = 0
                fwt(i, 2) = 0
            Next
        Else
            FSon = False
            FSport.DiscardInBuffer()              'Erase old Fuel Data from Serial input buffer
            FSport.Close()
            'Invalidate FC Measurment
            TextBox26.Text = "N/A"
        End If
        find = 0                                'Zero index of fuel weight array

    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click ' Reset Odometer
        km = 0
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        'Drive Cycle
        If DrvCycle.Visible Then               'If it's open and visable, then we must want to close, so save variable values
            DrvCycle.Chart1.Series(0).Points.Clear()
            DrvCycle.Chart1.Series(1).Points.Clear()
            DrvCycle.Close()
        Else
            '  Dim form3 As New Form3
            DrvCycle.Chart1.Series.Clear()
            DrvCycle.Chart1.Series.Add("Schedule")
            DrvCycle.Chart1.Series(0).Color = Color.Green
            DrvCycle.Chart1.Series.Add("Speed")
            DrvCycle.Chart1.Series(1).Color = Color.Red
            DrvCycle.Chart1.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Spline

            DrvCycle.Chart1.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
            DrvCycle.Chart1.Series(0).ChartType = DataVisualization.Charting.MarkerStyle.Circle

            DrvCycle.Chart1.Series(0).CustomProperties = "ShowMarkerLines = True"
            DrvCycle.Chart1.Series(0).MarkerSize = 4
            DrvCycle.Chart1.ChartAreas(0).AxisX.LabelStyle.Format = "N1"


            DrvCycle.Chart1.ChartAreas(0).AxisY.Minimum = 0
            DrvCycle.Chart1.ChartAreas(0).AxisY.Maximum = 100
            DrvCycle.Chart1.ChartAreas(0).AxisX.Minimum = 0
            DrvCycle.Chart1.ChartAreas(0).AxisX.Maximum = 15
            DrvCycle.Show()    ' This opens the "form3" wind nb nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ow (which is what we want to work on in this subroutine)
            DrvCycle.TextBox1.Text = drivecyc           'Display the existing values, and allow them to be manipulated
            tss = time
            DCn = DCn + 1
            DrvCycle.TextBox3.Text = DCn

            drvFile = "C:\Focus\" + drivecyc
            If System.IO.File.Exists(drvFile) = True Then
                Using FileRead As New System.IO.StreamReader(drvFile)
                    dummy = FileRead.ReadLine()                    'Header
                    Do
                        s = FileRead.ReadLine()
                        If Not (s Is Nothing) Then
                            dt = Val(Mid(s, 1, InStr(s, ",")))                  '1st half is Time (s)
                            dv = Val(Mid(s, InStr(s, ",") + 1, Len(s) - 1))         '2nd half of line is KPH
                            '                           MsgBox("Data is :" + Str(dt) + "," + Str(dv))
                            DrvCycle.Chart1.Series(1).Points.AddXY(dt, dv)          'Draw Drive Cycle
                        End If
                    Loop Until s Is Nothing
                    tcmax = dt                                          'Set max Cycle Time

                    '      Catch e As Exception
                    ' Let the user know what went wrong.
                    'Console.WriteLine("The file could not be read:")
                    '    Console.WriteLine(e.Message)
                    '  End Try
  
                    FileRead.Close()            'Gotta use these 2 lines to close the file after reading
                    FileRead.Dispose()
                End Using
                DrvCycle.TextBox4.Text = TCMax               'Drive Cycle Duration 
            Else
                MsgBox(drvfile + " Does Not Exist. Generate and retry.")            'Use Defaults and create DynoCal file
            End If



        End If           'Adjust Calibrations
    End Sub
End Class


