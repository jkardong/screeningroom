REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and creates a Welcome Screen
REM ==
REM ********************************************************************
REM ********************************************************************



REM ====================================================================
REM == NAME: InitWelcomeScreen
REM == INPUT PARAMETERS: 
REM == OUTPUT: Returns roParagraphScreen
REM == DESCRIPTION: Creates and sets the text for the initial Welcome
REM == screen that users see on the first time they log into SSRS
REM ====================================================================
Function InitWelcomeScreen() As Object

    'Print to Debugger
    DebugPrint("Initializing Welcome - [ WelcomeScreen.InitWelcomeScreen() ]")

    'Create Message Port
    port = CreateObject("roMessagePort")

    'Create Parapraph Screen
    screen = CreateObject("roParagraphScreen")

    'Set The Message Port
    screen.SetMessagePort(port)

    'Populate the Screen Text
    screen = ConfigSetMultiLineScreenText(screen, "WELCOME")
    
    'Display The Screen
    screen.Show()

    'Return the Screen
    Return screen

End Function

