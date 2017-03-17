REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: May 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Main entry point for SSRS.  This is where all actions
REM ==  start for the initialization of SSRS.
REM ==
REM ********************************************************************
REM ********************************************************************



REM ====================================================================
REM == NAME: Main
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: NONE
REM == DESCRIPTION: Main entry point for SSRS
REM ====================================================================
Sub Main()

    'Print to Debugger
    DebugPrint("   ")
    DebugPrint("   ")
    DebugPrint("************ STARTING SSRS ****************")
    DebugPrint("   ")

    'Create Login Object
    isLoginSuccess = True

    '/////////////////// TESTING GROUNDS ////////////////////////

        'Clear Out Registry
        'ClearRegistrationKey() 
       
        'Playing With Conversion
        'xyz = "1"
        'Print(xyz.toint() + 3)

        'DebugPrint(ConfigReturnSSRSResetURL())
        'DebugPrint(ConfigReturnErrorSaveWebServiceURL("no user found"))

        'DebugPrint(ConfigGetPlayBackPositionURL("12345"))
        'DebugPrint("Authorization Key: " + ConfigReturnGenerateAuthenticationKeyURL())
        'DebugPrint("GET Playback: " + ConfigGetPlayBackPositionURL("188841"))
        'DebugPrint("SET Playback: " + ConfigSavePlayBackPositionURL("188841", "478"))

        'SendError("Test")

        'Exit
        'GoTo SSRSExit

        'Skip to Studios Screen (bypass login)
        'GoTo ShowStudios

    '/////////////////// TESTING GROUNDS ////////////////////////

    'Print Start Time To Debugger
    PrintDateTime()

    'Create Splash Screen 
    screenFacade = ShowSplashBackground()

    'Display Splash Screen
    screenFacade.Show()
    
    'Initalize The Look and Feel
    InitializeTheme(False)

    'Check To Determine if Application Needs To Be Reset
    If(CheckForScreenersReset()) Then 

        'Get Login Error
        dialog = ConfigDisplayMessage("FORCERESET", False)

        'Get User Action
        userAction = HandleWait(dialog)

        'Determine if user Selected "OK" or "Cancel"
        If(userAction) then 

            'Print To Debugger
            DebugPrint("Resetting Screeners", True)

            'Renew The User Flag So Reset Won't Happen Again
            RenewUserResetFlag()

            'Clear Registry
            ClearRegistrationKey()

        Else '// User Selected "Cancel"

            'Print To Debugger
            DebugPrint("Exiting Screeners", True)

            'Exit SSRS
            End

        End If

    End If

    'Display Splash Screen A Bit Longer
    sleep(1500)

    'Start Roku Validation
    StartValidation:

    'Read From Registry to Determine if Roku is Valid Box
    If Not (ValidRoku()) Then

        'Create Splash Screen 
        splashScreen = InitSplashScreen(screenFacade)
      
        'Show Welcome Screen
        welcome = InitWelcomeScreen()

        'Wait For A User Action On Welcome Screen
        If Type(welcome) = "roParagraphScreen" Then

            'Wait For User Action
            While True

                'Set The Message Port
                MsgBox = wait(0, welcome.GetMessagePort())
 
                'Determine What Button A User Selected
                If (MsgBox.GetIndex()) = 1 Then '// User Selected "Let's Get Started"

                    'Close the Welcome Screen so user won't see again
                    welcome.Close()

                    'Exit And Continue
                    Exit While

                ElseIf (MsgBox.GetIndex()) = 2 Then '//User Selected "Cancel"

                    'Clear Anything From Registry
                    ClearRegistrationKey()

                    'Exit Application
                    GoTo SSRSExit

                End If '// End User Selection

            End While '// End User Action Wait
        End If '// End roParagraphScreen

        'Look For Existance of ID CODE
        If Not (ValidateRegistryValue(ConfigIDCodeSectionName())) Then

            'Display ID Code Screen
            isValidated = InitIDCodeScreen()

        Else 

            'Flig Flag
            isValidated = true

        End If '// End ID Code Logic

        'Check For Existance of PIN
        If Not(ValidateRegistryValue(ConfigPINSectionName())) Then

            'Display PIN Screen
            isValidated = InitPINScreen()

        Else 

            'Flig Flag
            isValidated = true

        End If '//End PIN Logic

        'Pair Roku Device to User
        If(isValidated) then

            'Print To Debugger
            DebugPrint("Roku Passes Validation, Calling Pairing of Device.")

            'Pair with C2MX
            isValidated = PairAndRegisterDevice(GetRegistryValue(ConfigPINSectionName()),GetRegistryValue(ConfigIDCodeSectionName()))
 
        Else

            DebugPrint("FUCK")

        End If '// End Pairing 

        'Call Register and Verify Roku
        If(isValidated) then

            'Print To Debugger
            DebugPrint("Pairing Success. Generate HKA and UAT keys.")

            'Generate HKA and UAT
            aaAuthorization =  GenerateHardwareKeyAuthorization()

            'Write UAT To Registry
            WriteToRegistry(ConfigUATSectionName(), aaAuthorization.user_api_token)

            'Write HKA Key To Registry
            WriteToRegistry(ConfigHKASectionName(), aaAuthorization.hardware_key_authentication)

            'Set Flag
            isLoginSuccess = true

        End If

    Else '// Roku Is Valid

        'Flip Flag
        isValidated = true
        isLoginSuccess = isValidated

    End If '//End Valid Roku

   'If Validation Was Successful, Validate At C2ME
    If(isLoginSuccess) Then

        'Print To Debugger
        DebugPrint("Roku Validation Success. Calling Login To C2MX.")

        'Registry Has Values, Verify With C2ME And Set Session Token
        If Not (ValidateLogin()) Then 

            'Get Login Error
            dialog = ConfigDisplayMessage("LOGINERROR", False)

            'Show Dialog To User
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

                            'Clear Registry
                            ClearRegistrationKey() 

                            'Flip Flag
                            isLoginSuccess = False

                            'Close Dialog Box
                            Exit While

                        End If '// End Get Index
                    End If '// End roMessageDialogEvent
                End While '// End Wait for User 
            End If '// End roMessageDialog

        End If '// End ValidateLogin()

    Else '// Login Error

        'Show Error
        DialogLoginError()

    End If

    'Studios Screen
    ShowStudios:

    'Roku and User Pass Validation, Show Studios Screen
    If(isLoginSuccess) Then 

        'Create Home Screen
        StudiosScreen = InitStudiosScreen()

        'Display Home Screen
        ShowStudiosScreen(StudiosScreen)

    End If

    'End of Main
     SSRSExit:

    DebugPrint("   ")
    DebugPrint("************ SSRS OVER AND OUT ************")
    DebugPrint("   ")
    DebugPrint("   ")

End Sub

