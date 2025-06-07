using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour, IPoolObject
{
    [SerializeField] GameObject Object1, Object2;
    
    // Start is called before the first frame update

    private void Start()
    {
        Object1 = gameObject;
        Object2 = GameObject.FindGameObjectWithTag("Obstacle");
        
    }
    public void OnObjectSpawn()
    {
        if (gameObject.transform.position.x < -20f)
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //destroys any obstacle thats too close to each other
        if (Vector3.Distance(Object1.transform.position, Object2.transform.position) < 0.5)
        {
            if (Object2 != null)
            {
                Vector3 direction = (Object2.transform.position - Object1.transform.position).normalized;
                float moveDistance = 1.3f;
                Object2.transform.position += direction * moveDistance;
            }
        }
    }
}
