## Application description
Zune Desktop Software scobbling for Last.fm client.

## Installation Instructions
1) Install the latest Zune Desktop Software.
2) Install the Last.fm client and login to your account.
3) Make sure that you installed the requirements found below.
4) Extract the ZuseMe directory to any directory you want.
5) Launch ZuseMe.exe (Runs hidden in the background and tray menu)
6) Have fun scrobbling with the Zune Desktop Software and Last.fm client.

## Tips and tricks
- Closing the Zune Desktop Software will send a stop scrobble command to the Last.fm client.
- If your last.fm client doesn't receive your currently playing song run ZuseMe as administrator.
- Place a ZuseMe shortcut in your Windows startup folder for easier scrobbling.
- ZuseMe works with all your devices connected (Zune HD, WP7, etc) and also works with a Zunepass.
- Play/Pause only works when you turn on the keyboard media keys button setting.
- Right mouse click on the ZuseMe tray icon to open the settings window.
- ZuseMe scrobbles the artist name tag, not the album artist name tag.

## Requirements
- Last.fm client v2.1.37 - Download: https://github.com/dumbie/ZuseMe/releases
- Zune Desktop Software v4.8.2345 - Download: https://github.com/dumbie/ZuseMe/releases
- .NET Framework 4 - Download: https://www.microsoft.com/en-us/download/details.aspx?id=17718
- Visual C++ 2010 x86 (Fixes dll errors) - Download: https://www.microsoft.com/en-us/download/details.aspx?id=26999

## Support and bug reporting
When you are walking into any problems or a bug you can go to my help page at https://help.arnoldvink.com so I can try to help you out and get everything working.

## Developer donation
If you appreciate this project and want to support me with my projects you can make a donation through https://donation.arnoldvink.com

## Special thanks
- ZuneNowPlaying - ZenWalker
- Zuse - Zach Howe (Zmanfx)
- MusicBrainz.org

## Changelog
v1.81 (28-october-2012)
- Added support for Last.fm Scrobbler v2.1.*

v1.80 (6-august-2012)
- Improved MusicBrainz song duration download speed
- Improved media keys play/pause detection and speed

v1.71 (18-february-2012)
- Added better Last.fm v2.- client support 
- Changed client id to Windows Media Player

v1.70 (29-january-2012)
- Changed WMP/Media center scrobble filter
- Fixed ASCII characters songs not scrobbling
- Improved MusicBrainz song length fetching speed

v1.65 (20-december-2011)
- Fixed MusicBrainz blocking ZuseMe
- Improved MusicBrainz song length detection

v1.64 (13-december-2011)
- Fixed default unknown track length not saving
- Mediakeys setting no longer requires a restart
- Improved overall performance and stability

v1.63 (24-november-2011)
- Added run asInvoker manifest
- Added "Stop Scrobble" button to tray menu
(Stop Scrobble stops the currently playing track)

v1.62 (10-november-2011)
- Fixed "WinZuseMe" showing up in alt+tab
- Fixed Windows XP not detecting current song
- Improved overall performance and stability

v1.60 (23-october-2011)
- Improved overall performance and stability

v1.54 (11-october-2011)
- Fixed too short tracks sometimes returning from MusicBrainz

v1.53 (25-september-2011)
- Improved ZuseMe startup speed
- Improved Zune to Last.fm client song send speed

v1.51 (17-september-2011)
- Now shows the version number in the settings window

v1.50 (17-september-2011)
- Added option to set scrobble location (Default: c:\)
- Changed default scrobble time to 30 seconds (Better results)
- Improved album name scrobbling (Fixes reported bugs, hopefully)
- Added website button that opens the ZuseMe Last.fm group page

v1.42 (06-august-2011)
- Added play/pause mediakey support (Pauses Last.fm client)
- Speed up/improved sending current track to Last.fm client
- Fixed hidden windows shown in alt+tab function

v1.40 (31-july-2011)
- Fixed songs with special characters not scrobbling
- Speed up / improved sending current track to Last.fm client
- Added a window to change settings for easier usage
- Added option to launch Zune with ZuseMe
- Added option to exit Zune/LastFM with ZuseMe
- Added option to enable/disable MusicBrainz (Song length detection)

v1.20 (27-july-2011)
- Last.fm will now automatically start with ZuseMe
- Added a tray icon with Settings/Exit buttons
- Added track length lookup with MusicBrainz
- Improved Zune to Last.fm scrobbling speed

v1.00 (25-july-2011)
- Initial release