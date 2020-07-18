authenticate()

func authenticate()
While Sleep(2000)
    ;If Save Dialog Pops Up
    If WinExists("[CLASS:MozillaDialogClass;]", "") Then
        $WinTitle = WinGetTitle("[CLASS:MozillaDialogClass;]", "")
         WinActivate($WinTitle)
	 Send("{ENTER}")
         Exit(0)
    else
     Exit(1)
    endif
WEnd
 endfunc