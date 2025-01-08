using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanNPCState : MonoBehaviour
{
    public enum HumanState 
    {
        Calm,
        Alert,
        Unsettled,
        Spooked,
        Scared,
        Fleeing
    }

    public HumanState currentState;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the NPC state to Calm
        currentState = HumanState.Calm;
        Debug.Log("NPC state initialized to: " + currentState);

        // Test state transitions
        TestStateTransitions();
    }

    // Update the state based on external input (e.g., spook health)
    public void UpdateState(int spookHealth)
    {
        if (spookHealth == 100)
        {
            currentState = HumanState.Calm;
        }
        else if (spookHealth >= 75)
        {
            currentState = HumanState.Alert;
        }
        else if (spookHealth >= 50)
        {
            currentState = HumanState.Unsettled;
        }
        else if (spookHealth >= 25)
        {
            currentState = HumanState.Spooked;
        }
        else if (spookHealth >= 1)
        {
            currentState = HumanState.Scared;
        }
        else
        {
            currentState = HumanState.Fleeing;
        }

        Debug.Log("NPC state updated to: " + currentState);
    }

    void TestStateTransitions()
    {
        // Example test cases
        UpdateState(100);  // Should set state to Calm
        UpdateState(85);   // Should set state to Alert
        UpdateState(60);   // Should set state to Unsettled
        UpdateState(40);   // Should set state to Spooked
        UpdateState(10);   // Should set state to Scared
        UpdateState(0);    // Should set state to Fleeing
    }
}
