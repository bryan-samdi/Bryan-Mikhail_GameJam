using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class Resource : InteractableBase
    {
        [Header("Resource Settings")]
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public override void OnInteract()
        {
            ApplyDamage(10f); 
        }

        public void ApplyDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                CollectResource();
            }
        }

        private void CollectResource()
        {
            Debug.Log("Collected " + gameObject.name);
            Destroy(gameObject);
        }

        public float GetHealthPercent()
        {
            return currentHealth / maxHealth;
        }
    }
}
