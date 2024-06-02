using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    PlayerMovement playerMovement;

  

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isWalking = targetVelocity.magnitude > 0;

        anim.SetBool("isWalking", isWalking);



        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("PlayerAttack2");
        }

        if (Input.GetKey(KeyCode.U))
        {
            anim.SetTrigger("UnequipKnife");
        } 

        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.SetTrigger("PlayerInspect");
        }
    }


}
