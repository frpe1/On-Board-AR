using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceHologram : MonoBehaviour
{
    // The prefab to instantiate on touch.
    [SerializeField]
    private GameObject _prefabToPlace;

    // Cache ARRaycastManager GameObject from ARCoreSession
    private ARRaycastManager _raycastManager;

    // List for raycast hits is re-used by raycast manager
    private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
    }
    void Update()
    {
        // Only consider single-finger touches that are beginning
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) { return; }

        // Perform AR raycast to any kind of trackable
        if (_raycastManager.Raycast(touch.position, Hits, TrackableType.AllTypes))
        {
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            var hitPose = Hits[0].pose;

            // Instantiate the prefab at the given position
            // Note: the object is not anchored yet!
            //Instantiate(_prefabToPlace, hitPose.position, hitPose.rotation);
            CreateAnchor(Hits[0]);

            // Debug output what we actually hit
            Debug.Log($"Instantiated on: {Hits[0].hitType}");
        }
    }

    ARAnchor CreateAnchor(in ARRaycastHit hit)
    {
        ARAnchor anchor;

        // ... here, we'll place the plane anchoring code!

        // Otherwise, just create a regular anchor at the hit pose

        // Note: the anchor can be anywhere in the scene hierarchy
        var instantiatedObject = Instantiate(_prefabToPlace, hit.pose.position, hit.pose.rotation);

        // Make sure the new GameObject has an ARAnchor component
        anchor = instantiatedObject.GetComponent<ARAnchor>();
        if (anchor == null)
        {
            anchor = instantiatedObject.AddComponent<ARAnchor>();
        }
        Debug.Log($"Created regular anchor (id: {anchor.nativePtr}).");

        return anchor;
    }
}
