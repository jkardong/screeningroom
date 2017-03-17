REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: Aug 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and sets up the Studios Screen
REM ==
REM ********************************************************************
REM ********************************************************************


REM ====================================================================
REM == NAME: InitStudiosScreen()
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: Returns roPosterScreen object
REM == DESCRIPTION: Inializes the Poster Screen that displays all studios
REM == that are available to the user
REM ====================================================================
Function InitStudiosScreen() As Object

    'Print to Debugger
    print("Initalize Studios Screen - [ scr_StudiosScreen.InitStudiosScreen() ]")

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Screen
    screen = CreateObject("roPosterScreen")

    'Set BreadCrumb
    screen.SetBreadcrumbText("Studios","")

    'Set The Port to the Screen
    screen.SetMessagePort(port)

    'Return the Screen
    Return screen

End Function

REM ====================================================================
REM == NAME: ShowStudiosScreen()
REM == INPUT PARAMETERS:
REM ==      screen: Expects a roPosterScreen object 
REM == OUTPUT: Returns True or False depending if screen was loaded
REM == DESCRIPTION: Sets up and loads the Poster Screen with Studios
REM == that are available to logged in user.
REM ====================================================================
Function ShowStudiosScreen(screen As Object) As Boolean

    'Print to Debugger
    print("Show Studios Screen - [ scr_StudiosScreen.ShowStudiosScreen() ] ")

    'Set Initial Status Flag
    IsError = False

    'Set the Look of the Images
    screen.SetListStyle(ConfigSSRSThemeType())

    'Get The Studios XML
    arrayStudioMetadata = GetStudiosXML()

    'Get Status
    aa_Status = arrayStudioMetadata.Status

    'Set Flag
    isStudio = False

    'Check Error Status
    If (aa_Status.Lookup("NOSTUDIOS") = "NOSTUDIOS") Then '// No Studios Returned In XML

        'Get Login Error
        dialog = ConfigDisplayMessage("NOSTUDIOS",False)

        'Toggle Error
        IsError = true

        'Set Is A Studio Flag
        isStudio = true

    ElseIf (aa_Status.Lookup("XMLERROR") = "XMLERROR") Then '// Error Parsing the XML or Invalid Values Returned

        'Get Login Error
        dialog = ConfigDisplayMessage("XMLERROR",False)

        'Toggle Error
        IsError = true

    End If

    'Determine If there was an error
    If(IsError) Then

        'Show Dialog
        dialog.Show()

        'Wait For User Interaction
        While true

            'Wait For User To Click
            MsgBox = wait(0, dialog.GetMessagePort()) 

            'If User Makes a UI Selection
            If Type(MsgBox) = "roMessageDialogEvent"

                'If User Select OK, Take Action
                If MsgBox.GetIndex() = 1

                    If(isStudio) Then 

                        'Exit To Studios Screen
                        Exit While

                    ElseIf Not(isStudio) Then 

                        'Exit Application
                        End

                    End If

                End If
            End If
        End While

    End If

    'Add To Screen for UI
    screen.SetContentList(arrayStudioMetadata.StudioItemsArray)

    'Display The Screen
    screen.Show()

    'Wait For An Item To Be Selected
    While True

        'Set the Wait on User Action
        msg = wait(0, screen.GetMessagePort())

        'Confirm user action is a event of Poster Screen
        If type(msg) = "roPosterScreenEvent" Then

            'Determine What Was Selected
            If msg.isListItemSelected() Then

                'Get The Index of the UI Item Selected
                index = msg.GetIndex()

                'Print To Debugger
                DebugPrint("Selected Studio Index: " + Stri(index))

                'Get The Studio Key From XML Associative Array
                studioKey = ParseStudioKey(arrayStudioMetadata.StudioData, index)

                'Create Movies Screen or Throw Error
                If (studioKey = "ERROR") then

                    'Get Login Error
                    dialog = ConfigDisplayMessage("MOVIES", False)

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

                                    'Display "Loading..." To User
                                    msgPleaseWait = ShowPleaseWait("Loading...", "")

                                    'Reinitialize The Studios Screen
                                    StudiosScreen = InitStudiosScreen()

                                    'Display Home Screen
                                    ShowStudiosScreen(StudiosScreen)

                                    'Close Dialog Box
                                    Exit While

                                End If
                            End If
                        End While
                    End If

                Else ' No Error Obtaining Studio Key, Inialize the Movies Screen

                    'Determine If User Selected a Studio or the Settings Icon
                    If(studioKey = "Settings") Then

                        'Print to Debugger
                        DebugPrint("Initalize Settings Screen")

                        'Initalize The Settings Screen
                        DisplaySettingsScreen()

                    Else

                        'Print To Debugger
                        DebugPrint("Initalize Movies Screen")

                        'Initialize Movies Screen
                        MoviesScreen = InitMoviesScreen()

                        'Create Movies Screen
                        ShowMoviesScreen(MoviesScreen, studioKey)

                    End If

                End If

            ElseIf msg.isScreenClosed() Then 'User Closed Screen

                'Print To Debugger
                DebugPrint("User Exited Studio Screen")

                'Return Exit
                Return 0

            End If

        End If

    End While

End Function


REM ====================================================================
REM == NAME: AddSettingsOption()
REM == INPUT PARAMETERS:
REM == OUTPUT: None
REM == DESCRIPTION: Creates the Settting menu option on the Studios Screen
REM ====================================================================
Function AddSettingsOption() As Object

    'Print to Debugger
    print("Add Settings Options To Studios Screen - [ scr_StudiosScreen.AddSettingsOption() ] ")

    'Create Associative Array Of XML Elements
    aa_PosterItems = CreateObject("roAssociativeArray")

    'Define Content
    aa_PosterItems.ContentType = "episode"

    'Add the Metadata from XML to Associative Array
    aa_PosterItems.ShortDescriptionLine1 = "Screening Room Settings"
    aa_PosterItems.ShortDescriptionLine2 = "Maintenance and Details"
    aa_PosterItems.SDPosterUrl = "pkg:/images/StudioScreen/Studio_Settings_SD.jpg"
    aa_PosterItems.HDPosterUrl = "pkg:/images/StudioScreen/Studio_Settings_HD.jpg"

    'Return AA
    Return aa_PosterItems

End Function

