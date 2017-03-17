
REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Contains XML methods and manipulations
REM ==
REM ********************************************************************
REM ********************************************************************



REM ====================================================================
REM == NAME: GetMoviesXML()
REM == INPUT PARAMETERS:
REM ==      StudioKey: The stuidio key obtained in GetStudiosXML()
REM == OUTPUT: Returns Array populated with AssociativeArray from C2ME
REM == DESCRIPTION: Constructs and calls C2ME web service call for Movies
REM == XML and verifies that the XML is valid then returns parsed Array.
REM ====================================================================
Function GetMoviesXML(StudioKey As String) As Object

    'Print to Debugger
    DebugPrint("Get Movie XML - [ utl_XML.GetMoviesXML() ]")

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Create XML Element
    xml = CreateObject("roXMLElement")

    'Set The Web Service
    xfer.SetURL(ConfigReturnMediaScreenWebServiceURL() + StudioKey)

    'Print Calling URL to Debugger
    DebugPrint("Calling URL: " + ConfigReturnMediaScreenWebServiceURL() + StudioKey, True)

    'Make Call To C2ME Web Service
    xmlString = xfer.GetToString() 

    'Determine If XML is valid
    If Not (xml.Parse(xmlString)) Then

        'Print To Debugger
        DebugPrint("Error Parsing Movie XML", true)

        'Print URL To Debugger
        DebugPrint("Referring URL: " + ConfigReturnMediaScreenWebServiceURL() + StudioKey, True)

        'Create Associative Array For Error
        aa_Error = CreateObject("roAssociativeArray")

        'Set the error message to be read by parent call
        aa_Error.AddReplace("ERROR","ERROR")

        'Return An Error
        Return aa_Error
    Else

        'Print To Debugger
        DebugPrint("Movie XML Is Valid", true)

    End If

    'Set Count Of Movies
    movieCount = Xml.GetChildElements().Count()

    'Determine if There are elements
    If (movieCount < 1) Then

        'Print To Debugger
        DebugPrint("Error: No Movies Found In Movie XML - [ utl_XML.GetMoviesXML() ]", True)

        'Return An Error
        Return "ERROR"

    End If

    'Get Values From XML Header
    For Each xmlAttribute In xml.GetAttributes()

        'Look For Count
        If xmlAttribute = "count" Then

            'Set the count from XML file
            setCount = xml@count

        'Look For Hardware Key
        ElseIf xmlAttribute = "hka" Then

            'Set the Hardware Key Authorization
            setHKA = xml@hka

        'Look for Studio Name
        ElseIf xmlAttribute = "studioName" Then

            'Set The Studio Name
            studioName = xml@studioName

        End If
    Next

    'Print To Debugger
    DebugPrint("Read Movie XML Header MetaData", True)
    DebugPrint("Studio Name: " + studioName, True)
    DebugPrint("Title Count: " + setCount, True)
    DebugPrint("Hardware Key Authorization: " + setHKA, True)

    'Create Associative Array For XML Header Data
    aa_StudioXMLHeaderData = CreateObject("roAssociativeArray")

    'Add Values To Associative Array
    aa_StudioXMLHeaderData.AddReplace("StudioName", studioName)
    aa_StudioXMLHeaderData.AddReplace("TitleCount", setCount)
    aa_StudioXMLHeaderData.AddReplace("HardwareKey", setHKA)

    'Create An Array To Return
    a_ListOfItems = CreateObject("roArray", setCount.toint(), True)

    'Create Associative Array Of XML Elements
    aa_CollectionOfTitles  = CreateObject("roAssociativeArray")

    'Create Counter
    titleIndex = 0

    'Populate the Titles into respective Associative Arrays
    For Each Title In xml.title

        'Print To Debugger
        DebugPrint("Parsing Movie Title Metadata: " + Title.titleName.GetText(), True) 

        'Create Associative Array Of XML Elements
        aa_PosterItems = CreateObject("roAssociativeArray")

        'Define Content
        aa_PosterItems.ContentType = "movie"

        'Add the Metadata from XML to Associative Array
        aa_PosterItems.ShortDescriptionLine1 = Title.titleName.GetText()
        aa_PosterItems.ShortDescriptionLine2 = "Expires " + Title.availableUntil.GetText()
        aa_PosterItems.SDPosterUrl = Title.sd_img.GetText()
        aa_PosterItems.HDPosterUrl = Title.hd_img.GetText()

        'Add The Associative Array to the Array
        a_ListOfItems.Push(aa_PosterItems)

        'Create Associative Array Of Title Metadata
        aa_TitleData = CreateObject("roAssociativeArray")

        'Set Title Metadata Values
        aa_TitleData.ContentType = "Movie"
        aa_TitleData.SDPosterUrl = Title.sd_img.GetText()
        aa_TitleData.HDPosterUrl = Title.hd_img.GetText()
        aa_TitleData.ShortDescriptionLine1 = Title.synopsis.GetText()  
        aa_TitleData.Description = Title.synopsis.GetText() 
        aa_TitleData.Rating = Title.rating.GetText()
        aa_TitleData.IsHD = True
        aa_TitleData.HDBranded = True 
        aa_TitleData.Director = Title.director.GetText()
        aa_TitleData.Actor = Title.actor.GetText()
        aa_TitleData.ReleaseDate = Title.year.GetText()
        aa_TitleData.Length = Title.runtime.GetText()
        aa_TitleData.Categories = [Title.genre.GetText() + "         Expires " + Title.availableUntil.GetText()]
        aa_TitleData.Title = Title.titleName.GetText() 
        aa_TitleData.streamURL = Title.streamURL.GetText()
        aa_TitleData.trailerURL = Title.trailerURL.GetText()
        aa_TitleData.screenPlayID = Title.screenplay_id.GetText()

        'Add Associative Array to Associative Array 
        aa_CollectionOfTitles.AddReplace(ValueToString(titleIndex), aa_TitleData)

        'Increment Studio Index
        titleIndex = titleIndex + 1

    Next

    'Create Associative Array For Pass Back
    aa_Final = CreateObject("roAssociativeArray")

    'Set AA Values
    aa_Final.MovieItemsArray = a_ListOfItems
    aa_Final.CollectionOfTitles = aa_CollectionOfTitles 
    aa_Final.XMLHeaderData = aa_StudioXMLHeaderData

    'Return Populated Associative Array
    Return aa_Final

