# Heibroch.Launch
This is for the person/team who just can't remember every darn shortcut or is too lazy to navigate to it.
It's sort of like any other shortcut tool (think Launchy), but instead you specify your own shortcuts. It also has a bit extra :)
Upon installing it default has a shortcut called "MyShortcuts.hscut". Here you can add various shortcuts. You can add multiple
files, as long as it has the extension ".hscut".

You can do various actions by adding a line in the shortcut file. All you need to remember is to separate the name of the shortcut
and the shortcut path with a ';'...

Like opening a folder...\r\n
MyTotallyNotPornFolder;"C:\source\Porn\"

Or opening a file...

MyTotallyNotPornFolder;"C:\source\Porn\SomePornVid.mov"

Or launching a website...
My Fav NOT porn site;"www.youporn.com"

Or running a cmd command...
Shutdown;[CMD]shutdown /s /f

Or sending an E-mail...
ComplainToPornSite;mailto:support@youporn.com

Or remoting to a server or pc. Note that "[remote]" must be before the server name...
Open Dat Porn Server;[remote]mydomain.pornserver

Or EVEN BETTER! You can share shortcuts by specifying a hscut-file on a network drive in order to share shortcuts! This
avoids pesky navigation when your coworker asks you where those documentation files are. Note that it will scan the folder for 
all the shortcut files available, so you can split them up as you see fit and even set read-permissions based on groups, which
will then allow for users to have same shortcut sets based on the AD-group they're in. Note that you can still have shortcuts locally
in your app data folder (Pssst! It's there the MyShortcuts.hscut-file is) :) ...
[Shared];\\MyNetworkServer\MyShare\
