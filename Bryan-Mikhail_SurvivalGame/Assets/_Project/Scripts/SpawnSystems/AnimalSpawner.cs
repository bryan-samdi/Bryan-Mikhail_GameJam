using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;

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

    private AzureTimeController timeController; // Reference to the time controller script

    private void Start()
    {
        // Find the AzureTimeController script in the scene
        timeController = FindObjectOfType<AzureTimeController>();

        // Subscribe to the day and night events
        timeController.m_onHourChange.AddListener(OnHourChanged);
    }

    // Method to handle changes in the hour (time)
    private void OnHourChanged()
    {
        // Check if it's daytime (6 am to 6 pm)
        if (timeController.m_hour >= 6 && timeController.m_hour < 18)
        {
            // Start spawning animals during the day
            StartDayTime();
        }
        else
        {
            // End spawning animals during the night
            EndDayTime();
        }
    }

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
        // Create a parent GameObject for the spawned animals
        GameObject animalsParent = GameObject.Find("Spawned Animals");
        if (animalsParent == null)
        {
            animalsParent = new GameObject("Spawned Animals");
        }

        // Spawn the animal as a child of the animalsParent GameObject
        GameObject spawnedAnimal = Instantiate(animalPrefab, spawnPoint.position, spawnPoint.rotation, animalsParent.transform);


        // Update the spawn timer for this spawn point
        spawnTimers[spawnPoint] = Time.time + spawnInterval;
    }
}