End Function

REM ====================================================================
REM == NAME: GetStudiosXML()
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: Returns Array populated with AssociativeArray from C2ME
REM == DESCRIPTION: Constructs and calls C2ME web service call for Studios
REM == XML and verifies that the XML is valid then returns parsed Array.
REM ====================================================================
Function GetStudiosXML() As Object

    'Print to Debugger
    DebugPrint("Get Studios XML - [ utl_XML.GetStudiosXML() ]")

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Create XML Element
    xml = CreateObject("roXMLElement")

    'Set The Web Service
    xfer.SetURL(ConfigReturnStudioScreenWebServiceURL())

    'Create Associative Array For Status
    aa_Status = CreateObject("roAssociativeArray") 

    'Print Calling URL to Debugger
    DebugPrint("Calling URL: " + ConfigReturnStudioScreenWebServiceURL(),True)

    'Make Call To C2ME Web Service
    xmlString = xfer.GetToString() 

    'Create Associative Array For Pass Back
    aa_Final = CreateObject("roAssociativeArray")

    'Set Default Values
    setCount = "0"
    setHKA = "No Key Found in XML"

    'Determine If XML is valid
    If Not (xml.Parse(xmlString)) Then

        'Print To Debugger
        DebugPrint("Error Parsing Studio XML", true)

        'Print URL To Debugger
        DebugPrint("Referring URL: " + ConfigReturnStudioScreenWebServiceURL(), true)

        'Set Status
        aa_Status.AddReplace("XMLERROR","XMLERROR")

        'Skip To End If Error
        GoTo SkipStudios

    Else

        'Print To Debugger
        DebugPrint("Studio XML Is Valid", true)

    End If

    'Set Count Of Studios
    studioCount = Xml.GetChildElements().Count()

    'Determine if There are elements
    If (studioCount < 1) Then

        'Print To Debugger
        DebugPrint("Error: No Studios Found In Studio Screen XML - [ utl_XML.GetStudiosXML() ]", true)

        'Return An Error
        Return "ERROR"

    End If

    'Get Values From XML Header
    For Each xmlAttribute In xml.GetAttributes()

        'Look For Count
        If xmlAttribute = "count" Then

            'Set the count from XML file
            setCount = xml@count

        'Look For Hardware Key
        ElseIf xmlAttribute = "hka" Then

            'Set the Hardware Key Authorization
            setHKA = xml@hka

        End If
    Next

    'Print To Debugger
    DebugPrint("Read Studio XML Header MetaData", True)
    DebugPrint("Studio Count: " + setCount, True)
    DebugPrint("Hardware Key Authorization: " + setHKA, True)

    'Determine if any XML Nodes were found
    If(setCount = "0") Then

        'Print To Debugger
        DebugPrint("ERROR - No Studios Returned in XML",True)

        'Set Error Message
        aa_Status.AddReplace("NOSTUDIOS","NOSTUDIOS")

        'Skip To End If Error
        GoTo SkipStudios

    End If

    'Create An Array To Return
    a_ListOfItems = CreateObject("roArray", setCount.toint(), True)

    'Create Associative Array Of XML Elements
    aa_StudioData = CreateObject("roAssociativeArray")

    'Create Counter
    studioIndex = 0

    'Populate the Studios into respective Associative Arrays
    For Each Studio In xml.studio

        'Print To Debugger
        DebugPrint("Parsing Studio Metadata: " + Studio.studioName.GetText() + " metadata", True) 

        'Create Associative Array Of XML Elements
        aa_PosterItems = CreateObject("roAssociativeArray")

        'Define Content
        aa_PosterItems.ContentType = "movie"

        'Add the Metadata from XML to Associative Array
        aa_PosterItems.ShortDescriptionLine1 = Studio.studioName.GetText()
        aa_PosterItems.ShortDescriptionLine2 = "Titles Available: " + Studio.titleCount.GetText()
        aa_PosterItems.SDPosterUrl = Studio.sd_img.GetText()
        aa_PosterItems.HDPosterUrl = Studio.hd_img.GetText()

        'Add The Associative Array to the Array
        a_ListOfItems.Push(aa_PosterItems)

        'Add Value to Associative Array 
        aa_StudioData.AddReplace(ValueToString(studioIndex), Studio.studioKey.GetText())

        'Print Studio Index To Debugger
        DebugPrint("Studio Index: " + ValueToString(studioIndex), True) 

        'Increment Studio Index
        studioIndex = studioIndex + 1

    Next

    'Add Settings Option to Screen
    a_ListOfItems.Push(AddSettingsOption())

    'Add Settings Lookup to Associative Array
    aa_StudioData.AddReplace(ValueToString(studioIndex), "Settings")

    'Set Return Associative Array Values
    aa_Final.StudioItemsArray = a_ListOfItems
    aa_Final.StudioData = aa_StudioData

    SkipStudios:
    aa_Final.Status = aa_Status

    'Return XML
    Return aa_Final

