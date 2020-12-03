using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCircle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer circle;
    // Start is called before the first frame update
    void Start()
    {
        // Unparent the object
        Transform parent = transform.parent;
        transform.parent = null;

        transform.localScale = Vector3.one;

        // Get the current circle's size, and remove y coordinate out of it
        Vector3 current_circle_size = circle.bounds.extents;
        current_circle_size = new Vector3(current_circle_size.x, current_circle_size.x, 0f);
        current_circle_size = new Vector3((current_circle_size.x.Equals(0f) ? 1f : current_circle_size.x),
                                    (current_circle_size.y.Equals(0f) ? 1f : current_circle_size.y),
                                    (current_circle_size.z.Equals(0f) ? 1f : current_circle_size.z));

        // Get the field's size, and remove y coordinate out of it
        Vector3 field_size = GameManager.GetInstance().GetDefenderField().GetComponent<MeshRenderer>().bounds.extents;
        field_size = new Vector3(field_size.x, field_size.x, 1f);

        Vector3 final_scale = (field_size * Constants.DEFENDER__DETECTION_RANGE);
        final_scale = new Vector3(final_scale.x / current_circle_size.x, 
                                final_scale.y / current_circle_size.y, 
                                final_scale.z / current_circle_size.z);

        // Expand this circle size to match 35% width of the field
        transform.localScale = final_scale;

        // Then put it back to where it should be
        transform.parent = parent;
    }
}
