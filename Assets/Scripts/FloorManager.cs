using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] GameObject[] floorPrefabs;

    public void SpawnFloor()
    {
        // Randomly select a floor prefab
        int randomIndex = Random.Range(0, floorPrefabs.Length);

        // Spawn the floor
        GameObject floor = Instantiate(floorPrefabs[randomIndex], transform);

        // Position the floor
        floor.transform.position = new Vector3(Random.Range(-3.8f, 3.8f), -6f, 0f);
    }
}
