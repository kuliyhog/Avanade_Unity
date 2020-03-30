using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button vuforiaButton;
    
    [SerializeField]
    private Button arFoundationButton;
    
    [SerializeField]
    private Button reactButton;

    [SerializeField]
    private Button blockchainButton;

    [SerializeField]
    private Button blobStorageButton;

    [SerializeField]
    private GameObject featureNotImplementedPanel;

    [SerializeField]
    private Button okayButton;

    private void Awake()
    {
        vuforiaButton.onClick.AddListener(LinkToVuforia);
        arFoundationButton.onClick.AddListener(LinkToArFoundation);
        reactButton.onClick.AddListener(LinkToReact);
        okayButton.onClick.AddListener(DisablePanel);
        blockchainButton.onClick.AddListener(LinkToBlockchain);
        blobStorageButton.onClick.AddListener(LinkToBlobStorage);
    }

    private void LinkToVuforia() {
        PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("VuforiaPrototype");
    }
    private void LinkToArFoundation()
    {
        PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Prototype2");
    }

    private void LinkToBlockchain()
    {
        PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("BlockchainConnection");
    }

    private void LinkToBlobStorage()
    {
        PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("BlobStorage");
    }


    private void LinkToReact()
    {
        featureNotImplemented();
    }

    private void featureNotImplemented() {
        featureNotImplementedPanel.SetActive(true);
    }
    private void DisablePanel()
    {
        featureNotImplementedPanel.SetActive(false);
    }
}
