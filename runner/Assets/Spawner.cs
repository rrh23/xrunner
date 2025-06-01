using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    public float obstacleSpawnTime = 0.706f;
    public float timeUntilSpawn;
    public float obstacleSpeed = 1.0f;
    public LogicScript logic;
    public bool isSpawning;

    private void Start()
    {
        isSpawning = true;
    }
    private void Update()
    {
        if (isSpawning)
        {
            SpawnLoop();
        }
    }

    private void SpawnLoop()
    {
        timeUntilSpawn += Time.deltaTime;

        if (timeUntilSpawn > obstacleSpawnTime)
        {
            Spawn();
            timeUntilSpawn = 0;
        }
    }

    private void Spawn()
    {
        float lowestpos = -5f;
        float highestpos = 5;

        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, new Vector3(transform.position.x, Random.Range(lowestpos, highestpos), 0), Quaternion.identity);
        //quarternion.id just copies rotation from the prefab
        //

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * obstacleSpeed;

        if (obstacleRB.position.x < -15f)
        {
            Destroy(spawnedObstacle);
        }
        
    }
}
