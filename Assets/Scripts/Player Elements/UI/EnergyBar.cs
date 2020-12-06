using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour, ISideSwitcher
{
    [SerializeField] private GameObject cell;
    private Player player = null;
    private GameObject[] cells;

    private int max_energy = 0;
    private int num_usable_energy = 0;

    public void SetNumEnergy(int number)
    {
        num_usable_energy = 0;

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

        Color fill_color = GameManager.GetInstance().GetFactionMaterial(player.GetFaction()).color;

        for (int i = 0; i < number; i++)
        {
            cells[i] = Instantiate(cell, transform);

            EnergyCell c = cells[i].GetComponent<EnergyCell>();
            c.SetWidth(cell_new_width)
                .SetAnchoredX(first_pos + cell_new_width * i)
                .SetFillColor(fill_color);
        }
    }

    public void Switch(Player player)
    {
        this.player = player;
        player.Energy = this;

        max_energy = GameManager.GetInstance().GetAttacker() == GetFaction() ? Constants.ATTACKER__MAXIMUM_ENGERGY : Constants.DEFENDER__MAXIMUM_ENERGY;

        SetNumEnergy(max_energy);
    }

    public void StartFill(int number = 0, float initial_fill = 0f)
    {
        if (number >= 0) num_usable_energy = number;
        if (number >= cells.Length) return;

        cells[number].GetComponent<EnergyCell>().StartFilling(this, number, initial_fill);
    }

    public void Empty()
    {
        for (int i = 0; i < max_energy; i++)
        {
            cells[i].GetComponent<EnergyCell>().EmptyCell();
        }
    }

    public bool CanUseEnergy(int amount)
    {
        if (num_usable_energy >= amount)
        {
            bool keep_filling = num_usable_energy < max_energy;
            int last_cell = (keep_filling ? num_usable_energy : max_energy);

            for (int i = last_cell - 1; i > last_cell - 1 - amount; i--)
            {
                cells[i].GetComponent<EnergyCell>().EmptyCell();
            }

            float last_cell_fill_amount = keep_filling ? cells[num_usable_energy].GetComponent<EnergyCell>().EmptyCell() : 0f;

            StartFill(num_usable_energy - amount, last_cell_fill_amount);
            return true;
        }

        return false;
    }

    public Player GetPlayer() => player;
    public Faction GetFaction() => player.GetFaction();
}
