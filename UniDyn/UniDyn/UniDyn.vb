Imports System                      'Dyno Monitor V-Basic for FAT Gen5 Dyno Controllers
Imports System.IO.Ports             'These "Imports" are just non-required subroutines required by this program
Imports System.Threading            'Goto View "Show Code" if you can't find this. 
Imports System.Threading.Thread     'Some times gotta run twice as it gives a bogus error on 1st run (No idea why).1
Imports System.Drawing.Graphics
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing
Imports Microsoft.Vbe.Interop
Imports System.Management
'Signing: fatvbproj UniDyn_TemporaryKey.pfx
'========================================= REVISIONS =====================================
'V9: Added factor of DGR in Speed Control Mode (incase using Engine Speed not Dyno Speed).
'           Probably still need RPM2/RPM1 ratio if using RPM2
'Remote 3rd button as AutoSave and Exist Graf added
'Switched Instagraf, Autosave, AutoLoad and AutoGraf to Buttons (from Check Boxes)
'Changed RPM reference modes to 3: Dyno, Calcd Engine and RPM2
'   To do this had o change "pull" parametersto be set up at begining of pull (to not mess up the "master" parameters controlling things). 
'Put in ALARM LED on bulat gages, but it is cheezy: redraws the LED if on every time (better to draw in different routing only when off->on) n erase when ON-OFF
'Improved the AUTO load and acelleration adjustments
'NEXT REV: "graf" gage takes way too much time at 20Hz, need a better way to "scroll" the graph
'V10-----------------------------------------------------------------
'Engine Torque is based off of SPEED2 x Factor + Ofset if we got a speed 2 reading (it used raw speed2 which is usually 1/2 the rpm thus 2x the torque)
' Chenged "graf" gages to scroll as an image, not the actual data so should be faster
'Added Gear Ratio of actual engine RPm as setting option
'Auto Tare, Fixed the "crash on exit if USB disconnected" 
'Changes InstaGraf hide to END of pull, and AUTO GRAF open to same place (stop speed). 
'V11: Added Controller Calibrations for DC5.5 (Upper/Lower Threshold levels for Speed readings) and graphic instructions
'Fixed ocassional "drop outs" in Graf when index rolls over 300
'V12------------------------------------------------------------------
'Fixed bug in GRAF where min=max at partial pulls, crashing the GRAF
'Set up graf major ticks for 10% of range, made sure that if MIN=0, MAX=0 then it is 0 and 1 on the Y axis
'Set so during pull if speed < RPM1 then end go back to start pull'
'Click on Insta Graf to close
'V13------------------------------------------------------------------
'Added Shop name and contact info to GRAF
'Added Filtering in Graf
'Made 4 graf overlays in GRAF
'Renamed DYNOMAS wherever easy
'Moved Graf settings to setting page
'Made GRAF re-sizable
'Erase grafs 3 and 4 upon GRAF startup
'Added Analog Label to Analog graf
'Autosave as Default
'Saves Units (wasn't saving!)
'V14------------------------------------------------------------------
'Fixed the "AFR" graf crash when using time (Min=Max)
'Limit Filter to .999, and set to 0.5 if greater
'Change Acelleration pull to TIME based
'Fixed up Tune Buddy (as per EZ Dyno) to read analogs, and re-did screen a bit
'Fixed tha Shortcut Calfile name loading. 
'Put in fix for multiple spaces from controller
'V15------------------------------------------------------------------
'Slowed down Controller Coms to 100ms to prevent accidental "crashing" ser Com outputs
'Added Logging in for helping debug things like bad sercom outs
'V16-----------------------------------------------------------------
'Changed Total Torque to read "factored" Raw Torque instead of raw, as was catching load cell glitches on Inert dynos
'Show DT in Cals window. 
'Updated Controler Cals Send/Read to include checksum (as per EZ Dyno)
'V17-----------------------------------------------------------------
'Acellaertion Pull was overloading on "old" controllers (4Hz), Fixed
'V18-----------------------------------------------------------------
'Changed for 4-button remote (inc/dec load set point)
'Added "total KM" in Cals (for maint ref). Must reset from edit of .CAL file
'Only allow pull if GearRatio is NOT open
'Only Allow Autotare if Cont Cals closed (to avoid messing with Calibration process)
'V19-----------------------------------------------------------------
'Always redisplay shop name and tag line in Graph
'Add "red line" stats at GRAF following remote up/dn button
'Add "Instagraf" feature
'V20----------------------------------------------------------------
'Fixed "Multiple Spaces" in Run File name
'Added Auto Comp Grax Y max and X interval
'Use Manual X-Graf interval on Instagraf
'Save RUN number 
'V21--------------------------------------------------------------
'Added CLEAR button to Grat to kill graf 2, 3, 4
'Added Keyboars short cuts to GR (ESC=Quit, Enter/Space = Set)
'Added new KZR Logo
'Added SAE Correction Factor
'June 2025 Added "MAG" factor for Jpg
'Skin and Background colors
'Default gages (but only on 2nd re-run)
'V22 July 2025----------------------------------------------------
'Added selectable colors at GRAF, Stats on Graf 3 and 4, background logo at graf
'Re-did Auto Friction to be same as EZ dyno (and work!)
'Fixed a GRAF bug where the actual graf (grf) showed up at the origin
'Added 2nd Analog to Graf
'Zerowed Frictional contribution at 0 speed
'V23 Feb 2026 --------------------------------------------------------
'Fixed crashing at Stats on Graf
'Added arrow keys for moving stats point
'V24 Mar 2026 --------------------------------------------------------
'Added Print a Graf and 6000 rpm GR set speed
'Fixeda bug in Header labesl on Graf2 (rewritten by Graf3, 4)
'Added an analog graf reference marker





Public Module DefineGlobals         'This is to define all "global" variables so they can be used in other forms. 
    Public TFirst, Tacc As Boolean     'First time to run Tune?, Tune Accumulate ON?
    ' Public TFirst, Tacc As Boolean     'First time to run Tune?, Tune Accumulate ON?
    Public TuneCal As String        'Channels for X, Y and Z of Tune window
    Public Speed1, speed2, speedC2, torque, gr As Double
    Public throttle As Single
    Public Dynoload As Single
    Public a(50) As Double                  'For sharing with Tune
    Public frav As Single

End Module

