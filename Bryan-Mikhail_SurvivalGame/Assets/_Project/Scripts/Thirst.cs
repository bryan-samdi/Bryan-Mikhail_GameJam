using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

namespace VHS
{
    public class Thirst : InteractableBase
    {
        public float thirstValue = 20f; // Amount to replenish thirst

        public override void OnInteract()
        {
            base.OnInteract();
            // Replenish the player's thirst
            PlayerSurvivalStats playerStats = FindObjectOfType<PlayerSurvivalStats>();
            if (playerStats != null)
            {
                playerStats.DrinkWater(thirstValue);
            }
            // Optionally, destroy the water item after interaction
            //Destroy(gameObject);
            Debug.Log("Player drinks water");

        }
    }
}