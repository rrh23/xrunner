using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPools;

public class ObjectPools : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject[] prefab;
        public int size;
    }

    public List<Pool> pools;
    public int prefabCount, prefabID;

    public Dictionary<int, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<int, Queue<GameObject>>();

        int poolIndex = 0;

        // For each Pool (group of prefabs)
        foreach (Pool pool in pools)
        {
            // For each prefab in that Pool
            for (int prefabIdx = 0; prefabIdx < pool.prefab.Length; prefabIdx++)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int j = 0; j < pool.size; j++)
                {
                    GameObject obj = Instantiate(pool.prefab[prefabIdx]);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(poolIndex, objectPool);
                poolIndex++;
            }
        }
    }



    public static ObjectPools Instance;

    private void Awake()
    {
        Instance = this;
    }
    public GameObject SpawnFromPool(int index, Vector3 position, Quaternion rot)
    {
        if (!poolDictionary.ContainsKey(index))
        {
            Debug.LogWarning("Pool with index " + index + " doesn't exist.");
            return null;
        }

        

        GameObject objToSpawn = poolDictionary[index].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rot;

        IPoolObject pooledObj = objToSpawn.GetComponent<IPoolObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[index].Enqueue(objToSpawn);

        return objToSpawn;
    }

    public GameObject SpawnRandomFromPools(Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.Count == 0)
        {
            Debug.LogWarning("No pools available.");
            return null;
        }

        int randomIndex = Random.Range(0, poolDictionary.Count - 1);

        prefabID = randomIndex; //saves number to preefabID so other script can recognize
        return SpawnFromPool(randomIndex, position, rotation);
    }

}
