REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: Sept 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and sets up the Settings Screen
REM ==
REM ********************************************************************
REM ********************************************************************


REM ====================================================================
REM == NAME: DisplaySettingsScreen()
REM == INPUT PARAMETERS: NONE
REM == OUTPUT: Returns roParagraphScreen
REM == DESCRIPTION: Initalizes the Settings Screen 
REM ====================================================================
Function DisplaySettingsScreen() As Object

    'Print to Debugger
    print("Initalize Settings Screen - [ scr_Settings.InitStudiosScreen() ]")

    'Set the Port Object
    port = CreateObject("roMessagePort")

    'Create Screen
    screen = CreateObject("roParagraphScreen")

    'Set BreadCrumb
    screen.SetBreadcrumbText(ConfigSettingsText(),"")

    'Set The Port to the Screen
    screen.SetMessagePort(port)

    'Set Text
    'screen.AddHeaderText("ScreenPlay Screening Room Settings")
    screen.AddParagraph(ConfigGetSettingsText())

    'Add Buttons
    'screen.AddButton(1, "Email Bookmarks")
    screen.AddButton(2, "About Screening Room")
    screen.AddButton(3, "Version")
    screen.AddButton(4, "Support")
    screen.AddButton(5, "About ScreenPlay, Inc")
    screen.AddButton(7, "Privacy Statement")
    screen.AddButton(6, "Reset Application")

    'Show Screen
    screen.show()

    'Wait For User Action
    While True

        'Create Message Port Listener
        MsgBox = wait(0, screen.GetMessagePort())

        'Determine What Action A User Has Selected
        If Type(MsgBox) = "roParagraphScreenEvent" Then

            'Print To Debugger
            DebugPrint("User Selected Index: " + ValueToString(MsgBox.GetIndex()), True)
            
            'Determine User Section
            If (MsgBox.GetIndex() = 1) Then 'User Selected Email
 
                'Show Confirm
                dialog = ConfigDisplayMessage("BOOKMARKS", False)

                'Show Message To User
                dialog.Show()

                'Wait For User Action
                While true

                    'Wait For User To Click
                    MsgBox = wait(0, dialog.GetMessagePort()) 

                    'If User Makes a UI Selection
                    If Type(MsgBox) = "roMessageDialogEvent"

                        'If User Select OK, Take Action
                        If MsgBox.GetIndex() = 1

                            'Print To Debugger
                            DebugPrint("Email Bookmarks")

                            'Display "Loading <Title>..." To User
                            msgPleaseWait = ShowPleaseWait("Emailing your Bookmarks...", "")

                            'Give Impression that something is happening
                            sleep(2500)

                            'Exit
                            Exit While

                        Else '// User Does not want to clear registry

                            'Print To Debugger
                            DebugPrint("User Exited Bookmarks")

                            'Display "Loading..." To User
                            msgPleaseWait = ShowPleaseWait("Loading...", "")

                            'Exit
                            Exit While

                        End If
                    End If
                End While

            ElseIf (MsgBox.GetIndex() = 6) Then 'User Selected Reset 

                'Show Warning Dialog
                dialog = ConfigDisplayMessage("RESET", False)

                'Show The Message Dialog
                dialog.show()

                'Check Action Type
                If Type(dialog) = "roMessageDialog" Then

                    'Wait For User Action
                    While true

                        'Wait For User To Click
                        MsgBox = wait(0, dialog.GetMessagePort()) 

                        'If User Makes a UI Selection
                        If Type(MsgBox) = "roMessageDialogEvent"

                            'If User Select OK, Take Action
                            If MsgBox.GetIndex() = 1

                                'Print To Debugger
                                DebugPrint("Clear Registry of all SSRS Values")

                                'Display "Loading..." To User
                                msgPleaseWait = ShowPleaseWait("Clearing Registry...", "")

                                'Clear The Registry
                                ClearRegistrationKey()

                                'End
                                End

                                'Exit
                                Exit While

                            Else '// User Does not want to clear registry

                                'Print To Debugger
                                DebugPrint("User Exited Registry Clear")

                                'Display "Loading..." To User
                                msgPleaseWait = ShowPleaseWait("Loading...", "")

                                'Exit
                                Exit While

                            End If
                        End If
                    End While
                End If

            Else '// User Selected Option That Does Not Requre a Pop-Up Warning

                'Set Default User Selection
                selectionArea = "ABOUT"

                'Determine What Action Was Taken
                If(MsgBox.GetIndex() = 2) Then 
                    selectionArea = "ABOUT"
                ElseIf (MsgBox.GetIndex() = 3) Then
                    selectionArea = "VERSION"
                ElseIf (MsgBox.GetIndex() = 4) Then
                    selectionArea = "SUPPORT"
                ElseIf (MsgBox.GetIndex() = 5) Then
                    selectionArea = "SCREENPLAY"
                ElseIf (MsgBox.GetIndex() = 7) Then
                    selectionArea = "LICENSE"
                End If

                'Create Settings Detail Screen
                settingsDetail = DisplaySettingsDetailScreen()

                'Populate the Screen Text
                settingsDetail = ConfigSetMultiLineScreenText(settingsDetail, selectionArea)

                'Show Form
                settingsDetail.Show()

                'Wait For User Action
                While true

                    'Wait For User To Click
                    MsgBox = wait(0, settingsDetail.GetMessagePort()) 

                    'If User Makes a UI Selection
                    If Type(MsgBox) = "roParagraphScreenEvent"

                        'If User Select OK, Take Action
                        If MsgBox.GetIndex() = 1

                            'Print To Debugger
                            DebugPrint("User Selected Settings Detail Index: " + ValueToString(MsgBox.GetIndex()), True)

                            'Display "Loading..." To User
                            msgPleaseWait = ShowPleaseWait("Loading...", "")

                            'Exit
                            Exit While

                        End If 
                    End If
                End While

                'Leave Settings Screen
                Exit While

            End If

            'Leave Settings Screen
            Exit While

        End If

    End While

End Function

