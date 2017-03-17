REM ====================================================================
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Utilities related to the Registry 
REM ==
REM ====================================================================




REM ====================================================================
REM == NAME: ReadFromRegistry
REM == INPUT PARAMETERS: Section = Area Under ScreenPlay_SSRS to look for
REM == OUTPUT: Returns True or False
REM == DESCRIPTION: Searches Registry for Section and determines if it 
REM == exists.  If it doesn't, a status of False is returned.
REM ====================================================================
Function ReadFromRegistry(Section As String) As Boolean

    'Print to Debugger
    DebugPrint("Read Registry - [ Registry.ReadFromRegistry() ] - Search For Value: " + Section, True)

    'Create Local Variable
    isSectionFound = False

    'Create Registry Object
    Registry = CreateObject("roRegistrySection", ConfigRegistryKeyName())

    'Look For Section In the Registry
    If Registry.Exists(Section) Then

        'Registry Value Is Found
        isSectionFound = True

    End If

    'Ensure Value Isn't ERROR
    If(GetRegistryValue(Section) = "ERROR") Then

        'Registry Value Is Found But Has No Value
        isSectionFound = False

    End If

    'Print Status
    If (isSectionFound) Then
        DebugPrint("Registry Entry [" + Section + "] Found.", True)
    Else
        DebugPrint("Registry Entry [" + Section + "] NOT Found.", True)
    End If

    'Return Value
    Return isSectionFound

End Function

REM ====================================================================
REM == NAME: PrintRegistry
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: NONE
REM == DESCRIPTION: Prints registry values
REM == EXAMPLE CALL: PrintRegistry()
REM ====================================================================
Sub PrintRegistry()

    'Print to Debugger
    DebugPrint("Print Registry - [ Registry.PrintRegistry() ]")

    'Create Registry Object
    Registry = CreateObject("roRegistrySection", ConfigRegistryKeyName())

    'Print All Areas
    DebugPrint("All Stored Keys:")
    Print(Registry.GetKeyList())

    'Print EMAIL
    DebugPrint("EMAIL ADDRESS: " +  Registry.Read(ConfigEMAILSectionName()))
               
    'Print PIN
    DebugPrint("PIN: " + Registry.Read(ConfigPINSectionName()))
               
    'Print TOKEN
    DebugPrint("UAT: " + Registry.Read(ConfigUATSectionName()))
               
    'Print HKEY
    DebugPrint("HKA: " + Registry.Read(ConfigHKASectionName()))

End Sub

REM ====================================================================
REM == NAME: GetRegistryValue
REM == INPUT PARAMETERS: Section
REM == OUTPUT: String
REM == DESCRIPTION: Returns a value for the registry section requested
REM == EXAMPLE CALL: Foo = GetRegistryValue(ConfigEMAILSectionName())
REM ====================================================================
Function GetRegistryValue(Section As String) As String

    'Create Registry Object
    Registry = CreateObject("roRegistrySection", ConfigRegistryKeyName())

    'Set Value
    regValue = Registry.Read(Section)

    'Check for value
    If(regValue = "") Then

        'Return Value Not Found Error
        Return "ERROR"

    Else

        'Return Registry Value
        Return regValue

    End If

End Function

REM ====================================================================
REM == NAME: WriteToRegistry
REM == INPUT PARAMETERS:
REM ==      Section: Definition area
REM ==      Value: Value associated with Section
REM == EXAMPLE CALL: WriteToRegistry("EMAILADDRESS", "foo@somecompany.com")
REM == DESCRIPTION: Writes to the ScreenPlay_SSRS Key area.
REM ====================================================================
Sub WriteToRegistry(Section As String, Value As Object)

    'Print to Debugger
    DebugPrint("Write To Registry - [ Registry.WriteToRegistry() ]", True)

    'Create Registry Object for ScreenPlay_SSRS Key
    sec = CreateObject("roRegistrySection", ConfigRegistryKeyName())

    'Write the Value to the SSRS Section
    sec.Write(Section, Value)

    'Print to Debugger
    DebugPrint("Value Written To Registry Section: [" + Section + "]", True)
    DebugPrint("Value Written To Registry Value:   [" + Value + "]", True)

    'Commit Write
    sec.Flush()

End Sub

REM ====================================================================
REM == NAME:ClearRegistrationToken
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: NONE
REM == DESCRIPTION: Clears the Registration of ScreenPlay_SSRS values.
REM ====================================================================
Sub ClearRegistrationKey()

    'Print to Debugger
    DebugPrint("Remove Registry Entry - [ Registry.ClearRegistrationKey() ]")

    'Create a new Registry Object for the ScreenPlay_SSRS section
    section = CreateObject("roRegistrySection", ConfigRegistryKeyName())

    'Remove The Email Address
    section.Delete(ConfigEMAILSectionName())

    'Remove The PIN
    section.Delete(ConfigPINSectionName())
    
    'Remove the ID CODE
    section.Delete(ConfigIDCodeSectionName())

    'Remove The HKEY
    section.Delete(ConfigHKASectionName())

    'Remove the STUDIO SESSION
    section.Delete(ConfigStudioSessionName())

    'Remove the SERIAL NUMBER
    section.Delete(ConfigScreenersSerialNumber())

    'Remove the FIRST NAME
    section.Delete(ConfigFirstName())

    'Remove the LAST NAME
    section.Delete(ConfigLastName())

    'Commit the delete
    section.Flush()

End Sub


REM ====================================================================
REM == NAME:ClearRegistrationToken
REM == INPUT PARAMETERS: 
REM ==      Section: Definition area
REM ==      Value: Value associated with Section
REM == OUTPUT: NONE
REM == DESCRIPTION: Clears the Registration For A Given Registy Section
REM ====================================================================
Sub ClearRegistrationSection(Section As String)

    'Print to Debugger
    DebugPrint("Remove Registry Entry: " + Section + " - [ Registry.ClearRegistrationKey() ]")

    'Create a new Registry Object for the ScreenPlay_SSRS section
    section = CreateObject("roRegistrySection", ConfigRegistryKeyName())

    'Remove The Email Address
    section.Delete(Section)

    'Commit the delete
    section.Flush()

End Sub













