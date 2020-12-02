using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : Element
{
    [SerializeField] private GameObject soldier;
    public override void Switch(Player player)
    {
        // Store the side
        parent = player;

        if (parent.GetFaction() == GameManager.GetInstance().GetAttacker())
        {
            // TODO
            // Spawn the ball randomly on this field
        }
    }

    public void Spawn(Vector3 pos)
    {
        GameObject _ = Instantiate(soldier, 
                                    pos + Vector3.up * soldier.GetComponent<MeshRenderer>().bounds.extents.y, 
                                    Quaternion.identity,
                                    transform
                                );
        _.GetComponent<ISideSwitcher>().Switch(parent);
    }
}
