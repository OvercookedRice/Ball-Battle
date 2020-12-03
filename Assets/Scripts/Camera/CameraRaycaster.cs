using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    [SerializeField] private LayerMask field_layer;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(ray, out info, 100, field_layer))
            {
                info.collider.GetComponent<Field>().Spawn(info.point);
            }
        }
//#if (UNITY_STANDALONE || UNITY_EDITOR)
//#elif (UNITY_ANDROID || UNITY_IOS)
//#endif
    }
}
