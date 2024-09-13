using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShot : MonoBehaviour
{
    [SerializeField]
    private GameObject _plasmaPrefab; // Plasma projectile prefab
    [SerializeField]
    private float _plasmaSpeed = 8f; // Speed of plasma shot
    [SerializeField]
    private float _fireRate = 0.5f; // Rate of fire
    [SerializeField]
    private float _canFire = -1f; // Time when player can fire again


    // Start is called before the first frame update
    void Start()
    {
        //speed of plasma shot

    }

    // Update is called once per frame
    void Update()
    {
        //translate laser right (forward)
        transform.Translate(Vector3.right * _plasmaSpeed * Time.deltaTime);
        Destroy(this.gameObject, 0.5f);

    }

    

}
