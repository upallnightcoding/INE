using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawn(EnemySO enemy, Vector3 position)
    {
        GameObject go = Instantiate(enemy.prefab, position, Quaternion.identity);
    }
}
