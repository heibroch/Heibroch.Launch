# Heibroch.Launch
This is for the person/team who just can't remember every darn shortcut or is too lazy to navigate to it.
It's sort of like any other shortcut tool (think Launchy), but instead you specify your own shortcuts. It also has a bit extra :)
Upon installing it default has a shortcut called "MyShortcuts.hscut". Here you can add various shortcuts. You can add multiple
files, as long as it has the extension ".hscut".

You can do various actions by adding a line in the shortcut file. All you need to remember is to separate the name of the shortcut
and the shortcut path with a ';'...

Like opening a folder..........

MyAwesomeFolder;"C:\source\MyFolder\"

Or opening a file..........

MyMyAwesomeFolder;"C:\source\MyFolder\SomeVideoFile.mov"

Or launching a website..........

My Favourite site;"www.github.com"

Or running a cmd command..........

Shutdown;[CMD]shutdown /s /f

Or sending an E-mail..........

ComplainToSite;mailto:support@github.com

Or remoting to a server or pc. Note that "[remote]" must be before the server name..........

Open Dat Server;[remote]mydomain.myserver

Or EVEN BETTER! You can share shortcuts by specifying a hscut-file on a network drive in order to share shortcuts! This
avoids pesky navigation when your coworker asks you where those documentation files are. Note that it will scan the folder for 
all the shortcut files available, so you can split them up as you see fit and even set read-permissions based on groups, which
will then allow for users to have same shortcut sets based on the AD-group they're in. Note that you can still have shortcuts locally
in your app data folder (Pssst! It's there the MyShortcuts.hscut-file is) :) ..........

[SearchPath];\\myserver.dk\myshare\HeibrochLaunchShortcutFolder\


NEXT...
- A [SwitchIfExists] optional tag will be possible to add to shortcuts. This means that if it's started a previous process, then it will switch to that existing process rather than starting a new one. This can help if you e.g. use some web-links often.
- On opening the shortcut window, it will display the last 5 things you selected, in an order of what you've used the most over the last week.
- In setting, being able to open the folder location or file for the given shortcuts.
- Management of shortcuts through entirely through settings.
- Exporting/importing settings.
- Option for using settings located in GDrive or OneDrive.
- Encrypting the files to avoid exploitation.
- Adding option for cached file search.
- Context menus for fast adding of shortcuts.
