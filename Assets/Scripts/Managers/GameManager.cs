using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private Faction attacker = Faction.Player;

    [SerializeField] private Material player_material = null;
    [SerializeField] private Material opponent_material = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager GetInstance() => instance;


    /******************************************
    *                                         *
    *              DATA RETRIEVAL             *
    *                                         *
    ******************************************/
    public Faction GetAttacker() => attacker;
    public Material GetFactionMaterial(Faction side)
    {
        switch(side)
        {
            case Faction.Player:
                return player_material;
            default:
                return opponent_material;
        }
    }

}
