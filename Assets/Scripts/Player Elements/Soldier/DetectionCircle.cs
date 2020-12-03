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
        float original_circle_size = current_circle_size.x;

        // Get the field's size, and remove y coordinate out of it
        Vector3 field_size = GameManager.GetInstance().GetDefenderField().GetComponent<MeshRenderer>().bounds.extents;
        float detection_range = field_size.x * Constants.DEFENDER__DETECTION_RANGE;
        Vector3 final_scale = new Vector3(detection_range / original_circle_size,
                                detection_range / original_circle_size, 
                                1f);

        // Expand this circle size to match 35% width of the field
        transform.localScale = final_scale;

        // Then put it back to where it should be
        transform.parent = parent;

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Soldier") && GameManager.GetInstance().GetBall()?.GetHolder() == collision.GetComponent<Soldier>())
        {
            transform.parent.GetComponent<Soldier>().Chase();
        }
    }
}
