TIMEOUT 2
set NowPath=%cd%
taskkill /f /PID 70440
TIMEOUT 1
XCOPY /E /Y "%NowPath%\NewVersion" "%NowPath%"
RD /S/Q "%NowPath%\NewVersion\"
start chrome.exe