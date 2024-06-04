using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player Movement Variables
    public float moveSpeed = 10f;
    public float jumpForce = 5f;
    public float sprintMultiplier = 2f;

    public float interactRange = 2f;

    //Player Camera Sense
    public float lookSensitivity = 2f;
    private float rotationX = 0;
    public float lookSmoothTime = 0.1f;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;

    public Transform playerCam;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;

    }

    private void Update()
    {
        LookAround();
        Move();
        Jump();

    }

    void LookAround()
    {
        //float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        //float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;
        //transform.Rotate(0, mouseX, 0);

        //rotationX -= mouseY;
        //rotationX = Mathf.Clamp(rotationX, -70f, 30f);
        //playerCam.localRotation = Quaternion.Euler(rotationX, 0, 0);

        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        Vector3 targetMouseDelta = new Vector3(mouseX, mouseY, 0); //creates a vector representing the target mouse delta - change in mouse position
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, lookSmoothTime); // smooth damp the mouse delta to avoid sudden jumps

        transform.Rotate(0, currentMouseDelta.x, 0); //rotate the player body horizontally based on the smooth mouse delta

        rotationX -= currentMouseDelta.y; //update the vertical rotation value based on the smooth mouse delta
        rotationX = Mathf.Clamp(rotationX, -70f, 60f);
        playerCam.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }



    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        if(Input.GetKey(KeyCode.LeftShift))
        {
            movement *= sprintMultiplier;
        }

        rb.MovePosition(transform.position + movement);

        //if(Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") && Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    movement = movement.normalized * moveSpeed  * Time.deltaTime * 2;
        //}

      
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }



}

   

   






