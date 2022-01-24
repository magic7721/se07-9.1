using System.Collections;
using System.Collections.Generic;
using UnityEngime;
using UnityEngime.XR.ARFoundation;
using UnityEngime.XR.ARSubsystems;

public class PlacementMGR : MonoBehaviour
{
    public GameObject placementIndicator;
    [SerializeField]
    private ARSessionOrigin arSessionOrigin;
    [SerializeField]
    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    [SerializeField]
    TrackableType trackableType = TrackableType.Planes;

    public GameObject objectToPlace;
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if(placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            PlaceObject();
        }
    }
    void PlaceObject() 
    {
        Instantiate(objectToPlace , placementPose.position, placementPose.rotation);
    }
    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position , placementPose.rotation);
        }else{
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = arSessionOrigin.camera.ViewportToScreenPoint(new Vector3(0.5f,0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits , trackableType);

        placementPoseIsValid = hits.Count > 0;
        if(placementPoseIsValid)
        {
        
            placementPose = hits[0].pose;
            var cameraForward = arSessionOrigin.camera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x , 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
