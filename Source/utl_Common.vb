REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Helper methods and utilities.  This is used through
REM ==  the SSRS as 
REM ==
REM ********************************************************************
REM ********************************************************************



REM ====================================================================
REM == NAME: InitalizeTheme
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: NONE
REM == DESCRIPTION: Configure the custom overhang and Logo attributes
REM == Theme attributes affect the branding of the application
REM == and are artwork, colors and offsets specific to the app
REM ====================================================================
Sub InitializeTheme(IsSpecialTheme As Boolean)

    'Print to Debugger
    DebugPrint("Initializing Theme - [ utl_Common.InitializeTheme() ]")

    'Create UI elements
    app = CreateObject("roAppManager")

    'Create Associative Array of Values
    theme = CreateObject("roAssociativeArray")

    'General Look And Feel
    theme.BreadcrumbTextRight = "#d9d9d9" 'top right-right text (ex: Living Temples)
    theme.BreadcrumbTextLeft = "#d9d9d9" 'top right-left text (ex: Third Planet)
    theme.BreadcrumbDelimiter = "#d9d9d9" 'top right-middle (ex: * )
    theme.DialogTitleText = "#d9d9d9" 'Sets text color on pop-up screens and such
    theme.DialogBodyText = "#d9d9d9" 'http://forums.roku.com/viewtopic.php?f=34&t=40358&p=269716&hilit=ThemeType#p269716
    theme.ThemeType = "generic-dark" 'Sets the color scheme so text can be read

    'Buttons
    theme.ButtonMenuNormalOverlayText = "E60E41"
    theme.ButtonMenuHighlightText = "61E60E"

    'Settings Screen (Paragraph Screen)
    theme.ParagraphBodyText = "#d9d9d9" 'Text on page body - "This screen allows you to..."
    theme.ParagraphHeaderText = "#d9d9d9" 'Header Text on page - "ScreenPlay Screening Room Setttings"
    theme.ButtonNormalColor = "#a4a4a4" 'Color of button text when not selected by user
    theme.ButtonHighlightColor = "#81c0ff" 'Color of button text when selected by user

    'Springboard Button Color
    theme.ButtonMenuHighlightText = "#81c0ff" 'Selected Button Color
    theme.ButtonMenuNormalText = "#a4a4a4" 'Normal Button Color - Un-Selected

    'Studios Screen
    theme.PosterScreenLine1Text = "#d9d9d9" 'Studio Name - Warner Brothers
    theme.PosterScreenLine2Text = "#d9d9d9" 'Titles Available - Titles Available: 10

    'Standard Definition Images
    theme.OverhangOffsetSD_X = "30"
    theme.OverhangOffsetSD_Y = "30"
    
    'High Definition Images
    theme.OverhangOffsetHD_X = "125"
    theme.OverhangOffsetHD_Y = "35"

    'Springboard Screen
    theme.SpringboardTitleText = "#ffffff" 'Movie Title Above "Play" button - Lord of the Rings
    theme.SpringboardActorColor = "#ffffff" 'Actor List - Michael Douglas Susan Sarandon
    theme.SpringboardSynopsisColor = "#a4a4a4" 'Title description under Actors - "Two small dudes foil a floating eye"
    theme.SpringboardGenreColor = "#a4a4a4" 'Genre - Horror, Adventure
    theme.SpringboardDirectorColor = "#ffffff" 'Director Name - Peter Jackson
    theme.SpringboardDirectorLabel = "#ffffff" 'Director Title - Director
    theme.SpringboardTitleText = "#ffffff" 'Movie Title Above "Play" button - Lord of the Rings
    theme.SpringboardActorColor = "#ffffff" 'Actor List
    theme.SpringboardSynopsisText = "#a4a4a4" 'Movie description - "Two little guys foil a floating eye..."
    theme.SpringboardGenreColor = "#a4a4a4" 'Genre - Adventure
    theme.SpringboardRuntimeColor = "#a4a4a4" '2h 02m

    'Springboard Button Color
    theme.ButtonMenuHighlightText = "#81c0ff" 'Selected Button Color
    theme.ButtonMenuNormalText = "#a4a4a4" 'Un-Selected Button Color

    'Set Header 
    If (IsSpecialTheme) Then

        'Header
        theme.OverhangSliceHD = "pkg:/images/Theme/Custom/Disney_Overhang_HD.png"
        theme.OverhangSliceSD = "pkg:/images/Theme/title_bar_SD.png"

        'Background Color
        theme.BackgroundColor = "#0e3f7b" 'background

    Else

        'Header
        theme.OverhangSliceHD = "pkg:/images/Theme/title_bar_HD.png"
        theme.OverhangSliceSD = "pkg:/images/Theme/title_bar_SD.png"

        'Background Color
        theme.BackgroundColor = "#1b1d25" 'background for all app

    End If
   
    'Set Theme
    app.SetTheme(theme)

