%~d0
cd %~dp0

%~d0
cd %~dp0

@echo on

geth --datadir d1 init .puppeth\genesis.json
geth --datadir d2 init .puppeth\genesis.json
geth --datadir d3 init .puppeth\genesis.json
geth --datadir d4 init .puppeth\genesis.json
geth --datadir d5 init .puppeth\genesis.json

copy keystore\* d1\keystore
copy keystore\* d2\keystore
copy keystore\* d3\keystore
copy keystore\* d4\keystore
copy keystore\* d5\keystore