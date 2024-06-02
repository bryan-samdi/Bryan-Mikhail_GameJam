using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
//Variables
        [Header("Interactable Settings")]
        [SerializeField] private float holdDuration = 1f;

        [Space]
        [SerializeField] private bool holdInteract =  true;
        [SerializeField]  private bool multipleUse = false;
        [SerializeField]  private bool isInteractable = true;

        [SerializeField] public string toolTipMessage = "interact";



        //Properties
        public float HoldDuration => holdDuration;
        public bool HoldInteract => holdInteract;
        public bool MultipleUse => multipleUse;
        public bool IsInteractable => isInteractable;

        public string ToolTipMessage => toolTipMessage;

        //Methods
        public virtual void OnInteract()
        {
            Debug.Log("Interacted with " + gameObject.name);
        }
    }
}
