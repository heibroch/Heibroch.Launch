Dependencies:
- Heibroch


# Heibroch.Launch
This is for the person/team who just can't remember every darn shortcut or is too lazy to navigate to it.
It's sort of like any other shortcut tool (think Launchy), but instead you specify your own shortcuts. It also has a bit extra :)
Upon installing it default has a shortcut called "MyShortcuts.hscut". Here you can add various shortcuts. You can add multiple
files, as long as it has the extension ".hscut".

You can do various actions by adding a line in the shortcut file. All you need to remember is to separate the name of the shortcut
and the shortcut path with a ';'...

------------------------------------------------------------------------------

# Shotcut options

#### OPENING A FOLDER
MyAwesomeFolder;"C:\source\MyFolder\"

#### OPENING A FILE
MyMyAwesomeFolder;"C:\source\MyFolder\SomeVideoFile.mov"

#### OPENING A LINK 
My Favourite site;"www.github.com"

#### RUNNING A CMD COMMAND  
Shutdown;[CMD]shutdown /s /f

#### SENDING AN EMAIL
ComplainToSite;mailto:support@github.com

#### REMOTING TO A MACHINE (NOTE TAG "[Remote]")
> Open Dat Server;[Remote]mydomain.myserver

#### COPYING PREDEFINED TEXT STRINGS (NOTE TAG "[CopyText]"
My database connection;[CopyText]db.dev.github.com;1433

#### DISTRIBUTED SHORTCUTS (NOTE TAG "[SearchPath]")
You can also share shortcuts by specifying a hscut-file on a network drive in order to share shortcuts! This
avoids pesky navigation when your coworker asks you where those documentation files are. Note that it will scan the folder for 
all the shortcut files available, so you can split them up as you see fit and even set read-permissions based on groups, which
will then allow for users to have same shortcut sets based on the AD-group they're in. Note that you can still have shortcuts locally
in your app data folder (Pssst! It's there the MyShortcuts.hscut-file is) :) ..........

[SearchPath];\\myserver.dk\myshare\HeibrochLaunchShortcutFolder\

An important thing to know is that you can add arguments to your shortcuts. It has the following syntax:
MyMyAwesomeFolder;"[<shortcut>][Arg=<argumentName>][</shortcut>]"
  
If you want an example:

Send mail to coworker;[Arg=Coworker Initials]@gmail.com


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
