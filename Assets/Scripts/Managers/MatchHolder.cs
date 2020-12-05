using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchHolder : MonoBehaviour
{
    [SerializeField] private Image overlay_image;
    [SerializeField] private TMP_Text winner_notification;
    [SerializeField] private TMP_Text player_score_text;
    [SerializeField] private TMP_Text opponent_score_text;

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
        overlay_image.gameObject.SetActive(false);

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
        if (GameOver()) return;

        switch (scenario)
        {
            case MatchScenario.PlayerWon:
                player_score++;
                break;

            case MatchScenario.OpponentWon:
                opponent_score++;
                break;
        }

        player_score_text.text = player_score.ToString();
        opponent_score_text.text = opponent_score.ToString();

        if (++matches >= Constants.MATCHES)
        {
            OnGameOver();
        }
    }

    public void ShowScore()
    {
        player_score_text.gameObject.SetActive(true);
        opponent_score_text.gameObject.SetActive(true);
    }

    public void HideScore()
    {
        player_score_text.gameObject.SetActive(false);
        opponent_score_text.gameObject.SetActive(false);
    }

    public void OnGameOver()
    {
        overlay_image.gameObject.SetActive(true);
        string final_text = "";

        if (player_score > opponent_score)
        {
            final_text = "Player Won! (" + player_score + " - " + opponent_score + ")";
        }
        else if (player_score < opponent_score)
        {
            final_text = "Enemy Won... (" + player_score + " - " + opponent_score + ")";
        }
        else
        {
            final_text = "Game Draw!\nPenalty!";
            GameManager.GetInstance().Invoke("NotifyDraw", 2.5f);
        }

        winner_notification.text = final_text;
    }

    public void NotifyPenaltyWon()
    {
        overlay_image.gameObject.SetActive(true);
        winner_notification.text = "Player won!";
    }

    public void NotifyPenaltyLose()
    {
        overlay_image.gameObject.SetActive(true);
        winner_notification.text = "Player lost...";
    }

    public bool GameOver() => matches >= Constants.MATCHES;
}

