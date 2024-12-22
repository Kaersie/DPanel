@echo off
taskkill /f /t /im DPanel.exe
copy .\*.* ..\
copy .\DPanel.exe ..\
..\DPanel.exe