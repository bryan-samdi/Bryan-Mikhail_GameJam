using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    public Animator anim;
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            Attack();
        }
    }

    private void Attack()
    {
        audioManager.PlaySFX(audioManager.playerhitSound);

        anim.SetTrigger("PlayerAttack");
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Animal")) 
                {
                    HealthSystem healthSystem = hitCollider.GetComponent<HealthSystem>();
                    if (healthSystem != null)
                    {
                        healthSystem.TakeDamage(attackDamage);
                    }
                }
            }
         
    }
}
