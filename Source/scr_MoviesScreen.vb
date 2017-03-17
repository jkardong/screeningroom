REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: Sept 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and sets up the Movies Screen
REM ==
REM ********************************************************************
REM ********************************************************************


REM ====================================================================
REM == NAME: InitMoviesScreen()
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: Outputs the instanciated roPosterScreen
REM == DESCRIPTION: A basic return of a initiated Poster Screen.
REM ====================================================================
Function InitMoviesScreen() As Object

    'Print to Debugger
    DebugPrint("Initalize Movies Screen - [ scr_MoviesScreen.InitMoviesScreen() ]")

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Screen
    screen = CreateObject("roPosterScreen")

    'Set BreadCrumb - Default - Reset In ShowMoviesScreen()
    screen.SetBreadcrumbText("ScreenPlay Screening Room","")

    'Set The Port to the Screen
    screen.SetMessagePort(port)

    'Return the Screen
    Return screen

End Function


REM ====================================================================
REM == NAME: ShowMoviesScreen()
REM == INPUT PARAMETERS:
REM ==      Screen: Expects a roPosterScreen object 
REM ==      StuiodKey: Unique key to be provided to C2ME 
REM == OUTPUT: Returns True or False depending if screen was loaded
REM == DESCRIPTION: Sets up and loads the Poster Screen with Movies
REM == that are available to logged in user.
REM ====================================================================
Function ShowMoviesScreen(Screen As Object, StudioKey As String) As Boolean

    'Print to Debugger
    DebugPrint("Show Movies Screen - [ scr_StudiosScreen.ShowStudiosScreen() ] ")

    'Set the Look of the Images
    screen.SetListStyle(ConfigMovieThemeType())

    'Get The Movies XML
    arrayMoviesMetadata = GetMoviesXML(StudioKey)

    'Determine if XML Had an issue
    If(arrayMoviesMetadata.Lookup("ERROR") = "ERROR")

        'Get Login Error
        dialog = ConfigDisplayMessage("XMLERROR",False)

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

                    'Print To Debugger
                    DebugPrint("Error Displayed To User")

                    'Exit
                    Exit While

                End If
            End If
        End While

        'Exit
        Return False

    End If

    'Add To Screen for UI
    screen.SetContentList(arrayMoviesMetadata.MovieItemsArray)

    'Get Metadata From XML Header
    aa_XMLHeaderData = arrayMoviesMetadata.XMLHeaderData

    'Set Studio Name
    screen.SetBreadcrumbText("Studios",aa_XMLHeaderData.Lookup("StudioName"))

    'Set New Theme
    If(aa_XMLHeaderData.Lookup("StudioName") = "Walt Disney Pictures") Then

        'Set Theme For Disney
        InitializeTheme(True)
    Else

        'Set Theme Standard
        InitializeTheme(False)

    End If

    'Display The Screen
    screen.Show()

    'Wait For An Item To Be Selected
    While True

        'Set the Wait on User Action
        msg = wait(0, screen.GetMessagePort())

        'Confirm user action is a event of Poster Screen
        If type(msg) = "roPosterScreenEvent" Then

            'Determine What Was Selected In Movies Screen
            If msg.isListItemSelected() Then

                'Get The Index of the UI Item Selected
                index = msg.GetIndex()

                'Print To Debugger
                DebugPrint("Initalize Springboard Screen for Selected Index: " + Stri(index))

                'Display "Loading..." To User
                msgPleaseWait = ShowPleaseWait("Loading...", "")

                'Get Assoicative Array associated with selected index
                aa_Title = ReturnTitleAssociativeArray(arrayMoviesMetadata.CollectionOfTitles, index)

                'Create Springboard Screen
                ShowSpringBoardScreen(aa_Title, aa_XMLHeaderData.Lookup("StudioName"))

            ElseIf msg.isScreenClosed() Then 'User Closed Screen

                'Set Theme Back To Standard
                InitializeTheme(False)

                'Print To Debugger
                DebugPrint("User Exited Studio Screen")

                'Return Exit
                Return 0

            End If

        End If

    End While

End Function