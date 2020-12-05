using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        attacker = Faction.Opponent;
    }

    private void ClearLists(bool clear_player)
    {
        soldiers.Clear();
        attackers.Clear();

        if (clear_player)
            players.Clear();
    }

    public void ChangeScene(string scene_name)
    {
        if (scene_name == Constants.MAIN_GAME_SCENE)
        {
            time_counter = null;
            ClearLists(true);

            SceneManager.LoadScene(Constants.MAIN_GAME_SCENE);
            StartOver();
        }
        else if (scene_name == Constants.PENALTY_GAME_SCENE)
        {
            time_counter = null;
            ClearLists(true);

            SceneManager.LoadScene(Constants.PENALTY_GAME_SCENE);
            StartPenalty();
        }
        else if (scene_name == Constants.MENU_SCENE)
        {
            SceneManager.LoadScene(Constants.MENU_SCENE);
        }
    }

    public void StartOver()
    {
        StartCoroutine(WaitAndSetting());

        IEnumerator WaitAndSetting()
        {

            while(time_counter == null)
            {
                yield return null;
            }

            playing_regular_game = true;

            match_holder.ShowScore();

            is_wiping_lists = true;

            foreach (Soldier _ in soldiers)
            {
                _.Bench(true);
            }

            is_wiping_lists = false;
            if (ball != null)
            {
                Destroy(ball.gameObject);
                ball = null;
            }

            ClearLists(false);

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
    }
    public void StartPenalty()
    {
        StartCoroutine(WaitAndSetting());

        IEnumerator WaitAndSetting()
        {
            playing_regular_game = false;

            while (time_counter == null)
            {
                yield return null;
            }

            attacker = Faction.Player;

            foreach (Player _ in players)
            {
                _.OnNewMatch();
            }

            time_counter.TeaBreak();
        }
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
        ChangeScene(Constants.PENALTY_GAME_SCENE);
    }

    public void NotifyMatchWinner(MatchScenario scenario)
    {
        if (playing_regular_game)
        {
            match_holder.NotifyMatchOver(scenario);

            if (!match_holder.GameOver())
            {
                StartOver();
            }
        }
        else
        {
            if (scenario == MatchScenario.Draw)
            {
                match_holder.NotifyPenaltyLose();
            }
            else
            {
                match_holder.NotifyPenaltyWon();
            }
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
