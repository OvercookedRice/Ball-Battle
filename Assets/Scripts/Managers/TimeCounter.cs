using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text time_indication;
    [SerializeField] private TMP_Text countdown_text;

    private float elapsed_time;
    private bool done_teabreak = false;
    private bool counting = false;
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

    public void TeaBreak()
    {
        StartCoroutine(Break());

        IEnumerator Break()
        {
            DisableCounting();
            done_teabreak = false;

            time_indication.text = (int)Constants.MATCH__TIME_LIMIT + "s";

            GameManager.GetInstance().GetCameraRaycaster().DisableCasting();
            countdown_text.text = "";

            yield return new WaitForSeconds(1.25f);
            for (int i = 3; i > 0; i--)
            {
                countdown_text.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

            countdown_text.text = "";
            GameManager.GetInstance().GetCameraRaycaster().EnableCasting();

            GameManager.GetInstance().StartMatch();

            done_teabreak = true;
            EnableCounting();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!counting) return;

        elapsed_time += Time.deltaTime;

        int time_left = (int)(Constants.MATCH__TIME_LIMIT - elapsed_time);
        time_left = time_left < 0 ? 0 : time_left;

        time_indication.text = time_left + "s";

        if (elapsed_time >= Constants.MATCH__TIME_LIMIT)
        {
            GameManager.GetInstance().NotifyMatchWinner(MatchScenario.Draw);
        }
    }

    public bool IsCounting() => counting;
    public void EnableCounting()
    {
        if (done_teabreak)
            counting = true;
        else
            TeaBreak();
    }
    public void DisableCounting() => counting = false;
    public void ForceDisableCounting()
    {
        if (!done_teabreak)
        {
            StopAllCoroutines();
        }

        DisableCounting();
    }
}
