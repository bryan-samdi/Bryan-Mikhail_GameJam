using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AzureSky;

public class DayCounter : MonoBehaviour
{
   // public TextMeshProUGUI dayCounterText; 
    public TextMeshProUGUI pauseMenuDayCounterText;  
    public TextMeshProUGUI gameOverDayCounterText;  
    public AzureTimeController azureTimeController;  

    void Start()
    {
        UpdateDayCounter();

        azureTimeController.m_onDayChange.AddListener(OnDayChange);
    }

    void OnDayChange()
    {
        UpdateDayCounter();
    }

    void UpdateDayCounter()
    {
        string dayText = "Days: " + azureTimeController.m_day;
       // dayCounterText.text = dayText;

        if (pauseMenuDayCounterText != null)
        {
            pauseMenuDayCounterText.text = dayText;
        }

        if (gameOverDayCounterText != null)
        {
            gameOverDayCounterText.text = dayText;
        }
    }

    void OnDestroy()
    {
        azureTimeController.m_onDayChange.RemoveListener(OnDayChange);
    }
}
