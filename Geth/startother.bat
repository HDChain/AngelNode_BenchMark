%~d0
cd %~dp0

%~d0
cd %~dp0

@echo on

set num=0
set maxnum=10

:loop
set /a num+=1

set /a port = 20000 + %num%
set /a rpcPort = 40000 + %num%

start geth --datadir dataother\d%num% ^
--bootnodes enode://d9840cf88e3880ae4ae1162e3d608e03b07e33e8509972104de434a9daa4a95cc8911b203fa4fc988399511552b2caeb3435c6b3b644bfdd49374f2a51b1a85a@127.0.0.1:30301 ^
--rpc --rpcapi eth,net,web3,personal ^
--port %port% ^
--ipcdisable ^
--cache 128 ^
--rpccorsdomain "*" --rpcaddr 0.0.0.0 --rpcport %rpcPort% --rpcvhosts * ^
console


if %num% == %maxnum% goto end
goto loop
:end
