using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLogic : MonoBehaviour
{
    public Slider slider;

    public void SetMaxVal(float maxValue)
    {
        if (slider != null)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }
    public void SetStamina(float stamina)
    {
        slider.value = stamina;
    }
}
