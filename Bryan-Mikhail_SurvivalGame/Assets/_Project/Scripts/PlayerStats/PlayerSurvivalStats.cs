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

    public float hungerDecayRate = 1f; // per minute
    public float thirstDecayRate = 1f; // per minute
    public float staminaDecayRate = 5f; // per second while running
    public float staminaRecoveryRate = 10f; // per second while resting
    public float thirstRunningMultiplier = 1.5f; // thirst goes down faster when running
    public float staminaMinForRun = 10f; // minimum stamina to run

    private void Start()
    {
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        UpdateBars();
        UpdateHunger();
        UpdateThirst();
        UpdateStamina();
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

    //public void TakeDamage(float amount)
    //{
    //    currentHealth -= amount;
    //    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    //}

    private void Die()
    {
        // Handle player death here
    }
}
