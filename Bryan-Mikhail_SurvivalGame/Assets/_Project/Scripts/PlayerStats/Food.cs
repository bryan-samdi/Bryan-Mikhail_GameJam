using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class Food : InteractableBase
    {
        public float hungerValue = 20f; 

        public override void OnInteract()
        {
            base.OnInteract();
            PlayerSurvivalStats playerStats = FindObjectOfType<PlayerSurvivalStats>();
            if (playerStats != null)
            {
                playerStats.EatFood(hungerValue);
            }
            Destroy(gameObject);
            //Debug.Log("Player eats food");
        }
    }
}