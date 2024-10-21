using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageCntrl : MonoBehaviour
{
    private float health = 30.0f;

    public void Set(EnemySO enemy)
    {
        health = enemy.health;
    }

    public bool TakeDamage(float damage)
    {
        health -= damage;

        return (health <= 0.0f);
    }
}