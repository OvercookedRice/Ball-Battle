using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour, ISideSwitcher
{
    protected Player parent;
    public virtual void Switch(Player player)
    {
        // Store the side
        parent = player;

        // Change color based on the faction
        gameObject.GetComponent<MeshRenderer>().material = GameManager.GetInstance().GetFactionMaterial(player.GetFaction());
    }

    public Player GetPlayer() => parent;
    public Faction GetFaction() => parent.GetFaction();
}