End Function


REM ====================================================================
REM == NAME: GetOrSetPlayBackPosition()
REM == INPUT PARAMETERS: ScreenplayID = The Screenplay ID to be saved
REM == OUTPUT: 
REM == DESCRIPTION: 
REM == CALLED FROM: 
REM ====================================================================
Function SendError(ErrorMessage As String) As Boolean

    'Print To Debugger
    DebugPrint("Sending Error Details - utl_XML.SendError()")

    'Set the URL To Call
    postURL = ConfigReturnErrorSaveWebServiceURL(ErrorMessage)

    'Print To Debugger
    DebugPrint("Calling URL: " + postURL, True)

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Create XML Element
    xml = CreateObject("roXMLElement")

    'Set The Web Service
    xfer.SetURL(postURL)

    'Make Call To C2ME Web Service
    xmlString = xfer.GetToString() 

    'Determine If XML is valid
    If Not (xml.Parse(xmlString)) Then '//Not Valid

        'Print To Debugger
        DebugPrint("Error With 'Error' XML. No Error Message Saved", true)

        'Return Status
        Return False

    End If

    'Return Status
    Return True

End Function

REM ====================================================================
REM == NAME: GetOrSetPlayBackPosition()
REM == INPUT PARAMETERS: ScreenplayID = The Screenplay ID to be saved
REM == OUTPUT: 
REM == DESCRIPTION: 
REM == CALLED FROM: 
REM ====================================================================
Function GetOrSetPlayBackPosition(ScreenplayID As String, IsGet = true, PlayBackPos = 0) As Integer

    'Print To Debugger
    DebugPrint("Get Or Set PlayBack Position - utl_XML.GetOrSetPlayBackPosition() ")

    'Set Initial SAVE PlayBack Variable
    callURL = ConfigSavePlayBackPositionURL(ScreenplayID, ValueToString(PlayBackPos))

    'If The Call is a GET PlayBack Position, Then Set URL
    If(IsGet) Then

        'Set URL
        callURL = ConfigGetPlayBackPositionURL(ScreenplayID)

    End If

    'Print To Debugger
    DebugPrint("Calling URL: " + callURL)

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Create XML Element
    xml = CreateObject("roXMLElement")

    'Set The Web Service
    xfer.SetURL(callURL)

    'Make Call To C2ME Web Service
    xmlString = xfer.GetToString() 

    'Determine If XML is valid
    If Not (xml.Parse(xmlString)) Then '//Not Valid

        'Print To Debugger
        DebugPrint("Error PlayBack XML. Returning Default Start Position.", true)

        'Return Default
        Return 0

    Else '// Valid XML

        'Print To Debugger
        DebugPrint("PlayBack XML Is Valid", true)

        'Find Value
        If Not(Xml.position_result.GetText() = "False") Then

            'Determine What Action Was Taken
            If Not(IsGet) Then

                'Print To Debugger
                DebugPrint("Returning PlayBack Position of " + Xml.position_result.GetText(), True)

            End If

            'Return Reboot Requested
            Return Xml.position_result.GetText().ToInt()

        End If

    End If

    'Return Default Start Position
    Return 0

