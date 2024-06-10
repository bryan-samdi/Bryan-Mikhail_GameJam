using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject healthBarUI;
    public Image healthBar;
    AudioManager audioManager;

    private Camera mainCamera;



    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        currentHealth = maxHealth;
        mainCamera = Camera.main;

        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (healthBarUI != null && healthBarUI.activeSelf && mainCamera != null)
        {
            healthBarUI.transform.LookAt(healthBarUI.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }

    public void TakeDamage(float amount)
    {
        audioManager.PlaySFX(audioManager.enemyHitSound);

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true);
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        audioManager.PlaySFX(audioManager.enemyDie);

        Destroy(gameObject);
    }
}
