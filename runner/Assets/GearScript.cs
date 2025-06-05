using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearScript : MonoBehaviour
{
    [SerializeField] private Transform GFX;
    private Rigidbody2D rb;
    [SerializeField] public float speed;
    // Start is called before the first frame update
    void Start()
    {
       rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GFX.transform.Rotate(Vector3.right, 1f * Time.deltaTime);
        rb.rotation = Time.deltaTime * speed;
    }
}
