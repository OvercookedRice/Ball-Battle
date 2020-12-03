using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Soldier parent;
    void Start()
    {
        parent = null;
        GameManager.GetInstance().RegisterBall(this);
    }

    public void Attach(Soldier to)
    {
        if (transform.parent != null) return;

        parent = to;
        transform.parent = to.transform;
    }

    public void Detach()
    {
        transform.parent = null;
        parent = null;
    }

    public Soldier GetHolder() => parent;
    public float SqrDistanceTo(Transform to) => (transform.position - to.position).sqrMagnitude;
    public bool IsAttached() => transform.parent != null;
}
