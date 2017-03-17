REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2012
REM ==  Created: January 2012
REM ==  BrightScript Version: 3.0
REM ==  Description: Wrapper for Hardware Authentication Key style
REM ==  of login.
REM ==
REM ********************************************************************
REM ********************************************************************


REM ====================================================================
REM == NAME:InitalizeAuthenticationKey
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Determines if the Roku is to be set up for a Hardware
REM == Authentication Key (HAK) or if it is not needed. 
REM == 
REM == FALSE = No HAK needed
REM == TRUE = Authorize Roku Box For HAK
REM ====================================================================
Function InitalizeAuthenticationKey() As Boolean

    'Print To Debugger
    DebugPrint("Initializing Hardware Authentication Check - [ HardwareAuthenticationKeyLogin.InitalizeAuthenticationKey() ]")

    'Call Web Service To Determine If A Authentication Key Is Needed
    If(GetAuthenticationKeyStatus()) Then

        'Print To Debugger
        DebugPrint("Initalizing User Name and Email Authentication",true)

        'Show Welcome Screen
        welcome = InitWelcomeScreen(true)

        'Wait For A User Action On Welcome Screen
        If Type(welcome) = "roParagraphScreen" Then

            'Wait For User Action
            While True

                'Set The Message Port
                MsgBox = wait(0, welcome.GetMessagePort())

                'Determine The User Imput Type
                If Type(MsgBox) = "roParagraphScreenEvent" Then
                    
                    'Determine What Button A User Selected
                    If (MsgBox.GetIndex()) = 1 Then '// User Selected "Let's Get Started"

                        'Initialize First Name Screen
                        InitFirstNameScreen()

                        'Close the Welcome Screen so user won't see again
                        welcome.Close()

                        'Exit And Continue
                        Exit While

                    ElseIf (MsgBox.GetIndex()) = 2 Then '//User Selected "Cancel"

                        'Clear Anything From Registry
                        ClearRegistrationKey()

                        'Return No Action
                        Return False

                    End If
                End If
            End While
        End If

    End If

    'Return Default
    Return False

End Function