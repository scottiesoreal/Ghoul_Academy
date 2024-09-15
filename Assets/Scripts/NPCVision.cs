using UnityEngine;

public class NPCVision : MonoBehaviour
{
    [SerializeField]
    private Transform _player;         // Reference to the player
    [SerializeField]
    private float _visionDistance = 10.0f;  // How far the NPC can see
    [SerializeField]
    private float _visionAngle = 120.0f;    // Field of vision in degrees

    private bool _canSeePlayer = false;    // Tracks if the NPC can see the player
    private NPCMovement _npcMovement;      // Reference to the NPCMovement script

    void Start()
    {
        // Assign player object automatically; no need for manual inspector assignment
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player").transform;
        }
        Debug.Log("Attempting to retrieve NPCMvement component");
        // Cache the NPCMovement script
        _npcMovement = GetComponent<NPCMovement>();
        if (_npcMovement != null)
        {
            Debug.Log("NPCMovement component found successfully!");
        }
        else
        {
            Debug.LogError("NPCMovement script is missing!");
        }
    }

    void Update()
    {
        CheckPlayerVisibility();

        Debug.DrawRay(transform.position, transform.forward * _visionDistance, Color.red);
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
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayerNormalized, out hit, distanceToPlayer))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (!_canSeePlayer)
                        {
                            _npcMovement.StartleJump();
                            Debug.Log("NPC was startled");
                        }
                        _canSeePlayer = true;
                        Debug.Log("NPC can see the player.");
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

}
