REM ********************************************************************
REM ********************************************************************
REM ==
REM ==  ScreenPlay Screening Room
REM ==  Author: J Kardong
REM ==  Copyright: ScreenPlay Labs 2011
REM ==  Created: Sept 2011
REM ==  BrightScript Version: 3.0
REM ==  Description: Initalizes and sets up the Video Screen
REM ==
REM ********************************************************************
REM ********************************************************************

REM ====================================================================
REM == NAME: ShowVideoScreen()
REM == INPUT PARAMETERS:
REM ==      TitleArray: Associative Array of metadata associated with a title
REM ==      VideoArea: Sets if the title is a MOVIE or a TRAILER so it can pick
REM ==      the correct URL to display
REM == OUTPUT: NONE
REM == DESCRIPTION: Takes a Associative Array of values that were originally
REM == generated in the utl_XML.GetMoviesXML() function and then builds
REM == a Associative Array to pass to a Video Screen so that the movie
REM == will be loaded and play.
REM ====================================================================
Function ShowVideoScreen(TitleArray As Object, VideoArea As String, StartPosition As Integer) 

    'Print to Debugger
    DebugPrint("Initalize Video Screen - [ scr_VideoScreen.ShowVideoScreen() ]")

    'Print To Debugger
    DebugPrint("Loading Title URL: " + TitleArray.Lookup("streamURL"), True)

    'Create Video Screen
    video = CreateObject("roVideoScreen")

    'Create Port
    port = CreateObject("roMessagePort")

    'Set The Port
    video.SetMessagePort(port)

    'Associative Array Of Video Values
    aa_Video = CreateObject("roAssociativeArray")

    'Set The Video
    If(VideoArea = "MOVIE") then

        'Set Movie URL
        aa_Video.Stream = {url:TitleArray.Lookup("streamURL")}

    ElseIf (VideoArea = "TRAILER") Then

        'Set Trailer URL
        aa_Video.Stream = {url:TitleArray.Lookup("trailerURL")}

    End If

    'Set Video Attributes
    aa_Video.IsHD = true
    aa_Video.StreamQualities = "HD"
    aa_Video.HDBranded = true
    aa_Video.StreamFormat = "mp4"
    aa_video.bitrate = 2500
    aa_Video.Title = TitleArray.Lookup("Title")
    aa_Video.PlayStart = StartPosition

    'Set Content To The Video Player
    video.SetContent(aa_Video)

    'Set the event timer - triggers isPlaybackPosition - set to 10 seconds
    video.SetPositionNotificationPeriod(10)

    'Load The Video 
    video.Show()

    'Set Listener
    while true

        'Set Msg From Video
        msg = Wait(0, video.GetMessagePort()) 

        'Determine User Action
        If Type(msg) = "roVideoScreenEvent"

            'User Closed Screen
            if msg.isScreenClosed()
                
                'Print to debugger
                DebugPrint("Screen Closed: " + msg.getMessage() + " | Stop Position = " + ValueToString(msg.GetIndex()),true)

                'Exit Video
                exit while

            'Video Failed To Load
            elseif msg.isRequestFailed()

                'Print To Debugger
                DebugPrint("Video request failure: " + ValueToString(msg.GetIndex()) + " " + msg.GetData(), true)
 
            'Button Press
            elseif msg.isButtonPressed()

                'Print to Debugger
                DebugPrint("Remote Button Click",true)
                DebugPrint("Index: " + ValueToString(msg.GetIndex()),true)
                DebugPrint("Get Data: " + ValueToString(msg.GetData()),true)

            'Playback Event For Determining Where User Was during play
            elseif msg.isPlaybackPosition()

                'Only Save Playback position for Movies
                If(VideoArea = "MOVIE") then

                    'Save to Registry
                    WriteToRegistry(ValueToString(TitleArray.Lookup("screenPlayID")),ValueToString(msg.GetIndex()))

                    'Save PlayBack Position To C2MX
                    'GetOrSetPlayBackPosition(ValueToString(TitleArray.Lookup("screenPlayID")),false,ValueToString(msg.GetIndex()))

                End If

            'Unhandled Event (expected)
            else
                DebugPrint("Expected Unhandled Event",true)
            end if

        Else
            DebugPrint("Non roVideoScreenEvent Event",true)
        End if

    end while

End Function