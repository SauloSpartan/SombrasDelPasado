using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    public float currentHealth;
    private float maxHealth = 100f;
    private float lerpSpeed;
    ControllerCharacter1 Player;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        Player = FindObjectOfType<ControllerCharacter1>();
    }


    private void Update()
    {
        HealthControl();

        lerpSpeed = 3f * Time.deltaTime;

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, lerpSpeed);

        ColorChanger();
    }

    private void HealthControl()
    {
        currentHealth = Player.health;
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    private void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (currentHealth / maxHealth));

        healthBar.color = healthColor;
    }
}
