# What
A Windows application that allows a user to switch between the windows of the app they're using with a keyboard shortcut.

# Why
Provide equivalent functionality to the MacOS feature that allows current-app-window-switching through the `` ⌘-` `` (Command–Grave accent) keyboard shortcut.

# How
With a low-level Windows keyboard hook inserted into the event procedure chain.

# Caveats
There is no way to save the shortcut right now, so you have to define it every time you launch the app.

Your security/IT policy might stop this app from running; Hooking into a stream of keyboard events looks like a keylogger.

The shortcut chosen might conflict with another application or Windows itself. In this case how the other process set up the shortcut, and when the process loaded determines who wins.