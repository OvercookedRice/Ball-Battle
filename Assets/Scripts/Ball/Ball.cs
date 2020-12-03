using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Soldier parent;
    Transform passing_target;

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
        passing_target = null;
    }

    public void Detach()
    {
        transform.parent = null;
        parent = null;
    }

    public void Pass(Soldier to)
    {
        passing_target = to.transform;
    }

    public Soldier GetHolder() => parent;
    public float SqrDistanceTo(Transform to) => (transform.position - to.position).sqrMagnitude;
    public bool IsAttached() => transform.parent != null;

    void Update()
    {
        if (passing_target != null)
        {
            Vector3 direction = (passing_target.position - transform.position).normalized;
            direction = new Vector3(direction.x, 0f, direction.z);

            transform.forward = direction;
            transform.position += direction * Constants.BALL__SPEED * Time.deltaTime;
        }
    }
}
