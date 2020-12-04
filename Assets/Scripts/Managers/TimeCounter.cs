using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text time_indication;
    [SerializeField] private TMP_Text countdown_text;

    private float elapsed_time;
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
            counting = false;
            time_indication.text = (int)Constants.MATCH__TIME_LIMIT + "s";

            Camera.main.gameObject.GetComponent<CameraRaycaster>().DisableCasting();
            countdown_text.text = "";

            yield return new WaitForSeconds(1.25f);
            for (int i = 3; i > 0; i--)
            {
                countdown_text.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

            countdown_text.text = "";
            Camera.main.gameObject.GetComponent<CameraRaycaster>().EnableCasting();

            GameManager.GetInstance().StartMatch();

            counting = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!counting) return;

        elapsed_time += Time.deltaTime;
        time_indication.text = (int)(Constants.MATCH__TIME_LIMIT - elapsed_time) + "s";

        if (elapsed_time >= Constants.MATCH__TIME_LIMIT)
        {
            GameManager.GetInstance().NotifyMatchWinner(MatchScenario.Draw);
        }
    }
}