End Function

REM ====================================================================
REM == NAME: GetOrSetPlayBackPosition()
REM == INPUT PARAMETERS: ScreenplayID = The Screenplay ID to be saved
REM == OUTPUT: True or False
REM == PARAMETERS:
REM ==      PIN: Pin number entered by user
REM ==      IDCode: ID Code entered by user
REM == DESCRIPTION: Calls C2MX providing HAK, PIN and ID CODE 
REM == CALLED FROM: Authentication.InitAuthentication
REM ====================================================================
Function PairAndRegisterDevice(PIN As String, IDCode As String) As Boolean

    'Print To Debugger
    DebugPrint("Pair and Register Device.")

    'Print To Debugger
    DebugPrint("Calling URL: " + ConfigReturnPairingURL(PIN, IDCode), true)

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Create XML Element
    xml = CreateObject("roXMLElement")

    'Set The Web Service
    xfer.SetURL(ConfigReturnPairingURL(PIN, IDCode))

    'Make Call To C2ME Web Service
    xmlString = xfer.GetToString() 

    'Determine If XML is valid
    If Not (xml.Parse(xmlString)) Then '//Not Valid

        'Print To Debugger
        DebugPrint("Error Pairing XML is not valid", true)

        'Return Default
        Return False

    Else '// Valid XML

        'Print To Debugger
        DebugPrint("Serial Number XML Is Valid", true)

        'Ensure that A Key Was Generated
        If (xml.hardware_key_authentication.GetText() = "False") then

            'Display Error Dialog
            DialogPairingFailed()
            
            'No Values Generated
            Return False

        End If

        'Determine if XML is returning the HKA
        If Not(xml.hardware_key_authentication.GetText() = "Request_Registration") Then

            'Print To Debugger
            DebugPrint("Returned HKA Key " + xml.hardware_key_authentication.GetText(), True)

            'Save HKA To Registry
            WriteToRegistry(ConfigHKASectionName(),xml.hardware_key_authentication.GetText()) 

            'Write the Email Address To Registry
            WriteToRegistry(ConfigEMAILSectionName(),xml.email_address.GetText()) 

            'Return Reboot Requested
            Return True

        Else '// XML contained "Request_Registration"

            'Return Failure
            Return False

        End If

    End If '// End XML Handling

    'Return Status
    Return False

