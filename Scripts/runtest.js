Web3 = require('web3');

solc = require('solc');
fs =  require('fs');

web3 = new Web3(new Web3.providers.HttpProvider("http://127.0.0.1:10001"));

acc0 = "0xd6bd66ad9ab4a451963b30479a5cb3a9217a1b83";
contractAddress = "0x55f46fb2aba65010fa00f4058e786e99917a1747";

code  = fs.readFileSync('./BatchTransfer.sol').toString();
CompiledCode = solc.compile(code);
abi = CompiledCode.contracts[':BatchTransfer'].interface;
contract = new web3.eth.Contract(JSON.parse(abi),contractAddress);

console.log("start");

gas = web3.utils.toWei('5', 'gwei');
value = web3.utils.toWei('10000', 'ether')

contract.methods.Batch(["0xc3C86b8693A7fa778f855fCbD11EEebf1E6BA4A8"]).send({from:acc0,gasPrice:gas,value:value})


