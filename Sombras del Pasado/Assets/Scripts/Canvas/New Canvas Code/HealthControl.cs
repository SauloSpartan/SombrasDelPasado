using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls health bar sincronization
public class HealthControl : MonoBehaviour
{
    // Variables
    [SerializeField] private Image _healthBar;
    private float _lerpSpeed;

    /// <summary>
    /// Function that controls health bar going low and color, it receives two floats.
    /// </summary>
    /// <param name="playerHealth"> Float that represents the health that changes over time.</param>
    /// <param name="playerMaxHealth"> Float that represents the health that doesn't change.</param>
    public void HealthBarControl(float playerHealth, float playerMaxHealth)
    {
        _lerpSpeed = 3f * Time.deltaTime;
        _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount, playerHealth / playerMaxHealth, _lerpSpeed);
        Color healthColor = Color.Lerp(Color.red, Color.green, (playerHealth / playerMaxHealth));
        _healthBar.color = healthColor;
    }
}
