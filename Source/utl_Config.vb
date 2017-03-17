REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Global values are set here.  Treat as a web.config
REM ==  file as that values can be set here.
REM ==
REM ********************************************************************
REM ********************************************************************




REM ====================================================================
REM == NAME: Config<area>Name/Text
REM == OUTPUT: Returns string value
REM == DESCRIPTION: Static values used throughout SSRS
REM ====================================================================
Function ConfigRegistryKeyName() As String
    Return "ScreenPlay_SSRS"
End Function
Function ConfigEMAILSectionName() As String
    Return "EMAILADDRESS"
End Function
Function ConfigPINSectionName() As String
    Return "PIN"
End Function
Function ConfigIDCodeSectionName() As String
    Return "IDCODE"
End Function
Function ConfigUATSectionName() As String
    Return "UAT"
End Function
Function ConfigHKASectionName() As String
    Return "HKA"
End Function
Function ConfigSessionTokenName() As String
    Return "SESSION"
End Function
Function ConfigStudioSessionName() As String
    Return "STUDIOSESSION"
End Function
Function ConfigScreenersSerialNumber() As String
    Return "SERIALNUMBER"
End Function
Function ConfigFirstName() As String
    Return "FIRSTNAME"
End Function
Function ConfigLastName() As String
    Return "LASTNAME"
End Function
Function ConfigSSRSName() As String
    Return "ScreenPlay Screening Room"
End Function
Function ConfigSettingsText() As String
    Return "Settings"
End Function
Function ConfigSSRSDefaultMessageText() As String
    Return "ScreenPlay Screening Room Service Message"
End Function
Function ConfigSSRSDefaultButtonText() As String
    Return "OK"
End Function
Function ConfigSSRSLoadingMessageText() As String
    Return "Loading..."
End Function
Function ConfigSSRSWelcomeText() As String
    Return "WELCOME!!"
End Function
Function ConfigSSRSThemeType() As String
    Return "flat-category"
End Function
Function ConfigMovieThemeType() As String
    Return "arced-portrait"
End Function
Function ConfigSSRSCMSWebsiteURL() As String
    Return "http://ssrs.screenplaylabs.com:8080/"
End Function
Function ConfigSSRSVersion() As Float
    Return 3.0
End Function
Function ConfigSSRSSupportEmailAddress() As String
    Return "FRANCISCO.MICHELENA@SCREENPLAYINC.COM"
End Function
Function ConfigGetRokuSerialNumber() As String
    Return (CreateObject("roDeviceInfo")).GetDeviceUniqueId()
End Function
Function ConfigGetSettingsText() As String
    Return("This screen allows you to perform specific user actions related to the ScreenPlay Screening Room. Please select your option from the selections below.")
End Function
Function ConfigRegisterAndVerifyRoku() As String
    Return ConfigC2ME_URL() + "register/ema=" + GetRegistryValue(ConfigEMAILSectionName()) + "/pin=" + GetRegistryValue(ConfigPINSectionName()) + "/hak=" + ConfigGetRokuSerialNumber()
    REM http://sandbox.c2mx-spl.appspot.com/roku/register/ema=mat@screenplaylabs.com/pin=Ft9RzZJYgk/hak=13A17R030705
End Function
Function ConfigReturnVerifyUserWebServiceURL() As String
    Return ConfigC2ME_URL() + "login/hak=" + ConfigGetRokuSerialNumber() + "/uat=" + GetRegistryValue(ConfigUATSectionName()) + "/hka=" + GetRegistryValue(ConfigHKASectionName())
    REM http://sandbox.c2mx-spl.appspot.com/roku/login/hak=13A17R030705/uat=682df45c8eeb9a752e1c2214b79accf9f692c88c7446850094cf2412/hka=65585F194755174D5559175F19085B4141
End Function
Function ConfigReturnRegisterAndVerifyRokuWebServiceURL() As String
    Return ConfigC2ME_URL() + "register/ema=" + GetRegistryValue(ConfigEMAILSectionName()) + "/pin=" + GetRegistryValue(ConfigPINSectionName()) + "/hak=" + ConfigGetRokuSerialNumber()
    REM http://sandbox.c2mx-spl.appspot.com/roku/register/ema=jay@screenplaylabs.com/pin=I1MTYY/hak=13A17R030705