End Sub


REM ====================================================================
REM == NAME:DebugPrint
REM == INPUT PARAMETERS: 
REM ==      Message: Message to print
REM ==      ChildMessage: Determines whether or not to add space
REM == OUTPUT: NONE
REM == DESCRIPTION: Prints message to debugger and debugger only
REM ====================================================================
Sub DebugPrint(Message As String, ChildMessage=false)

    'Print the statement
    If (ChildMessage) Then

        'Add Spaces To Nest For Visual Sanity
        Print("     " + Message)

    Else

        'Show Message
        Print(Message)

    End If

End Sub

REM ====================================================================
REM == NAME: ShowMessage
REM == INPUT PARAMETERS:
REM ==      Title: Title of Messagebox
REM ==      Message: Message to display to the UI
REM == OUTPUT: NONE
REM == DESCRIPTION: Displays a message box to the Roku UI that the user
REM == can take an action on if desired.
REM ====================================================================
Function ShowMessage(Title As String, Message As String, ButtonText As String, ShowBusy As Boolean) As Object

    'Print to Debugger
    DebugPrint("ShowMessage - [ Common.ShowMessage() ]")

    'Create a Message Port
    port = CreateObject("roMessagePort")

    'Create a Message Dialog
    dialog = CreateObject("roMessageDialog")

    'Associate the Port to the Message
    dialog.SetMessagePort(port)

    'Set the Title of the Message
    dialog.SetTitle(title)

    'Set the Message Body
    dialog.SetText(message)

    'Add a button to Message Box
    dialog.AddButton(1, ButtonText)

    'Show Spinning Wheel
    If(ShowBusy) Then
        dialog.ShowBusyAnimation()
    End If

    'Display the Message Box To UI
    dialog.Show()

    'Display and wait for user action
    while true
        
        'Create Dialog Object
        dlgMsg = wait(0, dialog.GetMessagePort())
        
        'If User Makes A Action, Begin Watch
        if type(dlgMsg) = "roMessageDialogEvent"

            'If User Selects Button #1, Exit
            If dlgMsg.GetIndex() = 1 Then
                Exit While
            End If

        end if
    end while

    'Return port
    Return dialog

End Function


REM ====================================================================
REM == NAME: ShowMessageDialog
REM == INPUT PARAMETERS:
REM ==      Values: All Values to be displayed in the UI
REM == OUTPUT: NONE
REM == DESCRIPTION: Creates message box to display to user based on 
REM == user selection in the UI. Called from Config.ConfigDisplayMessage()
REM ====================================================================
Function ShowMessageDialog(Values As Object) As Object

    'Print to Debugger
    DebugPrint("ShowMessage - [ utl_Common.ShowMessageDialog() ]", True)

    'Create a Message Port
    port = CreateObject("roMessagePort")

    'Create a Message Dialog
    MsgBox = CreateObject("roMessageDialog")

    'Associate the Port to the Message
    MsgBox.SetMessagePort(port)

    'Get Button Associative Array
    aa_Buttons = Values.Lookup("Buttons")

    'Get Text Associative Array
    aa_TextMessage = Values.Lookup("MessageDetails")

    'Get Count Of Buttons
    buttonCount = GetCountFromObject(aa_Buttons)

    'Get Message Title
    title = aa_TextMessage.Lookup("Title")

    'Get Message Body Text
    message = aa_TextMessage.Lookup("Message")

    'Get Show Busy
    showBusy = aa_TextMessage.Lookup("ShowBusy")

    'Print Values To Debugger
    DebugPrint("Button Count: " + ValueToString(buttonCount), True)
    DebugPrint("Title: " + title, True)
    DebugPrint("Message: " + message, True)
    
    'Set the Title of the Message
    MsgBox.SetTitle(title)

    'Set the Message Body
    MsgBox.SetText(message)

    'Add a button(s) to Message Box
    For i = 1 to buttonCount

        'Add Button and Text
        MsgBox.AddButton(i, aa_Buttons.Lookup(ValueToString(i)))

        'Print Button To Debugger
        DebugPrint("Button Text: " + aa_Buttons.Lookup(ValueToString(i)), True)

    Next
    
    'Show Spinning Wheel If Specified
    If(showBusy = "True") Then
        MsgBox.ShowBusyAnimation()
    End If

    'Return port
    Return MsgBox

