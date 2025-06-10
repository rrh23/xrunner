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
        staminaSlider.maxValue = maxValue;
        staminaSlider.value = maxValue;
    }
}

