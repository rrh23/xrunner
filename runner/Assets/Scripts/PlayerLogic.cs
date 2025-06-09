using System.Collections;
using System.Collections.Generic;
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
        currentStamina = Mathf.Clamp(mov.jumpTime - mov.jumpTimer, 0f, maxStamina);
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        stamina.SetMaxStamina(currentStamina);


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
            if (currentHealth > 0 && damageCooldown <= 0.1f)
            {
                currentHealth -= 1;
                health.SetHealth(currentHealth,maxHealth);
                Debug.Log("ugh!" + "(health: " + currentHealth + ")");
            }
            else if(currentHealth == 0) logic.GameOver();

            if(gameObject.activeSelf) StartCoroutine(Damaged());
        }
    }

    private IEnumerator Damaged()
    {
        damageCooldown = 1f;

        while (damageCooldown > 0)
        {
            damageCooldown -= Time.deltaTime;
            sr.color = Color.Lerp(Color.red, rgb, 1 - damageCooldown);
            yield return null;
        }

        sr.color = rgb;
        damageCooldown = 0f;
    }


    private IEnumerator FlashGreen()
    {
        sr.color = Color.green;
        yield return new WaitForSeconds(0.3f);
        sr.color = rgb;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("EnergyDrink"))
        {
            StartCoroutine(FlashGreen());
            currentHealth = maxHealth;
            health.SetHealth(maxHealth, maxHealth);
            logic.currentScore += 10;
            logic.isEDCollected = true;
        }
    }
}
