using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public TextMeshProUGUI resourceText;

    void Update()
    {
        DisplayResources();
    }

    void DisplayResources()
    {
        resourceText.text = "Resources:\n";
        foreach (var resource in playerInventory.GetResources())
        {
            resourceText.text += resource.Key + ": " + resource.Value + "\n";
        }
    }
}
