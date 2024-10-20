using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName ="Create/Enemy")]
public class EnemySO : ScriptableObject
{
    public GameObject prefab;

    public GameObject spawnPrefab;

    public float health;
}
