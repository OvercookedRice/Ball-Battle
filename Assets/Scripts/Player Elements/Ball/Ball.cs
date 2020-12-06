using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Soldier parent;
    Transform passing_target;
    GameField game_field;

    void Start()
    {
        game_field = FindObjectOfType<GameField>();

        transform.localScale = game_field.transform.localScale;

        float y_extent = GetComponentInChildren<MeshRenderer>().bounds.extents.y;

        transform.parent = game_field.transform;

        if (GameManager.GetInstance().ARMode)
        {
            transform.position = new Vector3(transform.position.x, game_field.transform.position.y + y_extent, transform.position.z);
        }

        parent = null;
        GameManager.GetInstance().RegisterBall(this);
    }

    public void Attach(Soldier to)
    {
        if (parent != null) return;

        parent = to;
        transform.parent = to.transform;

        transform.localPosition = new Vector3(0, transform.localPosition.y, 1.25f);
        passing_target = null;
    }

    public void Attach(ControlledSoldier to)
    {
        transform.parent = to.transform;

        transform.localPosition = new Vector3(0, transform.localPosition.y, 1.25f);
    }

    public void Detach()
    {
        transform.parent = game_field.transform;
        parent = null;
    }

    public void Pass(Soldier to)
    {
        passing_target = to.transform;
    }

    public Soldier GetHolder() => parent;
    public float SqrDistanceTo(Transform to) => (transform.position - to.position).sqrMagnitude;
    public bool IsAttached() => parent != null;

    void Update()
    {
        if (passing_target != null)
        {
            Vector3 direction = (passing_target.position - transform.position).normalized;
            direction = new Vector3(direction.x, 0f, direction.z);

            transform.forward = direction;
            transform.position += direction * Constants.BALL__SPEED * Time.deltaTime * game_field.GetScale();
        }
    }
}
