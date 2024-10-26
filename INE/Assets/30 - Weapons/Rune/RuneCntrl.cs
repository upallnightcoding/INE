using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCntrl : MonoBehaviour
{
    [SerializeField] private WeaponSO weapon;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            EventManager.Instance.InvokeOnRuneTrigger(weapon);
        }
    }
}
