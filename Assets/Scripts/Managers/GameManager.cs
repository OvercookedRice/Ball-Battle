using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private Faction attacker = Faction.Player;

    [SerializeField] private Material player_material = null;
    [SerializeField] private Material opponent_material = null;
    [SerializeField] private Material greyscale_material = null;
    private Ball ball = null;
    private Fence defender_fence = null;

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
    *      REGISTER/UNREGISTER RETRIEVAL      *
    *                                         *
    ******************************************/
    public void RegisterBall(Ball ball)
    {
        this.ball = ball;
    }

    public void RegisterDefenderFence(Fence fence)
    {
        defender_fence = fence;
    }

    /******************************************
    *                                         *
    *              DATA RETRIEVAL             *
    *                                         *
    ******************************************/
    public Faction GetAttacker() => attacker;
    public Ball GetBall() => ball;
    public Fence GetDefenderFence() => defender_fence;
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
    public Material GetGreyscaleMaterial() => greyscale_material;
}
