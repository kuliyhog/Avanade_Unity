using System.Collections;
using System.Collections.Generic;
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
        // add try catch phrase for null pointer exception -- not urgent
        toggleVisibilityButton.onClick.AddListener(TogglePrefab);
    }

    private void TogglePrefab()
    {
        bool isActive = prefabHouse.activeSelf;
        prefabHouse.SetActive(!isActive);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
