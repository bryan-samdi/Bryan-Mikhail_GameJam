using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Rigidbody rb;
    PlayerMovement playerMovement;
    public GameObject currentHandObject;
    private Animator currentHandAnimator;
    AudioManager audioManager;


    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        if (currentHandObject != null && currentHandAnimator != null)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            bool isWalking = targetVelocity.magnitude > 0;

            currentHandAnimator.SetBool("isWalking", isWalking);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentHandAnimator.SetTrigger("PlayerAttack");
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                currentHandAnimator.SetTrigger("PlayerInspect");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                currentHandAnimator.SetTrigger("PlayerReload");
            }
        }
    }

    public void EquipWeapon(GameObject newHandObject)
    {
        if (currentHandObject == newHandObject)
        {
            return;
        }

        if (currentHandObject != null)
        {
            currentHandObject.SetActive(false); 
        }

        currentHandObject = newHandObject;
        currentHandObject.SetActive(true);
        currentHandAnimator = currentHandObject.GetComponent<Animator>();
    }
}
