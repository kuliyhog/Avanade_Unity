using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.Blocks;
using Nethereum.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;


public class RetrieveBalance : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI consoleMessage;

    [SerializeField]
    private Button retrieveAccBal;
    
    [SerializeField]
    private Button retrieveHash;

    [Header("Ethereum Settings")]

    [Tooltip("The Ethereum network we will make calls to")]
    public string networkUrl = "https://ropsten.infura.io/v3/d06b5b115e3d4d128749be04cd9f6520";
    public string ethAccKey = "0x2FfABa69a7eed36d4cB862465e8e7CC11Ed92bC3";

    [Tooltip("Remember don't ever reveal your LIVE network private key, it is not safe to store that in game code. This is a test account so it is ok.")]
    public string ethAccPKey = "8781c920ad1e9b0197a473f24af246179997ad4be45d85977d241d8ae09af4c8";


    private IEnumerator currentCoroutine;


    void Awake(){
        retrieveAccBal.onClick.AddListener(GetAccountBalance);
    }

    private async void GetAccountBalance() {
        try
        {
            var web3 = new Web3(networkUrl);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(ethAccKey);
            consoleMessage.text = $"Account Balance: {Nethereum.Util.UnitConversion.Convert.FromWei(balance, 18).ToString("n8")}eth";
        }
        catch (System.Exception e)
        {
            consoleMessage.text = "Error: Check Debug Log";
            Debug.Log(e);
        }
    }




}

