using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Nethereum.Web3;
using Nethereum.Quorum;
using ServerSettings;

public class BlockchainContractInteraction : MonoBehaviour
{

    [SerializeField]
    private Button runScriptButton;

    [SerializeField]
    private Button retrieveAccBalButton;

    [SerializeField]
    private Button retrieveHashButton;

    [SerializeField]
    private TextMeshProUGUI consoleMessage;

    private void Awake()
    {
        runScriptButton.onClick.AddListener(RopstenContractInteraction);
        retrieveAccBalButton.onClick.AddListener(GetAccountBalance);
        retrieveHashButton.onClick.AddListener(QuorumContractInteraction);
    }

    private async void GetAccountBalance()
    {
        RopstenSettings rs = new RopstenSettings();
        MetamaskSettings ms = new MetamaskSettings();
        /* Get Account balance*/
        var privateKey = ms.PrivateKey;
        var publicKey = ms.PublicKey;

        try
        {
            var web3 = new Web3(rs.Url);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(publicKey);
            var etherAmount = Web3.Convert.FromWei(balance.Value);
            var print = $"Account Balance of {ms.PublicKey} on Ropsten Test Network\nBalance in Wei: {balance.Value}\nBalance in Ether: {etherAmount}";
            consoleMessage.text = print;
            Debug.Log($"Account Balance of {ms.PublicKey} on Ropsten Test Network");
            Debug.Log($"Balance in Wei: {balance.Value}");
            Debug.Log($"Balance in Ether: {etherAmount}");
        }
        catch (System.Exception e)
        {
            consoleMessage.text = "Error: Check Debug Log";
            Debug.Log(e);
        }
        
        
    }


    private async void RopstenContractInteraction()
    {
        RopstenSettings rs = new RopstenSettings();

        /* Ropsten Test Network Contract */
        var abi = rs.ContractAbi;
        var contractAddress = rs.ContractAddress;
        var web3 = new Web3(rs.Url);
        try
        {
            /* Getting the contract from the contract address */
            var contract = web3.Eth.GetContract(abi, contractAddress);
            var functionset = contract.GetFunction("multiply");
            var result = await functionset.CallAsync<int>(5);
            var print = $"Multiplication by 7 from Contract on Ropsten Test Network\nResult: {result}\nResult should be 35, hence {Equals(result, 35)}";
            consoleMessage.text = print;
            Debug.Log(print);

        }
        catch (Exception err)
        {
            consoleMessage.text = "Error: Check Debug Log";
            Debug.Log(err);
        }
    }

    private async void QuorumContractInteraction()
    {
        QuorumSettings qs = new QuorumSettings();
        /* Azure Quorum Contract */
        //var client = new Nethereum.JsonRpc.Client.RpcClient(new Uri(qs.Url));

        var abi = qs.ContractAbi;
        var contractAddress = qs.ContractAddress;
        /* Setting it up so that all the transactions will be private */
        var urlNode1 = qs.Url;
        var web3Node1 = new Web3Quorum(urlNode1);
        var privateFor = new List<string>(new[] { qs.PublicKey });
        web3Node1.SetPrivateRequestParameters(privateFor);

        try
        {
            /* Getting the contract from the contract address */
            var contract = web3Node1.Eth.GetContract(abi, contractAddress); 
            var functionset = contract.GetFunction("multiply");
            var result = await functionset.CallAsync<int>(5);
            var print = $"Multiplication by 7 from Contract on Azure Blockchain Service Quorum\nResult: {result}\nResult should be 35, hence {Equals(result, 35)}";
            consoleMessage.text = print;
            Debug.Log(print);

        }
        catch (Exception err)
        {
            consoleMessage.text = "Error: Check Debug Log";
            Debug.Log(err);
        }
	}
}
