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
Sub MainSHIT()

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

        'DebugPrint("Authorization Key: " + ConfigReturnGenerateAuthenticationKeyURL())
        'DebugPrint("GET Playback: " + ConfigGetPlayBackPositionURL("188841"))
        'DebugPrint("SET Playback: " + ConfigSavePlayBackPositionURL("188841", "478"))

        'InitIDCodeScreen()

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

    'TODO - UNCOMMENT
    'Display Splash Screen A Bit Longer
    'sleep(1500)

    'TODO - UNCOMMENT
    ''Verify Roku Firmware Version 
    'If Not (ValidateRokuVersion()) Then

    '    'Display Error Message
    '    dialog = ConfigCreateMessageToUser("ROKUVERSION")

    '    'Exit Application
    '    GoTo SSRSExit

    'End If



    'Start Roku Validation
    StartValidation:

    'Set Flag
    isLoginSuccess = false
    
    ''Start Authentication
    'If(InitAuthenticationFUCK(screenFacade)) Then

    '    'Print To Debugger
    '    DebugPrint("Starting SSRS Channel")

    '    'Flip Flag
    '    isLoginSuccess = true

    'Else

    '    'Print To Debugger
    '    DebugPrint("Roku Validation Failed")

    'End If
    isValidated = false

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

                'Determine The User Imput Type
                If Type(MsgBox) = "roParagraphScreenEvent" Then
                    
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

                    End If
                End If
            End While
        End If
    End If

    'Look For Existance of ID Code
    If Not (ValidateRegistryValue(ConfigIDCodeSectionName())) Then

        'Display ID Code Screen
        isValidated = InitIDCodeScreen()

    End If

    'Check ID COde Status
    If Not(ValidateRegistryValue(ConfigPINSectionName())) Then

        'Display PIN Screen
        isValidated = InitPINScreen()

    Else
        DebugPrint("BLA BLA ERROR")
    End If





        'Set The Loop Point For Failed Email Entry
        'ResetEmail:

        ''Search For EMAIL ADDRESS
        'If Not (ValidateRegistryValue(ConfigEMAILSectionName())) Then

        '    'Display Email Screen
        '    isLoginSuccess = InitEmailScreen()

        '    'Clear Registry If Email Is Not Valid
        '    If Not(isLoginSuccess) Then 

        '        'Print To Debugger
        '        DebugPrint("Clear Email Address")

        '        'Clear Registry
        '        ClearRegistrationKey()

        '        'Go Back to Start Of Email Registration
        '        GoTo ResetEmail

        '    End If

        'End If

        ''Determine if PIN is entered in Registry
        'If Not ValidateRegistryValue(ConfigPINSectionName()) Then

        '    'Create PIN Screen if No PIN Found In Registry
        '    isLoginSuccess = InitPINScreen()

        '    'Restart Validation If Something Failed
        '    If Not (isLoginSuccess) Then

        '        'Start Over
        '        GoTo StartValidation

        '    End If

        'End If

        ''Determine if TOKEN is entered in Registry
        'If Not ValidateRegistryValue(ConfigTOKENSectionName()) Then

        '    'Call C2ME To Get Access Token and Hardware Auth Key
        '    aaAuthorization =  GenerateTokenAndHAuthKey()

        '    'Determine if An Error Happened
        '    If(aaAuthorization.accessToken = "ERROR") Then 

        '        'Flip Flag
        '        isLoginSuccess = false

        '    Else
                
        '        'Write Access Token Key To Regisry
        '        WriteToRegistry(ConfigTOKENSectionName(), aaAuthorization.accessToken)

        '        'Write HAUTH Key To Registry
        '        WriteToRegistry(ConfigHKEYSectionName(), aaAuthorization.hauth)

        '        'Set Flag
        '        isLoginSuccess = true

        '    End If

        'End If

        '' ''Show Error If Login Didn't Work
        ' ''If Not (isLoginSuccess) Then 

        ' ''    'Get Dialog Details
        ' ''    dialog = ConfigDisplayMessage("LOGINERROR",False)

        ' ''    'Show Dialog Message
        ' ''    dialog.Show()

        ' ''    'Determine If Dialog Was Created, If So Wait For Input
        ' ''    If Type(dialog) = "roMessageDialog" Then

        ' ''        'Show Dialog
        ' ''        While true

        ' ''            'Set Message Port And Wait for Input From User
        ' ''            MsgBox = wait(0, dialog.GetMessagePort()) 

        ' ''            'Determine Type of Input
        ' ''            If Type(MsgBox) = "roMessageDialogEvent"

        ' ''                'If User selects "OK" then Exit
        ' ''                If MsgBox.GetIndex() = 1

        ' ''                    'Clear Registry
        ' ''                    ClearRegistrationKey()

        ' ''                    'Close Dialog Box
        ' ''                    Exit While

        ' ''                End If
        ' ''            End If
        ' ''        End While
        ' ''    End If
        ' ''End If
        
    'End If 

    'If Validation Was Successful, Validate At C2ME
    If(isLoginSuccess) Then

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

                            'Flip Flag
                            isLoginSuccess = False

                            'Close Dialog Box
                            Exit While

                        End If
                    End If
                End While
            End If

        End If

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


