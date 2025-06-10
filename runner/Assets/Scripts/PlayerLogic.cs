using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public LogicScript logic;
    public SliderLogic slider;
    public HealthLogic health;
    public StaminaLogic stamina;
    public PlayerMovement mov;
    public SpriteRenderer sr;

    private float damageCooldown = 0f;
    private Color rgb;

    public float currentStamina,
        maxStamina;
    public float currentHealth,
        maxHealth = 5;

    public float EDRegen;

    public static PlayerLogic instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

        EDRegen = 5f;
        maxStamina = mov.jumpTime;
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        stamina.SetMaxStamina(maxStamina);
        health.SetMaxHealth(maxHealth);


        sr = GetComponentInChildren<SpriteRenderer>();
        rgb = sr.color;
    }
    private void Update()
    {
        //currentStamina = Mathf.Clamp(mov.jumpTime - mov.jumpTimer, 0f, maxStamina);
        currentStamina = mov.jumpTime - mov.jumpTimer;
        stamina.SetStamina(currentStamina);

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        if (logic.isEDCollected)
        {
            while (EDRegen > 0)
            {
                EDRegen -= Time.deltaTime;
            }
            if(EDRegen <= 0) logic.isEDCollected = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Spike"))
        {
            if(gameObject.activeSelf) StartCoroutine(Damaged());
        }
    }

    public IEnumerator Damaged()
    {
        if (currentHealth > 0 && damageCooldown <= 0.1f)
        {
            currentHealth -= 1;
            health.SetHealth(currentHealth,maxHealth);
            // Debug.Log("ugh!" + "(health: " + currentHealth + ")");
        }
        
        if(currentHealth == 0) logic.GameOver();

        if (!gameObject.activeSelf) yield break;
        damageCooldown = 1;
        Tween.Color(sr, Color.red, Color.white, 0.3f).OnComplete(() => damageCooldown = 0);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("EnergyDrink"))
        {
            // StartCoroutine(FlashGreen());
            currentHealth = maxHealth;
            health.SetHealth(maxHealth, maxHealth);
            logic.currentScore += 10;
            logic.isEDCollected = true;
            Tween.Color(sr, Color.green, Color.white, 0.8f);
        }
    }
}
