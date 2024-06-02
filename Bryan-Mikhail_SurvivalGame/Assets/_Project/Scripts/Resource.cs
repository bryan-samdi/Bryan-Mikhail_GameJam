using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int resourceAmount = 5; // Amount of resource to collect
    public string resourceName; // Name of the resource
    public GameObject healthBarPrefab; // Prefab for the health bar

    private GameObject healthBarInstance;
    private Image frontHealthBar;
    private Image backHealthBar;
    public float chipSpeed = 2f;

    void Start()
    {
        currentHealth = maxHealth;
        InitializeHealthBar();
    }

    void Update()
    {
        if (healthBarInstance.activeSelf)
        {
            FaceHealthBarToCamera();
        }
    }

    void InitializeHealthBar()
    {
        healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
        frontHealthBar = healthBarInstance.transform.Find("FrontHealthBar").GetComponent<Image>();
        backHealthBar = healthBarInstance.transform.Find("BackHealthBar").GetComponent<Image>();
        SetHealthBar(1f, 1f);
        healthBarInstance.SetActive(false);
    }

    void FaceHealthBarToCamera()
    {
        healthBarInstance.transform.LookAt(Camera.main.transform);
        healthBarInstance.transform.Rotate(0, 180, 0);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            CollectResource();
        }
    }

    void UpdateHealthUI()
    {
        float healthFraction = (float)currentHealth / maxHealth;
        SetHealthBar(healthFraction, healthFraction);
    }

    void SetHealthBar(float frontFillAmount, float backFillAmount)
    {
        frontHealthBar.fillAmount = frontFillAmount;
        backHealthBar.fillAmount = backFillAmount;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healthBarInstance.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healthBarInstance.SetActive(false);
        }
    }

    public void CollectResource()
    {
        Debug.Log("Resource collected: " + resourceName);
        Destroy(healthBarInstance);
        Destroy(gameObject);
    }
}
