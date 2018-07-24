pragma solidity ^0.4.22;

contract UserIdData {
    address public ContractOwner;
    
    mapping(uint256 => string) public UserData;


    constructor () public {
        ContractOwner = msg.sender;
    }

    modifier OnlyContractOwner() {
        require(msg.sender == ContractOwner);
        _;
    }

    function AddData(uint256 id,string data) public {
        UserData[id] = data;
    }

}