using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class XRRaycaster : MonoBehaviour
{
    [SerializeField]
    private GameObject placeObject;

    public GameObject PlaceObject
    {
        get { return placeObject; }
        set { placeObject = value; }
    }

    private ARRaycastManager raycast_manager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycast_manager = GetComponent<ARRaycastManager>();
    }

    private bool GetTouchPosition(out Vector2 touch_position)
    {
        if (Input.touchCount > 0)
        {
            touch_position = Input.GetTouch(0).position;
            return true;
        }

        touch_position = default;
        return false;
    }

    private void Update()
    {
        if (!GetTouchPosition(out Vector2 touch_position))
            return;

        if (raycast_manager.Raycast(touch_position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            var hit_pose = hits[0].pose;

            Instantiate(placeObject, hit_pose.position, hit_pose.rotation);
        }
    }
}
