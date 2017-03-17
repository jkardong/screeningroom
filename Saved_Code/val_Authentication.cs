'REM ********************************************************************
'REM ********************************************************************
'REM ==
'REM ==  ScreenPlay Screening Room
'REM ==  Author: J Kardong
'REM ==  Copyright: ScreenPlay Labs 2012
'REM ==  Created: January 2012
'REM ==  BrightScript Version: 3.0
'REM ==  Description: Initalizes all Authentication of Roku box and user
'REM ==
'REM ********************************************************************
'REM ********************************************************************

                           

'REM ====================================================================
'REM == NAME:InitAuthentication
'REM == INPUT PARAMETERS: NONE
'REM == OUTPUT: True or False
'REM == DESCRIPTION: Main entry point for all authentication and user
'REM == authorization.
'REM ====================================================================
'Function InitAuthentication(ScreenFacade As Object) As Boolean

'    'Print To Debugger
'    DebugPrint("Starting Authentication - [ Authentication.InitAuthentication() ]")

'    'Determine if HKA and UAT values are found in Roku Registry
'    If Not (ValidRoku()) Then

'        'Print to Debugger
'        DebugPrint("No HKA/UAT Found In Registry. Starting Authentication Process.", True)

'        'Create Splash Screen 
'        splashScreen = InitSplashScreen(ScreenFacade)
      
'        'Show Welcome Screen
'        welcome = InitWelcomeScreen()

'        'Wait For A User Action On Welcome Screen
'        If Type(welcome) = "roParagraphScreen" Then

'            'Wait For User Action
'            While True

'                'Set The Message Port
'                MsgBox = wait(0, welcome.GetMessagePort())

'                'Determine The User Imput Type
'                If Type(MsgBox) = "roParagraphScreenEvent" Then
                    
'                    'Determine What Button A User Selected
'                    If (MsgBox.GetIndex()) = 1 Then '// User Selected "Let's Get Started"

'                        'Close the Welcome Screen so user won't see again
'                        welcome.Close()

'                        'Open ID CODE Screen
'                        If(InitIDCodeScreen()) Then

'                            'Open PIN Screen
'                            If(InitPINScreen()) Then

'                                'Run Validation To C2MX
'                                If(PairAndRegisterDevice(GetRegistryValue(ConfigPINSectionName()),GetRegistryValue(ConfigIDCodeSectionName()))) then

'                                    'Print To Debugger
'                                    DebugPrint("Pairing Was Successful. OK to start SSRS Channel.", true)

'                                    'Return Everything is Ok
'                                    Return True

'                                Else '// Pairing Falure

'                                    DebugPrint("Hit1")

'                                    'Return Failure
'                                    Return False

'                                End If '//Valid Login

'                            Else '// PIN Failure
'                                DebugPrint("Hit2")
'                                'Return Failure
'                                Return False

'                            End If '// PIN OK

'                        Else '// ID Code Failure
'                            DebugPrint("Hit3")
'                            'Return Failure
'                            Return False

'                        End If '// ID Code OK
'                        DebugPrint("Hit4")
'                        'Exit And Continue
'                        Exit While

'                    ElseIf (MsgBox.GetIndex()) = 2 Then '//User Selected "Cancel"

'                        'Clear Anything From Registry
'                        ClearRegistrationKey()

'                        'Return False
'                        Return False

'                    End If '// End Index Selection
'                End If '// End roParagraphScreenEvent
'            End While '// End While Waiting for user Action

'            DebugPrint("Fuck")

'        End If '// End roParagraphScreen

'    Else '// HKA Found in Registry meaning box has been validated

'        'Print to Debugger
'        DebugPrint("HKA Found In Registry. OK to Start SSRS Channel", True)

'        'Roku Is Ok
'        Return true

'    End If

'End Function
