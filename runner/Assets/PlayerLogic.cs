using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public LogicScript logic;
    public SliderLogic slider;
    public PlayerMovement mov;
    public SpriteRenderer sr;
    private Color rgb;

    public float currentStamina,
        maxStamina = 1;
    public float currentHealth,
        maxHealth = 5;

    public float EDRegen;

    private void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

        EDRegen = 5f;
        maxStamina = mov.jumpStrength;
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        slider.SetMaxStamina(maxStamina);
        slider.SetMaxHealth(maxHealth);

        sr = GetComponentInChildren<SpriteRenderer>();
        rgb = sr.color;
    }
    private void Update()
    {
        currentStamina = mov.jumpTime - mov.jumpTimer;
        slider.SetStamina(currentStamina, maxStamina);

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
            if (currentHealth > 0)
            {
                currentHealth -= 1;
                slider.SetHealth(currentHealth, maxHealth);

                StartCoroutine(FlashRed());
            }
            else logic.GameOver();
        }
    }

    private IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = rgb;
    }

    private IEnumerator FlashGreen()
    {
        sr.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sr.color = rgb;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("EnergyDrink"))
        {
            StartCoroutine(FlashGreen());
            currentHealth = maxHealth;
            slider.SetHealth(maxHealth, maxHealth);
            logic.currentScore += 10;
            logic.isEDCollected = true;
        }
    }
}
