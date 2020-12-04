using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private Faction attacker = Faction.Opponent;

    [SerializeField] private Material player_material = null;
    [SerializeField] private Material opponent_material = null;
    [SerializeField] private Material greyscale_material = null;
    [SerializeField] private GameObject spawning_ball = null;

    private Ball ball = null;
    private Fence defender_fence = null;
    private Gate defender_gate = null;
    private Field defender_field = null;

    private MatchHolder match_holder = null;
    private TimeCounter time_counter = null;

    private List<Player> players = null;
    private List<Soldier> attackers = null;
    private List<Soldier> soldiers = null;

    private bool is_wiping_lists = false;
    private bool playing_regular_game = true;

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
        soldiers = new List<Soldier>();
        players = new List<Player>();

        StartPenalty();
    }

    public void StartOver()
    {
        playing_regular_game = true;

        match_holder.ShowScore();

        is_wiping_lists = true;

        foreach(Soldier _ in soldiers)
        {
            _.Bench(true);
        }

        is_wiping_lists = false;
        if (ball != null)
        {
            Destroy(ball.gameObject);
            ball = null;
        }

        soldiers.Clear();
        attackers.Clear();

        defender_fence = null;
        defender_gate = null;
        defender_field = null;

        attacker = (attacker == Faction.Player ? Faction.Opponent : Faction.Player);

        foreach (Player _ in players)
        {
            _.OnNewMatch();
            _.Discharge();
        }

        time_counter.TeaBreak();
    }
    public void StartPenalty()
    {
        playing_regular_game = false;

        foreach (Player _ in players)
        {
            _.OnNewMatch();
        }

        time_counter.TeaBreak();
    }
    public void StartMatch()
    {
        if (playing_regular_game)
        {
            foreach (Player _ in players)
            {
                _.Recharge();
            }

            match_holder.HideScore();
        }
        else
        {

        }

        time_counter.StartOver();
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

    public void RegisterSoldier(Soldier soldier)
    {
        if (!soldiers.Contains(soldier))
        {
            soldiers.Add(soldier);
        }
    }

    public void UnregisterSoldier(Soldier soldier)
    {
        if (!is_wiping_lists && soldiers.Contains(soldier))
        {
            soldiers.Remove(soldier);
        }
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

    public void RegisterMatchHolder(MatchHolder match_holder)
    {
        this.match_holder = match_holder;
    }

    public void UnregisterMatchHolder()
    {
        match_holder = null;
    }

    public void RegisterPlayer(Player player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    public void RegisterTimeCounter(TimeCounter time_counter)
    {
        this.time_counter = time_counter;
    }
    /******************************************
    *                                         *
    *           NOTIFICATION CENTER           *
    *                                         *
    ******************************************/

    public void NotifyDraw()
    {
        // GO TO THE PROCEDURAL GENERATOR THING
    }

    public void NotifyMatchWinner(MatchScenario scenario)
    {
        match_holder.NotifyMatchOver(scenario);

        if (!match_holder.GameOver())
        {
            StartOver();
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

    public bool IsPenalty() => !playing_regular_game;
}
