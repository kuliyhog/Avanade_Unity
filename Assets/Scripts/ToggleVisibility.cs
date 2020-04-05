using UnityEngine;
using UnityEngine.UI;

public class ToggleVisibility : MonoBehaviour
{

    [SerializeField]
    private Button toggleVisibilityButton;

    [SerializeField]
    private GameObject prefabHouse;

    void Awake()
    {
        toggleVisibilityButton.onClick.AddListener(TogglePrefab);
    }

    private void TogglePrefab()
    {
        bool isActive = prefabHouse.activeSelf;
        prefabHouse.SetActive(!isActive);
    }

}
