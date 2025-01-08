using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookHealth : MonoBehaviour
{
    [SerializeField]
    private float spookHealth = 100f; // Spook health value
    [SerializeField]
    private float maxSpookHealth = 100f; // Maximum spook health value
    [SerializeField]
    private float scaredThreshold = 0f; // Threshold before NPC runs out of building

    private HumanNPCState humanNPCState; // Reference to the HumanNPCState component

    // Start is called before the first frame update
    void Start()
    {
        // Find the HumanNPCState component on the same GameObject
        humanNPCState = GetComponent<HumanNPCState>();
        if (humanNPCState == null)
        {
            Debug.LogError("HumanNPCState script is missing from this GameObject!");
        }
        else
        {
            humanNPCState.UpdateState((int)spookHealth); // Update the NPC state based on initial spook health
        }

       
    }

    public void TakeSpookDamage(float damage)
    {
        spookHealth = Mathf.Clamp(spookHealth - damage, scaredThreshold, maxSpookHealth);
        Debug.Log($"Spookhealth: {spookHealth}"); // Log the updated spook health value

        if (humanNPCState != null)
        {
            humanNPCState.UpdateState((int)spookHealth); // Update the NPC state
        }
    }

    public void ResetSpookhealth()
    {
        spookHealth = maxSpookHealth;
        Debug.Log("Spook health reset to max.");

        if (humanNPCState != null)
        {
            humanNPCState.UpdateState((int)spookHealth); // Reset the NPC state
        }
    }

    public float GetCurrentHealth()
    {
        return spookHealth;
    }


    // Update is called once per frame
    void Update()
    {
        // Intentionally left empty for now
    }
}
