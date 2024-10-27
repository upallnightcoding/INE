using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEnemyManager()
    {
        SpawnOneEnemy(gameData.enemy[1], new Vector3(5.0f, 0.0f, 0.0f));
    }

    private void Spawn()
    {
        //SpawnTwoEnemies(testEnemy, testEnemy);
    }

    /**
     * SpawnTwoEnemies() - 
     */
    private void SpawnTwoEnemies(EnemySO left, EnemySO right)
    {
        float u = Random.Range(0.0f, 1.0f) * ((Random.Range(0, 2) == 0) ? -1.0f : 1.0f);
        float w = Random.Range(0.0f, 1.0f) * ((Random.Range(0, 2) == 0) ? -1.0f : 1.0f);
        Vector3 direction = (new Vector3(u, 0.0f, w)).normalized;

        Vector3 playerPos = player.transform.position;

        Vector3 leftPos = playerPos + direction * 5.0f;
        Vector3 rightPos = playerPos + direction * -5.0f;

        SpawnOneEnemy(PickEnemy(), leftPos);
        SpawnOneEnemy(PickEnemy(), rightPos);
    }

    /**
     * PickEnemy() - 
     */
    private EnemySO PickEnemy()
    {
        int choice = Random.Range(0, gameData.enemy.Length);

        return (gameData.enemy[choice]);
    }

    private void SpawnOneEnemy(EnemySO enemy, Vector3 position)
    {
        if (enemy.spawnPrefab != null)
        {
            Instantiate(enemy.spawnPrefab, position, Quaternion.identity);
        }

        GameObject go = Instantiate(enemy.prefab, position, Quaternion.identity);
        go.GetComponent<EnemyCntrl>().SetXp(enemy.xp);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnEnemyDestroy += Spawn;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEnemyDestroy -= Spawn;
    }
}
