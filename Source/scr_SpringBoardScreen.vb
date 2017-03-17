REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: Sept 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and sets up the Springboard Screen
REM ==
REM ********************************************************************
REM ********************************************************************


REM ====================================================================
REM == NAME: InitSpingBoardScreen()
REM == INPUT PARAMETERS:
REM ==      MovieTitle: Used in the breadcrumb
REM == OUTPUT: Returns initialized SpringBoard Screen
REM == DESCRIPTION: 
REM ====================================================================
Function InitSpingBoardScreen(MovieTitle As String) As Object

    'Print to Debugger
    DebugPrint("Initalize SpringBoard Screen - [ scr_SpringBoardScreen.InitSpringBoardScreen() ]")

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Screen
    screen = CreateObject("roSpringboardScreen")

    'Set BreadCrumb
    screen.SetBreadcrumbText("ScreenPlay Screening Room", MovieTitle)

    'Set The Port to the Screen
    screen.SetMessagePort(port)

    'Return the Screen
    Return screen

End Function


REM ====================================================================
REM == NAME: ShowSpringBoardScreen()
REM == INPUT PARAMETERS:
REM == OUTPUT: Returns True or False depending if screen was loaded
REM == DESCRIPTION: Sets up and loads the Springboard Screen with Movie
REM == selected on the scr_MoviesScreen
REM ====================================================================
Function ShowSpringBoardScreen(SpringBoardData As Object, StudioName As String) As Boolean

    'Print to Debugger
    DebugPrint("Show SpringBoard Screen - [ scr_SpringBoardScreen.ShowSpringBoardScreen() ] ")

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Screen
    screen = CreateObject("roSpringboardScreen")

    'Set The Port to the Screen
    screen.SetMessagePort(port)

    'Set BreadCrumb
    screen.SetBreadcrumbText(StudioName, SpringBoardData.Lookup("Title"))

    'Set Description
    screen.SetDescriptionStyle("Movie")

    'Clear Buttons
    screen.ClearButtons()

    'Create Buttons
    screen.AddButton(1, "Play Movie")

    'Check That A Trailer Is Available
    If Not (SpringboardData.Lookup("trailerURL") = "NOFILE") Then

        'Add Trailer Button Only When Trailer Is Available
        screen.AddButton(2, "Play Trailer")

    End If

    'Add Other Buttons
    screen.AddButton(5, "Resume Play")
    screen.AddButton(3, "Back to Movie List")
    'screen.AddButton(4, "Bookmark")

    'Set Poster Display - NOTE: Overrides ContentType
    'screen.SetPosterStyle("multiple-portrait-generic")

    'Set the default Start Position of the title
    StartPosition = 0

    'Set Items To Screen
    screen.SetContent(SpringBoardData)

    'Disable The Star Rating
    screen.SetStaticRatingEnabled(False)

    'Display Springboard
    screen.Show()

    'Wait For An Item To Be Selected
    While True

        'Set the Wait on User Action
        Msg = wait(0, screen.GetMessagePort())

        If Msg.isScreenClosed() Then

            'Print To Debugger
            DebugPrint("Exit Springboard Screen",True)

            'Exit To Titles Screen
            Exit While

        ElseIf Msg.isButtonPressed() Then '// User Selected A Button Choice

            'Print To Debugger
            DebugPrint("Selected Index: " + ValueToString(Msg.GetIndex()),True)

            'Determine What Action To Take Based On User Choice
            If(Msg.GetIndex() = 1) Then '// Selected PLAY

                'Create Video Screen
                video = ShowVideoScreen(SpringBoardData, "MOVIE", StartPosition)

            ElseIf(Msg.GetIndex() = 2) Then '// Selected TRAILER

                'Create Video Screen
                video = ShowVideoScreen(SpringBoardData, "TRAILER", StartPosition)

            ElseIf(Msg.GetIndex() = 3) Then '// Selected BACK

                'Exit to Titles Screen
                Exit While

            ElseIf(Msg.GetIndex() = 4) Then '// Selected BOOKMARK

                'Display "Loading <Title>..." To User
                msgPleaseWait = ShowPleaseWait("Saving '" + SpringBoardData.Lookup("Title") + "' to your Bookmarks...", "")

                'Give Impression that something is happening
                sleep(2500)

                'Exit to Titles Screen
                Exit While

            ElseIf(Msg.GetIndex() = 5) Then '// Selected RESUME PLAY

                'Get Value From Registry/C2MX
                'playBackPosition = GetOrSetPlayBackPosition(ValueToString(SpringBoardData.Lookup("screenPlayID")),true)

                'Get Playback Position from registry
                playBackPosition = GetRegistryValue(ValueToString(SpringBoardData.Lookup("screenPlayID")))

                'Print to debugger
                DebugPrint("ScreenplayID: " + ValueToString(SpringBoardData.Lookup("screenPlayID")), true)
                DebugPrint("Playback Position: " + ValueToString(playbackPosition), true)

                'Determine if a value was found
                If Not(ValueToString(playBackPosition) = "ERROR") then

                    'Set Playback Position to be passed to video screen 
                    StartPosition = playBackPosition
                    
                End If

                'Load Title
                video = ShowVideoScreen(SpringBoardData, "MOVIE", StartPosition.ToInt())

            End If

        End If

    End While

End Function

