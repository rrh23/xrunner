using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkScript : MonoBehaviour
{
    public LogicScript logic;
    public SpriteRenderer sr;
    private Color rgb;
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rgb = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (logic.isEDCollected)
        {
            StartCoroutine(FlashClear());
            Debug.Log("full health!!");
        }
    }

    private IEnumerator FlashClear()
    {
        sr.color = Color.clear;
        yield return new WaitForSeconds(6f);
        sr.color = rgb;
    }
}

