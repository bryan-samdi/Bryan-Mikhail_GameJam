using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public float spawnRate; // Spawn rate in seconds
        [Range(0, 1)]
        public float spawnChance; // Probability of spawning
    }

    public List<EnemySpawnInfo> enemyList = new List<EnemySpawnInfo>();
    public Transform[] spawnPoints;
    public float minSpawnDistance = 5f; // Minimum distance between enemy spawns

    private bool isNightTime = false;
    private float spawnInterval = 180f; // Time interval between spawn cycles
    private Dictionary<Transform, float> spawnTimers = new Dictionary<Transform, float>();

    public void StartNightTime()
    {
        isNightTime = true;
        StartCoroutine(SpawnEnemies());
    }

    public void EndNightTime()
    {
        isNightTime = false;
        StopCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        InitializeSpawnTimers();

        while (isNightTime)
        {
            foreach (var spawnInfo in enemyList)
            {
                foreach (Transform spawnPoint in spawnPoints)
                {
                    if (CanSpawnEnemy(spawnInfo.spawnChance) && CanSpawnAtPoint(spawnPoint))
                    {
                        SpawnEnemy(spawnInfo.enemyPrefab, spawnPoint);
                        yield return new WaitForSeconds(spawnInfo.spawnRate);
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

    private bool CanSpawnEnemy(float spawnChance)
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

    private void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        GameObject enemiesParent = GameObject.Find("Spawned-NightTime-Enemies");
        if (enemiesParent == null)
        {
            enemiesParent = new GameObject("Spawned-NightTime-Enemies");
        }

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, enemiesParent.transform);
        spawnTimers[spawnPoint] = Time.time + spawnInterval;
    }
}
