using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Nethereum.Web3;
using ServerSettings;
using BCI;
using System.Threading.Tasks;
using Nethereum.Web3.Accounts.Managed;

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
        downloadButton.onClick.AddListener(Download);
        retrieveFromBlockchainButton.onClick.AddListener(DisplayData);
    }


    // Function to getHash from blockchain and display the results
    private async void DisplayData()
    {
        var result = await RetrieveFromBlockchain();
        if (result != null)
        {
            consoleMessage.text = "GetHash/Download Link: " + result;
        }
        else {
            consoleMessage.text = "Error: Check Debug Log\nPossibly no internet access!";
        }
    }

    // Function to download file and display contents
     private async void Download()
    {
        string downloadLink = await RetrieveFromBlockchain();
        UnityWebRequest www = new UnityWebRequest(downloadLink);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SendWebRequest();
        while (!www.isDone)
            await Task.Delay(100);


        if (www.isNetworkError || www.isHttpError)
        {
            consoleMessage.text = "Error: Check Debug Log\nPossibly no internet access!";
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            consoleMessage.text = "File Contents: " + www.downloadHandler.text;
            Debug.Log(www.downloadHandler.text);
        }
    }

    // Function to retreive data form the blockchain
    private async Task<string> RetrieveFromBlockchain()
    {
        QuorumSettings qs = new QuorumSettings();
        WalletSettings ws = new WalletSettings();
        try
        {
            var account = await BlockchainContractInteraction.GetAccount();
            var managedAccount = new ManagedAccount(account.Address, ws.Password);
            var web3Managed = new Web3(managedAccount, qs.UrlWithAccessKey);
            var web3 = new Web3(account, qs.UrlWithAccessKey);
            var contract = web3.Eth.GetContract(qs.ContractAbi, qs.ContractAddress);

            var functionSet = contract.GetFunction("getHash");
            var result = await functionSet.CallAsync<string>(2);
            return result;
        }
        catch (System.Exception e) {
            consoleMessage.text = "Error: Check Debug Log\nPossibly no internet access!";
            Debug.Log(e);
            return null;
        }

    }

}