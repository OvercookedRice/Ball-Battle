using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class XRRaycaster : MonoBehaviour
{
    public bool HasPlacedField
    {
        get { return game_field_placed; }
    }

    private ARRaycastManager raycast_manager;
    [SerializeField] private ARPlaneManager plane_manager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool game_field_placed = false;

    private void Awake()
    {
        raycast_manager = GetComponent<ARRaycastManager>();
        game_field_placed = false;

        plane_manager.enabled = true;
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
        if (game_field_placed || !GetTouchPosition(out Vector2 touch_position))
            return;

        if (raycast_manager.Raycast(touch_position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds))
        {
            var hit_pose = hits[0].pose;

            GameManager.GetInstance().PutGameField(hit_pose.position, hit_pose.rotation);
            game_field_placed = true;

            plane_manager.SetTrackablesActive(false);
            plane_manager.enabled = false;
        }
    }

    private void OnEnable()
    {
        GameManager.GetInstance()?.RegisterXRRaycaster(this);
    }

    private void OnDestroy()
    {
        GameManager.GetInstance()?.UnregisterXRRaycaster();
    }

}
