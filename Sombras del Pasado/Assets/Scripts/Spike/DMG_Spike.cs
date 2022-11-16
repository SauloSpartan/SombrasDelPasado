using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMG_Spike : MonoBehaviour
{
    public float Damage;
    private float _baseDamage;

    void Awake()
    {
        _baseDamage = 15;
        Difficulty();
    }

    /// <summary>
    /// Function that changes spikes damage depending on difficulty.
    /// </summary>
    private void Difficulty()
    {
        if (MainMenu.Difficulty == 1) // Easy
        {
            Damage = _baseDamage / 2;
        }
        else if (MainMenu.Difficulty == 2) // Normal
        {
            Damage = _baseDamage;
        }
        else if (MainMenu.Difficulty == 3) // Hard
        {
            Damage = _baseDamage * 2;
        }
    }
}
