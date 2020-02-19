using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.Blocks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class RetrieveBalance : MonoBehaviour
{
    [SerializeField]
    private Text accBal;

    [SerializeField]
    private Button retrieveAccBal;
    
    [SerializeField]
    private Button getBlockNumber;

    [Header("Ethereum Settings")]

    [Tooltip("The Ethereum network we will make calls to")]
    public string networkUrl = "https://ropsten.infura.io/v3/d06b5b115e3d4d128749be04cd9f6520";
    public string networkUrl2 = "https://ropsten.infura.io/";
    public string playerEthereumAccount = "0x2FfABa69a7eed36d4cB862465e8e7CC11Ed92bC3";

    [Tooltip("Remember don't ever reveal your LIVE network private key, it is not safe to store that in game code. This is a test account so it is ok.")]
    public string playerEthereumAccountPK = "8781c920ad1e9b0197a473f24af246179997ad4be45d85977d241d8ae09af4c8";


    private IEnumerator currentCoroutine;


    void Awake(){
        // add try catch phrase for null pointer exception -- not urgent
        retrieveAccBal.onClick.AddListener(GetAccountBalance);
        getBlockNumber.onClick.AddListener(GetBlockNumber);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCoroutine = GetAccountBalanceCoroutine();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetAccountBalance() 
    {
        StopCoroutine(currentCoroutine);
        currentCoroutine = GetAccountBalanceCoroutine();
        StartCoroutine(currentCoroutine);
    }

    public void GetBlockNumber() 
    {
        StopCoroutine(currentCoroutine);
        currentCoroutine = GetBlockNumberCoroutine();
        StartCoroutine(currentCoroutine);
    }

    public IEnumerator GetAccountBalanceCoroutine()
    {
        var getBalanceRequest = new EthGetBalanceUnityRequest(networkUrl);
        // Send balance request with player's account, asking for balance in latest block
        yield return getBalanceRequest.SendRequest(playerEthereumAccount, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (getBalanceRequest.Exception == null)
        {
            var balance = getBalanceRequest.Result.Value;
            // Convert the balance from wei to ether and round to 8 decimals for display
            accBal.text = $"Account Balance: {Nethereum.Util.UnitConversion.Convert.FromWei(balance, 18).ToString("n8")}";
            Debug.Log(accBal.text);
        }
        else
        {
            Debug.Log("Get Account Balance gave an exception: " + getBalanceRequest.Exception.Message);
        }
    }

    public IEnumerator GetBlockNumberCoroutine()
    {
        var wait = 1;
        while (true)
        {
            yield return new WaitForSeconds(wait);
            wait = 10;
            var blockNumberRequest = new EthBlockNumberUnityRequest("https://ropsten.infura.io/v3/d06b5b115e3d4d128749be04cd9f6520");
            yield return blockNumberRequest.SendRequest();
            if (blockNumberRequest.Exception == null)
            {
                var blockNumber = blockNumberRequest.Result.Value;

                Debug.Log("Block: " + blockNumber.ToString());
            }
        }
    }
}

