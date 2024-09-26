using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVision : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private float _visionDistance = 10f;
    [SerializeField]
    private float _visionAngle = 120f;

    private NPCMovement _npcMovement;
    private bool _canSeePlayer = false;

    //spook state & level
    [SerializeField]
    private float _spookLevel = 0f;//starting spook level is in calm state
    [SerializeField]
    private float _maxSpookLevel = 100f;//max spook level that causes NPC to run
    [SerializeField]
    private float _spookIncreaseRate = 10f;//increase rate when scaring NPCs
    [SerializeField]
    private bool _isSpooked = false;//spook level is activated
    [SerializeField]
    private float _lastSpookedTime; //last time NPC was spooked
    [SerializeField]
    private float _calmRegenRate = 5f;//decrease rate when not scaring NPCs

    //Calm state
    [SerializeField]
    private bool _isCalm = true;//default NPC state
    


    void Start()
    {
        // Assign player object automatically; no need for manual inspector assignment
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player").transform;
        }

        // Cache the NPCMovement script
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

        if (distanceToPlayer <= _visionDistance)
        {
            Vector3 directionToPlayerNormalized = directionToPlayer.normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayerNormalized);

            if (angleToPlayer <= _visionAngle / 2f)
            {
                // Check if the player is invisible
                PlayerScript playerScript = _player.GetComponent<PlayerScript>();
                if (playerScript != null && playerScript.IsPlayerInvisible())
                {
                    _canSeePlayer = false;  // Player is invisible, can't be seen
                    Debug.Log("Player is invisible");
                    return;
                }

                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayerNormalized, out hit, distanceToPlayer))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (!_canSeePlayer)
                        {
                            _npcMovement.StartleJump();
                            Debug.Log("NPC was startled");
                            _npcMovement.RunToExit();  // Make the NPC run away after being startled
                            Debug.Log("NPC is running away.");
                        }
                        _canSeePlayer = true;
                        Debug.Log("NPC can see the player.");

                        //Increase spook level
                        _isCalm = false;
                        _isSpooked = true;
                        Debug.Log("NPC is spooked");
                        _lastSpookedTime = Time.time; // track the last time NPC was spooked
                        _spookLevel += _spookIncreaseRate * Time.deltaTime;

                        //spook level clamp
                        if (_spookLevel >= _maxSpookLevel)
                        {
                            _spookLevel = _maxSpookLevel;
                            Debug.Log("NPC has reached maximum spook level!");

                            // Run to the church
                            _npcMovement.RunToChurch();
                            Debug.Log("NPC maxed spook, running to church...");
                        }

                    }
                    else
                    {
                        _canSeePlayer = false;
                    }
                }
            }
            else
            {
                _canSeePlayer = false;
            }
        }
        else
        {
            _canSeePlayer = false;
        }
    }

    public bool CanSeePlayer()
    {
        return _canSeePlayer;
    }
}
