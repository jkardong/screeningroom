REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2012
REM ==  Created: January 2012
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and creates the user First name screen
REM ==
REM ********************************************************************
REM ********************************************************************


REM ====================================================================
REM == NAME:InitFirstNameScreen
REM == INPUT PARAMETERS: 
REM ==  IsBackSelected = If user is going back to this form from the 
REM ==  LAST NAME screen, repopulate with FIRST NAME
REM == OUTPUT: True or False
REM == DESCRIPTION: Creates and displays the First Name screen for users
REM == validated against a Serial Number.
REM ====================================================================
Function InitFirstNameScreen(IsBackSelected = false) As Boolean

    'Print to Debugger
    DebugPrint("Initializing First Name Screen - [ FirstNameScreen.InitFirstNameScreen() ]", True)

    'Starting Point 
    StartFirstName:

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Email Screen
    screen = CreateObject("roKeyboardScreen")

    'Set the Port to the Screen
    screen.SetMessagePort(port)

    'If User Selected BACK from LAST NAME, get FIRST NAME from Registry
    If(IsBackSelected) then

        'Get Value From Registry
        screen.SetText(GetRegistryValue(ConfigFirstName()))

    Else

        'TODO - REMOVE
        'screen.SetText("Jay")

    End If

    'Set the Screen Title
    screen.SetTitle("Please Enter Your FIRST NAME")

    'Set Instructions
    screen.SetDisplayText("FIRST NAME")

    'Set The Max Length Of Input
    screen.SetMaxLength(50)

    'Add Next Button
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

            ElseIf msg.isButtonPressed() Then '// NEXT button is selected by user

                'If User Selects NEXT Take Action
                If msg.GetIndex() = 1 Then

                    If(ValidateInput(screen.GetText())) then
                        
                        'Save The Email To Registry
                        WriteToRegistry(ConfigFirstName(), screen.GetText())

                        'Print Email Address to Debugger
                        DebugPrint("User First Name: " + screen.GetText(), True)

                        'Initalize Last Name Screen
                        InitLastNameScreen()

                        'Return Valid Entry
                        Return True

                    Else '// Email Was Invalid

                        'Print To Debugger
                        DebugPrint("First Name Failed Validation",true)

                        'Display Message To User
                        dialog = ConfigDisplayMessage("INVALIDFIRSTNAME", False)

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
                                    DebugPrint("Error Displayed To User", True)

                                    'Restart Form
                                    GoTo StartFirstName

                                End If
                            End If
                        End While

                    End If

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