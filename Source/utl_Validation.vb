REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Runs validation methods for logged in user
REM ==
REM ********************************************************************
REM ********************************************************************

 


REM ====================================================================
REM == NAME: ValidateRegistryValue
REM == INPUT PARAMETERS: Section = Area Under ScreenPlay_SSRS to look for
REM == OUTPUT: True or False
REM == DESCRIPTION: Searches registry to determine if Sections exist. 
REM ====================================================================
Function ValidateRegistryValue(Section As String) As Boolean

    'Print to Debugger
    DebugPrint("Validate Roku Section - [ Validation.ValidateRegistryValue() ]")

    'Search Registry For Values
    If (ReadFromRegistry(Section)) Then

        'Print To Debugger
        DebugPrint("Value successfully found in registry. - [" + Section + "]",True)

        'Is Valid
        Return True

    End If

    'Print Failure Status to UI
    DebugPrint("Value was NOT found in registry. Returning value of FALSE. - [" + Section + "]",True)

    'Return Value
    Return False

End Function


REM ====================================================================
REM == NAME: ValidRoku
REM == INPUT PARAMETERS: Section = Area Under ScreenPlay_SSRS to look for
REM == OUTPUT: True or False
REM == DESCRIPTION: Searches registry to determine if Sections exist. 
REM ====================================================================
Function ValidRoku() As Boolean

    'Print to Debugger
    DebugPrint("Validate Roku - [ Registry.ValidateRoku() ]")

    'Set Default Flag
    isValidRoku = true

    'Search Regisry for HKA
    If Not (ReadFromRegistry(ConfigHKASectionName())) Then 

        'Print to Debugger
        DebugPrint("Invalid Login.  No HKA found.", True)

        'Is InValid Login
        isValidRoku = False

    End If 

    'Search Registry For UAT
    If Not (ReadFromRegistry(ConfigUATSectionName())) Then 

        'Print to Debugger
        DebugPrint("Invalid Login.  No UAT found.", True)

        'Is InValid Login
        isValidRoku = False

    End If

    'Print to Debugger Status
    If(isValidRoku) then
        DebugPrint("Roku passes validation.", True)
    Else
        DebugPrint("Roku fails validation.", True)
    End If  

    'Return Valid Login
    Return isValidRoku

End Function


REM ====================================================================
REM == NAME: ValidateLogin
REM == INPUT PARAMETERS: None
REM == OUTPUT: True or False
REM == DESCRIPTION: Calls C2ME to determine if user/roku is authorized
REM == to use SSRS.  Returns FALSE if user is not authorized else TRUE
REM ====================================================================
Function ValidateLogin() As Boolean

    'Print to Debugger
    DebugPrint("Validate Login - [ Validation.ValidateLogin() ] ")

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Create Associative Array
    loginAuth = CreateObject("roAssociativeArray")

    'Print URL to Debugger
    DebugPrint("Calling URL: " + ConfigReturnVerifyUserWebServiceURL(), True)

    'Set The Web Service - LOGIN call
    xfer.SetURL(ConfigReturnVerifyUserWebServiceURL())

    'Call Web Service
    auth = xfer.GetToString()

    'Create XML Element
    xml=CreateObject("roXMLElement")

    'Read The XML
    If xml.Parse(auth) then

        'Set Status
        loginAuth.status = xml.status.GetText()

        'Set Session Token
        loginAuth.sessionToken = xml.login.GetText()

        'Print to Debugger
        DebugPrint("Validation Of User: " + loginAuth.status, True)
        DebugPrint("Session Token: " + loginAuth.sessionToken, True)

        'Return Success if Ok
        If(loginAuth.status = "Ok") Then

            'Save Session Token To Registry
            WriteToRegistry(ConfigSessionTokenName(),loginAuth.sessionToken)

            'Return Status Of ALL GOOD
            Return True

        ElseIf (loginAuth.status = "Err") Then 'Login Did Not Validate
            Return False
        End If
            
    End If

    'Default
    Return False

End Function


REM ====================================================================
REM == NAME: GetUserToken
REM == INPUT PARAMETERS: None
REM == OUTPUT: Token Number or ERROR
REM == DESCRIPTION: Calls C2ME to get the user token.  This should then
REM == be stored in the registry for later confirmation and verification.
REM == A return of ERROR means that user token is not accepted.
REM ====================================================================
Function GetUserToken() As String

    'Print to Debugger
    DebugPrint("Return User Token - [ Registry.GetUserToken() ] ")

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Set The Web Service
    xfer.SetURL(ConfigReturnTokenWebServiceURL())

    'TODO - REMOVE FOR PRODUCTION
    DebugPrint("Calling URL: " + ConfigReturnTokenWebServiceURL(),True)

    'Call Web Service
    auth = xfer.GetToString()

    'Create XML Element
    xml=CreateObject("roXMLElement")

    'Read The XML
    If xml.Parse(auth) then

        'Get the Status Value
        token = xml.access_token.GetText()

        'Print to Debugger
        DebugPrint("User Token: " + token)

        'Return Token
        Return token
            
    End If

    'Default
    Return "ERROR"

