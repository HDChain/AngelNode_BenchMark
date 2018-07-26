%~d0
cd %~dp0

%~d0
cd %~dp0

@echo on

set acc=0xbd9abc5c0201b87fcc9067d505fe177875b99aff

geth --datadir d2 ^
--bootnodes enode://d9840cf88e3880ae4ae1162e3d608e03b07e33e8509972104de434a9daa4a95cc8911b203fa4fc988399511552b2caeb3435c6b3b644bfdd49374f2a51b1a85a@127.0.0.1:30301 ^
--txpool.globalslots 1000000 ^
--gcmode archive --syncmode=full ^
--rpc --rpcapi eth,net,web3,personal ^
--targetgaslimit 9223372036854775808 ^
--gasprice 0 ^
--port 30002 ^
--ipcdisable ^
--cache 128 ^
--rpccorsdomain "*" --rpcaddr 0.0.0.0 --rpcport 10002 --rpcvhosts * ^
--password pass.txt --unlock %acc% --mine --etherbase %acc% ^
console