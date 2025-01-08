using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sample;
using UnityEngine.Serialization;

public class NPCVision : MonoBehaviour
{
    [SerializeField]
    private Transform _player;  // Reference to the player (ghost)
    [SerializeField]
    private float _visionDistance = 10f;  // How far the NPC can see
    [SerializeField]
    private float _visionAngle = 120f;  // NPC's field of view
    [SerializeField]
    private float _spookDamageCoolDown = 3f;

    private float _nextSpookDamageTime = 0f; // Time to apply spook damage
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
        // Calculate the direction to the player
        Vector3 directionToPlayer = _player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Get the angle between the NPC's forward direction and the direction to the player
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Only consider objects in the "Ghost" layer
        int layerMask = LayerMask.GetMask("Ghost");

        // Visualize the NPC's forward direction
        Debug.DrawRay(transform.position + Vector3.up * 1.0f, transform.forward * _visionDistance, Color.blue); // NPC's forward view
        Debug.DrawRay(transform.position + Vector3.up * 1.0f, directionToPlayer.normalized * _visionDistance, Color.red); // Toward ghost (optional)

        // Check if the player is within the NPC's field of view
        if (angleToPlayer < _visionAngle / 2 && distanceToPlayer <= _visionDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 1.0f, directionToPlayer.normalized, out hit, _visionDistance, layerMask))
            {
                Debug.Log("Raycast hit object: " + hit.collider.name);

                if (hit.collider.CompareTag("Player")) // Assuming the ghost has the "Player" tag
                {
                    // Get the GhostScript component to check visibility
                    GhostScript ghostScript = _player.GetComponent<GhostScript>();

                    if (ghostScript != null && ghostScript.IsVisible()) // Ensure the ghost is visible
                    {
                        Debug.Log("Ghost is visible and within field of view.");

                        // Handle spook health decrement
                        SpookHealth spookHealth = GetComponent<SpookHealth>();
                        if (spookHealth != null)
                        {
                            spookHealth.TakeSpookDamage(1f); // Apply damage
                            _nextSpookDamageTime = Time.time + _spookDamageCoolDown; // Set next allowed damage time
                            Debug.Log($"Spook damage applied. Next damage allowed after: {_nextSpookDamageTime}");


                            // Trigger fleeing only at zero health
                            if (spookHealth.GetCurrentHealth() <= 0)
                            {
                                Debug.Log("NPC is terrified and will flee!");
                                _npcMovement.RunToExit(); // Trigger the NPC's fleeing behavior
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Ghost is invisible, NPC cannot see the ghost.");
                    }
                }
                else
                {
                    Debug.Log("Raycast hit something else, NPC cannot see the ghost.");
                }
            }
        }
        else
        {
            Debug.Log("Player is outside the NPC's field of view.");
        }
    }



    public bool CanSeePlayer()
    {
        return _canSeePlayer;
    }
}