End Function


REM ====================================================================
REM == NAME: GenerateHardwareKeyAuthorization
REM == INPUT PARAMETERS: None
REM == OUTPUT: Associative Array  
REM == DESCRIPTION: Calls C2ME to get the access token and hardware auth key.  
REM == This should then be stored in the registry for later confirmation 
REM == and verification.
REM ====================================================================
Function GenerateHardwareKeyAuthorization() As Object

    'Print to Debugger
    DebugPrint("Return User HKey - [ Validation.GenerateHardwareKeyAuthorization() ] ")

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Create Associative Array
    authKeys = CreateObject("roAssociativeArray")

    'Set The Web Service
    xfer.SetURL(ConfigReturnRegisterAndVerifyRokuWebServiceURL())

    'Print To Debugger
    DebugPrint("Calling URL: " + ConfigReturnRegisterAndVerifyRokuWebServiceURL(),True)

    'Call Web Service
    auth = xfer.GetToString()

    'Create XML Element
    xml=CreateObject("roXMLElement")

    'Read The XML
    If xml.Parse(auth) then

        'Set Hauth Value
        authKeys.user_api_token = xml.user_api_token.GetText()

        'Set Access Token Value
        authKeys.hardware_key_authentication = xml.hardware_key_authentication.GetText()

        'Determine if Token Was Returned 
        If authKeys.hardware_key_authentication = "" Then 
            authKeys.hardware_key_authentication = "ERROR"
        End If

        'Determine if Hardware Auth 
        If authKeys.user_api_token = "" Then
            authKeys.user_api_token = "ERROR"
        End If

        'Print to Debugger
        DebugPrint("User UAT: " + authKeys.user_api_token, true)
        DebugPrint("User HKA: " + authKeys.hardware_key_authentication, true)
           
    End If

    'Default
    Return authKeys

End Function


REM ====================================================================
REM == NAME: GetUserHKey
REM == INPUT PARAMETERS: None
REM == OUTPUT: Token Number or ERROR
REM == DESCRIPTION: Calls C2ME to get the user token.  This should then
REM == be stored in the registry for later confirmation and verification.
REM == A return of ERROR means that user token is not accepted.
REM ====================================================================
Function GetUserHKey() As String

    'Print to Debugger
    DebugPrint("Return User HKey - [ Validation.GetUserKey() ] ")

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Set The Web Service
    xfer.SetURL(ConfigReturnRegisterAndVerifyRokuWebServiceURL())

    'TODO - REMOVE FOR PRODUCTION
    DebugPrint("Calling URL: " + ConfigReturnRegisterAndVerifyRokuWebServiceURL(),True)

    'Call Web Service
    auth = xfer.GetToString()

    'Create XML Element
    xml=CreateObject("roXMLElement")

    'Read The XML
    If xml.Parse(auth) then

        'Get the Status Value
        key = xml.hauth.GetText()

        'Print to Debugger
        DebugPrint("User HKEY: " + key)

        'Return Token
        Return key
            
    End If

    'Default
    Return "ERROR"

End Function


REM ====================================================================
REM == NAME: ValidateRokuVersion
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Determines what version of BrightScript the Roku
REM == box is running and returns a status of TRUE or FALSE if the version
REM == is approved.
REM ====================================================================
Function ValidateRokuVersion() As Boolean

    'Print To Debugger
    DebugPrint("Get Roku Firmware Version - [ Validation.ValidateRokuVersion() ]")

    'Create Device Info
    deviceInfo = CreateObject("roDeviceInfo")

    'Get Version
    rokuVersion = deviceInfo.GetVersion()

    'Iterate Through Values
    majorVersion = Mid(rokuVersion, 3, 1)
    minorVersion = Mid(rokuVersion, 5, 2)
    buildVersion = Mid(rokuVersion, 8, 5)

    'Concatenate Version
    fullVersion = majorVersion + "." + minorVersion

    'Print Version Details to Debugger
    DebugPrint("Roku Firmware Version: " + fullVersion, True)

    'Determine If Version is accepted version
    If (fullVersion.tofloat() < ConfigSSRSVersion()) Then

        'Print Version Details to Debugger
         DebugPrint("Firmware Version Is Not Valid", True)

        'Version Does Not Meet Minimum Requirements
        Return False

    End If

    'Print Version Details to Debugger
    DebugPrint("Firmware Version Is Valid", True)

    'Return Status of OK
    Return True

End Function


