using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HealthLogic : MonoBehaviour
{
    public UnityEngine.UI.Slider healthSlider;
    public Color low, high;
    public Vector3 offset;

    public void SetHealth(float health, float maxHealth)
    {
        high = Color.green;
        healthSlider.gameObject.SetActive(health < maxHealth);
        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;

        healthSlider.fillRect.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.Lerp(low, high, healthSlider.normalizedValue);
    }
    public void SetMaxHealth(float maxValue)
    {
        healthSlider.maxValue = maxValue;
        healthSlider.value = maxValue;
    }

    private void Update()
    {
        //offset = new Vector3(0, 1f, 0);
        //healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
        //healthSlider.transform.position = Camera.main.transform.position + offset;
    }
}

