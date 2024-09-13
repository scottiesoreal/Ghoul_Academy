using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    // List to store all the walls with the "VanishingWall" tag
    private List<GameObject> vanishingWalls = new List<GameObject>();

    // Transparent and Opaque materials (you will assign these in the Inspector)
    [SerializeField]
    private Material _transparentMaterial;  // Assign "Transparent_mat" in the Inspector
    [SerializeField]
    private Material _opaqueMaterial;       // Assign "HouseFloor_mat" in the Inspector

    void Start()
    {
        // Find all walls with the tag "VanishingWall" and store them in the list
        GameObject[] walls = GameObject.FindGameObjectsWithTag("VanishingWall");

        // Add them to the vanishingWalls list
        foreach (GameObject wall in walls)
        {
            vanishingWalls.Add(wall);
        }
    }

    // This function will make walls transparent
    private void MakeWallsTransparent()
    {
        foreach (GameObject wall in vanishingWalls)
        {
            // Change the material of the wall to the transparent material
            Renderer wallRenderer = wall.GetComponent<Renderer>();
            if (wallRenderer != null)
            {
                Debug.Log("Making wall transparent:" + wall.name);
                wallRenderer.material = _transparentMaterial;  // "Transparent_mat" in the Inspector
            }
        }
    }

    // This function will revert walls back to opaque
    private void MakeWallsOpaque()
    {
        foreach (GameObject wall in vanishingWalls)
        {
            // Change the material of the wall to the opaque material
            Renderer wallRenderer = wall.GetComponent<Renderer>();
            if (wallRenderer != null)
            {
                wallRenderer.material = _opaqueMaterial;  // "HouseFloor_mat" in the Inspector
            }
        }
    }

    // Detect when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Make all walls transparent
            Debug.Log("Player entered trigger zone");
            MakeWallsTransparent();
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Revert all walls to opaque
            
            MakeWallsOpaque();
        }
    }
}
