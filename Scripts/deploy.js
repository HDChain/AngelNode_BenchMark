Web3 = require('web3');


var arguments = process.argv.splice(2);

rpc = "http://localhost:10001"

if (arguments.length > 0){
	rpc = arguments[0];
}

process.on('uncaughtException', function(err) {
    console.error('Error caught in uncaughtException event:', err);
});

defaultWeb3 = new Web3(Web3.givenProvider || rpc);
var newacc = defaultWeb3.eth.accounts.create();

const PrivateKeyProvider = require("truffle-privatekey-provider");

acc = newacc.address;
privateKey = Buffer.from( newacc.privateKey.substr(2), 'hex')

const provider =  new PrivateKeyProvider(privateKey, rpc);
web3 = new Web3(provider);

var useriddataContract = new web3.eth.Contract([{"constant":true,"inputs":[{"name":"","type":"uint256"}],"name":"UserData","outputs":[{"name":"","type":"string"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"id","type":"uint256"},{"name":"data","type":"string"}],"name":"AddData","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[],"name":"ContractOwner","outputs":[{"name":"","type":"address"}],"payable":false,"stateMutability":"view","type":"function"},{"inputs":[],"payable":false,"stateMutability":"nonpayable","type":"constructor"}],
	null,{
		
     from: acc, 
     data: '0x608060405234801561001057600080fd5b5060008054600160a060020a03191633179055610327806100326000396000f3006080604052600436106100565763ffffffff7c0100000000000000000000000000000000000000000000000000000000600035041663092956a8811461005b5780635791fa74146100e85780635a63fbc914610148575b600080fd5b34801561006757600080fd5b50610073600435610186565b6040805160208082528351818301528351919283929083019185019080838360005b838110156100ad578181015183820152602001610095565b50505050905090810190601f1680156100da5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b3480156100f457600080fd5b5060408051602060046024803582810135601f81018590048502860185019096528585526101469583359536956044949193909101919081908401838280828437509497506102209650505050505050565b005b34801561015457600080fd5b5061015d610244565b6040805173ffffffffffffffffffffffffffffffffffffffff9092168252519081900360200190f35b60016020818152600092835260409283902080548451600294821615610100026000190190911693909304601f81018390048302840183019094528383529192908301828280156102185780601f106101ed57610100808354040283529160200191610218565b820191906000526020600020905b8154815290600101906020018083116101fb57829003601f168201915b505050505081565b6000828152600160209081526040909120825161023f92840190610260565b505050565b60005473ffffffffffffffffffffffffffffffffffffffff1681565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106102a157805160ff19168380011785556102ce565b828001600101855582156102ce579182015b828111156102ce5782518255916020019190600101906102b3565b506102da9291506102de565b5090565b6102f891905b808211156102da57600081556001016102e4565b905600a165627a7a72305820277130688b4f53daf658c89193cea19b5c891882055b514c9aff17185509c9010029', 
     gas: '4700000',
     gasPrice:'0'
	});

useriddataContract.deploy()
.send({
    
}, function(error, transactionHash){ 
	console.log("hx1 : " + transactionHash);
 })
.on('error', function(error){ 
	console.log("error : "+error);
 })
.on('receipt', function(receipt){
   console.log("receipt : " + receipt.contractAddress) 
})
.then(function(newContractInstance){
    console.log("new : " + newContractInstance.options.address) 
	
	setTimeout(doAddData,5000,newContractInstance);
});	

function doAddData(newContractInstance){
	
	var runCount = 0;
	var runTotleCount = 100;
	
	for(var i = 0 ; i< runTotleCount;i++){
		newContractInstance.methods.AddData(i + 1,"string" + i).send({
		from : acc,
		gasPrice:'0'
		}).on('receipt', function(receipt){
			console.log("hx " + receipt.transactionHash + " status : " + receipt.status)
			runCount+=1;
			
			if(runCount == runTotleCount){
				process.exit(1);
			}
		}).on('error', function(error){ 
			console.log("error : "+error);
			runCount +=1;
			
			if(runCount == runTotleCount){
				process.exit(1);
			}
		});
	}
	
}
	
	
	
	