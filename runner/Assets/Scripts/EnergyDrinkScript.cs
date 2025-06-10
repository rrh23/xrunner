using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkScript : MonoBehaviour
{
    public LogicScript logic;
    public SpriteRenderer sr;
    private Color rgb;
    public ObstacleScript obstacleScript;
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
        //if (logic.isEDCollected)
        //{
        //    StartCoroutine(FlashClear());
        //    Debug.Log("full health!!");
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            obstacleScript.Release();
        }
    }
    
}

