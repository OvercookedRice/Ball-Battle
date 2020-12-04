using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Field : Element
{
    [SerializeField] private float spawn_offset = 1.5f;
    [SerializeField] private GameObject soldier;
    public override void Switch(Player player)
    {
        // Store the side
        parent = player;

        if (parent.GetFaction() == GameManager.GetInstance().GetAttacker())
        {
            if (GameManager.GetInstance().IsPenalty()) return;

            // Spawn the ball randomly on this field
            Vector3 extents = GetComponent<MeshRenderer>().bounds.extents;

            // Calculate the NEW extents with defined offset, so that the ball would be spawned somewhere better.
            extents = new Vector3(extents.x - spawn_offset, 0f, extents.z - spawn_offset);
            Vector3 randomized_position = transform.position + new Vector3(Random.Range(-extents.x, extents.x),
                                             0,
                                            Random.Range(-extents.z, extents.z));

            GameManager.GetInstance().SpawnBall(randomized_position);
        }
        else
        {
            GameManager.GetInstance().RegisterDefenderField(this);
        }
    }

    public void Spawn(Vector3 pos)
    {
        bool is_defender = (GetFaction() == GameManager.GetInstance().GetAttacker() ? false : true);

        if (parent.Energy.CanUseEnergy(is_defender ? Constants.DEFENDER__ENERGY_COST_TO_SPAWN : Constants.ATTACKER__ENERGY_COST_TO_SPAWN))
        {
            GameObject _ = soldier.Spawn(pos + Vector3.up * soldier.GetComponentInChildren<MeshRenderer>().bounds.extents.y);

            _.GetComponent<ISideSwitcher>().Switch(parent);
        }        
    }
}
