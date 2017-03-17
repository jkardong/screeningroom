REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2012
REM ==  Created: January 2012
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and creates the user Last Name screen
REM ==
REM ********************************************************************
REM ********************************************************************

REM ====================================================================
REM == NAME:InitLastNameScreen
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Creates and displays the Last Name screen for users
REM == validated against a Serial Number.
REM ====================================================================
Function InitLastNameScreen(IsBackSelected = false) As Boolean

    'Print to Debugger
    DebugPrint("Initializing Last Name Screen - [ LastNameScreen.InitLastNameScreen() ]", True)

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Email Screen
    screen = CreateObject("roKeyboardScreen")

    'Set the Port to the Screen
    screen.SetMessagePort(port)

    'If User Selected BACK from LAST NAME, get LAST NAME from Registry
    If(IsBackSelected) then

        'Get Value From Registry
        screen.SetText(GetRegistryValue(ConfigLastName()))

    Else

        'TODO - REMOVE
        'screen.SetText("Kardong")

    End If

    'Set the Screen Title
    screen.SetTitle("Please Enter Your LAST NAME")

    'Set Instructions
    screen.SetDisplayText("LAST NAME")

    'Set The Max Length Of Input
    screen.SetMaxLength(50)

    'Add Next Button
    screen.AddButton(1, "NEXT")

    'Add Back Button
    screen.AddButton(2, "BACK")

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

                'User Selected NEXT
                If (msg.GetIndex() = 1) Then

                    'Save The Email To Registry
                    WriteToRegistry(ConfigLastName(), screen.GetText())

                    'Print Email Address to Debugger
                    DebugPrint("User Last Name: " + screen.GetText(), True)

                    'Search For EMAIL ADDRESS
                    If Not (ValidateRegistryValue(ConfigEMAILSectionName())) Then

                        'Display Email Screen
                        isLoginSuccess = InitEmailScreen(true)

                        'Clear Registry If Email Is Not Valid
                        If Not(isLoginSuccess) Then 

                            'Print To Debugger
                            DebugPrint("Clear Email Address")

                            'Clear Registry
                            ClearRegistrationKey()

                        End If

                    End If

                    'Return Valid Entry
                    Return True

                'User Selected BACK 
                ElseIf (msg.GetIndex() = 2) Then

                    'Close Last Name Screen
                    screen.close()

                    'Go Back To First Name Screen
                    InitFirstNameScreen(true)

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