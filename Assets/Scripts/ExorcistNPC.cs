using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExorcistNPC : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    // Reference to the church position
    [SerializeField] private Transform _church;

    // Reference to the entrance of the house
    [SerializeField] private Transform _entrance;

    // List to hold all unvisited room colliders
    private List<Collider> _unvisitedRooms = new List<Collider>();

    // Flag to track if Exorcist has entered the house
    [SerializeField]
    private bool _hasEnteredHouse = false;

    void Start()
    {
        // Cache the NavMeshAgent component
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // Start by moving to the house entrance
        MoveToEntrance();
    }

    void MoveToEntrance()
    {
        _navMeshAgent.SetDestination(_entrance.position);
        Debug.Log("Moving to the house entrance...");
    }

    public void StartRoomSearch()
    {
        // Find all rooms tagged with "Room" for searching
        Collider[] rooms = FindObjectsOfType<Collider>();
        foreach (Collider room in rooms)
        {
            if (room.CompareTag("Room"))
            {
                _unvisitedRooms.Add(room);
            }
        }

        // Start moving toward the first unvisited room
        MoveToNextRoom();
    }

    void MoveToNextRoom()
    {
        if (_unvisitedRooms.Count > 0)
        {
            Collider nearestRoom = FindNearestRoom();
            if (nearestRoom != null)
            {
                Debug.Log("Moving to next room: " + nearestRoom.name);
                _navMeshAgent.SetDestination(nearestRoom.transform.position);
            }
            else
            {
                Debug.Log("No valid room found to move to.");
            }
        }
        else
        {
            Debug.Log("All rooms visited. Moving to exit...");
            MoveToExit();
        }
    }

    void MoveToExit()
    {
        // Move back to the entrance after searching all rooms
        _navMeshAgent.SetDestination(_entrance.position);
        Debug.Log("All rooms checked. Returning to the entrance...");
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
        // Handle entrance to the house
        if (other.CompareTag("Exit") && !_hasEnteredHouse)
        {
            _hasEnteredHouse = true;
            Debug.Log("Exorcist entered the house. Starting room search...");
            StartRoomSearch(); // Begin searching rooms after entering
        }

        // Handle exiting the house after all rooms have been searched
        else if (other.CompareTag("Exit") && _hasEnteredHouse)
        {
            Debug.Log("Exorcist exited the house. Returning to the church.");
            _navMeshAgent.SetDestination(_church.position); // Move to the church after exiting
        }

        // Handle returning to the church
        else if (other.CompareTag("Church"))
        {
            Debug.Log("Exorcist returned to the church. Despawning...");
            Destroy(gameObject); // Despawn the Exorcist NPC
        }

        // Handle entering a room for the search
        else if (other.CompareTag("Room"))
        {
            Debug.Log("Entered room: " + other.name);
            _unvisitedRooms.Remove(other); // Mark room as visited
            Debug.Log("Remaining rooms to visit: " + _unvisitedRooms.Count);
            MoveToNextRoom(); // Move to the next unvisited room
        }
    }
}
