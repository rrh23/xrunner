using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderLogic : MonoBehaviour
{
    public GameObject slider1,slider2;
    public UnityEngine.UI.Slider staminaSlider, healthSlider;
    public Color low, high;
    public Vector3 offset;

    public void SetStamina(float health, float maxHealth)
    {
        //slider.gameObject.SetActive(health < maxHealth);
        staminaSlider.value = health;
        staminaSlider.maxValue = maxHealth;

        staminaSlider.fillRect.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.Lerp(low, high, staminaSlider.normalizedValue);
    }

    public void SetHealth(float health, float maxHealth)
    {
        healthSlider.gameObject.SetActive(health < maxHealth);
        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;

        healthSlider.fillRect.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.Lerp(low, high, healthSlider.normalizedValue);
    }

    public void SetMaxStamina(float maxValue)
    {
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxValue;
            staminaSlider.value = maxValue;
        }
    }
    public void SetMaxHealth(float maxValue)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxValue;
            healthSlider.value = maxValue;
        }
    }

    private void Start()
    {
        //stays invisible for 1s because transition
        StartCoroutine(FlashInvisible());

        if (staminaSlider != null) staminaSlider.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    private void Update()
    {

        offset = new Vector3(1f, -0.3f, 0);
        staminaSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);

        offset = new Vector3(0, 1f, 0);
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    private IEnumerator FlashInvisible()
    {
        //sr.color = Color.clear;
        slider1.SetActive(false);
        slider2.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        slider1.SetActive(true);
        slider2.SetActive(true);
        //sr.color = rgb;
    }
}

