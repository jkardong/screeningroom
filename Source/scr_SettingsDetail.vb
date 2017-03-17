REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: Sept 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and sets up the Settings Detail Screen
REM ==
REM ********************************************************************
REM ********************************************************************

REM ====================================================================
REM == NAME: DisplaySettingsDetailScreen()
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: Returns roParagraphScreen
REM == DESCRIPTION: Initalizes the Settings Detail Screen 
REM ====================================================================
Function DisplaySettingsDetailScreen() As Object

    'Print to Debugger
    print("Initalize Settings Detail Screen - [ scr_SettingsDetail.DisplaySettingsDetailScreen() ]")

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Screen
    screen = CreateObject("roParagraphScreen")

    'Set The Port to the Screen
    screen.SetMessagePort(port)

    'Return The Screen
    Return Screen

End Function

