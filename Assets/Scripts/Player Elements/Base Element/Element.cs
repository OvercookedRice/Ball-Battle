using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour, ISideSwitcher
{
    protected Faction side;
    public virtual void Switch(Faction faction)
    {
        // Store the side
        side = faction;

        // Change color based on the faction
        gameObject.GetComponent<MeshRenderer>().material = GameManager.GetInstance().GetFactionMaterial(side);
    }

    public Faction GetSide() => side;
}
