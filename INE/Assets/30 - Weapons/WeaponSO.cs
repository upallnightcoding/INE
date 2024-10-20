using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Create/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;

    public GameObject prefab;

    public float force;

    public float damage;
}
