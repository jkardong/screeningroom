REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and creates the PIN Screen
REM ==
REM ********************************************************************
REM ********************************************************************




REM ====================================================================
REM == NAME:InitPINScreen
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Starts up the PIN number entry screen. A success on 
REM == entry returns TRUE. Failure returns FALSE
REM ====================================================================
Function InitPINScreen() As Boolean

    'Start PIN Screen
    StartPIN:

    'Print to Debugger
    DebugPrint("Initializing PIN Screen - [ PINScreen.InitPINScreen() ] ")

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Email Screen
    screen = CreateObject("roPinEntryDialog")

    'Set the Port to the Screen
    screen.SetMessagePort(port)

    'Set the Screen Title
    screen.SetTitle("Please Enter Supplied PIN")

    'Add Submit Button
    screen.AddButton(1, "SUBMIT")

    'Add Back Button
    screen.AddButton(2, "BACK")

    'Add Cancel Button
    screen.AddButton(3, "Cancel")

    'Set the Number of Entry Fields
    screen.SetNumPinEntryFields(6)

    'Display Screene
    screen.Show()

    'Wait For User to Enter PIN
    While True

        'Get Message Port 
        msg = wait(0, screen.GetMessagePort())

        'Begin Evaluation of Screen
        If type(msg) = "roPinEntryDialogEvent" Then

            'User Made A UI Selection
            If msg.isButtonPressed() Then

                'User Button Selections
                If (msg.GetIndex() = 1) Then '// User Selected SUBMIT

                    'Set The PIN To Variable
                    PIN = screen.Pin()

                    'Run Validation of input
                    If(ValidateInput(PIN, True, "PIN"))

                        'Print To Debugger
                        DebugPrint("User Selected SUBMIT With Value: " + PIN, true)

                        'Save The PIN To Registry
                        WriteToRegistry(ConfigPINSectionName(), PIN)

                        'Print Email Address to Debugger
                        DebugPrint("User Entered PIN: " + PIN, True)

                        'Return Valid
                        Return True

                        'Exit 
                        Exit While

                    Else '// Display Error To User

                        'Print To Debugger
                        DebugPrint("Invalid PIN Entered.",true)

                        'Display Error To User
                        DialogPINInvalid()

                        'Destroy From
                        screen.Close()

                        'Recall PIN
                        InitPINScreen()

                        'Return Validated (this has to be here as a catch)
                        Return True
                        
                    End If

                ElseIf (msg.GetIndex() = 2) then '// User Selected BACK

                    'Print To Debugger
                    DebugPrint("User Selected BACK", true)

                    'Close PIN Screen
                    screen.Close()

                    'Open ID CODE Screen
                    If(InitIDCodeScreen()) then
                        GoTo StartPIN
                    End If

                ElseIf (msg.GetIndex() = 3) then '// User Selected CANCEL

                    'Print To Debugger
                    DebugPrint("User Selected CANCEL", true)

                    'Show Dialog And Take Action Based On User Selection
                    If(DialogShowCancelSetup()) Then '// User Wants to Cancel

                        'Print To Debugger
                        DebugPrint("Clear Registry And Cancel Setup", true)

                        'Clear Registry
                        ClearRegistrationKey() 

                        'Return Status
                        Return False                       

                    End If '// End User Cancel

                End If '// End Index Selections

            End If '// End isButton Pressed

        End If '// End roPinEntryDialogEvent

    End While '// End PIN Wait

    'Return Error
    Return False

End Function





