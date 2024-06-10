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
        public float spawnRate; 
        [Range(0, 1)]
        public float spawnChance;
    }

    public List<AnimalSpawnInfo> animalList = new List<AnimalSpawnInfo>();
    public Transform[] spawnPoints;
    public float minSpawnDistance = 5f;

    private bool isDayTime = false;
    private float spawnInterval = 180f; 
    private Dictionary<Transform, float> spawnTimers = new Dictionary<Transform, float>();

    private AzureTimeController timeController; 

    private void Start()
    {
        timeController = FindObjectOfType<AzureTimeController>();

        timeController.m_onHourChange.AddListener(OnHourChanged);
    }

    private void OnHourChanged()
    {
        if (timeController.m_hour >= 6 && timeController.m_hour < 18)
        {
            StartDayTime();
        }
        else
        {
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
        GameObject animalsParent = GameObject.Find("Spawned Animals");
        if (animalsParent == null)
        {
            animalsParent = new GameObject("Spawned Animals");
        }
        GameObject spawnedAnimal = Instantiate(animalPrefab, spawnPoint.position, spawnPoint.rotation, animalsParent.transform);
        spawnTimers[spawnPoint] = Time.time + spawnInterval;
    }
}
