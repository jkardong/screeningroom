REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and creates a Screeners Splash Screen
REM ==
REM ********************************************************************
REM ********************************************************************



REM ====================================================================
REM == NAME: InitSplashScreen
REM == INPUT PARAMETERS: 
REM ==      BodyText: Associative Array of values to display 
REM == OUTPUT: Object - roParagraphScreen
REM == EXAMPLE CALL: InitSplashScreen()
REM == DESCRIPTION: Initializes a Splash Screen
REM ====================================================================
Function InitSplashScreen(ScreenFacade As Object) As Object

    'Print to Debugger
    DebugPrint("Initializing Splash - [ scr_Splash.InitWelcomeScreen() ]")

    'Create Message Port
    port = CreateObject("roMessagePort")

    'Create Parapraph Screen
    screen = CreateObject("roImageCanvas")

    'Set The Message Port
    screen.SetMessagePort(port)

    'Get Display Mode
    mode = CreateObject("roDeviceInfo").GetDisplayMode()

    'Print Screen Resolution to Debugger
    if (mode = "720p") Then

        'Set HD Canvas Items
        canvasItems = [
                        {
                            url:"pkg:/images/Theme/Title_Bar_HD.png"
                            TargetRect:{x:0,y:0,w:1280,h:138}
                        },
                        {
                            Text:ConfigSSRSLoadingMessageText()
                            TextAttrs:{Color:"#FFCCCCCC", Font:"Medium", HAlign:"HCenter", VAlign:"VCenter", Direction:"LeftToRight"}
                            TargetRect:{x:390,y:357,w:500,h:60}
                        }
                        ]
    Else

        'Set SD Canvas Items
        canvasItems = [
                        {
                            url:"pkg:/images/Theme/Title_Bar_SD.png"
                            TargetRect:{x:0,y:0,w:1280,h:122}
                        },
                        {
                            Text:ConfigSSRSLoadingMessageText()
                            TextAttrs:{Color:"#FFCCCCCC", Font:"Medium", HAlign:"HCenter", VAlign:"VCenter", Direction:"LeftToRight"}
                            TargetRect:{x:100,y:250,w:500,h:60}
                        }
                     ]

    End If

    'Set The Back Color
    screen.SetLayer(0,{Color:"#1b1d25", CompositionMode:"Source"})

    'Show Images Once All Are Downloaded
    screen.SetRequireAllImagesToDraw(true)

    'Set The Images
    screen.SetLayer(1,canvasItems)

    'Display The Screen
    screen.Show()

    'Close The Existing Splash Screen
    ScreenFacade.Close()

    'Return the Screen
    Return screen

End Function

'TODO - IS THIS USED?
REM ====================================================================
REM == NAME: ShowSplashScreen
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: None
REM == EXAMPLE CALL: ShowSplashScreen()
REM == DESCRIPTION: Creates a roImageCanvas that displays when SSRS
REM == is first loaded.
REM ====================================================================
'Sub ShowSplashScreen()

'    canvasItems = [
'                    { 
'                        url:"http://assets.c2mx.com/95786e665c4944f509ef1f860b7bfd03c8de75652c343167925e2d5d/static/screeners/images/poster/mm_icon_focus_hd.png"
'                        TargetRect:{x:100,y:100,w:400,h:300}
'                    },
'                    { 
'                        url:"http://assets.c2mx.com/95786e665c4944f509ef1f860b7bfd03c8de75652c343167925e2d5d/static/screeners/images/poster/mm_icon_focus_hd.png"
'                        TargetRect:{x:500,y:400,w:400,h:300}
'                    },
'                    { 
'                        Text:"Loading ScreenPlay Screening Room..."
'                        TextAttrs:{Color:"#FFCCCCCC", Font:"Medium",
'                        HAlign:"HCenter", VAlign:"VCenter",
'                        Direction:"LeftToRight"}
'                        TargetRect:{x:390,y:357,w:500,h:60}
'                    },
'    ]

'    canvas = CreateObject("roImageCanvas")
'    port = CreateObject("roMessagePort")
'    canvas.SetMessagePort(port)

'    'Set opaque background
'    canvas.SetLayer(0, {Color:"#FF0000", CompositionMode:"Source"})
'    canvas.SetRequireAllImagesToDraw(true)
'    canvas.SetLayer(1, canvasItems)
'    canvas.Show()

'    while(true)
'        msg = wait(0,port)

'        if type(msg) = "roImageCanvasEvent" then
'            if (msg.isRemoteKeyPressed()) then
'                i = msg.GetIndex()
'                print "Key Pressed - " ; msg.GetIndex()
'                if (i = 2) then
'                    ' Up - Close the screen.
'                    canvas.close()
'                end if
'            else if (msg.isScreenClosed()) then
'                print "Closed"
'                return
'            end if
'        end if
'    end while

'End Sub

