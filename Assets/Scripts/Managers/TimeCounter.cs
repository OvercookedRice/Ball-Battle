using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text time_indication;
    private float elapsed_time;
    // Start is called before the first frame update
    void Start()
    {
        StartOver();
        GameManager.GetInstance().RegisterTimeCounter(this);
    }

    public void StartOver()
    {
        elapsed_time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed_time += Time.deltaTime;
        time_indication.text = (int)(Constants.MATCH__TIME_LIMIT - elapsed_time) + "s";

        if (elapsed_time >= Constants.MATCH__TIME_LIMIT)
        {
            GameManager.GetInstance().NotifyMatchWinner(MatchScenario.Draw);
        }
    }
}
