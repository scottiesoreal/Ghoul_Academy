using AOT;
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

    //transparency check
    //[SerializeField]
    //private bool _isTransparent = false;

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

    // Detect when the Exorcist or NPC enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone and make walls transparent
        if (other.CompareTag("Player"))        {
            
            MakeWallsTransparent(); // Existing functionality for player
        }

        // Handle Exorcist entering the house
        if (other.CompareTag("ExorcistNPC"))
        {
            // Exorcist enters the house and starts searching rooms
            ExorcistNPC exorcist = other.GetComponent<ExorcistNPC>();//
            if (exorcist != null)
            {
                exorcist.StartRoomSearch(); // Call the method to begin room search
                Debug.Log("Exorcist entered the house, starting room search...");
            }
        }

        // Handle NPCs exiting the house and running to the church
        if (other.CompareTag("HumanNPC"))
        {
            // Get the NPC's vision script
            NPCVision npcVision = other.GetComponent<NPCVision>();

            // Only run to church if the NPC can see the player/ghost
            if (npcVision != null && npcVision.CanSeePlayer())
            {
                NPCMovement npc = other.GetComponent<NPCMovement>();
                if (npc != null)
                {
                    npc.RunToChurch();  // Only run to the church if ghost is visible
                    Debug.Log("NPC saw the ghost, running to church...");
                }
            }
            else
            {
                Debug.Log("NPC can't see the ghost, staying in place...");
            }
        }
    }

    // Detect when the Player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Revert all walls to opaque when player exits
            MakeWallsOpaque();
        }
    }
}
