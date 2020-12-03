using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyCell : MonoBehaviour
{
    [SerializeField] private Image fill;

    public void SetWidth(float width)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 new_size = new Vector2(width, rect.sizeDelta.y);

        rect.sizeDelta = new_size;
        fill.GetComponent<RectTransform>().sizeDelta = new_size;
    }

    public void SetAnchoredX(float x)
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
    }
}
