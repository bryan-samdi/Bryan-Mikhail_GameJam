using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 5f;
    public float sprintMultiplier = 2f;
    public float interactRange = 2f;
    public float lookSensitivity = 2f;
    public float lookSmoothTime = 0.1f;
    public float attackRange = 2f;
    public float attackDamage = 10f;

    private float rotationX = 0;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;
    public Transform playerCam;
    Rigidbody rb;
    PlayerSurvivalStats playerSurvivalStats;

    public bool isRunning { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerSurvivalStats = GetComponent<PlayerSurvivalStats>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        LookAround();
        Move();
        Jump();
        Attack();
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        Vector3 targetMouseDelta = new Vector3(mouseX, mouseY, 0);
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, lookSmoothTime);

        transform.Rotate(0, currentMouseDelta.x, 0);

        rotationX -= currentMouseDelta.y;
        rotationX = Mathf.Clamp(rotationX, -70f, 60f);
        playerCam.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift) && playerSurvivalStats.currentStamina > playerSurvivalStats.staminaMinForRun)
        {
            movement *= sprintMultiplier;
            playerSurvivalStats.currentStamina -= playerSurvivalStats.staminaDecayRate * Time.deltaTime;
            isRunning = true;
        }
        else
        {
            playerSurvivalStats.currentStamina += playerSurvivalStats.staminaRecoveryRate * Time.deltaTime;
            isRunning = false;
        }

        playerSurvivalStats.currentStamina = Mathf.Clamp(playerSurvivalStats.currentStamina, 0, playerSurvivalStats.maxStamina);
        rb.MovePosition(transform.position + movement);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded() && playerSurvivalStats.currentStamina > playerSurvivalStats.staminaMinForRun)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerSurvivalStats.currentStamina -= playerSurvivalStats.staminaDecayRate;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Resource"))
                {
                    HealthSystem healthSystem = hitCollider.GetComponent<HealthSystem>();
                    if (healthSystem != null)
                    {
                        healthSystem.TakeDamage(attackDamage); // Apply damage to the object
                    }
                }
            }
        }
    }
}

   

   






