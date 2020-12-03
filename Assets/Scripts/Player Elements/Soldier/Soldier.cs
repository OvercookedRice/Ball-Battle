using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Element
{
    [SerializeField] protected MeshRenderer mesh_renderer;
    protected StateMachine state_machine = null;
    [SerializeField] protected DetectionCircle detection_circle = null;
    private DetectionCircle real_circle = null;

    public override void Switch(Player player)
    {
        parent = player;

        GameManager GM = GameManager.GetInstance();
        mesh_renderer.material = GM.GetFactionMaterial(parent.GetFaction());

        GM.RegisterSoldier(this);

        if (GM.GetAttacker() == player.GetFaction())
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
            mesh_renderer.material = GameManager.GetInstance()
                .GetGreyscaleMaterial();
        }
        else
        {
            mesh_renderer.material = GameManager.GetInstance()
                .GetFactionMaterial(parent.GetFaction());
        }
    }

    public void EnableDetection()
    {
        if (real_circle == null)
        {
            Vector3 extents = mesh_renderer.bounds.extents;

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

    public void Chase()
    {
        state_machine.ChangeState((state_machine as DefenderStateMachine).ChaseState);
    }
   
    void OnEnable()
    {
        DisableDetection();
    }
    public void Attack() => GameManager.GetInstance().RegisterAttacker(this);
    public void Bench(bool kill = false)
    {
        if (GetFaction() == GameManager.GetInstance().GetAttacker())
        {
            GameManager.GetInstance().UnregisterAttacker(this);
        }

        if (kill)
        {
            GameManager.GetInstance().UnregisterSoldier(this);
            gameObject.Kill();
        }
    }
    void Update()
    {
        state_machine.Update();
    }
}
