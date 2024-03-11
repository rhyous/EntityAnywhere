@echo off
setlocal disableDelayedExpansion
set root="%~dp0\Controllers\WebApi.Main"
set "prevTest=none"
set "prevFile=none"
for /f "tokens=1-3 delims=:" %%A in (
  '"(for /r "%root%" %%F in (*) do @echo %%~znxF:%%~fF:)|sort"'
) do (
  set "currTest=%%A"
  set "currFile=%%B:%%C"
  setlocal enableDelayedExpansion
  if !currTest! equ !prevTest! echo "!currFile!"
  endlocal
  set "prevTest=%%A"
)