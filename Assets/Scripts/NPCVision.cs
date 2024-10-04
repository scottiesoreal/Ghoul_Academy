using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sample;

public class NPCVision : MonoBehaviour
{
    [SerializeField]
    private Transform _player;  // Reference to the player (ghost)
    [SerializeField]
    private float _visionDistance = 10f;  // How far the NPC can see
    [SerializeField]
    private float _visionAngle = 120f;  // NPC's field of view

    private NPCMovement _npcMovement;  // Reference to NPCMovement script
    private bool _canSeePlayer = false;  // Tracks if the NPC can see the player

    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player").transform;  // Auto-assign player if not set
        }

        _npcMovement = GetComponent<NPCMovement>();
        if (_npcMovement == null)
        {
            Debug.LogError("NPCMovement script is missing!");
        }
    }

    void Update()
    {
        CheckPlayerVisibility();
    }

    private void CheckPlayerVisibility()
    {
        Vector3 directionToPlayer = _player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Visualize the ray
        Debug.DrawRay(transform.position, directionToPlayer.normalized * _visionDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, distanceToPlayer))
        {
            Debug.Log("Raycast hit object: " + hit.collider.name);

            if (hit.collider.CompareTag("Player"))
            {
                // Get the GhostScript component to check visibility
                GhostScript ghostScript = _player.GetComponent<GhostScript>();

                // Use the existing IsPlayerInvisible() method to check if the ghost is invisible
                if (ghostScript != null && ghostScript.IsVisible())  // Ensure the ghost is visible
                {
                    Debug.Log("Ghost is visible.");

                    if (!_canSeePlayer)
                    {
                        _npcMovement.StartleJump();
                        Debug.Log("NPC was startled by the visible ghost and is running to the exit.");
                        _npcMovement.RunToExit();
                    }

                    _canSeePlayer = true;
                    Debug.Log("NPC can see the ghost.");
                }
                else
                {
                    _canSeePlayer = false;
                    Debug.Log("Ghost is invisible, NPC cannot see the ghost.");
                }
            }
            else
            {
                _canSeePlayer = false;
                Debug.Log("Raycast hit something else, NPC cannot see the ghost.");
            }
        }
        else
        {
            _canSeePlayer = false;
            Debug.Log("Raycast did not hit any object.");
        }
    }

    public bool CanSeePlayer()
    {
        return _canSeePlayer;
    }
}
