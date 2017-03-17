REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  Screenplay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: Screenplay Labs 2011
REM ==  Created: July 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Stuff that need not get forgotton but not implemented
REM ==
REM ********************************************************************
REM ********************************************************************


Function ShowCustomScreen() As Boolean


    'Print to Debugger
    print("Initalize Movies Screen - [ scr_MoviesScreen.InitMoviesScreen() ]")

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Screen
    screen = CreateObject("roPosterScreen")

    'Set BreadCrumb - Default - Reset In ShowMoviesScreen()
    screen.SetBreadcrumbText("Screenplay Screening Room","")

    'Set The Port to the Screen
    screen.SetMessagePort(port)

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

                'Print To Debugger
                DebugPrint("User Exited Studio Screen")

                'Return Exit
                Return 0

            End If

        End If

    End While


End Function
