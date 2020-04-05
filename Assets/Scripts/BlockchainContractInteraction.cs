using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Nethereum.Web3;
using ServerSettings;
using System.Threading.Tasks;
using Nethereum.Web3.Accounts;
using Nethereum.HdWallet;
using Nethereum.Web3.Accounts.Managed;

namespace BCI {
    public class BlockchainContractInteraction : MonoBehaviour
    {

        [SerializeField]
        private Button runScriptButton;

        [SerializeField]
        private Button retrieveAccBalButton;

        [SerializeField]
        private Button retrieveHashButton;
        
        [SerializeField]
        private Button transferEthButton;

        [SerializeField]
        private TextMeshProUGUI consoleMessage;

        private void Awake()
        {
            runScriptButton.onClick.AddListener(RopstenContractInteraction);
            retrieveAccBalButton.onClick.AddListener(GetAccountBalance);
            retrieveHashButton.onClick.AddListener(QuorumContractInteraction);
            transferEthButton.onClick.AddListener(TransferEther);
        }

        // setup a wallet and get an account for the transactions
        static public async Task<Account> GetAccount()
        {

            RopstenSettings rp = new RopstenSettings();
            WalletSettings ws = new WalletSettings();
            MetamaskSettings ms = new MetamaskSettings();

            var words = ws.Words;
            var password = ws.Password;
            var wallet = new Wallet(words, password);

            var account = new Wallet(words, password).GetAccount(0);

            return account;
        }

        private async void GetAccountBalance()
        {
            RopstenSettings rs = new RopstenSettings();

            var account = await GetAccount();

            try
            {
            /* Get Account balance*/
                var web3 = new Web3(rs.Url);
                var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
                var etherAmount = Web3.Convert.FromWei(balance.Value);
                var print = $"Account Balance of {account.Address} on Ropsten Test Network\nBalance in Wei: {balance.Value}\nBalance in Ether: {etherAmount}";
                consoleMessage.text = print;
            }
            catch (System.Exception e)
            {
                consoleMessage.text = "Error: Check Debug Log\nPossibly no internet access!";
                Debug.Log(e);
            }


        }


        // transfer 0.01ether from sender to receiver
        private async void TransferEther()
        {
            MetamaskSettings ms = new MetamaskSettings();
            RopstenSettings rp = new RopstenSettings();
            try
            {
                var account = await GetAccount();
                var web3 = new Web3(account, rp.Url);
                var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
                var balance2 = await web3.Eth.GetBalance.SendRequestAsync(ms.PublicKey);
                var print = "The account balance of Sender " + account.Address + "is: " + balance.Value + "\nThe account balance of Receiver " + ms.PublicKey + " is: " + balance2.Value;
                consoleMessage.text = print;

                var toAddress = ms.PublicKey;
                var transactionReceipt = await web3.Eth.GetEtherTransferService()
                    .TransferEtherAndWaitForReceiptAsync(toAddress, 0.01m, 2);
                consoleMessage.text = print;

                print = $"Transaction {transactionReceipt.TransactionHash} for amount of 0.01 Ether completed";
                consoleMessage.text = print;
                balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
                balance2 = await web3.Eth.GetBalance.SendRequestAsync(ms.PublicKey);

                await Task.Delay(2000);

                print = "New account balance of Sender " + account.Address + "is: " + balance.Value + "\nNew account balance of Receiver " + ms.PublicKey + " is: " + balance2.Value;
                consoleMessage.text = print;
            }
            catch (System.Exception e)
            {
                consoleMessage.text = "Error: Check Debug Log\nPossibly no internet access!";
                Debug.Log(e);
            }
            
        }


        private async void RopstenContractInteraction()
        {
            RopstenSettings rp = new RopstenSettings();

            try
            {
                var account = await GetAccount();
                var web3 = new Web3(account, rp.Url);
                var abi = rp.ContractAbi;
                var contract = web3.Eth.GetContract(abi, rp.ContractAddress);

                var functionSet = contract.GetFunction("getHash");
                var result = await functionSet.CallAsync<string>(22);
                var print = "getHash(22) contract function returns: " + result;
                consoleMessage.text = print;
            }
            catch (System.Exception e)
            {
                consoleMessage.text = "Error: Check Debug Log\nPossibly no internet access!";
                Debug.Log(e);
            }
            
        }


        
        private async void QuorumContractInteraction()
        {
            QuorumSettings qs = new QuorumSettings();
            WalletSettings ws = new WalletSettings();

            var account = await GetAccount();
            var managedAccount = new ManagedAccount(account.Address, ws.Password);
            var web3Managed = new Web3(managedAccount, qs.UrlWithAccessKey);

            try
            {
                var blockNumber = await web3Managed.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                var print = "Current BlockNumber: " + blockNumber.Value;                

                var balance = await web3Managed.Eth.GetBalance.SendRequestAsync(account.Address);
                print += "\n" + "Account Balance of " + account.Address + " on Quorum: " + Web3.Convert.FromWei(balance.Value);

                var contract = web3Managed.Eth.GetContract(qs.ContractAbi, qs.ContractAddress);

                var functionSet = contract.GetFunction("getLatestFileIndex");
                var result = await functionSet.CallAsync<int>();

                print += $"\ngetLatestFileIndex(): " + result;

                consoleMessage.text = print;

            }
            catch (Exception e)
            {
                consoleMessage.text = "Error: Check Debug Log\nPossibly no internet access!";
                Debug.Log(e);
            }
        }
    }
}
