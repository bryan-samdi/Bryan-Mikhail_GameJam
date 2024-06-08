using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public GameObject[] treePrefabs; // Assign the tree prefabs with HealthSystem in the inspector
    private Terrain terrain;

    void Start()
    {
        // Get the Terrain component from the current game object or find it in the scene
        terrain = GetComponent<Terrain>();

        if (terrain == null)
        {
            terrain = FindObjectOfType<Terrain>();
            if (terrain == null)
            {
                Debug.LogError("No Terrain component found. Please ensure there is a Terrain in the scene.");
                return;
            }
        }

        ReplaceTerrainTrees();
    }

    void ReplaceTerrainTrees()
    {
        TreeInstance[] trees = terrain.terrainData.treeInstances;
        List<GameObject> treeObjects = new List<GameObject>();

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
            GameObject treeObject = Instantiate(treePrefab, worldPosition, Quaternion.identity);
            treeObjects.Add(treeObject);
        }

        //terrain.terrainData.treeInstances = new TreeInstance[0]; // Clear terrain trees
    }
}
