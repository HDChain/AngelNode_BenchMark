%~d0
cd %~dp0

%~d0
cd %~dp0

@echo on

set acc=0x53058ee5a9a7c8dc65bf29dec4d7f35c9af33474

geth --datadir d3 ^
--bootnodes enode://d9840cf88e3880ae4ae1162e3d608e03b07e33e8509972104de434a9daa4a95cc8911b203fa4fc988399511552b2caeb3435c6b3b644bfdd49374f2a51b1a85a@127.0.0.1:30301 ^
--rpc --rpcapi eth,net,web3,personal ^
--targetgaslimit 9223372036854775808 ^
--gasprice 0 ^
--port 30003 ^
--ipcdisable ^
--cache 128 ^
--rpccorsdomain "*" --rpcaddr 0.0.0.0 --rpcport 10003 --rpcvhosts * ^
--password pass.txt --unlock %acc% --mine --etherbase %acc% ^
console