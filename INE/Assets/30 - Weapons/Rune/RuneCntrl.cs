using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private WeaponSO weapon;

    private bool triggered = false;
    private float spinning;

    // Start is called before the first frame update
    void Start()
    {
        spinning = gameData.weaponsSpinning;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += transform.up * spinning * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            spinning = 0.0f;
            Debug.Log("You have triggered player ...");
            EventManager.Instance.InvokeOnRuneTrigger(weapon);
        }
    }
}
