using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : Element
{
    public override void Switch(Player player)
    {
        base.Switch(player);

        if (player.GetFaction() != GameManager.GetInstance().GetAttacker())
        {
            GameManager.GetInstance().RegisterDefenderFence(this);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (GameManager.GetInstance().IsPenalty()) return;

        if (collision.CompareTag("Soldier") && collision.GetComponent<Element>().GetFaction() != GetFaction())
        {
            collision.GetComponent<Soldier>().Bench(true);
        }
    }
}
