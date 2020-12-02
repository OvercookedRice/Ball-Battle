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
}
