using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AzureSky;

public class DayCounter : MonoBehaviour
{
    public TextMeshProUGUI dayCounterText;  // Reference to the UI Text
    public AzureTimeController azureTimeController;  // Reference to the Azure Time Controller

    void Start()
    {
        // Update the UI text initially
        UpdateDayCounter();

        // Subscribe to the day change event
        azureTimeController.m_onDayChange.AddListener(OnDayChange);
    }

    void OnDayChange()
    {
        // Update the UI text when the day changes
        UpdateDayCounter();
    }

    void UpdateDayCounter()
    {
        // Set the day counter text
        dayCounterText.text = "Days: " + azureTimeController.m_day;
    }

    void OnDestroy()
    {
        // Unsubscribe from the day change event
        azureTimeController.m_onDayChange.RemoveListener(OnDayChange);
    }
}
