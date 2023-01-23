using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirection : MonoBehaviour
{
    public float force = 2;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = randir() * force;
    }
    Vector2 randir()
    {
        Vector2 dir = Vector2.zero;
        while (dir.magnitude < 0.5f)
        {
            dir = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        }
        return dir;
    }
}
