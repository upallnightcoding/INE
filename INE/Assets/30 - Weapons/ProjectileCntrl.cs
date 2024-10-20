using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCntrl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TakeDamageCntrl>(out TakeDamageCntrl takeDamage))
        {
            if (takeDamage.TakeDamage(10.0f))
            {
                Destroy(other.gameObject);
            }

            Destroy(gameObject);
        }
    }
}
