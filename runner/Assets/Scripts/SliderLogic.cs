using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SliderLogic : MonoBehaviour
{
    public GameObject slider1,slider2;

    private void Start()
    {
        //stays invisible for 1s because transition
        StartCoroutine(FlashInvisible());
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

