using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Create/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;

    public GameObject prefab;

    public bool manual = true;

    public float force;

    public float damage;

    public int maxRounds;

    public float reloadSec;

    public float roundsPerSec;

    public float destroyTiming;
}
