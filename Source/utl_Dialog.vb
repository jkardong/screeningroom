REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2012
REM ==  Created: January 2012
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes dialogs for UI
REM ==
REM ********************************************************************
REM ********************************************************************



REM ====================================================================
REM == NAME:ShowCancelSetup
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Displays Login Error Dialog to user
REM ====================================================================
Sub DialogLoginError()

    'Print to Debugger
    DebugPrint("Display Login Error Dialog", true)
    
    'Display Message To User
    dialog = ConfigDisplayMessage("LOGINERROR", False)

    'Show Message
    dialog.Show()

    'Determine If Dialog Was Created, If So Wait For Input
    If Type(dialog) = "roMessageDialog" Then

        'Show Dialog
        While true

            'Set Message Port And Wait for Input From User
            MsgBox = wait(0, dialog.GetMessagePort()) 

            'Determine Type of Input
            If Type(MsgBox) = "roMessageDialogEvent"

                'If User selects "Cancel Setup" then Exit
                If MsgBox.GetIndex() = 1

                    'Close Dialog
                    dialog.Close()

                    'Exit
                    Exit While 

                End If
            End If
        End While
    End If
End Sub

REM ====================================================================
REM == NAME:DialogIDCodeError
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Displays ID Code Error Dialog to user
REM ====================================================================
Sub DialogIDCodeError()

    'Print to Debugger
    DebugPrint("Display Login Error Dialog", true)
    
    'Display Message To User
    dialog = ConfigDisplayMessage("IDCODELENGTH", False)

    'Show Message
    dialog.Show()

    'Determine If Dialog Was Created, If So Wait For Input
    If Type(dialog) = "roMessageDialog" Then

        'Show Dialog
        While true

            'Set Message Port And Wait for Input From User
            MsgBox = wait(0, dialog.GetMessagePort()) 

            'Determine Type of Input
            If Type(MsgBox) = "roMessageDialogEvent"

                'If User selects "Cancel Setup" then Exit
                If MsgBox.GetIndex() = 1

                    'Close Dialog
                    dialog.Close()

                    'Exit
                    Exit While 

                End If
            End If
        End While
    End If
End Sub

REM ====================================================================
REM == NAME:DialogShowCancelSetup
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Displays Cancel Dialog to user
REM ====================================================================
Function DialogShowCancelSetup() As Boolean

    'Print to Debugger
    DebugPrint("Display Cancel Setup Dialog", true)
    
    'Display Message To User
    dialog = ConfigDisplayMessage("CANCELSETUP", False)

    'Show Message
    dialog.Show()

    'Determine If Dialog Was Created, If So Wait For Input
    If Type(dialog) = "roMessageDialog" Then

        'Show Dialog
        While true

            'Set Message Port And Wait for Input From User
            MsgBox = wait(0, dialog.GetMessagePort()) 

            'Determine Type of Input
            If Type(MsgBox) = "roMessageDialogEvent"

                'If User selects "Cancel Setup" then Exit
                If MsgBox.GetIndex() = 1

                    'Close Dialog
                    dialog.Close()

                    'Close Dialog Box
                    Return True

                ElseIf (MsgBox.GetIndex() = 2)'// "Return To Setup"

                    'Close Dialog
                    dialog.Close()

                    'Exit
                    Return False 

                End If

            End If

        End While

    End If

    'Return Default Status
    Return False

End Function

REM ====================================================================
REM == NAME:DialogPINInvalid
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Displays PIN Error Dialog to user
REM ====================================================================
Function DialogPINInvalid()

    'Print to Debugger
    DebugPrint("Display PIN Error Dialog", true)

    'Display Message To User
    dialog = ConfigDisplayMessage("PININVALID", False)

    'Show Message
    dialog.Show()

    'Determine If Dialog Was Created, If So Wait For Input
    If Type(dialog) = "roMessageDialog" Then

        'Show Dialog
        While true

            'Set Message Port And Wait for Input From User
            MsgBox = wait(0, dialog.GetMessagePort()) 

            'Determine Type of Input
            If Type(MsgBox) = "roMessageDialogEvent"

                'If User selects "OK" then Exit
                If MsgBox.GetIndex() = 1

                    'Return Status
                    Return True

                    'Close Dialog Box
                    Exit While

                End If
            End If
        End While
    End If


End Function

REM ====================================================================
REM == NAME:DialogPairingFailed
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Displays error to user if Pairing Failed
REM ====================================================================
Function DialogPairingFailed()

    'Print to Debugger
    DebugPrint("Display Pairing Error Dialog", true)

    'Display Message To User
    dialog = ConfigDisplayMessage("PAIRFAIL", False)

    'Show Message
    dialog.Show()

    'Determine If Dialog Was Created, If So Wait For Input
    If Type(dialog) = "roMessageDialog" Then

        'Show Dialog
        While true

            'Set Message Port And Wait for Input From User
            MsgBox = wait(0, dialog.GetMessagePort()) 

            'Determine Type of Input
            If Type(MsgBox) = "roMessageDialogEvent"

                'If User selects "OK" then Exit
                If MsgBox.GetIndex() = 1

                    'Return Status
                    Return True

                    'Close Dialog Box
                    Exit While

                End If
            End If
        End While
    End If


End Function