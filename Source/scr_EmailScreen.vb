REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Creates the Email Entry Screen and records the 
REM ==  email address to registry on user submit
REM ==
REM ********************************************************************
REM ********************************************************************




REM ====================================================================
REM == NAME:InitEmailScreen
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Starts up the email screen and returns a status of
REM == true or false depending on user action.
REM ====================================================================
Function InitEmailScreen(IsNameAndEmailSetup = false) As Boolean

    'Print to Debugger
    DebugPrint("Initializing Email Screen - [ EmailScreen.InitEmailScreen() ]")

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Email Screen
    screen = CreateObject("roKeyboardScreen")

    'Set the Port to the Screen
    screen.SetMessagePort(port)

    'Set the Screen Title
    screen.SetTitle("Please Enter Email Address")

    'TODO - REMOVE
    screen.SetText("jay@screenplaylabs.com")
    'screen.SetText("sam.kirk@screenplayinc.com")
    'screen.SetText("mark.vrieling@screenplayinc.com")
    'screen.SetText("akono@image-entertainment.com")
    'screen.SetText("akono@image-entertainment.com")

    'Set Instructions
    screen.SetDisplayText("Enter Your Approved Email Address And Select NEXT below")

    'Set The Max Length Of Input
    screen.SetMaxLength(50)

    'Determine Which Action To Print
    If(IsNameAndEmailSetup) then

        'Add FINISH Button
        screen.AddButton(1, "FINISH")

        'Add BACK Button
        screen.AddButton(2, "BACK")

        'Set email if exist
        screen.SetText(GetRegistryValue(ConfigEMAILSectionName()))

    Else

        'Add NEXT Button
        screen.AddButton(1, "NEXT")

    End If

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

            ElseIf msg.isButtonPressed() Then 'NEXT/FINISH button is selected by user

                'If User Selects NEXT/FINISH Take Action
                If msg.GetIndex() = 1 Then

                    'Set The Email Address To Variable
                    emailAddress = screen.GetText()

                    'Validate Email Address
                    If(ValidateEmailAddress(emailAddress)) Then

                        'Save The Email To Registry
                        WriteToRegistry(ConfigEMAILSectionName(), emailAddress)

                        'Print Email Address to Debugger
                        DebugPrint("User Entered Email Address:" + emailAddress, True)

                        'Return Valid Entry
                        Return True

                    Else '//Invalid Email Address

                        'Print To Debugger
                        DebugPrint("Invalid Email Address", True)

                        'Display Message To User
                        dialog = ConfigDisplayMessage("INVALIDEMAIL", False)

                        'Show Dialog
                        dialog.Show()

                        'Wait For User Interaction
                        While true

                            'Wait For User To Click
                            MsgBox = wait(0, dialog.GetMessagePort()) 

                            'If User Makes a UI Selection
                            If Type(MsgBox) = "roMessageDialogEvent"

                                'If User Select OK, Take Action
                                If MsgBox.GetIndex() = 1

                                    'Print To Debugger
                                    DebugPrint("Error Displayed To User")

                                    'Exit
                                    Return False 

                                End If
                            End If
                        End While

                    End If

                'User Selected BACK 
                ElseIf (msg.GetIndex() = 2) Then

                    'Save The Email To Registry
                    WriteToRegistry(ConfigEMAILSectionName(), emailAddress)

                    'Print Email Address to Debugger
                    DebugPrint("User Entered Email Address:" + emailAddress, True)

                    'Close Last Name Screen
                    screen.close()

                    'Go Back To First Name Screen
                    InitLastNameScreen(true)

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













