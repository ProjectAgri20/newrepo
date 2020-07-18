
$username = $cmdLine[1]
$password = $cmdLine[2]

authenticate($username, $password)

func authenticate($username, $password)
winwaitactive("","Authentication Required", "10");
if winexists("Authentication Required") then
  send($username)
  Send("{TAB}")
  send($password)
Sleep(10)
  Send("{ENTER}")
  Exit(0)
 else
   Exit(1)
 endif
 endfunc