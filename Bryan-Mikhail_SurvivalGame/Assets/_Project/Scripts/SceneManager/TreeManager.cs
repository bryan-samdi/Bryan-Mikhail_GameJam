using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public GameObject[] treePrefabs; // Assign the tree prefabs with HealthSystem in the inspector
    private Terrain terrain;

    private List<TreeInstance> originalTrees = new List<TreeInstance>();

    void Start()
    {
        terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            terrain = FindObjectOfType<Terrain>();
        }

        if (terrain == null)
        {
            Debug.LogError("No Terrain component found in the scene.");
            return;
        }

        ReplaceTerrainTrees();
    }

    void OnDisable()
    {
        // Restore the original trees when the game stops
        terrain.terrainData.treeInstances = originalTrees.ToArray();
    }

    void ReplaceTerrainTrees()
    {
        TreeInstance[] trees = terrain.terrainData.treeInstances;
        originalTrees.AddRange(trees); // Store original trees to restore later

        foreach (TreeInstance tree in trees)
        {
            // Get the corresponding prefab for this tree instance
            int treeIndex = tree.prototypeIndex;
            if (treeIndex < 0 || treeIndex >= treePrefabs.Length)
            {
                Debug.LogWarning("Tree index out of range. Skipping this tree.");
                continue;
            }

            GameObject treePrefab = treePrefabs[treeIndex];
            Vector3 worldPosition = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;
            Instantiate(treePrefab, worldPosition, Quaternion.identity);
        }

        // Make terrain trees invisible
        TreeInstance[] emptyTreeArray = new TreeInstance[trees.Length];
        for (int i = 0; i < trees.Length; i++)
        {
            emptyTreeArray[i] = trees[i];
            emptyTreeArray[i].color = new Color32(0, 0, 0, 0); // Set trees to be invisible
        }
        terrain.terrainData.treeInstances = emptyTreeArray;
    }
}
