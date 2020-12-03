using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private Faction attacker = Faction.Player;

    [SerializeField] private Material player_material = null;
    [SerializeField] private Material opponent_material = null;
    [SerializeField] private Material greyscale_material = null;
    [SerializeField] private GameObject spawning_ball = null;

    private Ball ball = null;
    private Fence defender_fence = null;
    private Gate defender_gate = null;
    private Field defender_field = null;

    private List<Soldier> attackers = null;

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

    void Start()
    {
        attackers = new List<Soldier>();
    }
    /******************************************
    *                                         *
    *            SPAWN BALL SECTION           *
    *                                         *
    ******************************************/
    public void SpawnBall(Vector3 position)
    {
        if (ball == null)
        {
            float ball_height = spawning_ball.GetComponentInChildren<MeshRenderer>().bounds.extents.y;
            Instantiate(spawning_ball, position + Vector3.up * ball_height, Quaternion.identity);
        }
    }
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

    public void RegisterDefenderGate(Gate gate)
    {
        defender_gate = gate;
    }

    public void RegisterDefenderField(Field field)
    {
        defender_field = field;
    }

    public void RegisterAttacker(Soldier soldier)
    {
        if (!attackers.Contains(soldier))
        {
            attackers.Add(soldier);
        }
    }

    public void UnregisterAttacker(Soldier soldier)
    {
        if (attackers.Contains(soldier))
        {
            attackers.Remove(soldier);
        }
    }

    /******************************************
    *                                         *
    *              DATA RETRIEVAL             *
    *                                         *
    ******************************************/
    public Faction GetAttacker() => attacker;
    public Ball GetBall() => ball;
    public Fence GetDefenderFence() => defender_fence;
    public Field GetDefenderField() => defender_field;
    public Gate GetDefenderGate() => defender_gate;
    public List<Soldier> GetAttackers(Soldier except = null)
    {
        if (except == null) return attackers;

        return attackers.Where(x => x != except).ToList();
    }
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
