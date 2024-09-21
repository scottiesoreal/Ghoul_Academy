using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehavior : MonoBehaviour
{
    [SerializeField]  
    private GameObject _exorcistPrefab;  // Reference to the exorcist prefab

    //[SerializeField]  
    public Transform _churchSpawnPoint; // Reference to the church spawn point

    private GameObject currentExorcist;

    public void SpawnExorcist()
    {
        if (currentExorcist == null)
        {
            currentExorcist = Instantiate(_exorcistPrefab, _churchSpawnPoint.position, _churchSpawnPoint.rotation);            
            Debug.Log("Exorcist spawns!");
        }
    }

    public void DespawnExorcist()
    {
        if (currentExorcist != null)
        {
            Destroy(currentExorcist);
            currentExorcist = null;
            Debug.Log("Exorcist despawns!");
        }
    }
}
