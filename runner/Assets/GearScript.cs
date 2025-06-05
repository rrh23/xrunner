using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearScript : MonoBehaviour
{
    [SerializeField] private Transform GFX;
    private Rigidbody2D rb;
    [SerializeField] public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Corrected: Use GetComponent on the same GameObject
    }

    void Update()
    {
        GFX.Rotate(Vector3.forward, speed * Time.deltaTime); // Rotate the GFX transform
        //rb.MoveRotation(rb.rotation + speed * Time.deltaTime); // Rotate the Rigidbody2D
    }
}
