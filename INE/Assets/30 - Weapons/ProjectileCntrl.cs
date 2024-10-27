using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCntrl : MonoBehaviour
{
    // Amount of damage created by the projectile
    private int damage = 0;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyCntrl>(out EnemyCntrl enemyCntrl))
        {
            TakeDamageCntrl takeDamageCntrl = other.GetComponent<TakeDamageCntrl>();

            if (takeDamageCntrl.TakeDamage(damage))
            {
                Destroy(other.gameObject);

                EventManager.Instance.InvokeOnEnemyDestroy();
                EventManager.Instance.InvokeOnDisplayXpKills(enemyCntrl.GetXp());
            }
        }


        Destroy(gameObject);
    }
}
