using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private GameObject cell;
    private GameObject[] cells;

    void Start()
    {
        SetNumEnergy(6);
    }

    public void SetNumEnergy(int number)
    {
        if (cells != null)
        {
            foreach(GameObject _ in cells)
            {
                Destroy(_);
            }
        }

        cells = new GameObject[number];

        float whole_bar_width = GetComponent<RectTransform>().rect.width;
        float cell_new_width = whole_bar_width / number;

        float first_pos = -whole_bar_width / 2 + cell_new_width / 2; 
        for (int i = 0; i < number; i++)
        {
            cells[i] = Instantiate(cell, transform);

            EnergyCell c = cells[i].GetComponent<EnergyCell>();
            c.SetWidth(cell_new_width);
            c.SetAnchoredX(first_pos + cell_new_width * i);
        }
    }
}
