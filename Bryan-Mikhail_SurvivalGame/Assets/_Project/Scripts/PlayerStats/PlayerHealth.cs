using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public float bulletDamage = 10f;

    public Image frontHealthBar;
    public Image backHealthBar;

    public Image overlay;
    public float duration;
    public float fadeSpeed;

    private float durationTimer;

    public float regenRate = 2f; // Health per second

    private PlayerSurvivalStats survivalStats;

    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        survivalStats = GetComponent<PlayerSurvivalStats>();

        if (survivalStats == null)
        {
            Debug.LogError("PlayerSurvivalStats component is missing from the player.");
            return;
        }
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (overlay.color.a > 0 && health >= 30)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }

        RegenerateHealth();
    }

    void RegenerateHealth()
    {
        if (survivalStats.currentHunger > 50 && survivalStats.currentThirst > 50 && health < maxHealth)
        {
            health += regenRate * Time.deltaTime;
            health = Mathf.Clamp(health, 0, maxHealth);
            lerpTimer = 0f; // Reset lerp timer to smooth the bar update
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;

        float hFraction = health / maxHealth;
        frontHealthBar.fillAmount = hFraction;

        if (fillB > hFraction)
        {
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        else if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.6f);

        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died");
        // Handle player death here
    }
}
