%~d0
cd %~dp0

%~d0
cd %~dp0

@echo on

set acc=0xb033f9c9112b12aa5475589524c0d5813f160405

geth --datadir d8 ^
--bootnodes enode://d9840cf88e3880ae4ae1162e3d608e03b07e33e8509972104de434a9daa4a95cc8911b203fa4fc988399511552b2caeb3435c6b3b644bfdd49374f2a51b1a85a@127.0.0.1:30301 ^
--rpc --rpcapi eth,net,web3,personal ^
--port 30008 ^
--ipcdisable ^
--cache 128 ^
--rpccorsdomain "*" --rpcaddr 0.0.0.0 --rpcport 10008 --rpcvhosts * ^
console