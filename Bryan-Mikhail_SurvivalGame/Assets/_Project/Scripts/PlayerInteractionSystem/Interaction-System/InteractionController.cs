using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class InteractionController : MonoBehaviour
    {
        [Header("Data")]
        public InteractionInputData interactionInputData;
        public InteractionData interactionData;

        [Space, Header("UI")]
        [SerializeField] private InteractionUIPanel uiPanel;

        [Space]
        [Header("Ray Settings")]
        public float rayDistance;
        public float raySphereRadius;
        public LayerMask interactableLayer;

        private Camera m_cam;
        private bool m_interacting;
        private float m_holdTimer = 0f;

        private void Awake()
        {
            m_cam = FindObjectOfType<Camera>();
        }

        private void Update()
        {
            CheckForInteractable();
            CheckForInteractableInput();
            CheckForHitInput(); // Check for hit input
        }

        void CheckForInteractable()
        {
            Ray _ray = new Ray(m_cam.transform.position, m_cam.transform.forward);
            RaycastHit _hitInfo;

            bool _hitSomething = Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance, interactableLayer);

            if (_hitSomething)
            {
                InteractableBase _interactable = _hitInfo.transform.GetComponent<InteractableBase>();
                if (_interactable != null)
                {
                    if (interactionData.IsEmpty())
                    {
                        interactionData.Interactable = _interactable;
                        uiPanel.SetToolTip(_interactable.ToolTipMessage);
                    }
                    else
                    {
                        if (!interactionData.IsSameInteractable(_interactable))
                        {
                            ResetInteractionState();
                            interactionData.Interactable = _interactable;
                            uiPanel.SetToolTip(_interactable.ToolTipMessage);
                        }
                    }
                }
            }
            else
            {
                if (!interactionData.IsEmpty())
                {
                    ResetInteractionState();
                }
                uiPanel.ResetUI();
                interactionData.ResetData();
            }
            Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);
        }

        void ResetInteractionState()
        {
            m_interacting = false;
            m_holdTimer = 0f;
            uiPanel.UpdateProgressBar(0);
            uiPanel.ResetUI();
        }

        void CheckForInteractableInput()
        {
            if (interactionData.IsEmpty())
                return;

            if (interactionInputData.InteractedClicked)
            {
                m_interacting = true;
                m_holdTimer = 0f;
            }

            if (interactionInputData.InteractedReleased)
            {
                m_interacting = false;
                m_holdTimer = 0f;
                uiPanel.UpdateProgressBar(0);
            }

            if (m_interacting)
            {
                if (!interactionData.Interactable.IsInteractable)
                    return;

                if (interactionData.Interactable.HoldInteract)
                {
                    m_holdTimer += Time.deltaTime;

                    float heldPercent = m_holdTimer / interactionData.Interactable.HoldDuration;
                    uiPanel.UpdateProgressBar(heldPercent);

                    if (heldPercent > 1f)
                    {
                        interactionData.Interact();
                        m_interacting = false;
                    }
                }
                else
                {
                    interactionData.Interact();
                    m_interacting = false;
                }

                if (interactionData.Interactable is Resource resource)
                {
                    uiPanel.UpdateHealthBar(resource.GetHealthPercent());
                }
            }
        }

        void CheckForHitInput()
        {
            if (Input.GetMouseButtonDown(0)) // Check if Mouse0 is pressed
            {
                Ray _ray = new Ray(m_cam.transform.position, m_cam.transform.forward);
                RaycastHit _hitInfo;

                if (Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance, interactableLayer))
                {
                    Resource resource = _hitInfo.transform.GetComponent<Resource>();
                    if (resource != null)
                    {
                        resource.ApplyDamage(10f); // Example damage value, adjust as needed
                        uiPanel.UpdateHealthBar(resource.GetHealthPercent());
                        Debug.Log("Hit " + resource.gameObject.name + ". Remaining health: " + resource.GetHealthPercent() * 100 + "%");
                    }
                }
            }
        }
    }
}
