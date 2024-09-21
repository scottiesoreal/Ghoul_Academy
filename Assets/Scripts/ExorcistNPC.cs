using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExorcistNPC : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private bool hasEnteredHouse = false; // Flag to track if the Exorcist has entered the house

    // Reference to the church and entrance positions
    [SerializeField]
    private Transform _church;
    [SerializeField]
    private Transform _entrance;

    // List to hold all unvisited room colliders
    private List<Collider> _unvisitedRooms = new List<Collider>();

    void Start()
    {
        // Cache the NavMeshAgent component
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // Find all room colliders with the tag "Room" and add to the list
        Collider[] rooms = FindObjectsOfType<Collider>();
        foreach (Collider room in rooms)
        {
            if (room.CompareTag("Room"))
            {
                _unvisitedRooms.Add(room);
            }
        }

        // Start moving toward the nearest unvisited room
        MoveToNextRoom();
    }

    void MoveToNextRoom()
    {
        // If there are unvisited rooms, move to the nearest one
        if (_unvisitedRooms.Count > 0)
        {
            Collider nearestRoom = FindNearestRoom();
            _navMeshAgent.SetDestination(nearestRoom.transform.position);
        }
        else
        {
            // If all rooms have been visited, return to the church
            _navMeshAgent.SetDestination(_church.position);
            Debug.Log("All rooms checked. Heading back to the church.");
        }
    }

    Collider FindNearestRoom()
    {
        Collider nearestRoom = null;
        float shortestDistance = Mathf.Infinity;

        // Loop through all unvisited rooms to find the closest one
        foreach (Collider room in _unvisitedRooms)
        {
            float distance = Vector3.Distance(transform.position, room.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestRoom = room;
            }
        }

        return nearestRoom;
    }

    // Handle when the Exorcist enters a room or returns to the church
    private void OnTriggerEnter(Collider other)
    {
        // Check if the Exorcist entered a room
        if (other.CompareTag("Room"))
        {
            Debug.Log("Entered room: " + other.name);
            _unvisitedRooms.Remove(other);
            MoveToNextRoom(); // After searching this room, move to the next
        }

        // Check if the Exorcist has returned to the church
        if (other.CompareTag("Church"))
        {
            Debug.Log("Exorcist returned to the church. Despawning...");
            Destroy(gameObject); // Despawn the Exorcist
        }
    }
}
