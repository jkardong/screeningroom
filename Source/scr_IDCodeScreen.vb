REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2012
REM ==  Created: January 2012
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and creates the ID Code Screen
REM ==
REM ********************************************************************
REM ********************************************************************



REM ====================================================================
REM == NAME:InitIDCodeScreen
REM == INPUT PARAMETERS: 
REM == OUTPUT: 
REM == DESCRIPTION: 
REM == CALLED FROM:
REM ====================================================================
Function InitIDCodeScreen() As Boolean

    'Print to Debugger
    DebugPrint("Initializing ID Code Screen - [ IDCodeScreen.InitIDCodeScreen() ]")

    'Start Process
    StartIDCode:

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Email Screen
    screen = CreateObject("roKeyboardScreen")

    'Set the Port to the Screen
    screen.SetMessagePort(port)

    'Set the Screen Title
    screen.SetTitle("Please Enter ID Code")

    'Set the text to be hidden
    screen.setSecureText(true)

    'Set Instructions
    screen.SetDisplayText("ID CODE")

    'Set The Max Length Of Input
    screen.SetMaxLength(8)

    'Add FINISH Button
    screen.AddButton(1, "NEXT")

    'Display Screen
    screen.Show()

    'Wait For User to Enter Email Address
    While True

        'Get Message Port 
        msg = wait(0, screen.GetMessagePort())

        'Begin Evaluation of Screen
        If type(msg) = "roKeyboardScreenEvent" Then

            'If Screen Closed, Exit
            If msg.isScreenClosed() Then

                'Exit With Error
                Return False

            ElseIf msg.isButtonPressed() Then 'NEXT button is selected by user

                'If User Selects NEXT Take Action
                If msg.GetIndex() = 1 Then

                    'Set The ID Code To Variable
                    idCode = screen.GetText()

                    'Validate ID CODE
                    If(ValidateInput(idCode)) Then

                        'Save The Email To Registry
                        WriteToRegistry(ConfigIDCodeSectionName(), idCode)

                        'Print Email Address to Debugger
                        DebugPrint("User Entered ID Code: " + idCode, True)

                        'Return Valid Entry
                        Return True

                    Else '// Input was not valid

                        'Print To Debugger
                        DebugPrint("Invalid ID CODE entered.", true)

                        'Show Dialog Error
                        DialogIDCodeError()

                        'Close Screen
                        screen.Close()

                        'Restart Screen
                        InitIDCodeScreen()

                    End If

                'User Selected BACK 
                ElseIf (msg.GetIndex() = 2) Then

                    'Print To Debugger
                    DebugPrint("User Selected BACK.", true)

                    'Create Splash Screen 
                    screenFacade = ShowSplashBackground()

                    'Close ID Code Screen
                    screen.close()

                    'Go Back To First Name Screen
                    InitSplashScreen(screenFacade)

                Else '// User Selected CANCEL

                    'Clear Registry
                    ClearRegistrationKey()

                    'Exit Application
                    End

                End If
            End If
        End If
    End While

    'Return Error
    Return False

End Function
