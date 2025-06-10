using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    bool initialized = false;
    void Update()
    {
        if (!initialized) return;
        if (gameObject.transform.position.x < -20f)
        {
            if (gameObject)
            {
                Release();
            }
        }
    }

    private Queue<GameObject> _queue;


    public void OnObjectSpawn(Queue<GameObject> queue)
    {
        initialized = true;
        _queue = queue;
    }

    public void Release()
    {
        initialized = false;
        _queue.Enqueue(gameObject);
        gameObject.SetActive(false);
    }
}