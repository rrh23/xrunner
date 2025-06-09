using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour, IPoolObject
{
    
    // Start is called before the first frame update

    private void Start()
    {
        
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
    }
}
