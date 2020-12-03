using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Faction faction = Faction.Player;
    [SerializeField] private GameObject[] children_elements;

    void Start()
    {
        OnNewMatch();
        GameManager.GetInstance().RegisterPlayer(this);
    }

    public void OnNewMatch()
    {
        foreach (GameObject _ in children_elements)
        {
            _.GetComponent<ISideSwitcher>()?.Switch(this);
        }
    }

    public Faction GetFaction() => faction;
}