End Function


REM ====================================================================
REM == NAME: ShowPleaseWait()
REM == INPUT PARAMETERS:
REM ==      title: Sets the Header Text
REM ==      text: Sets the body Text 
REM == OUTPUT: Returns roMessageDialog or roOneLineDialog
REM == DESCRIPTION: Shows the "Loading..." type screen that is displayed
REM == when a screen is loading or when a title is pulling from Akamai
REM ====================================================================
Function ShowPleaseWait(title As dynamic, text As dynamic) As Object

    'Print To Debugger
    DebugPrint("Set Please Wait Dialog", True)

    'Ensure that values are Populated, if not, give a default
    if not isstr(title) title = "Loading"
    if not isstr(text) text = "Please Wait..."

    'Create Message Port
    port = CreateObject("roMessagePort")

    'Set Default Value to Invalid for Dialog
    dialog = invalid

    'If No Text, Then Just Show a One Line 
    if text = ""
        
        'Create Dialog
        dialog = CreateObject("roOneLineDialog")

    else '//Text Found

        'Create Message Dialog
        dialog = CreateObject("roMessageDialog")

        'Set The Text
        dialog.SetText(text)

    endif

    'Set The Message Port
    dialog.SetMessagePort(port)

    'Set Title
    dialog.SetTitle(title)

    'Show the Spinning Wheel
    dialog.ShowBusyAnimation()

    'Display
    dialog.Show()

    'Return Populated Dialog
    return dialog

End Function


REM ====================================================================
REM == NAME: GetRokuIPAddress
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: String
REM == DESCRIPTION: Finds and returns the IP Address for Roku
REM ====================================================================
Function GetRokuIPAddress() As String

    'Debug Code
    DebugPrint("Get Roku IP Address: [ Common.GetRokuIPAddress() ]", True)

    'Set Device Info
    deviceInfo = CreateObject("roDeviceInfo")

    'Set All Values
    ipAddresses = deviceInfo.GetIPAddrs()

    'Get IP Address
    For Each key In ipAddresses

        'Set the Value From Array
        ipAddress = ipAddresses[key]

        'Determine if value is the IP Address
        If ipAddress <> invalid And ipAddress.Len() > 0 Then

            'Debug Code
            DebugPrint("IP Address: " + ipAddress, True)

            'Return IP Address
            Return ipAddress

        End If
    Next

    'Return Error
    Return "ERROR"

End Function


REM ====================================================================
REM == NAME:PrintTimespan
REM == INPUT PARAMETERS: 
REM ==      IsBegin: Determines if this is the start or end of the recording
REM == OUTPUT: Prints time
REM == DESCRIPTION: Prints message to debugger on time an action took
REM ====================================================================
Sub PrintTimespan(IsBegin As Boolean)

    'Determine if action is start or finish
    If(IsBegin) Then 

        'Create Timer Option
        Timer = CreateObject("roTimespan")

        'Start Time To Now
        Timer.Mark()

    Else

        'Stop Time and Print Timeframe
        DebugPrint("Transaction Time: " + timer.TotalMilliseconds())

    End If

End Sub


REM ====================================================================
REM == NAME: HandleWait
REM == INPUT PARAMETERS: 
REM ==      Dialog: Prepopulated roMessageDialog 
REM == OUTPUT: True or False
REM == DESCRIPTION: Handles the user action of a roMessageDialog. 
REM ====================================================================
Function HandleWait(Dialog As Object) As Boolean

    'Display Dialog To User
    Dialog.Show()

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

                    'User OK's Action
                    Return True

                ElseIf MsgBox.GetIndex() = 2 '// User Selected Cancel/False

                    'Don't Perform Action
                    Return False

                End If

            End If
        End While
    End If

    'Return Default Value
    Return False

End Function


REM ====================================================================
REM == NAME:PrintDateTime()
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: Returns Date as a String
REM == DESCRIPTION: Calculates the datetime and returns today's date
REM ====================================================================
Sub PrintDateTime()

    'Create DateTime Object
    roDate = CreateObject("roDateTime")

    'Start Now
    roDate.mark()

    'Print Date and Time
    DebugPrint(roDate.asDateStringNoParam() + " " +  roDate.getHours().tostr() + ":" + roDate.getMinutes().tostr())

    'Print Spacer
    DebugPrint(" ")

End Sub