End Function


REM ====================================================================
REM == NAME: GetAuthenticationKeyStatus()
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: 
REM == DESCRIPTION: Calls out to C2MX to determine if a Screeners Serial 
REM == can be generated. If a number returns, then it's valid, if FALSE
REM == returns, then user will be redirected to set up thier account via
REM == email address and 
REM == CALLED FROM: Call initiated from src_HardwareAuthenticationKeyLogin.InitalizeAuthenticationKey
REM ====================================================================
Function GetAuthenticationKeyStatus() As Boolean

    'Print To Debugger
    DebugPrint("Call Serial Number Generation Service", true)

    Return true

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Create XML Element
    xml = CreateObject("roXMLElement")

    'Set The Web Service
    xfer.SetURL(ConfigReturnGenerateAuthenticationKeyURL())

    'Print Calling XML To Debugger
    DebugPrint("Calling URL: " + ConfigReturnGenerateAuthenticationKeyURL(), True)

    'Make Call To C2ME Web Service
    xmlString = xfer.GetToString() 

    'Determine If XML is valid
    If Not (xml.Parse(xmlString)) Then '//Not Valid

        'Print To Debugger
        DebugPrint("Error Reading Serial Number XML", true)

        'Return Default
        Return False

    Else '// Valid XML

        'Print To Debugger
        DebugPrint("Serial Number XML Is Valid", true)

        'Find Value
        If Not(Xml.serialnumber.GetText() = "False") Then

            'TODO - Hook up real value from XML - serialnumber.GetText()

            'Print To Debugger
            DebugPrint("Roku Device Has Been Validated for a Screeners Serial Number", True)

            'Save Serial Number To Registry
            WriteToRegistry(ConfigScreenersSerialNumber(), Xml.serialnumber.GetText())

            'Return Serial Number Was Created And Saved
            Return True

        End If

    End If

    'Print To Debugger
    DebugPrint("No Serial Number Generated", true)

    'Return Default
    Return False 

End Function

REM ====================================================================
REM == NAME: GetSSRSResetStatus()
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: Returns True or False
REM == DESCRIPTION: Calls web service for specific Roku box and User
REM == to determine if the box should be reset. Returns a TRUE if the box
REM == should be reset, and FALSE if not.
REM == CALLED FROM: utl_Validation.CheckForScreenersReset()
REM ====================================================================
Function GetSSRSResetStatus() As Boolean

    'Print To Debugger
    DebugPrint("Call SSRS Reset Web Service", True)

    'Create URL Object
    xfer = CreateObject("roURLTransfer")

    'Create XML Element
    xml = CreateObject("roXMLElement")

    'Set The Web Service
    xfer.SetURL(ConfigReturnSSRSResetURL())

    'Print Calling XML To Debugger
    DebugPrint("Calling URL: " + ConfigReturnSSRSResetURL(), True)

    'Make Call To C2ME Web Service
    xmlString = xfer.GetToString() 

    'Determine If XML is valid
    If Not (xml.Parse(xmlString)) Then '//Not Valid

        'Print To Debugger
        DebugPrint("Error Reading Reset XML", true)

        'Return Default
        Return False

    Else '// Valid XML

        'Print To Debugger
        DebugPrint("Reset XML Is Valid", true)

        'Find Value
        If(Xml.reset_result.GetText() = "True") Then

            'Print To Debugger
            DebugPrint("A Remote SSRS Reset Has Been Requested", True)

            'Return Reboot Requested
            Return True

        End If

    End If

    'Print To Debugger
    DebugPrint("No SSRS Remote Reset Requested", True)

    'Return Default
    Return False 

End Function