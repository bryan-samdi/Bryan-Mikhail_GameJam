using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSurvivalStats : MonoBehaviour
{
    public Image hungerBar;
    public Image thirstBar;
    public Image staminaBar;

    public float maxHunger = 100f;
    public float maxThirst = 100f;
    public float maxStamina = 100f;

    public float currentHunger;
    public float currentThirst;
    public float currentStamina;

    public float hungerDecayRate = 1f;
    public float thirstDecayRate = 1f;
    public float staminaDecayRate = 5f;
    public float staminaRecoveryRate = 10f;
    public float thirstRunningMultiplier = 1.5f;
    public float staminaMinForRun = 10f;
    public float damageOverTime = 1f;

    private float damageTimer = 0f;
    private float damageInterval = 15f;

    private PlayerHealth playerHealth;

    private void Start()
    {
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        currentStamina = maxStamina;
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        UpdateBars();
        UpdateHunger();
        UpdateThirst();
        UpdateStamina();
        ApplyDamageOverTime();
    }

    private void UpdateBars()
    {
        hungerBar.fillAmount = currentHunger / maxHunger;
        thirstBar.fillAmount = currentThirst / maxThirst;
        staminaBar.fillAmount = currentStamina / maxStamina;
    }

    private void UpdateHunger()
    {
        currentHunger -= hungerDecayRate / 60f * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }

    private void UpdateThirst()
    {
        float decayRate = thirstDecayRate / 60f;
        if (GetComponent<PlayerMovement>().isRunning)
        {
            decayRate *= thirstRunningMultiplier;
        }
        currentThirst -= decayRate * Time.deltaTime;
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
    }

    private void UpdateStamina()
    {
        currentStamina += staminaRecoveryRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    private void ApplyDamageOverTime()
    {
        damageTimer += Time.deltaTime;

        if (damageTimer >= damageInterval)
        {
            if (currentHunger < 30f || currentThirst < 30f)
            {
                playerHealth.TakeDamage(damageOverTime);
            }

            damageTimer = 0f;
        }
    }

    public void EatFood(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }

    public void DrinkWater(float amount)
    {
        currentThirst += amount;
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
    }
}