REM ====================================================================
REM == NAME:ParseStudioKey()
REM == INPUT PARAMETERS: 
REM ==      KeyList: Associative Array of Keys 
REM ==      SelectedIndex: User selected index in UI
REM == OUTPUT: Returns Studio Key as a String
REM == DESCRIPTION: Takes a associative array of values created in the 
REM == utl_XML.GetStudiosXML method and finds the match to what the 
REM == user selected in the Roku UI
REM ====================================================================
Function ParseStudioKey(KeyList As Object, SelectedIndex As Object) As String

    'Debug Code
    DebugPrint("Return Studio Key From C2ME Data: [ Common.ParseStudioKey() ]", True)

    'Loop Through Values
    For Each Value In KeyList
        
        'Set Value From Associative Array
        foundValue = KeyList.Lookup(Value)
       
        'Action on the Values
        DebugPrint("Selected Index: " + ValueToString(SelectedIndex) + ", Key = " + KeyList.Lookup(ValueToString(SelectedIndex)), True)
                   
        'Return Value
        Return KeyList.Lookup(ValueToString(SelectedIndex))

    End For

    'Saftey Catch
    Return "ERROR"

End Function


REM ====================================================================
REM == NAME:ShowSplashBackground()
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: NONE
REM == DESCRIPTION: Displays a simple splash type background screen
REM ====================================================================
Function ShowSplashBackground() As Object

    'Debug Print
    DebugPrint("Initialize Splash Screen")

    'Create Canvas
    screenFacade = CreateObject("roImageCanvas")

    'Get Mode
    mode = CreateObject("roDeviceInfo").GetDisplayMode()

    'Print Screen Resolution to Debugger
    if (mode = "720p") Then

        'Print To Debugger
        DebugPrint("Screen Setting: 720p", True)

        'Create Associative Array Of Items To Display
        canvasItems = [
                        { 
                            url:"pkg:/images/Splash/Splash_HD.png"
                            TargetRect:{x:0, y:0, w:1280, h:720}
                        }
                      ]

    Else '// Standard Def

        'Print To Debugger
        DebugPrint("Screen Setting: Standard Definition", True)

        'Create Associative Array Of Items To Display
        canvasItems = [
                        { 
                            url:"pkg:/images/Splash/Splash_SD.png"
                            TargetRect:{x:0, y:0, w:720, h:480}
                        }
                      ]

    End If

    'Set The Back Color
    screenFacade.SetLayer(0,{Color:"#1b1d25", CompositionMode:"Source"})

    'Show Images Once All Are Downloaded
    screenFacade.SetRequireAllImagesToDraw(true)

    'Set The Images
    screenFacade.SetLayer(1,canvasItems)

    'Return Screen
    Return screenFacade

End Function


REM ====================================================================
REM == NAME:ReturnTitleAssociativeArray()
REM == INPUT PARAMETERS: 
REM ==      ArrayOfArrays: Associative Array of Associative Arrays 
REM ==      SelectedIndex: User selected index in UI
REM == OUTPUT: Returns Associative Array
REM == DESCRIPTION: Finds the metadata for a selected title. The Associative
REM == Array has an array of populated Associative Arrays listed by an 
REM == index. Creation is in utl_XML.GetMoviesXML(). Called from 
REM == scr_MoviesScreen.ShowMoviesScreen()
REM ====================================================================
Function ReturnTitleAssociativeArray(ArrayOfArrays As Object, SelectedIndex As Integer) As Object

    'Set Associative Array
    SpringBoardItems = ArrayOfArrays.Lookup(ValueToString(SelectedIndex))

    'Check Associative Array to ensure it was loaded
    If SpringBoardItems = Invalid then

        'Create Error Associative Array For Return For Display
        SpringBoardItems = { ContentType:"Movie"
            SDPosterUrl:""
            HDPosterUrl:""
            IsHD:False
            HDBranded:False
            ShortDescriptionLine1:"ERROR - There was an error loading your movie - ERROR"
            ShortDescriptionLine2:"Screening Room has encountered a failure"
            Description:"It appears that there was an issue loading your selected movie. Please contact " + ConfigSSRSSupportEmailAddress() + " with details of the error."
            Categories:["Error","Talk"]
            Title:"Screening Room Error Loading Movie"
        }

    End If

    'Return Selected Associative Array
    Return SpringBoardItems

End Function


REM ====================================================================
REM == NAME:GetCountFromObject()
REM == INPUT PARAMETERS: 
REM ==      ObjectToCount: Associative Array to get count from 
REM == OUTPUT: Returns Count Integer
REM == DESCRIPTION: Takes a associative array of values and returns the 
REM == count from that AA.
REM ====================================================================
Function GetCountFromObject(ObjectToCount As Object) As Integer

    'Loop Through The Object
    count=0:for each object in ObjectToCount:count=count+1:next:?count

    'Return The Count
    Return count

