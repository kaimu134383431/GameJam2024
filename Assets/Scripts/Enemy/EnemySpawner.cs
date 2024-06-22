using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs; // �G�L�����N�^�[�̃v���n�u�z��
    [SerializeField] float spawnRate = 2f;      // �G���X�|�[��������Ԋu
    [SerializeField] Transform[] spawnPoints;   // �G�̃X�|�[���ʒu�̔z��

    private float nextSpawn = 0f;

    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
    }
}