End Function
Function ConfigReturnStudioScreenWebServiceURL() As String
    Return ConfigC2ME_URL() + "poster/uat=" + GetRegistryValue(ConfigUATSectionName()) + "/hak=" + ConfigGetRokuSerialNumber() + "/hka=" + GetRegistryValue(ConfigHKASectionName()) + "/lit=" + GetRegistryValue(ConfigSessionTokenName())
    REM http://sandbox.c2mx-spl.appspot.com/roku/poster/uat=a1a4027289757634ff3a70190ac39361c8693451d79e30c3c06602e4/hak=10:93:e9:0d:9d:82/hka=505F5F57475204564F5E1052580B5F4D46/lit=535F545F59585642475D205A535555424E5C514F5E5D404C5852
End Function
Function ConfigReturnMediaScreenWebServiceURL() As String
    Return ConfigC2ME_URL() + "content/uat=" + GetRegistryValue(ConfigUATSectionName()) + "/hak=" + ConfigGetRokuSerialNumber() + "/hka=" + GetRegistryValue(ConfigHKASectionName()) + "/stk=" 
    REM http://sandbox.c2mx-spl.appspot.com/roku/content/uat=682df45c8eeb9a752e1c2214b79accf9f692c88c7446850094cf2412/hak=13A17R030705/hka=655B2411433D42475F5E4350/stk=aghjMm14LXNwbHIPCxIHQ29tcGFueRiD7gIM1494D
End Function
Function ConfigReturnGenerateAuthenticationKeyURL() As String
    Return ConfigC2ME_URL() + "pair/hak=" + ConfigGetRokuSerialNumber()
    REM http://sandbox.c2mx-spl.appspot.com/roku/pair/hak=1234
End Function
Function ConfigSavePlayBackPositionURL(ScreenplayID As String, PlayBackPosition As String)
    Return ConfigGetPlaybackPositionURL(ScreenplayID) + "/pos=" + PlayBackPosition
    REM http://sandbox.c2mx-spl.appspot.com/roku/position/uat=2caf2c80ac5ef6be7d47c8850b2eb9b6ede033a48f450ebb3957e932/spi=188851/hak=12345678/hka=655A56144159454C/pos=5432
End Function
Function ConfigGetPlayBackPositionURL(ScreenplayID As String)
    Return ConfigC2ME_URL() + "position/uat=" + GetRegistryValue(ConfigUATSectionName()) + "/spi=" + ScreenplayID + "/hak=" + ConfigGetRokuSerialNumber() + "/hka=" + GetRegistryValue(ConfigHKASectionName())
    REM http://sandbox.c2mx-spl.appspot.com/roku/position/uat=2caf2c80ac5ef6be7d47c8850b2eb9b6ede033a48f450ebb3957e932/spi=188851/hak=12345678/hka=655A56144159454C
End Function
Function ConfigReturnPairingURL(PIN As String, IDCode As String)
    Return ConfigC2ME_URL() + "pairing/pin=" + PIN + "/idc=" + IDCode + "/hak=" + ConfigGetRokuSerialNumber()
    REM http://sandbox.c2mx-spl.appspot.com/roku/pairing/pin=424815/idc=3T9UYP/hak=13A16M005826
End Function
Function ConfigReturnErrorSaveWebServiceURL(ErrorMessage As String)
    Return ConfigC2ME_URL() + "err/msg=" + ErrorMessage + "/ema=" + GetRegistryValue(ConfigEMAILSectionName()) + "/uat=" + GetRegistryValue(ConfigUATSectionName()) + "/hak=" + GetRegistryValue(ConfigUATSectionName()) + "/hka=" + GetRegistryValue(ConfigHKASectionName())
    REM http://sandbox.c2mx-spl.appspot.com/roku/err/msg=TEST/ema=mat@miga/uat=2caf2c80ac5ef6be7d47c8850b2eb9b6ede033a48f450ebb3957e932/hak=12345678/hka=12345678