End Function


REM ====================================================================
REM == NAME: ValueToString()
REM == INPUT PARAMETERS:
REM ==      value: Value to convert to string 
REM == SAMPLE CALL: Foo = ValueToString(123)
REM == OUTPUT: Returns as formatted string
REM == DESCRIPTION: Takes a value and determines what type it is and converts
REM == to a string
REM ====================================================================
Function ValueToString(value)

    'Set Type
    ret = AnyToString(value)

    'Return Type if Invalid
    if ret = invalid ret = type(value)

    'Failsafe
    if ret = invalid ret = "unknown" 

    'Return Value
    return ret

End Function


REM ====================================================================
REM == NAME: AnyToString()
REM == INPUT PARAMETERS:
REM ==      Value: Value to parse 
REM == SAMPLE CALL: Foo = AnyToString(123)
REM == OUTPUT: Returns a dynamic type
REM == DESCRIPTION: Takes a value and runs a conversion. Basically this
REM == is a wrapper function for ValueToString()
REM ====================================================================
Function AnyToString(Value As Dynamic) As Dynamic
    if Value = invalid return "invalid"
    if isstr(Value) return Value
    if isint(Value) return itostr(Value)
    if isbool(Value)
        if Value = true return "true"
        return "false"
    endif
    if isfloat(Value) return Str(Value)
    if type(Value) = "roTimespan" return itostr(Value.TotalMilliseconds()) + "ms"
    return invalid
End Function


REM ====================================================================
REM == NAME: isstr()
REM == INPUT PARAMETERS: 
REM ==      obj: Dynamic typed input value
REM == SAMPLE CALL: if(isstr(123)) then : foo
REM == OUTPUT: Returns True or False
REM == DESCRIPTION: Determines if a value is a string or not
REM ====================================================================
Function isstr(obj as dynamic) As Boolean
    if obj = invalid return false
    if GetInterface(obj, "ifString") = invalid return false
    return true
End Function


REM ====================================================================
REM == NAME: isint()
REM == INPUT PARAMETERS: 
REM ==      obj: Dynamic typed input value
REM == SAMPLE CALL: if(isint(123)) then : foo
REM == OUTPUT: Returns True or False
REM == DESCRIPTION: Determines if input value is a integer. If so it 
REM == will return TRUE else FALSE
REM ====================================================================
Function isint(obj as dynamic) As Boolean
    if obj = invalid return false
    if GetInterface(obj, "ifInt") = invalid return false
    return true
End Function


REM ====================================================================
REM == NAME: isbool()
REM == INPUT PARAMETERS: 
REM ==      obj: Dynamic typed input value
REM == SAMPLE CALL: if(isbool(123)) then : foo
REM == OUTPUT: Returns True or False
REM == DESCRIPTION: Determines if input value is a boolean. If so it 
REM == will return TRUE else FALSE
REM ====================================================================
Function isbool(obj as dynamic) As Boolean
    if obj = invalid return false
    if GetInterface(obj, "ifBoolean") = invalid return false
    return true
End Function


REM ====================================================================
REM == NAME: isfloat()
REM == INPUT PARAMETERS: 
REM ==      obj: Dynamic typed input value
REM == SAMPLE CALL: if(isfloat(12345687)) then : foo
REM == OUTPUT: Returns True or False
REM == DESCRIPTION: Determines if input value is a float. If so it 
REM == will return TRUE else FALSE
REM ====================================================================
Function isfloat(obj as dynamic) As Boolean
    if obj = invalid return false
    if GetInterface(obj, "ifFloat") = invalid return false
    return true
End Function


REM ====================================================================
REM == NAME: itostr()
REM == INPUT PARAMETERS: 
REM ==      i: Integer typed input value
REM == SAMPLE CALL:
REM == OUTPUT: Returns a string
REM == DESCRIPTION: Returns a trimmed string
REM ====================================================================
Function itostr(i As Integer) As String
    str = Stri(i)
    return strTrim(str)
End Function


REM ====================================================================
REM == NAME: strTrim()
REM == INPUT PARAMETERS: 
REM ==      str: Input value of a string to be trimmed
REM == SAMPLE CALL: foo = strTrim("King Kong ")
REM == OUTPUT: String
REM == DESCRIPTION: Does a simple trim of data
REM ====================================================================
Function strTrim(str As String) As String
    st=CreateObject("roString")
    st.SetString(str)
    return st.Trim()
End Function