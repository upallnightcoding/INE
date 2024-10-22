using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private WeaponSO weapon;

    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += transform.up * gameData.weaponsSpinning * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            Debug.Log("You have triggered player ...");
        }
    }
}
