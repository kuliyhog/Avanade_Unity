using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButtonScript : MonoBehaviour
{
    [SerializeField]
    private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(UnloadScene);
    }

    private void UnloadScene() {
        string sceneName = PlayerPrefs.GetString("lastLoadedScene");
        SceneManager.LoadScene(sceneName);
    }
    
}
