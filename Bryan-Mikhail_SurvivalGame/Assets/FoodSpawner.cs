using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [System.Serializable]
    public class FoodSpawnInfo
    {
        public GameObject foodPrefab;
        [Range(0, 1)]
        public float spawnChance; // Probability of spawning
    }
    public float spawnChance;
    public List<FoodSpawnInfo> foodList = new List<FoodSpawnInfo>();
    public Transform[] spawnPoints;
    public float minSpawnDistance = 5f; // Minimum distance between food spawns

    private float spawnInterval = 180f; // Time interval between spawn cycles
    private Dictionary<Transform, float> spawnTimers = new Dictionary<Transform, float>();

    void Start()
    {
        InitializeSpawnTimers();
        StartCoroutine(SpawnFood());
    }

    private IEnumerator SpawnFood()
    {
        while (true)
        {
            foreach (var spawnInfo in foodList)
            {
                foreach (Transform spawnPoint in spawnPoints)
                {
                    if (CanSpawnFood(spawnInfo.spawnChance) && CanSpawnAtPoint(spawnPoint))
                    {
                        SpawnFoodItem(spawnInfo.foodPrefab, spawnPoint);
                        yield return new WaitForSeconds(spawnInfo.spawnChance);
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

    private bool CanSpawnFood(float spawnChance)
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

    private void SpawnFoodItem(GameObject foodPrefab, Transform spawnPoint)
    {
        GameObject foodParent = GameObject.Find("Spawned-Food");
        if (foodParent == null)
        {
            foodParent = new GameObject("Spawned-Food");
        }

        Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity, foodParent.transform);
        spawnTimers[spawnPoint] = Time.time + spawnInterval;
    }
}
