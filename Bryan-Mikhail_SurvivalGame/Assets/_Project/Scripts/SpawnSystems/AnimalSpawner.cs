using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [System.Serializable]
    public class AnimalSpawnInfo
    {
        public GameObject animalPrefab;
        public float spawnRate; // Spawn rate in seconds
        [Range(0, 1)]
        public float spawnChance; // Probability of spawning
    }

    public List<AnimalSpawnInfo> animalList = new List<AnimalSpawnInfo>();
    public Transform[] spawnPoints;
    public float minSpawnDistance = 5f; // Minimum distance between animal spawns

    private bool isDayTime = false;
    private float spawnInterval = 180f; // Time interval between spawn cycles
    private Dictionary<Transform, float> spawnTimers = new Dictionary<Transform, float>();

    public void StartDayTime()
    {
        isDayTime = true;
        StartCoroutine(SpawnAnimals());
    }

    public void EndDayTime()
    {
        isDayTime = false;
        StopCoroutine(SpawnAnimals());
    }

    private IEnumerator SpawnAnimals()
    {
        InitializeSpawnTimers();

        while (isDayTime)
        {
            foreach (var animalInfo in animalList)
            {
                foreach (Transform spawnPoint in spawnPoints)
                {
                    if (CanSpawnAnimal(animalInfo.spawnChance) && CanSpawnAtPoint(spawnPoint))
                    {
                        SpawnAnimal(animalInfo.animalPrefab, spawnPoint);
                        yield return new WaitForSeconds(animalInfo.spawnRate);
                    }
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void InitializeSpawnTimers()
    {
        spawnTimers.Clear();
        foreach (Transform spawnPoint in spawnPoints)
        {
            spawnTimers.Add(spawnPoint, 0f);
        }
    }

    private bool CanSpawnAnimal(float spawnChance)
    {
        return Random.value <= spawnChance;
    }

    private bool CanSpawnAtPoint(Transform spawnPoint)
    {
        foreach (var otherSpawnPoint in spawnPoints)
        {
            if (spawnPoint != otherSpawnPoint && Vector3.Distance(spawnPoint.position, otherSpawnPoint.position) < minSpawnDistance)
            {
                return false;
            }
        }
        return Time.time >= spawnTimers[spawnPoint];
    }

    private void SpawnAnimal(GameObject animalPrefab, Transform spawnPoint)
    {
        Instantiate(animalPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnTimers[spawnPoint] = Time.time + spawnInterval;
    }
}
