using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class Food : InteractableBase
    {
        public float hungerValue = 20f; // Amount to replenish hunger

        public override void OnInteract()
        {
            base.OnInteract();
            //Replenish the player's hunger
            PlayerSurvivalStats playerStats = FindObjectOfType<PlayerSurvivalStats>();
            if (playerStats != null)
            {
                playerStats.EatFood(hungerValue);
            }
            //Optionally, destroy the food item after interaction
            Destroy(gameObject);
            Debug.Log("Player eats food");
        }
    }
}