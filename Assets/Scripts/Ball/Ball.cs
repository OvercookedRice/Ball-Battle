using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    void Start()
    {
        GameManager.GetInstance().RegisterBall(this);
    }

    public void Attach(Transform to)
    {
        transform.parent = to;
    }

    public void Detach()
    {
        transform.parent = null;
    }

    public bool IsAttached() => transform.parent != null;
}
