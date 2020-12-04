using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyCell : MonoBehaviour
{
    [SerializeField] private Image fill;

    private Color original_color;
    private Coroutine fill_coroutine;

    void Start()
    {
        fill.fillAmount = 0f;
    }

    public void StartFilling(EnergyBar bar, int number, float start_amount = 0f)
    {
        fill_coroutine = StartCoroutine(Filling());

        IEnumerator Filling()
        {
            float elapsed_time = 0f;

            float fill_time = 1f / (GameManager.GetInstance().GetAttacker() == bar.GetFaction() ? Constants.ATTACKER__ENERGY_REGENERATION_RATE : Constants.DEFENDER__ENERGY_REGENERATION_RATE);

            if (start_amount > 0f)
            {
                // If we're filling from the middle of the cell, recalculate the elapsed time to be accurate!
                elapsed_time = start_amount * fill_time;
            }

            ToggleColor(true);

            while (elapsed_time < fill_time)
            {
                fill.fillAmount = (elapsed_time / fill_time);
                elapsed_time += Time.deltaTime;

                yield return null;
            }

            ToggleColor(false);
            fill.fillAmount = 1f;

            bar.StartFill(++number);

            fill_coroutine = null;
        }
    }

    private void ToggleColor(bool filling)
    {
        if (filling)
        {
            fill.color = original_color - new Color(0.25f, 0.25f, 0.25f, 0f);
        }
        else
        {
            fill.color = original_color;
        }
    }

    public float EmptyCell()
    {
        if (fill_coroutine != null)
        {
            StopCoroutine(fill_coroutine);
            fill_coroutine = null;

            ToggleColor(false);
        }

        float filled_amount = fill.fillAmount;
        fill.fillAmount = 0f;

        return filled_amount;
    }

    public EnergyCell SetWidth(float width)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 new_size = new Vector2(width, rect.sizeDelta.y);

        rect.sizeDelta = new_size;
        fill.GetComponent<RectTransform>().sizeDelta = new_size;

        return this;
    }

    public EnergyCell SetAnchoredX(float x)
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);

        return this;
    }

    public EnergyCell SetFillColor(Color color)
    {
        original_color = color;
        fill.color = color;

        return this;
    }

}
