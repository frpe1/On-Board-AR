using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using AugmentedRealityCourse;

public class ARPlaceHologram : MonoBehaviour
{
    // The prefab to instantiate on touch.
    [SerializeField]
    private GameObject _prefabToPlace;

    private GameObject instantiatedObject;

    // Cache ARRaycastManager GameObject from ARCoreSession
    private ARRaycastManager _raycastManager;

    private ARAnchorManager _anchorManager;

    private ARPlaneManager _planeManager;

    private ARAnchor _anchor;

    // List for raycast hits is re-used by raycast manager
    private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _anchorManager = GetComponent<ARAnchorManager>();
        _planeManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        // Only consider single-finger touches that are beginning
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) { return; }

        // Perform AR raycast to any kind of trackable
        if (_raycastManager.Raycast(touch.position, Hits, TrackableType.FeaturePoint))
        {
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            Pose hitPose = Hits[0].pose;

            // Instantiate the prefab at the given position
            // Note: the object is not anchored yet!
            if (_anchor == null)
                _anchor = CreateAnchor(Hits[0]);
            else
            {
                // Detta förflyttar runt kopian, ska senare 
                // justeras med hjälp av image trackingen (om det behövs)
                instantiatedObject.transform.position = hitPose.position;
                instantiatedObject.transform.rotation = hitPose.rotation;
            }
        }
    }

    ARAnchor CreateAnchor(in ARRaycastHit hit)
    {
        ARAnchor anchor;

        if (instantiatedObject == null)
        {
            instantiatedObject = Instantiate(_prefabToPlace, hit.pose.position, hit.pose.rotation);
        }

        if (hit.trackable is ARPlane plane)
        {

            // Dessa fyra rader har skrivits i syfte att skapa en anchor punkt PÅ ett
            // plan
            var oldPrefab = _anchorManager.anchorPrefab;
            _anchorManager.anchorPrefab = instantiatedObject;
            anchor = _anchorManager.AttachAnchor(plane, hit.pose);
            _anchorManager.anchorPrefab = oldPrefab;
                
            DebugManager.Instance.AddDebugMessage($"A  (id: {anchor.nativePtr}).");

        }
        else
        {
            // Make sure the new GameObject has an ARAnchor component
            anchor = instantiatedObject.GetComponent<ARAnchor>();
            if (anchor == null)
            {
                anchor = instantiatedObject.AddComponent<ARAnchor>();
            }

            DebugManager.Instance.AddDebugMessage($"B (id: {anchor.nativePtr}).");
        
        }

        return anchor;
    }
}
