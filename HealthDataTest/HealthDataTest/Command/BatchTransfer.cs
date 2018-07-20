using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace HealthDataTest.Command
{
    class BatchTransfer : CmdBase
    {

        private static RpcClient Rpcclient = new RpcClient(new Uri("http://127.0.0.1:10001"));

        private static string RootPrivateKey = "0x3ba2dc5aa1f631b4f1465b92557e2434e44d70ae116986a87e9e368fa6efa19a";
        private static string RootPubAddress = "0xd6bd66ad9ab4a451963b30479a5cb3a9217a1b83";

        private static string Abi =
            "[{\"constant\":false,\"inputs\":[{\"name\":\"tos\",\"type\":\"address[]\"}],\"name\":\"Batch\",\"outputs\":[],\"payable\":true,\"stateMutability\":\"payable\",\"type\":\"function\"}]";

        private string _contractAddress= "0x55f46fb2aba65010fa00f4058e786e99917a1747";


        public override void Run(string[] args) {
            var cmdArgs = this.ParseArgs(args);

            var rootAccount = new Account(RootPrivateKey);
            var rootWeb3 = new Web3(rootAccount,Rpcclient);
            
            var contract = rootWeb3.Eth.GetContract(Abi, _contractAddress);
            var function = contract.GetFunction("Batch");


            using (var fr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"acc.txt"))) {
                List<string> adds = new List<string>();
                while (true) {
                    var line = fr.ReadLine();
                    if (string.IsNullOrEmpty(line)) {
                        break;
                    }


                    var addr = line.Split(",")[0];


                    adds.Add(addr);
                    if (adds.Count == 100) {
                        try {
                            var gas = function.EstimateGasAsync(RootPubAddress,
                                new HexBigInteger(10000000),
                                new HexBigInteger(Web3.Convert.ToWei(2, UnitConversion.EthUnit.Ether)),
                                new object[]{adds.ToArray()}).Result;

                            var ti = new TransactionInput() {
                                From = RootPubAddress,
                                Gas = new HexBigInteger(gas.Value),
                                Value = new HexBigInteger(Web3.Convert.ToWei(10000, UnitConversion.EthUnit.Ether)),
                                GasPrice = new HexBigInteger(Web3.Convert.ToWei(2, UnitConversion.EthUnit.Gwei)),
                                To = _contractAddress
                            };
                            
                            var ret = function.SendTransactionAndWaitForReceiptAsync(ti,null,new object[]{adds.ToArray()}).Result;
                        } catch (Exception ex) {

                            Console.WriteLine(ex.Message);
                        }

                        adds.Clear();
                    }

                }
            }

            
        }
    }
}