End Function
Function ConfigClearGoogleURL() As String
    Return ConfigC2ME_URL() + "deleteuser/ema=" + GetRegistryValue(ConfigEMAILSectionName()) 
    REM http://5.c2mx-spl.appspot.com/roku/deleteuser/ema=jay@screenplaylabs.com
End Function
Function ConfigReturnSSRSResetURL() As String
    Return ConfigC2ME_URL() + "reset/uat=" + GetRegistryValue(ConfigUATSectionName()) + "/hak=" + ConfigGetRokuSerialNumber()
    REM http://sandbox.c2mx-spl.appspot.com/roku/reset/uat=2caf2c80ac5ef6be7d47c8850b2eb9b6ede033a48f450ebb3957e932/hak=12345678/hka=12345678    
    REM Return "http://www.sadgravity.com/SSRS/Reset/Default.Aspx?eml=" + GetRegistryValue(ConfigEMAILSectionName()) + "&hak=" + ConfigGetRokuSerialNumber()
    REM http://www.sadgravity.com/SSRS/Reset/Default.Aspx?eml=jay@screenplaylabs.com&hak=123456789
End Function
Function ConfigRenewResetFlagURL() As String
    Return "http://www.sadgravity.com/SSRS/Renew/Default.aspx?eml=" + GetRegistryValue(ConfigEMAILSectionName())
    REM http://www.sadgravity.com/SSRS/Renew/Default.aspx?eml=jay@screenplaylabs.com
End Function
Function ConfigC2ME_URL() As String 
    REM Return "http://dev.c2mx-spl.appspot.com/roku/"
    Return "http://sandbox.c2mx-spl.appspot.com/roku/"
    REM Return "http://image-sandbox.c2mx-spl.appspot.com/roku/"
End Function



