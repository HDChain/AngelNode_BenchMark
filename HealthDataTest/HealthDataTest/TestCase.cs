using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace HealthDataTest
{
    class TestCase
    {
        private Account _account;
        private static RpcClient Rpcclient;
        private static Account DefaultAccount;
        private static Web3 DefaultWeb3;
        private Web3 _web3;

        private static string Abi = "[{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"UserData\",\"outputs\":[{\"name\":\"\",\"type\":\"string\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"id\",\"type\":\"uint256\"},{\"name\":\"data\",\"type\":\"string\"}],\"name\":\"AddData\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"ContractOwner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"DataLength\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"}]";
        private static string code = "0x608060405234801561001057600080fd5b5060008054600160a060020a03191633178155600255610368806100356000396000f3006080604052600436106100615763ffffffff7c0100000000000000000000000000000000000000000000000000000000600035041663092956a881146100665780635791fa74146100f35780635a63fbc9146101535780639fdfa22714610191575b600080fd5b34801561007257600080fd5b5061007e6004356101b8565b6040805160208082528351818301528351919283929083019185019080838360005b838110156100b85781810151838201526020016100a0565b50505050905090810190601f1680156100e55780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b3480156100ff57600080fd5b5060408051602060046024803582810135601f81018590048502860185019096528585526101519583359536956044949193909101919081908401838280828437509497506102529650505050505050565b005b34801561015f57600080fd5b5061016861027f565b6040805173ffffffffffffffffffffffffffffffffffffffff9092168252519081900360200190f35b34801561019d57600080fd5b506101a661029b565b60408051918252519081900360200190f35b60016020818152600092835260409283902080548451600294821615610100026000190190911693909304601f810183900483028401830190945283835291929083018282801561024a5780601f1061021f5761010080835404028352916020019161024a565b820191906000526020600020905b81548152906001019060200180831161022d57829003601f168201915b505050505081565b60008281526001602090815260409091208251610271928401906102a1565b505060028054600101905550565b60005473ffffffffffffffffffffffffffffffffffffffff1681565b60025481565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106102e257805160ff191683800117855561030f565b8280016001018555821561030f579182015b8281111561030f5782518255916020019190600101906102f4565b5061031b92915061031f565b5090565b61033991905b8082111561031b5760008155600101610325565b905600a165627a7a7230582035a51c2ff34b303ce89c2b34a7d1110aecce3404f2ddb094969a5ca6259d60210029";


        public static void Init(string port) {
            Rpcclient = new RpcClient(new Uri($"http://192.168.1.235:{port}"));
            var ecKey = EthECKey.GenerateKey();
            var rootAccount = new Account("0x00d16f50df6aae6f6647eac7e4b58ed40995a95609e689f80ed1e58779dc3e9770");
            var rootWeb3 = new Web3(rootAccount,Rpcclient);

            DefaultAccount = new Account(ecKey);
            DefaultWeb3=new Web3(DefaultAccount,Rpcclient);

            Console.WriteLine($"DefaultAccount {DefaultAccount.Address} " );



            try {
                var ti = new TransactionInput() {
                    From = rootAccount.Address,
                    GasPrice = new HexBigInteger(200000000000),
                    To = DefaultAccount.Address,
                    Value = new HexBigInteger(Web3.Convert.ToWei(100, UnitConversion.EthUnit.Tether))
                };
                var ret = rootWeb3.TransactionManager.SendTransactionAsync(ti).Result;

                for (int i = 0; i < 10; i++) {
                    try {
                        var rec = rootWeb3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(ret).Result;

                        if (rec?.Status?.Value == 1) {
                            break;
                        }
                    } catch (Exception ex) {

                    }

                    Thread.Sleep(1000);
                }
            } catch (Exception ex) {
                Console.WriteLine($" tranfer root {ex.Message}");
                return;
            }

        }


        public void Run() {
            var ecKey = EthECKey.GenerateKey();
            _account = new Account(ecKey);

            Console.WriteLine($"run account : {_account.Address}");

            var ret = string.Empty;

            
            try {
                lock (DefaultAccount) {
                    _web3 = new Web3(_account, Rpcclient);
                    var ti = new TransactionInput() {
                        From = DefaultAccount.Address,
                        GasPrice = new HexBigInteger(2000000000000),
                        To = _account.Address,
                        Value = new HexBigInteger(Web3.Convert.ToWei(100))
                    };
                    ret = DefaultWeb3.TransactionManager.SendTransactionAsync(ti).Result;
                }

                for (int i = 0; i < 100; i++) {
                    try {
                        var rec = _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(ret).Result;

                        if (rec?.Status?.Value == 1) {
                            break;
                        }
                    } catch (Exception ex) {

                    }

                    Thread.Sleep(1000);
                }
            } catch (Exception ex) {
                Console.WriteLine($"{_account.Address} tranfer {ex.Message}");
            }
            
            try {
                Console.WriteLine($"tranfer finish account : {_account.Address}");

                var txn1 = _web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(Abi, code, _account.Address, 
                    new HexBigInteger(1000000) ,new HexBigInteger(200000000000),new HexBigInteger(0)).Result;

                RunTest(txn1.ContractAddress);
            } catch (Exception ex) {
                Console.WriteLine($"{_account.Address} deploy {ex.Message}");
            }
        }

        private void RunTest(string contractAddr) {
            var abi =
                "[{\"constant\":false,\"inputs\":[{\"name\":\"id\",\"type\":\"uint256\"},{\"name\":\"data\",\"type\":\"string\"}],\"name\":\"AddData\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"constant\":true,\"inputs\":[],\"name\":\"ContractOwner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"UserData\",\"outputs\":[{\"name\":\"\",\"type\":\"string\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"}]";

            var contract = _web3.Eth.GetContract(abi, contractAddr);
            var function = contract.GetFunction("AddData");

            for (var x = 1; x <= 100; x++) {
                var newid = Guid.NewGuid().ToString("N");
                try {
                    var ret = function.SendTransactionAsync(_account.Address,
                        new HexBigInteger(100000),
                        new HexBigInteger(2000000000),
                        new HexBigInteger(0),
                        x,
                        newid).Result;

                    //Console.WriteLine($"{x} {newid} {ret}");
                } catch (Exception ex) {
                    Console.WriteLine($"{_account.Address} add {ex.Message}");
                }
            }
        }

    }
}
