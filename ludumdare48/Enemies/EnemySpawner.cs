using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPositions;
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] List<EnemySO> enemyData;
    [SerializeField] [Range(2f, 5f)] float maxSpawnDelay = 2;
    [SerializeField] Transform liftTargetTransform;

    private void OnEnable()
    {
        StartSpawner();    
    }

    public void StartSpawner()
    {
        StartCoroutine(Spawner());
    }
    
    public void SetMaxSpawnDelay(float delay)
    {
        maxSpawnDelay = delay;
    }

    IEnumerator Spawner()
    {
        SpawnAnEnemy();
        yield return new WaitForSeconds(Random.Range(0, maxSpawnDelay));
        StartSpawner();
    }

    private void SpawnAnEnemy()
    {
        GameObject enemyGO = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
        EnemyController enemyController = enemyGO.GetComponent<EnemyController>();
        enemyController.Setup(GetRandomEnemyData(), GetRandomSpawnPosition(), liftTargetTransform);
    }

    private EnemySO GetRandomEnemyData()
    {
        return enemyData[Random.Range(0, enemyData.Count)];
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 position = spawnPositions[Random.Range(0, spawnPositions.Count)].position + new Vector3(0, Random.Range(-4f, 4f), 0);
        return position;
    }
}
