using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TreeManager : MonoBehaviour
{
    public GameObject[] treePrefabs; // Assign the tree prefabs with HealthSystem in the inspector
    public Terrain terrain;
    public TreeInstance[] originalTrees;

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

        originalTrees = terrain.terrainData.treeInstances;
        ReplaceTerrainTrees();
    }

    void OnDisable()
    {
        // Restore the original trees when the game stops
        if (terrain != null)
        {
            terrain.terrainData.treeInstances = originalTrees;
        }
    }

    void ReplaceTerrainTrees()
    {
        // Create a parent GameObject for the trees
        GameObject treesParent = new GameObject("Spawned-Trees");

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

            // Instantiate the tree as a child of the Trees parent GameObject
            GameObject treeObject = Instantiate(treePrefab, worldPosition, Quaternion.identity, treesParent.transform);

            // Apply rotation from the TreeInstance to the instantiated tree
            treeObject.transform.rotation = Quaternion.AngleAxis(tree.rotation * Mathf.Rad2Deg, Vector3.up);
            treeObject.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);

            // Add the instantiated tree to the list
            treeObjects.Add(treeObject);
        }

        // Clear terrain trees
        terrain.terrainData.treeInstances = new TreeInstance[0];
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public static class PlayModeStateChanged
{
    static PlayModeStateChanged()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            // Restore trees when exiting play mode
            TreeManager[] managers = Object.FindObjectsOfType<TreeManager>();
            foreach (var manager in managers)
            {
                if (manager.terrain != null)
                {
                    manager.terrain.terrainData.treeInstances = manager.originalTrees;
                }
            }
        }
    }
}
#endif
