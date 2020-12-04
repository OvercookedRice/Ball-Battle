using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private Faction faction = Faction.Player;
    [SerializeField] private GameObject[] children_elements;
    [SerializeField] private TMP_Text indication_text;
    
    public EnergyBar Energy
    {
        get { return energy_bar; }
        set { energy_bar = value; }
    }

    private EnergyBar energy_bar = null;
    void Start()
    {
        OnNewMatch();
        GameManager.GetInstance().RegisterPlayer(this);
    }

    public void OnNewMatch()
    {
        if (indication_text != null)
        {
            indication_text.text = (faction == Faction.Player ? "Player" : "Enemy - AI");
            indication_text.text += (faction == GameManager.GetInstance().GetAttacker() ? " (Attacker)" : " (Defender)");
        }

        foreach (GameObject _ in children_elements)
        {
            _.GetComponent<ISideSwitcher>()?.Switch(this);
        }
    }

    public void Recharge()
    {
        energy_bar.StartFill();
    }

    public void Discharge()
    {
        energy_bar.Empty();
    }

    public Faction GetFaction() => faction;
}
