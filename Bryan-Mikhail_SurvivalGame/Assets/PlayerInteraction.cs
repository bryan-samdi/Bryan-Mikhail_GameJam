using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public int damageAmount = 10;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Resource resource = hit.collider.GetComponent<Resource>();
                if (resource != null)
                {
                    resource.TakeDamage(damageAmount);
                }
            }
        }
    }
}
