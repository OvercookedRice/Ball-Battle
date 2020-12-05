using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Element
{
    private Color original_color;
    private Color faded_color;

    public override void Switch(Player player)
    {
        base.Switch(player);

        if (player.GetFaction() != GameManager.GetInstance().GetAttacker())
        {
            GameManager.GetInstance().RegisterDefenderGate(this);
        }

        original_color = GetComponent<MeshRenderer>().material.color;
        faded_color = original_color - new Color(0, 0, 0, 0.5f);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Ball") && GetFaction() != GameManager.GetInstance().GetAttacker())
        {
            GameManager.GetInstance().NotifyMatchWinner((GetFaction() == Faction.Player ? MatchScenario.OpponentWon : MatchScenario.PlayerWon));
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("ControlledSoldier"))
        {
            GetComponent<MeshRenderer>().material.color = faded_color;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("ControlledSoldier"))
        {
            GetComponent<MeshRenderer>().material.color = original_color;
        }
    }
}