REM ====================================================================
REM == NAME: ConfigDisplayMessage
REM == INPUT PARAMETERS: 
REM ==      MessageArea: Area to return text for
REM == OUTPUT: Returns a Associative Array
REM == DESCRIPTION:Creates and sets message to display to user by calling
REM == the ShowMessageDialog() method.
REM ====================================================================
Function ConfigDisplayMessage(MessageArea As String, ReturnOnlyArray As Boolean) As Object

    'Print to Debugger
    DebugPrint("Create Message Text - [ Config.ConfigDisplayMessage() ] ")

    'Print to Debugger
    DebugPrint("Message Display Area: " + MessageArea, True)

    'Create Associative Array
    aa_Final = CreateObject("roAssociativeArray")
    aa_MessageDetails = CreateObject("roAssociativeArray")
    aa_ButtonText = CreateObject("roAssociativeArray")

    'Set Defaults for Local Values
    headerText = ConfigSSRSName()
    messageText = ConfigSSRSDefaultMessageText()
    buttonText = ConfigSSRSDefaultButtonText()
    showBusy = "False"

    'Iterate Through Various Errors
    If (MessageArea = "ROKUVERSION") Then '// Firmware of Roku is not proper version

        'Set Message Text
        messageText = "You cannot access ScreenPlay Screening Room on your Roku firmware version.  The minimum version is " + ConfigSSRSVersion() + ". Please update from 'Settings ==>> Player Info ==>> Check For Update'. "
        showBusy = "False"

    ElseIf (MessageArea = "SETUP") Then '// User is either a first time user or has to go through registration

        'Set Message Text
        messageText = "Welcome to ScreenPlay Screening Room.  You will now be guided through the initalization.  You are only a few steps away!"
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","Let's Get Started")

    ElseIf (MessageArea = "LOGINERROR") Then '// Some kind of login error, typically related to C2ME call error

        'Set Message Text
        headerText = "ERROR - ScreenPlay Screening Room - ERROR"
        messageText = "There was an error logging into the ScreenPlay ScreenPlay Screening Room. Please contact Support at " + ConfigSSRSSupportEmailAddress()
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","Quit")

    ElseIf (MessageArea = "IDCODELENGTH") Then '// Error when entering the ID Code Length

        'Set Message Text
        headerText = "ID Code Is Required"
        messageText = "A ID Code is required. If you do not have an assigned code, please visit " + ConfigSSRSCMSWebsiteURL() + " to start the process."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","OK")

    ElseIf (MessageArea = "MOVIES") Then '//An error occured when trying to get the Studio Key in scr_StudiosScreen.ShowStudiosScreen()

        'Set Message Text
        headerText = "ERROR - ScreenPlay Screening Room - ERROR"
        messageText = "Screening Room experienced an error generating movie list. Please exit and again. If this error continues, please contact Support at " + ConfigSSRSSupportEmailAddress()
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","Ok")

    ElseIf(MessageArea = "NOACTION") Then '// User took an action that is not ready for primetime (not coded)

        'Set Message Text
        headerText = "This feature is not live yet"
        messageText = "Go about your business, nothing to see here. Move along. These aren't the droids you're looking for."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","Ok")

    ElseIf(MessageArea = "RESET") Then '// User wants to reset all of SSRS

        'Set Message Text
        headerText = "Reset ScreenPlay Screeing Room"
        messageText = "This will clear all your personal settings from the ScreenPlay Screening Room. This means that you will have to reinitalize ScreenPlay Screening Room the next time you log in."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","OK")
        aa_ButtonText.AddReplace("2","Cancel")

    ElseIf (MessageArea = "CANCELSETUP") Then '// User Cancels Setup

        'Set Message Text
        headerText = "Cancel Screening Room Setup?"
        messageText = "You have opted to cancel setup of ScreenPlay Screening Room. This action will clear all user detail entered."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","Cancel Setup")
        aa_ButtonText.AddReplace("2","Return to Setup")

    ElseIf (MessageArea = "BOOKMARKS") Then '// Text for Send Bookmarks

        'Set Message Text
        headerText = "Email Bookmarks"
        messageText = "Send all saved 'Bookmarks' to email address " + GetRegistryValue(ConfigEMAILSectionName()) + "?"
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","Send")
        aa_ButtonText.AddReplace("2","Cancel")

    ElseIf (MessageArea = "XMLERROR") Then '// XML Read Was Bad

        'Set Message Text
        headerText = "Error Reading Data"
        messageText = "There was an error reading data for selected item. Please try again and if it persists, contact Support at " + ConfigSSRSSupportEmailAddress()
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","OK")

    ElseIf (MessageArea = "NOSTUDIOS") Then '// XML Read Was Bad

        'Set Message Text
        headerText = "No Studios Found"
        messageText = "No Studios have been approved for your login. Please try again and if issue persists, contact Support at " + ConfigSSRSSupportEmailAddress() + " to correct the issue."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","OK")

    ElseIf (MessageArea = "INVALIDEMAIL") Then '// User entered a invalid email on registration

        'Set Message Text
        headerText = "Invalid Email Address"
        messageText = "Your email address was not valid. Please double check and enter again. If issue persists, contact Support at " + ConfigSSRSSupportEmailAddress() + "."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","OK")

    ElseIf (MessageArea = "INVALIDFIRSTNAME") Then '// User entered a invalid first name on registration

        'Set Message Text
        headerText = "Invalid Entry"
        messageText = "Your First Name was not valid. Please double check and enter again. If issue persists, contact Support at " + ConfigSSRSSupportEmailAddress() + "."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","OK")

    ElseIf (MessageArea = "FORCERESET") Then '// Message to user if a remote reset was scheduled

        'Set Message Text
        headerText = "Reset Screening Room"
        messageText = "A remote reset has been initiated by ScreenPlay Support to correct a fatal error. Please select 'OK' to reset the Screening Room setup or 'Cancel' to exit Screeners."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","OK")
        aa_ButtonText.AddReplace("2","Cancel")

    ElseIf (MessageArea = "PININVALID") Then '//User Entered A Pin that is not valid

        'Set Message Text
        headerText = "PIN Not Valid"
        messageText = "Your PIN must be six (6) characters long."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","OK")

    ElseIf (MessageArea = "PAIRFAIL") Then '//Pairing Failed For Undetermined Reason

        'Set Message Text
        headerText = "Roku Device Pairing Failure"
        messageText = "Pairing of this Roku device and the Screenplay Engine has experienced a fatal error. Please contact Support at " + ConfigSSRSSupportEmailAddress() + " if this issue persists."
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","OK")
        
        
    ElseIf (MessageArea = "") Then '// Default

        'Set Message Text
        headerText = "ERROR - ScreenPlay Screening Room - ERROR"
        messageText = "ScreenPlay Screening Room experienced an unexpected error. Details have been sent to support. For further details contact Support at " + ConfigSSRSSupportEmailAddress()
        showBusy = "False"

        'Set Buttons
        aa_ButtonText.AddReplace("1","Quit")

    End If

    'Add Values To Associative Array
    aa_MessageDetails.AddReplace("Title", headerText)
    aa_MessageDetails.AddReplace("Message", messageText)
    aa_MessageDetails.AddReplace("ShowBusy",showBusy)
    
    'Populate Final Associative Array
    aa_Final.AddReplace("MessageDetails",aa_MessageDetails)
    aa_Final.AddReplace("Buttons",aa_ButtonText)

    'Return The Associative Array If Dialog is Not Needed
    If(ReturnOnlyArray) Then

        'Return Associative Array
        Return aa_Final

    End If

    'Display Message To User
    MsgBox = ShowMessageDialog(aa_Final)

    'Return Dialog
    Return MsgBox

