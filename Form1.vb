Imports System                      'Dyno Monitor V-Basic for FAT Gen5 Dyno Controllers
Imports System.IO.Ports             'These "Imports" are just non-required subroutines required by this program
Imports System.Threading            'Goto View "Show Code" if you can't find this. 
Imports System.Threading.Thread     'Some times gotta run twice as it gives a bogus error on 1st run (No idea why).
Imports System.Drawing.Graphics
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing
Imports Microsoft.Vbe.Interop
Imports System.Management
'Imports System.Windows.Forms.MouseEventArgs


Public Module DefineGlobals         'This is to define all "global" variables so they can be used in other forms. 
    Public TFirst, Tacc As Boolean     'First time to run Tune?, Tune Accumulate ON?
    Public TuneCal As String        'Channels for X, Y and Z of Tune window
    Public Speed1, speed2, speedc2, torque, gr As Double
    Public throttle As Single
    Public Dynoload As Single
    Public analog(30) As Single
    Public frav As Single
End Module

'REVISIONS ================================================================================
'REV 7: Changed "check boxs" to Toggle Buttons for autos ----------------------------
'added AUto Save feature, Auto Tare
'Moved SAVE to RPM2
'Put in "clear data" when toggleing Instagraf
'Allow re-run of Pull without decending all the way to Idle (start rpm/2)
'Full Quad with averaging on Friction re-cal.
' Rev 8: Auto ID "Prolific" Ports as default ----------------------------------------
'Implement seperate Screen Refresh Rate
'Added Remote control and Load and Breaking capabilities, including Brekaing after PULL
'Changed GRAF o have 1 or 2 grafs, Fine/Corse grids, and "hidden" settings acessable via button
'Rev 9: 23-02-2021 Hz----------------------------------------------------------------
'Bold Major Graf grid lines
'Big Summary ON/OFF
'Set RPM "CLAMP" button to set current speed as set speed
'GRAF filtering
'Fixed insta graf so has all 4 of power, torque and AFR
'REV 10--------------------------------------------------------------------------------
'Made coms button RED if coms lost
'Move BIG labels with resize of GRAF, Color of labels in GRAF ikut graph colors
'Fixed Remote Load/Breaking with pause(20) to prevent "carshing" with outher outogin comands
'Only send updated LOAD commands (from a pull) every 100ms max (was sending too many redundent commands)
'Fixed a bit of the Remore mode in/out
'Put on Auto copy of previous Pull to graf2 (needs some help for stats and etc)
'Putin Graf option for having same parameters on both pull graphs
'Put in auto/man X axis min/max and Y1 and Y2 max
'Got gage and Skin colors, Graf Background colors
'Made BIG power label "transperent" in GRAF
'Variable line width on Graf Primary graf
'REV 11-------------------------------------------------------------------------------
'GRAF major grids at 10% of the span
'Added "target" line for AFR in Graf
'Set Grax "X" to close (not cansel)
'Added Load Bypass button on Front Screen
'Auto close Instagraf if "stalled" during pull
'Added "TESTING" mode with Instagraf held open and bypass save and Graf
'Got Temp/Pressure switch NO/NC settings in Controller Cals
'Added in Analog target value at Instagraf
'Click to close Instagraf
'Analog Graf with 4 major ticks
'REV 12 -----------------------------------------------------------------------------
'Fixed Linear Speed Calculation
'Fixed Lin Speed header in file
'AFR Target dips to 0 sometimes
'Fixed "X" at Controller Cals crashing
'Lost "summary Boxes" when BIG is on (at GRAF)
'Testing mode: make BIG, Alomst Opaque, Shows RPM
'Closing GRAF without Send last data to 2nd graf was crashing, fixed that
'REV 13 ----------------------------------------------------------------------------
'Use SPEED and Torque * FACT + OFS in power calcs (but not filtering)
'Implemented Weather station and SAE correction
'Implemented OBD
'REV14  ----------------------------------------------------------------------------
'Acelleration: in SEC not rpm/sec
'Display SAE when On, Variable KP (Acell) but only via direct editing in file
'Erased last leftover point in GRAF File Load
'Fixed small bug with chaning GR: Now sets Dyno RPM1 and 2 correctly for new GR
'Implemented CheckSum in controller Cals
'REV 15 ---------------------------------------------------------------------------
'Line showing where stats are from
'+,-, <,> and arrow keys can be used to move stats marker in Graf
'Only accumulate Average Ping time if RPM=0 and Load = 0 (to avoide distorsions when Coms slowed at computer from EMI)
'Fixed TiME as Analog crashing (Graf 2 MIN=MAX)
'Change RPM Cal Speeds: 1, 2, 4, 6, 8k rpm
'Fixed so can spesify Cal file from Command line
'Merged all TUNING loads together and got +/- buttons on them, and keyboard </> and +/-
'Added MAX and Averaged MAX line to gages, resets with right click
'OBD Data now saved to file and grafable
'Can use OBD speed in Gear Ratio Cal
'RED 16 --------------------------------------------------------------------------
' Got the Redline in GRAF a little closer to the actual stats point
' Save last Graf location, height and Width, and restart it at that place
'Right click RUN button to set run number to other than 0
'Kinda (?) got the form to re-start at the old size and position
'Scaled the Analog Instagraf to the Min/Max declared for the Analog channel being used
'Set up GRAF for 4 overlayed pulls
'REV 17 ---------------------------------------------------------------------------
'Changed Pull loading from 0-100% Load 1 at 50%-100% rpm1 to 0-100% Load 1 at 50-75% RPM1
'Added Remote buttons "UP"/DN" to increase/dec load (by mode) 
'Save position of Instageaf/PLAY
'Combind ALARM into RESET button and expanded button so we can see the alarm state full text
'REV18 ----------------------------------------------------------------------------
'Fixed "non reading" of Analog values in Tune Buddy and cleaned up display a little
'Fixed crazy decimals on "testing Analog y graf
'Added text numbers in Testing Graf
'Added Roller Brek
'Repositioned the Gage Units labels
'Allow Re-run of pull even if not down below RPM1 in Testing mode
'Added option to automatically close graf upon re-run of pull
'REV19 ----------------------------------------------------------------------------
'On Comparison GRAF can click "Cancel" on graf 3/4 to remove from display
'Put in fix for "double spaces" in incomming SerCom
'Made Graf Summary Max from Averaged Data (not raw)
'PINK RPm boxes if RPM2<RPM1
'Add AUTO Gear Ratio
'Dissalow AutoTare When Controller Cal's are open 
'Re-load TARE controller values when hit TARE from Controller Cals Form
'Fixed a LOAD ON glitch where wan's sending Load point and COLOR LOAD button Grn/Red is ON/Off
'REV20 ---------------------------------------------------------------------------
'Dissallow Pulls when Gear Ratio form is open
'Added Time and Pull Duration to Graf 
'Fixed "recomended" com port numbers 
'Print Screen fix for Magnicfcation screens
'REV21 --------------------------------------------------------------------------
'Added CLEAR button at Graf
'Save Run# at SW close (to avoid overwriting saved filed upon re-start)
'Added ESC/Enter to Cancel or SET from various forms
'Added Run# to the Log file
'In GRAF made Stats read from "smoothed" data of plots
'Added an RPM readout in Graf (if in pull mode)
'Fixed run#>100-Spaces in name
'Save as MAXimized/not
'X button -> YES/NO buttons
'REV22------------------------------------------------------------------------- 25-Sept-2023
'Graf X: change to Auto/Interval set (instead of Min/Max)
' Fixed OBD stuff
' Average "Max Power" a lot
' Made text boxes a darker gray when not in use
' Added 4th trace on Graf
' Grafs selectable colors
' Added "PULL"/"WAIT" to GRAF live RPM display
' Made GRAF stat #'s 2x bigger
' In PULL mode only send load if desaired load is different from actual. This hsould help "drops"
'REV23------------------------------------------------------------------------- June 2024
'Actually started using SAE correction factor
'Made RPM2 "auto selected" at GR select if got RPM1 and RPM2
'Fixed decimal places at GRAF stats
'REV24-------------------------------------------------------------------------- Mar 2026
'Print fixed in GRAF
'Fixed up a few things in Graf Compare
'Keep INSTAGRAF on top if in Play mode (Testing)
'Hit "G" to set GR to RPM2 IFF GR has been set to RPM2 in GR Set screen
'Fixed Gage Units from wandering around on resize
'===========================================================================================
'STILL TO DO:

'Prevent Load Dropouts during hard pulls
'Auto re-pull even if not down to start speed (as option)
'==========================================================================================




