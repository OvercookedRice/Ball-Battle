using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Element
{
    protected StateMachine state_machine = null;
    [SerializeField] protected DetectionCircle detection_circle = null;
    private DetectionCircle real_circle = null;

    public override void Switch(Player player)
    {
        base.Switch(player);

        if (GameManager.GetInstance().GetAttacker() == player.GetFaction())
        {
            state_machine = new AttackerStateMachine(this);
        }
        else
        {
            state_machine = new DefenderStateMachine(this);
        }
    }

    public void ChangeMaterial(bool inactivate)
    {
        if (inactivate)
        {
            GetComponent<MeshRenderer>().material = GameManager.GetInstance()
                .GetGreyscaleMaterial();
        }
        else
        {
            GetComponent<MeshRenderer>().material = GameManager.GetInstance()
                .GetFactionMaterial(parent.GetFaction());
        }
    }

    public void EnableDetection()
    {
        if (real_circle == null)
        {
            Vector3 extents = GetComponent<MeshRenderer>().bounds.extents;

            real_circle = Instantiate(detection_circle, transform.position + (Vector3.down * 0.98f) * extents.y, Quaternion.Euler(-90, 0, 0), transform);
        }
        else
        {
            real_circle.gameObject.SetActive(true);
        }
    }

    public void DisableDetection()
    {
        real_circle?.gameObject.SetActive(false);
    }

    public void Caught()
    {
        if (parent.GetFaction() == GameManager.GetInstance().GetAttacker())
        {
            state_machine.ChangeState((state_machine as AttackerStateMachine).CaughtState);
        }
    }
    void Update()
    {
        state_machine.Update();
    }
}
