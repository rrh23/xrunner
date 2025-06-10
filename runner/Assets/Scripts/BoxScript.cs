using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    //im relearning college/midschool math for this it better be fucking worth it
    //"dot product"
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        var normal = other.contacts[0].normal;
        var safeSurface = Mathf.Abs(Vector2.Dot(Vector2.up, normal)) > 0.71f;
        if (safeSurface) return;

        StartCoroutine(PlayerLogic.instance.Damaged());
    }
}
