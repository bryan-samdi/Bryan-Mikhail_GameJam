using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

namespace VHS
{
    public class Thirst : InteractableBase
    {
        public float thirstValue = 20f; 

        public override void OnInteract()
        {
            base.OnInteract();
            PlayerSurvivalStats playerStats = FindObjectOfType<PlayerSurvivalStats>();
            if (playerStats != null)
            {
                playerStats.DrinkWater(thirstValue);
            }
           

        }
    }
}