using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHolder : MonoBehaviour
{
    public int Matches
    {
        get { return matches; }
    }

    private int matches;
    private int player_score;
    private int opponent_score;

    // Start is called before the first frame update
    void Start()
    {
        Rematch();
        GameManager.GetInstance()?.RegisterMatchHolder(this);
    }


    public void Rematch()
    {
        matches = 0;
        player_score = 0;
        opponent_score = 0;
    }

    void OnDisable()
    {
        GameManager.GetInstance().UnregisterMatchHolder();
    }

    public void NotifyMatchOver(MatchScenario scenario)
    {
        switch (scenario)
        {
            case MatchScenario.PlayerWon:
                player_score++;
                break;

            case MatchScenario.OpponentWon:
                opponent_score++;
                break;
        }

        if (++matches >= Constants.MATCHES)
        {
            if (player_score > opponent_score)
            {
                GameManager.GetInstance().NotifyWinner(true);
            }
            else if (player_score > opponent_score)
            {
                GameManager.GetInstance().NotifyWinner(false);
            }
            else
            {
                GameManager.GetInstance().NotifyDraw();
            }
        }
    }

    public bool GameOver() => matches >= Constants.MATCHES;
}

