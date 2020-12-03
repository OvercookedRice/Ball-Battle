using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Element
{
    public override void Switch(Player player)
    {
        base.Switch(player);

        if (player.GetFaction() != GameManager.GetInstance().GetAttacker())
        {
            GameManager.GetInstance().RegisterDefenderGate(this);
        }
    }

    void OnTriggerEnter(Collider collsion)
    {
        if (collsion.CompareTag("Ball") && GetFaction() != GameManager.GetInstance().GetAttacker())
        {
            // DO SOMETHING TO INDICATE THAT ATTACKER'S WON
            //GameManager.GetInstance()
            Debug.Log("Attacker's Won");
        }
    }
}
