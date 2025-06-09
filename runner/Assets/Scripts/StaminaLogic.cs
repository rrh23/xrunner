using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StaminaLogic : MonoBehaviour
{
    public UnityEngine.UI.Slider staminaSlider;
    public Color low, high;
    public Vector3 offset;

    public void SetStamina(float health)
    {
        staminaSlider.value = health;
        staminaSlider.fillRect.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.Lerp(low, high, staminaSlider.normalizedValue);
    }

    public void SetMaxStamina(float maxValue)
    {
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxValue;
            staminaSlider.value = maxValue;
        }
    }

    private void Start()
    {
        if (staminaSlider != null) staminaSlider.transform.rotation = Quaternion.Euler(0, 0, 90);

        //resize stamina slider
        staminaSlider.transform.localScale = new Vector3(0.8f, 1, 1);
    }

    private void Update()
    {

        offset = new Vector3(1f, 0f, 0);
        staminaSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}