Public Class DynoMonitor

    Private WithEvents GearRatio As New GearRatio                   'Set Pull RPM factor
    Private WithEvents Ftune As New FTun                           'Tuning Screen
    Private WithEvents Cals As New Cals                             'Cal form
    Private WithEvents fcomp As New FComp                           'Graph Compare
    Private WithEvents dispsel As New Ldunno                       'Cal form
    Private WithEvents fgraf As New Fgraf
    Private WithEvents GrafSet As New GrafSet
    Private WithEvents contcals As New ContCals                     'Controler Calibration Form
    Private WithEvents FOBD As New FOBD                             'OBD Controls
    Private WithEvents Timer1 As New System.Windows.Forms.Timer()
    Private WithEvents Timer2 As New System.Windows.Forms.Timer()
    '  Private WithEvents DrvCycle As New Form
    ' Dim form2 As New form2
    Dim WithEvents COMport As New SerialPort
    Dim WithEvents EMISport As New SerialPort
    Dim WithEvents FSport As New SerialPort
    Dim WithEvents DAQport As New SerialPort
    Dim WithEvents OBDport As New SerialPort


    Dim RXtxt As String
    Dim x As Integer


    Dim rs As New Resizer                           'For Form auto Resize

    Dim sw = Stopwatch.StartNew()       'For Ser Com WDT
    Dim tsw, tstartWDT As TimeSpan         'For Ser Com WDT
    Dim ann As String           'Annotation for file
    Dim ptrq(10), ppow(10) As Double 'Max Torque and Power
    Dim pal As Boolean        'SI or Hp, Auto Load
    Dim panach, pam, psc, plm, pgrafanach, prun, pc As Integer       'Analog Channel to display (0 for none), and Auto Mode, Stability Count, load mode
    Dim precentrpm, precenttrq, paf, prf, PullTargAcell As Double                'Recent average Torque and Stimeavpeed (for determining of closed throttle at end of Pull test)
    Dim oldcb, Peng2 As Boolean        'Old RPM check box, Pull using RPM2 as Engine speed

    Dim SaveFile As String
    Dim CalFile As String
    Dim s, spare(100) As String             'Following are the Cal File variables (if used) delcared in order of appearance in the file
    Dim sparecnt As Integer                 'Number of "spare" cal file lines
    Dim dummy As String
    Dim ver As String
    Dim diam As Double
    Dim frictoff, frictofft, frictquadt As Double   'Frictional offset and Temporary value
    Dim frictfact, frictfactt, FrictQuad As Double 'Frictional Factor and Temporary Value
    Dim Fauto(2, 10001) As Double        'Frictional spin down (rpm, frict) for Auto Cal at 20Hz might need several minutes to spin down 180 x 20 = 3600 points
    Dim FAC, FACi, FACiFmin, FACi90, FACi10 As Integer            'Frictional AUto Cal Counters
    Dim Fmin, rpmin, rpm90, rpm10, fer90, fer10, frpmax As Double
    Dim TrqAvgT, Speed1AvgT As Double        'Temp values of Speed/Torque averaging
    Dim ContCal(100), ccc(100) As Integer            'Contoller Calibration Factors
    Dim CCz As Integer                  'Controller Clas that are Zero
    Dim comBR As Integer                'Dyno Controller Com Baud Rate
    Dim FACon As Boolean                'Frict Autocal On?
    Dim lastcal As String
    Dim dynotype As String

    Dim inert, inerteng, IETemp As Double   'Inert is Inertia of Dyno, InertEng is the inertia of the Engine at the dyno (Inertia egnine x gear ratio²)
    Dim dynoMaxTrq As Double    'Maximum torque of the Dyno at 100% load (for Pull loading estimate)
    Dim compt As Integer    'Com Port Value 
    Dim cpstr As String     'Com Port Input String
    Dim comstr As String    'Dyno Com Port in String form

    Dim i, j, k As Integer            'Loop counter
    Dim rt As Integer           'Refresh timer
    Public Shared sendcmd As String       'Remote Send Command


    Dim temp As Integer
    Dim find As Integer                     'Index of Fuel Measurment
    '    Dim km As Single                        'Kilometers Traveled
    Dim tss As Single                       'Time of Start of the drive Cycle
    Dim DCn As Integer                      'Drive Cycle Number (of cycles)
    Dim drvfile As String                   'Drive Cycle File Name
    Dim dt As Single                        'Drive Cycle Time
    Dim tcmax As Single                     'Max tie in Drive Cycle (to trigger quit)
    ' Dim dv As Single                        'Driver cycle Velocity

    Dim SAEtemp As Double                   'Temp for SAE correction Factor
    Dim SAEpress As Double                  'Pressure for SAE correstion Factor
    Dim SAErh As Double                     '%RH for SAE Correction Factor
    Public SAEcf As Double                     'SAE Correction Factor (nominally 1)
    Dim SAEauto, SAEcomp As Boolean                  'SAE Auto Temp/Press Collection?

    Dim serports As String                  'Names of all serial ports available
    Dim saveint As Integer                  'Data Logging Save Interval
    Dim savecnt As Integer                  'Save Integer count
    Dim tagline As String                   'Permanent Tag Line
    Dim tempstr, tempstr2 As String
    Dim jfile As String                    'Jpeg filename for screen shot

    Dim speedraw, speedrawo, speedrawd, speedrawod As Double
    Dim Speed1o, speed1od As Double   'Old Speed 1 value
    Dim speed1av, speed1avD As Double
    Dim speedaf As Double   'Averaging Factor
    Dim speed1d As Double
    Dim torqued As Single    'Raw + Friction + Inertial Torque
    Dim TorqueRaw, TorqueRawd As Single 'Raw torque reading from load cell	
    Dim torqueo, torqueod As Single
    Dim torqueAF As Single  'Torque Averaging Factor (also applied to Acellerational Torques)
    Dim acell As Single     'Acelleration average
    Dim torqueav, torqueavd As Single

    Dim mode As String
    Dim setpt As Double

    Dim throttleo As Single 'Old Throttle
    Dim power As Double
    Dim powmax As Double    'Max Power for Display
    Dim powmaxu As Boolean  'True = Kw, False = Hp
    Dim alarm As Integer
    Dim altxt As String     'Alarm Text (either OK, ALARM or Analog Channel: Value

    Dim time As Double
    Dim timeo As Double
    Dim tlastload As Single             'Time of last load command (during pull)
    Dim tctr As Integer     'Time Click Counter (from start of grpah scroll) for erasing old data
    Dim timeav As Double
    Dim tstart As Double
    Dim tgrafX, tgxo As Single   'Torque graph scale x and  Y and old values
    Dim tgrafY, tgyo As Single
    Dim tb(20) As TextBox   'For Display of DAQ Parameters
    Dim mtp(5), mpp(5), mtpr(5), mppr(5) As Label       'Max numbers for PULL screen

    Dim datafile As String          'For saving averaged instantanious data
    Dim savedat As Boolean          'Already got SaveDat file going?
    Dim HAspeed1, HASpeed2 As Double 'Heavy Average of parameters for "SAVE" datafile
    Dim HATorqueRaw As Double
    Dim HATorque As Double
    Dim HApower As Double
    Dim HAdynoload As Double
    Dim HAthrottle As Double
    Dim HAA1 As Double
    Dim HAA2 As Double
    Dim HAA3 As Double
    Dim HAA4 As Double
    Dim HABSFC As Double

    Dim starts(10) As String    'Start Comandline Strings

    Dim RPMDynoMax As Single        'Maximum Dyno RPM
    Public Analab(30), Anaunit(30), anadec(30) As String
    Public anamax(30), anamin(30), anafilt(30), anaofs(30), anafact(30), aavmx(30), aavmn(30) As Single      'Ana Avg Max and Min are for 
    Dim MaxON, MaxAV, maxfirst As Boolean
    Dim anaMajTix(30), anaMinTix(30) As Integer                                                                          'Resetable needels on the gages
    Dim ppr1, ppr2 As Integer
    Dim TorqDynoMax As Single
    Dim g1CH, g2CH, g3CH, d1CH, d2CH, d3CH, d4CH, d5ch, d6ch As Integer
    Dim G1ReTik, G2Retik, G3Retik As Boolean                    'If true, then Re-draw ticks/Labels
    Dim pullcom(5, 2) As String                                      'Pull Commands
    Dim pullmode, loadmode As Integer                                         ' Mode: Idle, Ramp, PULL, Return to idle
    Dim pulltime As Single                                          'Time from start of pull
    Dim rpm1, rpm2, load1, load2, Pload, acel, PullDur, PullKp As Single                          'Speeds and Loads for Pull
    Dim rpmload, manload As Single                  'Tuning load values
    Dim chan, dispCH As Integer                             'Number of dispay channels, Display Channle temporary
    Dim disprb(30) As RadioButton
    Dim dislab(30) As Label
    Dim run, dataindex As Integer                                  '# of Run
    Dim VehID, DataPath, runfile, rundatafile As String                               'File name for Individual Runs
    Dim timepull As Single                              'Time of Start of Pull
    Public timelaps As Single                           'Duration of Pull
    Dim pullmaxrpm As Single                            'Max RPM during pull (for stall calculation)
    Dim runsave As Boolean                              'Did we save this run's data or not ma?
    Public pulldata(4, 2002, 31)                               'pull data for saving to file
    Dim newtix1, newtix2, newtix3 As Boolean                'New Ticks?
    Public Xgraf, Y1Graf, Y2graf, Y3graf As Integer            'Channel numbers for the default X (rpm), Y1 (Torq) Y2 (power) and Y3 (AFR) channels
    Public Y4Graf As Integer
    Dim dgr As Single                                   'Display Gear Ratio
    Dim logo As Integer                                 'Number of Logo
    Dim deltaT As Single                                'Ping Rate Initial Target (sec)
    Dim g1al, g2al, g3al, g1ol, g2ol, g3ol As Boolean
    Public points As Integer                            'Number of data points in pull graf
    Dim ix As Single                                    'Counter with some resolution
    Dim serenab, calupdate As Boolean                              'Enable/dissable Setial Coms
    Public Shared nch As Integer                                  'Number of Chanbnels 
    Dim distnc As Single                                  'Distance from start of pull
    Public dic, calch As Integer                                 'Data index Counter (for pulls data)
    Dim calenab, admin As Boolean                                  'Enables editing of Calibrations, ADMIN=TRUE means no need for Cal Code
    Public AnaGrTop As Boolean                                     'Analog on top (or bottom) graf (in Graf)
    Public lang As Integer                              '0=English, 1=CN
    Dim coms(2), modes(5, 2), rembreks(2), remloads(2) As String
    Dim autograf, autoload, autosave, instagraf, autotare As Boolean  'Button Toggles
    Public butcol2, butcol1, butcol0, ColSkin(10), ColGage(10), ColGrafBak(10) As Color     'Colors to highlite buttons on/off
    Public Cskin, cgage As Integer                                 'Index of color for skin and gages
    Dim OCGage As Integer                                       'Old Gage Color
    Dim tareclick, taretime As Double
    Dim saveresult = DialogResult.OK
    Dim AFRPM, x1, x2, x3, y1, y2, y3 As Double                    'Auto Friction RPMs and Torques
    Dim AFAV1, afav2, afav3 As Integer                                             'Auto Frcit averages
    Dim scrnref, scrnlast As Single                             'Screen Refresh rate and time of last
    Dim refresh As Boolean                                      'Refresh this cycle?
    Dim brek2p, brekstop, brekramp, brekon, remote, loadon, remoff As Boolean
    Dim brekload As Single                                  '% Load for Breaking
    Dim RemV, lastrem As Double                             'Last Value of Remote
    Dim remch As Integer                                    'Remote Input Channel
    Dim prevmode As String                                  'Previous Mode box Text
    Dim LastLMode, lastload, testc As Integer               'Prev Load Mode, Lmode is a variable that just keeps track of the loading mode (0=none, 1=Man, 2= RPM, ...)
    Public GraFilt As Integer                               'Number of points to average in Graf
    Public grafgrid, grafsumtop, bigsum, G2Last, GAxSame, GManX, GManY, GMan2Y, GraFirst As Boolean          'Analog Fine Graf Grid, Summary at Top
    Public AutoAnaY1, AutoAnaY2 As Boolean                  'Auto Y Axis
    Public GRA1Y1, GRA1Y2, GRA2Y1, GRA2Y2 As Single         'Analog Axis Min/Max's
    Dim ComOK As Boolean                                    'Coms check out OK (or not)
    Public GRx1, GRx2, GRy2, GR2Y2 As Single                   'Manual Graph Axis numbers
    Public g1ratioW, g1ratioH, XSG3, YSG3, xsg1, ysg1, GrAnaTarg As Single
    Public GLineW, GBaCol As Integer                        'Graf Primary line width and Back Ground Color
    Dim alarms(4, 26) As String                               'Alarm Text
    Dim instaClick As Single                                'Time of last Instaclick
    Dim play As Boolean                                     '"Play" mode (instagraf ON but no pull logging)

    Public OBDon, OBDchon(20) As Boolean
    Dim OBDportnum, OBDbr, OBDnch, OBDBytes(20) As Integer
    Dim OBDinit, OBDcmd(20) As String
    Dim OBDtime, OBDLast As Single                                      'Refresh rate and time of last OBD ping
    Dim OBDCB(20) As CheckBox
    Public OBDisLab(20) As Label
    Dim testb(100) As Integer                                           'Checksum Test Bytes from Controller

    Dim roadload As Single                                              'For Tuning boxes/buttons
    Public GrafTop, GrafLeft, GrafWidth, GrafHeight As Integer          'Graf form position and size
    Dim INSTTop, INSTLeft, INSTWidth, INSTHeight As Integer          'Graf form position and size
    Private FTop, FLeft, FWidth, FHeight As Integer                     'This forms position and size
    Dim RolBrek, Fmax As Boolean                                        'Roller Brake (only actuated if RPM<200, then apply 50% for 5 sec), Is this Form Max?
    Dim RolBrekTime As Integer                                          'Time at start of Roller Brek (~5 sec)
    Public AutoCloseGraf As Boolean                                        'Automatically closes graf if new pull begun
    Dim LowSpeedPostPull As Single                                          'Lowest speed since post pull (to trigger re-pull)
    Dim AutoGR As Boolean                                               'Automatic Gear Ratio
    Dim GRAV, RPM2Av, AcelAv, TGRclk As Single                          'Auto Gear Ratio Controls
    Dim TrqTare1, TrqTare2 As Integer                                   'Torque Tare Value
    Public Col(20) As Color                                          'Graf Trace Colors
    Public Tcol(20) As Integer                                         'Color of the various graf traces
    Public MAG As Single                                                'Screen Magnification Factor





    Public Sub New()             ' This call is required by "the designer", whatever that means.
        InitializeComponent()    ' Add any initialization stuff in here. 
        Randomize(Date.Today.ToOADate)
        'starts = System.Environment.GetCommandLineArgs()
        Dim starts As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs         '1st parameter is the Cal Filename or "Chose" to 

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

        g1al = False : g1ol = False
        g2al = False : g2ol = False
        g3al = False : g3ol = False
        serenab = True                                              'Turn on (excpet when acessing controller cals)
        admin = False
        calenab = False

        disprb(1) = dispsel.RB1 : dislab(1) = dispsel.L1
        disprb(2) = dispsel.RB2 : dislab(2) = dispsel.L2
        disprb(3) = dispsel.RB3 : dislab(3) = dispsel.L3
        disprb(4) = dispsel.RB4 : dislab(4) = dispsel.L4
        disprb(5) = dispsel.RB5 : dislab(5) = dispsel.L5
        disprb(6) = dispsel.RB6 : dislab(6) = dispsel.L6
        disprb(7) = dispsel.RB7 : dislab(7) = dispsel.L7
        disprb(8) = dispsel.RB8 : dislab(8) = dispsel.L8
        disprb(9) = dispsel.RB9 : dislab(9) = dispsel.L9
        disprb(10) = dispsel.RB10 : dislab(10) = dispsel.L10
        disprb(11) = dispsel.RB11 : dislab(11) = dispsel.L11
        disprb(12) = dispsel.RB12 : dislab(12) = dispsel.L12
        disprb(13) = dispsel.RB13 : dislab(13) = dispsel.L13
        disprb(14) = dispsel.RB14 : dislab(14) = dispsel.L14
        disprb(15) = dispsel.RB15 : dislab(15) = dispsel.L15
        disprb(16) = dispsel.RB16 : dislab(16) = dispsel.L16
        disprb(17) = dispsel.RB17 : dislab(17) = dispsel.L17
        disprb(18) = dispsel.RB18 : dislab(18) = dispsel.L18
        disprb(19) = dispsel.RB19 : dislab(19) = dispsel.L19
        disprb(20) = dispsel.RB20 : dislab(20) = dispsel.L20
        disprb(21) = dispsel.RB21 : dislab(21) = dispsel.L21
        disprb(22) = dispsel.RB22 : dislab(22) = dispsel.L22
        disprb(23) = dispsel.RB23 : dislab(23) = dispsel.L23
        disprb(24) = dispsel.RB24 : dislab(24) = dispsel.L24
        disprb(25) = dispsel.RB25 : dislab(25) = dispsel.L25
        disprb(26) = dispsel.RB26 : dislab(26) = dispsel.L26
        disprb(27) = dispsel.RB27 : dislab(27) = dispsel.L27

        tb(1) = Cals.t1
        tb(2) = Cals.t2
        tb(3) = Cals.t3
        tb(4) = Cals.t4
        tb(5) = Cals.t5
        tb(6) = Cals.t6
        tb(7) = Cals.t7
        tb(8) = Cals.t8
        tb(9) = Cals.t9
        tb(10) = Cals.t10

        Cals.ChaName.Items.Add("RPM1 - Dyno")
        Cals.ChaName.Items.Add("RPM2 - Engine")
        Cals.ChaName.Items.Add("Eng Calc RPM")
        Cals.ChaName.Items.Add("Raw Dyno Torque")
        Cals.ChaName.Items.Add("Total Dyno Torque")
        Cals.ChaName.Items.Add("Engine Torque")
        Cals.ChaName.Items.Add("Power (kW)")
        Cals.ChaName.Items.Add("Power (hp)")
        Cals.ChaName.Items.Add("Load (%)")
        Cals.ChaName.Items.Add("Analog 1")
        Cals.ChaName.Items.Add("Analog 2")
        Cals.ChaName.Items.Add("Analog 3")
        Cals.ChaName.Items.Add("Analog 4")
        Cals.ChaName.Items.Add("Distance")
        Cals.ChaName.Items.Add("Linear Speed")

        Cals.ChaName.Items.Add("OBD Eng. Load") : OBDCB(1) = FOBD.CB1
        Cals.ChaName.Items.Add("OBD Coolant T") : OBDCB(2) = FOBD.CB2
        Cals.ChaName.Items.Add("OBD MAP") : OBDCB(3) = FOBD.CB3
        Cals.ChaName.Items.Add("OBD RPM") : OBDCB(4) = FOBD.CB4
        Cals.ChaName.Items.Add("OBD KPH") : OBDCB(5) = FOBD.CB5
        Cals.ChaName.Items.Add("OBD Spark Adv.") : OBDCB(6) = FOBD.CB6
        Cals.ChaName.Items.Add("OBD Air T") : OBDCB(7) = FOBD.CB7
        Cals.ChaName.Items.Add("OBD Mass AF") : OBDCB(8) = FOBD.CB8
        Cals.ChaName.Items.Add("OBD Throttle") : OBDCB(9) = FOBD.CB9
        Cals.ChaName.Items.Add("OBD O2 1") : OBDCB(10) = FOBD.CB10
        Cals.ChaName.Items.Add("OBD O2 2") : OBDCB(11) = FOBD.CB11
        Cals.ChaName.Items.Add("OBD Lambda") : OBDCB(12) = FOBD.CB12

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

        ColSkin(0) = Color.Silver
        ColSkin(1) = Color.Tan
        ColSkin(2) = Color.LightSteelBlue
        ColSkin(3) = Color.Thistle
        ColSkin(4) = Color.DarkKhaki

        ColGage(0) = Color.DarkGray
        ColGage(1) = Color.LightBlue
        ColGage(2) = Color.LightGreen
        ColGage(3) = Color.Red
        ColGage(4) = Color.Yellow

        ColGrafBak(0) = Color.White
        ColGrafBak(1) = Color.MistyRose
        ColGrafBak(2) = Color.BlanchedAlmond
        ColGrafBak(3) = Color.MintCream
        ColGrafBak(4) = Color.Azure

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
            fgraf.Chart2.Series(dic - 1).BorderWidth = 3
            fgraf.Chart2.Series(dic - 1).IsVisibleInLegend = False
            fgraf.Chart2.Series(dic - 1).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
            fgraf.Chart2.Series(dic - 1).Color = Color.Black
        Next
        fgraf.Chart2.Series.Add("TARG")                            '12th series
        fgraf.Chart2.Series(4).BorderWidth = 2
        fgraf.Chart2.Series(4).IsVisibleInLegend = False
        fgraf.Chart2.Series(4).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
        fgraf.Chart2.Series(4).Color = Color.DarkGoldenrod

        fgraf.Chart1.AntiAliasing = False
        fgraf.Chart2.AntiAliasing = False
   

        ' fgraf.Chart1.ChartAreas(0).AxisX.Minimum = Double.NaN
        'fgraf.Chart1.ChartAreas(0).AxisX.Minimum = 0

        fgraf.Chart1.ChartAreas(0).AxisX.LabelStyle.Format = "N0"
        fgraf.Chart1.ChartAreas(0).AxisY.Minimum = 0

        fgraf.Chart1.ChartAreas(0).AxisX.Maximum = Double.NaN
        fgraf.Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
        ' fgraf.Chart1.ChartAreas(0).AxisX.Maximum = 12000
        'fgraf.Chart1.ChartAreas(0).AxisY.Maximum = 250

        fgraf.Chart2.ChartAreas(0).AxisX.Minimum = Double.NaN
        fgraf.Chart2.ChartAreas(0).AxisX.Maximum = Double.NaN
        'fgraf.Chart2.ChartAreas(0).AxisX.Minimum = 0
        ' fgraf.Chart2.ChartAreas(0).AxisX.Maximum = 12000

        'fgraf.Chart2.ChartAreas(0).AxisY.Minimum = 0
        fgraf.Chart2.ChartAreas(0).AxisY.Maximum = Double.NaN
        'fgraf.Chart2.ChartAreas(0).AxisY.Maximum = 250

        fgraf.Chart2.ChartAreas(0).AxisX.LabelStyle.Format = "N0"
        fgraf.Chart1.ChartAreas(0).CursorX.IsUserEnabled = True

        Call Defaults()                     'Fisrt: Set up defaults to get any "new" parameters in by default

        If starts.Count > 0 Then
            If starts(0) = "Chose" Or starts(0) = "CHOSE" Or starts(0) = "chose" Then
                'Open "OPen Cal File" box
                Dim LoadFileBox As New OpenFileDialog
                With LoadFileBox
                    .FileName = CalFile
                    .DefaultExt = "CSV"
                    .Filter = "CSV|*.csv"
                    .Title = "Load Calibration File"
                    '  .InitialDirectory = applicationSettingsDirectory
                    Dim result = .ShowDialog()
                    If result = DialogResult.OK Then
                        CalFile = .FileName
                        'Call loadcals()
                    End If
                End With
            Else
                CalFile = starts(0)
                'Call loadcals()
            End If
        End If
        Call loadcals()
        '   Dim oFileInfo As New System.IO.FileInfo(CalFile)
        '  paths = oFileInfo.DirectoryName
        '  DataPath = paths                                  'Assign Data File Path
        '  pfile = paths + "\DynoPull.CSV"
        '  drivecyc = paths + "\ECER40.DRV"
        '  jfile = paths + "\Screen.JPG"
        '  CalFile = paths + "\DynoShopCal.CSV"
        '  runfile = paths + "\PullData"

        DCn = 0
        comstr = "COM" + LTrim(Str(compt))
        COMport.PortName = comstr    'OPen Dyno Controller Com Port: NOTE Can use USB com ports just like internal ones!
        If comBR = 0 Then comBR = 9600
        COMport.BaudRate = comBR
        COMport.ReadBufferSize = 128
        COMport.Handshake = Handshake.None
        COMport.RtsEnable = True
        COMport.DtrEnable = True
    
        If COMport.IsOpen Then COMport.Close()

        Try
            COMport.Open()

        Catch ex As Exception                       'This only allows 1 retry!!!
            serports = ""
            For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                serports = serports + sp + ", "
            Next
            tempstr2 = ""                               'This code segment allows "auto selection" of Prolific USB-SerCom converters
            Try
                Dim searcher As New ManagementObjectSearcher("root\cimv2", "SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0") ' PnPEntity Or SreialPort
                For Each queryObj As ManagementObject In searcher.Get()
                    tempstr = queryObj("Name")
                    If tempstr.Contains("COM") Then
                        ' Console.WriteLine(tempstr)
                        If tempstr.Contains("Prolific") Then
                            Dim nonNumericCharacters As New System.Text.RegularExpressions.Regex("[^0-9]")
                            tempstr = Mid(tempstr, InStr(tempstr, "("))                                     'To eliminate the non-Prolific com port numbers
                            tempstr = nonNumericCharacters.Replace(tempstr, String.Empty)
                            compt = Val(tempstr)                                                            'If there are multimple units it'll just 
                            'Console.WriteLine("Port: " + tempstr)                                           'Assign the last one as the default
                            tempstr2 = tempstr2 + tempstr + ", "                                            'But will "recomend" and Prolific cable ports
                        End If
                    End If
                Next
            Catch ex2 As Exception
                MsgBox("Got an error while looking for serial com port." + Chr(13) + "Either you switched USB ports, or its just Windows, nothing to worry about.")
            End Try
  
            cpstr = InputBox("The default " + comstr + " Does Not Exist. Available Ports:" + Chr(13) + "0=offline, " + serports + Chr(13) + Chr(13) + "Recomended Ports: " + tempstr2, "Enter New Com Port Number", compt, 5, 5)
            If cpstr <> "" Then
                compt = Val(cpstr)
                comstr = "COM" + LTrim(Str(compt))
                '     If PortIsAvailable(comstr) Then
                COMport.PortName = comstr    'OPen Dyno Controller Com Port: NOTE Can use USB com ports just like internal ones!
                cpstr = InputBox("Baud Rate: " + Str(comBR) + Chr(13) + "Typically 9600 or 115200" + Chr(13) + "Enter Dyno Controller Com Baud Rate ", , Str(comBR))
                If cpstr <> "" Then comBR = Val(cpstr)
                COMport.BaudRate = comBR
                If compt <> 0 Then COMport.Open() '0 for offline (and Simulation)
            Else
                End     'If they cancel, it's cuase they dont wanna run. 
            End If
        End Try

        SaveFile = UCase(CalFile)                      'As there could be multiple instances running, the dataloging file name must be unique, so use the Cal file name
        'MsgBox(CalFile + Chr(13) + SaveFile)
        If InStr(SaveFile, "CAL") Then                  'As a seed, and append LOG, or replace CAL with LOG to form new unique datalog file name
            SaveFile = Mid(CalFile, 1, InStr(SaveFile, "CAL") - 1) + "LOG" + Mid(CalFile, InStr(SaveFile, "CAL") + 3, Len(CalFile))
        Else
            If InStr(SaveFile, ".") Then
                SaveFile = Mid(CalFile, 1, InStr(CalFile, ".") - 1) + "LOG.CSV"
            Else
                SaveFile = CalFile + "LOG.CSV"
            End If

        End If

        If System.IO.File.Exists(SaveFile) Then                                      'Generate backup file
            Dim FileNameInfo As New System.IO.FileInfo(SaveFile)
            Dim path As String = FileNameInfo.DirectoryName
            Dim fileName As String = FileNameInfo.Name
            tempstr = fileName                                                      'Seperate file name for modification
            If InStr(SaveFile, ".") Then
                tempstr = Mid(tempstr, 1, InStr(tempstr, ".") - 1) + "BAK.CSV"
            Else
                tempstr = tempstr + "BAK.CSV"
            End If
            If System.IO.File.Exists(path + "\" + tempstr) Then My.Computer.FileSystem.DeleteFile(path + "\" + tempstr) 'Kill previous backup
            My.Computer.FileSystem.RenameFile(SaveFile, tempstr)                                         'Rename current to backup
        End If
        'MsgBox(CalFile + Chr(13) + SaveFile)
        'MsgBox("Opening output file")

        FileOpen(2, SaveFile, OpenMode.Output)          'OPen Output Datalogging File 
        WriteLine(2, "Focus Applied Technologies Dyno Controller Log Version: " + ver + "    Opened: " + Now)
        Write(2, "Time")
        For i = 1 To 27 : Write(2, Analab(i)) : Next
        WriteLine(2, " Run ")
        Write(2, "sec") : For i = 1 To 27 : Write(2, Anaunit(i)) : Next
        'Write(2, "Time (s)", "RPM1", "RPM2", "Torque Raw (Nm)", "Total Torque (Nm)", "Power (W)", "Load", "Mode", "SetPoint", "Throttle", "Alarm", "1:" + Analab(1), "2:" + Analab(2), "3:" + Analab(3), "4:" + Analab(4))
        WriteLine(2, " # ")
        ButLog.Text = SaveFile                        'Display Save File name so user doesn't forget it

        TFirst = True                          'Tuns hasn't been run yet this session
        'VerBox.Text = ver
        'TBDynoType.Text = dynotype
        Logging.Checked = True                  'Turn logging ON
        torqueav = 0
        'Start with difualt delta T, later update to average (in case we change update rates)
        tctr = 0                               'Strat removing graph points from 0
        tstart = Now.ToOADate                  'Start Time from now
        rt = 0                                  'Reset refresh timer
        If deltaT = 0 Then deltaT = 0.05 'Had a problem where it got set to 0 then couldn't average
        timeav = deltaT                           'Start with previous Delta T


        savecnt = 0

        find = 0
        If compt <> 0 Then COMport.DiscardInBuffer()
        powmax = 0
        savedat = False

        ann = ""
        'AddHandler ButEx.Click, AddressOf Me.Butex_Click                'Gotta do this for buttons that get messed up/renamed/deleated/etc. 


        AddHandler Gage1.Paint, AddressOf Me.Gage1_Paint
        'AddHandler GTIX1.paint, AddressOf Me.Gage1_Ticks
        AddHandler Gage2.Paint, AddressOf Me.Gage2_Paint
        AddHandler Gage3.Paint, AddressOf Me.Gage3_Paint
        AddHandler CBMan.CheckedChanged, AddressOf Me.CBManD
        AddHandler CBRPM.CheckedChanged, AddressOf Me.CBRPMD
        AddHandler CB2Point.CheckedChanged, AddressOf Me.CB2Point_CheckedChanged
        AddHandler CBAcel.CheckedChanged, AddressOf Me.CBAcel_CheckedChanged
        AddHandler CBRoad.CheckedChanged, AddressOf Me.CBRoadD
        AddHandler Flock.SendLock, AddressOf Me.LockSend                'Deals with lock out code stuff
        AddHandler Cals.LockCheck, AddressOf Me.LockChk                 'Check the Lock Status
        AddHandler FOBD.OBDclr, AddressOf Me.obdclearerr
        AddHandler FOBD.OBDchk, AddressOf Me.OBDCheckErr


        altxt = "ALARM"
        '    MsgBox("Size: " + Str(PictureBox5.Width) + " x " + Str(PictureBox5.Height))

        newtix1 = True
        newtix2 = True
        newtix3 = True          'Draw Tix first time

        Call Gage1_Ticks()
        Call Gage2_Ticks()
        Call Gage3_Ticks()

        LD1.Text = Analab(d1CH)
        LD2.Text = Analab(d2CH)
        LD3.Text = Analab(d3CH)
        LD4.Text = Analab(d4CH)
        LD5.Text = Analab(d5CH)
        LD6.Text = Analab(d6CH)
        LG1.Text = Analab(g1CH) : LUG1.Text = Anaunit(g1CH)
        LG2.Text = Analab(g2CH) : LUG2.Text = Anaunit(g2CH)
        LG3.Text = Analab(g3CH) : LUG3.Text = Anaunit(g3CH)
        ButRun.Text = "RUN: " + Str(run)
        TBRunFile.Text = runfile

        '  fgraf.Location = New Point(0, 0)

        ' x = Screen.PrimaryScreen.WorkingArea.Width - Me.Width
        ' y = Screen.PrimaryScreen.WorkingArea.Height - Me.Height
        '  Me.Location = New Point(x, y)
        Select Case logo
            Case 0
                PrintBut.BackgroundImage = My.Resources.Logo
            Case 1
                PrintBut.BackgroundImage = My.Resources.CBK
            Case 2
                PrintBut.BackgroundImage = My.Resources.KZR
            Case 3
                PrintBut.BackgroundImage = My.Resources.ZNB2
            Case 4
                PrintBut.BackgroundImage = My.Resources.Fast4ward
            Case 5
                PrintBut.BackgroundImage = My.Resources.GSM
            Case 6
                PrintBut.BackgroundImage = My.Resources.JO1
            Case 7
                PrintBut.BackgroundImage = My.Resources.DDM
            Case 8
                PrintBut.BackgroundImage = My.Resources.CMT
            Case 9
                PrintBut.BackgroundImage = My.Resources.XPM
        End Select


                PrintBut.BackgroundImageLayout = ImageLayout.Stretch
        'TBRoadSP.Text = "100"
        ButOBD.BackColor = Color.Transparent
        BUTAG.BackColor = Color.Transparent
        BUTAS.BackColor = Color.Transparent
        BUTAL.BackColor = Color.Transparent
        BRatio.BackColor = Color.Transparent
        BUTINSTA.BackColor = Color.Transparent
        BuTare.BackColor = Color.Transparent

                rs.FindAllControls(Me) 'For form autoresize
                LG1.Parent = Gage1 : LUG1.Parent = Gage1
                LG2.Parent = Gage2 : LUG2.Parent = Gage2
                LG3.Parent = Gage3 : LUG3.Parent = Gage3
                LG1.BackColor = Color.Transparent : LUG1.BackColor = Color.Transparent
                LG2.BackColor = Color.Transparent : LUG2.BackColor = Color.Transparent
                LG3.BackColor = Color.Transparent : LUG3.BackColor = Color.Transparent
                LG1.BringToFront() : LUG1.BringToFront()
                LG2.BringToFront() : LUG2.BringToFront()
                LG3.BringToFront() : LUG3.BringToFront()
                LG1.Left = 4 : LG1.Top = 7
                LG2.Left = 2 : LG2.Top = 4
                LG3.Left = 2 : LG3.Top = 4
        'LUG1.Left = 175 : LUG1.Top = 160                  'Positions hardwired in paint routine
        'LUG2.Left = 200 : LUG2.Top = 170
        'LUG3.Left = 200 : LUG3.Top = 170
        nch = 27                                        '"Hard wired" number of channels
                fcomp.first = True
                dic = 0                                       'Start with 1st position (for pull data graphs)
                calch = 1

                Timer2.Interval = 1000                         'Set timer for 1 sec
                Timer2.Start()
                GraFirst = True                                   'Mark as Graf not run yet
        If GrafWidth < 100 Or GrafHeight < 100 Then
            fcomp.Location = New Point(10, 10)
        Else
            fcomp.Location = New Point(GrafLeft, GrafTop)
            fcomp.Width = GrafWidth
            fcomp.Height = GrafHeight
        End If
        'Start 'em at the top so we can see their buttons on small screens
        GrafSet.Location = New Point(10, 10)                      'Start 'em at the top so we can see their buttons on small screens
        OBDon = False
        maxfirst = True
        If FWidth * FHeight <> 0 Then
            Me.Height = FHeight : Me.Width = FWidth : Me.Top = FTop : Me.Left = FLeft
        End If
        If Fmax Then Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub Defaults()
        ver = "Default"
        CalFile = "C:\Focus\DynoShopCal.CSV"
        runfile = "C:\Focus\DynoRun"
        DataPath = "C:\Focus"
        VehID = "ID"
        INSTTop = 10
        INSTLeft = 10
        INSTWidth = 1200 '893
        INSTHeight = 650 '430
        roadload = 100
        PullKp = 0.4                'Moderate Acell Gain
        PullDur = 15                  '15 Sec
        scrnref = 0.1             '100 ms screen refresh
        lang = 0                  'English
        logo = 0                  '(FAT)
        dgr = 3
        Xgraf = 2
        Y1Graf = 6
        Y2graf = 8
        Y3graf = 11
        Y4Graf = 12
        powmax = 0
        run = 0               ' no runs yet
        g1CH = 1              'Dyno Speed
        g2CH = 6              'Total Torque
        g3CH = 8              'AFR
        d1CH = 3              'Eng Calc
        d2CH = 6              'Load
        d3CH = 2              'RPM2
        d4CH = 7              'Throttle
        d5ch = 8
        d6ch = 9

        alarms(0, 0) = "ALARM"
        alarms(0, 1) = "TEMP SW"        '1
        alarms(0, 2) = "PRES SW"        '2
        alarms(0, 3) = "STALL"          '4
        alarms(0, 4) = "Over RPM"       '8
        alarms(0, 5) = "Over TRQ"       '16
        alarms(0, 6) = "COMP"           '32
        alarms(0, 7) = "ACPS"           '64
        alarms(0, 8) = "OTHER"          '128

        alarms(1, 0) = "警报"
        alarms(1, 1) = "TMP 关掉"        '1
        alarms(1, 2) = "PRS 关掉"        '2
        alarms(1, 3) = "失速"          '4
        alarms(1, 4) = "超速"       '8
        alarms(1, 5) = "过转矩"       '16
        alarms(1, 6) = "电脑"           '32
        alarms(1, 7) = "电源"           '64
        alarms(1, 8) = "其他警报"          '128

        pullcom(1, 0) = "IDLE: Increase speed"
        pullcom(2, 0) = "LOADING: Stady at start"
        pullcom(3, 0) = "PULL: WIDE OPEN THROTTLE"
        pullcom(4, 0) = "UNLOADING: Return to IDLE"
        pullcom(5, 0) = "Return to IDLE"
        pullcom(1, 1) = "空闲：提高速度"
        pullcom(2, 1) = "加载: 保持启动速度:"
        pullcom(3, 1) = "拉力：全节流"
        pullcom(4, 1) = "卸载：返回闲置状态"
        pullcom(5, 1) = "返回闲置状态"
        rembreks(0) = "BRAKING"
        remloads(0) = "LOAD OFF"
        rembreks(1) = "遥控刹车"
        remloads(1) = "远程卸载"

        coms(0) = "PUT CONTROLLER IN COM MODE" : coms(1) = "将控制器置于COM模式"
        modes(1, 0) = "MAN LOAD" : modes(1, 1) = "手动加载"
        modes(2, 0) = "SPEED LOAD" : modes(2, 1) = "速度负载"
        modes(3, 0) = "ROAD LOAD" : modes(3, 1) = "道路负荷"
        modes(4, 0) = "SELECT LOAD MODE" : modes(4, 1) = "选择加载模式"
        pullmode = 0
        RPMDynoMax = 8000                 'Max RPM of Dyno
        TorqDynoMax = 250
        ppr1 = 30
        ppr2 = 1
        gr = 4
        frictoff = 4
        frictfact = 0.002
        FrictQuad = 0
        lastcal = "Uncaled"
        dynotype = "MCD-100"
        diam = 0.165
        inert = 1.3
        compt = 5
        comBR = 115200
        '   Dim Analab(20), Anaunit(20) As String
        '   Dim anamax(20), anamin(20), anafilt(20), anaofs(20), anafact(20) As Single
        '   Dim anaMajTix(20), anaMinTix(20), anaDec(20) As Integer
        For i = 1 To 27
            Analab(i) = "CH" + Str(i) : Anaunit(i) = "Unit" : anamin(i) = 0 : anamax(i) = 100 : anafilt(i) = 0 : anafact(i) = 1 : anaofs(i) = 0 : anadec(i) = "N0" : anaMajTix(i) = 10 : anaMinTix(i) = 1
        Next
        Analab(1) = "Dyno Speed" : Anaunit(1) = "RPM" : anamin(1) = 0 : anamax(1) = RPMDynoMax : anaMajTix(1) = 5 : anaMinTix(1) = 5
        Analab(2) = "Engine Speed" : Anaunit(2) = "RPM" : anamin(2) = 0 : anamax(2) = 12000 : anaMajTix(2) = 10 : anaMinTix(2) = 1
        Analab(3) = "Engine Calc" : Anaunit(3) = "RPM" : anamin(3) = 0 : anamax(3) = 12000 : anaMajTix(3) = 10 : anaMinTix(3) = 1
        Analab(4) = "Dyno Torque" : Anaunit(4) = "Nm" : anamax(4) = TorqDynoMax : anadec(4) = "N1"
        Analab(5) = "Total Torque" : Anaunit(5) = "Nm" : anamax(5) = TorqDynoMax * 2 : anadec(5) = "N1"
        Analab(6) = "Engine Torq" : Anaunit(6) = "Nm" : anamax(6) = 50 : anadec(6) = "N2"
        Analab(7) = "Power kW" : Anaunit(7) = "kW" : anafact(7) = 1 : anamax(7) = TorqDynoMax * 2 * RPMDynoMax / 1000 : anadec(7) = "N1"
        Analab(8) = "Power hp" : Anaunit(8) = "hp" : anafact(8) = 1 : anamax(8) = TorqDynoMax * 2 * RPMDynoMax / 750 : anadec(8) = "N1"
        Analab(9) = "Load" : Anaunit(9) = "%" : anadec(9) = "N1"
        Analab(10) = "AFR (A1)" : Anaunit(10) = "%" : anaofs(10) = 8 : anafact(10) = 3 : anamax(10) = 23 : anadec(10) = "N1"
        Analab(11) = "Analog Input 2" : Anaunit(11) = "V" : anamax(11) = 5 : anadec(11) = "N2"
        Analab(12) = "Analog Input 3" : Anaunit(12) = "V" : anamax(12) = 5 : anadec(12) = "N2"
        Analab(13) = "Analog Input 4" : Anaunit(13) = "V" : anamax(13) = 5 : anadec(13) = "N2"
        Analab(14) = "Distance" : Anaunit(14) = "m" : anamax(14) = 1000 : anadec(14) = "N1"
        Analab(15) = "Linear Speed" : Anaunit(15) = "kph" : anamax(15) = 200 : anadec(15) = "N1"

        Analab(16) = "OBD % Load" : Anaunit(16) = "%" : anamax(16) = 120 : anadec(16) = "F0"
        Analab(17) = "OBD Eng. T" : Anaunit(17) = "C" : anamax(17) = 120 : anadec(17) = "F0"
        Analab(18) = "OBD MAP" : Anaunit(18) = "kPa" : anamax(18) = 110 : anadec(18) = "F0"
        Analab(19) = "OBD RPM" : Anaunit(19) = "rpm" : anamax(19) = 12000 : anadec(19) = "F0"
        Analab(20) = "OBD KPH" : Anaunit(20) = "kpm" : anamax(20) = 300 : anadec(20) = "F1"
        Analab(21) = "OBD Spark Adv." : Anaunit(21) = "deg" : anamax(21) = 45 : anadec(21) = "F1"
        Analab(22) = "OBD Air T" : Anaunit(22) = "C" : anamax(22) = 80 : anadec(22) = "F0"
        Analab(23) = "OBD Mass AF" : Anaunit(23) = "g/s" : anamax(23) = 400 : anadec(23) = "F1"
        Analab(24) = "OBD Throttle" : Anaunit(24) = "%" : anamax(24) = 100 : anadec(24) = "F1"
        Analab(25) = "OBD O2 1" : Anaunit(25) = "V" : anamax(25) = 1.2 : anadec(25) = "F2"
        Analab(26) = "OBD O2 2" : Anaunit(26) = "V" : anamax(26) = 1.2 : anadec(26) = "F2"
        Analab(27) = "OBD Fuel Pres." : Anaunit(27) = "kPa" : anamax(27) = 5000 : anadec(27) = "F1"

        brekload = 10
        brekramp = True
        brekstop = True
        brek2p = True

        deltaT = 0.25
        timeav = deltaT

        SAEtemp = 30
        SAEpress = 101.3
        saerh = 50
        saeauto = False                         'Assume no weather station
        SAEcomp = False                         'Dont Comp

        saveint = 1                             'Save ALL Data as default
        savecnt = 0

        powmaxu = True
        tagline = "For More Information Visit: WWW.FocusAppliedTechnologies.com"
        powmax = 0
        'Use Dyno speed as default
        plm = 5
        'Pick Default Location
        Peng2 = False

        speedaf = 0.5
        Logging.Checked = False                  'Turn logging OFF to avoid opening in a non-existant location

        TBMode.Text = "SELECT LOAD MODE"
        admin = False
        loadon = True
        GraFilt = 0                     'NO filtering in GRAF
        GRx1 = 0 : GRx2 = 12500         'Manual Graf Limits
        GRy2 = 100 : GR2Y2 = 100
        Cskin = 0 : cgage = 0
        GBaCol = 0                      'Default Graf Background Color White
        GLineW = 3                        'Primary line width = 3
        play = False                    'Not in "play" mode
        fcomp.Lrpm.BackColor = Color.White
        MAG = 1
    End Sub
    Private Sub Receiver(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs) Handles COMport.DataReceived
        If serenab = True Then          'Only false when loading Controller Cals
            Try
                RXtxt = COMport.ReadLine
                If Len(RXtxt) < 10 Then RXtxt = ""
                Me.Invoke(New MethodInvoker(AddressOf Display)) 'If it pukes on this when running, just RE-RUN!
            Catch ex As Exception
                RXtxt = "SerCom Error"
                ' MsgBox("Serial Com Glitch.")              'Conitinue (freezez if we don't catch this error)
            End Try                                             'Don't actually need the msgbox, but gotta have the "catch"
            '                                                    to avoid "freexing" the computer. 
        End If
    End Sub
    Private Sub Gage1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)  ' Load neddle gage
        Dim center, labpoint As Point
        Dim tickO, tickI As Point
        Dim baseL, baseR, BaseM, tip As Point
        Dim X1 As Integer = (Gage1.Width / 2)             'Y is 0 at top
        Dim y1 As Integer = Gage1.Height
        Dim ofsy As Integer
        Dim AnaTrunk As Double
        Dim aPen = New Pen(Color.DarkRed)
        aPen.Width = 2.0F
        Dim BPen = New Pen(Color.Black)
        BPen.Width = 2.0F

        center.X = X1
        center.Y = 0.77 * y1


        If MaxON Then                                                   'Draw Redline at MAX (if on)
            AnaTrunk = (aavmx(g1CH) - anamin(g1CH)) / (anamax(g1CH) - anamin(g1CH))
            If aavmx(g1CH) > anamax(g1CH) Then AnaTrunk = 1.01
            tip.X = center.X - 0.8 * X1 * Math.Cos(Math.PI * AnaTrunk)
            tip.Y = center.Y - 0.6 * y1 * Math.Sin(Math.PI * AnaTrunk)
            BaseM.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk)
            BaseM.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk)
            e.Graphics.DrawLine(aPen, BaseM, tip)
        End If

        AnaTrunk = (analog(g1CH) - anamin(g1CH)) / (anamax(g1CH) - anamin(g1CH))
        If analog(g1CH) < anamin(g1CH) Then AnaTrunk = -0.01

        If analog(g1CH) > anamax(g1CH) Then AnaTrunk = 1.01
        tip.X = center.X - 0.8 * X1 * Math.Cos(Math.PI * AnaTrunk)
        tip.Y = center.Y - 0.6 * y1 * Math.Sin(Math.PI * AnaTrunk)
        BaseM.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk)
        BaseM.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk)
        baseL.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk - 0.07)
        baseL.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk - 0.07)
        baseR.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk + 0.07)
        baseR.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk + 0.07)
        e.Graphics.DrawLine(aPen, BaseM, tip)
        e.Graphics.DrawLine(BPen, baseL, tip)
        e.Graphics.DrawLine(BPen, baseR, tip)

        g1al = False                                                    'If we just went into/out of Alarm
        If analog(g1CH) > anamax(g1CH) Then g1al = True 'Light it up up up up. light it up!
        If g1al = True And g1ol = False Then Call Gage1_Alarm(1)
        If g1al = False And g1ol = True Then Call Gage1_Alarm(0)
        g1ol = g1al
        LUG1.Left = Gage1.Width * 0.7       'Could never get these guys to resize propelry, so hard coading
        LUG1.Top = Gage1.Height * 0.85
    End Sub
    Private Sub Gage2_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)  ' Load neddle gage
        Dim center As Point
        Dim baseL, baseR, BaseM, tip As Point
        Dim X1 As Integer = (Gage2.Width / 2)             'Y is 0 at top
        Dim y1 As Integer = Gage2.Height
        Dim AnaTrunk As Double
        Dim aPen = New Pen(Color.DarkRed)
        aPen.Width = 2.0F
        Dim BPen = New Pen(Color.Black)
        BPen.Width = 2.0F

        center.X = X1
        center.Y = 0.77 * y1

        If MaxON Then                                                   'Draw Redline at MAX (if on)
            AnaTrunk = (aavmx(g2CH) - anamin(g2CH)) / (anamax(g2CH) - anamin(g2CH))
            If aavmx(g2CH) > anamax(g2CH) Then AnaTrunk = 1.01
            tip.X = center.X - 0.8 * X1 * Math.Cos(Math.PI * AnaTrunk)
            tip.Y = center.Y - 0.6 * y1 * Math.Sin(Math.PI * AnaTrunk)
            BaseM.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk)
            BaseM.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk)
            e.Graphics.DrawLine(aPen, BaseM, tip)
        End If

        AnaTrunk = (analog(g2CH) - anamin(g2CH)) / (anamax(g2CH) - anamin(g2CH))
        If analog(g2CH) > anamax(g2CH) Then AnaTrunk = 1.01
        If analog(g2CH) < anamin(g2CH) Then AnaTrunk = -0.01
        tip.X = center.X - 0.8 * X1 * Math.Cos(Math.PI * AnaTrunk)
        tip.Y = center.Y - 0.6 * y1 * Math.Sin(Math.PI * AnaTrunk)
        BaseM.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk)
        BaseM.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk)
        baseL.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk - 0.07)
        baseL.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk - 0.07)
        baseR.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk + 0.07)
        baseR.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk + 0.07)
        e.Graphics.DrawLine(aPen, BaseM, tip)
        e.Graphics.DrawLine(BPen, baseL, tip)
        e.Graphics.DrawLine(BPen, baseR, tip)

        g2al = False                                                    'If we just went into/out of Alarm
        If analog(g2CH) > anamax(g2CH) Then g2al = True 'Light it up up up up. light it up!
        If g2al = True And g2ol = False Then Call Gage2_Alarm(1)
        If g2al = False And g2ol = True Then Call Gage2_Alarm(0)
        g2ol = g2al
        LUG2.Left = Gage2.Width * 0.7       'Could never get these guys to resize propelry, so hard coading
        LUG2.Top = Gage2.Height * 0.85
    End Sub
    Private Sub Gage3_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)  ' Load neddle gage
        Dim center As Point
        Dim baseL, baseR, BaseM, tip As Point
        Dim X1 As Integer = (Gage3.Width / 2)             'Y is 0 at top
        Dim y1 As Integer = Gage3.Height
        Dim AnaTrunk As Double
        Dim aPen = New Pen(Color.DarkRed)
        aPen.Width = 2.0F
        Dim BPen = New Pen(Color.Black)
        BPen.Width = 2.0F

        center.X = X1
        center.Y = 0.77 * y1

        If MaxON Then                                                   'Draw Redline at MAX (if on)
            AnaTrunk = (aavmx(g3CH) - anamin(g3CH)) / (anamax(g3CH) - anamin(g3CH))
            If aavmx(g3CH) > anamax(g3CH) Then AnaTrunk = 1.01
            tip.X = center.X - 0.8 * X1 * Math.Cos(Math.PI * AnaTrunk)
            tip.Y = center.Y - 0.6 * y1 * Math.Sin(Math.PI * AnaTrunk)
            BaseM.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk)
            BaseM.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk)
            e.Graphics.DrawLine(aPen, BaseM, tip)
        End If


        AnaTrunk = (analog(g3CH) - anamin(g3CH)) / (anamax(g3CH) - anamin(g3CH))
        If analog(g3CH) < anamin(g3CH) Then AnaTrunk = -0.01
        If analog(g3CH) > anamax(g3CH) Then AnaTrunk = 1.01
        tip.X = center.X - 0.8 * X1 * Math.Cos(Math.PI * AnaTrunk)
        tip.Y = center.Y - 0.6 * y1 * Math.Sin(Math.PI * AnaTrunk)
        BaseM.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk)
        BaseM.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk)
        baseL.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk - 0.07)
        baseL.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk - 0.07)
        baseR.X = center.X - 0.1 * X1 * Math.Cos(Math.PI * AnaTrunk + 0.07)
        baseR.Y = center.Y - 0.06 * y1 * Math.Sin(Math.PI * AnaTrunk + 0.07)
        e.Graphics.DrawLine(aPen, BaseM, tip)
        e.Graphics.DrawLine(BPen, baseL, tip)
        e.Graphics.DrawLine(BPen, baseR, tip)
        g3al = False                                                    'If we just went into/out of Alarm
        If analog(g3CH) > anamax(g3CH) Then g3al = True 'Light it up up up up. light it up!
        If g3al = True And g3ol = False Then Call Gage3_Alarm(1)
        If g3al = False And g3ol = True Then Call Gage3_Alarm(0)
        g3ol = g3al
        LUG3.Left = Gage3.Width * 0.7       'Could never get these guys to resize propelry, so hard coading
        LUG3.Top = Gage3.Height * 0.85
    End Sub
    Private Sub Gage1_Alarm(ByVal al As Integer)  ' Load neddle gage
        Dim LEDoff As New Pen(Color.FromArgb(110, 90, 90))
        Using g As Graphics = Graphics.FromImage(Gage1.Image)
            For i = 10 To 48
                If al = 1 Then
                    g.DrawLine(Pens.Red, 345 + i * 2, i, 547, i)
                Else
                    g.DrawLine(LEDoff, 345 + i * 2, i, 547, i)
                End If
            Next
        End Using
    End Sub
    Private Sub Gage2_Alarm(ByVal al As Integer)  ' Load neddle gage
        Dim LEDoff As New Pen(Color.FromArgb(110, 90, 90))
        Using g As Graphics = Graphics.FromImage(Gage2.Image)
            For i = 10 To 48
                If al = 1 Then
                    g.DrawLine(Pens.Red, 345 + i * 2, i, 547, i)
                Else
                    g.DrawLine(LEDoff, 345 + i * 2, i, 547, i)
                End If
            Next
        End Using
    End Sub
    Private Sub Gage3_Alarm(ByVal al As Integer)  ' Load neddle gage
        Dim LEDoff As New Pen(Color.FromArgb(110, 90, 90))
        Using g As Graphics = Graphics.FromImage(Gage3.Image)
            For i = 10 To 48
                If al = 1 Then
                    g.DrawLine(Pens.Red, 345 + i * 2, i, 547, i)
                Else
                    g.DrawLine(LEDoff, 345 + i * 2, i, 547, i)
                End If
            Next
        End Using
    End Sub
    Private Sub Gage1_Ticks()  ' Load neddle gage
                Select cgage
            Case 0
                Gage1.Image = My.Resources.Gage_2_3
            Case 1
                Gage1.Image = My.Resources.Gage_2b
            Case 2
                Gage1.Image = My.Resources.Gage_2g
            Case 3
                Gage1.Image = My.Resources.Gage_2r
            Case 4
                Gage1.Image = My.Resources.Gage_2y
        End Select
        'Reload "Blank" gage then redraw ticks

        Dim center, labpoint As Point
        Dim tickO, tickI As Point
        Dim x1 As Integer = Gage1.Width * 0.92           'Y is 0 at top
        Dim y1 As Integer = Gage1.Height * 2 * 0.92     'For some reason I need this random scaling factor... Not needed when doing in 'Paint' routine 
        Dim ofsy, labwidth As Integer
        Dim fatpen As New Pen(Brushes.Black, 3)
        Dim tikval As Single                            'Number for Tick 
        x1 = 0.92 * 302                                 'Dont need to scale as this is drawing on the BMP, not the Paint Box
        y1 = 2 * 0.92 * 205

        center.X = x1
        center.Y = 0.77 * y1


        If 1 = 1 Then
            Using g As Graphics = Graphics.FromImage(Gage1.Image)

                For ix = 0 To 100 Step 100 / (anaMajTix(g1CH) * anaMinTix(g1CH))            'Minor Tix
                    tickO.X = center.X - 0.9 * x1 * Math.Cos(Math.PI * ix / 100)
                    tickO.Y = center.Y - 0.67 * y1 * Math.Sin(Math.PI * ix / 100)
                    tickI.X = center.X - 0.95 * x1 * Math.Cos(Math.PI * ix / 100)
                    tickI.Y = center.Y - 0.72 * y1 * Math.Sin(Math.PI * ix / 100)
                    g.DrawLine(Pens.Black, tickO, tickI)
                Next
                For ix = 0 To 100 Step 100 / anaMajTix(g1CH)
                    If ix = 0 Or ix > 99.5 Then                                                 'The 99.5 prevents "missing" offset with 99.99 situation
                        ofsy = y1 * 0.07                                                        'Vertical offset for first/last numbers
                    Else : ofsy = 0
                    End If
                    tickO.X = center.X - 0.85 * x1 * Math.Cos(Math.PI * ix / 100)
                    tickO.Y = center.Y - 0.62 * y1 * Math.Sin(Math.PI * ix / 100)
                    tickI.X = center.X - 0.95 * x1 * Math.Cos(Math.PI * ix / 100)
                    tickI.Y = center.Y - 0.72 * y1 * Math.Sin(Math.PI * ix / 100)
                    g.DrawLine(fatpen, tickO, tickI)



                    ' tikval = Int(ix * anamax(g1CH) + 0.5) / 100                                     'This rounds up to avoid "99.99" situations
                    tikval = anamin(g1CH) + ix * (anamax(g1CH) - anamin(g1CH)) / 100
                    'g.DrawString(tikval.ToString(anadec(g1CH)), New Font("Arial", 20, FontStyle.Bold), New SolidBrush(Color.Black), labpoint)
                    ' labwidth = g.MeasureString(Str(anamin(g1CH) + ix * (anamax(g1CH) - anamin(g1CH)) / 100), New Font("Arial", 20, FontStyle.Bold)).Width
                    labwidth = g.MeasureString((tikval).ToString(anadec(g1CH)), New Font("Arial", 20, FontStyle.Bold)).Width
                    labpoint.X = center.X - 0.78 * x1 * Math.Cos(Math.PI * ix / 100) - labwidth * 0.92 * 0.28 - 0.06 * x1
                    labpoint.Y = center.Y - 0.63 * y1 * Math.Sin(Math.PI * ix / 100) - ofsy
                    'g.DrawString((anamin(g1CH) + ix * (anamax(g1CH) - anamin(g1CH)) / 100).ToString(anadec(g1CH)), New Font("Arial", 20, FontStyle.Bold), New SolidBrush(Color.Black), labpoint)
                    g.DrawString(tikval.ToString(anadec(g1CH)), New Font("Arial", 20, FontStyle.Bold), New SolidBrush(Color.Black), labpoint)
                Next
            End Using
        End If
    End Sub
    Private Sub Gage2_Ticks()  ' Load neddle gage
        Dim center, labpoint As Point
        Dim tickO, tickI As Point
        Dim x1 As Integer = Gage2.Width * 1.37         'Y is 0 at top
        Dim y1 As Integer = Gage2.Height * 2 * 1.37
        Dim fatpen As New Pen(Brushes.Black, 3)
        Dim ofsy, labwidth As Integer

        x1 = 204 * 1.37                                       'No need to re-scale
        y1 = 2 * 1.37 * 136

        center.X = x1
        center.Y = 0.77 * y1

        Select Case cgage           'Could use an array, but then it'd take more memory (got array and images as well)
            Case 0
                Gage2.Image = My.Resources.Gage_2_3
            Case 1
                Gage2.Image = My.Resources.Gage_2b
            Case 2
                Gage2.Image = My.Resources.Gage_2g
            Case 3
                Gage2.Image = My.Resources.Gage_2r
            Case 4
                Gage2.Image = My.Resources.Gage_2y
        End Select

        Using g As Graphics = Graphics.FromImage(Gage2.Image)
            For ix = 0 To 100 Step 100 / (anaMajTix(g2CH) * anaMinTix(g2CH))
                tickO.X = center.X - 0.9 * x1 * Math.Cos(Math.PI * ix / 100)
                tickO.Y = center.Y - 0.67 * y1 * Math.Sin(Math.PI * ix / 100)
                tickI.X = center.X - 0.95 * x1 * Math.Cos(Math.PI * ix / 100)
                tickI.Y = center.Y - 0.72 * y1 * Math.Sin(Math.PI * ix / 100)
                g.DrawLine(Pens.Black, tickO, tickI)
            Next
            For ix = 0 To 100 Step 100 / anaMajTix(g2CH)
                If ix = 0 Or ix > 99.5 Then
                    ofsy = y1 * 0.07                              'Vertical offset for first/last numbers
                Else : ofsy = 0
                End If
                tickO.X = center.X - 0.85 * x1 * Math.Cos(Math.PI * ix / 100)
                tickO.Y = center.Y - 0.62 * y1 * Math.Sin(Math.PI * ix / 100)
                tickI.X = center.X - 0.95 * x1 * Math.Cos(Math.PI * ix / 100)
                tickI.Y = center.Y - 0.72 * y1 * Math.Sin(Math.PI * ix / 100)
                g.DrawLine(fatpen, tickO, tickI)

                'labwidth = g.MeasureString(Str(anamin(g2CH) + ix * (anamax(g2CH) - anamin(g2CH)) / 100), New Font("Arial", 20, FontStyle.Bold)).Width
                labwidth = g.MeasureString((anamin(g2CH) + ix * (anamax(g2CH) - anamin(g2CH)) / 100).ToString(anadec(g2CH)), New Font("Arial", 20, FontStyle.Bold)).Width
                labpoint.X = center.X - 0.78 * x1 * Math.Cos(Math.PI * ix / 100) - labwidth * 0.92 * 0.28 - 0.06 * x1
                labpoint.Y = center.Y - 0.63 * y1 * Math.Sin(Math.PI * ix / 100) - ofsy
                g.DrawString((anamin(g2CH) + ix * (anamax(g2CH) - anamin(g2CH)) / 100).ToString(anadec(g2CH)), New Font("Arial", 20, FontStyle.Bold), New SolidBrush(Color.Black), labpoint)
            Next
        End Using

    End Sub
    Private Sub Gage3_Ticks()  ' Load neddle gage
        Dim center, labpoint As Point
        Dim tickO, tickI As Point
        Dim x1 As Integer = Gage3.Width * 1.37         'Y is 0 at top
        Dim y1 As Integer = Gage3.Height * 2 * 1.37
        Dim fatpen As New Pen(Brushes.Black, 3)
        Dim ofsy, labwidth As Integer

        x1 = 204 * 1.37                                 'No need to re-scale
        y1 = 2 * 1.37 * 136

        center.X = x1
        center.Y = 0.77 * y1

        Select Case cgage
            Case 0
                Gage3.Image = My.Resources.Gage_2_3
            Case 1
                Gage3.Image = My.Resources.Gage_2b
            Case 2
                Gage3.Image = My.Resources.Gage_2g
            Case 3
                Gage3.Image = My.Resources.Gage_2r
            Case 4
                Gage3.Image = My.Resources.Gage_2y
        End Select

        Using g As Graphics = Graphics.FromImage(Gage3.Image)
            For ix = 0 To 100 Step 100 / (anaMajTix(g3CH) * anaMinTix(g3CH))
                tickO.X = center.X - 0.9 * x1 * Math.Cos(Math.PI * ix / 100)
                tickO.Y = center.Y - 0.67 * y1 * Math.Sin(Math.PI * ix / 100)
                tickI.X = center.X - 0.95 * x1 * Math.Cos(Math.PI * ix / 100)
                tickI.Y = center.Y - 0.72 * y1 * Math.Sin(Math.PI * ix / 100)
                g.DrawLine(Pens.Black, tickO, tickI)
            Next
            For ix = 0 To 100 Step 100 / anaMajTix(g3CH)
                If ix = 0 Or ix > 99.5 Then
                    ofsy = y1 * 0.07                              'Vertical offset for first/last numbers
                Else : ofsy = 0
                End If
                tickO.X = center.X - 0.85 * x1 * Math.Cos(Math.PI * ix / 100)
                tickO.Y = center.Y - 0.62 * y1 * Math.Sin(Math.PI * ix / 100)
                tickI.X = center.X - 0.95 * x1 * Math.Cos(Math.PI * ix / 100)
                tickI.Y = center.Y - 0.72 * y1 * Math.Sin(Math.PI * ix / 100)
                g.DrawLine(fatpen, tickO, tickI)

                'labwidth = g.MeasureString(Str(ix * anamax(g3CH) / 100), New Font("Arial", 20, FontStyle.Bold)).Width
                labwidth = g.MeasureString((anamin(g3CH) + ix * (anamax(g3CH) - anamin(g3CH)) / 100).ToString(anadec(g3CH)), New Font("Arial", 20, FontStyle.Bold)).Width
                labpoint.X = center.X - 0.78 * x1 * Math.Cos(Math.PI * ix / 100) - 0.06 * x1 - labwidth * 0.92 * 0.28
                labpoint.Y = center.Y - 0.63 * y1 * Math.Sin(Math.PI * ix / 100) - ofsy
                g.DrawString((anamin(g3CH) + ix * (anamax(g3CH) - anamin(g3CH)) / 100).ToString(anadec(g3CH)), New Font("Arial", 20, FontStyle.Bold), New SolidBrush(Color.Black), labpoint)
                'g.DrawString((ix * anamax(g3CH) / 100).ToString(anadec(g3CH)), New Font("Arial", 20, FontStyle.Bold), New SolidBrush(Color.Black), labpoint)
            Next
        End Using

    End Sub
    Private Sub timeout() Handles Timer2.Tick
        If COMport.IsOpen Then COMport.Write("V") 'Incase we're timed out but got a valid port, send a Verbose command to get back on-line
        Console.WriteLine("Senvding Verbose Comand.")
        Call Display()                      'Just run the display and COM button will go red

    End Sub

    Private Sub Display()           'This is the main loop. It's triggered by a serial read or the "clock" in SIMulation mode

        'Format should be:    ZSSSS1_SSSS2_TT.TTT_LLL_Mode_SP_TTL_Alarm_A1_A2_A3_A4 [CR] 
        'FuelWt = Val(RXtxt)
        time = Int(8640000 * (Now.ToOADate - tstart)) / 100           'Calculate time elapsed in seconds fractional
        If time < timeo Then timeo = timeo - 24 * 3600 'Compensate for 24 hour rollover
        If Math.Abs(time - scrnlast) >= scrnref Then
            scrnlast = time                                               'This allows refresh of gages and temp pull graf on a slower rate then the data
            refresh = True                                                'Turn to FALSE at end of loop
        End If
        rt = rt + 1
        Cals.LDT.Text = "Ping: " + (time - timeo).ToString("N3")

        If RolBrek = True Then                                           'If 5 sec since break actuation, RELEASE breaks!
            If time - RolBrekTime >= 8 Then BrollerBrek.PerformClick()
        End If

        If autotare = True Then                                             'AUTO TARE
            If time - taretime > 60 Then
                If Speed1 < 1 And Dynoload < 1 Then
                    taretime = time
                    If COMport.IsOpen And Not (contcals.Visible) Then COMport.Write("Z") 'Tare Load Cell
                End If
            End If
        End If

        ComOK = False                                                             'Say it's bad unless we get a ping
        If InStr(RXtxt, "Z") = 1 And Len(RXtxt) > 30 And InStr(RXtxt, "!") = 0 And InStr(RXtxt, "<") = 0 And InStr(RXtxt, "?") = 0 Then '?<>! all indicate bad data
            ComOK = True                                                        'Got a valid looking ping
            Timer2.Stop() : Timer2.Start()                                      'Recycle timer to prevent firing
            TextBox1.Text = RXtxt                                               'Echo to screen 
            RXtxt = RXtxt.Replace("  ", " ")                                    'Get rid of double spaces, set to single
            RXtxt = Mid(RXtxt, 2, Len(RXtxt) - 1)
            Speed1 = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            speed2 = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            TorqueRawd = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
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
            analog(10) = analog(10) * anafilt(10) + (1 - anafilt(10)) * ((Val(Mid(RXtxt, 1, InStr(RXtxt, " "))) * 5 / 1023) * anafact(10) + anaofs(10))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            analog(11) = analog(11) * anafilt(11) + (1 - anafilt(11)) * ((Val(Mid(RXtxt, 1, InStr(RXtxt, " "))) * 5 / 1023) * anafact(11) + anaofs(11))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            analog(12) = analog(12) * anafilt(12) + (1 - anafilt(12)) * ((Val(Mid(RXtxt, 1, InStr(RXtxt, " "))) * 5 / 1023) * anafact(12) + anaofs(12))
            RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
            analog(13) = analog(13) * anafilt(13) + (1 - anafilt(13)) * ((Val(RXtxt) * 5 / 1023) * anafact(13) + anaofs(13))   'Units = V x Anafact + Anaofs
            If SAEauto = True Then
                If Len(RXtxt) > 4 Then
                    RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
                    SAEpress = 10 + (Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))) / 10                        'Coding is (kPa - 10) *10
                    RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
                    SAEtemp = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
                    RXtxt = Mid(RXtxt, InStr(RXtxt, " ") + 1, Len(RXtxt) - 1)
                    SAErh = Val(Mid(RXtxt, 1, InStr(RXtxt, " ")))
                End If
            End If

            If SAEcomp = False Then
                SAEcf = 1 : LSAE.Text = "" 'Set to 1 if not using
            Else
                If SAEpress >= 95 And SAEpress <= 105 And SAEtemp >= 15 And SAEtemp <= 35 Then
                    SAEcf = 1.176 * ((99 / SAEpress) * ((SAEtemp + 273) / 298) ^ 0.5) - 0.176 'Calculate Correction Factor
                    LSAE.Text = "SAE"
                Else : SAEcf = 1.0 : LSAE.Text = ""                                'Set to 1 if out of range
                End If
            End If

            RXtxt = ""
            'ACCUMULATE AVERAGE DELTA T: Only accumulate if rpm1 is 0 and Load = 0 (to eliminate accumulating during a pull when EMI can delay the reciept of data, and effect Delta T)
            If Speed1 < 1 And Dynoload = 0 Then
                If Math.Abs(Math.Abs(time - timeo) - timeav) < timeav * 10 Then timeav = timeav * 0.99 + Math.Abs(time - timeo) * 0.01 'Delta T in windows is +/- 15ms, so average heavily. When USB "Seizes" this will keep a 
            End If

            deltaT = timeav                                                     'Setus up for distance measurment and save in cals
            timeo = time                                                        'better delta T for acelleration calculations

            If alarm = 0 Then
                EStop.BackColor = Color.Transparent : EStop.Text = alarms(lang, 0)
            Else
                EStop.BackColor = Color.Red : EStop.Text = alarms(lang, 1 + Int(Math.Log(alarm, 2)))
            End If

            If time < 0.5 Then speed1avD = speed1d 'Start Avg speed at actual when starting to avoid "infinite" acellerations if restarting controller while on the run

            analog(1) = analog(1) * anafilt(1) + (1 - anafilt(1)) * (Speed1 * anafact(1) + anaofs(1))                                       '1=RPM1
            analog(2) = analog(2) * anafilt(2) + (1 - anafilt(2)) * (speed2 * anafact(2) + anaofs(2))                                       '2=RPM2
            speedc2 = speed2 * anafact(2) + anaofs(2)                                                'For torque and GR calcs, use the "calibrated" number w/o filtering
            analog(3) = analog(3) * anafilt(3) + (1 - anafilt(3)) * ((Speed1 * gr) * anafact(3) + anaofs(3))                                '3=Calced Engine Speed
            analog(4) = analog(4) * anafilt(4) + (1 - anafilt(4)) * (TorqueRawd * anafact(4) + anaofs(4))                                   '4=Dyno Torque

            If Speed1 = 0 Then                                                                                                              'This loses the friction offset at RPM=0
                torqued = (TorqueRawd * anafact(4) + anaofs(4)) + (Speed1 * anafact(1) + anaofs(1)) * frictfact + FrictQuad * (Speed1 * anafact(1) + anaofs(1)) ^ 2           'Add frictional compensation  
            Else
                torqued = (TorqueRawd * anafact(4) + anaofs(4)) + frictoff + (Speed1 * anafact(1) + anaofs(1)) * frictfact + FrictQuad * (Speed1 * anafact(1) + anaofs(1)) ^ 2           'Add frictional compensation  
            End If

            If timeav <> 0 Then                                                 'Acellerational (Inertial) Torque Calculation
                speedrawd = Speed1 * anafact(1) + anaofs(1) 'Delete this line to use raw # (else inert torq is from averaged numbers)
                acell = ((speedrawd - speedrawod) * anafact(1) + anaofs(1)) * 2 * Math.PI / (60 * timeav)  'Non Averaged Speed
                speedrawod = speedrawd
                torqued = torqued + (inert + inerteng) * acell                  'Acellerational Torque from Inertia. 
            End If

            If SAEcomp = True And Math.Abs(1 - SAEcf) < 0.05 Then torqued = torqued * SAEcf

            analog(5) = analog(5) * anafilt(5) + (1 - anafilt(5)) * torqued * anafact(5) + anaofs(5)                                        '5 = Total Torq
            analog(6) = analog(6) * anafilt(6) + (1 - anafilt(6)) * ((torqued / gr) * anafact(6) + anaofs(6))                               '6 = Engine Torque
            analog(7) = analog(7) * anafilt(7) + (1 - anafilt(7)) * (torqued * (Speed1 * anafact(1) + anaofs(1)) * 2 * SAEcf * (Math.PI / 60000) * anafact(7) + anaofs(7))     '7=kW
            analog(8) = analog(8) * anafilt(8) + (1 - anafilt(8)) * (torqued * (Speed1 * anafact(1) + anaofs(1)) * 2 * SAEcf * (Math.PI / 60 / 750) * anafact(8) + anaofs(8))  '8=hp
            analog(9) = analog(9) * anafilt(9) + (1 - anafilt(9)) * (Dynoload * anafact(9) + anaofs(9))                                     '9=Load
            distnc = distnc + Speed1 * deltaT * Math.PI * diam / 60    'Distance in Meters                                                  '10-13 are the Analog nputs (above)
            analog(14) = analog(14) * anafilt(14) + (1 - anafilt(14)) * (distnc * anafact(14) + anaofs(14))                                 '14th is Distance
            analog(15) = analog(15) * anafilt(15) + (1 - anafilt(15)) * (Speed1 * Math.PI * diam * 60 * anafact(15) / 1000 + anaofs(15))     '15th is Linear Speed

            lastrem = RemV : RemV = analog(9 + remch)            'Remote Value and previous Rev Value
            '    Console.WriteLine("Remote: " + Str(RemV))
            contcals.TBActNm.Text = analog(4)                       'Send actual torque to TB in Controller Cal form
            contcals.TBRPM1.Text = analog(1)
            contcals.TBRPM2.Text = analog(2)
            speed1d = analog(1)                                     'This setsup Auto Cal Frict


            If AutoGR = True Then                                       'Check and set up the GR automatically
                RPM2Av = 0.9 * RPM2Av + 0.1 * speedc2
                AcelAv = 0.92 * AcelAv + 0.08 * (acell)
                'Console.WriteLine("Acell Av:" + AcelAv.ToString("F3"))
                If AcelAv < 20 And AcelAv > -2 And RPM2Av > 100 And analog(1) > 10 And Math.Abs(RPM2Av - speedc2) < 0.05 * RPM2Av Then
                    GRAV = 0.9 * GRAV + 0.1 * (RPM2Av / analog(1))
                    If Math.Abs(GRAV - RPM2Av / analog(1)) < 0.03 * (RPM2Av / analog(1)) Then gr = GRAV
                End If
            End If '============== AUTO GR ===================== AUTO GR =====================


            If OBDon And Math.Abs(time - OBDLast) > OBDtime Then        'PROCESS OBD 
                OBDLast = time
                Call obdprocess()
            End If

            'ALL ANALOGS SET UP, NOW check for MIN/MAX
            If MaxON Then
                If maxfirst = True Then
                    For i = 1 To 30
                        aavmx(i) = analog(i)
                        aavmn(i) = analog(i)
                    Next
                    maxfirst = False
                Else
                    For i = 1 To 30
                        If MaxAV Then
                            aavmn(i) = aavmn(i) * 0.99 + 0.01 * analog(i)
                            aavmx(i) = aavmx(i) * 0.99 + 0.01 * analog(i)
                        End If
                        If analog(i) < aavmn(i) Then aavmn(i) = analog(i)
                        If analog(i) > aavmx(i) Then aavmx(i) = analog(i)
                    Next
                End If
            End If


            'DELETER THESE LINES (just for testing OBD):
            ' OBDchon(1) = True : analog(16) = Dynoload * 2
            ' OBDchon(4) = True : analog(19) = Speed1 * 2.3

            If powmaxu = False Then
                If analog(8) * 0.1 + powmax * 0.9 > powmax Then powmax = powmax * 0.9 + 0.1 * analog(8) 'Set max hp Power in text box (Heavy Average)
            Else
                If analog(7) * 0.1 + powmax * 0.9 > powmax Then powmax = powmax * 0.9 + 0.1 * analog(7) 'Set max kW Power in text box
            End If

            If CBAcel.Checked = False Then : TBAcel.BackColor = Color.Gray
            Else
                If TBAcel.Focused = False Then
                    If PullDur < 10 Or PullDur > 25 Then
                        TBAcel.BackColor = Color.Pink
                    Else
                        TBAcel.BackColor = Color.White
                    End If
                End If
            End If
            If loadon = False Then
                TBTarg.BackColor = Color.Gray
                TBLoad1.BackColor = Color.Gray : Tbrpm1.BackColor = Color.Gray : TBRPM2.BackColor = Color.Gray
                TBLoad2.BackColor = Color.Gray
            Else
                If CBRoad.Checked = True Or CBMan.Checked = True Or CBRPM.Checked = True Then : TBTarg.BackColor = Color.White : Else : TBTarg.BackColor = Color.Gray : End If
                If CBRoad.Checked = True Or CBMan.Checked = True Or CBRPM.Checked = True Then : TBTarg.BackColor = Color.White : Else : TBTarg.BackColor = Color.Gray : End If
                If CB2Point.Checked = False Then : TBLoad2.BackColor = Color.Gray : Else : TBLoad2.BackColor = Color.White : End If
                If CBAcel.Checked = False AndAlso CB2Point.Checked = False Then
                    TBLoad1.BackColor = Color.Gray : Tbrpm1.BackColor = Color.Gray : TBRPM2.BackColor = Color.Gray
                Else
                    TBLoad1.BackColor = Color.White : Tbrpm1.BackColor = Color.White : TBRPM2.BackColor = Color.White
                    If rpm1 > rpm2 Then Tbrpm1.BackColor = Color.Pink : TBRPM2.BackColor = Color.Pink 'This one's for you , Stephan!
                End If
            End If
            ' If CBRoad.Checked = True Or CBMan.Checked = True Or CBRPM.Checked = True Then : TBTarg.BackColor = Color.White : Else : TBTarg.BackColor = Color.LightGray : End If
            'If CB2Point.Checked = False Then : TBLoad2.BackColor = Color.LightGray : Else : TBLoad2.BackColor = Color.White : End If
            '  If CBAcel.Checked = False AndAlso CB2Point.Checked = False Then
            ' TBLoad1.BackColor = Color.LightGray : Tbrpm1.BackColor = Color.LightGray : TBRPM2.BackColor = Color.LightGray
            ' Else
            '     TBLoad1.BackColor = Color.White : Tbrpm1.BackColor = Color.White : TBRPM2.BackColor = Color.White
            ' End If

            If Cals.Visible = True Then                         'Show the actual value during Calibrations setting
                j = Cals.ChaName.SelectedIndex + 1
                Cals.LCurr.Text = "VALUE: " + (analog(j) * Val(Cals.t4.Text) / anafact(j) - anaofs(j) + Val(Cals.t3.Text)).ToString(anadec(j))
                Cals.LSAECF.Text = Str(Int((1.176 * ((99 / SAEpress) * ((SAEtemp + 273) / 298) ^ 0.5) - 0.176) * 1000) / 10) + " %"
            End If

            If play = True Then                                                     'Play means we're in "Testing Mode"
                fgraf.L1.Text = "RPM: " + Str(analog(Xgraf - 1))                    'RPM Label at InstaGraf
                fgraf.L2.Text = Analab(Y1Graf - 1) + ": " + analog(Y1Graf - 1).ToString(anadec(Y1Graf - 1))     'Give 'e nice big numbers on the screen
                fgraf.L3.Text = Analab(Y2graf - 1) + ": " + analog(Y2graf - 1).ToString(anadec(Y2graf - 1))
                fgraf.L4.Text = Analab(Y3graf - 1) + ": " + analog(Y3graf - 1).ToString(anadec(Y3graf - 1))
                'fgraf.L5.Text = Analab(Y4Graf - 1) + ": " + analog(Y4Graf - 1).ToString(anadec(Y4Graf - 1))
            End If


            If (CB2Point.Checked = True Or CBAcel.Checked = True) And Not (GearRatio.Visible) Then       '===================  PULL  PULL  PULL  PULL (but not if GR disp is open)
                If fcomp.Visible = True Then                'Give RPM readout in GRAF
                    If analog(1) < 10 Then
                        fcomp.Lrpm.Hide()
                    Else
                        fcomp.Lrpm.Show()
                    End If

                    If analog(1) < rpm1 Then
                        fcomp.Lrpm.ForeColor = Color.DarkGreen
                        fcomp.Lrpm.Text = "PULL " + analog(1).ToString("F0")
                    Else
                        fcomp.Lrpm.ForeColor = Color.DarkRed
                        fcomp.Lrpm.Text = "WAIT " + analog(1).ToString("F0")
                    End If
                End If
                If (Speed1 < rpm1 * 0.5) Or (pullmode = 4 And Speed1 < rpm1 And Speed1 > Speed1o) Then 'This allows faster re-runs of pull without going down to RPM1/2
                    pullmode = 1                                                                        'Idle - Will immediatley switch to mode 2 if "rerun"
                End If
                If play = True And Speed1 > LowSpeedPostPull * 1.05 And pullmode = 4 Then pullmode = 1 'Re-pull if in Play mode and speed up more than 5%
                Select Case pullmode
                    Case 1                        'IDLE Nothing to do here except put load at 0
                        sendcmd = "M0"
                        Call SendPullLoad()
                        If Speed1 > rpm1 * 0.5 Then pullmode = 2
                    Case 2                                              'RAMP Load to START Load
                        If (Speed1 < rpm1 * 0.5) Then
                            pullmode = 1
                        Else                                            'Setting RAMP loading HERE: 0 at 50% RPM1 to Load1 at 75% RPM1 
                            Pload = 2 * load1 * (Speed1 - 0.5 * rpm1) / (0.5 * rpm1)
                            If Pload > load1 Then Pload = load1
                        End If
                        If Speed1 > rpm1 And fcomp.Visible And AutoCloseGraf = True Then fcomp.Hide() '  This Auto Closes the GRAF window if in Auto Graf Close Mode
                        If Speed1 > rpm1 And Not (fcomp.Visible) Then                                   'This prevents re-run of pull if GRAF is still open
                            pullmode = 3                    'Go WOT and "log" run
                            If PullDur < 5 Then PullDur = 15 'This is just a check for the next line to prevent /0
                            acel = (rpm2 - rpm1) / PullDur
                            fcomp.LData1.Text = "Current Pull Data"
                            timepull = time                 'Time of starting Pull
                            pullmaxrpm = 0                    'Reset Max Pull RPM (for stall calculation)
                            run = run + 1                       'Inc run number
                            ButRun.Text = "RUN: " + Str(run)
                            runsave = False
                            dataindex = 0
                            distnc = 0                              'Reset pull distance
                            dic = dic + 1 : If dic = 5 Then dic = 1 'Set Pull Data Index
                            If instagraf = True Or play = True Then
                                fgraf.Chart1.Series(2 * dic - 2).Points.Clear()                     'Clear previous graf points
                                fgraf.Chart1.Series(2 * dic - 1).Points.Clear()
                                fgraf.Chart2.Series(dic - 1).Points.Clear()
                                'fgraf.Chart2.Series(4).Points.Clear()
                                fgraf.Chart2.ResetAutoValues()
                                fgraf.Chart2.ChartAreas(0).AxisY.LabelStyle.Format = anadec(Y3graf - 1)
                                ' fgraf.Chart2.ChartAreas(0).AxisY2.LabelStyle.Format = anadec(Y4graf - 1)
                                If play = True Then                                             'Testing mode: Make BIG and OPAQUE
                                    fgraf.Opacity = 0.85 : fgraf.Size = New Size(INSTWidth, INSTHeight)  'Size(893 * 1.5, Screen.PrimaryScreen.WorkingArea.Height * 0.8)
                                    fgraf.L1.Parent = fgraf.Chart1
                                    fgraf.L2.Parent = fgraf.Chart1
                                    fgraf.L3.Parent = fgraf.Chart1
                                    fgraf.L4.Parent = fgraf.Chart1
                                    fgraf.L1.Show() : fgraf.L2.Show() : fgraf.L3.Show() : fgraf.L4.Show()
                                Else
                                    fgraf.Opacity = 0.6 : fgraf.Size = New Size(INSTWidth, INSTHeight) 'Size(893, 430)                   
                                    fgraf.L1.Hide() : fgraf.L2.Hide() : fgraf.L3.Hide() : fgraf.L4.Hide()
                                End If

                                If anamax(Y3graf - 1) > anamin(Y3graf - 1) Then
                                    fgraf.Chart2.ChartAreas(0).AxisY.Minimum = anamin(Y3graf - 1)
                                    fgraf.Chart2.ChartAreas(0).AxisY.Maximum = anamax(Y3graf - 1)
                                    If GManX = True Then
                                        fgraf.Chart1.ChartAreas(0).AxisX.Interval = GRx1
                                        fgraf.Chart2.ChartAreas(0).AxisX.Interval = GRx1
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
                                    fgraf.Chart2.Series(i - 1).BorderWidth = 1
                                    fgraf.Chart2.Series(i - 1).Color = Color.Gray
                                Next                                                                'Then turn latest to dark, and thick
                                fgraf.Chart1.Series(2 * dic - 2).BorderWidth = 3
                                fgraf.Chart1.Series(2 * dic - 2).Color = Color.DarkBlue
                                fgraf.Chart1.Series(2 * dic - 1).BorderWidth = 2
                                fgraf.Chart1.Series(2 * dic - 1).Color = Color.DarkRed
                                fgraf.Chart2.Series(dic - 1).BorderWidth = 3
                                fgraf.Chart2.Series(dic - 1).Color = Color.Black
                            End If
                        End If
                        sendcmd = "M" + Str(Int(100 * Pload) / 10).Replace(" ", "")
                        Call SendPullLoad()
                    Case 3                                                                          'MAIN PULL EVENT  WOT!
                        timelaps = time - timepull                                                  'Elapsed Time
                        'distnc = distnc + Speed1 * deltaT * Math.PI * diam / 60                    'Distance in Meters
                        dataindex = dataindex + 1
                        If dataindex > 2000 Then dataindex = 2000 '                                     Max data limit
                        If dataindex = 5 And G2Last = True And GraFirst = False Then                                     'Assign old graf1 to graf2
                            'Console.WriteLine("Copying Grafs...")
                            fcomp.Chart1.Series("Torque2").Points.Clear()
                            fcomp.Chart1.Series("Power2").Points.Clear()
                            fcomp.Chart2.Series("AFR2-1").Points.Clear()
                            fcomp.Chart2.Series("AFR2-2").Points.Clear()
                            fcomp.Chart3.Series("Torque2").Points.Clear()
                            fcomp.Chart3.Series("Power2").Points.Clear()
                            fcomp.Chart3.Series("AFR2-1").Points.Clear()
                            fcomp.Chart3.Series("AFR2-2").Points.Clear()

                            fcomp.Chart1.DataManipulator.CopySeriesValues("Torque1", "Torque2")
                            fcomp.Chart1.DataManipulator.CopySeriesValues("Power1", "Power2")
                            fcomp.Chart2.DataManipulator.CopySeriesValues("AFR1-1", "AFR2-1")
                            fcomp.Chart2.DataManipulator.CopySeriesValues("AFR1-2", "AFR2-2")
                            fcomp.Chart3.DataManipulator.CopySeriesValues("Torque1", "Torque2")
                            fcomp.Chart3.DataManipulator.CopySeriesValues("Power1", "Power2")
                            fcomp.Chart3.DataManipulator.CopySeriesValues("AFR1-1", "AFR2-1")
                            fcomp.Chart3.DataManipulator.CopySeriesValues("AFR1-2", "AFR2-2")  'Need to clear first, and prevent clearning at GRAF
                        End If
                        pulldata(dic, dataindex, 1) = timelaps
                        For i = 1 To nch + 2
                            pulldata(dic, dataindex, i + 1) = analog(i)
                        Next                                                                         'Save Data to Array!

                        If (instagraf = True Or play = True) AndAlso refresh = True Then
                            fgraf.Chart1.Series(2 * dic - 2).Points.AddXY(analog(Xgraf - 1), analog(Y1Graf - 1))    'Torq
                            fgraf.Chart1.Series(2 * dic - 1).Points.AddXY(analog(Xgraf - 1), analog(Y2graf - 1))        'Power
                            fgraf.Chart2.Series(dic - 1).Points.AddXY(analog(Xgraf - 1), analog(Y3graf - 1))    'Analog Add data to instant graph
                            If fgraf.Chart2.Series(4).Points.Count < 500 Then fgraf.Chart2.Series(4).Points.AddXY(analog(Xgraf - 1), GrAnaTarg)
                            'fgraf.Chart2.ResetAutoValues()
                        End If
                        If Speed1 > pullmaxrpm Then pullmaxrpm = Speed1
                        If Speed1 < rpm1 Then
                            pullmode = 2                                                                    'Stalled
                            If instagraf = True And play = False Then fgraf.Hide() 'Close Instagraf and go back to 2nd Mode
                        Else
                            If loadmode = 4 Then                                                            '2-Point Loading
                                Pload = load1 + (load2 - load1) * ((Speed1 - rpm1) / (rpm2 - rpm1))
                            Else
                                Pload = Pload + PullKp * timeav * ((Speed1 - Speed1o) / timeav - acel) / acel  'Acell MODE.  Kp was ~0.2, but can go up to ~1
                            End If
                            Math.Round(Pload, 1)                                                                 'Round to single place
                        End If
                        If (Speed1 > rpm2) Or (Speed1 < pullmaxrpm * 0.95) Or timelaps > 50 Then             'Trigger end of pull if over stop speed, slowing down, or >50 sec
                            LowSpeedPostPull = Speed1
                            pullmode = 4
                            If G2Last = True Then
                                For i = 1 To 2000                           'Copy over data for "clicks"
                                    For j = 1 To 20
                                        fcomp.FileDat(2, i, j) = fcomp.FileDat(1, i, j)
                                    Next
                                Next
                            End If
                            If play = False Then
                                INSTTop = fgraf.Top
                                INSTLeft = fgraf.Left
                                INSTWidth = fgraf.Width
                                INSTHeight = fgraf.Height
                                fgraf.Hide() 'Close Instant Graf
                            End If

                            points = dataindex
                            If autoload = True And timelaps > 2 Then                                        'Automatic Load Correction (if pull <5 sec, or >20 sec), <2 sec was a bad launch
                                If timelaps > 20 Then
                                    If loadmode = 4 Then                                                        '2-Point Took Too Long
                                        load1 = Int(10 * load1 * 20 / timelaps) / 10
                                        load2 = Int(10 * load2 * 20 / timelaps) / 10
                                        TBLoad1.Text = Str(load1)
                                        TBLoad2.Text = Str(load2)
                                    Else                                                                        'Acell mode  Took Too Long 
                                        load1 = Int(10 * load1 * 20 / timelaps) / 10

                                        If acel > (rpm2 - rpm1) / 10 Then acel = acel * timelaps / PullDur 'Set acell for 15 sec run
                                        'acel = Int(acel * 20 / timelaps)
                                        TBLoad1.Text = Str(load1)
                                        TBAcel.Text = PullDur.ToString("F1")
                                    End If
                                ElseIf timelaps < 8 Then
                                    If loadmode = 4 Then                                                        '2-Point  Went too fast
                                        load1 = Int(10 * load1 * 5 / timelaps) / 10
                                        load2 = Int(10 * load2 * 5 / timelaps) / 10
                                        TBLoad1.Text = Str(load1)
                                        TBLoad2.Text = Str(load2)
                                    Else                                                                          'Acell mode went too fact
                                        load1 = Int(10 * load1 * timelaps / 5) / 10
                                        If acel > (rpm2 - rpm1) / 10 Then acel = acel * timelaps / 10 'Set acell for 10 sec run
                                        TBLoad1.Text = Str(load1)
                                        TBAcel.Text = PullDur.ToString("F1")
                                    End If
                                End If
                            End If
                            For i = dataindex + 1 To 2000                                                        'Set any old data to zeros
                                For j = 1 To nch
                                    pulldata(dic, i, j) = 0
                                Next
                            Next
                            If autograf = True And play = False Then                                                 'Auto Open big graf
                                fcomp.Lrun1.Text = run
                                GrafSet.Label4.Text = "From Main"
                                fcomp.LID.Text = runfile
                                fcomp.Ltime.Text = Now
                                fcomp.Lsec.Text = timelaps.ToString("F1")
                                fcomp.ButFile.Text = "Click to load file" 'This text is refferred to in FComp, if you change it here, change it there
                                'If fcomp.first = True Then fcomp.ButFile.Text = "Click to load file" 'This text is refferred to in FComp, if you change it here, change it there
                                If fcomp.Visible = False Then fcomp.Show() 'If it isn't on allready, then turn it on
                            End If
                        End If
                        If Pload < 0 Then Pload = 0
                        If Pload > 1000 Then Pload = 1000
                        If Dynoload <> Pload Then
                            sendcmd = "M" + Str(Int(10 * Pload)).Replace(" ", "")
                            ann = sendcmd
                            Call SendPullLoad() 'Only send if it changed
                        End If

                    Case 4
                        If Speed1 < LowSpeedPostPull Then LowSpeedPostPull = Speed1 'Set up to see if increasing speed from lowest for re-pull
                        If Speed1 < rpm2 And runsave = False And play = False Then                   'Save File just as speed drops to STOP speed
                            runsave = True
                            Dim SaveFileBox As New SaveFileDialog
                            With SaveFileBox

                                runfile = runfile.Trim                              'Remove Leading/Trailing spaces

                                rundatafile = runfile + Str(run) + ".CSV"
                                .FileName = rundatafile
                                .DefaultExt = "CSV"
                                .Filter = "CSV|*.csv"
                                .Title = "Pull Data File"
                                'Dim result = .ShowDialog()
                                If autosave = False Then
                                    saveresult = .ShowDialog()
                                Else
                                    saveresult = DialogResult.OK            'Automatically Save Results
                                End If
                                If saveresult = DialogResult.OK Then
                                    rundatafile = .FileName
                                    Console.Write("RunFile WAS: >" + runfile + "<")
                                    '                                    If InStr(rundatafile, " ") > 3 Then runfile = Mid(rundatafile, 1, InStr(rundatafile, " ") - 1) 'Trim
                                    '                                    If InStr(runfile, ".") > 3 Then runfile = Mid(runfile, 1, InStr(runfile, ".") - 3) 'Trim
                                    rundatafile = rundatafile.Replace("  ", " ")
                                    If run < 100 Then
                                        If InStr(rundatafile, ".") > 3 Then runfile = Mid(rundatafile, 1, InStr(rundatafile, ".") - 3) 'Trim
                                    Else
                                        If InStr(rundatafile, ".") > 4 Then runfile = Mid(rundatafile, 1, InStr(rundatafile, ".") - 4) 'Trim
                                    End If
                                    Console.WriteLine("   IS: >" + runfile + "<")
                                    FileClose(4)
                                    FileOpen(4, rundatafile, OpenMode.Output)
                                    PrintLine(4, "Focus Applied Technologies Dyno Pull Version: " + ver + "    Opened: " + Now)
                                    Print(4, "Pull Data File: " + rundatafile + ",   Run #: " + Str(run) + ",    Points: " + Str(dataindex) + ",    Time: " + timelaps.ToString("N2"))
                                    If SAEcomp = True Then Print(4, ",   SAE Comp: " + SAEcf.ToString("F3"))
                                    PrintLine(4, " ")
                                    PrintLine(4, " ")
                                    Print(4, "Time")
                                    For i = 1 To nch
                                        Print(4, "," + Analab(i))
                                    Next
                                    'For i = 1 To 20 : If OBDchon(i) = True Then : Print(4, "," + Analab(i)) : End If : Next 'OBD Labels
                                    PrintLine(4, " ")
                                    For i = 1 To dataindex
                                        For j = 1 To nch + 1                                              'Max # of Channels plus 1 (time is added as first)
                                            Print(4, Str(pulldata(dic, i, j)) + ", ")
                                        Next
                                        'For j = 1 To 20
                                        '    If OBDchon(j) = True Then Print(4, Str(analog(j + 15)) + ", ")
                                        'Next
                                        PrintLine(4, " ")
                                    Next
                                    PrintLine(4, "END")                                                   'End Of Line
                                    FileClose(4)
                                End If
                                'Purge Serialcoms
                                COMport.DiscardInBuffer()
                            End With
                        End If
                        If brek2p = True Then
                            Pload = load2                                               'Break using 2-point Load (Stop % Load)
                        Else
                            Pload = brekload                                            'Break using Breaking Cal Load
                        End If
                        If brekramp = True Then
                            If brekstop = True Then                                     'Ramps down from RPM2 to RPM1
                                If rpm2 <> rpm1 Then Pload = Pload * (Speed1 - rpm1) / (rpm2 - rpm1)
                                If Pload < 0 Then Pload = 0
                            Else                                                        'Ramps from RPM2 down to zero speed
                                If rpm2 <> 0 Then Pload = Pload * Speed1 / rpm2
                            End If
                        End If
                        'Breaking Load
                        sendcmd = "M" + Str(Int(10 * Pload)).Replace(" ", "")
                        Call SendPullLoad()
                    Case 5
                        'Console.WriteLine("Mode 5, SPEED1: " + Str(Speed1) + "  RPM1: " + Str(rpm1))
                        If Speed1 < rpm1 Then pullmode = 1
                End Select
                If pullmode = 3 Then
                    TBMode.Text = pullcom(pullmode, lang) + " " + (Int(timelaps * 10) / 10).ToString("N1") + "s  "
                Else
                    TBMode.Text = pullcom(pullmode, lang)
                End If
            Else
                fcomp.Lrpm.Hide()
                If loadon = True Then
                    Select Case True                                'Set COms box text
                        Case CBMan.Checked
                            TBMode.Text = modes(1, lang)
                        Case CBRPM.Checked
                            TBMode.Text = modes(2, lang)
                        Case CBRoad.Checked
                            TBMode.Text = modes(3, lang)
                        Case brekon
                            TBMode.Text = rembreks(lang)
                        Case (loadon = False)
                            TBMode.Text = remloads(lang)
                        Case Else
                            TBMode.Text = modes(4, lang)            'Put in Com Mode
                    End Select
                End If
            End If

            If SAEauto Then                                        'Grab data if in AUTO mode from last 3 positions as SAE data
                'DO SAE COMP
            End If

            If time < 1 Then torqued = TorqueRawd 'Avoid "infinite" acellerations on power up of spinning system 
            torqueavd = torqueavd * torqueAF + torqued * (1 - torqueAF)         'Torque Averaging Calculation
            torqued = torqueavd
            power = speed1d * torqued * 2 * Math.PI / 60                        'Power

            'time, Int(Speed1), speed2, TorqueRaw, torque, Int(10 * power) / 10, Dynoload, mode, setpt, throttle, alarm, analog(1), analog(2), analog(3), analog(4), FuelWt, ann
            HAspeed1 = HAspeed1 * 0.9 + 0.1 * Speed1            'Heavy Average of parameters for "SAVE" datafile
            HASpeed2 = HASpeed2 * 0.9 + 0.1 * speed2
            HATorqueRaw = HATorqueRaw * 0.9 + 0.1 * TorqueRaw
            HATorque = HATorque * 0.9 + 0.1 * torque
            HApower = HApower * 0.9 + 0.1 * power
            HAdynoload = HAdynoload * 0.9 + 0.1 * Dynoload
            HAthrottle = HAthrottle * 0.9 + 0.1 * throttle
            HAA1 = HAA1 * 0.9 + 0.1 * analog(1)
            HAA2 = HAA2 * 0.9 + 0.1 * analog(2)
            HAA3 = HAA3 * 0.9 + 0.1 * analog(3)
            HAA4 = HAA4 * 0.9 + 0.1 * analog(4)



            'PASS VALUES TO TUNE BUDDY
            If Ftune.Visible = True Then Call Ftune.main()

            TBPowMax.Text = powmax.ToString("n2")
            LTagline.Text = tagline


            TBD1.Text = analog(d1CH).ToString(anadec(d1CH))         'Display numbers in Display Boxes
            TBD2.Text = analog(d2CH).ToString(anadec(d2CH))
            TBD3.Text = analog(d3CH).ToString(anadec(d3CH))
            TBD4.Text = analog(d4CH).ToString(anadec(d4CH))
            TBD5.Text = analog(d5ch).ToString(anadec(d5ch))
            TBD6.Text = analog(d6ch).ToString(anadec(d6ch))
            TBG1.Text = analog(g1CH).ToString(anadec(g1CH))
            TBG2.Text = analog(g2CH).ToString(anadec(g2CH))
            TBG3.Text = analog(g3CH).ToString(anadec(g3CH))

            TimeBox.Text = time.ToString("n1") + " s"

            If Logging.Checked = True Then                                  'NEED TO ADD OBD
                savecnt = savecnt + 1
                If savecnt >= saveint Then
                    Write(2, time)
                    For i = 1 To nch : Write(2, analog(i)) : Next
                    'For i = 1 To 20
                    '    If OBDchon(i) = True Then Write(2, analog(i + 15))
                    'Next
                    Write(2, run)
                    WriteLine(2, ann) ' Save to file
                    ann = ""
                End If
                savecnt = 0
            End If

            'Graph
            throttleo = throttle
            Speed1o = Speed1
            speedrawo = speedraw
            speedrawod = speedrawd
            throttleo = throttle
        Else
            If RXtxt <> "" Then TextBox1.Text = "Invalid Serial Input"
            'TextBox1.Text = ">>>" + RXtxt + "<<<<"
        End If
        'Console.WriteLine("Display Loop.")
        If ComOK = False Then : ComBut.BackColor = Color.Red : Else : ComBut.BackColor = Color.LightGreen : End If

        If GearRatio.Visible = True Then
            GearRatio.speed1 = Speed1
            GearRatio.speedC2 = speedc2
            GearRatio.speedco = analog(19)                'OBD speed
            Call GearRatio.main() 'Gotta run the loop
        End If

        BRatio.Text = "RATIO " + gr.ToString("F2")
        If refresh = True Then
            Gage1.Invalidate(True)              'Redraw
            Gage2.Invalidate(True)              'Redraw
            Gage3.Invalidate(True)              'Redraw
        End If
        If loadon = True Then : ButLoad.BackColor = butcol1 : Else : ButLoad.BackColor = butcol2 : End If
        If OBDon Then
            ButOBD.BackColor = butcol1
        Else
            ButOBD.BackColor = Color.Transparent
        End If
        Call remoteCheck()                                  'Service Remote Control
        Call autofrict()                                    'Calibrate Friction (if requested)
        Call breaking()                                     'Perform breaking following schedule


        refresh = False
    End Sub
    Private Sub openOBD() Handles FOBD.OBDOpen
        If OBDon = True Then
            Console.WriteLine("Close OBD")
            OBDon = False
            FOBD.Hide()
            If OBDport.IsOpen Then OBDport.Close()
            FOBD.Lverified.Text = "OBD COMS CLOSED"
        Else
            If OBDport.IsOpen Then OBDport.Close()
            Console.WriteLine("Trying to OPEN OBD")
            OBDportnum = Val(FOBD.TBport.Text)                  'Load latest Com Port #'s
            OBDbr = Val(FOBD.TBBaud.Text)
            OBDinit = FOBD.TBInit.Text
            comstr = "COM" + LTrim(Str(OBDportnum))
            OBDport.PortName = comstr                           'OPen Dyno Controller Com Port
            If OBDbr = 0 Then OBDbr = 38400
            OBDport.BaudRate = OBDbr
            OBDport.ReadBufferSize = 512                         'Normally 128
            OBDport.Handshake = Handshake.None
            OBDport.RtsEnable = True
            OBDport.DtrEnable = True
            OBDport.ReadTimeout = 500
            OBDport.DtrEnable = True
            OBDport.RtsEnable = True
            serports = ""
            Try
                OBDport.Open()
            Catch ex As Exception                       'This only allows 1 retry!!!

                For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                    serports = serports + sp + ", "
                Next

                Dim searcher As New ManagementObjectSearcher("root\cimv2", "SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0") ' PnPEntity Or SreialPort
                For Each queryObj As ManagementObject In searcher.Get()
                    tempstr = queryObj("Name")
                    '  Console.WriteLine(tempstr)
                    If tempstr.Contains("COM") Then
                        ' Console.WriteLine(tempstr)
                        If tempstr.Contains("USB Serial Port") Then
                            Dim nonNumericCharacters As New System.Text.RegularExpressions.Regex("[^0-9]")
                            tempstr = nonNumericCharacters.Replace(tempstr, String.Empty)
                            compt = Val(tempstr)                                                            'If there are multimple units it'll just 
                            'Console.WriteLine("Port: " + tempstr)                                           'Assign the last one as the default
                            tempstr2 = tempstr2 + tempstr + ", "                                            'But will "recomend" and Prolific cable ports
                        End If
                    End If
                Next

                cpstr = InputBox("Available Ports: (0 = offline), " + serports + Chr(13) + "Recomended: " + tempstr2, "Enter USB Com Port #", 1, 5, 5)
                If cpstr <> "" Then
                    OBDportnum = Val(cpstr)
                    comstr = "COM" + LTrim(Str(OBDportnum))
                    '     If PortIsAvailable(comstr) Then
                    OBDport.PortName = comstr    'OPen Dyno Controller Com Port: NOTE Can use USB com ports just like internal ones!
                    cpstr = InputBox("Baud Rate: " + Str(OBDbr) + Chr(13) + "Typically 9600 or 38400" + Chr(13) + "Enter OBD Scanner Com Baud Rate ", , Str(OBDbr))
                    If cpstr <> "" Then OBDbr = Val(cpstr)
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
                If OBDport.IsOpen Then
                    OBDport.WriteLine("ATZ")            'Restt ALL
                    Sleep(200)
                    OBDport.WriteLine("ATSP0")           'Send Protocall 0 
                    Sleep(200)
                    OBDport.WriteLine("ATE0")           'NO Echo
                    Sleep(200)
                    OBDport.WriteLine(OBDinit)           'Whatever other command to send
                    Sleep(20)
                End If
                OBDon = True
                FOBD.Lverified.Text = "OBD COMS LIVE"
                FOBD.Bopen.Text = "CLOSE PORT"
            Else
                OBDon = False                           'Cant Open it
            End If

            'FOBD.Show()
        End If
    End Sub

    Private Sub obdprocess()
        Dim IndCMD, IndDAT As Integer
        Dim OBDrecs, OBDHex(20), obdsends As String
        Dim obdat(20) As Single
        'Console.Write("OBD OPEN - ")
        If OBDport.IsOpen Then
            If OBDport.BytesToRead >= 0 Then
                OBDrecs = OBDport.ReadExisting
                'OBDrecs = "00E 0: 41 05 83 04 7D 0C " + Chr(13) + "1: 0E 96 10 01 A2 15 98 " + Chr(13) + "2: FF 00 00 00 00 00 00>"
                OBDrecs = OBDrecs.Replace(vbCr, "").Replace(vbLf, "")
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
                FOBD.Lverified.Text = "OBD COMS RETURN:" + Chr(13) + OBDrecs
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
                            obdat(IndCMD) = CInt("&H" & OBDHex(IndDAT))         'This is for all the 1-byte response data
                            Select Case OBDHex(IndCMD)
                                Case "04"
                                    analog(16) = obdat(IndCMD) / 2.55           'Engine Load %
                                    Console.WriteLine("Got Load")
                                Case "05"
                                    analog(17) = obdat(IndCMD) - 40            'CLT
                                    Console.WriteLine("Got CLT")
                                Case "0B"
                                    Console.WriteLine("Got MAP")
                                    analog(18) = obdat(IndCMD)                 'MAP
                                Case "0C"
                                    obdat(IndCMD) = obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))
                                    Console.WriteLine("Got RPM")
                                    analog(19) = obdat(IndCMD) / 4             'RPM (2)
                                    IndDAT += 1                                 'Go to next Data point
                                    IndCMD += 1
                                Case "0D"
                                    Console.WriteLine("Got kph")
                                    analog(20) = obdat(IndCMD)                 'kPh
                                Case "0E"
                                    Console.WriteLine("Got Advan")
                                    analog(21) = obdat(IndCMD) / 2 - 64        'Timing Adv
                                Case "0F"
                                    Console.WriteLine("Got IAT")
                                    analog(22) = obdat(IndCMD) - 40            'IAT
                                Case "10"
                                    Console.WriteLine("Got MAF")
                                    analog(23) = (obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))) / 100           'MAF (2) 
                                    IndDAT += 1                                 'Go to next Data point
                                    IndCMD += 1
                                Case "11"
                                    Console.WriteLine("Got TTL")
                                    analog(24) = obdat(IndCMD) / 2.55           'TTL %
                                Case "14"
                                    Console.WriteLine("Got O2-1")
                                    obdat(IndCMD) = obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))
                                    analog(25) = (Int(obdat(IndCMD) / 256)) / 200    'O2-1 (2 bytes, only use MSB)

                                    IndDAT += 1                                 'Go to next Data point
                                    IndCMD += 1
                                Case "15"
                                    Console.WriteLine("Got O2-3")
                                    obdat(IndCMD) = obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))
                                    analog(26) = (Int(obdat(IndCMD) / 256)) / 200    ''O2-1 (2 bytes, only use MSB)

                                    IndDAT += 1                                 'Go to next Data point
                                    IndCMD += 1
                                Case "44"
                                    obdat(IndCMD) = obdat(IndCMD) * 256 + CInt("&H" & OBDHex(IndDAT + 1))
                                    analog(27) = 0.079 * obdat(IndCMD)       'Lambda 

                                    IndDAT += 1                                 'Go to next Data point
                                    IndCMD += 1
                                Case Else
                                    Console.WriteLine("Blown Comand: " + OBDHex(IndCMD))
                            End Select
                            IndDAT = IndDAT + 2                                 'Go to next Data point
                            IndCMD = IndCMD + 2
                        Loop While IndCMD < OBDHex.Length

                        ' Console.WriteLine(" ")
                    End If
                Catch ex As Exception
                    Debug.WriteLine("OBD Return Code Translation Error: " + ex.ToString)
                End Try
            End If
            obdsends = "01"                                                     'Give Current Data Comand
            For i = 1 To 12
                If OBDchon(i) Then
                    obdsends = obdsends + OBDcmd(i) 'Add code for all requested data
                End If
            Next
            ' Console.Write("Comands: " + OBDSends)
            If Len(obdsends) > 2 Then    'Only send requests on schedule
                OBDport.Write(obdsends + Chr(13))                                     'Send request
                FOBD.Lout.Text = "OUTGOING: " + obdsends                              ' Console.WriteLine("OBD SEND: " + obdsends)
                OBDLast = time
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
            If OBDport.BytesToRead > 0 Then OBDRecs = OBDport.ReadExisting 'Clear the pipeline
            OBDSends = "0101"                                                  'Got how many error codes?
            OBDport.Write(OBDSends + Chr(13))                                'Send request
            'FOBD.TBOut.Text = OBDSends                                          'Show send $ to screen
            Sleep(OBDtime * 1000)

            Dim IndCMD, IndDAT, ThisCMD As Integer
            If OBDport.BytesToRead >= 0 Then

                OBDRecs = OBDport.ReadExisting
                'OBDRecs = "41 01 83 07 05 01"                'FOR TEST ONLY !!! DELETE 3-error code
                'OBDRecs = "41 01 82 07 05 01"                'FOR TEST ONLY !!! DELETE 2-error code
                'OBDRecs = "41 01 MIL*128 + # of codes" eg. 129 is MIL on and 1 error, 03 ie 3 errors but no MIL
                errors = OBDRecs
                OBDRecs = OBDRecs.Replace(vbCr, "").Replace(vbLf, "")
                'FOBD.TBIn.Text = OBDRecs
                FOBD.Lverified.Text = obdrecs
                If InStr(OBDRecs, "41") >= 1 Then OBDRecs = Mid(OBDRecs, InStr(OBDRecs, "41") + 5, 3) ' Response is 41 01 MIL... so cut the 41 01 out

                OBDRecs += "  "
                OBDRecs = OBDRecs.Replace(" ", "")
                Nerr = Val("&H" & OBDRecs)                                            'Convert to Decimal from Hex
                If Nerr > 127 Then
                    MIL = 1
                    Nerr = Nerr - 128
                End If

                'FOBD.TBIn.Text = "MIL:" + Str(MIL) + " Errors:" + Str(Nerr)
                FOBD.Lverified.Text = "MIL:" + Str(MIL) + " Errors:" + Str(Nerr)
                'Send return string to screen
                If Nerr <> 0 Then
                    OBDSends = "03"                                                 'Request Error Codes
                    OBDport.Write(OBDSends + Chr(13))                            'Send request
                    'FOBD.TBOut.Text = OBDSends                                      'Show send $ to screen
                    Sleep(1000)                                                     '500ms was too short
                    If OBDport.BytesToRead > 0 Then

                        obdrecs = OBDport.ReadExisting

                        'OBDRecs = "43 Xe E1 Oy E2" where X is 0-3=P, 4-7=C, 8-11=B, 12-15=U and error #1 is eE1, eE2 is error #2...

                        ' OBDRecs = "0: 43 03 05 07 05 01 02 " + vbCr + "1: 17 00 00 00 00 00 "                'FOR TEST ONLY !!! DELETE 3-error code
                        'OBDRecs = "43 05 07 05 01"                'FOR TEST ONLY !!! DELETE 2-error code
                        OBDRecs = OBDRecs.Replace(vbCr, "").Replace(vbLf, "")
                        If InStr(OBDRecs, ":") <> 0 Then
                            MultiLine = True
                            OBDRecs += "  "                                                 'Pad with spaces incase ":" is last character
                            OBDRecs = OBDRecs.Substring(InStr(OBDRecs, ":") + 1)            'Take string from 1 position beyond the 1st ":"
                            OBDRecs = OBDRecs.Replace("0: ", "").Replace("1: ", " ").Replace("2: ", " ").Replace("3: ", " ")
                            'Remove 0:, 1: 
                        End If
                        OBDRecs = OBDRecs.Replace("  ", " ")
                        ' Console.WriteLine("BEFORE trim: " + OBDRecs)
                        FOBD.Lverified.Text = "Err Hex: " + obdrecs
                        If InStr(OBDRecs, "43") > 0 Then OBDRecs = Mid(OBDRecs, InStr(OBDRecs, "43") + 3)
                        ' Console.WriteLine("HALF trim: " + OBDRecs)
                        If MultiLine Then OBDRecs = Mid(OBDRecs, 3).TrimStart 'Skip the next byte as it is the number of lines ( I think)
                        ' Console.WriteLine("FULL trim: " + OBDRecs)
                        ' FOBD.TBIn.Text = OBDRecs

                        ReDim OBDHex(20)                                                'The freaking "split" command reduces the size of the array when used?!
                        For ik = 1 To 20 : OBDHex(ik) = "" : Next
                        OBDHex = OBDRecs.Split(" ")
                        errors = ""
                        For ik = 0 To Nerr * 2 - 1 Step 2
                            Console.Write("Error #" + Str(ik) + " (" + OBDHex(ik) + "): " + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1) + " =>")
                            Select Case Mid(OBDHex(ik), 1, 1)
                                Case "1", "2", "3", "0"
                                    errors = errors + " P" + Mid(OBDHex(ik), 1, 1) + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1)
                                Case "4", "5", "6", "7"
                                    errors = errors + " C" + Chr(Asc(Mid(OBDHex(ik), 1, 1)) - 4) + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1)
                                Case "8", "9"
                                    errors = errors + " B" + Chr(Asc(Mid(OBDHex(ik), 1, 1)) - 8) + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1)
                                Case "A"
                                    errors = errors + " B3" + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1)
                                Case "B"
                                    errors = errors + " B4" + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1)
                                Case "C"
                                    errors = errors + " U1" + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1)
                                Case "D"
                                    errors = errors + " U2" + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1)
                                Case "E"
                                    errors = errors + " U3" + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1)
                                Case "F"
                                    errors = errors + " U4" + Mid(OBDHex(ik), 2, 1) + OBDHex(ik + 1)
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
            If COMport.IsOpen Then
                'COMport.Write(sendcmd) 'Done "slowly" to avoid "cloging up" the coms line
                COMport.WriteLine(sendcmd) 'Done "slowly" to avoid "cloging up" the coms line

            End If
            tlastload = time
            ' Console.WriteLine("Pull Sending " + sendcmd)
        End If
    End Sub
    Private Sub ClseFOBD() Handles FOBD.OBDSav            'Set up OBD numbers WITHOUT closing the OBD window
        OBDportnum = Val(FOBD.TBport.Text)
        OBDbr = Val(FOBD.TBBaud.Text)
        OBDinit = FOBD.TBInit.Text
        For i = 1 To 12
            If OBDCB(i).Checked = True Then
                OBDchon(i) = True
            Else
                OBDchon(i) = False
            End If
        Next
    End Sub

    Private Sub breaking()
        If brekon = True Then                                               'BREK   BREK    BREK   BREK   BREK    BREK   BREK   BREK    BREK   
            If brek2p = True Then
                Pload = load2                                               'Break using 2-point Load (Stop % Load)
            Else
                Pload = brekload                                            'Break using Breaking Cal Load
            End If
            If brekramp = True Then
                If brekstop = True Then                                     'Ramps down from RPM2 to RPM1
                    If rpm2 <> rpm1 Then Pload = Pload * (Speed1 - rpm1) / (rpm2 - rpm1)
                    If Pload < 0 Then Pload = 0
                Else                                                        'Ramps from RPM2 down to zero speed
                    If rpm2 <> 0 Then Pload = Pload * Speed1 / rpm2
                End If
            Else
                If brekstop = True Then                                     'Ramps down from RPM2 to RPM1
                    'Console.WriteLine("Breaking stop at " + Str(rpm1))
                    If Speed1 < rpm1 Then Pload = 0
                End If
            End If
            sendcmd = "M" + Str(Int(10 * Pload)).Replace(" ", "") 'Only send when it changes, otherwise we overload
            Sleep(20)
            If COMport.IsOpen And Pload <> Dynoload Then COMport.Write(sendcmd)
            lastload = Pload
            If Speed1 < 1 Or Pload < 0.1 Then
                Call brekrelease()
            End If
        Else
            lastload = 0
        End If
    End Sub
    Private Sub remoteCheck()

        If remote AndAlso RemV > 0.2 AndAlso Math.Abs(RemV - lastrem) / RemV < 0.1 AndAlso remoff = True Then        'Remote is pushed and same +/-5% as last
            Select Case True                                                            'Remote is forward biased diodes so ~0.7V button
                Case (2.1 < RemV And RemV < 2.7)        'BUTTON #4 "DOWN"/"BACK"
                    If fcomp.Visible Then
                        fcomp.xmark = fcomp.xmark - 0.005
                        Call fcomp.Chrt1Stats(Nothing)
                    Else
                        Select Case loadmode
                            Case 1, 2, 3                      'MAN, RPM or Road
                                Call ButDN_Click(Nothing, Nothing)
                            Case 4, 5                     '2-point
                                TBLoad1.Text = Val(TBLoad1.Text) - 0.5
                                Call TBload1_TextChanged(Nothing, Nothing)
                        End Select
                    End If
                    remoff = False
                Case (1.52 < RemV And RemV < 2.1)        'BUTTON #3 "UP"/"NEXT"
                    If fcomp.Visible Then
                        fcomp.xmark = fcomp.xmark + 0.005
                        Call fcomp.Chrt1Stats(Nothing)
                    Else
                        Select Case loadmode
                            Case 1, 2, 3                      'MAN, RPM or Road
                                Call ButUP_Click(Nothing, Nothing)
                            Case 4, 5                     '2-point
                                TBLoad1.Text = Val(TBLoad1.Text) + 0.5
                                Call TBload1_TextChanged(Nothing, Nothing)
                        End Select
                        remoff = False
                        'If remoff = True Then
                        '    If autosave = True Then
                        '        autosave = False : BUTAS.BackColor = butcol0
                        '    Else
                        '        autosave = True : BUTAS.BackColor = butcol1
                        '        saveresult = DialogResult.OK            'Save Results (wonn't work as Save fiel dialog box "hold open" thread, prevening incomming pings).
                        '    End If
                        '    remoff = False                              'Debounce
                        'End If
                    End If
                Case (0.95 < RemV And RemV < 1.5)                        'BUTTON #2  Load Button
                    If fcomp.Visible Then
                        Call fcomp.exitgraf()
                    Else
                        If remoff = True Then
                            Console.Write("Click Load: ")
                            If loadon = False Then                                      'load ON        6666
                                loadon = True : loadmode = LastLMode : TBMode.Text = prevmode
                                Console.WriteLine("ON Mode:" + Str(loadmode))
                                Select Case loadmode        'Turn ON load before clicking Check Boxes as they check for this before sending Set Point
                                    Case 1 : CBMan.Checked = True : Call CBManD(Nothing, Nothing)                      'Turning these on is supposed to re-send the set points
                                    Case 2 : CBRPM.Checked = True : Call CBRPMD(Nothing, Nothing)
                                    Case 3 : CBRoad.Checked = True : Call CBRoadD(Nothing, Nothing)
                                    Case 4
                                        pullmode = 5 : CB2Point.Checked = True          'Set for "spin down"
                                        Console.Write("mode 5 ")
                                    Case 5
                                        Console.Write("mode 5 ")
                                        pullmode = 5 : CBAcel.Checked = True            'Set for "spin down"
                                End Select

                            Else                                            'LOAD OFF (bypassed)
                                loadon = False : LastLMode = loadmode : prevmode = TBMode.Text
                                CBMan.Checked = False : CBRPM.Checked = False : CBRoad.Checked = False
                                CB2Point.Checked = False : CBAcel.Checked = False : sendcmd = "M0"
                                Sleep(20)                                               'Gotta have this pause to prevent "clashing" with other outogint comands
                                If COMport.IsOpen Then COMport.WriteLine(sendcmd)
                                TBMode.Text = remloads(lang)
                                Console.WriteLine("OFF  I said M0!")
                            End If
                        End If

                    End If
                    remoff = False                                       'Debounce
                Case (0.3 < RemV And RemV < 0.94)                          'BUTTON #1    Brek Button
                    If remoff = True Then
                        Console.Write("Click Brek: ")
                        If brekon = False Then
                            TBMode.Text = rembreks(lang) : Call brek()
                            Console.WriteLine("ON ")
                        Else
                            Call brekrelease()
                            Console.WriteLine("OFF ")
                        End If
                    End If
                    remoff = False                                      'Diss allow a "double tap"
                Case RemV > 4.8                                        'Released Button
                    remoff = True
            End Select
        End If                                        'END REMOTE ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        If RemV > 4.8 Then remoff = True
    End Sub
    Private Sub autofrict()

        If FACon = True Then                                                '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$  Auto Friction Calc
            tagline = "FRICT. AUTOCAL DYNO RPM = " + speed1d.ToString("N0")
            FAC = FAC + 1
            If FAC = 1 Then
                AFRPM = analog(1)       'Start RPM
                AFAV1 = 0 : afav2 = 0 : afav3 = 0                'Reset # of averages
                x1 = 0 : x2 = 0 : x3 = 0
                y1 = 0 : y2 = 0 : y3 = 0
            End If
            Select Case analog(1)
                Case Is < 0.15 * AFRPM
                    If afav3 < 3 Then
                        afav3 = afav3 + 1                       'Average 3 readings
                        x3 = x3 + analog(1)                     'Add up speeds
                        y3 = y3 - analog(5)                     'Add up torques
                    End If
                Case Is < 0.5 * AFRPM
                    If afav2 < 3 Then
                        afav2 = afav2 + 1
                        x2 = x2 + analog(1)                     'Add up speeds
                        y2 = y2 - analog(5)                     'Add up torques
                    End If
                Case Is < 0.95 * AFRPM
                    If AFAV1 < 3 Then
                        AFAV1 = AFAV1 + 1
                        x1 = x1 + analog(1)                     'Add up speeds
                        y1 = y1 - analog(5)                     'Add up torques
                    End If
            End Select
            If analog(1) = 0 Then afav3 = 3 'Worst case, set to done is speed = 0
            If afav3 = 3 Then                                    'Done when down to 10% of initial speed  
                tagline = tempstr2
                LTagline.Text = tempstr2
                FACon = False                                    'Shut off AutoCal
                Fmin = 0
                x1 = x1 / 3 : x2 = x2 / 3 : x3 = x3 / 3          'Make Averages
                y1 = y1 / 3 : y2 = y2 / 3 : y3 = y3 / 3

                Console.WriteLine("RPMs: " + Str(x1) + " " + Str(x2) + " " + Str(x3))
                Console.WriteLine("Torques " + Str(y1) + " " + Str(y2) + " " + Str(y3))
                frictoff = (x3 * (x2 * (x2 - x3) * y1 + x1 * (x3 - x1) * y2) + x1 * (x1 - x2) * x2 * y3) / ((x1 - x2) * (x1 - x3) * (x2 - x3))
                frictfact = (x3 * x3 * (y1 - y2) + x1 * x1 * (y2 - y3) + x2 * x2 * (y3 - y1)) / ((x1 - x2) * (x1 - x3) * (x2 - x3))
                FrictQuad = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / ((x1 - x2) * (x1 - x3) * (x2 - x3))
                If FrictQuad < 0 Then
                    FrictQuad = 0                       'Generally never going to have a negative quadratic, so if got one, stuff lin approx
                    frictfact = (y1 - y3) / (x1 - x3)
                    frictoff = y3 - x3 * frictfact
                End If
                torqueAF = TrqAvgT
                speedaf = Speed1AvgT
                frictoff = Math.Truncate(frictoff * 1000) / 1000
                frictfact = Int(frictfact * 1000000000) / 1000000000
                FrictQuad = Math.Truncate(FrictQuad * 10000000000.0) / 10000000000.0
                inerteng = IETemp                                                       'Reset Inertie Engine to previous value
                temp = MessageBox.Show("Previous: Offset = " + frictofft.ToString("n3") + "   Factor = " + frictfactt.ToString("n5") + "   QUAD = " + frictquadt.ToString("E3") + Chr(13) + "Current:  Offset = " + frictoff.ToString("n3") + "   Factor = " + frictfact.ToString("n5") + "   QUAD = " + FrictQuad.ToString("E3") + Chr(13) + "     OK to Accept         Cancel to Reject", "Friction Auto Calibration Results", MessageBoxButtons.OKCancel)
                If temp = 2 Then
                    frictoff = frictofft
                    frictfact = frictfactt
                    FrictQuad = frictquadt
                End If
            End If
        End If                                                          '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ Auto Friction Cal

    End Sub

    Private Sub loadcals()
        'Read Dynocal File========================================================================
        Dim lines As Integer        'Number of lines read (more or less)
        Dim arg, orig As String           'Arguments
        ' MsgBox("Loading Calibrations from: " + CalFile)
        lines = 0
        chan = 0
        sparecnt = 0

        If System.IO.File.Exists(CalFile) = True Then
            Try
                Using FileRead As New System.IO.StreamReader(CalFile)
                    dummy = FileRead.ReadLine()
                    lines = 1
                    Do
                        If FileRead.Peek = -1 Then                                  'This would mean that there's no more lines in the file, so were at the END
                            s = "END"
                        Else
                            s = FileRead.ReadLine()
                            orig = s
                        End If
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
                                    ver = Val(arg)
                                Case "tagline"
                                    tagline = arg.TrimEnd(",")
                                Case "type"
                                    dynotype = arg.TrimEnd(",")
                                Case "lastcal"
                                    lastcal = arg.TrimEnd(",")
                                Case "rollerdiam"
                                    diam = Val(arg)
                                Case "dynomaxrpm"
                                    RPMDynoMax = Val(arg)
                                Case "dynomaxtrq"
                                    TorqDynoMax = Val(arg)
                                Case "inertia"
                                    inert = Val(arg)
                                Case "frictofs"
                                    frictoff = Val(arg)
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
                                    gr = Val(arg)
                                Case "gage1"
                                    g1CH = Val(arg)
                                Case "gage2"
                                    g2CH = Val(arg)
                                Case "gage3"
                                    g3CH = Val(arg)
                                Case "disp1" : d1CH = Val(arg)
                                Case "disp2" : d2CH = Val(arg)
                                Case "disp3" : d3CH = Val(arg)
                                Case "disp4" : d4CH = Val(arg)
                                Case "disp5" : d5ch = Val(arg)
                                Case "disp6" : d6ch = Val(arg)
                                Case "logint"
                                    saveint = Val(arg)
                                Case "run"
                                    run = Val(arg)
                                Case "screenmag"
                                    MAG = Val(arg)
                                Case "metric"
                                    If arg = "False" Or arg = "false" Then powmaxu = False 'Default was TRUE
                                Case "inerteng"
                                    inerteng = Val(arg)
                                Case "rpmload"
                                    rpmload = Val(arg)
                                Case "manload"
                                    manload = Val(arg)
                                Case "rpm1"
                                    rpm1 = Val(arg)
                                Case "rpm2"
                                    rpm2 = Val(arg)
                                Case "load1"
                                    load1 = Val(arg)
                                Case "load2"
                                    load2 = Val(arg)
                                Case "acelfact"
                                    acel = Val(arg)
                                Case "loadmode"
                                    loadmode = Val(arg)
                                Case "pullanach" : pgrafanach = Val(arg)
                                Case "graftop" : GrafTop = Val(arg)
                                Case "grafleft" : GrafLeft = Val(arg)
                                Case "grafwidth" : GrafWidth = Val(arg)
                                Case "grafheight" : GrafHeight = Val(arg)
                                Case "insttop" : INSTTop = Val(arg)
                                Case "instleft" : INSTLeft = Val(arg)
                                Case "instwidth" : INSTWidth = Val(arg)
                                Case "instheight" : INSTHeight = Val(arg)
                                Case "ftop"
                                    FTop = Val(arg)
                                Case "fleft"
                                    FLeft = Val(arg)
                                Case "fwidth"
                                    FWidth = Val(arg)
                                Case "fheight"
                                    FHeight = Val(arg)
                                Case "fmax"
                                    If arg = "True" Or arg = "False" Then Fmax = arg
                                Case "datapath"
                                    DataPath = arg.TrimEnd(",")
                                Case "pullrpm2"                     'This means are we using RPM2 as the pull rpm
                                    If arg = "True" Or arg = "False" Then Peng2 = arg
                                Case "pullduration"
                                    PullDur = Val(arg)
                                Case "pullkp"
                                    PullKp = Val(arg)
                                Case "runfile"
                                    runfile = arg.TrimEnd(",")
                                Case "tunecfg"
                                    TuneCal = arg.TrimEnd(",")
                                Case "xgraf"
                                    Xgraf = Val(arg)
                                Case "y1graf"
                                    Y1Graf = Val(arg)
                                Case "y2graf"
                                    Y2graf = Val(arg)
                                Case "y3graf"
                                    Y3graf = Val(arg)
                                Case "y4graf"
                                    Y4Graf = Val(arg)
                                Case "logo"
                                    logo = Val(arg)
                                Case "lang"
                                    lang = Val(arg)
                                Case "screenrefresh"
                                    scrnref = Val(arg)
                                Case "graffiltering"
                                    GraFilt = Val(arg)

                                Case "grafxmin"
                                    GRx1 = Val(arg)
                                Case "grafxmax"
                                    GRx2 = Val(arg)
                                Case "grafymax"
                                    GRy2 = Val(arg)
                                Case "grafy2max"
                                    GR2Y2 = Val(arg)

                                Case "graflinewidth"
                                    GLineW = Val(arg)
                                Case "grafbackcolor"
                                    GBaCol = Val(arg)
                                Case "grafafrtarget"
                                    GrAnaTarg = Val(arg)
                                Case "gra1min"
                                    GRA1Y1 = Val(arg)
                                Case "gra1max"
                                    GRA1Y2 = Val(arg)
                                Case "gra2min"
                                    GRA2Y1 = Val(arg)
                                Case "gra2max"
                                    GRA2Y2 = Val(arg)
                                Case "grana1auto"
                                    If arg = "True" Or arg = "False" Then AutoAnaY1 = arg
                                Case "grana2auto"
                                    If arg = "True" Or arg = "False" Then AutoAnaY2 = arg
                                Case "tracecolor1" : Tcol(1) = Val(arg)
                                Case "tracecolor2" : Tcol(2) = Val(arg)
                                Case "tracecolor3" : Tcol(3) = Val(arg)
                                Case "tracecolor4" : Tcol(4) = Val(arg)
                                Case "tracecolor5" : Tcol(5) = Val(arg)
                                Case "tracecolor6" : Tcol(6) = Val(arg)
                                Case "tracecolor7" : Tcol(7) = Val(arg)
                                Case "tracecolor8" : Tcol(8) = Val(arg)

                                Case "obdport"
                                    OBDportnum = Val(arg)
                                Case "obdbaud"
                                    OBDbr = Val(arg)
                                Case "obdinit"
                                    OBDinit = arg
                                Case "obdon"
                                    If arg = "True" Or arg = "False" Then OBDon = arg
                                Case "obdchon"
                                    OBDchon(Val(arg)) = True
                                Case "saetemp"
                                    SAEtemp = Val(arg)
                                Case "saepress"
                                    SAEpress = Val(arg)
                                Case "saerh"
                                    SAErh = Val(arg)
                                Case "autogear"
                                    If arg = "True" Or arg = "False" Then AutoGR = arg
                                Case "saeauto"
                                    If arg = "True" Or arg = "False" Then SAEauto = arg
                                Case "saecomp"
                                    If arg = "True" Or arg = "False" Then SAEcomp = arg
                                Case "tuneacc"
                                    If arg = "True" Or arg = "False" Then Tacc = arg
                                Case "autoload"
                                    If arg = "True" Or arg = "False" Then autoload = arg
                                Case "autograf"
                                    If arg = "True" Or arg = "False" Then instagraf = arg
                                Case "autocomp"
                                    If arg = "True" Or arg = "False" Then autograf = arg
                                Case "autosave"
                                    If arg = "True" Or arg = "False" Then autosave = arg
                                Case "autotare"
                                    If arg = "True" Or arg = "False" Then autotare = arg
                                Case "maxaverage"
                                    If arg = "True" Or arg = "False" Then MaxAV = arg
                                Case "maxtracking"
                                    If arg = "True" Or arg = "False" Then MaxON = arg
                                Case "pening"                                               'Require code for controller calibrations?
                                    If arg = "True" Or arg = "False" Then admin = arg
                                    calenab = Not admin
                                Case "analograf"
                                    If arg = "True" Or arg = "False" Then AnaGrTop = arg
                                Case "finegraf"
                                    If arg = "True" Or arg = "False" Then grafgrid = arg
                                Case "break2point"
                                    If arg = "True" Or arg = "False" Then brek2p = arg
                                Case "breakramp"
                                    If arg = "True" Or arg = "False" Then brekramp = arg
                                Case "breakstop"
                                    If arg = "True" Or arg = "False" Then brekstop = arg
                                Case "remote"
                                    If arg = "True" Or arg = "False" Then remote = arg
                                Case "autoclosegraf"
                                    If arg = "True" Or arg = "False" Then AutoCloseGraf = arg
                                Case "bigsummary"
                                    If arg = "True" Or arg = "False" Then bigsum = arg
                                Case "graf2last"
                                    If arg = "True" Or arg = "False" Then G2Last = arg
                                Case "grafaxisame"
                                    If arg = "True" Or arg = "False" Then GAxSame = arg
                                Case "grafmanx"
                                    If arg = "True" Or arg = "False" Then GManX = arg
                                Case "grafmany"
                                    If arg = "True" Or arg = "False" Then GManY = arg
                                Case "grafman2y"
                                    If arg = "True" Or arg = "False" Then GMan2Y = arg
                                Case "breakload"
                                    brekload = Val(arg)
                                Case "remotechannel"
                                    remch = Val(arg)
                                Case "skincolor"
                                    Cskin = Val(arg)
                                Case "gagecolor"
                                    cgage = Val(arg)
                                Case "rpmref"
                                    If arg = " Dyno" Then : Bref.Text = "RPM: DYNO" : Else : Bref.Text = "RPM: ENGINE" : End If
                                Case "channels"
                                    'chan = Val(arg)
                                Case "ch"
                                    chan = chan + 1                             'Inc no of channels
                                    s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                    ' MsgBox(s + " : " + arg)
                                    Analab(chan) = s
                                    arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                    s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                    ' MsgBox(s + " : " + arg)
                                    Anaunit(chan) = s
                                    arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                    s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                    '  MsgBox(s + " : " + arg)
                                    anamin(chan) = Val(s)
                                    arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                    s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                    '   MsgBox(s + " : " + arg)
                                    anamax(chan) = Val(s)
                                    arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                    s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                    'MsgBox(s + " : " + arg)
                                    anafact(chan) = Val(s)
                                    arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                    s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                    'MsgBox(s + " : " + arg)
                                    anaofs(chan) = Val(s)
                                    arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                    s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                    ' MsgBox(s + " : " + arg)
                                    anafilt(chan) = Val(s)
                                    arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                    s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                    ' MsgBox("Next " + s + " : " + arg)
                                    anaMajTix(chan) = Val(s)
                                    arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                    s = Mid(arg, 1, InStr(arg, ",") - 1) 'Eliminate first element
                                    ' MsgBox("Last " + s + " : " + arg)
                                    anaMinTix(chan) = Val(s)
                                    arg = Mid(arg, InStr(arg, ",") + 1, Len(arg) - 1) 'Eliminate first element
                                    anadec(chan) = arg
                                Case Else
                                    sparecnt = sparecnt + 1
                                    spare(sparecnt) = orig                              'Save to put in Cal file later
                            End Select
                        End If
                    Loop Until s = "END"
                    FileRead.Close()            'Gotta use these 2 lines to close the file after reading
                    FileRead.Dispose()
                End Using
            Catch ex As Exception
                '"being used" If got this then tell 'em to close it n re-run
                If InStr(ex.Message.ToString(), "being used") > 0 Then
                    MsgBox(CalFile + " is being help open by another application." + Chr(13) + "Please CLOSE it and re-run Dynomonitor.")
                    End
                Else
                    'MsgBox("Had a problem reading the Calibration file at Line " + Str(lines) + Chr(13) + s + " : " + arg)
                    tempstr = InputBox("Had trouble after reading " + Str(lines) + " lines from: " + CalFile + Chr(13) + "This may be because of a recent software update." + Chr(13) + "Continue using current values?", "Calibration File Problem", "Yes")
                    If tempstr <> "" Then tempstr = Mid(tempstr, 1, 1)
                    If tempstr = "Y" Or tempstr = "y" Then
                        'Good to go!
                    Else
                        Call Defaults()
                    End If
                End If
            Finally
            End Try
        Else
            MsgBox(CalFile + " Does Not Exist. Using Defaults.")            'Use Defaults and create DynoCal file
            Call Defaults()
        End If
        'Setup Screen Boxes
        '  MsgBox("AcelFact" + Str(acel))

        ' TBMan.Text = Str(LoadMan)
        If Bref.Text = "RPM: ENGINE" Then : dgr = gr : Else : dgr = 1 : End If                 ' Added the DGR=1, not sure if correct 7777
        Tbrpm1.Text = (rpm1 * dgr).ToString("F0")
        TBRPM2.Text = (rpm2 * dgr).ToString("F0")
        TBLoad1.Text = Str(load1)
        TBLoad2.Text = Str(load2)
        TBAcel.Text = PullDur.ToString("F1")
        Ppul.Visible = True
        Ptun.Visible = False
        Select Case loadmode
            Case 1
                TBTarg.Text = manload.ToString("F1")
                Ppul.Visible = False
                Ptun.Visible = True
            Case 2
                TBTarg.Text = (rpmload * dgr).ToString("F0")
                Ppul.Visible = False
                Ptun.Visible = True
            Case 3
                TBTarg.Text = roadload.ToString("F1")
                Ppul.Visible = False
                Ptun.Visible = True
        End Select

        If gr = 0 Then gr = 1
        If lang = 1 Then CBCN.Checked = True

        If AutoGR = True Then : BRatio.BackColor = butcol1 : Else : BRatio.BackColor = Color.Transparent : End If
        If autograf = True Then : BUTAG.BackColor = butcol1 : Else : BUTAG.BackColor = Color.Transparent : End If
        If autoload = True Then : BUTAL.BackColor = butcol1 : Else : BUTAL.BackColor = Color.Transparent : End If
        If autosave = True Then : BUTAS.BackColor = butcol1 : Else : BUTAS.BackColor = Color.Transparent : End If
        If instagraf = True Then : BUTINSTA.BackColor = butcol1 : Else : BUTINSTA.BackColor = butcol0 : End If
        If play = True Then BUTINSTA.BackColor = butcol2
        If autotare = True Then : BuTare.BackColor = butcol1 : Else : BuTare.BackColor = Color.Transparent : End If
        If powmaxu = True Then : Lpow.Text = "kW" : Else : Lpow.Text = "hp" : End If
        Panel2.BackColor = ColSkin(Cskin)

        OCGage = 0                                'Setup to re-color
        If cgage <> 0 Then Call recolgage() 'Colorize bmp if required
        '   If loadmode = 1 Then CBMan.Checked = True    Start with NO LOADING
        '  If loadmode = 2 Then CBRPM.Checked = True
        ' If loadmode = 3 Then CB2Point.Checked = True
        'If loadmode = 4 Then CBAcel.Checked = True
    End Sub
    Private Sub savecal()
        Dim cs As String = ", "
        ' MsgBox("Saving Calfile " + CalFile)
        FTop = Me.Top : FLeft = Me.Left : FHeight = Me.Height : FWidth = Me.Width
        Fmax = (Me.WindowState = FormWindowState.Maximized)
        If System.IO.File.Exists(CalFile) Then                                      'Generate backup file
            Dim FileNameInfo As New System.IO.FileInfo(CalFile)
            Dim path As String = FileNameInfo.DirectoryName
            Dim fileName As String = FileNameInfo.Name
            'MsgBox("Trying to save Cal File")
            tempstr = fileName                                                      'Seperate file name for modification
            If InStr(CalFile, ".") Then
                tempstr = Mid(tempstr, 1, InStr(tempstr, ".") - 1) + "BAK.CSV"
            Else
                tempstr = tempstr + "BAK.CSV"
            End If
            ' MsgBox(CalFile + Chr(13) + tempstr + Chr(13) + path + "\" + tempstr + Chr(13) + Str(System.IO.File.Exists(path + "\" + tempstr)))
            If System.IO.File.Exists(path + "\" + tempstr) Then My.Computer.FileSystem.DeleteFile(path + "\" + tempstr) 'Kill previous backup
            My.Computer.FileSystem.RenameFile(CalFile, tempstr)                                         'Rename current to backup
        End If
        Using FileWrite As New System.IO.StreamWriter(CalFile)
            FileWrite.WriteLine("Focus Applied Technologies Dyno Shop Calibration File, DC5")                    'Header
            FileWrite.WriteLine("TagLine," + tagline)                           'Permanent Tag Line
            FileWrite.WriteLine("Ver," + ver)
            FileWrite.WriteLine("Type," + dynotype)
            FileWrite.WriteLine("RollerDiam," + Str(diam))
            FileWrite.WriteLine("DynoMaxRPM," + Str(RPMDynoMax))
            FileWrite.WriteLine("DynoMaxTrq," + Str(TorqDynoMax))
            FileWrite.WriteLine("Inertia," + Str(inert))
            FileWrite.WriteLine("FrictOFS," + Str(frictoff))
            FileWrite.WriteLine("FrictFact," + Str(frictfact))
            FileWrite.WriteLine("FrictQuad," + Str(FrictQuad))                               'Was formerly PPR (unused)
            FileWrite.WriteLine("ComPort," + Str(compt))
            FileWrite.WriteLine("ComBR," + Str(comBR))
            FileWrite.WriteLine("PingSec," + Str(deltaT))                                   'Ping Rate (sec)
            FileWrite.WriteLine("GearRatio," + Str(gr))

            FileWrite.WriteLine("LastCal," + lastcal)
            FileWrite.WriteLine("Gage1," + Str(g1CH))
            FileWrite.WriteLine("Gage2," + Str(g2CH))
            FileWrite.WriteLine("Gage3," + Str(g3CH))
            FileWrite.WriteLine("Disp1," + Str(d1CH))
            FileWrite.WriteLine("Disp2," + Str(d2CH))
            FileWrite.WriteLine("Disp3," + Str(d3CH))
            FileWrite.WriteLine("Disp4," + Str(d4CH))
            FileWrite.WriteLine("Disp5," + Str(d5ch))
            FileWrite.WriteLine("Disp6," + Str(d6ch))
            FileWrite.WriteLine("Channels," + Str(nch))
            For Me.i = 1 To nch                                             'Analog Labels and Factors
                FileWrite.WriteLine("CH," + Analab(Me.i) + "," + Anaunit(Me.i) + "," + Str(anamin(Me.i)) + "," + Str(anamax(Me.i)) + "," + Str(anafact(Me.i)) + "," + Str(anaofs(Me.i)) + "," + Str(anafilt(Me.i)) + "," + Str(anaMajTix(Me.i)) + "," + Str(anaMinTix(Me.i)) + "," + anadec(Me.i))
            Next
            FileWrite.WriteLine("LogInt," + Str(saveint))
            FileWrite.WriteLine("Metric," + Str(powmaxu))
            FileWrite.WriteLine("ScreenMag," + Str(MAG))
            FileWrite.WriteLine("InertEng," + Str(inerteng))
            FileWrite.WriteLine("RPMLoad," + Str(rpmload))
            FileWrite.WriteLine("ManLoad," + Str(manload))
            FileWrite.WriteLine("RPM1," + Str(rpm1))
            FileWrite.WriteLine("RPM2," + Str(rpm2))
            FileWrite.WriteLine("Load1," + Str(load1))
            FileWrite.WriteLine("Load2," + Str(load2))
            FileWrite.WriteLine("AcelFact," + Str(acel))
            FileWrite.WriteLine("LoadMode," + Str(loadmode))
            FileWrite.WriteLine("PullAnaCh," + Str(pgrafanach))
            FileWrite.WriteLine("DataPath," + DataPath)
            FileWrite.WriteLine("PullRPM2," + Str(Peng2))
            FileWrite.WriteLine("PullDuration," + Str(PullDur))
            FileWrite.WriteLine("PullKp," + Str(PullKp))
            FileWrite.WriteLine("TuneCfg," + TuneCal)
            FileWrite.WriteLine("TuneAcc," + Str(Tacc))
            FileWrite.WriteLine("SAEAuto," + Str(SAEauto))
            FileWrite.WriteLine("SAEComp," + Str(SAEcomp))
            FileWrite.WriteLine("SAETemp," + Str(SAEtemp))
            FileWrite.WriteLine("SAEPress," + Str(SAEpress))
            FileWrite.WriteLine("SAERh," + Str(SAErh))
            FileWrite.WriteLine("RunFile," + runfile)
            FileWrite.WriteLine("Run," + Str(run))
            FileWrite.WriteLine("Xgraf," + Str(Xgraf))
            FileWrite.WriteLine("Y1graf," + Str(Y1Graf))
            FileWrite.WriteLine("Y2graf," + Str(Y2graf))
            FileWrite.WriteLine("Y3graf," + Str(Y3graf))
            FileWrite.WriteLine("Y4graf," + Str(Y4Graf))
            FileWrite.WriteLine("AutoGear," + Str(AutoGR))
            FileWrite.WriteLine("AutoCloseGraf," + Str(AutoCloseGraf))
            FileWrite.WriteLine("AutoGraf," + Str(instagraf))
            FileWrite.WriteLine("AutoLoad," + Str(autoload))
            FileWrite.WriteLine("AutoComp," + Str(autograf))
            FileWrite.WriteLine("AutoSave," + Str(autosave))
            FileWrite.WriteLine("AutoTare," + Str(autotare))
            FileWrite.WriteLine("MaxAverage," + Str(MaxAV))
            FileWrite.WriteLine("MaxTracking," + Str(MaxON))
            FileWrite.WriteLine("Logo," + Str(logo))
            FileWrite.WriteLine("Lang," + Str(lang))
            FileWrite.WriteLine("Pening," + Str(admin))                         'This is for requiring code for cals
            FileWrite.WriteLine("GrafFiltering," + Str(GraFilt))
            FileWrite.WriteLine("GrafXMin," + Str(GRx1))
            FileWrite.WriteLine("GrafXMax," + Str(GRx2))
            FileWrite.WriteLine("GrafYMax," + Str(GRy2))
            FileWrite.WriteLine("GrafY2Max," + Str(GR2Y2))
            FileWrite.WriteLine("GrafLineWidth," + Str(GLineW))
            FileWrite.WriteLine("GrafBackColor," + Str(GBaCol))
            FileWrite.WriteLine("GrafAFRTarget," + Str(GrAnaTarg)) 'grafafrtarget
            FileWrite.WriteLine("GrafTop," + Str(GrafTop))
            FileWrite.WriteLine("GrafLeft," + Str(GrafLeft))
            FileWrite.WriteLine("GrafWidth," + Str(GrafWidth))
            FileWrite.WriteLine("GrafHeight," + Str(GrafHeight))
            FileWrite.WriteLine("InstTop," + Str(INSTTop))
            FileWrite.WriteLine("InstLeft," + Str(INSTLeft))
            FileWrite.WriteLine("InstWidth," + Str(INSTWidth))
            FileWrite.WriteLine("InstHeight," + Str(INSTHeight))
            FileWrite.WriteLine("FTop," + Str(FTop))
            FileWrite.WriteLine("FLeft," + Str(FLeft))
            FileWrite.WriteLine("FWidth," + Str(FWidth))
            FileWrite.WriteLine("FHeight," + Str(FHeight))
            FileWrite.WriteLine("FMax," + Str(Fmax))
            FileWrite.WriteLine("SkinColor," + Str(Cskin))
            FileWrite.WriteLine("GageColor," + Str(cgage))
            FileWrite.WriteLine("GrAna1Auto," + Str(AutoAnaY1))
            FileWrite.WriteLine("GrAna2Auto," + Str(AutoAnaY2))

            FileWrite.WriteLine("Analograf," + Str(AnaGrTop))
            FileWrite.WriteLine("FineGraf," + Str(grafgrid))
            FileWrite.WriteLine("BigSummary," + Str(bigsum))
            FileWrite.WriteLine("Graf2Last," + Str(G2Last))
            FileWrite.WriteLine("GrafAxiSame," + Str(GAxSame))
            FileWrite.WriteLine("GrafManX," + Str(GManX))
            FileWrite.WriteLine("GrafManY," + Str(GManY))
            FileWrite.WriteLine("GrafMan2Y," + Str(GMan2Y))
            FileWrite.WriteLine("GrA1Min," + Str(GRA1Y1))
            FileWrite.WriteLine("GrA1Max," + Str(GRA1Y2))
            FileWrite.WriteLine("GrA2Min," + Str(GRA2Y1))
            FileWrite.WriteLine("GrA2Max," + Str(GRA2Y2))
            FileWrite.WriteLine("TraceColor1," + Str(Tcol(1)))
            FileWrite.WriteLine("TraceColor2," + Str(Tcol(2)))
            FileWrite.WriteLine("TraceColor3," + Str(Tcol(3)))
            FileWrite.WriteLine("TraceColor4," + Str(Tcol(4)))
            FileWrite.WriteLine("TraceColor5," + Str(Tcol(5)))
            FileWrite.WriteLine("TraceColor6," + Str(Tcol(6)))
            FileWrite.WriteLine("TraceColor7," + Str(Tcol(7)))
            FileWrite.WriteLine("TraceColor8," + Str(Tcol(8)))


            FileWrite.WriteLine("Break2Point," + Str(brek2p))
            FileWrite.WriteLine("BreakRamp," + Str(brekramp))
            FileWrite.WriteLine("BreakStop," + Str(brekstop))
            FileWrite.WriteLine("BreakLoad," + Str(brekload))
            FileWrite.WriteLine("Remote," + Str(remote))
            FileWrite.WriteLine("RemoteChannel," + Str(remch))
            FileWrite.WriteLine("ScreenRefresh," + Str(scrnref))

            FileWrite.WriteLine("OBDPort," + Str(OBDportnum))
            FileWrite.WriteLine("OBDBaud," + Str(OBDbr))
            FileWrite.WriteLine("OBDInit," + OBDinit)
            FileWrite.WriteLine("OBDOn," + Str(OBDon))
            For i = 1 To 12
                If OBDchon(i) = True Then FileWrite.WriteLine("OBDchON," + Str(i))
            Next

            If Bref.Text = "RPM: DYNO" Then : FileWrite.WriteLine("RpmRef, Dyno") : Else : FileWrite.WriteLine("RpmRef, Engine") : End If
            For i = 1 To sparecnt
                FileWrite.WriteLine(spare(i))                           'These are "unuesd" cals (in case running down rev version)
            Next
            FileWrite.WriteLine("END")                                  'Always tell 'e when to stop
            FileWrite.Close()
            FileWrite.Dispose()
        End Using
    End Sub
    'Private Sub Butex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)  'Exit Button Routine
    '    dynotype = TBDynoType.Text
    '    ver = VerBox.Text

    '    Call savecal()
    '    FileClose(1)
    '    FileClose(2)
    '    Me.Close()          'Exit when "Exit" button hit 
    'End Sub
    Private Sub EStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EStop.Click
        If COMport.IsOpen Then
            'Sleep(30)
            If alarm = 0 Then
                COMport.Write("X") 'Send ALARM ON signal, plus CR, LF (or controller gets stuck)
            Else
                COMport.Write("x") 'Send ALARM OFF signal
            End If
            'Sleep(30)
        End If
    End Sub


    Private Sub ComBut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComBut.Click
        Dim oldBR As Integer
        serports = ""
        For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
            serports = serports + sp + ", "
        Next
        cpstr = InputBox("Currently Reading From: " + Str(compt) + Chr(13) + Chr(13) + "Available Ports: " + serports, "Enter Dyno Controller Com Port ", Str(compt))
        If cpstr <> "" Then
            compt = cpstr
            comstr = "COM" + LTrim(Str(compt))
            oldBR = comBR
            If COMport.IsOpen Then
                COMport.Close()
                If comBR = 9600 Then                    'If we are connected and NOT comming, then try the other baud rate
                    comBR = 115200
                Else
                    comBR = 9600
                End If
            End If
            COMport.PortName = comstr    'OPen  Com Port: NOTE Can use USB com ports just like internal ones!
            cpstr = InputBox("Baud Rate WAS: " + Str(oldBR) + Chr(13) + "Typically 9600 or 115200" + Chr(13) + "Enter Dyno Controller Com Baud Rate ", , Str(comBR))
            If cpstr <> "" Then comBR = Val(cpstr)
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
        If COMport.IsOpen Then COMport.Write("V") 'Just incase they're here cause lose coms (non-Verbose)
    End Sub
    Private Sub BuTare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuTare.Click
        If time - tareclick < 1 Then        'Is this the 2nd click in <1 sec? Then toggle AUTO TARE
            If autotare = True Then
                autotare = False
                BuTare.BackColor = Color.Transparent 'butcol0
            Else
                autotare = True
                BuTare.BackColor = butcol1
            End If
        End If
        tareclick = time
        COMport.Write("Z")                      'Tare Load Cell
    End Sub
    Private Sub ButEx_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButEx.Click
        'dynotype = TBDynoType.Text
        'ver = VerBox.Text
        Call savecal()
        FileClose(1)
        FileClose(2)

        sendcmd = "M0"                                  'Shutdown Current when closing!
        If COMport.IsOpen Then COMport.WriteLine(sendcmd) 'Will puke here if USB disconnected, so do last. 
        Sleep(20)
        End
    End Sub
    Private Sub Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Dim sav As String
        e.Cancel = True         'This cancels the closing
        sendcmd = "M0"                                  'Shutdown Current when closing!
        If COMport.IsOpen Then COMport.WriteLine(sendcmd) 'Will puke here if USB disconnected, so do last.
        Sleep(20)
        sav = MsgBox("Save Calibrations?", vbYesNo, "Save Calibrations Check")
        If sav = vbYes Then Call savecal()
        FileClose(1)
        FileClose(2)
        End
    End Sub
    Private Sub ButLog_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButLog.Click
        'tempstr = InputBox("Currently Logging to: " + SaveFile, "Enter New Log Fielname ", "C:\Focus\DMV5.CSV")
        Dim SaveFileBox As New SaveFileDialog
        With SaveFileBox
            .FileName = SaveFile
            .DefaultExt = "CSV"
            .Filter = "CSV|*.csv"
            .Title = "Log File to SAVE to"
            '  .InitialDirectory = applicationSettingsDirectory
            Dim result = .ShowDialog()
            If result = DialogResult.OK Then
                SaveFile = .FileName

                FileClose(2)
                FileOpen(2, SaveFile, OpenMode.Output)
                WriteLine(2, "Focus Applied Technologies Dyno Controller Log Version: " + ver + "    Opened: " + Now)
                Write(2, "Time")
                For i = 1 To nch
                    Write(2, Analab(i))
                Next
                WriteLine(2, " ")
                Write(2, "sec")
                For i = 1 To nch
                    Write(2, Anaunit(i))
                Next
                'Write(2, "Time (s)", "RPM1", "RPM2", "Torque Raw (Nm)", "Total Torque (Nm)", "Power (W)", "Load", "Mode", "SetPoint", "Throttle", "Alarm", "1:" + Analab(1), "2:" + Analab(2), "3:" + Analab(3), "4:" + Analab(4))
                WriteLine(2, " ")
                ButLog.Text = SaveFile
            End If
        End With


    End Sub
    Private Sub ButSav_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButSav.Click
        If savedat = False Then
            Dim SaveFileBox As New SaveFileDialog
            With SaveFileBox
                datafile = SaveFile.Replace("CSV", "DAT")
                datafile = datafile.Replace("csv", "DAT")
                .FileName = datafile
                .DefaultExt = "DAT"
                .Filter = "DAT|*.dat"
                .Title = "Data Logging File"
                '  .InitialDirectory = applicationSettingsDirectory
                Dim result = .ShowDialog()
                If result = DialogResult.OK Then
                    datafile = .FileName
                    FileClose(4)
                    FileOpen(4, datafile, OpenMode.Output)
                    WriteLine(4, "Focus Applied Technologies Dyno Controller Log Version: " + ver + "    Opened: " + Now)
                    Write(4, "Time (s)", "RPM1", "RPM2", "Torque Raw (Nm)", "Total Torque (Nm)", "Power (W)", "Load", "Throttle", "1:" + Analab(1), "2:" + Analab(2), "3:" + Analab(3), "4:" + Analab(4))
                    WriteLine(4)                  'End Of Line
                    Write(4, time)
                    For i = 1 To nch
                        Write(4, analog(i))
                    Next
                    WriteLine(4)
                    FileClose(4)
                    savedat = True
                End If
            End With
        Else
            FileOpen(4, datafile, OpenMode.Append)
            Write(4, time)
            For i = 1 To nch
                Write(4, analog(i))
            Next
            WriteLine(4)                  'End Of Line
            FileClose(4)
        End If
    End Sub
    Private Sub CBManD(ByVal sender As System.Object, ByVal e As System.EventArgs) 'MAN Check Box Changed
        If CBMan.Checked = True Then
            Ltarg.Text = "MANUAL %"
            ButPreset.Text = "ZERO"
            TBTarg.Text = manload.ToString("F1")
            If TBTarg.Text = "" Then TBTarg.Text = "0.0"
            If Val(TBTarg.Text) > 100 Then TBTarg.Text = "0.0"
            sendcmd = "M" + Mid(Str(Int(10 * Val(TBTarg.Text))), 2, Len(Str(Int(10 * Val(TBTarg.Text))) - 1))
            Console.Write("Sending: ")
            If COMport.IsOpen AndAlso TBTarg.Focused = False AndAlso loadon = True Then
                Sleep(20)
                COMport.WriteLine(sendcmd)
                Console.Write(sendcmd)
            End If
            loadmode = 1 : LastLMode = 1 ': loadon = True
            CBAcel.Checked = False
            CB2Point.Checked = False
        End If
    End Sub
    Private Sub CBRPMD(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If CBRPM.Checked = True Then
            Ltarg.Text = "SPEED RPM"
            ButPreset.Text = "CLAMP"
            TBTarg.Text = (rpmload * dgr).ToString("F0")
            sendcmd = "S" + Mid(Str(Int(rpmload)), 2, Len(Str(Int(rpmload)) - 1))
            If COMport.IsOpen AndAlso TBTarg.Focused = False AndAlso loadon = True Then
                Sleep(20)
                COMport.WriteLine(sendcmd)
                Console.WriteLine("Sending: " + sendcmd)
            End If
            loadmode = 2 : LastLMode = 2 ': loadon = True
            CBAcel.Checked = False
            CB2Point.Checked = False

        End If
    End Sub
    Private Sub CBRoadD(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If CBRoad.Checked = True Then
            Ltarg.Text = "ROAD %"
            ButPreset.Text = "100"
            TBTarg.Text = roadload.ToString("F0")
            If TBTarg.Text = "" Then TBTarg.Text = "0.0"
            If Val(TBTarg.Text) > 255 Then TBTarg.Text = "0.0"
            sendcmd = "R" + Mid(Str(Int(Val(TBTarg.Text))), 2, Len(Str(Int(Val(TBTarg.Text))) - 1))
            If COMport.IsOpen AndAlso loadon = True AndAlso TBTarg.Focused = False Then
                'Console.WriteLine("               Road Really ON:" + sendcmd)
                Sleep(20)                               'Have to add this to allow dyno controller to "get" residual comands from the last control mode
                COMport.WriteLine(sendcmd)
            End If
            loadmode = 3 : LastLMode = 3 ': loadon = True
            CBAcel.Checked = False
            CB2Point.Checked = False
        End If
    End Sub

    Private Sub TB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)  '              Triggers when moving out of box
        Select Case loadmode
            Case 1
                If CBMan.Checked = True Then
                    If TBTarg.Text = "" Then TBTarg.Text = "0.0"
                    If Val(TBTarg.Text) > 100 Then TBTarg.Text = "0.0"
                    manload = Val(TBTarg.Text)
                    sendcmd = "M" + Mid(Str(Int(10 * Val(TBTarg.Text))), 2, Len(Str(Int(10 * Val(TBTarg.Text))) - 1))
                    If COMport.IsOpen AndAlso TBTarg.Focused = False AndAlso loadon = True Then
                        Sleep(20)
                        COMport.WriteLine(sendcmd)
                    End If
                End If
            Case 2
                If TBTarg.Text = "" Then TBTarg.Text = "0"
                If Val(TBTarg.Text) > RPMDynoMax * dgr Then TBTarg.Text = RPMDynoMax * dgr
                rpmload = Val(TBTarg.Text) / dgr
                If CBRPM.Checked = True AndAlso TBTarg.Focused = False AndAlso loadon = True Then
                    sendcmd = "S" + Mid(Str(Int(rpmload)), 2, Len(Str(Int(rpmload)) - 1))
                    'MsgBox(sendcmd)
                    If COMport.IsOpen Then
                        Sleep(20)
                        COMport.WriteLine(sendcmd)
                    End If
                End If
            Case 3
                If CBRoad.Checked = True Then
                    If TBTarg.Text = "" Then TBTarg.Text = "0.0"
                    If Val(TBTarg.Text) > 255 Then TBTarg.Text = "0.0"
                    roadload = Val(TBTarg.Text)
                    sendcmd = "R" + Mid(Str(Int(Val(TBTarg.Text))), 2, Len(Str(Int(Val(TBTarg.Text))) - 1))
                    If COMport.IsOpen AndAlso TBTarg.Focused = False AndAlso loadon = True Then
                        Sleep(20)
                        COMport.WriteLine(sendcmd)
                    End If
                End If
        End Select

    End Sub

    Private Sub inkey(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        'MUST set Property of KEYPREVIEW to TRUE to use this stuff
        If TBTarg.Focused Or TBLoad1.Focused Or TBLoad2.Focused Or TBAcel.Focused Then              'Gotta do this or it'll "sequester" . , - and =
        Else
            Select Case Asc(e.KeyChar)
                Case 71, 103                                                'G or g
                    If GearRatio.RadioButton7.Checked = True And analog(1) > 1 Then
                        gr = speedc2 / analog(1)
                        e.Handled = True
                    Else

                    End If
                Case 46, 61
                    Call ButUP_Click(Nothing, Nothing) '< or - keys
                    e.Handled = True                'Prevent key strike from making it into a text box or whatever
                Case 44, 45
                    Call ButDN_Click(Nothing, Nothing) '> or + keys (= actually)
                    e.Handled = True                'Prevent key strike from making it into a text box or whatever
            End Select
        End If
    End Sub
    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
    '    If keyData = 39 Then Call ButUP_Click(Nothing, Nothing)
    '    If keyData = 37 Then Call ButDN_Click(Nothing, Nothing)
    '    Return (Nothing)
    'End Function
    Private Sub TBrpm1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)  '              Triggers when moving out of box
        If Tbrpm1.Text = "" Then Tbrpm1.Text = "0"
        If Val(Tbrpm1.Text) < 0 Then Tbrpm1.Text = "0"
        If Val(Tbrpm1.Text) > RPMDynoMax * dgr Then Tbrpm1.Text = (RPMDynoMax * dgr).ToString("F0")
        rpm1 = Int(Val(Tbrpm1.Text) / dgr)

    End Sub
    Private Sub TBrpm2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)  '              Triggers when moving out of box
        If TBRPM2.Text = "" Then TBRPM2.Text = "0"
        If Val(TBRPM2.Text) < 0 Then TBRPM2.Text = "0"
        If Val(TBRPM2.Text) > RPMDynoMax * dgr Then TBRPM2.Text = (RPMDynoMax * dgr).ToString("F0")
        rpm2 = Int(Val(TBRPM2.Text) / dgr)
    End Sub
    Private Sub TBload1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)  '              Triggers when moving out of box
        If TBLoad1.Text = "" Then TBLoad1.Text = "0"
        If Val(TBLoad1.Text) < 0 Then TBLoad1.Text = "0"
        If Val(TBLoad1.Text) > 100 Then TBLoad1.Text = "0"
        load1 = Val(TBLoad1.Text)
    End Sub
    Private Sub TBload2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)  '              Triggers when moving out of box
        If TBLoad2.Text = "" Then TBLoad2.Text = "0"
        If Val(TBLoad2.Text) < 0 Then TBLoad2.Text = "0"
        If Val(TBLoad2.Text) > 100 Then TBLoad2.Text = "0"
        load2 = Val(TBLoad2.Text)
    End Sub
    Private Sub CB2Point_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
            If Val(Tbrpm1.Text) > RPMDynoMax * dgr Then Tbrpm1.Text = (RPMDynoMax * dgr).ToString("F0")
            If Val(TBRPM2.Text) > RPMDynoMax * dgr Then TBRPM2.Text = (RPMDynoMax * dgr).ToString("F0")
            If Val(TBLoad1.Text) > 100 Then TBLoad1.Text = "0"
            If Val(TBLoad2.Text) > 100 Then TBLoad2.Text = "0"
            rpm1 = Val(Tbrpm1.Text) / dgr
            rpm2 = Val(TBRPM2.Text) / dgr
            load1 = Val(TBLoad1.Text)
            load2 = Val(TBLoad2.Text)
            pullmode = 5            'Start in "spin down" mode
            loadmode = 4
            'Beep()
            CBMan.Checked = False
            CBRPM.Checked = False
            CBRPM.Checked = False
        End If
    End Sub

    Private Sub CBAcel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If CBAcel.Checked = True Then
            If TBAcel.Text = "" Then TBAcel.Text = "15"
            'acel = Int(Val(TBAcel.Text) / dgr)            'Convert to Dyno speed
            pullmode = 5                'Start in "spin down" mode
            loadmode = 5
            CBMan.Checked = False
            CBRPM.Checked = False
            CBRPM.Checked = False
        End If
    End Sub

    Private Sub TBAcel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If TBAcel.Text = "" Then TBAcel.Text = "15"
        PullDur = Val(TBAcel.Text)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BRatio.Click
        If time - TGRclk < 1 Then
            AutoGR = Not (AutoGR)
            If AutoGR = True Then : BRatio.BackColor = butcol1 : Else : BRatio.BackColor = Color.Transparent : End If
            GearRatio.Hide()
        Else
            If GearRatio.Visible Then
                GearRatio.Hide()
            Else
                GearRatio.TBFact.Text = Str(gr)               'Set initial Gear Ratio to current
                GearRatio.RadioButton3.Checked = True
                If Speed1 > 50 And speed2 > 500 Then GearRatio.RadioButton7.Checked = True 'Set to RPM2 if got both RPM1 and 2 readings for fast set
                GearRatio.Show()
            End If
        End If
        TGRclk = time
    End Sub
    Private Sub setgr() Handles GearRatio.setgr             'Gets the "calibrated" gear ratio from the GR setting form when SET is hit
        gr = Int(GearRatio.grs * 100) / 100                 'Truncate a bit
        If gr = 0 Then gr = 1 'Catch div zero
        'SHOULD RE_DO ALL ENG RPM REFERENCE NUMBERS
        If Bref.Text = "RPM ENGINE" Then
            dgr = gr
            If CBRPM.Checked = True Then
                If CBRPM.Checked = True Then
                    rpmload = Int(Val(TBTarg.Text) / dgr)                 'Probably we wanna change the dyno set rpm to match the new gear ratio, and displayed engine RPM
                    sendcmd = "S" + Mid(Str(Int(rpmload)), 2, Len(Str(Int(rpmload)) - 1))
                    If COMport.IsOpen Then COMport.WriteLine(sendcmd)
                End If
            End If
            rpm1 = Int(Val(Tbrpm1.Text)) / dgr
            rpm2 = Int(Val(TBRPM2.Text)) / dgr
            '   TBrpmSP.Text = Str(Int(rpmload * dgr))          'Decided NOT to change RPMs cause we usually wanna use back the same Engine RPMs
            '   Tbrpm1.Text = Str(Int(rpm1 * dgr))
            '   TBRPM2.Text = Str(Int(rpm2 * dgr))
            '   TBAcel.Text = Str(Int(acel * dgr))
        End If
    End Sub
    Private Sub Form1_Resize_1(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        '    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize   8888 KEEPS LOSING HANDLES CLAUSE!!!
        XSG3 = Gage3.Width
        YSG3 = Gage3.Height
        XSG1 = Gage1.Width
        YSG1 = Gage1.Height
        rs.ResizeAllControls(Me)                                'Auto repositions and resizes everything on the screen. 
        LUG1.Font = TBG1.Font
        LUG2.Font = TBG2.Font
        LUG3.Font = TBG3.Font
        LG1.Font = TBG1.Font
        LG2.Font = TBG2.Font
        LG3.Font = TBG3.Font
        ' MsgBox("Resize")
        'Aussomness thenks to : https://www.codeproject.com/Tips/1025766/VB-NET-Dynamically-Resize-and-Reposition-All-Contr
    End Sub

    Private Sub CalBut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalBut.Click
        If Cals.Visible = True Then
            Cals.Hide()
        Else
            OCGage = cgage                                  'Assign old color to see if it changed
            Cals.Tbtit.Text = tagline
            Cals.TBDiam.Text = diam
            Cals.TBMaxrpm.Text = RPMDynoMax
            Cals.TBMaxTorq.Text = TorqDynoMax
            Cals.TBInert.Text = inert
            Cals.TBEngInert.Text = inerteng
            Cals.TBFofs.Text = frictoff
            Cals.TBFfact.Text = frictfact
            Cals.TBFquad.Text = FrictQuad
            Cals.TBType.Text = dynotype
            Cals.TBgr.Text = gr
            Cals.LEngInertEquiv.Text = Int(1000 * inerteng * gr ^ 2) / 1000
            Cals.LRef.Text = "Screen Ref: " + Str(scrnref)
            Cals.LKp.Text = "Kp(a): " + Str(PullKp)
            Cals.TBMag.Text = (MAG * 100).ToString

            Cals.TBSAEP.Text = SAEpress
            Cals.TBSAET.Text = SAEtemp
            Cals.TBSAER.Text = SAErh
            Cals.CBSAEAuto.Checked = SAEauto
            Cals.CBSAEComp.Checked = SAEcomp

            Cals.CBMax.Checked = MaxON
            Cals.CBMaxAv.Checked = MaxAV

            If calenab = False Then
                Cals.TBDiam.BackColor = Color.LightGray
                Cals.TBMaxrpm.BackColor = Color.LightGray
                Cals.TBMaxTorq.BackColor = Color.LightGray
                Cals.TBInert.BackColor = Color.LightGray
                Cals.TBEngInert.BackColor = Color.LightGray
                Cals.TBFofs.BackColor = Color.LightGray
                Cals.TBFfact.BackColor = Color.LightGray
                Cals.TBFquad.BackColor = Color.LightGray
                Cals.TBType.BackColor = Color.LightGray
                Cals.TBgr.BackColor = Color.LightGray
            End If

            calupdate = False                       'Dont change when selecting new item's data
            Cals.ChaName.SelectedIndex = calch - 1
            calupdate = True

            Cals.ButColSkin.BackColor = ColSkin(Cskin)
            Cals.ButColGage.BackColor = ColGage(cgage)

            tb(1).Text = Analab(calch)
            tb(2).Text = Anaunit(calch)
            tb(3).Text = anaofs(calch)
            tb(4).Text = anafact(calch)
            tb(5).Text = anamin(calch)
            tb(6).Text = anamax(calch)
            tb(7).Text = anafilt(calch)
            tb(8).Text = anaMajTix(calch)
            tb(9).Text = anaMinTix(calch)
            tb(10).Text = anadec(calch)

            Cals.CBRemote.Checked = remote
            Cals.TBRch.Text = remch
            Cals.TBBLoad.Text = brekload
            Cals.RB2PS.Checked = brek2p
            Cals.RBLoad.Checked = Not brek2p
            Cals.CBRamp.Checked = brekramp
            Cals.CBStart.Checked = brekstop

            Cals.Show()

        End If
    End Sub
    Private Sub Zcalcode() Handles Cals.Calcode
        Dim code, ccode As Integer
        Dim ccs As String
        code = 1000 * Rnd(1) + 20
        ccs = InputBox("Enter Cal Enable Code (" + Str(code) + "): ", "Calibration Enable", "0")
        If Val(ccs) <> 0 Then ccode = Val(ccs)
        calenab = False                                   'This needs to be stored somehow, not just assumed, cause with an "unlock" code they can actually bypass
        If ccode = Int(code / 10) Then calenab = True
        If calenab = True Then
            Cals.TBDiam.BackColor = Color.White
            Cals.TBMaxrpm.BackColor = Color.White
            Cals.TBMaxTorq.BackColor = Color.White
            Cals.TBInert.BackColor = Color.White
            Cals.TBEngInert.BackColor = Color.White
            Cals.TBFofs.BackColor = Color.White
            Cals.TBFfact.BackColor = Color.White
            Cals.TBFquad.BackColor = Color.White
            Cals.TBType.BackColor = Color.White
            Cals.TBgr.BackColor = Color.White
        Else

            Cals.TBDiam.BackColor = Color.LightGray
            Cals.TBMaxrpm.BackColor = Color.LightGray
            Cals.TBMaxTorq.BackColor = Color.LightGray
            Cals.TBInert.BackColor = Color.LightGray
            Cals.TBEngInert.BackColor = Color.LightGray
            Cals.TBFofs.BackColor = Color.LightGray
            Cals.TBFfact.BackColor = Color.LightGray
            Cals.TBFquad.BackColor = Color.LightGray
            Cals.TBType.BackColor = Color.LightGray
            Cals.TBgr.BackColor = Color.LightGray

        End If
    End Sub
    Private Sub setCals() Handles Cals.setcal             'Gets the "calibrated" gear ratio from the GR setting form when SET is hit
        Cals.Hide()
        tagline = Cals.Tbtit.Text

        remote = Cals.CBRemote.Checked
        brekload = Val(Cals.TBBLoad.Text)
        brek2p = Cals.RB2PS.Checked
        brekramp = Cals.CBRamp.Checked
        brekstop = Cals.CBStart.Checked
        remch = Val(Cals.TBRch.Text)

        SAEpress = Val(Cals.TBSAEP.Text)
        SAEtemp = (Cals.TBSAET.Text)
        SAErh = (Cals.TBSAER.Text)
        SAEauto = Cals.CBSAEAuto.Checked
        SAEcomp = Cals.CBSAEComp.Checked

        If Cals.CBMax.Checked And MaxON = False Then
            maxfirst = True                               'Re-do max/min on this loop
        End If
        MaxON = Cals.CBMax.Checked
        MaxAV = Cals.CBMaxAv.Checked
        MAG = Val(Cals.TBMag.Text) / 100

        If calenab = True Then
            diam = Val(Cals.TBDiam.Text)
            RPMDynoMax = Val(Cals.TBMaxrpm.Text)
            TorqDynoMax = Val(Cals.TBMaxTorq.Text)
            inert = Val(Cals.TBInert.Text)
            inerteng = Val(Cals.TBEngInert.Text)
            frictoff = Val(Cals.TBFofs.Text)
            frictfact = Val(Cals.TBFfact.Text)
            FrictQuad = Val(Cals.TBFquad.Text)
            dynotype = Cals.TBType.Text
            'TBDynoType.Text = dynotype
        End If
        'Refresh at screen


        i = Cals.ChaName.SelectedIndex + 1
        Analab(i) = tb(1).Text
        Anaunit(i) = tb(2).Text
        anaofs(i) = Val(tb(3).Text)
        anafact(i) = Val(tb(4).Text)
        anamin(i) = Val(tb(5).Text)
        anamax(i) = Val(tb(6).Text)
        anafilt(i) = Val(tb(7).Text)
        If anafilt(i) > 0.999 Then anafilt(i) = 0.999
        anaMajTix(i) = Val(tb(8).Text)
        anaMinTix(i) = Val(tb(9).Text)
        anadec(i) = tb(10).Text

        'SHOULD RE_DO ALL screen numbers
        Call Gage1_Ticks()
        Call Gage2_Ticks()
        Call Gage3_Ticks()
        newtix1 = True
        newtix2 = True
        newtix3 = True

        LD1.Text = Analab(d1CH)
        LD2.Text = Analab(d2CH)
        LD3.Text = Analab(d3CH)
        LD4.Text = Analab(d4CH)
        LD5.Text = Analab(d5ch)
        LD6.Text = Analab(d6ch)
        LG1.Text = Analab(g1CH) : LUG1.Text = Anaunit(g1CH)
        LG2.Text = Analab(g2CH) : LUG2.Text = Anaunit(g2CH)
        LG3.Text = Analab(g3CH) : LUG2.Text = Anaunit(g2CH)
        Panel2.BackColor = ColSkin(Cskin)
        If cgage <> OCGage Then Call recolgage()

    End Sub
    Sub recolgage()
        Select Case cgage
            Case 0
                Gage1.Image = My.Resources.Gage_2_3
                Gage2.Image = My.Resources.Gage_2_3
                Gage3.Image = My.Resources.Gage_2_3
            Case 1
                Gage1.Image = My.Resources.Gage_2b
                Gage2.Image = My.Resources.Gage_2b
                Gage3.Image = My.Resources.Gage_2b
            Case 2
                Gage1.Image = My.Resources.Gage_2g
                Gage2.Image = My.Resources.Gage_2g
                Gage3.Image = My.Resources.Gage_2g
            Case 3
                Gage1.Image = My.Resources.Gage_2r
                Gage2.Image = My.Resources.Gage_2r
                Gage3.Image = My.Resources.Gage_2r
            Case 4
                Gage1.Image = My.Resources.Gage_2y
                Gage2.Image = My.Resources.Gage_2y
                Gage3.Image = My.Resources.Gage_2y
        End Select

        Call Gage1_Ticks()
        Call Gage2_Ticks()
        Call Gage3_Ticks()
        ' LUG1.BackColor = ColGage(cgage)
        ' LUG2.BackColor = ColGage(cgage)
        ' LUG3.BackColor = ColGage(cgage)

    End Sub
    Private Sub ButBrek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButBrek.Click
        If brekon = False Then
            TBMode.Text = rembreks(lang)
            Call brek()
        Else
            Call brekrelease()
        End If
    End Sub
    Private Sub brek()
        prevmode = TBMode.Text
        lastload = loadmode
        ButBrek.BackColor = Color.Red
        LastLMode = loadmode
        CBMan.Checked = False
        CBRPM.Checked = False
        CBRoad.Checked = False
        CB2Point.Checked = False
        CBAcel.Checked = False
        brekon = True
    End Sub
    Private Sub brekrelease()
        TBMode.Text = prevmode
        prevmode = ""
        ButBrek.BackColor = Color.Transparent
        loadmode = LastLMode
        ' Console.Write("Released: " + Str(speed1) + "    Mode: " + Str(loadMode) + "  Comand: ")
        Select Case loadmode
            Case 1 : CBMan.Checked = True : Call CBManD(Nothing, Nothing)
            Case 2 : CBRPM.Checked = True : Call CBRPMD(Nothing, Nothing)
            Case 3 : CBRoad.Checked = True : Call CBRoadD(Nothing, Nothing)
            Case 4 : CB2Point.Checked = True
            Case 5 : CBAcel.Checked = True
        End Select
        brekon = False
    End Sub
    Private Sub Z_EngInert() Handles Cals.EIchanged
        Cals.LEngInertEquiv.Text = Int(1000 * Val(Cals.TBEngInert.Text) * Val(Cals.TBgr.Text) ^ 2) / 1000
    End Sub
    Private Sub seldisplay() Handles dispsel.setSel
        ' dispCH=dispsel.
        For i = 1 To nch
            If disprb(i).Checked Then dispCH = i
        Next
        dispsel.Hide()
        Select Case dispsel.Litem.Text
            Case "Display 1"
                d1CH = dispCH
                LD1.Text = Analab(dispCH)
            Case "Display 2"
                d2CH = dispCH
                LD2.Text = Analab(dispCH)
            Case "Display 3"
                d3CH = dispCH
                LD3.Text = Analab(dispCH)
            Case "Display 4"
                d4CH = dispCH
                LD4.Text = Analab(dispCH)
            Case "Display 5"
                d5ch = dispCH
                LD5.Text = Analab(dispCH)
            Case "Display 6"
                d6ch = dispCH
                LD6.Text = Analab(dispCH)
            Case "Gage 1"
                g1CH = dispCH
                LG1.Text = Analab(g1CH)
                LUG1.Text = Anaunit(g1CH)
                Call Gage1_Ticks()
                newtix1 = True
            Case "Gage 2"
                g2CH = dispCH
                LUG2.Text = Anaunit(g2CH)
                LG2.Text = Analab(g2CH)
                Call Gage2_Ticks()
                newtix2 = True
            Case "Gage 3"
                g3CH = dispCH
                LUG3.Text = Anaunit(g3CH)
                LG3.Text = Analab(g3CH)
                Call Gage3_Ticks()
                newtix2 = True
        End Select
    End Sub
    Private Sub DispSelAuf()
        For i = 1 To 27
            dislab(i).Text = Analab(i)
            If i > 15 Then
                ' Console.WriteLine("OBD CH" + Str(i - 15) + " is " + OBDchon(i - 15).ToString)
                OBDisLab(i - 15).Text = "OFF"
                If OBDchon(i - 15) Then
                    If OBDon Then OBDisLab(i - 15).Text = "OBD" + Str(i - 15)
                End If
            End If
        Next
        dispsel.Show()
    End Sub
    Private Sub TBD1_Chang(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBD1.Click
        dispCH = d1CH
        disprb(dispCH).Checked = True
        dispsel.Litem.Text = "Display 1"
        Call DispSelAuf()
    End Sub

    Private Sub TBD2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBD2.Click
        dispCH = d2CH
        disprb(dispCH).Checked = True
        dispsel.Litem.Text = "Display 2"
        Call DispSelAuf()
    End Sub

    Private Sub TBD3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBD3.Click
        dispCH = d3CH
        disprb(dispCH).Checked = True
        dispsel.Litem.Text = "Display 3"
        Call DispSelAuf()
    End Sub

    Private Sub TBD4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBD4.Click
        dispCH = d4CH
        disprb(dispCH).Checked = True
        dispsel.Litem.Text = "Display 4"
        Call DispSelAuf()
    End Sub
    Private Sub TBD5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBD5.Click
        dispCH = d5ch
        disprb(dispCH).Checked = True
        dispsel.Litem.Text = "Display 5"
        Call DispSelAuf()
    End Sub
    Private Sub TBD6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBD6.Click
        dispCH = d6ch
        disprb(dispCH).Checked = True
        dispsel.Litem.Text = "Display 6"
        Call DispSelAuf()
    End Sub

    Private Sub Gage2_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Gage2.Click
        If e.Button = MouseButtons.Right Then
            Beep()
            If MaxON Then
                aavmx(g2CH) = analog(g2CH)
                aavmn(g2CH) = analog(g2CH)
            End If
        Else
            dispCH = g2CH
            disprb(dispCH).Checked = True
            dispsel.Litem.Text = "Gage 2"
            Call DispSelAuf()
        End If

    End Sub

    Private Sub Gage1_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Gage1.Click
        If e.Button = MouseButtons.Right Then
            Beep()
            If MaxON Then
                aavmx(g1CH) = analog(g1CH)
                aavmn(g1CH) = analog(g1CH)
            End If
        Else

            dispCH = g1CH
            disprb(dispCH).Checked = True
            dispsel.Litem.Text = "Gage 1"
            Call DispSelAuf()
        End If
    End Sub

    Private Sub Gage3_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Gage3.Click
        If e.Button = MouseButtons.Right Then
            Beep()
            If MaxON Then
                aavmx(g3CH) = analog(g3CH)
                aavmn(g3CH) = analog(g3CH)
            End If
        Else

            dispCH = g3CH
            disprb(dispCH).Checked = True
            dispsel.Litem.Text = "Gage 3"
            Call DispSelAuf()
        End If
    End Sub

    Private Sub ButRunFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButRunFile.Click
        Dim SaveFileBox As New SaveFileDialog
        With SaveFileBox
            '     rundatafile = runfile + Str(run) + ".CSV"
            'rundatafile = runfile
            .FileName = DataPath + "\" + VehID
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

                VehID = runfile.Substring(runfile.LastIndexOf("\") + 1)         'Seperate our Vehicle ID
                DataPath = Mid(runfile, 1, Len(runfile) - Len(VehID) - 1)           'And Data Path

                'MsgBox("Run File: " + runfile + Chr(13) + "Path: " + DataPath + Chr(13) + "Veh ID: " + VehID)

                ButRunFile.Text = VehID
                run = 0                                                        'Reset Run Number
                TBRunFile.Text = runfile
            End If
        End With

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButGraf.Click

        If fcomp.Visible Then
            ' fcomp.Hide()
            '  If fcomp.ButAnaGraf.Text = "TOP" Then : AnaGrTop = True : Else : AnaGrTop = False : End If
        Else
            fcomp.Lrun1.Text = run
            fcomp.LID.Text = runfile
            fcomp.Ltime.Text = Now
            fcomp.Lsec.Text = timelaps.ToString("F1")
            fcomp.ButFile.Text = "Click to load file"           'This text is refferred to in FComp, if you change it here, change it there too
            fcomp.LM12.Text = "-" : fcomp.LM22.Text = "-"
            fcomp.LY12.Text = "-" : fcomp.LY22.Text = "-"
            fcomp.Lrun2.Text = "-" : fcomp.LLM12.Text = "-" : fcomp.LLM22.Text = "-"
            fcomp.Show()
        End If

    End Sub

    Private Sub Button1_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Select Case Bref.Text
            Case "RPM: DYNO"
                Bref.Text = "RPM: ENGINE"
                dgr = gr
                If loadmode = 2 Then TBTarg.Text = Str(Int(rpmload * dgr))
                Tbrpm1.Text = Str(Int(rpm1 * dgr))
                TBRPM2.Text = Str(Int(rpm2 * dgr))
                TBAcel.Text = PullDur.ToString("F1")
                Ldyno2.Text = "Engine"  ' Ldyno1.Text = "Engine" 
            Case "RPM: ENGINE"
                'TogRPM.Text = "CALC"
                Bref.Text = "RPM: DYNO"
                dgr = 1
                If loadmode = 2 Then TBTarg.Text = Str(Int(rpmload))
                Tbrpm1.Text = Str(Int(rpm1))
                TBRPM2.Text = Str(Int(rpm2))
                TBAcel.Text = PullDur.ToString("F1")
                Ldyno2.Text = "Dyno" '  Ldyno1.Text = "Dyno" :
            Case "RPM: CALC"
                Bref.Text = "RPM: DYNO"
                dgr = 1
                If loadmode = 2 Then TBTarg.Text = Str(Int(rpmload))
                Tbrpm1.Text = Str(Int(rpm1))
                TBRPM2.Text = Str(Int(rpm2))
                TBAcel.Text = PullDur.ToString("F1")
                Ldyno2.Text = "Dyno" '   Ldyno1.Text = "Dyno" :
        End Select
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Dim foto As Graphics = Nothing
        Dim bmp As New Bitmap(Me.Width, Me.Height)
        Me.DrawToBitmap(bmp, New Rectangle(0, 0, Me.Width, Me.Height))  'This works independend of "screen magnification", below had trouble when Mag<>100%
        ' Dim fmloc As System.Drawing.Point = Me.Bounds.Location
        ' Dim jfile As String
        ' foto = Graphics.FromImage(bmp)
        ' foto.CopyFromScreen(fmloc.X, fmloc.Y, 0, 0, bmp.Size)          '5, 20 adjust from edge of form 1 to edge of pannel2
        jfile = InputBox("Enter Filename for Screen Shot", DefaultResponse:="C:\Focus\EZDyno.JPG")
        If jfile <> "" Then bmp.Save(jfile, System.Drawing.Imaging.ImageFormat.Jpeg) '
    End Sub


    Private Sub ButRun_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ButRun.MouseDown
        Dim runs As String
        Console.WriteLine("Click: " + Str(e.Button))
        If e.Button = MouseButtons.Right Then
            runs = InputBox("ENTER NEXT RUN # ", "SET RUN", "")
            If runs <> "" Then run = Val(runs) - 1
            ButRun.Text = "Run:" + Str(run)
        Else
            run = 0
            ButRun.Text = "Run: 0"
        End If

    End Sub

    Private Sub PrintBut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintBut.Click
        Dim msgs(2) As String
        msgs(0) = "Want to see your logo here?" + Chr(13) + "To become a dealer contact us:" + Chr(13) + "www.FocusAppliedTechnologies.com" + Chr(13) + "Tel: +(6016) 484-6524"
        msgs(1) = "您想在这里看到您的徽标吗?" + Chr(13) + "要成为经销商，请联系我们：" + Chr(13) + "www.FocusAppliedTechnologies.com" + Chr(13) + "电话: +(6016) 484-6524"
        MsgBox(msgs(lang))
    End Sub
    Private Sub SetTorqCal() Handles contcals.CalSet
        Dim calfact, CalFactNew As Double
        If analog(4) <> 0 Then                                      'Ana4 is Raw Torque Value
            calfact = Val(contcals.TBTargNm.Text) / analog(4)
            CalFactNew = Int(calfact * Val(contcals.TextBox23.Text))
            MsgBox("Adjustment Factor: " + Str(Int(calfact * 100) / 100) + Chr(13) + "New Torque Calibration: " + Str(CalFactNew))
            If calfact * Val(contcals.TextBox23.Text) > 9999 Then
                MsgBox("The controller calibration factor " + Str(CalFactNew) + " Is out of range." + Chr(13) + "Check Load Cell or Change # of Decimal PLaces")
            Else
                contcals.TextBox23.Text = Str(CalFactNew)
                Call Z_SetCC()              'Set Cals)
            End If
        End If
    End Sub
    Private Sub BuTune_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuTune.Click
        If Ftune.Visible Then
            Ftune.Hide()
        Else
            Ftune.Show()
        End If
    End Sub
    Private Sub z_CloseTune() Handles Ftune.closeTune     'Close Tune Window
        Ftune.Hide()
    End Sub
    Private Sub Z_ContTare() Handles contcals.CTare
        COMport.Write("Z")                  'NOTE: NEED to re-read Controller Cals as this changes the TARE value (which we we re-send when writing to controller
        Console.WriteLine("TARE")           '-OR- Recalc the changed TARE value
        '========
        Sleep(100)
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
                    TrqTare1 = ContCal(44) + ContCal(45) * 256
                    TrqTare2 = ContCal(46) + ContCal(47) * 256 ' Only Performing Read for the Torque Tare values here                    TrqTare1 = ContCal(44) + ContCal(45) * 256
                    Console.WriteLine("READ Tare: " + Str(ContCal(44)) + "," + Str(ContCal(45)) + "," + Str(ContCal(46)) + "," + Str(ContCal(47)) + ",")
                    'Console.WriteLine("Torq TarE: " + Str(TrqTare))
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
                serenab = True                      'Turn SerCom reading back on
                COMport.Write("V")                   'Set Verbose Mode ON
            End If
        End If
        '========
    End Sub
    Public Sub Z_AutoFrict() Handles Cals.autofrict
        Dim temp As Integer
        temp = MessageBox.Show("Zero Torque and Spin dyno up to high speed." + Chr(13) + "Remove power/clutch Motor/Engine and hit OK" + Chr(13) + "Or Cancel this window to exit.", "Auto Calibrate Friction", MessageBoxButtons.OKCancel)
        'temp = MessageBox.Show("This option is not available yet.", "Auto Calibrate Friction", MessageBoxButtons.OKCancel)

        If temp = 1 And analog(1) > 100 Then                                'Go into Auto Friction mode
            CalBut.PerformClick()                       'Close Cal Box
            tempstr2 = tagline
            frictofft = frictoff
            frictfactt = frictfact
            frictquadt = FrictQuad
            TrqAvgT = torqueAF
            Speed1AvgT = speedaf
            torqueAF = 0.5
            speedaf = 0
            frictoff = 0
            frictfact = 0
            ' MsgBox("doing it")
            FAC = 0
            FACon = True            'Friction will be calculated from decelleration of inertia (excluding Engine Inert)
            IETemp = inerteng       'Temporarily store Engine Inertia 
            inerteng = 0            'And set it = 0 so it doesn't figgure into friction
        End If
    End Sub
    Private Sub Z_OpenContCals() Handles Cals.OpenCC
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
                    contcals.CBTS.Checked = False
                    contcals.CBPS.Checked = False
                    If (ContCal(7) And 8) = 8 Then contcals.CBTS.Checked = True
                    If (ContCal(7) And 16) = 16 Then contcals.CBPS.Checked = True
                    '  Console.WriteLine("Byte 7: " + Str(ContCal(7)) + " B7&8: " + Str((ContCal(7) And 8)))

                    contcals.TBMaxRPM.Text = Str(ContCal(26) + ContCal(27) * 256)     'RPM1 Max
                    contcals.TBRPMAlarm.Text = Str(ContCal(28) + ContCal(29) * 256)     'RPM1 Alarm
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
                    TrqTare1 = ContCal(44) + ContCal(45) * 256
                    TrqTare2 = ContCal(46) + ContCal(47) * 256 ' Only Performing Read for the Torque Tare values here
                    'TrqTare = ContCal(44) + ContCal(45) * 256 + ContCal(46) * 256 * 256 + ContCal(47) * 256 * 256 * 256
                    Console.WriteLine("READ Tare: " + Str(ContCal(44)) + "," + Str(ContCal(45)) + "," + Str(ContCal(46)) + "," + Str(ContCal(47)) + ",")
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
                serenab = True                      'Turn SerCom reading back on
                COMport.Write("V")                   'Set Verbose Mode ON
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
        Dim contQuest As String                     'Continue with questionable save?
        CCz = 0
        '====================Read cals from text boxes and qualify
        ContCal(1) = Val(contcals.TextBox1.Text)
        ContCal(2) = Val(contcals.TextBox2.Text)
        If ContCal(1) < 1 Then ContCal(1) = 1 : contcals.TextBox1.Text = "1"
        If ContCal(2) < 1 Then ContCal(2) = 1 : contcals.TextBox2.Text = "1"
        If ContCal(1) > 255 Then ContCal(1) = 255 : contcals.TextBox1.Text = "255"
        If ContCal(2) > 255 Then ContCal(2) = 255 : contcals.TextBox2.Text = "255"

        ContCal(4) = Int(Val(contcals.TextBox23.Text) / 256)
        ContCal(3) = Val(contcals.TextBox23.Text) - 256 * ContCal(4) 'Torq Calibration
        Console.Write("Before: " + Str(ContCal(7)) + "  ")
        ContCal(7) = Int(ContCal(7) / 8) * 8 + Val(contcals.TextBox17.Text)  'Torq DP
        ContCal(7) = (ContCal(7) And 231)                                       'Eliminate bits 3 and 4 (8 and 16)

        If contcals.CBTS.Checked = True Then ContCal(7) = ContCal(7) + 8
        If contcals.CBPS.Checked = True Then ContCal(7) = ContCal(7) + 16
        Console.WriteLine("After: " + Str(ContCal(7)))

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

        ContCal(27) = Int(Val(contcals.TBMaxRPM.Text) / 256)
        ContCal(26) = Val(contcals.TBMaxRPM.Text) - 256 * ContCal(27)
        ContCal(29) = Int(Val(contcals.TBRPMAlarm.Text) / 256)
        ContCal(28) = Val(contcals.TBRPMAlarm.Text) - 256 * ContCal(29)

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

        ContCal(45) = Int(TrqTare1 / 256)                                   ' Had to re-read if tared cause send
        ContCal(44) = TrqTare1 - ContCal(45) * 256
        ContCal(47) = Int(TrqTare2 / 256)
        ContCal(46) = TrqTare2 - ContCal(47) * 256
        Console.WriteLine("Send Tare: " + Str(ContCal(44)) + "," + Str(ContCal(45)) + "," + Str(ContCal(46)) + "," + Str(ContCal(47)) + ",")

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
            If ContCal(i) = 0 Then CCz = CCz + 1
            If testb(i) <> ContCal(i) Then Console.WriteLine("Different from read at " + Str(i) + " Byte: " + Str(ContCal(i)) + "  Ori: " + Str(testb(i)))
        Next

        ' MsgBox("5&6: " + Str(bcc(5)) + " " + Str(bcc(6)))
        If CCz > 30 Then
            contQuest = InputBox("Calibrations appear to be INVALID (" + Str(CCz) + ") are Zero's. Continue to SAVE?", "Controller Values Check", "YES")
            contQuest = UCase(contQuest)
            If contQuest = "Y" Or contQuest = "YES" Then
                COMport.WriteLine("v")                                      'Verbose OFF
                Thread.Sleep(50)                                            'This is probably not needed
                bcc(0) = Asc("L")                                           'L means "Load"
                COMport.Write(bcc, 0, 52)                                   'Send Controller Cals
                Thread.Sleep(100)                                           'Needs a pause while ser coms go out
            End If
        Else
            COMport.WriteLine("v")                                          'Verbose OFF
            Thread.Sleep(50)                                                'This is probably not needed
            bcc(0) = Asc("L")                                               'L means "Load" (to Controller)
            COMport.Write(bcc, 0, 52)                                       'Send Controller Cals
            Thread.Sleep(100)                                               'Needs a pause while ser coms go out
            Beep()                                                          'Beep in acknoledgement of sending
        End If
        COMport.WriteLine("V")                                              'Verbose mode back ON
    End Sub
    Private Sub Z_Burn() Handles contcals.Burn       'Burn Controller Calibrations to Controller
        Dim bcc(100) As Byte
        bcc(0) = Asc("G")
        COMport.Write(bcc, 0, 1)
        Thread.Sleep(10)
        Beep()
    End Sub
    Private Sub RsetCals() Handles Cals.ChanChange             'Gets the "calibrated" gear ratio from the GR setting form when SET is hit

        If calupdate = True Then
            Analab(calch) = tb(1).Text
            Anaunit(calch) = tb(2).Text
            anaofs(calch) = Val(tb(3).Text)
            anafact(calch) = Val(tb(4).Text)
            anamin(calch) = Val(tb(5).Text)
            anamax(calch) = Val(tb(6).Text)
            anafilt(calch) = Val(tb(7).Text)
            anaMajTix(calch) = Val(tb(8).Text)
            anaMinTix(calch) = Val(tb(9).Text)
            anadec(calch) = tb(10).Text
        End If

        'SHOULD RE_DO ALL screen NAMES, LINES and TIX

        calch = Cals.ChaName.SelectedIndex + 1         'Load new channel cals

        tb(1).Text = Analab(calch)
        tb(2).Text = Anaunit(calch)
        tb(3).Text = anaofs(calch)
        tb(4).Text = anafact(calch)
        tb(5).Text = anamin(calch)
        tb(6).Text = anamax(calch)
        tb(7).Text = anafilt(calch)
        tb(8).Text = anaMajTix(calch)
        tb(9).Text = anaMinTix(calch)
        tb(10).Text = anadec(calch)

    End Sub
    Private Sub CBCN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBCN.CheckedChanged
        lang = 0
        If CBCN.Checked = True Then lang = 1
    End Sub

    Private Sub BUTAL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BUTAL.Click
        If autoload = True Then
            autoload = False
            BUTAL.BackColor = butcol0
        Else
            autoload = True
            BUTAL.BackColor = butcol1
        End If
    End Sub

    Private Sub BUTAS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BUTAS.Click
        If autosave = True Then
            autosave = False
            BUTAS.BackColor = butcol0
        Else
            autosave = True
            BUTAS.BackColor = butcol1
        End If
    End Sub

    Private Sub BUTAG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BUTAG.Click
        If autograf = True Then
            autograf = False
            BUTAG.BackColor = butcol0
        Else
            autograf = True
            BUTAG.BackColor = butcol1
        End If
    End Sub

    Private Sub BUTINSTA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BUTINSTA.Click
        Dim li As Integer
        If time - instaClick < 1 Then
            play = Not (play)
            If play = True Then
                BUTINSTA.Text = "TESTING"
                BUTINSTA.BackColor = butcol2
            Else
                BUTINSTA.Text = "INSTAGRAF"
                If instagraf = True Then BUTINSTA.BackColor = butcol1
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
                If instagraf = True Then
                    instagraf = False
                    BUTINSTA.BackColor = butcol0
                    fgraf.Chart2.Series(4).Points.Clear()
                    For li = 1 To 4                                         'CLEAR OLD Instagraf DATA
                        fgraf.Chart1.Series(2 * li - 2).Points.Clear()
                        fgraf.Chart1.Series(2 * li - 1).Points.Clear()
                        fgraf.Chart2.Series(li - 1).Points.Clear()
                        fgraf.Chart2.ResetAutoValues()
                    Next

                Else
                    instagraf = True
                    BUTINSTA.BackColor = butcol1
                End If
            End If
        End If
        instaClick = time
    End Sub

    Private Sub Lpow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lpow.Click
        If powmaxu = True Then
            powmaxu = False
            Lpow.Text = "hp"
        Else
            powmaxu = True
            Lpow.Text = "kW"
        End If
        powmax = 0                        'Reste max power
    End Sub

    Private Sub TBPowMax_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBPowMax.Click
        powmax = 0                        'Reste max power
    End Sub
    Private Sub CloseInst() Handles fgraf.QuitInst
        INSTTop = fgraf.Top
        INSTLeft = fgraf.Left
        INSTWidth = fgraf.Width
        INSTHeight = fgraf.Height
        fgraf.Hide()
        If play = True Then
            play = False 'Shut OFF Testig mode automatically
            BUTINSTA.Text = "INSTAGRAF"
            instagraf = Not (instagraf)                                    'Invert to get back to orig setting
            If instagraf = True Then BUTINSTA.BackColor = butcol1
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

    Private Sub Button1_Click_4(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButLoad.Click
        If loadon = False Then
            loadon = True : loadmode = LastLMode : TBMode.Text = prevmode
            Console.WriteLine("ON ")
            Select Case loadmode
                Case 1 : CBMan.Checked = True : Call CBManD(Nothing, Nothing)
                Case 2 : CBRPM.Checked = True : Call CBRPMD(Nothing, Nothing)
                Case 3 : CBRoad.Checked = True : Call CBRoadD(Nothing, Nothing)
                Case 4 : CB2Point.Checked = True
                Case 5 : CBAcel.Checked = True
            End Select
        Else
            loadon = False : LastLMode = loadmode : prevmode = TBMode.Text : CBMan.Checked = False : CBRPM.Checked = False : CBRoad.Checked = False
            CB2Point.Checked = False : CBAcel.Checked = False : sendcmd = "M0"
            Sleep(20)                                               'Gotta have this pause to prevent "clashing" with other outogint comands
            If COMport.IsOpen Then COMport.WriteLine(sendcmd)
            TBMode.Text = remloads(lang)
            Console.WriteLine("OFF  I said M0!")
        End If

    End Sub



    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButOBD.Click
        If FOBD.Visible Then                'Close
            FOBD.Hide()
            OBDportnum = Val(FOBD.TBport.Text)
            OBDbr = Val(FOBD.TBBaud.Text)
            OBDinit = FOBD.TBInit.Text
            For i = 1 To 12
                If OBDCB(i).Checked = True Then
                    OBDchon(i) = True
                Else
                    OBDchon(i) = False
                End If
            Next

        Else                                     'Open
            serports = ""
            For Each sp As String In My.Computer.Ports.SerialPortNames      'This gives the names of available com ports
                serports = serports + sp + ", "
            Next
            serports = serports.Replace("COM", "")              'Lose the "COM"s to shorten so we can see them
            FOBD.LPorts.Text = "Ports: " + serports
            FOBD.TBport.Text = OBDportnum
            FOBD.TBBaud.Text = OBDbr
            FOBD.TBInit.Text = OBDinit
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

    Private Sub BrollerBrek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrollerBrek.Click
        If RolBrek = True Then
            RolBrek = False
            BrollerBrek.BackgroundImage = My.Resources.BrekOFF
            sendcmd = "M0"
            If COMport.IsOpen Then COMport.WriteLine(sendcmd)
        Else
            If Speed1 < 200 Then                                              'Only actuate if RPM<200
                RolBrek = True
                RolBrekTime = time
                sendcmd = "M500"
                If COMport.IsOpen Then COMport.WriteLine(sendcmd)
                BrollerBrek.BackgroundImage = My.Resources.BrekON
            Else
                TBMode.Text = "SLOW to below 200 RPM before BRAKING"
            End If
        End If
    End Sub
    Private Sub locksend()
        Dim bcc(100) As Byte                    'Literal codes to send
        Dim scode As String
        Dim Ulock(20) As Integer
        Dim random As New Random()
        scode = Flock.TBScode.Text.Replace(" ", "")
        scode = scode.ToUpper
        ' Counter sign:     K  AddTime0  Key1  
        Ulock(1) = 75                           'K
        Ulock(3) = Val("&H" & Mid(scode, 9, 2)) 'Key byte 1
        Ulock(4) = Val("&H" & Mid(scode, 1, 2)) 'get from their numbers
        Ulock(5) = Val("&H" & Mid(scode, 11, 2))
        Ulock(6) = Int(Val("&H" & Mid(scode, 5, 2)) / 25)                                 'Addnl Time byte 1
        Ulock(2) = 10 * (Val("&H" & Mid(scode, 5, 2)) - 25 * Ulock(6))              'Addnl time Byte 0
        Ulock(7) = Val("&H" & Mid(scode, 5, 2)) 'get from their numbers
        Ulock(8) = Val("&H" & Mid(scode, 7, 2)) 'Key byte 0
        Ulock(10) = random.Next(0, 255) 'get from their numbers
        If Val("&H" & Mid(scode, 3, 2)) + Val("&H" & Mid(scode, 5, 2)) = 255 Then
            Ulock(2) = 255
            Ulock(6) = 255                  'Unolck Permanently if 2nd and 3rd bytes add to 255
        End If
        Console.writeline("Adding time: " + Str(Ulock(2) + 25 * Ulock(6)))
        For k = 1 To 8
            Ulock(9) += Ulock(k)
        Next
        Ulock(9) += Ulock(10)
        Ulock(9) = Ulock(9) And 255           'Truncate to lowest byte
        'Send!
        For k = 1 To 10
            bcc(k - 1) = Ulock(k)
            Console.WriteLine("COUNTER SIGN(" + Str(k) + ") : " + Str(Ulock(k))) ' + Ulock(2) + Ulock(3) + Ulock(4) + Ulock(5) + Ulock(6) + Ulock(7) + Ulock(8) + Ulock(9) + Ulock(10))
        Next

        If COMport.IsOpen Then COMport.Write("v") 'Set Verbose Mode OFF
        Sleep(300) 'Wait for 1 ping
        serenab = False                         'Shut OFF ser Com reading (of data)
        If COMport.IsOpen Then COMport.DiscardInBuffer()
        COMport.ReadTimeout = 100              'Just to throw an exception below
        COMport.DiscardNull = False             'Not needed
        Try
            If COMport.IsOpen Then '
                COMport.WriteLine("v")                                          'Verbose OFF
                Thread.Sleep(50)                                                'This is probably not needed
                COMport.Write(bcc, 0, 10)                                       'Send Controller Cals
                Thread.Sleep(200)                                               'Needs a pause while ser coms go out
                Beep()
            End If
            'COMport.Write(Ulock(1) + Ulock(2) + Ulock(3) + Ulock(4) + Ulock(5) + Ulock(6) + Ulock(7) + Ulock(8) + Ulock(9) + Ulock(10)) 'Send Countersign "S"Code
            If COMport.IsOpen Then COMport.DiscardInBuffer()
            '===================Show and Populate Table
        Catch ex As TimeoutException
            MsgBox("We had a problem sending S-Code. Please Retry.")
            If COMport.IsOpen Then COMport.DiscardInBuffer() 'Never gets back in to VERBOSE mode here!!
            COMport.DiscardOutBuffer()
            If COMport.IsOpen Then If COMport.IsOpen Then COMport.Close()
            If Not (COMport.IsOpen) Then COMport.Open()
            Sleep(50)                          'Give time to get open
            serenab = True                      'Turn SerCom reading back on
            If COMport.IsOpen Then COMport.Write("V")
        End Try
        serenab = True                      'Turn SerCom reading back on
        If COMport.IsOpen Then COMport.Write("V")
        Flock.Hide()        'Clear the form
        Sleep(300) 'Wait for 1 ping
        LockChk()           'Open the form

    End Sub
    Private Sub LockChk()
        Dim checksum, Lock(20), sc1, sc2, Icode1, icode2 As Integer
        If Flock.Visible Then
            Flock.Hide()
        Else
            Flock.Show()
            Flock.TBScode.Text = ""                       'Blank the S-Code
            If COMport.IsOpen Then COMport.Write("v") 'Set Verbose Mode OFF
            Sleep(300) 'Wait for 1 ping
            serenab = False                         'Shut OFF ser Com reading (of data)
            If COMport.IsOpen Then COMport.DiscardInBuffer()
            COMport.ReadTimeout = 100              'Just to throw an exception below
            COMport.DiscardNull = False             'Not needed
            If COMport.IsOpen Then COMport.Write("k") 'Request Lock Status
            Try
                For i = 1 To 10                                       '       1    2      3      4     5    6     7     8      9      10  
                    If COMport.IsOpen Then Lock(i) = COMport.ReadByte 'Read: Rnd, Time0, Code1, rnd, Stat, Time1, Rnd, Lock0, Check. Rnd 
                    If i <> 9 Then checksum = checksum + Lock(i)
                Next
                checksum = checksum And 255                   'Truncate to lowest byte
                Thread.Sleep(20)
                If COMport.IsOpen Then COMport.DiscardInBuffer()
                '===================Show and Populate Table
                Console.WriteLine("Status Byte: " + Convert.ToString(Lock(5), 2))

                Flock.Lstat.Text = "UNLOCKED"
                If Lock(5) And 128 = 128 Then Flock.Lstat.Text = "LOCKED"
                If Lock(5) And 64 = 64 Then Flock.Lstat.Text = "TEMPORARILY OPEN"
                Flock.Lhours.Text = Str((Lock(6) * 255 + Lock(2)) / 10)
                Flock.Lcode.Text = Lock(6).ToString("X2") + " " + Lock(3).ToString("X2") + " " + Lock(2).ToString("X2") + " " + Lock(7).ToString("X2") + " " + Lock(8).ToString("X2") + " " + Lock(10).ToString("X2")
                Console.WriteLine("Controller Read Checksum: " + Str(checksum And 127))
                If checksum <> Lock(9) Then
                    MsgBox("Bad Read from Controller: Checksum: " + Str(checksum) + " Sent CS: " + Str(Lock(9)) + Chr(13) + "Please Exit and Reload")
                End If
                Flock.lcounter.hide()

                '===DECODING CODE REMOVE BEFORE PUBLISHING!! =====
                Icode1 = ((Lock(8) + Lock(3) * 256) Xor 53955) And 65535
                Console.WriteLine(Str(Lock(8) + 256 * Lock(3)) + " -> " + Str(Icode1))
                icode2 = ((Icode1 * 16) And 65535) + Int(Icode1 / 256 / 16)
                Console.WriteLine(Str(icode2))
                icode2 = (icode2 + 1355) And 65535
                Console.WriteLine(Str(icode2))
                sc1 = icode2 And 255
                sc2 = Int(icode2 / 256)
                Console.WriteLine("Decode: xx xx xx " + sc1.ToString("X2") + " " + sc2.ToString("X2") + " xx")
                'Flock.Lcounter.Text = "Decode: xx xx xx " + sc1.ToString("X2") + " " + sc2.ToString("X2") + " xx"
                '================================================

            Catch ex As TimeoutException
                MsgBox("Lock Status Not Fully Loaded: " + Str(i) + " Bytes of 10" + Chr(13) + "First Bytes:" + Str(Lock(1)) + Str(Lock(2)) + Str(Lock(3)) + Chr(13) + " Please Retry.")
                If COMport.IsOpen Then COMport.DiscardInBuffer() 'Never gets back in to VERBOSE mode here!!
                COMport.DiscardOutBuffer()
                If COMport.IsOpen Then If COMport.IsOpen Then COMport.Close()
                If Not (COMport.IsOpen) Then COMport.Open()
                Sleep(50)                          'Give time to get open
                serenab = True                      'Turn SerCom reading back on
                If COMport.IsOpen Then COMport.Write("V")
            End Try
            serenab = True                      'Turn SerCom reading back on
            If COMport.IsOpen Then COMport.Write("V")
        End If
    End Sub

    Private Sub Gage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gage1.Click

    End Sub

    Private Sub Gage2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gage2.Click

    End Sub

    Private Sub Gage3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gage3.Click

    End Sub

    Private Sub Btun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btun.Click
        If Btun.Text = "LOAD: TUNE" Then
            Btun.Text = "LOAD: PULL"
            Ppul.Visible = True
            Ptun.Visible = False
        Else
            Btun.Text = "LOAD: TUNE"
            Ppul.Visible = False
            Ptun.Visible = True
        End If
    End Sub
    Private Sub ButPRESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButPreset.Click
        Select Case loadmode
            Case 1
                TBTarg.Text = "0.0"
                manload = 0
                If COMport.IsOpen AndAlso TBTarg.Focused = False Then
                    Sleep(30)                   'Extra long sleep to get this one through
                    COMport.WriteLine("M0")
                End If
                loadon = True                       '               Only in this case do we send it
            Case 2
                rpmload = Int(Speed1 / 10 + 0.5) * 10
                TBTarg.Text = Str(rpmload * dgr)               'Set RPM set point to instantanious speed
                If CBRPM.Checked = True Then
                    sendcmd = "S" + Mid(Str(Int(rpmload)), 2, Len(Str(Int(rpmload)) - 1))
                    If COMport.IsOpen AndAlso TBTarg.Focused = False AndAlso loadon = True Then
                        COMport.WriteLine(sendcmd)
                    End If
                End If
            Case 3
                TBTarg.Text = "100"
                roadload = 100
                If COMport.IsOpen AndAlso TBTarg.Focused = False AndAlso loadon = True Then
                    Sleep(30)                   'Extra long sleep to get this one through
                    COMport.WriteLine("R100")
                End If
                'loadon = True
        End Select


    End Sub
    Private Sub ButUP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButUp.Click
        Select Case loadmode
            Case 1
                If TBTarg.Text = "" Then TBTarg.Text = "0.0"
                TBTarg.Text = Str(TBTarg.Text + 1)
                If Val(TBTarg.Text) > 100 Then TBTarg.Text = "0.0"
                manload = Val(TBTarg.Text)
                sendcmd = "M" + Mid(Str(Int(10 * Val(TBTarg.Text))), 2, Len(Str(Int(10 * Val(TBTarg.Text))) - 1))
                If COMport.IsOpen AndAlso CBMan.Checked = True AndAlso loadon = True Then
                    Sleep(20)
                    COMport.WriteLine(sendcmd)
                    'loadmode = 1 : LastLMode = 1 : loadon = True
                    'MsgBox("CBman " + Str(CBMan.Checked))
                End If
            Case 2
                If Bref.Text = "RPM: ENGINE" Then
                    TBTarg.Text = Val(TBTarg.Text) + 100
                    rpmload = Val(TBTarg.Text) / dgr
                Else
                    rpmload = rpmload + 100
                    TBTarg.Text = Str(rpmload)
                End If

                If CBRPM.Checked = True Then
                    sendcmd = "S" + Mid(Str(Int(rpmload)), 2, Len(Str(Int(rpmload)) - 1))
                    If COMport.IsOpen AndAlso TBTarg.Focused = False AndAlso loadon = True Then
                        COMport.WriteLine(sendcmd)
                    End If
                End If
            Case 3
                If TBTarg.Text = "" Then TBTarg.Text = "0.0"
                TBTarg.Text = Str(TBTarg.Text + 1)
                If Val(TBTarg.Text) > 255 Then TBTarg.Text = "0.0"
                roadload = Val(TBTarg.Text)
                sendcmd = "R" + Mid(Str(Int(Val(TBTarg.Text))), 2, Len(Str(Int(Val(TBTarg.Text))) - 1))
                If COMport.IsOpen AndAlso CBRoad.Checked = True AndAlso loadon = True Then
                    Sleep(20)
                    COMport.WriteLine(sendcmd)
                    Console.WriteLine("Sent: " + sendcmd)
                    ' loadmode = 1 : LastLMode = 1 : loadon = True
                    'MsgBox("CBman " + Str(CBMan.Checked))
                End If
        End Select
    End Sub
    Private Sub ButDN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButDn.Click
        Select Case loadmode
            Case 1
                If TBTarg.Text = "" Then TBTarg.Text = "0.0"
                TBTarg.Text = Str(TBTarg.Text - 1)
                If Val(TBTarg.Text) < 0 Then TBTarg.Text = "0.0"
                manload = Val(TBTarg.Text)
                sendcmd = "M" + Mid(Str(Int(10 * Val(TBTarg.Text))), 2, Len(Str(Int(10 * Val(TBTarg.Text))) - 1))
                If COMport.IsOpen AndAlso CBMan.Checked = True AndAlso loadon = True Then
                    Sleep(20)
                    COMport.WriteLine(sendcmd)
                    ' loadmode = 1 : LastLMode = 1 : loadon = True
                    'MsgBox("CBman " + Str(CBMan.Checked))
                Else
                    sendcmd = ""
                End If
            Case 2
                If Bref.Text = "RPM: ENGINE" Then
                    TBTarg.Text = Val(TBTarg.Text) - 100
                    rpmload = Val(TBTarg.Text) / dgr
                Else
                    rpmload = rpmload - 100
                    TBTarg.Text = Str(rpmload)
                End If
                If rpmload < 0 Then rpmload = 0

                If CBRPM.Checked = True Then
                    sendcmd = "S" + Mid(Str(Int(rpmload)), 2, Len(Str(Int(rpmload)) - 1))
                    If COMport.IsOpen AndAlso TBTarg.Focused = False AndAlso loadon = True Then
                        COMport.WriteLine(sendcmd)
                    End If
                End If
            Case 3
                If TBTarg.Text = "" Then TBTarg.Text = "0.0"
                TBTarg.Text = Str(TBTarg.Text - 1)
                If Val(TBTarg.Text) < 0 Then TBTarg.Text = "0.0"
                roadload = Val(TBTarg.Text)
                sendcmd = "R" + Mid(Str(Int(Val(TBTarg.Text))), 2, Len(Str(Int(Val(TBTarg.Text))) - 1))
                If COMport.IsOpen AndAlso CBRoad.Checked = True AndAlso loadon = True Then
                    Sleep(20)
                    COMport.WriteLine(sendcmd)
                    ' loadmode = 1 : LastLMode = 1 : loadon = True
                Else
                    sendcmd = ""
                End If
        End Select
    End Sub

    Private Sub TogRPM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bref.Click
        Select Case Bref.Text
            Case "RPM: DYNO"
                Bref.Text = "RPM: ENGINE"
                dgr = gr
                If loadmode = 2 Then TBTarg.Text = Str(Int(rpmload * dgr))
                Tbrpm1.Text = Str(Int(rpm1 * dgr))
                TBRPM2.Text = Str(Int(rpm2 * dgr))
                TBAcel.Text = PullDur.ToString("F1")
                Ldyno2.Text = "Engine"  ' Ldyno1.Text = "Engine" 
            Case "RPM: ENGINE"
                'TogRPM.Text = "CALC"
                Bref.Text = "RPM: DYNO"
                dgr = 1
                If loadmode = 2 Then TBTarg.Text = Str(Int(rpmload))
                Tbrpm1.Text = Str(Int(rpm1))
                TBRPM2.Text = Str(Int(rpm2))
                TBAcel.Text = PullDur.ToString("F1")
                Ldyno2.Text = "Dyno" '  Ldyno1.Text = "Dyno" :
            Case "RPM: CALC"
                Bref.Text = "RPM: DYNO"
                dgr = 1
                If loadmode = 2 Then TBTarg.Text = Str(Int(rpmload))
                Tbrpm1.Text = Str(Int(rpm1))
                TBRPM2.Text = Str(Int(rpm2))
                TBAcel.Text = PullDur.ToString("F1")
                Ldyno2.Text = "Dyno" '   Ldyno1.Text = "Dyno" :
        End Select
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

                        f = ctl.Font
                        fontRatioW = ctl.Width / c.originalWidth
                        fontRatioH = ctl.Height / c.originalHeight
    
                        If (ret) Then
                            fontRatio = (fontRatioW + fontRatioH) / 2 '-- average change in control Height and Width

         

                            If (ctl.Name = "LG1" Or ctl.Name = "LG2" Or ctl.Name = "LG3" Or ctl.Name = "LUG1" Or ctl.Name = "LUG2" Or ctl.Name = "LUG3") Then
                                '  kjhkjh()
                                If ctl.Name = "LUG1" Then
                                    'Apparently this control is getting moved some place else, and moving it here only gives it 2x the "scaling". No idea why
                                Else
                                    'Console.WriteLine("Got " + ctl.Name + " Ratios: " + Str(DynoMonitor.g1ratioH) + ", " + Str(DynoMonitor.g1ratioW))
                                    ctl.Top = Int(ctl.Top * DynoMonitor.g1ratioH)
                                    ctl.Left = Int(ctl.Left * DynoMonitor.g1ratioW)
                                    ctl.Font = New Font(f.FontFamily, c.originalFontSize * (DynoMonitor.g1ratioW + DynoMonitor.g1ratioH) / 2, f.Style)
                                End If
                            Else
                                '-- Size
                                ctl.Width = Int(parentWidth * c.widthPercent)
                                ctl.Height = Int(parentHeight * c.heightPercent)
                                '-- Position
                                ctl.Top = Int(parentHeight * c.topOffsetPercent)
                                ctl.Left = Int(parentWidth * c.leftOffsetPercent)
                                '-- Font
                                f = ctl.Font
                                fontRatioW = ctl.Width / c.originalWidth
                                fontRatioH = ctl.Height / c.originalHeight
                                fontRatio = (fontRatioW + fontRatioH) / 2 '-- average change in control Height and Width
                                ctl.Font = New Font(f.FontFamily, c.originalFontSize * fontRatio, f.Style)
                                frav = ctl.Font.Size / c.originalFontSize
                            End If

                            If ctl.Name = "Gage3" Or ctl.Name = "Gage2" Then
                                DynoMonitor.g1ratioW = (ctl.Width / DynoMonitor.XSG3)  'DynoMonitor.Gage3.Width
                                DynoMonitor.g1ratioH = (ctl.Height / DynoMonitor.YSG3)  ' DynoMonitor.Gage3.Height
                                Console.WriteLine("Got Gage 3/2" + " Ratios: " + Str(DynoMonitor.g1ratioH) + ", " + Str(DynoMonitor.g1ratioW))
                            End If
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
        '   Form1.gage1.LG1.Top = Form1.LG1.Top * g1ratioH
        '   Form1.LG1.Left = Form1.LG1.Left * g1ratioW

    End Sub

End Class