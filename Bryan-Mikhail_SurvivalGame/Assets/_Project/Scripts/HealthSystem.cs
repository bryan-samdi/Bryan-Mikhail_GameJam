using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject healthBarUI; // Assign the health bar UI element in the inspector
    public Image healthBar; // Assign the health bar image in the inspector

    private Camera mainCamera;

    private void Start()
    {
        currentHealth = maxHealth;
        mainCamera = Camera.main;

        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false); // Ensure health bar UI is initially inactive
        }
    }

    private void Update()
    {
        if (healthBarUI != null && healthBarUI.activeSelf)
        {
            healthBarUI.transform.LookAt(healthBarUI.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Object took damage");
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true); // Activate health bar UI when damaged
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Add your death logic here (e.g., destroy the game object, play an animation, etc.)
        Destroy(gameObject);
        Debug.Log("Object took Ded");

    }
}
