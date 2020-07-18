$path= $cmdLine[1]

upload($path)

func upload($path)

WinWaitActive("File Upload")
Send($path)
Send("{ENTER}")