End Function


REM ====================================================================
REM == NAME: ConfigSetMultiLineScreenText
REM == INPUT PARAMETERS: 
REM ==      Screen: Blank roParagraphScreen
REM ==      MessageArea: Area to return text for
REM == OUTPUT: Returns a Associative Array
REM == DESCRIPTION:Creates and sets message to display to user by calling
REM == the ShowMessageDialog() method.
REM ====================================================================
Function ConfigSetMultiLineScreenText(Screen As Object, MessageArea As String) As Object

    'Print To Debugger
    DebugPrint("Set Text For Screen - [ Config.ConfigSetMultiLineScreenText ]", True)

    'Set Defaults
    title = "ScreenPlay Screening Room"
    button = "OK"

    'Ensure that a Paragraph Screen Is Being Passed
    If Type(Screen) = "roParagraphScreen" Then

        'Walk Logic Tree
        If (MessageArea = "ABOUT") Then '// Settings Screen, User Selected About Screening Room

            'Set The Header Text
            title = "About"
 
            'Set Body Text
            Screen.AddParagraph("The ScreenPlay Screening Room Service (SSRS) was created to facilitate a secure way for users, companies and businesses to view movie screeners instantly and without the need of a DVD or Blu-Ray disc.")
            Screen.AddParagraph("Utilizing several layers of obfuscation and encryption, titles are securely protected against infringement or duplication.")
            Screen.AddParagraph("SSRS uses a sophisticated engine called C2MX (Content Context Media Exchange) to accurately and securely determine what Titles a user is allowed to view and ensures that only authorized Roku devices receive content.")      
            Screen.AddParagraph("For more information on SSRS or the C2MX engine, contact ScreenPlay Inc. at " + ConfigSSRSSupportEmailAddress())
            
            'Set Button
            Screen.AddButton(1, button)

            'Set Header
            Screen.SetBreadcrumbText(ConfigSettingsText(),title)

        ElseIf (MessageArea = "SCREENPLAY") Then '// Settings Screen, User Selected About ScreenPlay

           'Set Header Text
            title = "ScreenPlay, Inc."

            'Set Body Text
            Screen.AddParagraph("ScreenPlay helps our partner networks create the most exciting custom environments based around promotional entertainment video while promoting that content to millions of viewers.")
            Screen.AddParagraph("ScreenPlay specializes in film, music and game promotional video; from the top current releases to the decades of past hits that make up the industry's most comprehensive catalog.")
            Screen.AddParagraph("Please visit us at online at http://www.ScreenPlayInc.com to find out more.")

            'Set Buttons
            Screen.AddButton(1, button)

            'Set Header
            Screen.SetBreadcrumbText(ConfigSettingsText(),title)

        ElseIf (MessageArea = "VERSION") Then '// Settings Screen, User Selected Version Information

            'Set Header Text
            title = "Version"

            'Set Body Text
            Screen.AddParagraph("The following information is used by support when troubleshooting an issue.")
            Screen.AddParagraph("============================================================================")
            Screen.AddParagraph("Roku Software Version: " + ReturnRokuSoftwareVersion())
            Screen.AddParagraph("Roku Serial Number: " + ConfigGetRokuSerialNumber())
            Screen.AddParagraph("IP Address: " + GetRokuIPAddress())
            Screen.AddParagraph("Screen Resolution: " + ValueToString(CreateObject("roDeviceInfo").GetDisplayMode()))
            Screen.AddParagraph("Registered Email Address: " + GetRegistryValue(ConfigEMAILSectionName()))

            'Determine if user used Type A or Type B when creating account
            If(GetRegistryValue(ConfigPINSectionName()) = "ERROR") then

                'Set Custom Message
                Screen.AddParagraph("Screeners Access ID: TYPE B")

            Else
            
                'Set The PIN to UI
                Screen.AddParagraph("Screeners Access ID: " + GetRegistryValue(ConfigPINSectionName()))

            End If
            

            'Set Buttons
            Screen.AddButton(1, button)

            'Set Header
            Screen.SetBreadcrumbText(ConfigSettingsText(),title)

        ElseIf (MessageArea = "LICENSE") Then '// Setttings Screen, User Selected LICENSE AGREEMENT

            'Set Header Text
            title = "License Agreement"

            'Set Body Text
            Screen.AddParagraph("ScreenPlay stores all user information in a secure database using high encryption and obfusication.")
            Screen.AddParagraph("ScreenPlay collects the following: ")
            Screen.AddParagraph("     - Titles Viewed")
            Screen.AddParagraph("     - Percent of Title Viewed")
            Screen.AddParagraph("     - Location of IP Address")
            Screen.AddParagraph("     - Roku Device ID") 
            Screen.AddParagraph("ScreenPlay is happy to address any questions or concerns you may have. To contact our support team, email us at " + ConfigSSRSSupportEmailAddress())
            Screen.AddParagraph("Typically someone will respond within 24 hours of your initial request excluding weekends and US holidays")

            'Set Buttons
            Screen.AddButton(1, button)

            'Set Header
            Screen.SetBreadcrumbText(ConfigSettingsText(),title)

        ElseIf (MessageArea = "SUPPORT") Then '// Settings Screen, User Selected SUPPORT

            'Set Header Text
            title = "Support"

            'Set Body Text
            Screen.AddParagraph("Screening Room Support")
            Screen.AddParagraph("============================================================================")
            Screen.AddParagraph("ScreenPlay is happy to address any questions or concerns you may have. To contact our support team, email us at " + ConfigSSRSSupportEmailAddress())
            Screen.AddParagraph("Typically someone will respond within 24 hours of your initial request excluding weekends and US holidays")
            
            'Set Buttons
            Screen.AddButton(1, button)

            'Set Header
            Screen.SetBreadcrumbText(ConfigSettingsText(),title)

        ElseIf (MessageArea = "WELCOME") Then '//Welcome Screen For Users to enter PIN and ID CODE

            'Set The Screen Title
            title = ConfigSSRSName()

            'Set The Header Text
            Screen.AddHeaderText(ConfigSSRSWelcomeText())

            'Add Details to the Body
            Screen.AddParagraph("Welcome to ScreenPlay Screening Room.  You will now be guided through the initalization.  You are only a few steps away!")
            Screen.AddParagraph("You will be asked to enter your assigned PIN and ID CODE to authorize this Roku device.")
            Screen.AddParagraph("If you do not have an assigned PIN and ID CODE, please visit " + ConfigSSRSCMSWebsiteURL() + " to start the request process.")

            'Add Buttons
            Screen.AddButton(1, "Let's Get Started")
            Screen.AddButton(2, "Cancel")

            'Set Header
            Screen.SetBreadcrumbText(title,"")

        End If

        'Set Values
        Screen.SetTitle(title)
        
        'Return Populated Screen
        Return Screen

    Else '// Something happened, fall into CATCH code

        'Print Error To Debugger
        DebugPrint("Screentype Failed Evaluation to roParagraphScreen", True)

        'Set Defaults
        Screen.SetTitle("ERROR")
        Screen.AddParagraph("An unexpected error occured. If the error persists, please contact Support at " + ConfigSSRSSupportEmailAddress())
        Screen.AddButton(1, button)

    End If

    'Return Unpopulated Screen
    Return Screen

End Function