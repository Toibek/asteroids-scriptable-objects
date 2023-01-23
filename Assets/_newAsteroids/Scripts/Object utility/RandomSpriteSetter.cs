using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteSetter : MonoBehaviour
{
    public Sprite[] Sprites;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Sprites[Random.Range(0, Sprites.Length)];
    }
}
