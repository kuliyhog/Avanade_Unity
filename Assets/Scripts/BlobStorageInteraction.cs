using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Nethereum.Web3;
using Nethereum.Quorum;
using ServerSettings;
using System;

public class BlobStorageInteraction : MonoBehaviour
{

    [SerializeField]
    private Button retrieveFromBlockchainButton;

    [SerializeField]
    private Button downloadButton;

    [SerializeField]
    private TextMeshProUGUI consoleMessage;

    private void Awake()
    {
        downloadButton.onClick.AddListener(StartDownload);
        retrieveFromBlockchainButton.onClick.AddListener(RetrieveFromBlockchain);
    }


    void StartDownload()
    {
        StartCoroutine(Download());
    }

    IEnumerator Download()
    {
        UnityWebRequest www = new UnityWebRequest("https://fileandhashes.blob.core.windows.net/file-3d/b8f437701570ca06000b35f9644c995e6f2886c8ecc8fcd32be0a6493223bb47");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            consoleMessage.text = www.downloadHandler.text;
            Debug.Log(www.downloadHandler.text);
        }
    }

    private async void RetrieveFromBlockchain()
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