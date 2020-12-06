using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    private Vector3 original_position;
    private Vector3 ar_position;

    private Quaternion ar_rotation;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GetInstance().RegisterGameField(this);
        original_position = transform.position;
    }

    private void OnDestroy()
    {
        GameManager.GetInstance().UnregisterGameField();
    }

    public float GetScale() => transform.localScale.x;

    public void SetARPosition(Vector3 position, Quaternion rotation)
    {
        ar_position = position;
        ar_rotation = rotation;
    }

    public void ToARPosition()
    {
        transform.position = ar_position;
        transform.rotation = ar_rotation;
        transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    }

    public void ToOriginalPosition()
    {
        transform.position = original_position;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

}
