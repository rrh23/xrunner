using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public float obstacleSpawnTime = 0.706f;
    public float timeUntilSpawn;
    public float obstacleSpeed = 1.0f;
    public LogicScript logic;
    public bool isSpawning;
    private int spawnAmount;
    private int randPrefab;
    private int poolCount;

    public ObjectPools objectPools;

    private void Start()
    {
        isSpawning = true;
        objectPools = ObjectPools.Instance;
        poolCount = objectPools.prefabCount;
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
        GameObject obstacleToSpawn;
        Rigidbody2D obstacleRB;

        //spawn tracker
        spawnAmount++;

        if(spawnAmount%10 == 0) //every multiple of 10 spawns a checkpoint pillar
        {

            obstacleToSpawn = objectPools.SpawnFromPool(6, new Vector3(transform.position.x, 0), Quaternion.identity);
            obstacleRB = obstacleToSpawn.GetComponent<Rigidbody2D>();
            obstacleRB.velocity = Vector2.left * obstacleSpeed;
        }
        else if(spawnAmount%10 != 0)
        {
            obstacleToSpawn = objectPools.SpawnRandomFromPools(new Vector3(transform.position.x, Random.Range(lowestpos, highestpos)), Quaternion.identity);
            obstacleRB = obstacleToSpawn.GetComponent<Rigidbody2D>();
            obstacleRB.velocity = Vector2.left * obstacleSpeed;


            //if (randPrefab < 3) //stray cubes & not actual structures
            //{
            //    obstacleToSpawn = objectPools.SpawnRandomFromPools(new Vector3(transform.position.x, Random.Range(lowestpos, highestpos), 0), Quaternion.identity);
            //    obstacleRB = obstacleToSpawn.GetComponent<Rigidbody2D>();
            //    obstacleRB.velocity = Vector2.left * obstacleSpeed;
            //}
            //else //actual structures stick to the ground
            //{
            //    obstacleToSpawn = objectPools.SpawnRandomFromPools(new Vector3(transform.position.x, Random.Range(-2, 1), 0), Quaternion.identity);
            //    obstacleRB = obstacleToSpawn.GetComponent<Rigidbody2D>();
            //    obstacleRB.velocity = Vector2.left * obstacleSpeed;
            //}

        }

    }
}