REM ====================================================================
REM == NAME: CheckForScreenersReset
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: True or False
REM == DESCRIPTION: Calls web service to determine if the Screeners channel
REM == should be reset or not. This allows for remote reset of Screeners.
REM ====================================================================
Function CheckForScreenersReset() As Boolean

    'Print To Debugger
    DebugPrint("Check for Screeners Reset - [ Validation.CheckForScreenersReset() ]")

    'Call Web Service
    Return GetSSRSResetStatus()

End Function


REM ====================================================================
REM == NAME: RenewUserResetFlag
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: NONE
REM == DESCRIPTION: Resets the TRUE or FALSE flag for user email and 
REM == Roku device so that a user won't be prompted again.
REM == CALLED FROM: Main in first few lines
REM ====================================================================
Sub RenewUserResetFlag()

    'Print To Debugger
    DebugPrint("Renew the user Reset Flag - [ Validation.RenewUserResetFlag() ]", True)

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Print To Debugger
    DebugPrint("Calling URL: " + ConfigRenewResetFlagURL(), True)

    'Set The Web Service
    xfer.SetURL(ConfigRenewResetFlagURL())

    'Make Call To C2ME Web Service
    xmlString = xfer.GetToString() 

End Sub


REM ====================================================================
REM == NAME: ReturnRokuSoftwareVersion
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: String
REM == DESCRIPTION: Finds the version of software running on the Roku
REM == and returns as a string
REM ====================================================================
Function ReturnRokuSoftwareVersion() As String

    'Create Device Info
    deviceInfo = CreateObject("roDeviceInfo")

    'Get Version
    rokuVersion = deviceInfo.GetVersion()

    'Iterate Through Values
    majorVersion = Mid(rokuVersion, 3, 1)
    minorVersion = Mid(rokuVersion, 5, 2)
    buildVersion = Mid(rokuVersion, 8, 5)

    'Concatenate Version
    fullVersion = majorVersion + "." + minorVersion

    'Return
    return ValueToString(fullVersion)

End Function

REM ====================================================================
REM == NAME: ValidateEmailAddress
REM == INPUT PARAMETERS: 
REM ==      EmailAddress: Email Address to be validated
REM == OUTPUT: True or False
REM == DESCRIPTION: Checks to determine that the email address is valid
REM == and returns TRUE or FALSE depending on what it finds.
REM ====================================================================
Function ValidateEmailAddress(EmailAddress As String) As Boolean

    'Print To Debugger
    DebugPrint("Validate Email Address - [ Validation.ValidateEmailAddress ]", True)

    'Regular Expression Email Address Match
    match = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b"

    'Check That Email Address Is Not Blank
    If(EmailAddress = "") Then

        'Return Error Status
        Return False

    End If

    'Create Full Match
    FullRegMatch = CreateObject("roRegex",match, "i")

    'Check the email address
    If Not(FullRegMatch.IsMatch(EmailAddress)) Then

        'Return Error Status
        Return False

    End If

    'Set Reg Ex Object or @ Sign
    emailATSign_regex = CreateObject("roRegex","@", "i")

    'Check for @ Sign
    If Not(emailATSign_regex.IsMatch(EmailAddress)) Then

        'Return Error Status
        Return False

    End If

    'Set Reg Ex Object or @ Sign
    emailDot_regex = CreateObject("roRegex",".", "i")

    'Check for . In Email
    If Not(emailDot_regex.IsMatch(EmailAddress)) Then

        'Return Error Status
        Return False

    End If

    'Print To Debugger
    DebugPrint("Email Passed Validation", True)

    'Return Valid Status
    Return True

End Function

REM ====================================================================
REM == NAME: ValidateInput
REM == INPUT PARAMETERS: 
REM ==      ValueToVerify: Value to run validation against
REM == OUTPUT: True or False
REM == DESCRIPTION: Determines whether input from UI is valid and returns
REM == status based on findings
REM ====================================================================
Function ValidateInput(ValueToVerify As String, IsSpecial = false, AreaToVerify = "none") As Boolean

    'Print to Debugger
    DebugPrint("Validate Input Value: " + ValueToVerify, true)

    'Validate that Value Has Text
    If(ValueToVerify = "") then

        'Print to Debugger
        DebugPrint("Value is invalid",true)

        'Failed Verification
        Return false

    End If

    'Special Handling
    If(IsSPecial) Then

        'PIN Number
        If(AreaToVerify = "PIN") Then

            'Ensure Length Is Proper
            If (Len(ValueToVerify) < 6) Then

                'Print to Debugger
                DebugPrint("Value is invalid",true)

                'Failed Verification
                Return False

            End If

        End If

    End If

    'Print to Debugger
    DebugPrint("Value is valid.",true)

    'Return Status
    Return true

End Function




