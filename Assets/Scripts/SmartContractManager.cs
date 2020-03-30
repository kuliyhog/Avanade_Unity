using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Nethereum.Web3;
using Nethereum.Quorum;
using ServerSettings;

public class SmartContractManager : MonoBehaviour
{
    private string address;
    [SerializeField]
    private Button runScriptButton;

    [SerializeField]
    private TextMeshProUGUI consoleMessage;

    private void Awake()
    {
        runScriptButton.onClick.AddListener(Main);
    }

    private void Main() {
        QuorumContractInteraction().Wait();
        RopstenContractInteraction().Wait();
        GetAccountBalance().Wait();
    }

    private async Task GetAccountBalance(){
        RopstenSettings rp = new RopstenSettings();
        MetamaskSettings ms = new MetamaskSettings();

    }
    private async Task QuorumContractInteraction(){}
    private async Task RopstenContractInteraction(){}
}