Public Class UniDyn
    Dim rs As New Resizer                           'For Form auto Resize

    Dim WithEvents COMport As New SerialPort
    Private WithEvents Ftune As New FTune                           'Tuning Screen
    Private WithEvents dispsel As New DispSel                       'Channel Display form
    Private WithEvents GearRatio As New GearRatio                   'Set Pull RPM factor
    Private WithEvents fgraf As New Fgraf
    Private WithEvents Cals As New Cals
    Private WithEvents Comp As New Comp
    Private WithEvents contcals As New ContCals                     'Controler Calibration Form
    Private WithEvents Timer1 As New System.Windows.Forms.Timer()
    Private WithEvents FOBD As New FOBD                             'OBD Controls
    Dim WithEvents OBDport As New SerialPort
    Public Shared wpfDash As DynoDashboard = Nothing


    Public OBDon, OBDchon(20), OBDSlow As Boolean                           'OBDSlow = Send PID requests one-by-one
    Dim OBDportnum, OBDbr, OBDnch, OBDBytes(20), OBDskp, OBDcnt, OBDcmds As Integer      'OBDskp is how many dyno ping cycles to skip between ECU readings, Comand #
    Dim OBDlegnth, OBDlen(20), OBDL, OBTR As Integer                        'Expected Return Message Legnth, Length of return (by pid) eith 1 or 2, length of current PID
    Dim OBDcmd(20), OBDos, OBDis As String                                  'Out and In Strings
    Dim OBDtime, OBDLast As Single                                          'Refresh rate and time of last OBD ping
    Dim OBDCB(20) As CheckBox
    Dim OBDECU, OBDREP As Boolean                                           'ECU/OBD, Repeate Init Seq?
    Dim OBDiT, OBDiB, OBDpT, OBDpB, obdsends, OBDp As String                'Text Initialization, Byte Init String, Ping Text, Ping Byte, , Send and Parity "N", "E", or "O"                                              '
    Public OBDisLab(20) As Label

    Dim gage(50), grf(50), grafovr As PictureBox             'Grf is the actual "picture" of data
    Dim gtype(50), Gch(50) As Integer        '1=Gage, 2=line, 3=LED, 4= Vert Bar, 5=Horz Bar, 6=Number, and Channel number (Analog data)
    Dim GLOfx(50), GLOfy(50) As Single                                'Offset in X and Y for Label
    Dim RXtxt As String
    Dim i, j, k, oh, ow As Integer
    Public gagemax As Integer                                       'Count of all gages
    Dim gmove, gstrech As Boolean
    Dim gind, gmx, gmy, GMXL, GMYL, gdatlast As Integer                         'Gage Index, and X and Y offsets (from curser position)
    Dim d(50) As String                                               'Incomming Data Strings

    Dim l(50), n(50) As Label                                         'Labels and Numbers for Indicators
    Dim tfont As Font
    Public amin(50), a1(50), a2(50), amax(50), aof(50), Afact(50), afilt(50) As Double                  'Min, Max and various steps inbetween  a(50)
    Dim amajtix(50), amintix(50) As Integer                             'Major and minot tix for graph
    Public alab(50), auni(50), adec(50) As String
    Dim gpi As Integer
    Dim gdat(40, 300), gcnt(40), Grafind, GIT As Double                                       'Graph Data
    Dim AON(40) As Integer                                                                     'Alarm by gage
    Dim starts(10) As String    'Start Comandline Strings
    Dim calfile As String
    Dim rpm1, rpm2, load1, load2, RPMload, MANLoad, dGR, PullDur As Single     'Pull Data    
    Dim chan As Integer
    Dim compt, comBR, saveint As Integer
    Dim tagline, dynotype As String
    Dim frictoff, frictofft, frictquadt As Double   'Frictional offset and Temporary value
    Dim frictfact, frictfactt, FrictQuad As Double 'Frictional Factor and Temporary Value
    Dim diam, rpmdynomax, TorqDynoMax, inert, frictof, deltaT, GR, inerteng As Single
    Dim logo, loadMode, pGrafAnaCh As Integer
    Dim pullcom(10, 4), sendcmd, ann As String
    Dim peng2 As Single
    Dim DataPath, runfile As String
    Dim lastcal, comstr, CPStr, serports, rundatafile As String
    ' Dim sCHT, sCHP, sCHR As String
    Dim saetemp, saepress, saeRH, SAEcf As Single                                             'Comp Factor
    Dim saeauto, saecomp, metric As Boolean
    Dim vehID, ver, mode As String
    Dim powmax, speedAF As Single
    Dim run, pullmode, ppr1, ppr2, savecnt, plm As Integer
    Dim disL(50), disT(50), disW(50), disH(50), disG(50) As Integer ' Display Items Location, Size and Gage Type
    Dim ghide(50), gSUdone, runsave As Boolean        'Hidden gage (for deleating)
    Dim os1, os2, load, torqraw, torqd, torqe, ai(8), acell, TorqTot As Single      'Input parameters
    Dim time, timeo, timeav, tstart, timepull As Double                               'Time tracking 
    Public timelaps As Double                                                      'Duration of Pull
    Dim pload, pullmaxrpm As Single
    Dim dataindex, Calch As Integer
    Public y1graf, y2graf, y3graf, y4graf, points, xgraf As Integer             '8888 need to add values for 2nd graf
    Public pulldata(4, 3000, 30) As Single                        'pull data for saving to file
    Dim ds(30) As RadioButton
    Public ls(20) As ComboBox
    Dim calupdate As Boolean
    Dim p2x, p2y, p2ox, p2oy, fx, fy As Integer           'Size of pannel 2 and form
    Dim ContCal(100), ccc(100), ccz As Integer            'Contoller Calibration Factors, and count of controller cals that are zero
    Dim serenab As Boolean                                'Enable/dissable Setial Coms
    Dim distnc As Single                                  'Distance from start of pull
    Public dic, Alarm As Integer                          'Data index Counter (for pulls data), Alarm
    Dim frictoft, speed1avgt, ietemp As Single

    Dim tempstr2, ShopName, ShopCont As String
    Dim FACon As Boolean
    'Dim frictofft As Double   'Frictional offset and Temporary value
    ' Dim Fauto(2, 10001) As Double        'Frictional spin down (rpm, frict) for Auto Cal
    ' Dim FAC, FACi, FACiFmin, FACi90, FACi10, temp As Integer            'Frictional AUto Cal Counters

    Dim AFRPM, x1, x2, x3, y1, y2, y3 As Double                  'Auto Friction RPMs and Torques
    Dim FAC, AFAV1, afav2, afav3 As Integer                           'Auto Frcit averages
    Dim Fmin, rpmin, rpm90, rpm10, fer90, fer10 As Double
    Dim AF1, AF4, AF5 As Double
    Dim brek2p, brekstop, brekramp, brekon, remote, loadon, remoff As Boolean
    Dim BREKload, LB1 As Single                                  '% Load for Breaking
    Dim RemV, lastrem As Double                             'Last Value of Remote
    Dim remch As Integer                                    'Remote Input Channel
    Dim prevmode As String                                  'Previous Mode box Text
    Dim LastLMode, lastload, testc As Integer               'Prev Load Mode
    Dim lang As Integer                                     '0 = English, 1 = Chineese, 2 = Vietnamese
    Dim modes(5), alarms(5), rembreks(5), remloads(5), modmans(5), modspeeds(5), commodes(5), ratios(5) As String
    Dim AutoSave, autograf, instgraf, autoload As Boolean           'Remote Enter Button (turned ON only when in dialog box requiring use)
    Dim saveresult = DialogResult.OK
    Dim butcol1, butcol0, butcol2 As Color
    Dim rpmode As Integer                                   'Pull RPM Mode: 1=Dyno, 2=Engine Clac, 3=RPM2
    Dim rpms(5, 3) As String                                'RPM Reference
    Dim prpm1, prpm2, pacel, pspeed, opspeed, Ops1 As Double               'Pull parameters
    Dim KPa As Single                                       'Acelleraition gain Proportional
    Dim ofspic, orgpic As Rectangle
    Dim tempbmp As Bitmap
    Dim autotare As Boolean
    Dim taretime, tareclick As Double                                  'Time of last tare
    Dim refresh, lastref As Single                                   'Screen Refresh rate (sec)
    Dim scrnref As Boolean                                          'Bit to show when to refresh in pull
    Dim PowUnit As Integer                                       'Units of Power in kW (0), Hp (1)
    Public GraFilt As Integer
    Dim tlastload As Single                                     'Time of last load command (during pull)
    Dim logon As Boolean                                        'Logging ON or not?
    Dim testb(100) As Integer                                  'Test Byte for Controller Cals
    Dim KMt As Single                                           'Total KM 
    Dim instaClick As Single                                'Time of last Instaclick
    Dim play As Boolean                                     '"Play" mode (instagraf ON but no pull logging)
    Public GrafTop, GrafLeft, GrafWidth, GrafHeight As Integer          'Graf form position and size
    Dim INSTTop, INSTLeft, INSTWidth, INSTHeight As Integer          'Graf form position and size
    Dim LowSpeedPostPull As Single                                          'Lowest speed since post pull (to trigger re-pull)
    Public GManX, GmanY, Gman2Y As Boolean                             'Man (vs. Auto) Graf Axis controls
    Public GrX1, GrY2, Gr2Y2, InstXmin As Single                                 'Graphing X interval, and Y, Y2 Maximum's, Instagraf X min
    Public GRAFx, GRAFy, GRAFh, GRAFw As Integer                              'X, Y offsets of GRAF screen, and H, W

    Public Cskin, Cback As Integer
    Public ColSkin(10), ColBack(10) As Color     'Colors to highlite buttons on/off
    Public MAG As Single                        'Screen Magnification caftor (for Jpgs)
    Public grafback As New NamedImage("GRBack", My.Resources.GRBack3)
    Public Col(20) As Color                                          'Graf Trace Colors
    Public Tcol(20) As Integer                                         'Color of the various graf traces
    Dim Tfrict As Single                                     'Torque w/ w/o friction
    Public AnaRef As Single                                        'Analog Graf Reference Line



    Public Sub New() ' This call is required by "the designer", (whatever that means). Basically you set up the initial defaults here. 
        InitializeComponent()
        rs.FindAllControls(Me) 'For form autoresize
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        ' Me.SizeGripStyle = SizeGripStyle.
        Dim starts As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs

        ColSkin(0) = Color.Silver
        ColSkin(1) = Color.Tan
        ColSkin(2) = Color.Turquoise
        ColSkin(3) = Color.Thistle
        ColSkin(4) = Color.DarkKhaki

        ColBack(0) = Color.Black
        ColBack(1) = Color.DarkBlue
        ColBack(2) = Color.DarkRed
        ColBack(3) = Color.DarkGreen
        ColBack(4) = Color.Yellow

        butcol1 = Color.LightGreen
        butcol0 = Color.LightGray
        butcol2 = Color.PaleVioletRed

        Col(1) = Color.Black
        Col(2) = Color.Navy
        Col(3) = Color.DarkRed : Tcol(2) = 3 : Tcol(6) = 2
        Col(4) = Color.DarkGreen
        Col(5) = Color.Goldenrod
        Col(6) = Color.Indigo : Tcol(4) = 6 : Tcol(8) = 6
        Col(7) = Color.Gray
        Col(8) = Color.Blue : Tcol(1) = 8 : Tcol(5) = 8
        Col(9) = Color.Red
        Col(10) = Color.Green : Tcol(3) = 10 : Tcol(7) = 10
        Col(11) = Color.Yellow
        Col(12) = Color.DarkOrchid
        Col(13) = Color.Silver
        Col(14) = Color.DodgerBlue
        Col(15) = Color.Salmon
        Col(16) = Color.Lime
        Col(17) = Color.Peru
        Col(18) = Color.Plum

        ds(1) = dispsel.RadioButton1
        ds(2) = dispsel.RadioButton2
        ds(3) = dispsel.RadioButton3
        ds(4) = dispsel.RadioButton4
        ds(5) = dispsel.RadioButton5
        ds(6) = dispsel.RadioButton6
        ds(7) = dispsel.RadioButton7
        ds(8) = dispsel.RadioButton8
        ds(9) = dispsel.RadioButton9
        ds(10) = dispsel.RadioButton10
        ds(11) = dispsel.RadioButton11
        ds(12) = dispsel.RadioButton12
        ds(13) = dispsel.RadioButton13
        ds(14) = dispsel.RadioButton14
        ds(15) = dispsel.RadioButton15
        ds(16) = dispsel.RB16           'E1
        ds(17) = dispsel.RB17           'E2
        ds(18) = dispsel.RB18           'E3
        ds(19) = dispsel.RB19           'E4
        ds(20) = dispsel.RB20           'E5
        ds(21) = dispsel.RB21           'E6
        ds(22) = dispsel.RB22           'E7
        ds(23) = dispsel.RB23           'E8
        ds(24) = dispsel.RB24           'E9
        ds(25) = dispsel.RB25           'E10
        ds(26) = dispsel.RB26           'E11
        ds(27) = dispsel.RB27           'E12
        ds(28) = dispsel.RadioButton16

        ls(1) = GrafSet.LB1
        ls(2) = GrafSet.LB2
        ls(3) = GrafSet.LB3
        ls(4) = GrafSet.LB4
        ls(5) = GrafSet.LB5
        ls(6) = GrafSet.LB6
        ls(7) = GrafSet.LB7
        ls(8) = GrafSet.LB8
        ls(9) = GrafSet.LB15
        ls(10) = GrafSet.LB19

        '   For i = 1 To 8
        ' ls(i).Items.Add("Time")
        ' ls(i).Items.Add("RPM1 - Dyno")
        ' ls(i).Items.Add("RPM2 - Engine")
        ' ls(i).Items.Add("Eng Calc RPM")
        ' ls(i).Items.Add("Raw Dyno Torque")
        ' ls(i).Items.Add("Total Dyno Torque")
        ' ls(i).Items.Add("Engine Torque")
        ' ls(i).Items.Add("Power (kW)")
        ' ls(i).Items.Add("Power (hp)")
        ' ls(i).Items.Add("Load (%)")
        ' ls(i).Items.Add("Analog 1")
        ' ls(i).Items.Add("Analog 2")
        ' ls(i).Items.Add("Analog 3")
        ' ls(i).Items.Add("Analog 4")
        ' Next


        Cals.ComboBox1.Items.Add("RPM1 - Dyno")
        Cals.ComboBox1.Items.Add("RPM2 - Engine")
        Cals.ComboBox1.Items.Add("Eng Calc RPM")
        Cals.ComboBox1.Items.Add("Raw Dyno Torque")
        Cals.ComboBox1.Items.Add("Total Dyno Torque")
        Cals.ComboBox1.Items.Add("Engine Torque")
        Cals.ComboBox1.Items.Add("Power (kW)")
        Cals.ComboBox1.Items.Add("Power (hp)")
        Cals.ComboBox1.Items.Add("Load (%)")
        Cals.ComboBox1.Items.Add("Analog 1")
        Cals.ComboBox1.Items.Add("Analog 2")
        Cals.ComboBox1.Items.Add("Analog 3")
        Cals.ComboBox1.Items.Add("Analog 4")
        Cals.ComboBox1.Items.Add("Distance")
        Cals.ComboBox1.Items.Add("Linear Speed")


        Cals.ComboBox1.Items.Add("ECU/OBD1") : OBDCB(1) = FOBD.CB1
        Cals.ComboBox1.Items.Add("ECU/OBD2") : OBDCB(2) = FOBD.CB2
        Cals.ComboBox1.Items.Add("ECU/OBD3") : OBDCB(3) = FOBD.CB3
        Cals.ComboBox1.Items.Add("ECU/OBD4") : OBDCB(4) = FOBD.CB4
        Cals.ComboBox1.Items.Add("ECU/OBD5") : OBDCB(5) = FOBD.CB5
        Cals.ComboBox1.Items.Add("ECU/OBD6") : OBDCB(6) = FOBD.CB6
        Cals.ComboBox1.Items.Add("ECU/OBD7") : OBDCB(7) = FOBD.CB7
        Cals.ComboBox1.Items.Add("ECU/OBD8") : OBDCB(8) = FOBD.CB8
        Cals.ComboBox1.Items.Add("ECU/OBD9") : OBDCB(9) = FOBD.CB9
        Cals.ComboBox1.Items.Add("ECU/OBD10") : OBDCB(10) = FOBD.CB10
        Cals.ComboBox1.Items.Add("ECU/OBD11") : OBDCB(11) = FOBD.CB11
        Cals.ComboBox1.Items.Add("ECU/OBD12") : OBDCB(12) = FOBD.CB12

        OBDcmd(1) = "04" : OBDBytes(1) = 1 : OBDisLab(1) = dispsel.LO1 'Engine Load
        OBDcmd(2) = "05" : OBDBytes(2) = 1 : OBDisLab(2) = dispsel.LO2   'Coolent Temp
        OBDcmd(3) = "0B" : OBDBytes(3) = 1 : OBDisLab(3) = dispsel.LO3 'MAP
        OBDcmd(4) = "0C" : OBDBytes(4) = 2 : OBDisLab(4) = dispsel.LO4  'RPM
        OBDcmd(5) = "0D" : OBDBytes(5) = 1 : OBDisLab(5) = dispsel.LO5  'kph
        OBDcmd(6) = "0E" : OBDBytes(6) = 1 : OBDisLab(6) = dispsel.LO6  'Spark Timing
        OBDcmd(7) = "0F" : OBDBytes(7) = 1 : OBDisLab(7) = dispsel.LO7 'Air Temp
        OBDcmd(8) = "10" : OBDBytes(8) = 2 : OBDisLab(8) = dispsel.LO8 'MAF
        OBDcmd(9) = "11" : OBDBytes(9) = 1 : OBDisLab(9) = dispsel.LO9 'Throttle
        OBDcmd(10) = "14" : OBDBytes(10) = 2 : OBDisLab(10) = dispsel.LO10 'O2-1 
        OBDcmd(11) = "15" : OBDBytes(11) = 2 : OBDisLab(11) = dispsel.LO11 'O2-2
        OBDcmd(12) = "44" : OBDBytes(12) = 2 : OBDisLab(12) = dispsel.LO12 'Lambda
        OBDtime = 0.15               'Ping rate. Some systems have trouble at 0.1 Sec but are OK at 0.2 sec

        modes(0) = "SELECT LOAD MODE"
        alarms(0) = "       = = =   A L A R M   = = ="
        rembreks(0) = "REMOTE BRAKE"
        remloads(0) = "REMOTE LOAD OFF"
        modmans(0) = "MANUAL LOAD"
        modspeeds(0) = "SPEED LOAD"
        commodes(0) = "Put controller in COM mode"
        ratios(0) = "Ratio "
        rpms(0, 1) = "RPM: DYNO"
        rpms(0, 2) = "RPM: GR"
        rpms(0, 3) = "RPM: ENG"

        modes(1) = "选择加载模式"
        alarms(1) = "       = = =   报警   = = ="
        rembreks(1) = "遥控刹车"
        remloads(1) = "远程卸载"
        modmans(1) = "手动负载"
        modspeeds(1) = "速度负载"
        commodes(1) = "将控制器置于COM模式"
        ratios(1) = "比 "
        rpms(1, 1) = "RPM: DYNO"
        rpms(1, 2) = "RPM: GR"
        rpms(1, 3) = "RPM: ENG"

        modes(2) = "CHỌN CHẾ ĐỘ TẢI"
        alarms(2) = "       = = =   BÁO THỨC   = = ="
        rembreks(2) = "PHANH TỪ XA"
        remloads(2) = "TẢI TỪ XA TẮT"
        modmans(2) = "HƯỚNG DẪN TẢI"
        modspeeds(2) = "TẢI TỐC ĐỘ"
        commodes(2) = "Đặt bộ điều khiển ở chế độ COM"
        ratios(2) = "Tỉ lệ "
        rpms(2, 1) = "RPM: DYNO"
        rpms(2, 2) = "RPM: GR"
        rpms(2, 3) = "RPM: ENG"


        For i = 1 To 40
            ghide(i) = False
            l(i) = New Label
            Me.Controls.Add(l(i))
            l(i).TextAlign = ContentAlignment.TopCenter
            l(i).AutoSize = False
            l(i).Font = New Font("Arial", 8, FontStyle.Bold)
            l(i).ForeColor = Color.DarkRed
            l(i).BackColor = Color.Transparent
            l(i).Left = 100 + i * 50
            l(i).Top = 100
            l(i).Text = "Indicator"
            l(i).BringToFront()
            l(i).Hide()

            n(i) = New Label
            Me.Controls.Add(n(i))
            ' n(i).TextAlign = ContentAlignment.MiddleCenter
            n(i).AutoSize = False
            n(i).Font = New Font("Arial", 16, FontStyle.Bold)
            n(i).ForeColor = Color.LimeGreen
            n(i).BackColor = Color.Transparent
            n(i).TextAlign = ContentAlignment.TopCenter
            n(i).Height = 25
            n(i).Text = "Indicator"
            n(i).BringToFront()
            n(i).Hide()
            AddHandler n(i).Click, AddressOf numclick                  'Add Click ability to the large number display (as they cover so much space)
        Next
        gSUdone = False
        calupdate = False


        For i = 1 To 40
            gage(i) = New PictureBox
            grf(i) = New PictureBox
            gage(i).Image = My.Resources.Gage_3                     'Start off as a gage
            grf(i).Image = My.Resources.GrafBlnk
            AddHandler gage(i).Click, AddressOf gageclick
            AddHandler grf(i).Click, AddressOf grfclick
            AddHandler gage(i).Paint, AddressOf Me.Gage_Paint
            AddHandler FOBD.OBDclr, AddressOf Me.obdclearerr
            AddHandler FOBD.OBDchk, AddressOf Me.OBDCheckErr
            AddHandler OBDport.DataReceived, AddressOf Me.obdprocess
        Next
        AddHandler CB2Point.CheckedChanged, AddressOf Me.CB2Point_CheckedChanged
        AddHandler CBAcel.CheckedChanged, AddressOf Me.CBAcelCh
        'AddHandler CBMan.CheckedChanged, AddressOf Me.CBManChanged
        'AddHandler CBRPM.CheckedChanged, AddressOf Me.CBRPMCh
        'AddHandler ButtSpeedup.Click, AddressOf Me.Speedup
        'AddHandler ButSpeeddn.Click, AddressOf Me.SpeedDn
        For dic = 1 To 4
            fgraf.Chart1.Series.Add("Torq" + Str(dic))
            fgraf.Chart1.Series(2 * dic - 2).BorderWidth = 3
            fgraf.Chart1.Series(2 * dic - 2).IsVisibleInLegend = False
            fgraf.Chart1.Series(2 * dic - 2).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
            fgraf.Chart1.Series(2 * dic - 2).Color = Color.Blue

            fgraf.Chart1.Series.Add("Power" + Str(dic))
            fgraf.Chart1.Series(2 * dic - 1).BorderWidth = 1
            fgraf.Chart1.Series(2 * dic - 1).IsVisibleInLegend = False
            fgraf.Chart1.Series(2 * dic - 1).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
            fgraf.Chart1.Series(2 * dic - 1).Color = Color.DarkRed

            fgraf.Chart2.Series.Add("AFR" + Str(dic))
            fgraf.Chart2.Series(2 * dic - 2).BorderWidth = 3
            fgraf.Chart2.Series(2 * dic - 2).IsVisibleInLegend = False
            fgraf.Chart2.Series(2 * dic - 2).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
            fgraf.Chart2.Series(2 * dic - 2).Color = Color.Black
            fgraf.Chart2.Series.Add("AFR" + Str(dic) + "-2")
            fgraf.Chart2.Series(2 * dic - 1).BorderWidth = 3
            fgraf.Chart2.Series(2 * dic - 1).IsVisibleInLegend = False
            fgraf.Chart2.Series(2 * dic - 1).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
            fgraf.Chart2.Series(2 * dic - 1).Color = Color.Black
        Next
        fgraf.Chart1.ChartAreas(0).AxisX.Minimum = Double.NaN
        fgraf.Chart1.ChartAreas(0).AxisX.LabelStyle.Format = "N0"
        fgraf.Chart1.ChartAreas(0).AxisY.Minimum = 0
        fgraf.Chart1.ChartAreas(0).AxisX.Maximum = Double.NaN
        fgraf.Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
        fgraf.Chart2.ChartAreas(0).AxisX.Minimum = Double.NaN
        fgraf.Chart2.ChartAreas(0).AxisX.Maximum = Double.NaN
        fgraf.Chart2.ChartAreas(0).AxisY.Minimum = 0
        fgraf.Chart2.ChartAreas(0).AxisY.Maximum = Double.NaN
        fgraf.Chart2.ChartAreas(0).AxisX.LabelStyle.Format = "N0"
        fgraf.Location = New Point(0, 0)


        GLOfx(1) = 0.01 : GLOfy(1) = 0.02 '      1=Chart   2=Graph   3=LED   4=Vert  5=Horz      6=Number
        GLOfx(2) = 0.5 : GLOfy(2) = 0.02 '           These are offsets for Text location on the display item (from top left)
        GLOfx(3) = 0.5 : GLOfy(3) = 0.05
        GLOfx(4) = 0.5 : GLOfy(4) = 0.02
        GLOfx(5) = 0.5 : GLOfy(5) = 0.03
        GLOfx(6) = 0.5 : GLOfy(6) = 0.03
        GLOfx(7) = 0.5 : GLOfy(7) = 0.65

        GRAFx = 10          'Default Graf position, height
        GRAFy = 10
        GRAFh = 700
        GRAFw = 1000

        gagemax = 0
        Call defaults()                     'Fisrt: Set up defaults to get any "new" parameters in by default

        If starts.Count > 0 Then
            If starts(0) = "Chose" Or starts(0) = "CHOSE" Or starts(0) = "chose" Then
                'Open "OPen Cal File" box
                Dim LoadFileBox As New OpenFileDialog
                With LoadFileBox
                    .FileName = calfile
                    .DefaultExt = "CSV"
                    .Filter = "CSV|*.csv"
                    .Title = "Load Calibration File"
                    '  .InitialDirectory = applicationSettingsDirectory
                    Dim result = .ShowDialog()
                    If result = DialogResult.OK Then
                        calfile = .FileName
                    End If
                End With
            Else
                calfile = starts(0)
            End If
        End If
        Call loadcals()                                 'This was erroneously placed within the If statement above, not exacuting properly 

        comstr = "COM" + LTrim(Str(compt))
        COMport.PortName = comstr
        If comBR = 0 Then comBR = 9600 'OPen Dyno Controller Com Port: Can use USB com ports like internal ones if proper driver installed
        COMport.BaudRate = comBR
        ' COMport.BaudRate = 115200
        COMport.ReadBufferSize = 128

        If COMport.IsOpen Then COMport.Close()
        Try
            COMport.Open()
            Sleep(100)
            COMport.WriteLine("M0")                'Set to ZERO load 
        Catch ex As Exception                       'This only allows 1 retry!!!

            serports = ""
            For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                serports = serports + sp + ", "
            Next
            CPStr = InputBox(comstr + " Does Not Exist." + Chr(13) + "Available Ports: (0 = offline), " + serports, "Enter New Com Port #", 1, 5, 5)
            If CPStr <> "" Then
                compt = Val(CPStr)
                comstr = "COM" + LTrim(Str(compt))
                '     If PortIsAvailable(comstr) Then
                COMport.PortName = comstr    'OPen Dyno Controller Com Port: NOTE Can use USB com ports just like internal ones!
                CPStr = InputBox("Baud Rate: " + Str(comBR) + Chr(13) + "Typically 9600 or 115200" + Chr(13) + "Enter Dyno Controller Com Baud Rate ", , Str(comBR))
                If CPStr <> "" Then comBR = Val(CPStr)
                COMport.BaudRate = comBR
                If compt <> 0 Then COMport.Open() '0 for offline (and Simulation)
            Else
                End     'If they cancel, it's cuase they dont wanna run. 
            End If
        End Try

        If COMport.IsOpen Then COMport.WriteLine("M0") 'Set to ZERO load 


        Comp.Chart1.Images.Add(grafback)

        Comp.Chart1.ChartAreas(0).BackImage = "GRBack"              'Is this like the STUPIDEST thing in the world, or what?!
        Comp.Chart1.ChartAreas(0).BackImageAlignment = ImageLayout.Stretch
        Comp.Chart1.ChartAreas(0).BackImageWrapMode = ChartImageWrapMode.Scaled 'ImageWrapMode.Stretch

        BG1.BackgroundImageLayout = ImageLayout.Stretch     'Icon button images
        BG2.BackgroundImageLayout = ImageLayout.Stretch
        BG3.BackgroundImageLayout = ImageLayout.Stretch
        BG4.BackgroundImageLayout = ImageLayout.Stretch
        BG6.BackgroundImageLayout = ImageLayout.Stretch
        BG5.BackgroundImageLayout = ImageLayout.Stretch

        PBcoms.Image = My.Resources.RED_LED
        Label5.Parent = PBcoms
        Label5.Location = New Point(3, 3)       'Otherwise it is moves out of sight!
        Label5.BackColor = Color.Transparent
        Label5.Show()
        Label5.BringToFront()
        LG.Text = "Display Items:"
        Timer1.Interval = 300                     'Set timer for 1 sec
        Timer1.Start()
        tstart = Now.ToOADate
        serenab = True
        Comp.first = True                         '!st time to setup grafs
        dic = 0                                       'Start with 1st position (for pull data graphs)
        loadon = True                                 'Allow loading (dissallowe with Remote Load switch)
        testc = 0
        TFirst = True                                 '1st time when running Tune Buddy
        OBDon = False
        OBDcnt = 0
        OBDskp = 4                                    '2 = Ping every other cycle
        Me.BackColor = ColSkin(Cskin)
        Panel2.BackColor = ColBack(Cback)

        ' Auto-launch Modern WPF Cyberpunk Dashboard
        Try
            If wpfDash Is Nothing OrElse Not wpfDash.IsLoaded Then
                wpfDash = New DynoDashboard()
            End If
            wpfDash.Show()
        Catch ex As Exception
        End Try

    End Sub
    Private Sub defaults()
        ver = "Default"
        calfile = "C:\Dynomas\UniDyn-Cal.CSV"
        runfile = "C:\Dynomas\DynoRun"
        DataPath = "C:\Dynomas"
        vehID = "ID"

        INSTTop = 10
        INSTLeft = 10
        INSTWidth = 893
        INSTHeight = 430
        MAG = 1
        KPa = 0.2                      'Acelleration Gain Proportional

        refresh = 0.1                     '100ms screen refresh

        rpmode = 1                    'Dyno
        lang = 0                    'English
        remch = 3                 'Analog 3 for Remote
        logo = 0                  '(FAT)
        dGR = 1
        PullDur = 15                  '15 sec pulls
        xgraf = 2               'One more than the label index (as we add time as the 0th option)
        y1graf = 6
        y2graf = 8
        y3graf = 11
        y4graf = 12

        powmax = 0
        run = 0               ' no runs yet
        Calch = 1                 'Calibration Channel number

        pullcom(1, 0) = "IDLE: Increase speed"
        pullcom(2, 0) = "LOADING: Stady at start"
        pullcom(3, 0) = "PULL: WIDE OPEN THROTTLE"
        pullcom(4, 0) = "UNLOADING: Return to IDLE"
        pullcom(5, 0) = "Return to IDLE"

        pullcom(1, 1) = "空闲：提高速度"
        pullcom(2, 1) = "负载增加:   在启动rpm时稳定"
        pullcom(3, 1) = "拉动:     加速节气门"
        pullcom(4, 1) = "正在卸载：返回空闲"
        pullcom(5, 1) = "返回空闲"

        pullcom(1, 2) = "IDLE: Tăng tốc độ"
        pullcom(2, 2) = "TĂNG TẢI: Ổn định ở vòng tua máy bắt đầu"
        pullcom(3, 2) = "PULL: Tăng tốc toàn bộ sức mạnh"
        pullcom(4, 2) = "GIẢM TẢI: Quay lại IDLE"
        pullcom(5, 2) = "Quay lại IDLE"

        pullmode = 0
        rpmdynomax = 4500                 'Max RPM of Dyno
        TorqDynoMax = 100
        ppr1 = 30
        ppr2 = 1
        GR = 4
        frictof = 0
        frictfact = 0
        frictquad = 0
        lastcal = "N/A"
        dynotype = "Unknown"
        diam = 0.3
        inert = 0
        compt = 3
        comBR = 115200          '9600

        brekload = 10
        brekramp = True
        brekstop = True
        brek2p = True
        brekon = False

        For i = 1 To 27
            alab(i) = "CH" + Str(i) : auni(i) = "Unit" : amin(i) = 0 : amax(i) = 100 : afilt(i) = 0 : Afact(i) = 1 : aof(i) = 0 : adec(i) = "N0" : amajtix(i) = 10 : amintix(i) = 1
        Next
        alab(0) = "Time" : auni(0) = "sec" : amin(0) = 0 : amax(0) = 20 : amajtix(0) = 10 : amintix(0) = 2 : adec(0) = "N1"
        alab(1) = "Dyno rpm" : auni(1) = "RPM" : amin(1) = 0 : amax(1) = rpmdynomax : amajtix(1) = 5 : amintix(1) = 5
        alab(2) = "Engine rpm" : auni(2) = "RPM" : amin(2) = 0 : amax(2) = 12000 : amajtix(2) = 10 : amintix(2) = 1
        alab(3) = "Eng Calc" : auni(3) = "RPM" : amin(3) = 0 : amax(3) = 12000 : amajtix(3) = 10 : amintix(3) = 1
        alab(4) = "Dyno Torque" : auni(4) = "Nm" : amax(4) = TorqDynoMax : adec(4) = "N1"
        alab(5) = "Total Torque" : auni(5) = "Nm" : amax(5) = TorqDynoMax * 2 : adec(5) = "N1"
        alab(6) = "Engine Torq" : auni(6) = "Nm" : amax(6) = 50 : adec(6) = "N2"
        alab(7) = "Power" : auni(7) = "kW" : Afact(7) = 1 : amax(7) = TorqDynoMax * 2 * rpmdynomax / 1000 : adec(7) = "N1"
        alab(8) = "Power" : auni(8) = "hp" : Afact(8) = 1 : amax(8) = TorqDynoMax * 2 * rpmdynomax / 750 : adec(8) = "N1"
        alab(9) = "Load" : auni(9) = "%" : adec(9) = "N1"
        alab(10) = "Throttle" : auni(10) = "%" : adec(10) = "N1"
        alab(11) = "AFR (A1)" : auni(11) = "%" : aof(11) = 8 : Afact(11) = 3 : amax(11) = 23 : adec(11) = "N1"
        alab(12) = "Analog Input 2" : auni(12) = "V" : amax(12) = 5 : adec(12) = "N2"
        alab(13) = "Analog Input 3" : auni(13) = "V" : amax(13) = 5 : adec(13) = "N2"
        alab(14) = "Analog Input 4" : auni(14) = "V" : amax(14) = 5 : adec(14) = "N2"
        alab(15) = "Linear Speed" : auni(15) = "kph" : amin(15) = 0 : amax(15) = 200 : amajtix(15) = 10 : amintix(15) = 3 : adec(15) = "N1"
        alab(16) = "Distance" : auni(16) = "m" : amin(16) = 0 : amax(16) = 2000 : amajtix(16) = 10 : amintix(16) = 3 : adec(16) = "N1"

        alab(17) = "ECU/OBD1" : auni(17) = "%" : amax(17) = 120 : adec(17) = "F0"
        alab(18) = "ECU/OBD2" : auni(18) = "C" : amax(18) = 120 : adec(18) = "F0"
        alab(19) = "ECU/OBD3" : auni(19) = "kPa" : amax(19) = 110 : adec(19) = "F0"
        alab(20) = "ECU/OBD4" : auni(20) = "rpm" : amax(20) = 12000 : adec(20) = "F0"
        alab(21) = "ECU/OBD5" : auni(21) = "kpm" : amax(21) = 300 : adec(21) = "F1"
        alab(22) = "ECU/OBD6" : auni(22) = "deg" : amax(22) = 45 : adec(22) = "F1"
        alab(23) = "ECU/OBD7" : auni(23) = "C" : amax(23) = 80 : adec(23) = "F0"
        alab(24) = "ECU/OBD8" : auni(24) = "g/s" : amax(24) = 400 : adec(24) = "F1"
        alab(25) = "ECU/OBD9" : auni(25) = "%" : amax(25) = 100 : adec(25) = "F1"
        alab(26) = "ECU/OBD10" : auni(26) = "V" : amax(26) = 1.2 : adec(26) = "F2"
        alab(27) = "ECU/OBD11" : auni(27) = "V" : amax(27) = 1.2 : adec(27) = "F2"
        alab(28) = "ECU/OBD12" : auni(28) = "kPa" : amax(28) = 5000 : adec(28) = "F1"


        deltaT = 0.1       'Set  between 4Hz and 20Hz so it'll converge
        timeav = deltaT

        saetemp = 25
        saepress = 101.3
        saeRH = 50
        saeauto = False                         'Assume no weather station
        saveint = 1                             'Save ALL Data as default
        savecnt = 0

        PowUnit = 0                            'Metric
        tagline = "For More Information Visit: WWW.FocusAppliedTechnologies.com"
        powmax = 0
        'Use Dyno speed as default
        plm = 5
        'Pick Default Location
        peng2 = False

        speedAF = 0.5
        Logging.Checked = False                  'Turn logging OFF to avoid opening in a non-existant location

        TBMode.Text = modes(lang)
        autotare = True                           'Turn OFF auto tare\
        AutoSave = True
        play = False                              'Not in Testing Mode
        'MsgBox("Def")
        gagemax = 11                                'Give 'em a reasonable starting screen
        gtype(1) = 0 : Gch(1) = 1 : gage(1).Left = 110 : gage(1).Top = 31 : gage(1).Width = 321 : gage(1).Height = 296
        gtype(2) = 0 : Gch(2) = 1 : gage(2).Left = 110 : gage(2).Top = 31 : gage(2).Width = 461 : gage(2).Height = 373
        gtype(3) = 6 : Gch(3) = 6 : gage(3).Left = 435 : gage(3).Top = 103 : gage(3).Width = 405 : gage(3).Height = 95
        gtype(4) = 7 : Gch(4) = 5 : gage(4).Left = 838 : gage(4).Top = 3 : gage(4).Width = 421 : gage(4).Height = 375
        gtype(5) = 6 : Gch(5) = 14 : gage(5).Left = 435 : gage(5).Top = 3 : gage(5).Width = 405 : gage(5).Height = 95
        gtype(6) = 7 : Gch(6) = 1 : gage(6).Left = 0 : gage(6).Top = 0 : gage(6).Width = 443 : gage(6).Height = 374
        gtype(7) = 6 : Gch(7) = 15 : gage(7).Left = 435 : gage(7).Top = 288 : gage(7).Width = 405 : gage(7).Height = 95
        gtype(8) = 6 : Gch(8) = 9 : gage(8).Left = 435 : gage(8).Top = 198 : gage(8).Width = 405 : gage(8).Height = 95
        gtype(9) = 5 : Gch(9) = 15 : gage(9).Left = 4 : gage(9).Top = 373 : gage(9).Width = 428 : gage(9).Height = 65
        gtype(10) = 5 : Gch(10) = 2 : gage(10).Left = 840 : gage(10).Top = 377 : gage(10).Width = 414 : gage(10).Height = 64
        gtype(11) = 3 : Gch(11) = 13 : gage(11).Left = 582 : gage(11).Top = 375 : gage(11).Width = 66 : gage(11).Height = 72

        lastref = Int(8640000 * (Now.ToOADate - tstart)) / 100 - refresh       'Set next screen refresh to now

        TBMan.Text = "0"
        pullmode = 1
        CBMan.Checked = True
    End Sub
    Private Sub gagemove()                                                  'This was only seperated out so they can play with the screen setup offline
        Dim gavfact As Single
        If gmove = True Then
            gage(gind).Left = MousePosition.X - gmx 'or whatever
            gage(gind).Top = MousePosition.Y - gmy
            If gage(gind).Left < 0 Then gage(gind).Left = 0 'Dont moce beyond edge of screen.
            If gage(gind).Top < 0 Then gage(gind).Top = 0
        End If
        If gstrech = True Then
            Select Case gtype(gind)
                Case 1                                          'Nedel gage
                    gage(gind).Width = MousePosition.X - gage(gind).Left + GMXL
                    gage(gind).Height = gage(gind).Width / 2
                    'gavfact = ((MousePosition.X - gage(gind).Left) + (MousePosition.Y - gage(gind).Top)) / 2
                    'If gavfact < 0.1 Then gavfact = 0.1
                    'If gavfact > 10 Then gavfact = 10
                    'gage(gind).Width = gavfact * 1.5 + GMXL
                    'gage(gind).Height = gavfact * 0.75 + GMYL
                Case 7                                          'Bulat gage
                    ' gavfact = gage(gind).Width/gage(gind)
                    gage(gind).Width = MousePosition.X - gage(gind).Left + GMXL
                    gage(gind).Height = gage(gind).Width 'MousePosition.Y - gage(gind).Top + GMYL
                    '    If gavfact < 0.1 Then gavfact = 0.1
                    '    If gavfact > 10 Then gavfact = 10
                    '    gage(gind).Width = Math.Abs(gavfact + GMXL)
                    '    gage(gind).Height = Math.Abs(gavfact + GMYL)
                    '    Console.WriteLine("Set W/H: " + gage(gind).Width.ToString + ", " + gage(gind).Height.ToString)
                Case Else                                   'All other gages
                    gage(gind).Width = MousePosition.X - gage(gind).Left + GMXL
                    gage(gind).Height = MousePosition.Y - gage(gind).Top + GMYL
            End Select
   

        End If
    End Sub
    Private Sub main() '########################################################################## M A I N #################################################
        'NOTE: Although this is supposed to trigger once per "ping" from the controller, it looks like sometimes it gets called multiple times... that can mess up the calcs if you "add" in a value, it'll get it multiple times
        'tempbmp = New Bitmap(PictureBox1.Width, PictureBox1.Height)
        'Dim tgr As Graphics = Graphics.FromImage(tempbmp)
        'orgpic = New Rectangle(1, 0, PictureBox1.Width, PictureBox1.Height)
        'ofspic = New Rectangle(0, 0, PictureBox1.Width, PictureBox1.Height)
        'tgr.DrawImage(PictureBox1.Image, ofspic, orgpic, GraphicsUnit.Pixel)
        'PictureBox1.Image = tempbmp

        Label3.Text = Now.ToString

        time = Int(8640000 * (Now.ToOADate - tstart)) / 100           'Calculate time elapsed in seconds fractional
        If time < timeo Then timeo = timeo - 24 * 3600 'Compensate for 24 hour rollover
        If Math.Abs(Math.Abs(time - timeo) - timeav) < (timeav * 5) Then timeav = timeav * 0.99 + Math.Abs(time - timeo) * 0.01 'Delta T in windows is +/- 15ms, so average heavily. When USB "Seizes" this will keep a 
        timeo = time
        deltaT = timeav                  'should only use one, but cosd references both, so there you are.
        Cals.LDT.Text = "DT:" + timeav.ToString("F3")
        If Cals.Visible Then
            If saepress >= 95 And saepress <= 105 And saetemp >= 15 And saetemp <= 35 Then
                SAEcf = 1.176 * ((99 / saepress) * ((saetemp + 273) / 298) ^ 0.5) - 0.176 'Calculate Correction Factor

            Else : SAEcf = 1.0                                 'Set to 1 if out of range
            End If
            If saecomp = False Then SAEcf = 1
            Cals.LSAECF.Text = (SAEcf * 100).ToString("N1") + "%"
        End If
            'Label4.Text = RXtxt            'Serial Coms Regurgitation Text
            If InStr(RXtxt, "Z") = 1 And Len(RXtxt) > 20 Then               'Good read
                'Format should be:    ZSSSS1_SSSS2_TT.TTT_LLL_Mode_SP_TTL_Alarm_A1_A2_A3_A4 [CR] 
                PBcoms.Image = My.Resources.GREEN_LED
                Timer1.Stop() : Timer1.Start()                          'Gotta do this to "reset" it
                RXtxt = " " + Mid(RXtxt, 2, Len(RXtxt) - 1) + "0 0 0 0 0 0 0 0 0 0"                            'Trim Z and pad
                RXtxt = RXtxt.Replace("  ", " ")                                            'Remove Double spaces (to single)
                d = RXtxt.Split(" ")

            Speed1 = Val(d(1))              'Raw Dyno speed
                speed2 = Val(d(2))
                torqraw = Val(d(3))
                load = Val(d(4)) / 10         'This one is in 1000's rather than %

                Dynoload = load
                mode = d(5)
                throttle = d(7)               'Just for TUning
                Alarm = Val(d(8))

            If saeauto = True And d.Length > 10 Then                                    'Read Weather Station Factors (if got)
                saepress = 10 + Val(d(9)) / 10                                          'Coding is (kPa - 10) *10
                saetemp = Val(d(10))
                saeRH = Val(d(11))
            End If

            p2ox = p2x : If Me.WindowState <> 1 Then p2x = Panel2.Width
            p2oy = p2y : If Me.WindowState <> 1 Then p2y = Panel2.Height 'Keep "old" height.width for ref
        End If
        If Alarm = 0 Then
            Lalarm.Text = "Universal Inertial/Loaded Dyno Program"
            Lalarm.BackColor = Color.Transparent
        Else
            Lalarm.Text = alarms(lang)
            Lalarm.BackColor = Color.Black
        End If
        LTagline.Text = tagline

        If CBAcel.Checked = False Then : TBAcel.BackColor = Color.LightGray : Else : TBAcel.BackColor = Color.White : End If
        If CB2Point.Checked = False Then : TBLoad2.BackColor = Color.LightGray : Else : TBLoad2.BackColor = Color.White : End If
        If CBMan.Checked = False Then : TBMan.BackColor = Color.LightGray : Else : TBMan.BackColor = Color.White : End If
        If CBRPM.Checked = False Then : TBrpmSP.BackColor = Color.LightGray : Else : TBrpmSP.BackColor = Color.White : End If
        If CBAcel.Checked = False AndAlso CB2Point.Checked = False Then
            TBLoad1.BackColor = Color.LightGray : Tbrpm1.BackColor = Color.LightGray : TBRPM2.BackColor = Color.LightGray
        Else
            TBLoad1.BackColor = Color.White : Tbrpm1.BackColor = Color.White : TBRPM2.BackColor = Color.White
        End If
        '                               Setup Analogs and Derrived Cahnnels             **********************************************


        a(1) = a(1) * afilt(1) + (1 - afilt(1)) * (Speed1 * Afact(1) + aof(1))              'Speed Dyno

        a(2) = a(2) * afilt(2) + (1 - afilt(2)) * (speed2 * Afact(2) + aof(2))              'Speed Engine
        speedC2 = speed2 * Afact(2) + aof(2)                                                'For torque and GR calcs, use the "calibrated" number w/o filtering
        a(3) = a(3) * afilt(3) + (1 - afilt(3)) * ((Speed1 * GR) * Afact(3) + aof(3))       'Speed Eng Calc
        a(4) = a(4) * afilt(4) + (1 - afilt(4)) * (torqraw * Afact(4) + aof(4))             'Raw Torq

        acell = ((Speed1 - os1) * Afact(1)) * 2 * Math.PI / (60 * timeav)                   'Non Averaged Speed (don't use offset as subtracts out)
        'TorqTot = (torqraw * Afact(4) + aof(4)) + (inert + inerteng) * acell                'Unfiltered torque + Acellerational Torque from Inertia (Exclude Frict at 0 speed).
        Tfrict = (torqraw * Afact(4) + aof(4)) + (inert + inerteng) * acell + (a(1) * frictfact + frictof)                 'Unfiltered torque + Acellerational Torque from Inertia + Frict
        TorqTot = (torqraw * Afact(4) + aof(4)) + (inert + inerteng) * acell                  'Unfiltered torque + Acellerational Torque from Inertia (Exclude Frict at 0 speed).
        If Speed1 > 5 Then
            TorqTot = Tfrict                                                                'With Friction.
        End If

        If saecomp = True And Math.Abs(1 - SAEcf) < 0.05 Then TorqTot = TorqTot * SAEcf 'Only comp if ON and in range
        a(5) = a(5) * afilt(5) + (1 - afilt(5)) * (TorqTot * Afact(5) + aof(5))             'Total Torq

        'If speedC2 = 0 Then
        If GR <> 0 Then a(6) = a(6) * afilt(6) + (1 - afilt(6)) * ((TorqTot / GR) * Afact(6) + aof(6)) 'Engine Torq
        'Else                                                                                           'Edited out the below as Engin RPM is unreliable
        'a(6) = a(6) * afilt(6) + (1 - afilt(6)) * (TorqTot * (Speed1 / speedC2) * Afact(6) + aof(6))     'Eng Torq using Speed 2 as engine speed
        'End If

        a(7) = a(7) * afilt(7) + (1 - afilt(7)) * (TorqTot * Speed1 * 2 * (Math.PI / 60000) * Afact(7) + aof(7))        'Pow KW
        a(8) = a(8) * afilt(8) + (1 - afilt(8)) * (TorqTot * Speed1 * 2 * (Math.PI / 60 / 750) * Afact(8) + aof(8))     'Pow hp
        a(9) = a(9) * afilt(9) + (1 - afilt(9)) * (load * Afact(9) + aof(9))                                            'Load
        For i = 10 To 13
            a(i) = a(i) * afilt(i) + (1 - afilt(i)) * (Val(d(i - 1)) * (5 / 1023) * Afact(i) + aof(i))                                'Analogs
        Next

        lastrem = RemV : RemV = Val(d(8 + remch))               'Remote Value and previous Rev Value
        If RemV > 1024 Then RemV = RemV / 10 'Think I got a FW bug, so gotta div 10?
        'Console.WriteLine("A4 Value: " + Str(RemV))
        distnc = distnc + Speed1 * deltaT * Math.PI * diam / 60    'Distance in Meters                                                  '10-13 are the Analog nputs (above)
        KMt = KMt + +Speed1 * deltaT * Math.PI * diam / 60000                                         'Increase Odometer for Maint Tracking Reference
        a(14) = a(14) * afilt(14) + (1 - afilt(14)) * (distnc * Afact(14) + aof(14))                                 '14th is Distance
        a(15) = a(15) * afilt(15) + (1 - afilt(15)) * (60 * Speed1 * Math.PI * diam * Afact(15) / 1000 + aof(15))     '15th is Linear Speed
        'a(16)-a(27) are ECU/OBD data (assigned in ProcessOBD sub)
        'Console.WriteLine("A13: " + Str(a(13)))


        If autotare = True Then                     'AUTO TARE
            BuTare.BackColor = butcol1      'Turn "Auto Tare" light ON
            If time - taretime > 60 Then
                If Speed1 < 1 And load < 1 Then
                    taretime = time
                    If COMport.IsOpen And Not contcals.Visible() Then COMport.Write("Z") 'Tare Load Cell
                End If
            End If
        Else
            BuTare.BackColor = butcol0      'Turn "Auto Tare" light ON
        End If
        'DONE ANALOGS SETUP                             *********************************************

        'If OBDon Then        'PROCESS OBD 8888
        '    If OBDECU Then
        '        'call only when SerialPort Data Recieved
        '    Else
        '        If Math.Abs(time - OBDLast) > OBDtime Then
        '            OBDLast = time
        '            Call obdprocess()
        '        End If
        '    End If                                     'Or should it be triggered by "recieved data"?
        'End If

        GIT = 0                           '(Temporary Graf Index, fcounts through the grafs)
        scrnref = False
        If Math.Abs(time - lastref) >= refresh Then       'Only update screens every REFRESH
            If Ftune.Visible = True Then Call Ftune.main() 'RUnning TUnein Refresh to reduce resources a bit
            lastref = time
            scrnref = True

            ' Send real-time dyno data to modern WPF Dashboard
            If wpfDash IsNot Nothing AndAlso wpfDash.IsLoaded AndAlso wpfDash.IsVisible Then
                Dim liveRpm As Double = a(1)
                If Ldyno2.Text <> "Dyno" AndAlso a(2) > 0 Then liveRpm = a(2)
                Dim liveSpeed As Double = a(15)
                Dim liveHp As Double = a(8)
                Dim liveTorq As Double = a(6)
                Dim liveLoad As Double = a(9)
                Dim liveTps As Double = Val(throttle)
                Dim liveTemp As Double = saetemp

                wpfDash.Dispatcher.InvokeAsync(Sub()
                    wpfDash.UpdateFromRealDyno(liveRpm, liveSpeed, liveHp, liveTorq, liveLoad, liveTps, liveTemp)
                End Sub)
            End If
            For i = 1 To gagemax
                Select Case gtype(i)                     '1=Gage 2=Graf  3=led   4=vert  5=Horz  6=Number
                    Case 1                                  'GAGE
                        gpi = i
                        ' gage(gpi).Invalidate(True)
                        gage(i).Refresh()
                    Case 2              'Graf
                        gpi = i                       'Data is indexed to Display itme directly. This leaves "un used" data in the array, but so what?
                        gcnt(gpi) = gcnt(gpi) + 1               'Increment Data Position
                        If gcnt(gpi) = 301 Then gcnt(gpi) = 1 'Roll over to first position
                        gdat(gpi, gcnt(gpi)) = a(Gch(i))
                        ' gage(gpi).Invalidate(True)
                        gage(i).Refresh()
                    Case 3                                    'LED
                        If a(Gch(i)) < a1(Gch(i)) Then
                            gage(i).Image = My.Resources.GREEN_LEDi
                        Else
                            If a(Gch(i)) < a2(Gch(i)) Then
                                gage(i).Image = My.Resources.YELLO_LEDi
                            Else
                                gage(i).Image = My.Resources.RED_LEDi
                            End If
                        End If
                        gage(i).Refresh()
                    Case 4                                  'VERT
                        gpi = i
                        ' gage(gpi).Invalidate(True)
                        gage(i).Refresh()
                    Case 5                                  'HORZ
                        gpi = i
                        'gage(gpi).Invalidate(True)
                        gage(i).Refresh()
                    Case 6                                  'Number
                        'n(i).Parent = gage(i)
                        n(i).Left = gage(i).Width / 2 - n(i).Width / 2
                        n(i).Top = gage(i).Height * 0.65 - n(i).Height / 2
                        n(i).Text = a(Gch(i)).ToString(adec(Gch(i)))
                        n(i).BringToFront()
                        n(i).TextAlign = ContentAlignment.TopCenter
                        n(i).BackColor = Color.Transparent
                        l(i).BringToFront()
                        n(i).Show()
                    Case 7                                  'GAGE Bulat
                        gpi = i
                        'gage(gpi).Invalidate(True)
                        gage(i).Refresh()
                End Select
                gpi = 0
                'l(i).Show()
            Next
        End If



        If remote AndAlso RemV > 50 AndAlso Math.Abs(RemV - lastrem) / RemV < 0.02 Then        'Remote is pushed and same +/-5% as last
            'Console.WriteLine("REMOTE: " + Str(RemV))
            Select Case True
                Case (431 < RemV And RemV < 540)          'BUTTON #3 LEFT // Load Down
                    If Comp.Visible Then
                        Comp.xmark = Comp.xmark - 0.005
                        Call Comp.Chrt1stats(Nothing)
                    Else
                        Select Case loadMode
                            Case 1 : Call ButtManDn(Nothing, Nothing)               'MAN
                            Case 2 : Call SpeedDn(Nothing, Nothing)                 'RPM 
                            Case 3, 4, 5                                            '2-point or Acell
                                TBLoad1.Text = Val(TBLoad1.Text) - 0.5
                                If Val(TBLoad1.Text) < 0 Then TBLoad1.Text = "0"
                                Call TBLoad1_TextChanged(Nothing, Nothing)
                        End Select
                    End If
                    remoff = False
                Case (310 < RemV And RemV < 430)             'BUTTON #4 RIGHT // LOAD UP
                    If Comp.Visible Then
                        Comp.xmark = Comp.xmark + 0.005
                        Call Comp.Chrt1stats(Nothing)
                    Else
                        Select Case loadMode
                            Case 1 : Call ButTManup(Nothing, Nothing)               'MAN
                            Case 2 : Call Speedup(Nothing, Nothing)                 'RPM
                            Case 3, 4, 5                                            '2-point, Timed
                                TBLoad1.Text = Val(TBLoad1.Text) + 0.5
                                If Val(TBLoad1.Text) > 100 Then TBLoad1.Text = "100"
                                Call TBLoad1_TextChanged(Nothing, Nothing)
                        End Select
                    End If
                    remoff = False
                Case (200 < RemV And RemV < 300)            'BUTTON #2 Load // Exit Graf

                    If remoff = True Then
                        If Comp.Visible Then
                            Comp.BGrafEx.PerformClick()
                        Else
                            'If AutoSave = True Then
                            '    AutoSave = False
                            '    BAS.BackColor = butcol0
                            'Else
                            '    AutoSave = True
                            '    BAS.BackColor = butcol1
                            'End If
                            'remoff = False
                            Console.WriteLine("Click on 2 MODE:" + Str(LastLMode) + " Loadon:" + Str(loadon))
                            If loadon = False Then
                                loadMode = LastLMode : TBMode.Text = prevmode
                                loadon = True
                                Select Case loadMode
                                    Case 1 : CBMan.Checked = True ': Call CBManChanged(Nothing, Nothing)
                                    Case 2 : CBRPM.Checked = True ': Call CBRPMCh(Nothing, Nothing)
                                    Case 3 : CB2Point.Checked = True : pullmode = 5
                                    Case 4 : CBAcel.Checked = True : pullmode = 5
                                End Select
                                'loadon = True
                            Else
                                CB2Point.Checked = False : CBAcel.Checked = False : prevmode = TBMode.Text
                                sendcmd = "M0" : If COMport.IsOpen Then COMport.WriteLine(sendcmd)
                                TBMode.Text = remloads(lang)
                                LastLMode = loadMode : CBMan.Checked = False : CBRPM.Checked = False
                                loadon = False
                            End If
                        End If
                    End If
                    remoff = False
                Case (80 < RemV And RemV < 170)         'BUTTON #1 Brek Button
                    'Console.WriteLine("Brek Button " + Str(RemV))
                    If remoff = True Then
                        If brekon = False Then
                            TBMode.Text = rembreks(lang)
                            Call brek()
                        Else
                            Call brekrelease()
                        End If
                    End If
                    remoff = False                  'Diss allow a "double tap"
                Case RemV > 960                      'Released Button
                    If remoff = False Then Console.WriteLine("Remote RESET")
                    remoff = True
            End Select
        End If
        ' If brekon AndAlso CBAcel.Checked = False AndAlso CB2Point.Checked = False Then
        'End If


        Call gagemove()

        If brekon = True Then                                               'BREK   BREK    BREK   BREK   BREK    BREK   BREK   BREK    BREK   
            If brek2p = True Then
                LB1 = load2                                               'Break using 2-point Load (Stop % Load)
            Else
                LB1 = BREKload                                            'Break using Breaking Cal Load
            End If
            pload = LB1                                                     'Set as designated Break Loading, then "catch" if lower via logic below
            If brekramp = True Then
                If brekstop = True Then                                     'Ramps down from RPM2 to RPM1
                    If rpm2 <> rpm1 Then pload = LB1 * (Speed1 - rpm1) / (rpm2 - rpm1)

                Else                                                        'Ramps from RPM2 down to zero speed
                    If rpm2 <> 0 Then pload = LB1 * Speed1 / rpm2
                End If
            Else
                If brekstop = True And Speed1 < rpm1 Then pload = 0 'Shutoff at RPM1 (no ramp)
            End If
            If pload > LB1 Then pload = LB1 'Dont let it exceed the assigned breaking load (extrapolate)
            If pload < 0 Then pload = 0
            'OutPut Breaking Load
            'Sometimes misses ping to put back in loading mode!!! Some times clashes with Man output below, other times it just misses
            'If Int(10 * pload) <> Int(10 * lastload) Then 'Only send when it changes, otherwise we overload
            sendcmd = "M" + Str(Int(10 * pload)).Replace(" ", "")
            If compt <> 0 Then COMport.Write(sendcmd)
            'End If
            lastload = pload
            If Speed1 < 5 Or pload < 0.1 Then
                Call brekrelease()                      'Exit Brake mode and send "M0"
            End If
        Else
            lastload = 0
        End If

        If FACon = True Then                                                '$$$$$$$$$$$$$$$$$$$$$$$$$$$   Auto Friction Calc      $$$$$$$
            Call autofrict()
        End If

        TBPowMax.Text = powmax.ToString("n2")
        If PowUnit = 1 Then
            Lpow.Text = "hp" : If a(8) > powmax Then powmax = a(8) 'Set max Power in text box
        Else
            Lpow.Text = "kW" : If a(7) > powmax Then powmax = a(7) 'Set max Power in text box
        End If

        If play = True Then                                                     'Play means we're in "Testing Mode"
            fgraf.L1.Text = "RPM: " + Str(Int(a(xgraf - 1)))                    'RPM Label at InstaGraf
            fgraf.L2.Text = alab(y1graf - 1) + ": " + a(y1graf - 1).ToString(adec(y1graf - 1))     'Give 'e nice big numbers on the screen
            fgraf.L3.Text = alab(y2graf - 1) + ": " + a(y2graf - 1).ToString(adec(y2graf - 1))
            fgraf.L4.Text = alab(y3graf - 1) + ": " + a(y3graf - 1).ToString(adec(y3graf - 1))
            fgraf.L5.Text = alab(y4graf - 1) + ": " + a(y4graf - 1).ToString(adec(y4graf - 1))
        End If
        '================================= PULL LOADING  ================================== PULL LOADING  ===========    PULL LOADING  ===========

        If (CB2Point.Checked = True Or CBAcel.Checked = True) And Not (GearRatio.Visible) Then   'Only PULL if in either 2-point, or Acell, and GR is OFF 
            pspeed = a(1)                             'Dyno Speed
            prpm1 = rpm1
            prpm2 = rpm2
            PullDur = Val(TBAcel.Text)
            If PullDur < 3 Then PullDur = 15
            TBAcel.Text = PullDur.ToString("F0")
            If PullDur <> 0 Then pacel = (rpm2 - rpm1) / PullDur 'Set Acell
            If rpmode = 3 Then                        'Engine Speed Mode (1=Dyno, 2=Engine Calc, 3=Engine RPM2)
                pspeed = a(2)                         'Engine Speed
                prpm1 = Val(Tbrpm1.Text)              'All RPMs are in DYNO speed EXCEPT in RPM2 Mode
                prpm2 = Val(TBRPM2.Text)                'Difference in ENg Calc and Dyno id done via changes in dGR variable
                PullDur = Val(TBAcel.Text)                                        'Assign text acell (in terms of engine RPM) to pull acell
                If PullDur <> 0 Then pacel = (prpm2 - prpm1) / PullDur 'Target Acelleration Used in Acell Control Mode
            End If
            If (pspeed < prpm1 * 0.5) Or (pullmode = 4 And pspeed < prpm1 And pspeed > opspeed) Then    'Lower Limit of "coast down"
                pullmode = 1 'Idle                        If fgraf.Visible Then fgraf.Hide()
            End If
            If play = True And Speed1 > LowSpeedPostPull * 1.05 And pullmode = 4 Then pullmode = 1 'Re-pull if in Play mode and speed up more than 5%
            Select Case pullmode
                Case 1                                              'IDLE Nothing to do here except put load at 0
                    sendcmd = "M0"
                    If compt <> 0 Then COMport.WriteLine(sendcmd)
                    If pspeed > prpm1 * 0.5 Then pullmode = 2
                Case 2                                              'RAMP: Only setup pull and begin loading AFTER getting some dyno speed
                    load2 = Val(TBLoad2.Text)                       'Define the 2nd load point at the begining of the pull           
                    'If rpmode = 3 Then pacel = (prpm2 - prpm1) / PullDur
                    If pspeed < prpm1 * 0.5 Then
                        pullmode = 1                                'Dropped below 1/2 of start RPM
                    Else
                        pload = load1 * (pspeed - 0.5 * prpm1) / (0.5 * prpm1)  'Apply ramp up to start load
                        If pload > load1 Then pload = load1
                    End If
                    If pspeed > prpm1 And (Comp.Visible = False) Then
                        pullmode = 3                        'Go WOT and "log" run
                        timepull = time                     'Time of starting Pull
                        pullmaxrpm = 0                      'Reset Max Pull RPM (for stall calculation)
                        run = run + 1                       'Inc run number
                        ButRun.Text = "RUN: " + Str(run)
                        runsave = False
                        dataindex = 0
                        distnc = 0                              'Reset pull distance
                        dic = dic + 1 : If dic = 5 Then dic = 1 'Set Pull Data Index
                        If instgraf = True Or play = True Then
                            fgraf.Chart1.Series(2 * dic - 2).Points.Clear()                     'Clear previous graf points
                            fgraf.Chart1.Series(2 * dic - 1).Points.Clear()
                            fgraf.Chart2.Series(2 * dic - 1).Points.Clear()
                            fgraf.Chart2.Series(2 * dic - 2).Points.Clear()
                            fgraf.Chart2.ResetAutoValues()
                            If play = True Then                                             'Testing mode: Make BIG and OPAQUE
                                fgraf.Opacity = 0.85 : fgraf.Size = New Size(INSTWidth, INSTHeight)  'Size(893 * 1.5, Screen.PrimaryScreen.WorkingArea.Height * 0.8)
                                fgraf.L1.Parent = fgraf.Chart1
                                fgraf.L2.Parent = fgraf.Chart1
                                fgraf.L3.Parent = fgraf.Chart1
                                fgraf.L4.Parent = fgraf.Chart1
                                fgraf.L5.Parent = fgraf.Chart1
                                fgraf.L1.Show() : fgraf.L2.Show() : fgraf.L3.Show() : fgraf.L4.Show() : fgraf.L5.Show()
                            Else
                                fgraf.Opacity = 0.7 : fgraf.Size = New Size(INSTWidth, INSTHeight) 'Size(893, 430)                   
                                fgraf.L1.Hide() : fgraf.L2.Hide() : fgraf.L3.Hide() : fgraf.L4.Hide() : fgraf.L5.Hide()
                            End If

                            If amax(y3graf - 1) > amin(y3graf - 1) Then                                 'Might need to put Y4Graf on 2nd axis and scale similarly
                                fgraf.Chart2.ChartAreas(0).AxisY.Minimum = amin(y3graf - 1)
                                fgraf.Chart2.ChartAreas(0).AxisY.Maximum = amax(y3graf - 1)
                                InstXmin = 1000000000.0
                                If GManX = True Then
                                    fgraf.Chart1.ChartAreas(0).AxisX.Minimum = 0
                                    fgraf.Chart1.ChartAreas(0).AxisX.Interval = GrX1                  'Set interval to follow COMP Interval
                                    fgraf.Chart2.ChartAreas(0).AxisX.Interval = GrX1
                                End If
                                'ChartAreas(0).AxisY2.Maximum
                            End If

                            fgraf.Location = New Point(INSTLeft, INSTTop) '(0, Screen.PrimaryScreen.WorkingArea.Height - fgraf.Height)
                            fgraf.Show()
                            If play = True Then                                             'Keep Insta on top if in "testing" mode (ie. play)
                                fgraf.TopMost = True
                            Else
                                fgraf.TopMost = False
                            End If
                            For i = 1 To 4                                                      'Set all "old" graphs to "light" and Thin    
                                fgraf.Chart1.Series(2 * i - 2).BorderWidth = 1
                                fgraf.Chart1.Series(2 * i - 2).Color = Color.Blue
                                fgraf.Chart1.Series(2 * i - 1).BorderWidth = 1
                                fgraf.Chart1.Series(2 * i - 1).Color = Color.Red
                                fgraf.Chart2.Series(2 * i - 1).BorderWidth = 1
                                fgraf.Chart2.Series(2 * i - 1).Color = Color.Gray
                                fgraf.Chart2.Series(2 * i - 2).BorderWidth = 1
                                fgraf.Chart2.Series(2 * i - 2).Color = Color.LightGreen
                            Next                                                                'Then turn latest to dark, and thick
                            fgraf.Chart1.Series(2 * dic - 2).BorderWidth = 3
                            fgraf.Chart1.Series(2 * dic - 2).Color = Color.DarkBlue
                            fgraf.Chart1.Series(2 * dic - 1).BorderWidth = 2
                            fgraf.Chart1.Series(2 * dic - 1).Color = Color.DarkRed
                            fgraf.Chart2.Series(2 * dic - 1).BorderWidth = 3
                            fgraf.Chart2.Series(2 * dic - 1).Color = Color.Black
                            fgraf.Chart2.Series(2 * dic - 2).BorderWidth = 2
                            fgraf.Chart2.Series(2 * dic - 2).Color = Color.DarkGreen
                        End If
                    End If
                    sendcmd = "M" + Str(Int(10 * pload)).Replace(" ", "")
                    Call SendPullLoad()                     'If compt <> 0 Then COMport.WriteLine(sendcmd) 'Send Ramp Loading
                Case 3                                                                   'MAIN PULL EVENT  WOT!
                    timelaps = time - timepull                                            'Elapsed Time
                    dataindex = dataindex + 1
                    If dataindex > 2000 Then dataindex = 2000 'Max data limit

                    pulldata(dic, dataindex, 1) = timelaps                               'TIME IS 1ST SLOT
                    For i = 1 To 27
                        pulldata(dic, dataindex, i + 1) = a(i)                           '2-15 are Analogs
                    Next

                    If (instgraf = True Or play = True) AndAlso scrnref = True Then
                        'MsgBox("Mode 3 - Adding Points")
                        fgraf.Chart1.Series(2 * dic - 2).Points.AddXY(a(xgraf - 1), a(y1graf - 1))      'Torque
                        fgraf.Chart1.Series(2 * dic - 1).Points.AddXY(a(xgraf - 1), a(y2graf - 1))      'Power
                        fgraf.Chart2.Series(2 * dic - 2).Points.AddXY(a(xgraf - 1), a(y3graf - 1))      'Analog Add data to instant graph
                        fgraf.Chart2.Series(2 * dic - 1).Points.AddXY(a(xgraf - 1), a(y4graf - 1))
                        If a(xgraf - 1) < InstXmin Then InstXmin = a(xgraf - 1)
                        'Console.WriteLine(alab(xgraf - 1) + ": " + Str(a(xgraf - 1)) + "  " + alab(y3graf - 1) + ": " + Str(a(y3graf - 1)))
                        If GManX = True Then
                            fgraf.Chart1.ChartAreas(0).AxisX.Minimum = Int(InstXmin / GrX1) * GrX1       'Set min to integer of Interval
                            fgraf.Chart2.ChartAreas(0).AxisX.Minimum = Int(InstXmin / GrX1) * GrX1
                            Console.WriteLine("Set INSTAGRAF Xmin to " + Str(Int(InstXmin / GrX1) * GrX1))
                        End If
                    End If
                    If pspeed > pullmaxrpm Then pullmaxrpm = pspeed

                    If pspeed < prpm1 * 0.98 Then                               'We slowed down
                        pullmode = 2
                        If instgraf = True And play = False Then fgraf.Hide() 'Close Instagraf and go back to 2nd Mode
                        'fgraf.Hide()                            'Close Instagraf if stalled
                    Else
                        If loadMode = 3 Then    '2-Point Loading
                            pload = load1 + (load2 - load1) * ((pspeed - prpm1) / (prpm2 - prpm1))
                            ' Console.WriteLine("2-Point: " + Str(pload) + " Rel Speed: " + Str((speed1 - rpm1) / (rpm2 - rpm1)))
                        Else                    'Acell Mode
                            pload = pload + KPa * timeav * ((pspeed - Ops1) / timeav - pacel) / pacel          'Acell  Factor is typically 0.2-0.4
                            Console.WriteLine("Load: " + Str(pload) + " RPM: " + pspeed.ToString("F0") + " Acell: " + ((pspeed - Ops1) / timeav).ToString("F0") + " Targ Acel: " + pacel.ToString("F0"))
                        End If
                    End If
                    If (pspeed > prpm2) Or (pspeed < pullmaxrpm * 0.95) Or timelaps > 40 Then 'If pased upper speed, 40 seconds or dropped to 95% of max speed
                        LowSpeedPostPull = Speed1
                        pullmode = 4                                            'Just Finished PULL

                        points = dataindex

                        If play = False Then
                            INSTTop = fgraf.Top
                            INSTLeft = fgraf.Left
                            INSTWidth = fgraf.Width
                            INSTHeight = fgraf.Height
                            fgraf.Hide() 'Close Instant Graf
                        End If
                        If autoload = True And timelaps > 2 Then                'Automatic Load Correction (if pull <5 sec, or >20 sec), if < 2 sec was a bad launch
                            If loadMode = 3 Then                                '2-Point
                                If timelaps > 25 Then                           '2-Point Too Slow
                                    load1 = Int(10 * load1 * 20 / timelaps) / 10
                                    load2 = Int(10 * load2 * 20 / timelaps) / 10
                                    TBLoad1.Text = Str(Int(load1 * 10) / 10)
                                    TBLoad2.Text = Str(Int(10 * load2) / 10)
                                ElseIf timelaps < 7.5 Then                      '2-Point Too Fast

                                    load1 = Int(10 * load1 * 10 / timelaps) / 10
                                    load2 = Int(10 * load2 * 10 / timelaps) / 10
                                    If load1 > 100 Then load1 = 100
                                    If load2 > 100 Then load2 = 100
                                    TBLoad1.Text = Str(Int(load1 * 10) / 10)
                                    TBLoad2.Text = Str(Int(load2 * 10) / 10)
                                End If


                            Else                                                'Acell Mode
                                If timelaps > PullDur * 1.1 Then                'Acell Mode too slow
                                    load1 = Int(10 * load1 * PullDur / timelaps) / 10
                                    'acel = Int(acel * 20 / timelaps)
                                    TBLoad1.Text = Str(load1)
                                ElseIf timelaps < PullDur * 0.9 Then            'Acell Mode too fast
                                    load1 = Int(10 * load1 * PullDur / timelaps) / 10
                                    If load1 > 100 Then load1 = 100
                                    TBLoad1.Text = Str(Int(load1 * 10) / 10)
                                End If
                            End If
                        End If


                        For i = dataindex + 1 To 2000                                   'Set any old data to zeros
                            For j = 1 To chan
                                pulldata(dic, i, j) = 0
                            Next
                        Next
                        'Auto Open big graf
                        If autograf = True And play = False Then                                                 'Auto Open big graf
                            Comp.Lrun1.Text = run
                            'GrafSet.Label4.Text = "From Main"
                            Comp.LID.Text = runfile
                            Comp.Ltime.Text = Now
                            Comp.Lsec.Text = timelaps.ToString("F1")
                            Comp.ButFile.Text = "Click to load file" 'This text is refferred to in FComp, if you change it here, change it there
                            'If fcomp.first = True Then fcomp.ButFile.Text = "Click to load file" 'This text is refferred to in FComp, if you change it here, change it there
                            Comp.Lcont.Text = ShopCont
                            Comp.LShop.Text = ShopName
                            If Comp.Visible = False Then Comp.Show() 'If it isn't on allready, then turn it on
                        End If

                    End If
                    If pload < 0 Then pload = 0
                    If pload > 1000 Then pload = 1000
                    sendcmd = "M" + Str(Int(10 * pload)).Replace(" ", "")
                    ann = sendcmd
                    Call SendPullLoad()             'If compt <> 0 Then COMport.WriteLine(sendcmd) '.RunSynchronously()
                Case 4                                                                  'Roll Down
                    If Speed1 < LowSpeedPostPull Then LowSpeedPostPull = Speed1 'Set up to see if increasing speed from lowest for re-pull
                    If pspeed < prpm2 And runsave = False And play = False Then                           'Save File
                        runsave = True
                        Dim SaveFileBox As New SaveFileDialog
                        With SaveFileBox
                            rundatafile = runfile + Str(run) + ".CSV"
                            .FileName = rundatafile
                            .DefaultExt = "CSV"
                            .Filter = "CSV|*.csv"
                            .Title = "Pull Data File"
                            If AutoSave = False Then
                                saveresult = .ShowDialog()
                            Else
                                saveresult = DialogResult.OK            'Automatically Save Results
                            End If

                            If saveresult = DialogResult.OK Then
                                rundatafile = .FileName
                                'If InStr(rundatafile, " ") > 3 Then runfile = Mid(rundatafile, 1, InStr(rundatafile, " ") - 1) 'Trim
                                'If InStr(runfile, ".") > 3 Then runfile = Mid(runfile, 1, InStr(runfile, ".") - 3)
                                If run < 100 Then
                                    If InStr(rundatafile, ".") > 3 Then runfile = Mid(rundatafile, 1, InStr(rundatafile, ".") - 3) 'Trim
                                Else
                                    If InStr(rundatafile, ".") > 4 Then runfile = Mid(rundatafile, 1, InStr(rundatafile, ".") - 4) 'Trim
                                End If
                                rundatafile = rundatafile.Replace("  ", " ")
                                FileClose(4)
                                FileOpen(4, rundatafile, OpenMode.Output) ' Has puked on this line "Cant find part of the path" when got crazy path name
                                PrintLine(4, "Focus Applied Technologies DynoMas Pull Version: " + ver + "    Opened: " + Now)
                                PrintLine(4, "Pull Data File: " + rundatafile + ",   Run #: " + Str(run) + ",    Points: " + Str(dataindex) + ",    Time: " + timelaps.ToString("N2"))
                                PrintLine(4, " ")
                                Print(4, "Time")
                                For i = 1 To chan
                                    Print(4, "," + alab(i))
                                Next
                                PrintLine(4, " ")
                                For i = 1 To dataindex
                                    For j = 1 To chan + 1                                 'Max # of Channels plus 1
                                        Print(4, Str(pulldata(dic, i, j)) + ", ")
                                    Next
                                    PrintLine(4, " ")
                                Next
                                PrintLine(4, "END")                                       'End Of Line
                                FileClose(4)
                            End If
                            'Purge Serialcoms
                            COMport.DiscardInBuffer()
                        End With
                        'AUTO GRAF was formerly HERE
                    End If
                    If brek2p = True Then
                        pload = load2                                               'Break using 2-point Load (Stop % Load)
                    Else
                        pload = brekload                                            'Break using Breaking Cal Load
                    End If
                    If brekramp = True Then
                        If brekstop = True Then                                     'Ramps down from RPM2 to RPM1
                            If prpm2 <> prpm1 Then pload = pload * (pspeed - prpm1) / (prpm2 - prpm1)
                            If pload < 0 Then pload = 0
                        Else                                                        'Ramps from RPM2 down to zero speed
                            If prpm2 <> 0 Then pload = pload * pspeed / prpm2
                        End If
                    End If
                    'Breaking Load
                    sendcmd = "M" + Str(Int(10 * pload)).Replace(" ", "")
                    If compt <> 0 Then COMport.WriteLine(sendcmd)
                Case 5
                    If pspeed < prpm1 Then pullmode = 1
            End Select
            If pullmode = 3 Then
                TBMode.Text = (Int(timelaps * 10) / 10).ToString("N1") + "s  " + pullcom(pullmode, lang)  'Display Pull Duration in comand box
            Else
                TBMode.Text = pullcom(pullmode, lang)
            End If
        Else
            If CBMan.Checked = True Then
                TBMode.Text = modmans(lang)
            Else
                If CBRPM.Checked = True Then TBMode.Text = modspeeds(lang)
            End If
        End If
        If Mid(mode, 1, 1) <> "C" Then
            TBMode.Text = commodes(lang)
        Else
            'If CBRPM.Checked = False And CB2Point.Checked = False And CBMan.Checked = False And CBAcel.Checked = False Then TBMode.Text = modes(lang)
        End If

        opspeed = pspeed
        If GearRatio.Visible = True Then
            GearRatio.speed1 = Speed1
            GearRatio.speedC2 = speedC2
            Call GearRatio.main() 'Gotta run the loop
        End If

        os1 = Speed1 : Ops1 = pspeed                                      'Set previous speed for Acelleration Calculations
        ButGR.Text = ratios(lang) + GR.ToString("N1")
        If contcals.Visible Then
            contcals.TBActNm.Text = torqraw 'Gotta put the actual measured Torque in the Cal Window
            contcals.TBRPM1.Text = Speed1
            contcals.TBRPM2.Text = speed2
        End If
        If logon = True Then
            Print(5, Str(time) + ",")
            For j = 1 To chan + 1                                 'Max # of Channels plus 1
                Print(5, Str(a(j)) + ", ")
            Next
            PrintLine(5, " ")
        End If

        If OBDon Then                                                        'This sends out the PINGS or OED data requests
            OBDSEND()                                                       'Send the Data Request PING
        Else
            ButOBD.BackColor = butcol0                                      'OBD OFF
        End If
    End Sub '################################################################# E N D ###### M A I N #################################################
    Private Sub ClseFOBD() Handles FOBD.OBDSav            'Set up OBD numbers WITHOUT closing the OBD window
        OBDportnum = Val(FOBD.TBport.Text)
        OBDbr = Val(FOBD.TBBaud.Text)
        OBDiT = FOBD.TBInitT.Text
        OBDiB = FOBD.TBinitB.Text
        OBDECU = FOBD.RBECU.Checked
        OBDREP = FOBD.CBRep.Checked
        OBDSlow = FOBD.CBSlow.Checked
        OBTR = Val(FOBD.TBBytes.Text)                       'Bytes To Recieve
        OBDp = FOBD.Bpar.Text                             'Parity
        OBDpT = FOBD.TBPngT.Text
        OBDpB = FOBD.TBPngB.Text
        OBDtime = Val(FOBD.TBMaxPR.Text)                           'Ping Rate
        For i = 1 To 12
            If OBDCB(i).Checked = True Then
                OBDchon(i) = True
            Else
                OBDchon(i) = False
            End If
        Next
    End Sub
    Private Sub BOBD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButOBD.Click
        If FOBD.Visible Then                'Close
            FOBD.Hide()
            OBDportnum = Val(FOBD.TBport.Text)
            OBDbr = Val(FOBD.TBBaud.Text)
            OBDiT = FOBD.TBInitT.Text
            OBDiB = FOBD.TBPngB.Text
            OBDECU = FOBD.RBECU.Checked
            OBDREP = FOBD.CBRep.Checked
            OBDSlow = FOBD.CBSlow.Checked
            OBTR = Val(FOBD.TBBytes.Text)                   'Bytes To Recieve
            For i = 1 To 12
                If OBDCB(i).Checked = True Then
                    OBDchon(i) = True
                Else
                    OBDchon(i) = False
                End If
            Next
            Console.WriteLine("OBD DEC = " + OBDiB)
        Else                                     'Open
            serports = ""
            For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                serports = serports + sp + ", "
            Next
            serports = serports.Replace("COM", "")              'Lose the "COM"s to shorten so we can see them
            FOBD.LPorts.Text = "Ports: " + serports
            FOBD.TBport.Text = OBDportnum
            FOBD.TBBaud.Text = OBDbr
            FOBD.TBInitT.Text = OBDiT
            FOBD.TBinitB.Text = OBDiB

            FOBD.TBPngB.Text = OBDpB
            FOBD.TBPngT.Text = OBDpT
            FOBD.RBECU.Checked = OBDECU
            FOBD.CBRep.Checked = OBDREP
            FOBD.RBOBD.Checked = Not (OBDECU)
            FOBD.CBSlow.Checked = OBDSlow
            FOBD.TBBytes.Text = OBTR
            FOBD.TBMaxPR.Text = OBDtime

            If OBDon Then
                FOBD.Bopen.Text = "CLOSE PORT"
            Else
                FOBD.Bopen.Text = "OPEN PORT"
            End If
            For i = 1 To 12
                If OBDchon(i) = True Then
                    OBDCB(i).Checked = True
                Else
                    OBDCB(i).Checked = False
                End If
            Next
            FOBD.Show()
        End If
    End Sub
    Sub OBDSEND()
        Dim OBDtot, OBSkips As Integer                                     'Temporary count of OBD parameters to send (total)
        OBDtot = 0
        OBDcnt = OBDcnt + 1
        ButOBD.BackColor = butcol1
        FOBD.Lverified.Text = OBDis
        'OBDis = "OBD COMS RETURN:                           "              'Clear the incomming
        If OBDSlow Then OBSkips = OBDskp * 2 'Really slow for some ECUs
        If OBDECU Then                                                      'ECU ECU ECU
            If OBDREP And OBDcnt >= OBDskp Then                           'Send repeating sequence
                'Send ECU Pings
                OBDcnt = 0                                                'Reset Ping Cycle Counter
                FOBD.Lout.Text = "OUTGOING: "
                If OBDpB <> "" Then
                    Dim DECs(20) As String
                    Dim num As Integer
                    DECs = OBDpB.Split(" ")
                    Dim BinDat(20) As Byte
                    DECs = OBDpB.Split(" ")                                'Send the "ping" commend
                    For num = 0 To DECs.Length - 1
                        BinDat(num) = Val(DECs(num))
                        'System.Diagnostics.Debug.Write(DECs(num) + ",")
                    Next
                    FOBD.Lout.Text += OBDpB
                    OBDport.Write(BinDat, 0, DECs.Length)
                End If
                If OBDpT <> "" Then
                    OBDport.Write(OBDpT)
                    FOBD.Lout.Text += OBDpT
                End If
             
            End If
        Else
            If OBDcnt >= OBSkips Then 'Send OBD Pings
                OBDcnt = 0                                                'Reset the count
                obdsends = "01"                                             'Give Current Data Comand 01
                If OBDSlow Then                                             'Setup for single PID requests, one at a time
                    OBDcmds += 1                                     'Increment OBD Output Counter
                    For ib = 1 To 12
                        If OBDchon(ib) Then
                            OBDtot += 1                                 'Increment Total to see if we are on the right one
                            If OBDcmds = OBDtot Then
                                OBDL = OBDBytes(ib)                               'Set the expected return # Bytes
                                obdsends = obdsends + OBDcmd(ib) 'Add code for current requested data
                            End If

                        End If
                    Next
                    If OBDcmds >= OBDtot Then OBDcmds = 0 'Reset to 0 for next increment
                Else
                    For ib = 1 To 12
                        If OBDchon(ib) Then
                            obdsends = obdsends + OBDcmd(ib)                    'Add code for each requested data
                        End If
                    Next

                End If
                If Len(obdsends) > 2 Then                                   'Only send if got requests 
                    OBDport.Write(obdsends + Chr(13))                       'Send request
                    FOBD.Lout.Text = "OUTGOING: " + obdsends
                    OBDLast = time
                End If
            End If
        End If
    End Sub

    Private Sub openOBD() Handles FOBD.OBDOpen
        If OBDon = True Then                    'Port is OPEN, so we wanna CLOSE it
            Console.WriteLine("Close OBD")
            OBDon = False
            FOBD.Hide()
            If OBDport.IsOpen Then OBDport.Close()
            FOBD.Lverified.Text = "OBD COMS CLOSED"
        Else                                    'Port is CLOSED, so we wanna OPEN it
            If OBDport.IsOpen Then OBDport.Close()
            Console.WriteLine("Trying to OPEN OBD")
            OBDportnum = Val(FOBD.TBport.Text)                  'Load latest Com Port #'s
            OBDbr = Val(FOBD.TBBaud.Text)
            OBDiT = FOBD.TBInitT.Text
            comstr = "COM" + LTrim(Str(OBDportnum))
            OBDport.PortName = comstr                           'OPen Dyno Controller Com Port
            If OBDbr = 0 Then OBDbr = 38400
            OBDport.BaudRate = OBDbr
            OBDport.ReadBufferSize = 512                         'Normally 128
            OBDport.Handshake = Handshake.None
            OBDport.RtsEnable = True
            OBDport.DtrEnable = True
            OBDport.ReadTimeout = 500
            OBDport.StopBits = StopBits.One
            OBDport.DataBits = 8
            Select Case OBDp
                Case "N" : OBDport.Parity = Parity.None
                Case "E" : OBDport.Parity = Parity.Even
                Case "O" : OBDport.Parity = Parity.Odd
                Case "M" : OBDport.Parity = Parity.Mark
                Case "S" : OBDport.Parity = Parity.Space
            End Select
      
            serports = ""
            Try
                OBDport.Open()
            Catch ex As Exception                       'This only allows 1 retry!!!
                For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                    serports = serports + sp + ", "
                Next
                'Dim searcher As New ManagementObjectSearcher("root\cimv2", "SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0") ' PnPEntity Or SreialPort
                'For Each queryObj As ManagementObject In searcher.Get()
                '    tempstr = queryObj("Name")
                '    '  Console.WriteLine(tempstr)
                '    If tempstr.Contains("COM") Then
                '        ' Console.WriteLine(tempstr)
                '        If tempstr.Contains("USB Serial Port") Then
                '            Dim nonNumericCharacters As New System.Text.RegularExpressions.Regex("[^0-9]")
                '            tempstr = nonNumericCharacters.Replace(tempstr, String.Empty)
                '            compt = Val(tempstr)                                                            'If there are multimple units it'll just 
                '            'Console.WriteLine("Port: " + tempstr)                                           'Assign the last one as the default
                '            tempstr2 = tempstr2 + tempstr + ", "                                            'But will "recomend" any Prolific cable ports
                '        End If
                '    End If
                'Next
                CPStr = InputBox("Available Ports: (0 = offline), " + serports + Chr(13) + "Recomended: " + tempstr2, "Enter USB Com Port #", 1, 5, 5)
                If CPStr <> "" Then
                    OBDportnum = Val(CPStr)
                    comstr = "COM" + LTrim(Str(OBDportnum))
                    '     If PortIsAvailable(comstr) Then
                    OBDport.PortName = comstr    'OPen Dyno Controller Com Port: NOTE Can use USB com ports just like internal ones!
                    CPStr = InputBox("Baud Rate: " + Str(OBDbr) + Chr(13) + "Typically 9600 or 38400" + Chr(13) + "Enter OBD Scanner Com Baud Rate ", , Str(OBDbr))
                    If CPStr <> "" Then OBDbr = Val(CPStr)
                    OBDport.BaudRate = OBDbr
                    If compt <> 0 And OBDport.IsOpen = False Then
                        Try
                            OBDport.Open() '0 for offline (and Simulation)
                        Catch ex2 As Exception
                            MsgBox("Problem opening OBD port:" + ex2.Message)
                        End Try

                    End If
                Else
                    Exit Sub 'If they cancel, it's cuase they dont wanna run. 
                End If
            End Try
            Sleep(20)
            If OBDport.IsOpen Then                      'Should be open... Only then will we do the following
                OBDport.Close()                         'Apparently I have to open, then close, then open again to get it to work
                Sleep(20)                               'Otherwise I'd have to click it open twice... Dunno why
                OBDport.Open()
                If OBDECU = True Then                           'OPEN ECU
                    If OBDport.IsOpen Then
                        If OBDiT <> "" Then OBDport.Write(OBDiT + Chr(13)) 'Whatever other command to send
                        Sleep(200)
                        If OBDiB <> "" Then
                            Dim DECs(20) As String                                                'The freaking "split" command reduces the size of the array when used?!
                            Dim num As Integer
                            Dim BinDat(20) As Byte
                            DECs = OBDiB.Split(" ")                                'Send the "ping" commend
                            For num = 0 To DECs.Length - 1
                                BinDat(num) = Val(DECs(num))
                                System.Diagnostics.Debug.Write(DECs(num) + ",")
                            Next
                            OBDport.Write(BinDat, 0, DECs.Length)
                            OBDport.Write(Chr(13))
                        End If
                    End If
                    OBDon = True
                    FOBD.Lverified.Text = "OBD COMS LIVE"
                    FOBD.Bopen.Text = "CLOSE PORT"
                Else
                    OBDon = False                           'Cant Open it
                End If
            Else                                            'OPEN OBD
                If OBDport.IsOpen Then
                    OBDport.Write("ATZ" + Chr(13))            'Restt ALL
                    Sleep(200)
                    OBDport.Write("ATSP0" + Chr(13))           'Send Protocall 0 
                    Sleep(200)
                    OBDport.Write("ATE0" + Chr(13))           'NO Echo
                    Sleep(200)
                    OBDport.Write("ATH0" + Chr(13))           'NO Header
                    Sleep(200)
                    If OBDiT <> "" Then OBDport.Write(OBDiT + Chr(13)) 'Whatever other command to send
                    Sleep(200)
                    If OBDiB <> "" Then
                        Dim DECs(20) As String                                                'The freaking "split" command reduces the size of the array when used?!
                        Dim num As Integer
                        Dim BinDat(20) As Byte
                        DECs = OBDiB.Split(" ")                                'Send the "ping" commend
                        For num = 0 To DECs.Length - 1
                            BinDat(num) = Val(DECs(num))
                            System.Diagnostics.Debug.Write(DECs(num) + ",")
                        Next
                        OBDport.Write(BinDat, 0, DECs.Length)
                        OBDport.Write(Chr(13))
                    End If
                    OBDon = True
                    FOBD.Lverified.Text = "OBD COMS LIVE"
                    FOBD.Bopen.Text = "CLOSE PORT"
                Else
                    OBDon = False                           'Cant Open it
            End If
            End If
        End If
    End Sub
    Private Sub obdisend() Handles FOBD.OBDSndI
        OBDon = False
        Sleep(200)
        OBDport.Write(FOBD.TBSend.Text + Chr(13))           'Just send from the text box directly
        Sleep(200)
        OBDon = True

    End Sub
    Private Sub obdprocess()                                'Triggered by Recieved Data
        Dim IndCMD, IndDAT, ii, Istrt As Integer
        Dim OBDrecs, OBDraw, OBDHex(20) As String
        Dim obdat(40) As Single
        Dim ECUdat(100) As Byte
        'Console.Write("OBD OPEN - ")

        If OBDECU = True Then                          '.......................... EEEE CCCC UUUU  ............
            If OBDport.IsOpen Then
                System.Diagnostics.Debug.Write(".")
                If OBTR = 0 Or OBDport.BytesToRead >= OBTR Then               'Trigger if NO limit or bytes >= Limit
                    Try
                        System.Diagnostics.Debug.Write("!")
                        'OBDrecs = OBDport.ReadExisting
                        OBDport.Read(ECUdat, 0, OBDport.BytesToRead)                    'Do in a TRY catcher as sometime pukes when starting
                        OBDis = "OBD COMS RETURN: " + Str(ECUdat.Length) + " Bytes"
                        System.Diagnostics.Debug.Write("OBD COMS RETURN: " + Str(ECUdat.Length) + " Bytes")
                        '==============Analyze ECU Response ============

                        If ECUdat.Length > 50 Then
                            'Array.Resize(ECUdat, 110)                                      'To prevent a >53 index when 161 isn't the 1st byte
                            'For ii = 0 To ECUdat.Length - 1
                            ' If ECUdat(ii) = 161 Then Istrt = ii : Exit For                'Synch to initialization character (161 for Kozi)
                            ' Next
                            If ECUdat.Length < 50 Then Array.Resize(ECUdat, 110)
                            obdat(16) = ECUdat(Istrt + 6) * 256 + ECUdat(Istrt + 7)                 '1 RPM
                            obdat(17) = 128 - ECUdat(Istrt + 2)                                     '2 Thead
                            obdat(18) = ECUdat(Istrt + 1)                                           '3 TPS
                            obdat(19) = (ECUdat(Istrt + 10) * 256 + ECUdat(Istrt + 11)) / 512 + 8   '4 AFR
                            obdat(20) = ECUdat(Istrt + 43) * 12 / 200                               '5 Vbat
                            obdat(21) = ECUdat(Istrt + 19) / 2.56                                   '6 MAP(kPa)
                            obdat(22) = (ECUdat(Istrt + 30) * 256 + ECUdat(Istrt + 31)) / 32 - 16   '7 Spark Advance(deg)
                            obdat(23) = (ECUdat(Istrt + 16) * 256 + ECUdat(Istrt + 17)) / 600       '8 PW (ms)
                            obdat(24) = ECUdat(Istrt + 9)                                           '9 Duty
                            obdat(25) = (ECUdat(Istrt + 14) * 256 + ECUdat(Istrt + 15)) / 600       '10 O2 Corrected PW
                            obdat(26) = 128 - ECUdat(Istrt + 13)                                    '11 Enrichment?
                            obdat(27) = ECUdat(Istrt + 35)                                          '12 Idle Air
                            'obdat(27) = ECUdat(Istrt + 28) * 256 + ECUdat(Istrt + 29)              '12 InjPhase (similar to IGN)
                            'ECUdat(Istrt + 25)                                                     'Zoning


                            System.Diagnostics.Debug.WriteLine("<")
                            For ii = 16 To 27                                                       'Include Analog Offset, Factor and Averaging
                                System.Diagnostics.Debug.Write(Str(ii - 15) + ":" + Str(obdat(ii)) + " ")
                                a(ii) = a(ii) * afilt(ii) + (1 - afilt(ii)) * (obdat(ii) * Afact(ii) + aof(ii))
                            Next
                            System.Diagnostics.Debug.WriteLine(">")
                            OBDport.DiscardInBuffer()
                        End If
                    Catch ex As Exception
                        OBDis = "OBD COMS ERROR!"
                        If OBDport.IsOpen Then OBDport.DiscardInBuffer()
                    End Try

                End If
            End If
        Else                                            '.......................... OOOOO BBBBB DDDDD ............
            If OBDport.IsOpen Then
                Sleep(200)
                'OBDport.Write("ATE0" + Chr(13))           'NO Echo = 18
                OBDlegnth = OBTR                        'This is a TEMPORARY expediant, to be eliminated

                If OBDSlow Then
                    OBDlegnth = 15
                    If OBDL = 1 Then OBDlegnth = 12
                End If
                If OBDport.BytesToRead >= OBDlegnth Then                    'Did we recieve enough bytes?
                    OBDrecs = OBDport.ReadExisting
                    OBDraw = OBDrecs
                    'OBDrecs = "00E 0: 41 05 83 04 7D 0C " + Chr(13) + "1: 0E 96 10 01 A2 15 98 " + Chr(13) + "2: FF 00 00 00 00 00 00>"
                    OBDrecs = OBDrecs.Replace(vbCr, "").Replace(vbLf, "")
                    OBDrecs = OBDrecs.Replace(">", "")                                  'Some ECU's keep the ">" in E0 mode
                    If InStr(OBDrecs, ":") <> 0 Then
                        OBDrecs += "  "                                                 'Pad with spaces incase ":" is last character
                        OBDrecs = OBDrecs.Substring(InStr(OBDrecs, ":") + 1)            'Take string from 1 position beyond the 1st ":"
                        OBDrecs = OBDrecs.Replace("1: ", "").Replace("2: ", "").Replace("3: ", "")
                        'Remove 0:, 1: 
                    End If
                    If Len(OBDrecs) > 3 Then
                        If OBDrecs.Substring(2) <> "41" Then
                            For i = 1 To Len(OBDrecs) - 2
                                If Mid(OBDrecs, i, 2) = "41" Then
                                    OBDrecs = Mid(OBDrecs, i)                       'Trim to "41xxxxxxxxx"
                                End If
                            Next
                        End If
                    End If
                    'Temporarily Log it as an annotation
                    'Console.Write("Trimmed Incomming: " + OBDRecs)
                    OBDis = "OBD COMS RETURN:" + Str(OBDraw.Length) + " Bytes" + Chr(13) + OBDrecs
                    '==============Analyze OBD Response ============
                    ReDim OBDHex(20)                                                'The freaking "split" command reduces the size of the array when used?!
                    For i = 1 To 20 : OBDHex(i) = "" : Next
                    OBDHex = OBDrecs.Split(" ")
                    Try
                        ann = ""
                        If OBDHex(0) = "41" Then                                                'Only accept string if it starts with the "PID Return" comand of "41"
                            IndCMD = 1 : IndDAT = 2                                             'Point to next comand and next data byte
                            'FOBD.Lout.Text = "GOT 41: "
                            Console.WriteLine("Got 41")

                            Do
                                '0C, 10, 14, 15, 44 are all 2-byte data

                                '   obdat(16)   '1 RPM
                                '   obdat(17)   '2 Thead
                                '   obdat(18)   '3 TPS
                                '   obdat(19)   '4 AFR
                                '   obdat(20)   '5 Vbat
                                '   obdat(21)   '6 MAP
                                '   obdat(22)   '7 Spark Advance?
                                '   obdat(23)   '8 Idle Air
                                '   obdat(24)   '9 Duty?
                                '   obdat(25)   '10 Enrich?
                                '   obdat(26)   '11 Temp2 or Enrichment?
                                '   obdat(27)   '12 Map?
                                Select Case OBDHex(IndCMD)
                                    Case "04"
                                        a(17) = obdat(IndCMD) / 2.55           'Engine Load %
                                    Case "05"
                                        a(18) = obdat(IndCMD) - 40            'CLT
                                    Case "0B"
                                        a(19) = obdat(IndCMD)                 'MAP
                                    Case "0C"
                                        obdat(IndCMD) = obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))
                                        a(20) = obdat(IndCMD) / 4             'RPM (2)
                                        IndDAT += 1                                 'Go to next Data point
                                        IndCMD += 1
                                    Case "0D"
                                        a(21) = obdat(IndCMD)                 'kPh
                                    Case "0E"
                                        a(22) = obdat(IndCMD) / 2 - 64        'Timing Adv
                                    Case "0F"
                                        a(23) = obdat(IndCMD) - 40            'IAT
                                    Case "10"
                                        a(24) = (obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))) / 100           'MAF (2) 
                                        IndDAT += 1                                 'Go to next Data point
                                        IndCMD += 1
                                    Case "11"
                                        a(25) = obdat(IndCMD) / 2.55           'TTL %
                                    Case "14"
                                        obdat(IndCMD) = obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))
                                        a(26) = (Int(obdat(IndCMD) / 256)) / 200    'O2-1 (2 bytes, only use MSB)
                                        IndDAT += 1                                 'Go to next Data point
                                        IndCMD += 1
                                    Case "15"
                                        obdat(IndCMD) = obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))
                                        a(27) = (Int(obdat(IndCMD) / 256)) / 200    ''O2-1 (2 bytes, only use MSB)
                                        IndDAT += 1                                 'Go to next Data point
                                        IndCMD += 1
                                    Case "44"
                                        obdat(IndCMD) = obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))
                                        a(28) = 0.079 * obdat(IndCMD)       'Lambda
                                        IndDAT += 1                                 'Go to next Data point
                                        IndCMD += 1
                                    Case Else
                                        Console.WriteLine("Blown Comand: " + OBDHex(IndCMD))
                                End Select
                                IndDAT = IndDAT + 2                                 'Go to next Data point
                                IndCMD = IndCMD + 2
                            Loop While IndCMD < OBDHex.Length

                        End If
                    Catch ex As Exception
                        Debug.WriteLine("OBD Return Code Translation Error: " + ex.ToString)
                    End Try
                End If

            End If
        End If
    End Sub
    Private Sub OBDCheckErr()
        Dim MIL, Nerr As Integer                                                'MIL on? # of Errors
        Dim errors, obdsends, obdrecs, obdhex(20) As String
        Dim OObd, MultiLine As Boolean                                                     'Old OBD
        OObd = OBDon
        Try

            OBDon = False                                                         'Shut off normal pings here...
            OBDport.Write(Chr(13))                                           'Terminate any existing outgoing characters as a command
            Sleep(OBDtime * 1000 * 2)
            If OBDport.BytesToRead > 0 Then obdrecs = OBDport.ReadExisting 'Clear the pipeline
            obdsends = "0101"                                                  'Got how many error codes?
            OBDport.Write(obdsends + Chr(13))                                'Send request
            'FOBD.TBOut.Text = OBDSends                                          'Show send $ to screen
            Sleep(OBDtime * 1000)

            Dim IndCMD, IndDAT, ThisCMD As Integer
            If OBDport.BytesToRead >= 0 Then

                obdrecs = OBDport.ReadExisting
                'OBDRecs = "41 01 83 07 05 01"                'FOR TEST ONLY !!! DELETE 3-error code
                'OBDRecs = "41 01 82 07 05 01"                'FOR TEST ONLY !!! DELETE 2-error code
                'OBDRecs = "41 01 MIL*128 + # of codes" eg. 129 is MIL on and 1 error, 03 ie 3 errors but no MIL
                errors = obdrecs
                obdrecs = obdrecs.Replace(vbCr, "").Replace(vbLf, "")
                'FOBD.TBIn.Text = OBDRecs
                FOBD.Lverified.Text = obdrecs
                If InStr(obdrecs, "41") >= 1 Then obdrecs = Mid(obdrecs, InStr(obdrecs, "41") + 5, 3) ' Response is 41 01 MIL... so cut the 41 01 out

                obdrecs += "  "
                obdrecs = obdrecs.Replace(" ", "")
                Nerr = Val("&H" & obdrecs)                                            'Convert to Decimal from Hex
                If Nerr > 127 Then
                    MIL = 1
                    Nerr = Nerr - 128
                End If

                'FOBD.TBIn.Text = "MIL:" + Str(MIL) + " Errors:" + Str(Nerr)
                FOBD.Lverified.Text = "MIL:" + Str(MIL) + " Errors:" + Str(Nerr)
                'Send return string to screen
                If Nerr <> 0 Then
                    obdsends = "03"                                                 'Request Error Codes
                    OBDport.Write(obdsends + Chr(13))                            'Send request
                    'FOBD.TBOut.Text = OBDSends                                      'Show send $ to screen
                    Sleep(1000)                                                     '500ms was too short
                    If OBDport.BytesToRead > 0 Then

                        obdrecs = OBDport.ReadExisting

                        'OBDRecs = "43 Xe E1 Oy E2" where X is 0-3=P, 4-7=C, 8-11=B, 12-15=U and error #1 is eE1, eE2 is error #2...

                        ' OBDRecs = "0: 43 03 05 07 05 01 02 " + vbCr + "1: 17 00 00 00 00 00 "                'FOR TEST ONLY !!! DELETE 3-error code
                        'OBDRecs = "43 05 07 05 01"                'FOR TEST ONLY !!! DELETE 2-error code
                        obdrecs = obdrecs.Replace(vbCr, "").Replace(vbLf, "")
                        If InStr(obdrecs, ":") <> 0 Then
                            MultiLine = True
                            obdrecs += "  "                                                 'Pad with spaces incase ":" is last character
                            obdrecs = obdrecs.Substring(InStr(obdrecs, ":") + 1)            'Take string from 1 position beyond the 1st ":"
                            obdrecs = obdrecs.Replace("0: ", "").Replace("1: ", " ").Replace("2: ", " ").Replace("3: ", " ")
                            'Remove 0:, 1: 
                        End If
                        obdrecs = obdrecs.Replace("  ", " ")
                        ' Console.WriteLine("BEFORE trim: " + OBDRecs)
                        FOBD.Lverified.Text = "Err Hex: " + obdrecs
                        If InStr(obdrecs, "43") > 0 Then obdrecs = Mid(obdrecs, InStr(obdrecs, "43") + 3)
                        ' Console.WriteLine("HALF trim: " + OBDRecs)
                        If MultiLine Then obdrecs = Mid(obdrecs, 3).TrimStart 'Skip the next byte as it is the number of lines ( I think)
                        ' Console.WriteLine("FULL trim: " + OBDRecs)
                        ' FOBD.TBIn.Text = OBDRecs

                        ReDim obdhex(20)                                                'The freaking "split" command reduces the size of the array when used?!
                        For ik = 1 To 20 : obdhex(ik) = "" : Next
                        obdhex = obdrecs.Split(" ")
                        errors = ""
                        For ik = 0 To Nerr * 2 - 1 Step 2
                            Console.Write("Error #" + Str(ik) + " (" + obdhex(ik) + "): " + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1) + " =>")
                            Select Case Mid(obdhex(ik), 1, 1)
                                Case "1", "2", "3", "0"
                                    errors = errors + " P" + Mid(obdhex(ik), 1, 1) + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1)
                                Case "4", "5", "6", "7"
                                    errors = errors + " C" + Chr(Asc(Mid(obdhex(ik), 1, 1)) - 4) + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1)
                                Case "8", "9"
                                    errors = errors + " B" + Chr(Asc(Mid(obdhex(ik), 1, 1)) - 8) + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1)
                                Case "A"
                                    errors = errors + " B3" + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1)
                                Case "B"
                                    errors = errors + " B4" + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1)
                                Case "C"
                                    errors = errors + " U1" + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1)
                                Case "D"
                                    errors = errors + " U2" + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1)
                                Case "E"
                                    errors = errors + " U3" + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1)
                                Case "F"
                                    errors = errors + " U4" + Mid(obdhex(ik), 2, 1) + obdhex(ik + 1)
                            End Select
                            Console.WriteLine(errors)
                        Next
                        'Decyphre Error Codes
                    End If 'bytes>0
                End If 'Nerr<>0
            End If
            If Nerr = 0 Then
                MsgBox("MIL:" + Str(MIL) + " Errors:" + Str(Nerr), , "OBD ERRORS")
            Else
                MsgBox("MIL:" + Str(MIL) + " Errors:" + Str(Nerr) + Chr(13) + errors, , "OBD ERRORS")
            End If

        Catch ex As Exception
            Debug.WriteLine("OBD Error Code Reading Error: " + ex.ToString)
            FOBD.Lverified.Text = " ERROR READING CODES " + Chr(13) + errors
            Sleep(200)
        End Try
        OBDon = OObd                                                            'Set back to initial state
    End Sub
    Private Sub obdclearerr()
        Dim OObd As Boolean                                                     'Old OBD
        OObd = OBDon
        'OBDSends = "04"                                                     'Reset Errors
        ' Console.Write("Comands: " + OBDSends)
        If MsgBox("You sure you want to erase the Error codes, and other temporary info?", MsgBoxStyle.YesNo, "OBD CLEAR CODES") = MsgBoxResult.Yes Then
            OBDon = False
            OBDport.Write(Chr(13))
            Sleep(500)
            OBDport.Write("04" + Chr(13))                                     'Send Clear 
            Sleep(500)
            OBDport.Write("04" + Chr(13))
            Sleep(1000)
        End If
        OBDon = OObd
    End Sub

    Private Sub SendPullLoad()
        If Math.Abs(tlastload - time) >= 0.1 Then                               'Schedule for sending Load comands to controller
            If COMport.IsOpen Then COMport.WriteLine(sendcmd) 'Done "slowly" to avoid "cloging up" the coms line
            tlastload = time
            Console.WriteLine("Pull Sending " + sendcmd)
        End If
    End Sub
    Private Sub autofrict()

        '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$  Auto Friction Calc
        tagline = "FRICT. AUTOCAL DYNO RPM = " + a(1).ToString("N0")
        FAC = FAC + 1
        If FAC = 1 Then
            AFRPM = a(1)       'Start RPM
            AFAV1 = 0 : afav2 = 0 : afav3 = 0                'Reset # of averages
            x1 = 0 : x2 = 0 : x3 = 0
            y1 = 0 : y2 = 0 : y3 = 0
        End If
        Select Case a(1)
            Case Is < 0.15 * AFRPM
                If afav3 < 3 Then
                    afav3 = afav3 + 1
                    x3 = x3 + a(1)                     'Add up speeds
                    y3 = y3 - a(5)                     'Add up torques
                End If

            Case Is < 0.5 * AFRPM
                If afav2 < 3 Then
                    afav2 = afav2 + 1
                    x2 = x2 + a(1)                     'Add up speeds
                    y2 = y2 - a(5)                     'Add up torques
                End If
            Case Is < 0.95 * AFRPM
                If AFAV1 < 3 Then
                    AFAV1 = AFAV1 + 1
                    x1 = x1 + a(1)                     'Add up speeds
                    y1 = y1 - a(5)                     'Add up torques
                End If
        End Select
        If a(1) = 0 Then afav3 = 3
        If afav3 = 3 Then                                    'Done when down to 10% of initial speed  
            Dim torqueAF, TrqAvgT As Single
            Dim temp As String
            tagline = tempstr2
            LTagline.Text = tempstr2
            FACon = False                                                   'Shut off AutoCal
            Fmin = 0
            x1 = x1 / 3 : x2 = x2 / 3 : x3 = x3 / 3             'Make Averages
            y1 = y1 / 3 : y2 = y2 / 3 : y3 = y3 / 3

            Console.WriteLine("RPMs: " + Str(x1) + " " + Str(x1) + " " + Str(x3))
            Console.WriteLine("Torques " + Str(y1) + " " + Str(y2) + " " + Str(y3))
            frictoff = (x3 * (x2 * (x2 - x3) * y1 + x1 * (x3 - x1) * y2) + x1 * (x1 - x2) * x2 * y3) / ((x1 - x2) * (x1 - x3) * (x2 - x3))
            frictfact = (x3 * x3 * (y1 - y2) + x1 * x1 * (y2 - y3) + x2 * x2 * (y3 - y1)) / ((x1 - x2) * (x1 - x3) * (x2 - x3))
            FrictQuad = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / ((x1 - x2) * (x1 - x3) * (x2 - x3))
            If FrictQuad < 0 Then
                FrictQuad = 0                       'Generally never going to have a negative quadratic, so if got one, stuff lin approx
                frictfact = (y1 - y3) / (x1 - x3)
                frictof = y3 - x3 * frictfact
            End If
            torqueAF = TrqAvgT
            speedAF = speed1avgt
            frictof = Math.Truncate(frictoff * 1000) / 1000
            frictfact = Int(frictfact * 1000000000) / 1000000000
            FrictQuad = Math.Truncate(FrictQuad * 10000000000.0) / 10000000000.0
            inerteng = ietemp                                                       'Reset Inertie Engine to previous value
            temp = MessageBox.Show("Previous: Offset = " + frictofft.ToString("n3") + "   Factor = " + frictfactt.ToString("n5") + "   QUAD = " + frictquadt.ToString("E3") + Chr(13) + "Current:  Offset = " + frictoff.ToString("n3") + "   Factor = " + frictfact.ToString("n5") + "   QUAD = " + FrictQuad.ToString("E3") + Chr(13) + "   OK to Accept            CANCEL to Reject", "Friction Auto Calibration Results", MessageBoxButtons.OKCancel)
            If temp = 2 Then
                frictof = frictofft
                frictfact = frictfactt
                FrictQuad = frictquadt
            End If
        End If
        '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ Auto Friction Cal

    End Sub
    Private Sub readcalsfile()
        Dim lines, TGT As Integer        'Number of lines read (more or less), Temp Gage Type
        Dim arg, s, dummy, tempstr As String           'Arguments
        'MsgBox("Loading Calibrations from: " + calfile)
        lines = 0
        chan = 0
        TGT = 0
        gagemax = 0
        Try
            Using FileRead As New System.IO.StreamReader(calfile)
                dummy = FileRead.ReadLine()
                lines = 1
                Do
                    s = FileRead.ReadLine()
                    If Len(s) > 2 Then
                        If Mid(s, 1, 3) = "end" Or Mid(s, 1, 3) = "END" Or Mid(s, 1, 3) = "End" Then s = "END" ' s.TrimEnd(",") 'This is just to catch when edits with Excell leave spare commas at end of line
                    End If
                    If s <> "END" Then
                        arg = Mid(s, InStr(s, ",") + 1, Len(s) - 1) 'Eliminate first element

                        s = Mid(s, 1, InStr(s, ",") - 1) 'Eliminate first element
                        s = s.ToLower
                        lines = lines + 1
                        Select Case s

                            Case "ver"
                                ver = arg
                            Case "tagline"
                                tagline = arg.TrimEnd(",")
                            Case "type"
                                dynotype = arg.TrimEnd(",")
                            Case "rollerdiam"
                                diam = Val(arg)
                            Case "dynomaxrpm"
                                rpmdynomax = Val(arg)
                            Case "dynomaxtrq"
                                TorqDynoMax = Val(arg)
                            Case "inertia"
                                inert = Val(arg)
                            Case "frictofs"
                                frictof = Val(arg)
                            Case "frictfact"
                                frictfact = Val(arg)
                            Case "frictquad"
                                FrictQuad = Val(arg)
                            Case "comport"
                                compt = Val(arg)
                            Case "combr"
                                comBR = Val(arg)
                            Case "pingsec"
                                deltaT = Val(arg)
                            Case "gearratio"
                                GR = Val(arg)
                            Case "logint"
                                saveint = Val(arg)
                            Case "run"
                                run = Val(arg)
                                ' Case "metric"
                                '     If arg = "True" Or arg = "False" Then metric = arg

                            Case "analogref"
                                AnaRef = Val(arg)
                            Case "skincolor"
                                Cskin = Val(arg)
                            Case "backcolor"
                                Cback = Val(arg)
                            Case "screenmagnification"
                                MAG = Val(arg)
                            Case "inerteng"
                                inerteng = Val(arg)
                                'Case "rpmload"             8888
                                RPMload = Val(arg)
                            Case "manload"
                                MANLoad = Val(arg)
                            Case "rpm1"
                                rpm1 = Val(arg)
                            Case "rpm2"
                                rpm2 = Val(arg)
                            Case "load1"
                                load1 = Val(arg)
                            Case "load2"
                                load2 = Val(arg)
                            Case "loadmode"
                                loadMode = Val(arg)
                            Case "rpmode"
                                rpmode = Val(arg)
                            Case "pullanach"
                                pGrafAnaCh = Val(arg)
                            Case "datapath"
                                DataPath = arg.TrimEnd(",")
                            Case "pull2rpm"
                                peng2 = Val(arg)
                            Case "pullduration"
                                PullDur = Val(arg)
                            Case "runfile"
                                runfile = arg.TrimEnd(",")
                            Case "tunecfg"
                                TuneCal = arg.TrimEnd(",")
                                'Console.WriteLine("Load1: " + TuneCal)
                            Case "obdport"
                                OBDportnum = Val(arg)
                            Case "obdbaud"
                                OBDbr = Val(arg)
                            Case "obdparity"
                                OBDp = arg
                            Case "obdinittext"
                                OBDiT = arg
                            Case "obdinitbyte"
                                OBDiB = arg
                            Case "obdpingbyte"
                                OBDpB = arg
                            Case "obdpingtext"
                                OBDpT = arg
                            Case "obdbytes2read"
                                OBTR = Val(arg)
                            Case "obdon"
                                If arg = "True" Or arg = "False" Then OBDon = arg
                            Case "obdchon"
                                OBDchon(Val(arg)) = True
                            Case "obdecu"
                                If arg = "True" Or arg = "False" Then OBDECU = arg
                            Case "obdrep"
                                If arg = "True" Or arg = "False" Then OBDREP = arg
                            Case "obdslow"
                                If arg = "True" Or arg = "False" Then OBDSlow = arg

                            Case "grafleft" : GRAFx = Val(arg)
                            Case "graftop" : GRAFy = Val(arg)
                            Case "grafheight" : GRAFh = Val(arg)
                            Case "grafwidth" : GRAFw = Val(arg)

                            Case "xgraf" : xgraf = Val(arg)
                            Case "y1graf" : y1graf = Val(arg)
                            Case "y2graf" : y2graf = Val(arg)
                            Case "y3graf"
                                y3graf = Val(arg)
                            Case "y4graf"
                                y4graf = Val(arg)
                            Case "grafxmin"
                                GrX1 = Val(arg)
                            Case "grafymax"
                                GrY2 = Val(arg)
                            Case "grafy2max"
                                Gr2Y2 = Val(arg)
                            Case "insttop" : INSTTop = Val(arg)
                            Case "instleft" : INSTLeft = Val(arg)
                            Case "instwidth" : INSTWidth = Val(arg)
                            Case "instheight" : INSTHeight = Val(arg)
                            Case "grafmanx"
                                If arg = "True" Or arg = "False" Then GManX = arg
                            Case "grafmany"
                                If arg = "True" Or arg = "False" Then GmanY = arg
                            Case "grafman2y"
                                If arg = "True" Or arg = "False" Then Gman2Y = arg
                            Case "grafilter"
                                GraFilt = Val(arg)
                            Case "saeauto"
                                If arg = "True" Or arg = "False" Then saeauto = arg
                            Case "saecomp"
                                If arg = "True" Or arg = "False" Then saecomp = arg
                            Case "temp"
                                saetemp = Val(arg)
                            Case "pres"
                                saepress = Val(arg)
                            Case "rh"
                                saeRH = Val(arg)
                            Case "tunefile"
                                TuneCal = arg
                            Case "logo"
                                logo = Val(arg)
                            Case "shopname"
                                ShopName = arg
                            Case "shopcont"
                                ShopCont = arg
                            Case "acelgainp"
                                KPa = Val(arg)
                            Case "language"
                                lang = Val(arg)
                            Case "powerunit"
                                PowUnit = Val(arg)
                            Case "totalkm"
                                KMt = Val(arg)
                            Case "autotare"
                                If arg = "True" Or arg = "False" Then autotare = arg
                            Case "autoload"
                                If arg = "True" Or arg = "False" Then autoload = arg
                            Case "autosave"
                                If arg = "True" Or arg = "False" Then AutoSave = arg
                            Case "autograf"
                                If arg = "True" Or arg = "False" Then instgraf = arg
                            Case "autocomp"
                                If arg = "True" Or arg = "False" Then autograf = arg
                            Case "break2point"
                                If arg = "True" Or arg = "False" Then brek2p = arg
                            Case "breakramp"
                                If arg = "True" Or arg = "False" Then brekramp = arg
                            Case "breakstop"
                                If arg = "True" Or arg = "False" Then brekstop = arg
                            Case "remote"
                                If arg = "True" Or arg = "False" Then remote = arg
                            Case "breakload"
                                brekload = Val(arg)
                            Case "remotechannel"
                                remch = Val(arg)
                            Case "screenrefresh"
                                refresh = Val(arg)
                            Case "tracecolor1" : Tcol(1) = Val(arg) ': MsgBox("TC1")
                            Case "tracecolor2" : Tcol(2) = Val(arg)
                            Case "tracecolor3" : Tcol(3) = Val(arg)
                            Case "tracecolor4" : Tcol(4) = Val(arg)
                            Case "tracecolor5" : Tcol(5) = Val(arg)
                            Case "tracecolor6" : Tcol(6) = Val(arg)
                            Case "tracecolor7" : Tcol(7) = Val(arg)
                            Case "tracecolor8" : Tcol(8) = Val(arg)
                            Case "tracecolor9" : Tcol(9) = Val(arg)
                            Case "tracecolor10" : Tcol(10) = Val(arg)
                            Case "tracecolor11" : Tcol(11) = Val(arg)
                            Case "tracecolor12" : Tcol(12) = Val(arg)
                            Case "xsize"                'X size of form
                                fx = Val(arg)
                                If fx > 100 Then Me.Width = fx
                            Case "ysize"
                                fy = Val(arg)             'Y size of form
                                If fy > 100 Then Me.Height = fy
                            Case "rpmref"
                                'Console.WriteLine(">" + arg + "<")
                                ' If arg = " Dyno" Then : TogRPM.Text = "RPM: DYNO" : Else : TogRPM.Text = "RPM: ENGINE" : End If
                            Case "channels"
                                'chan = Val(arg)
                            Case "ch"
                                chan = chan + 1                             'Inc no of channels
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                ' MsgBox(s + " : " + arg)
                                alab(chan) = s
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                ' MsgBox(s + " : " + arg)
                                auni(chan) = s
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                '  MsgBox(s + " : " + arg)
                                amin(chan) = Val(s)
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element

                                a1(chan) = Val(s)
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                a2(chan) = Val(s)
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element

                                amax(chan) = Val(s)
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                'MsgBox(s + " : " + arg)
                                Afact(chan) = Val(s)
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                'MsgBox(s + " : " + arg)
                                aof(chan) = Val(s)
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                ' MsgBox(s + " : " + arg)
                                afilt(chan) = Val(s)
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                ' MsgBox("Next " + s + " : " + arg)
                                amajtix(chan) = Val(s)
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                ' MsgBox("Last " + s + " : " + arg)
                                amintix(chan) = Val(s)
                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                adec(chan) = arg
                            Case "gages"
                                'No need to record the number of gages, as we load 1-by-1
                            Case "gage"
                                'Inc no of gages

                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                TGT = Val(s)                        'Get gage type abnd perform "click" adding it
                                gagemax = gagemax + 1
                                gSUdone = False

                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                Gch(gagemax) = Val(s)

                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                gage(gagemax).Left = Val(s)

                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                gage(gagemax).Top = Val(s)

                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                gage(gagemax).Width = Val(s)

                                arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                's = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                gage(gagemax).Height = Val(arg)

                                Select Case TGT
                                    Case 1
                                        l(gagemax).Font = New Font("Arial", 6, FontStyle.Bold)
                                        Call addg1()            'Gage
                                    Case 2
                                        l(gagemax).Font = New Font("Arial", 8, FontStyle.Bold)
                                        Call addg2()           'Graf
                                    Case 3
                                        l(gagemax).Font = New Font("Arial", 6, FontStyle.Bold)
                                        Call addg3()              'LED
                                    Case 4
                                        l(gagemax).Font = New Font("Arial", 8, FontStyle.Bold)
                                        Call addg4()            'Vert
                                    Case 5
                                        l(gagemax).Font = New Font("Arial", 8, FontStyle.Bold)
                                        Call addg5()            'Horz
                                    Case 6
                                        n(gagemax).Font = New Font("Arial", 16, FontStyle.Bold)
                                        l(gagemax).Font = New Font("Arial", 8, FontStyle.Bold)
                                        Call addg6()            'Number
                                        'MsgBox("6")
                                    Case 7
                                        l(gagemax).Font = New Font("Arial", 6, FontStyle.Bold)
                                        Call addg7()            'Gage Bulat
                                End Select

                        End Select
                    End If
                Loop Until s = "END"
                If autoload = True Then BAL.BackColor = butcol1
                If instgraf = True Then Binst.BackColor = butcol1
                If autograf = True Then BAG.BackColor = butcol1
                If AutoSave Then BAS.BackColor = butcol1
                'If metric = True Then PowUnit = 0 '1 for Metric
                TBDynoType.Text = dynotype
                FileRead.Close()            'Gotta use these 2 lines to close the file after reading
                FileRead.Dispose()
            End Using
        Catch ex As Exception
            '"being used" If got this then tell 'em to close it n re-run
            If InStr(ex.Message.ToString(), "being used") > 0 Then
                MsgBox(calfile + " is being help open by another application." + Chr(13) + "Please CLOSE it and re-run Dynomonitor.")
                End
            Else
                'MsgBox("Had a problem reading the Calibration file at Line " + Str(lines) + Chr(13) + s + " : " + arg)
                tempstr = InputBox("Had trouble after reading " + Str(lines) + " lines from: " + calfile + Chr(13) + "This may be because of a recent software update." + Chr(13) + "Continue using current values?", "Calibration File Problem", "Yes")
                If tempstr <> "" Then tempstr = Mid(tempstr, 1, 1)
                If tempstr = "Y" Or tempstr = "y" Then
                    'Good to go!
                Else
                    Call defaults()
                End If
            End If
        Finally
        End Try
        If y4graf = 0 Then y4graf = 9 'Just to avoid error
        ButRun.Text = "RUN: " + Str(run)
    End Sub
    Private Sub loadcals()
        'Read Dynocal File========================================================================

        If System.IO.File.Exists(calfile) = True Then
            Call readcalsfile()
        Else
            Dim LoadFileBox As New OpenFileDialog
            With LoadFileBox
                .FileName = calfile
                .DefaultExt = "CSV"
                .Filter = "CSV|*.csv"
                .Title = "Calibration Data File"
                '  .InitialDirectory = applicationSettingsDirectory
                Dim result = .ShowDialog()
                If result = DialogResult.OK Then
                    calfile = .FileName
                End If
            End With
            If System.IO.File.Exists(calfile) = True Then
                Call readcalsfile()
            Else
                MsgBox(calfile + " Does Not Exist. Using Defaults.")            'Use Defaults and create DynoCal file
                Call defaults()
            End If
        End If
        'Console.WriteLine("Loaded: " + TuneCal)
        'Setup Screen Boxes
        '  MsgBox("AcelFact" + Str(acel))
        '  If TogRPM.Text = "RPM: ENGINE" Then
        ' metric = False : If PowUnit = 0 Then metric = True
        If rpmode = 1 Then
            TogRPM.Text = rpms(lang, 1)
        Else
            TogRPM.Text = rpms(lang, 2)
            If rpmode = 3 Then TogRPM.Text = rpms(lang, 3)
            dGR = GR
            Ldyno2.Text = "Engine"
        End If
        ' TBMan.Text = Str(LoadMan)
        vehID = runfile.Substring(runfile.LastIndexOf("\") + 1)         'Seperate our Vehicle ID
        ButRunFile.Text = vehID
        TBRunFile.Text = runfile
        Tbrpm1.Text = Str(Int(rpm1 * dGR))
        TBRPM2.Text = Str(Int(rpm2 * dGR))
        TBLoad1.Text = Str(load1)
        TBLoad2.Text = Str(load2)
        TBAcel.Text = Str(PullDur)
        TBrpmSP.Text = Str(Int(RPMload * dGR))
        TBMan.Text = "0" 'Str(MANLoad)
        PictureBox1.Image = My.Resources.Logo
        If logo = 1 Then PictureBox1.Image = My.Resources.CBK
        If logo = 2 Then PictureBox1.Image = My.Resources.KZR
        Select Case loadMode
            Case 1 : CBMan.Checked = True
            Case 2 : CBRPM.Checked = True
            Case 3 : CB2Point.Checked = True
            Case 4 : CBAcel.Checked = True
        End Select
    End Sub
    Private Sub savecal()
        Dim tempstr As String
        Dim Ngages As Integer
        ' MsgBox("Saving Calfile " + CalFile)
        If System.IO.File.Exists(calfile) Then                                      'Generate backup file
            Dim FileNameInfo As New System.IO.FileInfo(calfile)
            Dim path As String = FileNameInfo.DirectoryName
            Dim fileName As String = FileNameInfo.Name
            'MsgBox("Trying to save Cal File")
            tempstr = fileName                                                      'Seperate file name for modification
            If InStr(calfile, ".") Then
                tempstr = Mid(tempstr, 1, InStr(tempstr, ".") - 1) + "BAK.CSV"
            Else
                tempstr = tempstr + "BAK.CSV"
            End If
            ' MsgBox(CalFile + Chr(13) + tempstr + Chr(13) + path + "\" + tempstr + Chr(13) + Str(System.IO.File.Exists(path + "\" + tempstr)))
            If System.IO.File.Exists(path + "\" + tempstr) Then My.Computer.FileSystem.DeleteFile(path + "\" + tempstr) 'Kill previous backup
            My.Computer.FileSystem.RenameFile(calfile, tempstr)                                         'Rename current to backup
        End If
        Using FileWrite As New System.IO.StreamWriter(calfile)
            FileWrite.WriteLine("Focus Applied Technologies DynoMas Calibration File, DC5")                    'Header
            FileWrite.WriteLine("TagLine," + tagline)                           'Permanent Tag Line
            FileWrite.WriteLine("Ver," + ver)
            FileWrite.WriteLine("Type," + dynotype)
            FileWrite.WriteLine("RollerDiam," + Str(diam))
            FileWrite.WriteLine("DynoMaxRPM," + Str(rpmdynomax))
            FileWrite.WriteLine("DynoMaxTrq," + Str(TorqDynoMax))
            FileWrite.WriteLine("Inertia," + Str(inert))
            FileWrite.WriteLine("FrictOFS," + Str(frictof))
            FileWrite.WriteLine("FrictFact," + Str(frictfact))
            FileWrite.WriteLine("FrictQuad," + Str(FrictQuad))                               'Was formerly PPR (unused)
            FileWrite.WriteLine("ComPort," + Str(compt))
            FileWrite.WriteLine("ComBR," + Str(comBR))
            FileWrite.WriteLine("PingSec," + Str(deltaT))                                   'Ping Rate (sec)
            FileWrite.WriteLine("GearRatio," + Str(GR))

            FileWrite.WriteLine("AnalogRef," + Str(AnaRef))
            FileWrite.WriteLine("LastCal," + lastcal)
            FileWrite.WriteLine("LogInt," + Str(saveint))
            'FileWrite.WriteLine("Metric," + Str(metric))
            FileWrite.WriteLine("InertEng," + Str(inerteng))
            FileWrite.WriteLine("RPMLoad," + Str(RPMload))
            FileWrite.WriteLine("ManLoad," + Str(MANLoad))
            FileWrite.WriteLine("RPM1," + Str(rpm1))
            FileWrite.WriteLine("RPM2," + Str(rpm2))
            FileWrite.WriteLine("Load1," + Str(load1))
            FileWrite.WriteLine("Load2," + Str(load2))
            FileWrite.WriteLine("Run," + Str(run))
            FileWrite.WriteLine("InstTop," + Str(INSTTop))
            FileWrite.WriteLine("InstLeft," + Str(INSTLeft))
            FileWrite.WriteLine("InstWidth," + Str(INSTWidth))
            FileWrite.WriteLine("InstHeight," + Str(INSTHeight))
            FileWrite.WriteLine("LoadMode," + Str(loadMode))
            FileWrite.WriteLine("RPMode," + Str(rpmode))
            FileWrite.WriteLine("PullAnaCh," + Str(pGrafAnaCh))
            FileWrite.WriteLine("DataPath," + DataPath)
            FileWrite.WriteLine("PullRPM2," + Str(peng2))
            FileWrite.WriteLine("PullDuration," + Str(PullDur))
            FileWrite.WriteLine("TuneCfg," + TuneCal)
            FileWrite.WriteLine("TuneAcc," + Str(Tacc))
            FileWrite.WriteLine("Temp," + Str(saetemp))
            FileWrite.WriteLine("Pres," + Str(saepress))
            FileWrite.WriteLine("RH," + Str(saeRH))
            FileWrite.WriteLine("SAEAuto," + Str(saeauto))
            FileWrite.WriteLine("SAEComp," + Str(saecomp))
            FileWrite.WriteLine("RunFile," + runfile)
            FileWrite.WriteLine("GrafLeft," + Str(GRAFx))
            FileWrite.WriteLine("GrafTop," + Str(GRAFy))
            FileWrite.WriteLine("GrafHeight," + Str(GRAFh))
            FileWrite.WriteLine("GrafWidth," + Str(GRAFw))
            FileWrite.WriteLine("Xgraf," + Str(xgraf))
            FileWrite.WriteLine("Y1graf," + Str(y1graf))
            FileWrite.WriteLine("Y2graf," + Str(y2graf))
            FileWrite.WriteLine("Y3graf," + Str(y3graf))
            FileWrite.WriteLine("Y4graf," + Str(y4graf))
            FileWrite.WriteLine("SkinColor," + Str(Cskin))
            FileWrite.WriteLine("BackColor," + Str(Cback))
            FileWrite.WriteLine("TraceColor1," + Str(Tcol(1)))
            FileWrite.WriteLine("TraceColor2," + Str(Tcol(2)))
            FileWrite.WriteLine("TraceColor3," + Str(Tcol(3)))
            FileWrite.WriteLine("TraceColor4," + Str(Tcol(4)))
            FileWrite.WriteLine("TraceColor5," + Str(Tcol(5)))
            FileWrite.WriteLine("TraceColor6," + Str(Tcol(6)))
            FileWrite.WriteLine("TraceColor7," + Str(Tcol(7)))
            FileWrite.WriteLine("TraceColor8," + Str(Tcol(8)))
            FileWrite.WriteLine("TraceColor9," + Str(Tcol(9)))
            FileWrite.WriteLine("TraceColor10," + Str(Tcol(10)))
            FileWrite.WriteLine("TraceColor11," + Str(Tcol(11)))
            FileWrite.WriteLine("ScreenMagnification," + Str(MAG))
            FileWrite.WriteLine("GrafXMin," + Str(GrX1))
            FileWrite.WriteLine("GrafYMax," + Str(GrY2))
            FileWrite.WriteLine("GrafY2Max," + Str(Gr2Y2))
            FileWrite.WriteLine("GrafManX," + Str(GManX))
            FileWrite.WriteLine("GrafManY," + Str(GmanY))
            FileWrite.WriteLine("GrafMan2Y," + Str(Gman2Y))

            FileWrite.WriteLine("ObdPort," + Str(OBDportnum))
            FileWrite.WriteLine("ObdBaud," + Str(OBDbr))
            FileWrite.WriteLine("ObdParity," + OBDp)
            FileWrite.WriteLine("ObdInitText," + OBDiT)
            FileWrite.WriteLine("ObdInitByte," + OBDiB)
            FileWrite.WriteLine("ObdPingByte," + OBDpB)
            FileWrite.WriteLine("obdPingText," + OBDpT)
            FileWrite.WriteLine("obdBytes2Read," + Str(OBTR))
            FileWrite.WriteLine("ObdOn," + Str(OBDon))
            FileWrite.WriteLine("OBDSlow," + Str(OBDSlow))
            For i = 1 To 12
                If OBDchon(i) = True Then FileWrite.WriteLine("OBDchON," + Str(i))
            Next
            FileWrite.WriteLine("ObdEcu," + Str(OBDECU))
            FileWrite.WriteLine("ObdRep," + Str(OBDREP))

            FileWrite.WriteLine("GraFilter," + Str(GraFilt))
            FileWrite.WriteLine("TuneFile," + TuneCal)
            FileWrite.WriteLine("AutoTare," + Str(autotare))
            FileWrite.WriteLine("AutoSave," + Str(AutoSave))
            FileWrite.WriteLine("AutoGraf," + Str(instgraf))
            FileWrite.WriteLine("AutoLoad," + Str(autoload))
            FileWrite.WriteLine("AutoComp," + Str(autograf))
            FileWrite.WriteLine("AcelGainP," + Str(KPa))
            FileWrite.WriteLine("Logo," + Str(logo))
            FileWrite.WriteLine("ShopName," + ShopName)
            FileWrite.WriteLine("ShopCont," + ShopCont)
            FileWrite.WriteLine("Language," + Str(lang))
            FileWrite.WriteLine("Xsize," + Str(Me.Width))
            FileWrite.WriteLine("Ysize," + Str(Me.Height))
            FileWrite.WriteLine("Break2Point," + Str(brek2p))
            FileWrite.WriteLine("BreakRamp," + Str(brekramp))
            FileWrite.WriteLine("BreakStop," + Str(brekstop))
            FileWrite.WriteLine("BreakLoad," + Str(brekload))
            FileWrite.WriteLine("Remote," + Str(remote))
            FileWrite.WriteLine("ScreenRefresh," + Str(refresh))
            FileWrite.WriteLine("RemoteChannel," + Str(remch))
            FileWrite.WriteLine("PowerUnit," + Str(PowUnit))
            FileWrite.WriteLine("TotalKm," + Str(KMt))
            If rpmode = 1 Then
                FileWrite.WriteLine("RpmRef, Dyno")
            Else
                FileWrite.WriteLine("RpmRef, Engine")           'This is for BOTH Calc'd and RPM2 engine RPMs
            End If
            FileWrite.WriteLine("Channels,15, Label, Units, Min, A1, A2, Max, Fact, Ofs, Filt, Major, Minor, Dec")
            For Me.i = 1 To 27                                              'Analog Labels and Factors
                FileWrite.WriteLine("CH," + alab(Me.i) + "," + auni(Me.i) + "," + Str(amin(Me.i)) + "," + Str(a1(Me.i)) + "," + Str(a2(Me.i)) + "," + Str(amax(Me.i)) + "," + Str(Afact(Me.i)) + "," + Str(aof(Me.i)) + "," + Str(afilt(Me.i)) + "," + Str(amajtix(Me.i)) + "," + Str(amintix(Me.i)) + "," + adec(Me.i))
            Next
            Ngages = 0
            For Me.i = 1 To gagemax
                If ghide(Me.i) = False Then Ngages = Ngages + 1 'Only count if not hidden
            Next
            FileWrite.WriteLine("Gages," + Str(Ngages))
            Ngages = 0
            For Me.i = 1 To gagemax
                If ghide(Me.i) = False Then
                    Ngages = Ngages + 1 'Only count if not hidden
                    FileWrite.WriteLine("GAGE," + Str(gtype(Me.i)) + "," + Str(Gch(Me.i)) + "," + Str(gage(Me.i).Left) + "," + Str(gage(Me.i).Top) + "," + Str(gage(Me.i).Width) + "," + Str(gage(Me.i).Height))
                End If
            Next
            FileWrite.WriteLine("END")                                  'Always tell 'e when to stop
            FileWrite.Close()
            FileWrite.Dispose()

        End Using
    End Sub
    Private Sub Gage_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)  ' Load neddle or other changing parts of the "gage"
        Dim j, k As Integer
        Dim LEDoff As New Pen(Color.FromArgb(110, 90, 90))
        If gpi <> 0 Then            'This re-draws graphice as needed on the various "gages" (the bar color data, number, needel or graf)
            Select Case gtype(gpi)
                Case 1                              'Gage (half bulat)
                    Dim center As Point
                    Dim baseL, baseR, BaseM, tip As Point
                    Dim X1 As Integer = (gage(gpi).Width / 2)             'Y is 0 at top
                    Dim y1 As Integer = gage(gpi).Height
                    Dim aledR, aledL, y1led, y2led As Integer
                    Dim AnaTrunk As Double
                    Dim aPen = New Pen(Color.DarkRed)
                    aPen.Width = 2.0F
                    Dim BPen = New Pen(Color.Black)
                    BPen.Width = 2.0F
                    center.X = X1
                    center.Y = 0.95 * y1
                    y1led = 0.03 * y1
                    y2led = 0.17 * y1
                    If amax(Gch(gpi)) <> 0 Then
                        AnaTrunk = a(Gch(gpi)) / amax(Gch(gpi))
                    Else
                        AnaTrunk = 0
                    End If

                    If a(Gch(gpi)) > amax(Gch(gpi)) Then
                        AnaTrunk = 1.01
                        AON(gpi) = 1                        'turn LED ON           
                        'Console.WriteLine("LED ON " + Str(gpi))
                        aledR = 2 * X1 * 0.98
                        For k = y1led To y2led                                          '7777 This is sloppy and should be replaced by something faster
                            aledL = 1.16 * X1 + k * 2
                            e.Graphics.DrawLine(Pens.Red, aledL, k, aledR, k)
                        Next k
                    End If

                    tip.X = center.X - 0.9 * X1 * Math.Cos(Math.PI * AnaTrunk)
                    tip.Y = center.Y - 0.85 * y1 * Math.Sin(Math.PI * AnaTrunk)
                    BaseM.X = center.X - 0.08 * X1 * Math.Cos(Math.PI * AnaTrunk)
                    BaseM.Y = center.Y - 0.08 * y1 * Math.Sin(Math.PI * AnaTrunk)
                    baseL.X = center.X - 0.08 * X1 * Math.Cos(Math.PI * AnaTrunk - 0.07)
                    baseL.Y = center.Y - 0.08 * y1 * Math.Sin(Math.PI * AnaTrunk - 0.07)
                    baseR.X = center.X - 0.08 * X1 * Math.Cos(Math.PI * AnaTrunk + 0.07)
                    baseR.Y = center.Y - 0.08 * y1 * Math.Sin(Math.PI * AnaTrunk + 0.07)
                    e.Graphics.DrawLine(aPen, BaseM, tip)
                    e.Graphics.DrawLine(BPen, baseL, tip)
                    e.Graphics.DrawLine(BPen, baseR, tip)

                Case 2                                              'Graf
                    Dim lhs, rhs As Point
                    Dim apen = New Pen(Color.LimeGreen)
                    Dim bpen = New Pen(Color.Black)
                    Dim cpen = New Pen(Color.White)
                    Dim iw, ih As Integer
                    apen.Width = gage(gpi).Height * 0.02
                    'bpen.Width = gage(gpi).Height * 0.65

                    grf(gpi).Left = gage(gpi).Left + 0.02 * gage(gpi).Width         'Setup the position of the graphic overlay

                    grf(gpi).Top = gage(gpi).Top + Int(0.3 * gage(gpi).Height)
                    grf(gpi).Width = gage(gpi).Width * 0.96
                    grf(gpi).Height = gage(gpi).Height * 0.66

                    iw = grf(gpi).Image.Width

                    tempbmp = New Bitmap(grf(gpi).Image.Width, grf(gpi).Image.Height)               'Shift Graph Data
                    Dim tgr As Graphics = Graphics.FromImage(tempbmp)
                    orgpic = New Rectangle(1, 0, iw, grf(gpi).Height)                               'Make the first number bigger for faster scrolling
                    ofspic = New Rectangle(0, 0, iw, grf(gpi).Height)
                    tgr.DrawImage(grf(gpi).Image, ofspic, orgpic, GraphicsUnit.Pixel)
                    grf(gpi).Image = tempbmp                                                        'These few lines shift the image in Grafovr

                    'Dim sgr As Graphics = Graphics.FromImage(grf(gpi).Image)             'This doesn't reall mean "from" it basically draws ON this image
                    ih = grf(gpi).Height
                    lhs.X = grf(gpi).Width : lhs.Y = 0
                    rhs.X = lhs.X : rhs.Y = grf(gpi).Height
                    tgr.DrawLine(bpen, lhs, rhs)                                                    'Black out last Vert line after shift
                    lhs.X = lhs.X - 2                                                              'Back up 1 pixel to draw line
                    gdatlast = gcnt(gpi) - 1
                    If gdatlast = 0 Then gdatlast = 300 'To avoid usuing "0" as data
                    lhs.Y = ih * (1 - (gdat(gpi, gdatlast) - amin(Gch(gpi))) / (amax(Gch(gpi)) - amin(Gch(gpi)))) 'Skip the -1 to just do dots
                    rhs.Y = ih * (1 - (gdat(gpi, gcnt(gpi)) - amin(Gch(gpi))) / (amax(Gch(gpi)) - amin(Gch(gpi))))
                    tgr.DrawLine(apen, lhs, rhs)                                                    'Draw new point
                    grf(gpi).Image = tempbmp                                                        'Assign to the Picture box

                    lhs.X = 0.02 * grf(gpi).Width 'Default in case of max div by 0
                    rhs.X = grf(gpi).Width 'Default in case of max div by 0  'Draws Hors White line at Alarm 2 Level
                    lhs.Y = ih * (1 - (a1(Gch(gpi)) - amin(Gch(gpi))) / (amax(Gch(gpi)) - amin(Gch(gpi))))
                    rhs.Y = lhs.Y
                    tgr.DrawLine(cpen, lhs, rhs)             'Draws Horz White line at Alarm 1 value
                    lhs.Y = ih * (1 - (a2(Gch(gpi)) - amin(Gch(gpi))) / (amax(Gch(gpi)) - amin(Gch(gpi))))
                    rhs.Y = lhs.Y
                    tgr.DrawLine(cpen, lhs, rhs)

                Case 4                                              'Vert

                    Dim lhs, rhs As Point
                    Dim aVpen = New Pen(Color.LimeGreen)
                    Dim bVpen = New Pen(Color.Yellow)
                    Dim cVpen = New Pen(Color.Red)
                    aVpen.Width = 0.9 * gage(gpi).Width
                    bVpen.Width = aVpen.Width
                    cVpen.Width = aVpen.Width
                    lhs.X = 0.5 * gage(gpi).Width
                    rhs.X = 0.5 * gage(gpi).Width
                    lhs.Y = 0.99 * gage(gpi).Height 'Default in case of max div by 0
                    rhs.Y = lhs.Y
                    If amax(Gch(gpi)) <> 0 Then lhs.Y = (0.99 - (a(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Height
                    If a(Gch(gpi)) < a1(Gch(gpi)) Then
                        e.Graphics.DrawLine(aVpen, lhs, rhs)
                    Else
                        lhs.Y = (0.99 - (a1(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Height
                        e.Graphics.DrawLine(aVpen, lhs, rhs)
                        If a(Gch(gpi)) < a2(Gch(gpi)) Then
                            rhs.Y = lhs.Y
                            lhs.Y = (0.99 - (a(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Height
                            e.Graphics.DrawLine(bVpen, lhs, rhs)
                        Else
                            rhs.Y = lhs.Y
                            lhs.Y = (0.99 - (a2(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Height
                            e.Graphics.DrawLine(bVpen, lhs, rhs)
                            rhs.Y = lhs.Y
                            lhs.Y = (0.99 - (a(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Height
                            e.Graphics.DrawLine(cVpen, lhs, rhs)
                        End If
                    End If

                Case 5                                              'Horz
                    Dim lhs, rhs As Point
                    Dim apen = New Pen(Color.LimeGreen)
                    Dim bpen = New Pen(Color.Yellow)
                    Dim cpen = New Pen(Color.Red)
                    apen.Width = gage(gpi).Height * 0.65
                    bpen.Width = apen.Width
                    cpen.Width = apen.Width
                    lhs.Y = 0.64 * gage(gpi).Height
                    rhs.Y = 0.64 * gage(gpi).Height
                    lhs.X = 0.02 * gage(gpi).Width 'Default in case of max div by 0
                    rhs.X = lhs.X
                    If amax(Gch(gpi)) <> 0 Then lhs.X = (0.02 + (a(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Width
                    If a(Gch(gpi)) < a1(Gch(gpi)) Then
                        e.Graphics.DrawLine(apen, lhs, rhs)
                    Else
                        lhs.X = (0.02 + (a1(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Width
                        e.Graphics.DrawLine(apen, lhs, rhs)
                        If a(Gch(gpi)) < a2(Gch(gpi)) Then
                            rhs.X = lhs.X
                            lhs.X = (0.02 + (a(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Width
                            e.Graphics.DrawLine(bpen, lhs, rhs)
                        Else
                            rhs.X = lhs.X
                            lhs.X = (0.02 + (a2(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Width
                            e.Graphics.DrawLine(bpen, lhs, rhs)
                            rhs.X = lhs.X
                            lhs.X = (0.02 + (a(Gch(gpi)) / amax(Gch(gpi))) * 0.8) * gage(gpi).Width
                            e.Graphics.DrawLine(cpen, lhs, rhs)
                        End If
                    End If
                Case 7                              'Gage Bulat
                    Dim center As Point
                    Dim baseL, baseR, BaseM, tip As Point
                    Dim X1 As Integer = (gage(gpi).Width / 2)             'Y is 0 at top
                    Dim y1 As Integer = gage(gpi).Height / 2
                    Dim aledR, aledL, y1led, y2led As Integer
                    Dim AnaTrunk As Double
                    Dim dPen = New Pen(Color.DarkRed)
                    dPen.Width = 2.0F
                    Dim BPen = New Pen(Color.Black)
                    BPen.Width = 2.0F
                    center.X = X1
                    center.Y = y1
                    y1led = 0.03 * y1
                    y2led = 0.16 * y1
                    If amax(Gch(gpi)) <> 0 Then
                        AnaTrunk = a(Gch(gpi)) / amax(Gch(gpi))
                    Else
                        AnaTrunk = 0
                    End If
                    If a(Gch(gpi)) > amax(Gch(gpi)) Then
                        'Console.WriteLine("LED ON " + Str(gpi))
                        AON(gpi) = 1
                        'turn LED ON           
                        ' Console.WriteLine("LED ON " + Str(gpi))
                        aledR = 2 * X1 * 0.98
                        For k = y1led To y2led                                          '7777 Sloppy and should be sped up
                            aledL = 1.18 * X1 + k * 2
                            e.Graphics.DrawLine(Pens.Red, aledL, k, aledR, k)
                        Next k
                    End If
                    If a(Gch(gpi)) > amax(Gch(gpi)) Then AnaTrunk = 1.01
                    tip.X = center.X - 0.9 * X1 * Math.Cos(1.67 * Math.PI * AnaTrunk - 0.33 * Math.PI)
                    tip.Y = center.Y - 0.85 * y1 * Math.Sin(1.67 * Math.PI * AnaTrunk - 0.33 * Math.PI)
                    BaseM.X = center.X - 0.08 * X1 * Math.Cos(1.67 * Math.PI * AnaTrunk - 0.33 * Math.PI)
                    BaseM.Y = center.Y - 0.08 * y1 * Math.Sin(1.67 * Math.PI * AnaTrunk - 0.33 * Math.PI)
                    baseL.X = center.X - 0.08 * X1 * Math.Cos(1.67 * Math.PI * AnaTrunk - 0.07 - 0.33 * Math.PI)
                    baseL.Y = center.Y - 0.08 * y1 * Math.Sin(1.67 * Math.PI * AnaTrunk - 0.07 - 0.33 * Math.PI)
                    baseR.X = center.X - 0.08 * X1 * Math.Cos(1.67 * Math.PI * AnaTrunk + 0.07 - 0.33 * Math.PI)
                    baseR.Y = center.Y - 0.08 * y1 * Math.Sin(1.67 * Math.PI * AnaTrunk + 0.07 - 0.33 * Math.PI)
                    e.Graphics.DrawLine(dPen, BaseM, tip)
                    e.Graphics.DrawLine(BPen, baseL, tip)
                    e.Graphics.DrawLine(BPen, baseR, tip)
            End Select

        End If
    End Sub
    Private Sub Gage_Ticks(ByVal gi As Integer)  ' Load neddle gage Half Bulat Gage
        gage(gi).Image = My.Resources.Gage_3               'Reload "Blank" gage then redraw ticks

        Dim center, labpoint As Point
        Dim tickO, tickI As Point
        Dim x1 As Integer
        Dim y1 As Integer
        Dim ofsy, labwidth As Integer
        Dim fatpen As New Pen(Brushes.Black, 3)
        x1 = 462                                'Dont need to scale as this is drawing on the BMP, not the Paint Box
        y1 = 520

        center.X = x1
        center.Y = 0.9 * y1


        If 1 = 1 Then
            Using g As Graphics = Graphics.FromImage(gage(gi).Image)

                For ix = 0 To 100 Step 100 / (amajtix(Gch(gi)) * amintix(Gch(gi)))
                    tickO.X = center.X - 0.92 * x1 * Math.Cos(Math.PI * ix / 100)
                    tickO.Y = center.Y - 0.8 * y1 * Math.Sin(Math.PI * ix / 100)
                    tickI.X = center.X - 0.97 * x1 * Math.Cos(Math.PI * ix / 100)
                    tickI.Y = center.Y - 0.85 * y1 * Math.Sin(Math.PI * ix / 100)
                    g.DrawLine(Pens.Black, tickO, tickI)
                Next
                For ix = 0 To 100.1 Step 100 / amajtix(Gch(gi))
                    If ix = 0 Or ix > 99 Then
                        ofsy = y1 * 0.07                              'Vertical offset for first/last numbers
                    Else : ofsy = 0
                    End If
                    tickO.X = center.X - 0.87 * x1 * Math.Cos(Math.PI * ix / 100)
                    tickO.Y = center.Y - 0.75 * y1 * Math.Sin(Math.PI * ix / 100)
                    tickI.X = center.X - 0.97 * x1 * Math.Cos(Math.PI * ix / 100)
                    tickI.Y = center.Y - 0.85 * y1 * Math.Sin(Math.PI * ix / 100)
                    g.DrawLine(fatpen, tickO, tickI)

                    labwidth = g.MeasureString((ix * amax(Gch(gi)) / 100).ToString(adec(Gch(gi))), New Font("Arial", 20, FontStyle.Bold)).Width
                    labpoint.X = center.X - 0.8 * x1 * Math.Cos(Math.PI * ix / 100) - 0.1 * x1 - labwidth * 0.92 * 0.28
                    labpoint.Y = center.Y - 0.75 * y1 * Math.Sin(Math.PI * ix / 100) - ofsy

                    g.DrawString((ix * amax(Gch(gi)) / 100).ToString(adec(Gch(gi))), New Font("Arial", 20, FontStyle.Bold), New SolidBrush(Color.Black), labpoint)
                Next
            End Using
        End If
    End Sub
    Private Sub Gage_Ticks4(ByVal gi As Integer)            'Load neddle gage Full Round gage
        gage(gi).Image = My.Resources.Gage_4                'Reload "Blank" gage then redraw ticks

        Dim center, labpoint As Point
        Dim tickO, tickI As Point
        Dim x1 As Integer
        Dim y1 As Integer
        Dim ofsy, labwidth As Integer
        Dim fatpen As New Pen(Brushes.Black, 3)
        x1 = 462                                'Dont need to scale as this is drawing on the BMP, not the Paint Box
        y1 = 520

        center.X = x1
        center.Y = 0.9 * y1


        If 1 = 1 Then
            Using g As Graphics = Graphics.FromImage(gage(gi).Image)

                For ix = 0 To 100 Step 100 / (amajtix(Gch(gi)) * amintix(Gch(gi)))
                    tickO.X = center.X - 0.92 * x1 * Math.Cos(Math.PI * ix / 60 - Math.PI / 3)
                    tickO.Y = center.Y - 0.8 * y1 * Math.Sin(Math.PI * ix / 60 - Math.PI / 3)
                    tickI.X = center.X - 0.97 * x1 * Math.Cos(Math.PI * ix / 60 - Math.PI / 3)
                    tickI.Y = center.Y - 0.85 * y1 * Math.Sin(Math.PI * ix / 60 - Math.PI / 3)
                    g.DrawLine(Pens.Black, tickO, tickI)
                Next
                For ix = 0 To 100.1 Step 100 / amajtix(Gch(gi))
                    If ix = 0 Or ix = 100 Then
                        ofsy = y1 * 0.07                              'Vertical offset for first/last numbers
                    Else : ofsy = 0
                    End If
                    tickO.X = center.X - 0.87 * x1 * Math.Cos(Math.PI * ix / 60 - Math.PI / 3)
                    tickO.Y = center.Y - 0.75 * y1 * Math.Sin(Math.PI * ix / 60 - Math.PI / 3)
                    tickI.X = center.X - 0.97 * x1 * Math.Cos(Math.PI * ix / 60 - Math.PI / 3)
                    tickI.Y = center.Y - 0.85 * y1 * Math.Sin(Math.PI * ix / 60 - Math.PI / 3)
                    g.DrawLine(fatpen, tickO, tickI)

                    labwidth = g.MeasureString((ix * amax(Gch(gi)) / 100).ToString(adec(Gch(gi))), New Font("Arial", 20, FontStyle.Bold)).Width
                    labpoint.X = center.X - 0.8 * x1 * Math.Cos(Math.PI * ix / 60 - Math.PI / 3) - 0.1 * x1 - labwidth * 0.92 * 0.28
                    labpoint.Y = center.Y - 0.75 * y1 * Math.Sin(Math.PI * ix / 60 - Math.PI / 3) - ofsy

                    g.DrawString((ix * amax(Gch(gi)) / 100).ToString(adec(Gch(gi))), New Font("Arial", 20, FontStyle.Bold), New SolidBrush(Color.Black), labpoint)
                Next
            End Using
        End If
    End Sub
    Private Sub CBManChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBMan.CheckedChanged
        If CBMan.Checked = True Then loadon = True
        If TBMan.Text = "" Then TBMan.Text = "0"
        If Val(TBMan.Text) > 100 Then TBMan.Text = "0"
        If Val(TBMan.Text) < 0 Then TBMan.Text = "0"
        sendcmd = "M" + Mid(Str(Int(10 * Val(TBMan.Text))), 2, Len(Str(Int(10 * Val(TBMan.Text))) - 1))

        If CBMan.Checked = True Then
            Console.WriteLine("Sending Load" + sendcmd)
            loadMode = 1
            If COMport.IsOpen Then COMport.Write(sendcmd)
        Else
            sendcmd = ""
        End If
    End Sub
    Private Sub ButtManDn(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButManDn.Click
        If TBMan.Text = "" Then TBMan.Text = "0"
        TBMan.Text = Str(TBMan.Text - 1)
        If Val(TBMan.Text) < 0 Then TBMan.Text = "0"
        sendcmd = "M" + Mid(Str(Int(10 * Val(TBMan.Text))), 2, Len(Str(Int(10 * Val(TBMan.Text))) - 1))
        If compt <> 0 AndAlso CBMan.Checked = True Then
            COMport.Write(sendcmd)
            'MsgBox("CBman " + Str(CBMan.Checked))
        Else
            sendcmd = ""
        End If
    End Sub
    Private Sub ButTManup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButManUp.Click
        If TBMan.Text = "" Then TBMan.Text = "0.0"
        TBMan.Text = Str(TBMan.Text + 1)
        If Val(TBMan.Text) > 100 Then TBMan.Text = "0.0"
        MANLoad = Val(TBMan.Text)
        sendcmd = "M" + Mid(Str(Int(10 * Val(TBMan.Text))), 2, Len(Str(Int(10 * Val(TBMan.Text))) - 1))
        If compt <> 0 AndAlso CBMan.Checked = True Then
            COMport.Write(sendcmd)
            'MsgBox("CBman " + Str(CBMan.Checked))
        Else
            sendcmd = ""
        End If
    End Sub

    Private Sub ButZeroMan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButZeroMan.Click
        If compt <> 0 Then COMport.Write("M0")
        TBMan.Text = "0"
    End Sub
    Private Sub TBMan_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBMan.LostFocus '              Triggers when moving out of box
        If CBMan.Checked = True Then
            If TBMan.Text = "" Then TBMan.Text = "0.0"
            If Val(TBMan.Text) > 100 Then TBMan.Text = "0.0"
            MANLoad = Val(TBMan.Text)
            sendcmd = "M" + Mid(Str(Int(10 * Val(TBMan.Text))), 2, Len(Str(Int(10 * Val(TBMan.Text))) - 1))
            If compt <> 0 AndAlso TBMan.Focused = False Then COMport.WriteLine(sendcmd) 'Cant do when focused or it'll send "partial" loads
        End If
    End Sub
    Private Sub CBRPMCh(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBRPM.CheckedChanged

        If CBRPM.Checked = True Then
            loadon = True
            sendcmd = "S" + Mid(Str(Int(RPMload)), 2, Len(Str(Int(RPMload)) - 1))       'NOTE: If using Eng RPM (rpm2) then might need to 
            ' Console.WriteLine("DGR: " + Str(dGR))                                                                             '  calculate GR from RPM1 vs. RPM2
            If COMport.IsOpen AndAlso TBrpmSP.Focused = False Then
                Sleep(15)
                COMport.Write(sendcmd)
                'Sleep(15)                                                           'Some times the controller doesnt respond to these
            End If                                                                  ' But the pauses seem to help
            Console.WriteLine(sendcmd)
            loadMode = 2                                                            'RPM mode
        End If
    End Sub

    Private Sub TBrpmD_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBrpmSP.LostFocus
        If TBrpmSP.Text = "" Then TBrpmSP.Text = "0"
        If Val(TBrpmSP.Text) > rpmdynomax * dGR Then TBrpmSP.Text = rpmdynomax * dGR
        RPMload = Val(TBrpmSP.Text) / dGR
        If CBRPM.Checked = True Then
            sendcmd = "S" + Mid(Str(Int(RPMload)), 2, Len(Str(Int(RPMload)) - 1))
            'MsgBox(sendcmd)
            If compt <> 0 Then
                COMport.Write(sendcmd)
            End If

        End If
    End Sub
    Private Sub Receiver(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs) Handles COMport.DataReceived

        If serenab = True Then
            Try
                RXtxt = COMport.ReadLine

                If Len(RXtxt) < 10 Then RXtxt = ""
                ' If rement = True Then                   'Allow Button 3 on Remote to trigger "ENTER" key
                ' RXtxt = " " + Mid(RXtxt, 2, Len(RXtxt) - 1) + " 0 0 0 0 0 0 0 0 0 0"                            'Trim Z and pad
                'd = RXtxt.Split(" ")
                'lastrem = RemV : RemV = Val(d(8 + remch))
                'Console.WriteLine("A3: " + Str(RemV))
                'If lastrem > 307 And lastrem < 430 And RemV > 307 And RemV < 430 Then
                'SendKeys.Send("~") 'NOTE: should be ("ENTER") but that doesn't work
                'Beep()
                'End If
                'End If
                Me.Invoke(New MethodInvoker(AddressOf main)) 'If it pukes on this when running, just RE-RUN!
            Catch ex As Exception
                RXtxt = "SerCom Error"
                ' MsgBox("Serial Com Glitch.")              'Conitinue (freezez if we don't catch this error)
            End Try                                             'Don't actually need the msgbox, but gotta have the "catch"
        End If            '                                                    to avoid "freexing" the computer. 
        ' If REMOTE ON and REMOTE 3 PRESSED Then SendKeys.Send("~") 8888
    End Sub
    Private Sub Speedup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtSpeedup.Click
        If Ldyno2.Text = "Dyno" Then
            RPMload = RPMload + 100
        Else
            RPMload = RPMload + 250 / dGR
        End If
        If RPMload > rpmdynomax Then RPMload = rpmdynomax
        TBrpmSP.Text = Str(Int(RPMload * dGR))
        If CBRPM.Checked = True Then
            sendcmd = "S" + Mid(Str(Int(RPMload)), 2, Len(Str(Int(RPMload)) - 1))
            If compt <> 0 AndAlso TBrpmSP.Focused = False Then COMport.Write(sendcmd)
        End If
    End Sub

    Private Sub SpeedDn(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButSpeeddn.Click
        If Ldyno2.Text = "Dyno" Then
            RPMload = RPMload - 100
        Else
            RPMload = RPMload - 250 / dGR
        End If
        If RPMload < 0 Then RPMload = 0
        TBrpmSP.Text = Str(Int(RPMload * dGR))
        If CBRPM.Checked = True Then
            sendcmd = "S" + Mid(Str(Int(RPMload)), 2, Len(Str(Int(RPMload)) - 1))
            If compt <> 0 AndAlso TBrpmSP.Focused = False Then COMport.Write(sendcmd)
        End If
    End Sub
    Private Sub CBAcelCh(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBAcel.CheckedChanged
        loadon = True
        If CBAcel.Checked = True Then
            If TBAcel.Text = "" Then TBAcel.Text = "15"
            'acel = Int(Val(TBAcel.Text) / dgr)            'Convert to Dyno speed
            pullmode = 5                'Start with "Return to Idle"
            loadMode = 4                'Acell
        End If
    End Sub
    Private Sub CB2Point_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CB2Point.CheckedChanged
        loadon = True
        If CB2Point.Checked = True Then
            'Beep()
            If Tbrpm1.Text = "" Then Tbrpm1.Text = "0"
            If TBRPM2.Text = "" Then TBRPM2.Text = "0"
            If TBLoad1.Text = "" Then TBLoad1.Text = "0"
            If TBLoad2.Text = "" Then TBLoad2.Text = "0"
            If Val(Tbrpm1.Text) < 0 Then Tbrpm1.Text = "0"
            If Val(TBRPM2.Text) < 0 Then TBRPM2.Text = "0"
            If Val(TBLoad1.Text) < 0 Then Tbrpm1.Text = "0"
            If Val(TBLoad2.Text) < 0 Then TBLoad2.Text = "0"
            If Val(Tbrpm1.Text) > rpmdynomax * dGR Then Tbrpm1.Text = (rpmdynomax * dGR).ToString("F0")
            If Val(TBRPM2.Text) > rpmdynomax * dGR Then TBRPM2.Text = (rpmdynomax * dGR).ToString("F0")
            If Val(TBLoad1.Text) > 100 Then TBLoad1.Text = "0"
            If Val(TBLoad2.Text) > 100 Then TBLoad2.Text = "0"
            rpm1 = Val(Tbrpm1.Text) / dGR
            rpm2 = Val(TBRPM2.Text) / dGR
            load1 = Val(TBLoad1.Text)
            load2 = Val(TBLoad2.Text)
            pullmode = 5                                    'Start in "Return to Idle" mode
            loadMode = 3                                    '2-Point
            'Beep()
        End If
    End Sub
    Private Sub TBRPM2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBRPM2.LostFocus
        If TBRPM2.Text = "" Then TBRPM2.Text = "0"
        rpm2 = Val(TBRPM2.Text) / dGR
    End Sub
    Private Sub TBRPM1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tbrpm1.LostFocus
        If Tbrpm1.Text = "" Then Tbrpm1.Text = "0"
        rpm1 = Val(Tbrpm1.Text) / dGR
    End Sub

    Private Sub TBAcel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBAcel.LostFocus
        If TBAcel.Text = "" Then TBAcel.Text = "15"
        PullDur = Val(TBAcel.Text)
    End Sub
    Private Sub TBLoad1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBLoad1.LostFocus
        load1 = Val(TBLoad1.Text)
    End Sub
    Private Sub TBLoad2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBLoad2.LostFocus
        load2 = Val(TBLoad2.Text)
    End Sub
    Protected Sub numclick(ByVal sender As Object, ByVal e As MouseEventArgs)       'This is just to send any clicks on the number display to the underlying Pitcure
        Dim PB As Label
        PB = sender                 'Where'd we get the click from?

        For i = 1 To 50
            If PB Is n(i) Then Call gageclick(gage(i), e) 'Gen index of the "gage"
        Next
    End Sub
    Private Sub setgr() Handles GearRatio.setgr             'Gets the "calibrated" gear ratio from the GR setting form when SET is hit
        GR = Int(GearRatio.grs * 100) / 100                 'Truncate a bit
        'SHOULD RE_DO ALL ENG RPM REFERENCE NUMBERS
        If rpmode = 2 Or rpmode = 3 Then
            dGR = GR
            TBrpmSP.Text = Str(Int(RPMload * dGR))
            Tbrpm1.Text = Str(Int(rpm1 * dGR))
            TBRPM2.Text = Str(Int(rpm2 * dGR))
            'TBAcel.Text = Str(Int(Acel * dGR))
        End If
    End Sub
    Private Sub setDataCh() Handles dispsel.setsel
        Dim gotone As Boolean
        gotone = False
        For i = 1 To 27
            If ds(i).Checked Then
                Gch(gind) = i
                gotone = True
            End If
        Next
        If gotone = True Then l(gind).Text = alab(Gch(gind))
        If gtype(gind) = 1 Then Call Gage_Ticks(gind) 'Re-do tix
        If gtype(gind) = 2 Then                         'Reset Graph
            For i = 1 To 300
                gdat(gind, i) = 0
            Next
        End If
        If gtype(gind) = 7 Then Call Gage_Ticks4(gind) 'Re-do tix
        If ds(28).Checked Then
            gage(gind).Hide() 'Hide it and only save if it is NOT hidden
            ghide(gind) = True
        End If
        '     gage(gind) 'Redraw Ticks (or whatever)
        dispsel.Hide()
    End Sub
    Protected Sub grfclick(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim PB As PictureBox
        PB = sender                 'Where'd we get the click from?

        For i = 1 To 50
            If PB Is grf(i) Then Call gageclick(gage(i), e) 'Gen index of the "gage"
        Next
    End Sub
    Protected Sub gageclick(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim PB As PictureBox
        PB = sender                 'Where'd we get the click from?
        If gmove = True Or gstrech = True Then
            gmove = False
            gstrech = False
            LG.Text = "Display Items:"
        Else
            For i = 1 To 50
                If PB Is gage(i) Then gind = i 'Gen index of the "gage"
            Next

            If e.Button = MouseButtons.Left AndAlso gstrech = False AndAlso gmove = False Then ' Allows "drop" of object w/ left click
                dispsel.Show()
                If Gch(gind) < 1 Then Gch(gind) = 1
                ds(Gch(gind)).Checked = True
            Else                                                        'Move

                gmx = MousePosition.X - PB.Left                       'Get Offsets (on screen)
                gmy = MousePosition.Y - PB.Top
                'If gmove = False AndAlso gstrech = False Then MsgBox("Xofset:" + Str(gmx) + Chr(13) + "PBL: " + Str(PB.Left) + Chr(13) + "CMouseX: " + Str(Control.MousePosition.X))
                If e.X >= Int(0.9 * PB.Width) AndAlso e.Y >= Int(0.9 * PB.Height) Then          'E.X is position within the Object
                    GMXL = PB.Right - MousePosition.X
                    GMYL = PB.Bottom - MousePosition.Y
                    'GMXL = PB.Right - PB.Left                                                   'Current Gage Width
                    'GMYL = PB.Bottom - PB.Top                                                   'Current Gage height
                    If gstrech = True Then                                                      'Dropping Item
                        gstrech = False
                        LG.Text = "Display Items:"
                        With l(gind)
                            .Font = New Font("Arial", .Font.Size * (gage(gind).Width / ow + gage(gind).Height / oh) / 2, .Font.Style, .Font.Unit)
                            .Height = .Height * gage(gind).Width / ow       'Re size
                            .Width = .Width * gage(gind).Width / ow
                            .Top = .Top * gage(gind).Height / oh            'Move to new location
                            .Left = .Left * gage(gind).Width / ow
                        End With
                        With n(gind)
                            .Font = New Font(.Font.Name, .Font.Size * (gage(gind).Width / ow + gage(gind).Height / oh) / 2, .Font.Style, .Font.Unit)
                            .Height = .Height * gage(gind).Width / ow
                            .Width = .Width * gage(gind).Width / ow
                        End With
                    Else
                        gstrech = True
                        oh = PB.Height : ow = PB.Width                                'Used to rescale font when dropped
                        LG.Text = "Strech " + Str(gind)
                    End If
                Else
                    If gmove = True Then
                        gmove = False
                        LG.Text = "Display Items:"
                    Else
                        gmove = True
                        LG.Text = "Move " + Str(gind)
                    End If
                End If

            End If
        End If
        Beep()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        sendcmd = "M0"                                  'Shutdown Current when closing!
        If COMport.IsOpen Then COMport.WriteLine(sendcmd) 'Will puke here if USB disconnected, so do last. 
        Sleep(20)
        Call savecal()                  'Save Calibrations
        End
    End Sub


    Private Sub Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel2.Click
        Dim done As Boolean                             'This just pickus up "stray" clicks ment for dropping streched objects
        If e.Button = MouseButtons.Left Then
            If gmove = True Or gstrech = True Then
                gmove = False
                gstrech = False
                LG.Text = "Display Items:"
                Beep()
            End If
        End If
    End Sub

    Private Sub BG1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BG1.Click
        'MsgBox("Got Click at But 1!")
        gagemax = gagemax + 1
        gage(gagemax).Width = 200 'or whatever
        gage(gagemax).Height = 100
        gage(gagemax).Top = 50 'or whatever
        gage(gagemax).Left = 50 * gagemax + 50
        l(gagemax).Width = 150
        l(gagemax).Height = 20
        Gch(gagemax) = 1                              'Assign to Dyno Speed initially
        Call addg1()
    End Sub
    Sub addg1()
        gtype(gagemax) = 1                'Gage
        gage(gagemax).Image = My.Resources.Gage_3
        gage(gagemax).SizeMode = PictureBoxSizeMode.StretchImage
        Me.Panel2.Controls.Add(gage(gagemax))
        gage(gagemax).Show()
        gage(gagemax).BringToFront()
        l(gagemax).Parent = gage(gagemax)
        n(gagemax).Parent = gage(gagemax)
        l(gagemax).Left = GLOfx(gtype(gagemax)) * gage(gagemax).Width
        l(gagemax).Top = GLOfy(gtype(gagemax)) * gage(gagemax).Height - 2
        l(gagemax).Text = alab(Gch(gagemax))
        l(gagemax).TextAlign = ContentAlignment.TopLeft
        With l(gagemax)
            .Font = New Font(.Font.Name, .Font.Size * (gage(gagemax).Width / 150 + gage(gagemax).Height / 50) / 2, .Font.Style, .Font.Unit)
            .Height = .Height * gage(gagemax).Height / 50
            .Width = .Width * gage(gagemax).Width / 150
        End With
        l(gagemax).BringToFront()
        l(gagemax).Show()
        Call Gage_Ticks(gagemax) 'Re-do tix
        gSUdone = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BG3.Click
        gagemax = gagemax + 1
        Gch(gagemax) = 1          'Default to channel 1
        gage(gagemax).Width = 50 'or whatever
        gage(gagemax).Height = 50
        gage(gagemax).Top = 50 'or whatever
        gage(gagemax).Left = 50 * gagemax + 50
        l(gagemax).Width = 50
        l(gagemax).Height = 20
        Gch(gagemax) = 1                              'Assign to Dyno Speed initially
        Call addg3()
    End Sub
    Private Sub addg3()
        gtype(gagemax) = 3                'LED
        gage(gagemax).Image = My.Resources.GREEN_LEDi
        gage(gagemax).SizeMode = PictureBoxSizeMode.StretchImage
        Me.Panel2.Controls.Add(gage(gagemax))
        gage(gagemax).Show()
        gage(gagemax).BringToFront()
        l(gagemax).Parent = gage(gagemax)
        n(gagemax).Parent = gage(gagemax)

        l(gagemax).Top = GLOfy(gtype(gagemax)) * gage(gagemax).Height - 2
        l(gagemax).Text = alab(Gch(gagemax))

        l(gagemax).TextAlign = ContentAlignment.TopCenter
        With l(gagemax)
            .Font = New Font(.Font.Name, .Font.Size * (gage(gagemax).Width / 50 + gage(gagemax).Height / 50) / 2, .Font.Style, .Font.Unit)
            .Height = .Height * gage(gagemax).Height / 50
            .Width = .Width * gage(gagemax).Width / 50
        End With
        l(gagemax).Left = GLOfx(gtype(gagemax)) * gage(gagemax).Width - l(gagemax).Width / 2
        l(gagemax).BringToFront()
        l(gagemax).Show()
        gSUdone = True
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BG4.Click
        gagemax = gagemax + 1
        Gch(gagemax) = 1          'Default to channel 1
        gage(gagemax).Width = 50 'or whatever
        gage(gagemax).Height = 150
        gage(gagemax).Top = 50 'or whatever
        gage(gagemax).Left = 50 * gagemax + 50
        l(gagemax).Width = 50
        l(gagemax).Height = 30
        Gch(gagemax) = 1                              'Assign to Dyno Speed initially
        Call addg4()
    End Sub
    Private Sub addg4()
        gtype(gagemax) = 4                'Vert
        gage(gagemax).Image = My.Resources.Verti
        gage(gagemax).SizeMode = PictureBoxSizeMode.StretchImage
        Me.Panel2.Controls.Add(gage(gagemax))
        gage(gagemax).Show()
        gage(gagemax).BringToFront()
        l(gagemax).Parent = gage(gagemax)
        n(gagemax).Parent = gage(gagemax)

        l(gagemax).Top = GLOfy(gtype(gagemax)) * gage(gagemax).Height - 2
        l(gagemax).Text = alab(Gch(gagemax))
        With l(gagemax)
            .Font = New Font(.Font.Name, .Font.Size * (gage(gagemax).Width / 50 + gage(gagemax).Height / 150) / 2, .Font.Style, .Font.Unit)
            .Height = .Height * gage(gagemax).Height / 150
            .Width = .Width * gage(gagemax).Width / 50
        End With
        l(gagemax).BringToFront()
        l(gagemax).Left = GLOfx(gtype(gagemax)) * gage(gagemax).Width - l(gagemax).Width / 2
        l(gagemax).Show()
        gSUdone = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BG5.Click
        gagemax = gagemax + 1
        Gch(gagemax) = 1          'Default to channel 1
        gage(gagemax).Image = My.Resources.Horzi
        gage(gagemax).Width = 150 'or whatever
        gage(gagemax).Height = 50
        gage(gagemax).Top = 50 'or whatever
        gage(gagemax).Left = 50 * gagemax + 50
        l(gagemax).Width = 150
        l(gagemax).Height = 30
        Gch(gagemax) = 1                              'Assign to Dyno Speed initially
        Call addg5()
    End Sub
    Private Sub addg5()
        gtype(gagemax) = 5                'Horz
        gage(gagemax).Image = My.Resources.Horzi
        gage(gagemax).SizeMode = PictureBoxSizeMode.StretchImage
        Me.Panel2.Controls.Add(gage(gagemax))
        gage(gagemax).Show()
        gage(gagemax).BringToFront()
        l(gagemax).Parent = gage(gagemax)
        n(gagemax).Parent = gage(gagemax)

        l(gagemax).Top = GLOfy(gtype(gagemax)) * gage(gagemax).Height - 2
        l(gagemax).Text = alab(Gch(gagemax))
        With l(gagemax)
            .Font = New Font(.Font.Name, .Font.Size * (gage(gagemax).Width / 150 + gage(gagemax).Height / 50) / 2, .Font.Style, .Font.Unit)
            .Height = .Height * gage(gagemax).Height / 50
            .Width = .Width * gage(gagemax).Width / 150
        End With

        l(gagemax).Left = GLOfx(gtype(gagemax)) * gage(gagemax).Width - l(gagemax).Width / 2
        l(gagemax).BringToFront()
        l(gagemax).Show()
        gSUdone = True
    End Sub


    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BG6.Click
        gagemax = gagemax + 1
        Gch(gagemax) = 1          'Default to channel 1
        gage(gagemax).Width = 150 'or whatever
        gage(gagemax).Height = 50
        gage(gagemax).Top = 50 'or whatever
        gage(gagemax).Left = 50 * gagemax + 50
        l(gagemax).Width = 150
        l(gagemax).Height = 30
        Gch(gagemax) = 1                              'Assign to Dyno Speed initially
        Call addg6()
    End Sub
    Private Sub addg6()
        gtype(gagemax) = 6                'Number
        gage(gagemax).Image = My.Resources.Horzi
        gage(gagemax).SizeMode = PictureBoxSizeMode.StretchImage
        Me.Panel2.Controls.Add(gage(gagemax))
        gage(gagemax).Show()
        gage(gagemax).BringToFront()
        n(gagemax).Parent = gage(gagemax)
        l(gagemax).Parent = gage(gagemax)


        l(gagemax).Top = GLOfy(gtype(gagemax)) * gage(gagemax).Height - 2
        l(gagemax).Text = alab(Gch(gagemax))
        With n(gagemax)
            .Font = New Font(.Font.Name, .Font.Size * (gage(gagemax).Width / 150 + gage(gagemax).Height / 50) / 2, .Font.Style, .Font.Unit)
            .Height = .Height * gage(gagemax).Height / 40
            .Width = .Width * gage(gagemax).Width / 150
            .TextAlign = ContentAlignment.TopCenter
            'MsgBox("Align")
        End With
        With l(gagemax)
            .Font = New Font(.Font.Name, .Font.Size * (gage(gagemax).Width / 150 + gage(gagemax).Height / 50) / 2, .Font.Style, .Font.Unit)
            .Height = .Height * gage(gagemax).Height / 80                   'This was 50, but then overlapped "ocassionally" with number (upon re-loads)
            .Width = .Width * gage(gagemax).Width / 150                     'Never figgured why it only did it with re-loads n not the 1st time loading
        End With
        l(gagemax).Left = GLOfx(gtype(gagemax)) * gage(gagemax).Width - l(gagemax).Width / 2
        l(gagemax).BringToFront()
        l(gagemax).Show()
        gSUdone = True                  ' 8888 apparently the fonts rescale twice? here. Also for saving the screens, we need to save them with the 
        'Over all size of the screen, or they'll be too big/small. 
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BG2.Click
        gagemax = gagemax + 1
        Gch(gagemax) = 1          'Default to channel 1
        gage(gagemax).Width = 150 'or whatever
        gage(gagemax).Height = 50
        grf(gagemax).Width = 145
        grf(gagemax).Height = 32
        gage(gagemax).Top = 50 'or whatever
        gage(gagemax).Left = 50 * gagemax + 50
        l(gagemax).Width = 150
        l(gagemax).Height = 30
        Gch(gagemax) = 1                              'Assign to Dyno Speed initially

        Call addg2()
    End Sub
    Private Sub addg2()
        gtype(gagemax) = 2                'Graph
        gage(gagemax).Image = My.Resources.Horzi
        gage(gagemax).SizeMode = PictureBoxSizeMode.StretchImage
        Me.Panel2.Controls.Add(gage(gagemax))
        Me.Panel2.Controls.Add(grf(gagemax))

        grf(gagemax).Left = gage(gagemax).Left + 0.02 * gage(gagemax).Width         'Setup the position of the graphic overlay
        grf(gagemax).Top = gage(gagemax).Top + Int(0.3 * gage(gagemax).Height)
        grf(gagemax).Width = gage(gagemax).Width * 0.96
        grf(gagemax).Height = gage(gagemax).Height * 0.66

        gage(gagemax).Show()
        gage(gagemax).BringToFront()
        grf(gagemax).BringToFront()
        l(gagemax).Parent = gage(gagemax)
        n(gagemax).Parent = gage(gagemax)

        l(gagemax).Top = GLOfy(gtype(gagemax)) * gage(gagemax).Height - 2
        l(gagemax).Text = alab(Gch(gagemax))

        With l(gagemax)
            .Font = New Font(.Font.Name, .Font.Size * (gage(gagemax).Width / 150 + gage(gagemax).Height / 50) / 2, .Font.Style, .Font.Unit)
            .Height = .Height * gage(gagemax).Height / 50
            .Width = .Width * gage(gagemax).Width / 150
        End With
        l(gagemax).Left = GLOfx(gtype(gagemax)) * gage(gagemax).Width - l(gagemax).Width / 2
        l(gagemax).BringToFront()
        l(gagemax).Show()
        Grafind = Grafind + 1
        gSUdone = True
    End Sub
    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BGbulat.Click
        'MsgBox("Got Click at But 1!")
        gagemax = gagemax + 1
        gage(gagemax).Width = 200 'or whatever
        gage(gagemax).Height = 200
        gage(gagemax).Top = 50 'or whatever
        gage(gagemax).Left = 50 * gagemax + 50
        l(gagemax).Width = 150
        l(gagemax).Height = 20
        Gch(gagemax) = 1                              'Assign to Dyno Speed initially
        Call addg7()
    End Sub
    Sub addg7()
        gtype(gagemax) = 7                'Gage - Bulat
        gage(gagemax).Image = My.Resources.Gage_4
        gage(gagemax).SizeMode = PictureBoxSizeMode.StretchImage
        Me.Panel2.Controls.Add(gage(gagemax))
        gage(gagemax).Show()
        gage(gagemax).BringToFront()
        l(gagemax).Parent = gage(gagemax)
        n(gagemax).Parent = gage(gagemax)

        l(gagemax).Top = GLOfy(gtype(gagemax)) * gage(gagemax).Height - 2
        l(gagemax).Text = alab(Gch(gagemax))
        l(gagemax).TextAlign = ContentAlignment.TopCenter
        'l(gagemax).BackColor = Color.LightGray
        With l(gagemax)
            .Font = New Font(.Font.Name, .Font.Size * (gage(gagemax).Width / 150 + gage(gagemax).Height / 50) / 2, .Font.Style, .Font.Unit)
            .Height = .Height * gage(gagemax).Height / 140
            .Width = .Width * gage(gagemax).Width / 150
        End With
        l(gagemax).Left = GLOfx(gtype(gagemax)) * gage(gagemax).Width - l(gagemax).Width / 2
        l(gagemax).BringToFront()
        l(gagemax).Show()
        Call Gage_Ticks4(gagemax) 'Re-do tix
        gSUdone = True
    End Sub

    Private Sub Form1_Resize_1(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        '    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize   8888 KEEPS LOSING HANDLES CLAUSE!!!
        rs.ResizeAllControls(Me)                                'Auto repositions and resizes everything on the screen. 
        ' MsgBox("Resize")
        'Aussom
    End Sub

    Private Sub BuTare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuTare.Click
        If time - tareclick < 1 Then        'Is this the 2nd click in <1 sec? Then toggle AUTO TARE
            If autotare = True Then
                autotare = False
            Else
                autotare = True
            End If
        End If
        tareclick = time
        COMport.Write("Z")                      'Tare Load Cell
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButGR.Click
        If GearRatio.Visible Then
        Else
            GearRatio.RadioButton4.Checked = True
            If speed2 > 500 And Speed1 > 50 Then GearRatio.RadioButton7.Checked = True 'Set to RPM2 for Fast SET
            GearRatio.TBFact.Text = Str(GR)
            GearRatio.Show()
        End If
    End Sub
    Private Sub Z_EngInert() Handles Cals.EIchanged
        Cals.LEngInertEquiv.Text = Int(1000 * Val(Cals.TBEngInert.Text) * Val(Cals.TBgr.Text) ^ 2) / 1000
    End Sub
    Private Sub CalBut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalBut.Click
        If Cals.Visible = True Then
            Cals.Hide()
        Else
            Cals.TBMag.Text = MAG
            Cals.Tbtit.Text = tagline
            Cals.TBDiam.Text = diam
            Cals.TBMaxrpm.Text = rpmdynomax
            Cals.TBMaxTorq.Text = TorqDynoMax
            Cals.TBInert.Text = inert
            Cals.TBEngInert.Text = inerteng
            Cals.TBFofs.Text = frictof
            Cals.TBFfact.Text = frictfact
            Cals.TBFquad.Text = FrictQuad
            Cals.TBType.Text = dynotype
            Cals.TBgr.Text = GR
            Cals.LEngInertEquiv.Text = Int(1000 * inerteng * GR ^ 2) / 1000
            Cals.CBRemote.Checked = remote
            Cals.TBRch.Text = remch
            Cals.TBAC.Text = KPa
            Cals.TBShop.Text = ShopName
            Cals.TBCont.Text = ShopCont

            Cals.CBSAEAuto.Checked = saeauto
            Cals.CBSAEComp.Checked = saecomp
            Cals.TBSAEP.Text = saepress
            Cals.TBSAET.Text = saetemp
            Cals.TBSAER.Text = saeRH


            calupdate = False                       'Dont change when selecting new item's data
            Cals.ComboBox1.SelectedIndex = Calch - 1
            calupdate = True

            Cals.tNAM.Text = alab(Calch)
            Cals.tUNI.Text = auni(Calch)
            Cals.tOF.Text = aof(Calch)
            Cals.tFAC.Text = Afact(Calch)
            Cals.tMIN.Text = amin(Calch)
            Cals.TA1.Text = a1(Calch)
            Cals.TA2.Text = a2(Calch)
            Cals.tMAX.Text = amax(Calch)
            Cals.tFLT.Text = afilt(Calch)
            Cals.tMAJ.Text = amajtix(Calch)
            Cals.tMNR.Text = amintix(Calch)
            Cals.tDEC.Text = adec(Calch)

            Cals.TBBLoad.Text = brekload
            Cals.RB2PS.Checked = brek2p
            Cals.RBLoad.Checked = Not brek2p
            Cals.CBRamp.Checked = brekramp
            Cals.CBStart.Checked = brekstop

            Cals.CBRemote.Checked = remote
            Cals.Lkm.Text = "Km: " + KMt.ToString("F1")
            Cals.Show()
        End If
    End Sub
    Private Sub setCals() Handles Cals.setcal             'Gets the "calibrated" gear ratio from the GR setting form when SET is hit
        Cals.Hide()

        remote = Cals.CBRemote.Checked
        brekload = Val(Cals.TBBLoad.Text)
        brek2p = Cals.RB2PS.Checked
        brekramp = Cals.CBRamp.Checked
        brekstop = Cals.CBStart.Checked
        MAG = Val(Cals.TBMag.Text)
        tagline = Cals.Tbtit.Text
        diam = Val(Cals.TBDiam.Text)
        rpmdynomax = Val(Cals.TBMaxrpm.Text)
        TorqDynoMax = Val(Cals.TBMaxTorq.Text)
        inert = Val(Cals.TBInert.Text)
        inerteng = Val(Cals.TBEngInert.Text)
        frictof = Val(Cals.TBFofs.Text)
        frictfact = Val(Cals.TBFfact.Text)
        FrictQuad = Val(Cals.TBFquad.Text)
        dynotype = Cals.TBType.Text
        TBDynoType.Text = dynotype                 'Refresh at screen
        GR = Val(Cals.TBgr.Text)
        KPa = Val(Cals.TBAC.Text)                     'Acell Gain Proportional
        ShopName = Cals.TBShop.Text
        ShopCont = Cals.TBCont.Text
        Calch = Cals.ComboBox1.SelectedIndex + 1

        alab(Calch) = Cals.tNAM.Text
        auni(Calch) = Cals.tUNI.Text
        aof(Calch) = Val(Cals.tOF.Text)
        Afact(Calch) = Val(Cals.tFAC.Text)
        amin(Calch) = Val(Cals.tMIN.Text)

        a1(Calch) = Val(Cals.TA1.Text)
        a2(Calch) = Val(Cals.TA2.Text)

        saeauto = Cals.CBSAEAuto.Checked
        saecomp = Cals.CBSAEComp.Checked
        saepress = Val(Cals.TBSAEP.Text)
        saetemp = Val(Cals.TBSAET.Text)
        saeRH = Val(Cals.TBSAER.Text)

        amax(Calch) = Val(Cals.tMAX.Text)
        afilt(Calch) = Val(Cals.tFLT.Text)
        If afilt(Calch) > 0.999 Then afilt(i) = 0.999
        amajtix(Calch) = Val(Cals.tMAJ.Text)
        amintix(Calch) = Val(Cals.tMNR.Text)
        adec(Calch) = Cals.tDEC.Text
        calupdate = True
        remch = Val(Cals.TBRch.Text)
        If remch < 1 Then remch = 1
        If remch > 4 Then remch = 4
        'SHOULD RE_DO ALL screen NAMES, LINES and TIX  8888
    End Sub
    Private Sub RsetCals() Handles Cals.calchanged             'Gets the "calibrated" gear ratio from the GR setting form when SET is hit

        If calupdate = True Then
            alab(Calch) = Cals.tNAM.Text                'Assign "changed" calibrations
            auni(Calch) = Cals.tUNI.Text
            aof(Calch) = Val(Cals.tOF.Text)
            Afact(Calch) = Val(Cals.tFAC.Text)
            amin(Calch) = Val(Cals.tMIN.Text)
            a1(Calch) = Val(Cals.TA1.Text)
            a2(Calch) = Val(Cals.TA2.Text)
            amax(Calch) = Val(Cals.tMAX.Text)
            afilt(Calch) = Val(Cals.tFLT.Text)
            If afilt(Calch) > 0.999 Then afilt(i) = 0.5
            amajtix(Calch) = Val(Cals.tMAJ.Text)
            amintix(Calch) = Val(Cals.tMNR.Text)
            adec(Calch) = Cals.tDEC.Text
        End If

        'SHOULD RE_DO ALL screen NAMES, LINES and TIX

        Calch = Cals.ComboBox1.SelectedIndex + 1         'Load new channel cals

        Cals.tNAM.Text = alab(Calch)
        Cals.tUNI.Text = auni(Calch)
        Cals.tOF.Text = aof(Calch)
        Cals.tFAC.Text = Afact(Calch)
        Cals.tMIN.Text = amin(Calch)
        Cals.TA1.Text = a1(Calch)
        Cals.TA2.Text = a2(Calch)
        Cals.tMAX.Text = amax(Calch)
        Cals.tFLT.Text = afilt(Calch)
        Cals.tMAJ.Text = amajtix(Calch)
        Cals.tMNR.Text = amintix(Calch)
        Cals.tDEC.Text = adec(Calch)

    End Sub

    Private Sub ButGraf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButGraf.Click
        If Comp.Visible Then
            ' Comp.Hide()
        Else
            Comp.Lrun1.Text = run
            Comp.LID.Text = runfile
            Comp.LShop.Text = ShopName
            Comp.Lcont.Text = ShopCont
            Comp.Ltime.Text = Now
            Comp.Lsec.Text = timelaps.ToString("F1")
            Comp.ButFile.Text = "Click to load file"           'This text is refferred to in FComp, if you change it here, change it there
            Comp.Show()
        End If
    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click
        serports = ""
        For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
            serports = serports + sp + ", "
        Next
        CPStr = InputBox("Currently Reading From: " + Str(compt) + Chr(13) + Chr(13) + "Available Ports: " + serports, "Enter Dyno Controller Com Port ", Str(compt))
        If CPStr <> "" Then
            compt = CPStr
            comstr = "COM" + LTrim(Str(compt))
            If COMport.IsOpen Then COMport.Close()
            COMport.PortName = comstr    'OPen  Com Port: NOTE Can use USB com ports just like internal ones!
            CPStr = InputBox("Baud Rate: " + Str(comBR) + Chr(13) + "Typically 9600 or 115200" + Chr(13) + "Enter Dyno Controller Com Baud Rate ", , Str(comBR))
            If CPStr <> "" Then comBR = Val(CPStr)
            COMport.BaudRate = comBR
            COMport.ReadBufferSize = 64
            Try
                COMport.Open()
            Catch ex As Exception                       'This only allows 1 retry!!!
                serports = ""
                For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                    serports = serports + sp + ", "
                Next
                compt = InputBox(comstr + " is unacessable." + Chr(13) + "Available Ports: " + serports, "Enter Dyno Controller Com Port #", 1, 5, 5)
                comstr = "COM" + LTrim(Str(compt))
                '     If PortIsAvailable(comstr) Then
                COMport.PortName = comstr
                COMport.BaudRate = 9600
                COMport.ReadBufferSize = 64
                If compt <> 0 Then COMport.Open()
            End Try
            If COMport.IsOpen Then
                COMport.DiscardInBuffer()
            End If
        End If
        If COMport.IsOpen Then COMport.Write("V")
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick     'Ser Com Watch Dog Timer
        PBcoms.Image = My.Resources.RED_LED                                                                     'If 5 pings have been missed, then it recycles
        Console.WriteLine("Sending V.")
        If COMport.IsOpen Then COMport.Write("V") 'Put in Verbose Mode
        Call gagemove()
    End Sub
    Private Sub MegaStrech(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel2.Resize
        Dim li As Integer           'Just streches Panel 2 (the display items)
        Dim sx, sy As Single        'Scalex and Y

        If p2x > 0 AndAlso p2y > 0 Then
            sx = Panel2.Width / p2x
            sy = Panel2.Height / p2y
            'MsgBox("Rescale   W:" + Str(sx) + "  H:" + Str(sy))
            If (sx + sy) / 2 > 0.6 AndAlso (sx + sy) / 2 < 2 Then
                For li = 1 To gagemax
                    If sx <> 1 Then
                        gage(li).Width = gage(li).Width * sx
                        gage(li).Left = gage(li).Left * sx
                        p2x = Panel2.Width                      'Otherwise it "double taps" when resizing for Y
                    End If
                    If sy <> 1 Then
                        gage(li).Height = gage(li).Height * sy
                        gage(li).Top = gage(li).Top * sy
                        p2y = Panel2.Height
                    End If
                    With l(li)
                        'If li = 1 Then Console.Write("Font Size " + Str(.Font.Size))
                        .Font = New Font("Arial", .Font.Size * (sx + sy) / 2, .Font.Style, .Font.Unit) 'Strech/Shrink Font
                        .Height = .Height * sy
                        .Width = .Width * sx
                        'If li = 1 Then Console.WriteLine("Changed to: " + Str(.Font.Size) + " When x " + Str((sx + sy) / 2))
                    End With
                    With n(li)
                        .Font = New Font(.Font.Name, .Font.Size * (sx + sy) / 2, .Font.Style, .Font.Unit) 'Strech/Shrink Font
                        .Height = .Height * sy
                        .Width = .Width * sx
                    End With
                Next
            End If
        End If

    End Sub

    Private Sub TogRPM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TogRPM.Click
        Select Case rpmode
            Case 1
                rpmode = 2
                TogRPM.Text = rpms(lang, 2)
                dGR = GR
                TBrpmSP.Text = Str(Int(RPMload * dGR))
                Tbrpm1.Text = Str(Int(rpm1 * dGR))
                TBRPM2.Text = Str(Int(rpm2 * dGR))
                'TBAcel.Text = Str(Int(Acel * dGR))
                Ldyno2.Text = "Engine"
            Case 2
                rpmode = 3
                TogRPM.Text = rpms(lang, 3)
            Case 3
                rpmode = 1
                TogRPM.Text = rpms(lang, 1)
                dGR = 1
                TBrpmSP.Text = Str(Int(RPMload))
                Tbrpm1.Text = Str(Int(rpm1))
                TBRPM2.Text = Str(Int(rpm2))
                'TBAcel.Text = Str(Int(Acel))
                Ldyno2.Text = "Dyno"
        End Select

    End Sub

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButRun.Click
        run = 0
        ButRun.Text = "Run: 0"
    End Sub

    Private Sub ButRunFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButRunFile.Click
        Dim SaveFileBox As New SaveFileDialog
        With SaveFileBox
            '     rundatafile = runfile + Str(run) + ".CSV"
            'rundatafile = runfile
            .FileName = DataPath + "\" + vehID
            .DefaultExt = "CSV"
            .Filter = "CSV|*.csv"
            .Title = "Enter Vehicle ID and File Location"
            Dim result = .ShowDialog()
            If result = DialogResult.OK Then

                rundatafile = .FileName

                If .FileName = "" Then .FileName = "PullData."
                If InStr(.FileName, ".") = 0 Then
                    .FileName = .FileName + "."
                End If
                runfile = Mid(rundatafile, 1, InStr(rundatafile, ".") - 1)

                vehID = runfile.Substring(runfile.LastIndexOf("\") + 1)         'Seperate our Vehicle ID
                DataPath = Mid(runfile, 1, Len(runfile) - Len(vehID) - 1)           'And Data Path

                'MsgBox("Run File: " + runfile + Chr(13) + "Path: " + DataPath + Chr(13) + "Veh ID: " + VehID)

                ButRunFile.Text = vehID
                run = 0                                                        'Reset Run Number
                TBRunFile.Text = runfile
            End If
        End With
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Select Case lang
            Case 0
                MsgBox("Want to see your logo here?" + Chr(13) + "To become a distributor, contact:" + Chr(13) + "www.FocusAppliedTechnologies.com" + Chr(13) + "+(6016) 484-6524")
            Case 1
                MsgBox("您想在这里看到您的徽标吗？" + Chr(13) + "要成为分销商，请联系我们:" + Chr(13) + "www.FocusAppliedTechnologies.com" + Chr(13) + "+(6016) 484-6524")
            Case 2
                MsgBox("Bạn có muốn xem logo của mình ở đây không?" + Chr(13) + "Để trở thành nhà phân phối, hãy liên hệ với chúng tôi:" + Chr(13) + "www.FocusAppliedTechnologies.com" + Chr(13) + "+(6016) 484-6524")
        End Select

    End Sub

    Private Sub ButMaxReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButMaxReset.Click
        powmax = 0
    End Sub
    Private Sub Z_OpenContCals() Handles Cals.opencc
        Dim bcc(100) As Byte
        Dim cct, checksum As Integer
        For i = 1 To 100                     'Erase Controller Cal Bytes
            ContCal(i) = 0
        Next

        'Verob OFF, Send "D" Recieve 51 bytes, then Verb back ON
        If Mid(mode, 1, 1) <> "C" Then
            MsgBox("Put controller in COMputer mode before attempting to EDIT calibrations.")
        Else
            If compt <> 0 Then
                COMport.Write("v")  'Set Verbose Mode OFF
                Sleep(350) 'Wait for 1 ping
                serenab = False                         'Shut OFF ser Com reading (of data)
                COMport.DiscardInBuffer()
                COMport.ReadTimeout = 100              'Just to throw an exception below
                COMport.DiscardNull = False             'Not needed
                COMport.Write("D")                      'Request DUMP of Dyno Controller Cals
                Try
                    For i = 1 To 51
                        ContCal(i) = COMport.ReadByte
                        ccc(i) = ContCal(i)
                    Next
                    'MsgBox("Controller Cals: " + Str(i - 1) + " Bytes Read")
                    Thread.Sleep(20)
                    COMport.DiscardInBuffer()
                    '===================Show and Populate Table
                    contcals.Show()
                    contcals.TextBox1.Text = Str(ContCal(1))                          'PPR1
                    contcals.TextBox2.Text = Str(ContCal(2))                          'PPR2
                    contcals.TextBox23.Text = Str(ContCal(3) + 256 * ContCal(4))       'Torq Calibration

                    contcals.TextBox19.Text = Str(ContCal(30) + 256 * ContCal(31) + 256 * 256 * ContCal(32) + 256 * 256 * 256 * ContCal(33))       'Torq Alarm
                    contcals.TextBox17.Text = Str(8 * (ContCal(7) / 8 - Int(ContCal(7) / 8)))                          'Torq Dec. Place (first 3 bits only)


                    contcals.TextBox3.Text = Str(ContCal(26) + ContCal(27) * 256)     'RPM1 Max
                    contcals.TextBox4.Text = Str(ContCal(28) + ContCal(29) * 256)     'RPM1 Alarm
                    contcals.TextBox24.Text = Str(ContCal(34))                        'Trq Filter

                    contcals.TextBox6.Text = Str(ContCal(8))                          'Kp rpm
                    contcals.TextBox8.Text = Str(ContCal(11))                         'Ki rpm
                    contcals.TextBox10.Text = Str(ContCal(14))                        'Kd rpm
                    contcals.TextBox5.Text = Str(ContCal(9) + ContCal(10) * 256)      'Fp
                    contcals.TextBox7.Text = Str(ContCal(12) + ContCal(13) * 256)     'Fi
                    contcals.TextBox9.Text = Str(ContCal(15) + ContCal(16) * 256)     'Fd

                    contcals.TextBox16.Text = Str(ContCal(17))                          'Kp Trq
                    contcals.TextBox14.Text = Str(ContCal(20))                          'Ki Trq
                    contcals.TextBox12.Text = Str(ContCal(23))                          'Kd Trq
                    contcals.TextBox15.Text = Str(ContCal(18) + ContCal(19) * 256)     'Fp
                    contcals.TextBox13.Text = Str(ContCal(21) + ContCal(22) * 256)     'Fi
                    contcals.TextBox11.Text = Str(ContCal(24) + ContCal(25) * 256)     'Fd

                    contcals.TextBox22.Text = Str(ContCal(35) + ContCal(36) * 256)     'Kfrict
                    contcals.TextBox20.Text = Str(ContCal(37) + ContCal(38) * 256)     'Kaero
                    contcals.TextBox18.Text = Str(ContCal(39) + ContCal(40) * 256)     'Kmass
                    '  MsgBox("5&6: " + Str(ContCal(5)) + " " + Str(ContCal(6)))

                    contcals.TBU1.Text = (ContCal(48) * 4.096 / 255).ToString("F2")     'UT1
                    contcals.TBL1.Text = (ContCal(49) * 4.096 / 255).ToString("F2")     'LT1
                    contcals.TBU2.Text = (ContCal(50) * 4.096 / 255).ToString("F2")     'UT2
                    contcals.TBL2.Text = (ContCal(51) * 4.096 / 255).ToString("F2")     'LT2

                    If ContCal(43) > 127 Then                                       'Got Checksum
                        checksum = 0
                        For i = 1 To 51
                            testb(i) = ContCal(i)
                            If i <> 43 Then checksum = checksum + ContCal(i)
                        Next
                        'checksum = (checksum And 127)                                   'Eliminate all but lower 7 bits
                        Console.WriteLine("Controller Read Checksum: " + Str(checksum And 127))
                        If (checksum And 127) <> (ContCal(43) And 127) Then
                            MsgBox("Bad Read from Controller: Checksum: " + Str(checksum And 127) + " Sent CS: " + Str(ContCal(43) And 127) + Chr(13) + "Please Exit and Reload")
                        End If
                    End If

                Catch ex As TimeoutException
                    MsgBox("Not Fully Loaded: " + Str(i) + " Bytes of 51" + Chr(13) + "First Bytes:" + Str(ContCal(1)) + Str(ContCal(2)) + Str(ContCal(3)) + Chr(13) + "Retry Getting Calibrations.")
                    COMport.DiscardInBuffer()
                End Try

                COMport.Write("V")                   'Set Verbose Mode ON
                serenab = True                      'Turn SerCom reading back on
            End If

        End If
    End Sub
    Private Sub Z_CloseContCals() Handles contcals.CCExit     'Clos Controller Calibrations Window
        contcals.Hide()                           'Just hide it
    End Sub
    Private Sub Z_SetCC() Handles contcals.SetCC               'Send Cals to Controller
        'Send "L" plus 51 bytes
        Dim bcc(100) As Byte
        Dim bsngl(2) As Byte
        Dim tempi, tempj, checksum As Integer
        ccz = 0
        '====================Read cals from text boxes and qualify
        ContCal(1) = Val(contcals.TextBox1.Text)
        ContCal(2) = Val(contcals.TextBox2.Text)
        If ContCal(1) < 1 Then ContCal(1) = 1 : contcals.TextBox1.Text = "1"
        If ContCal(2) < 1 Then ContCal(2) = 1 : contcals.TextBox2.Text = "1"
        If ContCal(1) > 255 Then ContCal(1) = 255 : contcals.TextBox1.Text = "255"
        If ContCal(2) > 255 Then ContCal(2) = 255 : contcals.TextBox2.Text = "255"

        ContCal(4) = Int(Val(contcals.TextBox23.Text) / 256)
        ContCal(3) = Val(contcals.TextBox23.Text) - 256 * ContCal(4) 'Torq Calibration


        ContCal(7) = Int(ContCal(7) / 8) * 8 + Val(contcals.TextBox17.Text)  'Torq DP
        ContCal(7) = (ContCal(7) And 231)                                       'Eliminate bits 3 and 4 (8 and 16)
        If contcals.CBTS.Checked = True Then ContCal(7) = ContCal(7) + 8
        If contcals.CBPS.Checked = True Then ContCal(7) = ContCal(7) + 16

        ' MsgBox("DP Byte" + Str(ContCal(7)))
        ' concal (30-33)=TextBox(19)
        tempi = Val(contcals.TextBox19.Text)
        tempj = Int(tempi / (256 ^ 3))
        ContCal(33) = tempj
        ' MsgBox(Str(tempi) + ", " + Str(tempj))
        tempi = tempi - (256 ^ 3) * tempj
        tempj = Int(tempi / (256 ^ 2))
        ContCal(32) = tempj
        ' MsgBox(Str(tempi) + ", " + Str(tempj))
        tempi = tempi - 256 * 256 * tempj
        tempj = Int(tempi / 256)
        ContCal(31) = tempj
        '  MsgBox(Str(tempi) + ", " + Str(tempj))
        tempi = tempi - 256 * tempj
        ContCal(30) = tempi
        ' MsgBox(Str(tempi) + ", " + Str(tempj))

        ContCal(34) = Val(contcals.TextBox24.Text)            'Trq Avging

        ContCal(27) = Int(Val(contcals.TextBox3.Text) / 256)
        ContCal(26) = Val(contcals.TextBox3.Text) - 256 * ContCal(27)
        ContCal(29) = Int(Val(contcals.TextBox4.Text) / 256)
        ContCal(28) = Val(contcals.TextBox4.Text) - 256 * ContCal(29)

        ContCal(8) = Val(contcals.TextBox6.Text)              'Kp rpm
        ContCal(11) = Val(contcals.TextBox8.Text)             'Ki rpm
        ContCal(14) = Val(contcals.TextBox10.Text)            'kD rpm

        ContCal(10) = Int(Val(contcals.TextBox5.Text) / 256)
        ContCal(9) = Val(contcals.TextBox5.Text) - 256 * ContCal(10)
        ContCal(13) = Int(Val(contcals.TextBox7.Text) / 256)
        ContCal(12) = Val(contcals.TextBox7.Text) - 256 * ContCal(13)
        ContCal(16) = Int(Val(contcals.TextBox9.Text) / 256)
        ContCal(15) = Val(contcals.TextBox9.Text) - 256 * ContCal(16)

        ContCal(17) = Val(contcals.TextBox16.Text)              'Kp Trq
        ContCal(20) = Val(contcals.TextBox14.Text)             'Ki 
        ContCal(23) = Val(contcals.TextBox12.Text)            'kD 


        ContCal(19) = Int(Val(contcals.TextBox15.Text) / 256)
        ContCal(18) = Val(contcals.TextBox15.Text) - 256 * ContCal(19)
        ContCal(22) = Int(Val(contcals.TextBox13.Text) / 256)
        ContCal(21) = Val(contcals.TextBox13.Text) - 256 * ContCal(22)
        ContCal(25) = Int(Val(contcals.TextBox11.Text) / 256)
        ContCal(24) = Val(contcals.TextBox11.Text) - 256 * ContCal(25)

        ContCal(36) = Int(Val(contcals.TextBox22.Text) / 256)
        ContCal(35) = Val(contcals.TextBox22.Text) - 256 * ContCal(36)
        ContCal(38) = Int(Val(contcals.TextBox20.Text) / 256)
        ContCal(37) = Val(contcals.TextBox20.Text) - 256 * ContCal(38)
        ContCal(40) = Int(Val(contcals.TextBox18.Text) / 256)
        ContCal(39) = Val(contcals.TextBox18.Text) - 256 * ContCal(40)
        If Val(contcals.TBU1.Text) > 4.096 Then contcals.TBU1.Text = "4.096"
        If Val(contcals.TBL1.Text) > 4.096 Then contcals.TBL1.Text = "4.096"
        If Val(contcals.TBU2.Text) > 4.096 Then contcals.TBU2.Text = "4.096"
        If Val(contcals.TBL2.Text) > 4.096 Then contcals.TBL2.Text = "4.096"
        ContCal(48) = Int(Val(contcals.TBU1.Text) * 255 / 4.096)                  'UT1
        ContCal(49) = Int(Val(contcals.TBL1.Text) * 255 / 4.096)                  'LT1
        ContCal(50) = Int(Val(contcals.TBU2.Text) * 255 / 4.096)                  'UT2
        ContCal(51) = Int(Val(contcals.TBL2.Text) * 255 / 4.096)                  'LT2


        checksum = 0
        For i = 1 To 51
            If i <> 43 Then checksum = checksum + ContCal(i)
        Next

        If ContCal(43) > 127 Then
            ContCal(43) = 128 + (checksum And 127)                  'Eliminate all but lower 7 bits (but only if USING CS, ie cc(43)>127)
        Else
            ContCal(43) = 0                                         'NO CHECKSUM!!
        End If

        For i = 1 To 51                                     'Check for too many zeros before sending
            bcc(i) = ContCal(i)
            If ContCal(i) = 0 Then ccz = ccz + 1
            If testb(i) <> ContCal(i) Then Console.WriteLine("Different from read at " + Str(i) + " Byte: " + Str(ContCal(i)) + "  Ori: " + Str(testb(i)))
        Next


        ' MsgBox("5&6: " + Str(bcc(5)) + " " + Str(bcc(6)))
        If ccz > 30 Then
            MsgBox("Controller Calibrations are INVALID (" + Str(ccz) + ") are Zero's")
        Else
            COMport.WriteLine("v")                                  'Verbose OFF
            Thread.Sleep(50)                                               'This is probably not needed
            bcc(0) = Asc("L")                                       'L means "Load"
            COMport.Write(bcc, 0, 52)                              'Send Controller Cals
            Thread.Sleep(100)                                      'Needs a pause while ser coms go out
            'MsgBox(Str(i - 1) + " Bytes Sent")
            'Turn Verbose back on
            Beep()                                                  'Beep in acknoledgement of sending
        End If

        COMport.WriteLine("V")
    End Sub
    Private Sub Z_Burn() Handles contcals.Burn       'Burn Controller Calibrations to Controller
        Dim bcc(100) As Byte
        bcc(0) = Asc("G")
        COMport.Write(bcc, 0, 1)
        Thread.Sleep(10)
        Beep()
    End Sub
    Private Sub SetTorqCal() Handles contcals.CalSet
        Dim calfact, CalFactNew As Double
        Console.WriteLine("Torque Re-Cal from Controller Cal")
        If torqraw <> 0 Then
            calfact = Val(contcals.TBTargNm.Text) / torqraw
            CalFactNew = Int(calfact * Val(contcals.TextBox23.Text))
            MsgBox("Adjustment Factor: " + Str(Int(calfact * 100) / 100) + Chr(13) + "New Torque Calibration: " + Str(CalFactNew))
            If calfact * Val(contcals.TextBox23.Text) > 9999 Then
                MsgBox("The controller calibration factor " + Str(CalFactNew) + " Is out of range." + Chr(13) + "Adjust Load Cell Amp or Change # of Decimal PLaces")
            Else
                contcals.TextBox23.Text = Str(CalFactNew)
                Call Z_SetCC()              'Set Cals)
            End If
        End If
    End Sub
    Private Sub Z_ContTare() Handles contcals.CTare
        COMport.Write("Z")
        Console.WriteLine("Tare from Controller Cal")
    End Sub
    Private Sub LoadView(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButView.Click
        Dim LoadFileBox As New OpenFileDialog
        Dim initcalfile As String
        initcalfile = calfile                                         'Save initial name
        With LoadFileBox
            .FileName = calfile
            .DefaultExt = "CSV"
            .Filter = "CSV|*.csv"
            .Title = "Load View Settings File"
            '  .InitialDirectory = applicationSettingsDirectory
            Dim result = .ShowDialog()
            If result = DialogResult.OK Then
                calfile = .FileName
                If System.IO.File.Exists(calfile) = True Then
                    For i = 1 To gagemax
                        ' Me.Panel2.Controls.Remove(gage(gagemax))
                        ghide(i) = False
                        l(i).TextAlign = ContentAlignment.TopCenter
                        l(i).AutoSize = False
                        l(i).Font = New Font("Arial", 8, FontStyle.Bold)
                        l(i).ForeColor = Color.DarkRed
                        l(i).BackColor = Color.Transparent
                        'l(i).BackColor = Color.White           'Uncomment to test label size
                        l(i).Left = 100 + i * 50
                        l(i).Top = 100
                        l(i).Height = 25
                        l(i).Text = "Indicator"
                        l(i).BringToFront()
                        l(i).Hide()

                        ' n(i).TextAlign = ContentAlignment.MiddleCenter
                        n(i).AutoSize = False
                        n(i).Font = New Font("Arial", 16, FontStyle.Bold)
                        n(i).ForeColor = Color.LimeGreen
                        n(i).BackColor = Color.Transparent
                        n(i).TextAlign = ContentAlignment.TopCenter
                        n(i).Height = 25
                        n(i).Text = "Indicator"
                        n(i).BringToFront()
                        n(i).Hide()
                    Next
                    gagemax = 0
                    Call loadcals()
                    'Put back name to orig cal file name
                End If
            End If
        End With
        calfile = initcalfile                                             'May not wanna use this here, so changes all go into the same view
    End Sub

    Private Sub SaveView(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButSave.Click
        Dim LoadFileBox As New SaveFileDialog
        Dim initcalfile As String
        initcalfile = calfile                                         'Save initial name
        With LoadFileBox
            .FileName = calfile
            .DefaultExt = "CSV"
            .Filter = "CSV|*.csv"
            .Title = "Save View Settings File"
            '  .InitialDirectory = applicationSettingsDirectory
            Dim result = .ShowDialog()
            If result = DialogResult.OK Then
                calfile = .FileName
                Call savecal()
                'Put back name to orig cal file name
            End If
        End With
        calfile = initcalfile                                     'Put back to whatever it was
    End Sub
    Public Sub Z_AutoFrict() Handles Cals.autofrict
        Dim temp As Integer
        temp = MessageBox.Show("Zero Torque and Spin dyno up to high speed." + Chr(13) + "Remove power/clutch Motor/Engine and hit OK" + Chr(13) + "Or Cancel this window to exit.", "Auto Calibrate Friction", MessageBoxButtons.OKCancel)
        'temp = MessageBox.Show("This option is not available yet.", "Auto Calibrate Friction", MessageBoxButtons.OKCancel)

        If temp = 1 Then
            CalBut.PerformClick()                       'Close Cal Box
            tempstr2 = tagline
            frictoft = frictof                          'Temporarily Store Friction Factors
            frictfactt = frictfact
            AF1 = afilt(1) : afilt(1) = 0               'Eliminate Filtering on Speed and Torque (almost)
            AF4 = afilt(4) : afilt(4) = 0.5
            AF5 = afilt(5) : afilt(5) = 0
            frictof = 0
            frictfact = 0
            ' MsgBox("doing it")
            FAC = 0
            FACon = True            'Friction will be calculated from decelleration of inertia (excluding Engine Inert)
            ietemp = inerteng       'Temporarily store Engine Inertia 
            inerteng = 0            'And set it = 0 so it doesn't figgure into friction
        End If
    End Sub
    Private Sub Lalarm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lalarm.Click
        If compt <> 0 Then COMport.Write("x")
    End Sub
    Private Sub ButBrek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButBrek.Click
        If brekon = False Then
            TBMode.Text = "BRAKE MODE"
            Call brek()
        Else
            Call brekrelease()
        End If
    End Sub
    Private Sub brek()
        prevmode = TBMode.Text
        lastload = 0
        ButBrek.BackColor = Color.Red
        LastLMode = loadMode
        CBMan.Checked = False
        CBRPM.Checked = False
        CB2Point.Checked = False
        CBAcel.Checked = False
        brekon = True
        Console.WriteLine("Breaking from mode: " + Str(LastLMode))
    End Sub
    Private Sub brekrelease()
        TBMode.Text = prevmode
        prevmode = ""
        ButBrek.BackColor = Color.Transparent
        loadMode = LastLMode
        Console.Write("Released: " + Str(Speed1) + "    Mode: " + Str(loadMode) + "  Comand: ")
        Select Case loadMode
            Case 1 : CBMan.Checked = True
            Case 2 : CBRPM.Checked = True
            Case 3 : CB2Point.Checked = True
            Case 4 : CBAcel.Checked = True
        End Select
        brekon = False
        sendcmd = "M0" : If compt <> 0 Then
            Sleep(1)
            COMport.Write(sendcmd)
        End If
        '888 send brake off load command?!
    End Sub
    Private Sub BuTune_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuTune.Click
        If Ftune.Visible Then
            Ftune.Hide()
        Else
            Console.WriteLine("Starting: " + TuneCal)
            Ftune.Show()
        End If
    End Sub
    Private Sub z_CloseTune() Handles Ftune.closeTune     'Close Tune Window
        Ftune.Hide()
    End Sub

    Private Sub Button2_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Blang.Click
        lang = lang + 1
        If lang = 3 Then lang = 0
        Select Case lang
            Case 0
                Blang.Text = "ENG"
            Case 1
                Blang.Text = "中文"
            Case 2
                Blang.Text = "Việt"

        End Select
    End Sub

    Private Sub BAL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BAL.Click
        If autoload = True Then
            autoload = False
            BAL.BackColor = butcol0
        Else
            autoload = True
            BAL.BackColor = butcol1
        End If
    End Sub

    Private Sub BAS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BAS.Click
        If AutoSave = True Then
            AutoSave = False
            BAS.BackColor = butcol0
        Else
            AutoSave = True
            BAS.BackColor = butcol1
        End If
    End Sub

    Private Sub BAG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BAG.Click
        If autograf = True Then
            autograf = False
            BAG.BackColor = butcol0
        Else
            autograf = True
            BAG.BackColor = butcol1
        End If
    End Sub

    Private Sub Binst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Binst.Click
        Dim li As Integer
        If time - instaClick < 1 Then
            play = Not (play)
            If play = True Then
                Binst.Text = "TUNE ASSIST"
                Binst.BackColor = butcol2
            Else
                Binst.Text = "INSTAGRAF"
                Binst.BackColor = butcol0
                If instgraf = True Then Binst.BackColor = butcol1
                If fgraf.Visible Then
                    INSTTop = fgraf.Top
                    INSTLeft = fgraf.Left
                    INSTWidth = fgraf.Width
                    INSTHeight = fgraf.Height
                    fgraf.Hide()
                    fgraf.Chart2.Series(4).Points.Clear()
                    For li = 1 To 4                                         'CLEAR OLD Instagraf DATA
                        fgraf.Chart1.Series(2 * li - 2).Points.Clear()
                        fgraf.Chart1.Series(2 * li - 1).Points.Clear()
                        fgraf.Chart2.Series(li - 1).Points.Clear()
                        fgraf.Chart2.ResetAutoValues()
                    Next

                End If
            End If
        Else
            If play = False Then
                If instgraf = True Then
                    instgraf = False
                    Binst.BackColor = butcol0
                    fgraf.Chart2.Series(4).Points.Clear()
                    For li = 1 To 4                                         'CLEAR OLD Instagraf DATA
                        fgraf.Chart1.Series(2 * li - 2).Points.Clear()
                        fgraf.Chart1.Series(2 * li - 1).Points.Clear()
                        fgraf.Chart2.Series(li - 1).Points.Clear()
                        fgraf.Chart2.ResetAutoValues()
                    Next

                Else
                    instgraf = True
                    Binst.BackColor = butcol1
                End If
            End If
        End If
        instaClick = time

        'If instgraf = True Then
        '    instgraf = False
        '    Binst.BackColor = butcol0
        'Else
        '    instgraf = True
        '    Binst.BackColor = butcol1
        'End If
        'For i = 0 To 7
        '    fgraf.Chart1.Series(i).Points.Clear()                     'Clear previous graf points
        'Next
        'For i = 0 To 3
        '    fgraf.Chart2.Series(i).Points.Clear()                     'Clear previous graf points
        'Next
    End Sub
    Private Sub CloseInst() Handles fgraf.quitinst
        INSTTop = fgraf.Top
        INSTLeft = fgraf.Left
        INSTWidth = fgraf.Width
        INSTHeight = fgraf.Height
        fgraf.Hide()
        If play = True Then
            play = False 'Shut OFF Testig mode automatically
            Binst.Text = "INSTAGRAF"
            instgraf = Not (instgraf)                                    'Invert to get back to orig setting
            If instgraf = True Then Binst.BackColor = butcol1
            If fgraf.Visible Then
                fgraf.Hide()
                fgraf.Chart2.Series(4).Points.Clear()
                For li = 1 To 4                                         'CLEAR OLD Instagraf DATA
                    fgraf.Chart1.Series(2 * li - 2).Points.Clear()
                    fgraf.Chart1.Series(2 * li - 1).Points.Clear()
                    fgraf.Chart2.Series(li - 1).Points.Clear()
                    fgraf.Chart2.ResetAutoValues()
                Next
            End If
        End If
    End Sub

    Private Sub Lpow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lpow.Click
        PowUnit = 1 - PowUnit           '1=hp, 0=kW
        Console.WriteLine("Now is " + Str(PowUnit))
    End Sub

    Private Sub Logging_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Logging.CheckedChanged
        If Logging.Checked = True Then
            Dim SaveFileBox As New SaveFileDialog
            With SaveFileBox
                rundatafile = runfile + "-LOG" + ".CSV"
                .FileName = rundatafile
                .DefaultExt = "CSV"
                .Filter = "CSV|*.csv"
                .Title = "Logging Data File"
                saveresult = .ShowDialog()
                If saveresult = DialogResult.OK Then
                    rundatafile = .FileName
                    If InStr(rundatafile, ".") > 3 Then runfile = Mid(rundatafile, 1, InStr(rundatafile, ".") - 3) 'Trim
                    FileClose(5)
                    FileOpen(5, rundatafile, OpenMode.Output)
                    PrintLine(5, "Focus Applied Technologies DynoMas Log Version: " + ver + "    Opened: " + Now)
                    PrintLine(5, " ")
                    Print(5, "Time")
                    For i = 1 To chan
                        Print(5, "," + alab(i))
                    Next
                    PrintLine(5, " ")
                    logon = True
                End If
            End With
        Else : logon = False
        End If
    End Sub
End Class

'-------------------------------------------------------------------------------
' Resizer
' This class is used to dynamically resize and reposition all controls on a form.
' Container controls are processed recursively so that all controls on the form
' are handled.
'
' Usage:
'  Resizing functionality requires only three lines of code on a form:
'
'  1. Create a form-level reference to the Resize class:
'     Dim myResizer as Resizer
'
'  2. In the Form_Load event, call the  Resizer class FIndAllControls method:
'     myResizer.FindAllControls(Me)
'
'  3. In the Form_Resize event, call the  Resizer class ResizeAllControls method:
'     myResizer.ResizeAllControls(Me)
'
'-------------------------------------------------------------------------------



Public Class Resizer

    '----------------------------------------------------------
    ' ControlInfo
    ' Structure of original state of all processed controls
    '----------------------------------------------------------
    Private Structure ControlInfo
        Public name As String
        Public parentName As String
        Public leftOffsetPercent As Double
        Public topOffsetPercent As Double
        Public heightPercent As Double
        Public originalHeight As Integer
        Public originalWidth As Integer
        Public widthPercent As Double
        Public originalFontSize As Single
    End Structure

    '-------------------------------------------------------------------------
    ' ctrlDict
    ' Dictionary of (control name, control info) for all processed controls
    '-------------------------------------------------------------------------
    Private ctrlDict As Dictionary(Of String, ControlInfo) = New Dictionary(Of String, ControlInfo)

    '----------------------------------------------------------------------------------------
    ' FindAllControls
    ' Recursive function to process all controls contained in the initially passed
    ' control container and store it in the Control dictionary
    '----------------------------------------------------------------------------------------
    Public Sub FindAllControls(ByVal thisCtrl As Control)

        '-- If the current control has a parent, store all original relative position
        '-- and size information in the dictionary.
        '-- Recursively call FindAllControls for each control contained in the
        '-- current Control
        For Each ctl As Control In thisCtrl.Controls
            Try
                If Not IsNothing(ctl.Parent) Then
                    Dim parentHeight = ctl.Parent.Height
                    Dim parentWidth = ctl.Parent.Width

                    Dim c As New ControlInfo
                    c.name = ctl.Name
                    c.parentName = ctl.Parent.Name
                    c.topOffsetPercent = Convert.ToDouble(ctl.Top) / Convert.ToDouble(parentHeight)
                    c.leftOffsetPercent = Convert.ToDouble(ctl.Left) / Convert.ToDouble(parentWidth)
                    c.heightPercent = Convert.ToDouble(ctl.Height) / Convert.ToDouble(parentHeight)
                    c.widthPercent = Convert.ToDouble(ctl.Width) / Convert.ToDouble(parentWidth)
                    c.originalFontSize = ctl.Font.Size
                    c.originalHeight = ctl.Height
                    c.originalWidth = ctl.Width
                    ctrlDict.Add(c.name, c)
                End If

            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try

            If ctl.Controls.Count > 0 Then
                FindAllControls(ctl)
            End If
        Next '-- For Each
    End Sub

    '----------------------------------------------------------------------------------------
    ' ResizeAllControls
    ' Recursive function to resize and reposition all controls contained in the Control
    ' dictionary
    '----------------------------------------------------------------------------------------
    Public Sub ResizeAllControls(ByVal thisCtrl As Control)

        Dim fontRatioW As Single
        Dim fontRatioH As Single
        Dim fontRatio As Single
        Dim f As Font

        '-- Resize and reposition all controls in the passed control
        For Each ctl As Control In thisCtrl.Controls
            Try
                If Not IsNothing(ctl.Parent) Then
                    Dim parentHeight = ctl.Parent.Height
                    Dim parentWidth = ctl.Parent.Width

                    Dim c As New ControlInfo

                    Dim ret As Boolean = False
                    Try
                        '-- Get the current control's info from the control info dictionary
                        ret = ctrlDict.TryGetValue(ctl.Name, c)

                        '-- If found, adjust the current control based on control relative
                        '-- size and position information stored in the dictionary
                        If (ret) Then
                            '-- Size
                            ctl.Width = Int(parentWidth * c.widthPercent)
                            ctl.Height = Int(parentHeight * c.heightPercent)
                            'MsgBox("First Ctl: " + ctl.Name)
                            '  If ctl.Name <> "LG1" AndAlso ctl.Name <> "LG2" AndAlso ctl.Name <> "LG3" Then
                            '-- Position
                            ctl.Top = Int(parentHeight * c.topOffsetPercent)
                            ctl.Left = Int(parentWidth * c.leftOffsetPercent)

                            '                            ctl.Font = New Font(f.FontFamily,
                            '                            c.originalFontSize * (1 + fontRatio), f.Style)
                            ' Else
                            '  ctl.Top = ctl.Top * (1 + c.heightPercent)
                            '  ctl.Left = ctl.Left * (1 + c.widthPercent)

                            '-- Font
                            f = ctl.Font
                            fontRatioW = ctl.Width / c.originalWidth
                            fontRatioH = ctl.Height / c.originalHeight
                            fontRatio = (fontRatioW +
                            fontRatioH) / 2 '-- average change in control Height and Width
                            ctl.Font = New Font(f.FontFamily,
                           c.originalFontSize * fontRatio, f.Style)
                            '                        End If
                        End If
                    Catch
                    End Try
                End If
            Catch ex As Exception
            End Try

            '-- Recursive call for controls contained in the current control
            If ctl.Controls.Count > 0 Then
                ResizeAllControls(ctl)
            End If

        Next '-- For Each
    End Sub

End Class
