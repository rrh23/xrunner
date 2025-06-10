using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float obstacleSpawnTime;
    public float timeUntilSpawn;
    public float obstacleSpeed = 1.0f;
    public LogicScript logic;
    private int spawnAmount;
    private int randPrefab;
    private int poolCount;
    private int obstacleID;
    private int isGroundOrCeiling;
    private bool isPlaying;

    public ObjectPools objectPools;

    private void Start()
    {
        objectPools = ObjectPools.Instance;
        poolCount = objectPools.prefabCount;
    }

    private void Update()
    {
        isPlaying = LogicScript.Instance.isPlaying;
        if (isPlaying)
        {
            SpawnLoop();

            obstacleSpeed += Time.deltaTime * 0.008f;
            obstacleSpawnTime = Mathf.Max(obstacleSpawnTime - Time.deltaTime * 0.01f, 0.706f); 
        }
    }

    private bool IsSpawnAreaClear(Vector3 position, float radius = 1f)
    {
        var hit = Physics2D.OverlapCircle(position, radius);
        return !hit;
    }

    private void SpawnLoop()
    {
        timeUntilSpawn += Time.deltaTime;

        if (timeUntilSpawn > obstacleSpawnTime) Spawn();
    }

    private void Spawn()
    {
        var lowestpos = -3f;
        float highestpos = 4;
        GameObject obstacleToSpawn;
        Rigidbody2D obstacleRB;


        //spawn tracker
        spawnAmount++;
        

        var spawnPos = new Vector3(transform.position.x, Random.Range(lowestpos, highestpos));

        if (!IsSpawnAreaClear(spawnPos)) return;
        timeUntilSpawn = 0;
        if (spawnAmount % 50 == 0) //every multiple of 50 spawns a checkpoint pillar
        {
            obstacleToSpawn =
                objectPools.SpawnFromPool(6, new Vector3(transform.position.x, 0), Quaternion.identity);
            // Debug.Log("checkpoint!");
        }
        else
        {
            obstacleID = objectPools.prefabID; //gets which prefab got spawned
            // Debug.Log("spawned object: "+ obstacleID);
            if (obstacleID < 2) //box and gear
            {
                obstacleToSpawn = objectPools.SpawnRandomFromPools(
                    new Vector3(transform.position.x, Random.Range(lowestpos, highestpos)), Quaternion.identity);
            }
            else if (obstacleID is >= 2 and <= 3) //floating structures
            {
                obstacleToSpawn = objectPools.SpawnRandomFromPools(
                    new Vector3(transform.position.x, Random.Range(lowestpos, highestpos)), Quaternion.identity);
            }
            else //big structures either stuck in ground or ceiling
            {
                isGroundOrCeiling = Random.Range(0, 2);
                //0 = ground; 1 = ceiling
                //range is weird that it excludes 1 if i use Range(0,1) so thats why its 2

                if (isGroundOrCeiling == 0)
                {
                    obstacleToSpawn = objectPools.SpawnRandomFromPools(
                        new Vector3(transform.position.x, Random.Range(-3, -1.9f)), Quaternion.identity);
                }
                else
                {
                    obstacleToSpawn = objectPools.SpawnRandomFromPools(
                        new Vector3(transform.position.x, Random.Range(2, 4)), Quaternion.Euler(0, 0, 180));
                }
            }
        }
        obstacleRB = obstacleToSpawn.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * obstacleSpeed;
    }
}