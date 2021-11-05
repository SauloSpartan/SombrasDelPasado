using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    public float currentHealth;
    private float maxHealth = 100f;
    ControllerCharacter1 Player;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        Player = FindObjectOfType<ControllerCharacter1>();
    }


    private void Update()
    {
        currentHealth = Player.health;
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
