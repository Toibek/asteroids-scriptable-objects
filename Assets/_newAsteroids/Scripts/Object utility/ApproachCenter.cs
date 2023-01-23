using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachCenter : MonoBehaviour
{
    public float force = 2;
    void Start()
    {
        Vector2 pos = transform.position;
        Vector2 dir = (new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)) - pos).normalized;
        GetComponent<Rigidbody2D>().velocity = dir * force;
    }
}
