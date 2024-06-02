using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class SpawnObjectButton : InteractableBase
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform spawnPoint;
        public override void OnInteract()
        {
            base.OnInteract();

            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
