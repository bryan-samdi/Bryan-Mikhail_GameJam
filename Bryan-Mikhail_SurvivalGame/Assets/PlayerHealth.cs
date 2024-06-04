using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int amount)
    {
        Debug.Log("Player took damage" + amount);
        health -= amount;
        if (health <= 0)
        {
            // Handle player death
            Debug.Log("Player Ded!!!");

        }
    }
}
