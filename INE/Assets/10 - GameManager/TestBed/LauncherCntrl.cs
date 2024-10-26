using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LauncherCntrl : MonoBehaviour
{
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            GameObject go = Instantiate(projectile, muzzlePoint.position, Quaternion.identity);
            go.GetComponentInChildren<Rigidbody>().AddForce(Vector3.forward * force, ForceMode.Impulse);
            Destroy(go, 2.0f);
        }
    }
}
