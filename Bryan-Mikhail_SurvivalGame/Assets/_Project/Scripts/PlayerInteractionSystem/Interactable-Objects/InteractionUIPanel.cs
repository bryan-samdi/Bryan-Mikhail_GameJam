using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VHS
{
    public class InteractionUIPanel : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI toolTipText;
        [SerializeField] private Image progressBar;
        [SerializeField] private Image healthBar;

        public void SetToolTip(string message)
        {
            toolTipText.text = message;
        }

        public void ResetUI()
        {
            toolTipText.text = "";
            progressBar.fillAmount = 0;
            healthBar.fillAmount = 0;
            healthBar.gameObject.SetActive(false); 
        }

        public void UpdateProgressBar(float value)
        {
            progressBar.fillAmount = value;
        }

        public void UpdateHealthBar(float value)
        {
            healthBar.gameObject.SetActive(true); 
            healthBar.fillAmount = value; 
        }
    }
}
