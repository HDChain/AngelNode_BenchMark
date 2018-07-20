pragma solidity ^0.4.22;

contract BatchTransfer{
    
    function Batch (address[] tos) public payable {
        
        require(msg.value > 0);
        uint256 len = tos.length;
        uint256 moneyForEach = msg.value / len;
        
        for(uint256 i= 0 ;i< len; i++){
            tos[i].transfer(moneyForEach);
        }
    }
    
}
