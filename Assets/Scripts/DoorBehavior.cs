using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public GameObject door;
    [SerializeField]
    private float openRot;
    [SerializeField]
    private float closeRot;
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool opening;

    private void Update()
    {
        Vector3 currentRot = door.transform.localEulerAngles;
        if (opening)
        {
            // Open the door by rotating towards openRot on the y-axis
            if (currentRot.y < openRot)
            {
                door.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, openRot, currentRot.z), speed * Time.deltaTime);
            }
        }
        else
        {
            // Close the door by rotating towards closeRot on the y-axis
            if (currentRot.y > closeRot)
            {
                door.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, closeRot, currentRot.z), speed * Time.deltaTime);
            }
        }
    }
}
