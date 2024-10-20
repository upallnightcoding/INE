using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private EnemySO testEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnOneEnemy(testEnemy, new Vector3(3.0f, 0.0f, 0.0f));

        SpawnTwoEnemies(testEnemy, testEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTwoEnemies(EnemySO left, EnemySO right)
    {
        float u = Random.Range(0.0f, 1.0f);
        float w = Random.Range(0.0f, 1.0f);
        Vector3 direction = (new Vector3(u, 0.0f, w)).normalized;

        Vector3 playerPos = player.transform.position;

        Vector3 leftPos = playerPos + direction * 5.0f;
        Vector3 rightPos = playerPos + direction * -5.0f;

        SpawnOneEnemy(testEnemy, leftPos);
        SpawnOneEnemy(testEnemy, rightPos);
    }

    private void SpawnOneEnemy(EnemySO enemy, Vector3 position)
    {
        if (enemy.spawnPrefab != null)
        {
            Instantiate(enemy.spawnPrefab, position, Quaternion.identity);
        }

        GameObject go = Instantiate(enemy.prefab, position, Quaternion.identity);
    }
}
