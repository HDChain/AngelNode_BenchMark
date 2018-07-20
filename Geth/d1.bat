%~d0
cd %~dp0

%~d0
cd %~dp0

@echo on

set acc=0xd6bd66ad9ab4a451963b30479a5cb3a9217a1b83

geth --datadir d1 ^
--bootnodes enode://d9840cf88e3880ae4ae1162e3d608e03b07e33e8509972104de434a9daa4a95cc8911b203fa4fc988399511552b2caeb3435c6b3b644bfdd49374f2a51b1a85a@127.0.0.1:30301 ^
--txpool.globalslots 1000000 ^
--targetgaslimit 9223372036854775808 ^
--rpc --rpcapi eth,net,web3,personal ^
--gasprice 0 ^
--port 30001 ^
--ipcdisable ^
--rpccorsdomain "*" --rpcaddr 0.0.0.0 --rpcport 10001 --rpcvhosts * ^
--password pass.txt --unlock %acc% --mine --etherbase %acc% ^
console