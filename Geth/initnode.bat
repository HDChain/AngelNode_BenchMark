%~d0
cd %~dp0

%~d0
cd %~dp0

@echo on

set num=0
set maxnum=10

:loop
set /a num+=1

geth --datadir dataother\d%num% init .puppeth\genesis.json

if %num% == %maxnum% goto end
goto loop
:end
