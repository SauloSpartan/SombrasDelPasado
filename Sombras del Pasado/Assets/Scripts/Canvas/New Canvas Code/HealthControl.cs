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
    /// <param name="characterHealth"> Float that represents the health that changes over time.</param>
    /// <param name="characterMaxHealth"> Float that represents the health that doesn't change.</param>
    public void HealthBarControl(float characterHealth, float characterMaxHealth)
    {
        _lerpSpeed = 3f * Time.deltaTime;
        _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount, characterHealth / characterMaxHealth, _lerpSpeed);
        Color healthColor = Color.Lerp(Color.red, Color.green, (characterHealth / characterMaxHealth));
        _healthBar.color = healthColor;
    }
}
