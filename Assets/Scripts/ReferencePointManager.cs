using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARReferencePointManager))]
[RequireComponent(typeof(ARPlaneManager))]

public class ReferencePointManager : MonoBehaviour
{
    [SerializeField]
    private Text debugLog;

    [SerializeField]
    private Text referencePointCount;

    [SerializeField]
    private Button togglePlaneDetectionButton;

    [SerializeField]
    private Button clearReferencePointButton;

    private ARRaycastManager arRaycastManager;
    private ARReferencePointManager arReferencePointManager;
    private ARPlaneManager arPlaneManager;
    private List<ARReferencePoint> referencePoints = new List<ARReferencePoint>();
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    void Awake(){
        arRaycastManager = GetComponent<ARRaycastManager>();
        arReferencePointManager = GetComponent<ARReferencePointManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();

        togglePlaneDetectionButton.onClick.AddListener(TogglePlaneDetection);
        clearReferencePointButton.onClick.AddListener(ClearReferencePoints);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0){
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began){
            return;
        }

        if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon)){
            Pose hitPose = hits[0].pose;
            ARReferencePoint referencePoint = arReferencePointManager.AddReferencePoint(hitPose);

            if (referencePoint == null){
                string errorEntry = "There was an error creating a reference point";
                Debug.Log(errorEntry);
                debugLog.text += errorEntry;
            }
            else {
                referencePoints.Add(referencePoint);
                referencePointCount.text = $"Reference Point Count: {referencePoints.Count}";
            }
        }
    }

    private void TogglePlaneDetection() {
        arPlaneManager.enabled = !arPlaneManager.enabled;

        foreach (ARPlane plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(arPlaneManager.enabled);
        }

        togglePlaneDetectionButton.GetComponentInChildren<Text>().text = arPlaneManager.enabled ?
            "Disable Plane Detection" : "Enable Plane Detection";

    }

    private void ClearReferencePoints(){
        foreach(ARReferencePoint referencePoint in referencePoints)
        {
            arReferencePointManager.RemoveReferencePoint(referencePoint);
        }
        referencePoints.Clear();
        referencePointCount.text = $"Reference Point Count: {referencePoints.Count}";
    }
    
}
