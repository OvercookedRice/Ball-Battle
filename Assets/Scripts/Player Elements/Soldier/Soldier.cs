using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Element
{
    protected StateMachine state_machine;

    public override void Switch(Player player)
    {
        base.Switch(player);

        if (GameManager.GetInstance().GetAttacker() == player.GetFaction())
        {
            state_machine = new AttackerStateMachine(this);
        }
        else
        {
            //state_machine = new DefenderStateMachine(this);
        }
    }
}
