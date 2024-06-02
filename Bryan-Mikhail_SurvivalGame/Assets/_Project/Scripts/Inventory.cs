using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> resources = new Dictionary<string, int>();

    public void AddResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] += amount;
        }
        else
        {
            resources.Add(resourceName, amount);
        }

        Debug.Log("Collected " + amount + " " + resourceName + ". Total: " + resources[resourceName]);
    }

    public Dictionary<string, int> GetResources()
    {
        return resources;
    }
}
