using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCntrl : MonoBehaviour
{
    // Amount of damage created by the projectile
    private int damage = 0;

    private GameObject destroyPrefab;

    public void SetDamage(int damage, GameObject destroyPrefab)
    {
        this.damage = damage;
        this.destroyPrefab = destroyPrefab;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyCntrl>(out EnemyCntrl enemyCntrl))
        {
            TakeDamageCntrl takeDamageCntrl = other.GetComponent<TakeDamageCntrl>();

            if (takeDamageCntrl.TakeDamage(damage))
            {
                if (destroyPrefab != null)
                {
                    GameObject explode = Instantiate(destroyPrefab, transform.position, Quaternion.identity);
                    Destroy(explode, 3.0f);
                }

                Destroy(other.gameObject);

                EventManager.Instance.InvokeOnEnemyDestroy();
                EventManager.Instance.InvokeOnDisplayXpKills(enemyCntrl.GetXp());
            }
        }


        Destroy(gameObject);
    }
}
