using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ModelManager : MonoBehaviour
{
    [SerializeField]
    private Button updateModelButton;
    
    void Awake(){
        // add try catch phrase for null pointer exception -- not urgent
        updateModelButton.onClick.AddListener(UpdateModel);
    }

    private void UpdateModel(){
        
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
