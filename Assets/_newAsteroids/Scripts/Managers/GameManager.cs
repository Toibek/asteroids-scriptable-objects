using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject SpawnedObject;
    [SerializeField] float spawnTime;
    [SerializeField] float spawnModifier;
    [SerializeField] float minTime;

    Vector2 maxPosition;
    Vector2 minPosition;
    Vector2 xPosition;
    Vector2 yPosition;

    float randX { get { return Random.Range(xPosition.x, xPosition.y); } }
    float randY { get { return Random.Range(yPosition.x, yPosition.y); } }

    Coroutine SpawningRoutine;
    float startTime;
    void OnGameStart()
    {
        if (SpawningRoutine == null) SpawningRoutine = StartCoroutine(RockSpawning());
    }
    IEnumerator RockSpawning()
    {
        startTime = Time.time;
        maxPosition = Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height)) * 1.1f;
        minPosition = Camera.main.ScreenToWorldPoint(Vector3.zero) * 1.1f;
        xPosition = new(minPosition.x, maxPosition.x);
        yPosition = new(minPosition.y, maxPosition.y);

        while (true)
        {
            float x = Random.Range(xPosition.x, xPosition.y);
            int dir = Random.Range(0, 5);
            Vector2 position;
            switch (dir)
            {
                case 0:
                    position = new(randX, yPosition.y);
                    break;
                case 1:
                    position = new(randX, yPosition.x);
                    break;
                case 2:
                    position = new(randY, xPosition.y);
                    break;
                case 3:
                default:
                    position = new(randY, xPosition.x);
                    break;
            }
            Instantiate(SpawnedObject, position, Quaternion.identity, transform);
            float waitTime = Mathf.Clamp(spawnTime - (spawnModifier * (Time.time - startTime)), minTime, Mathf.Infinity);
            yield return new WaitForSeconds(waitTime);
        }
    }
    void OnGameStop()
    {
        Debug.Log("Disabling spawning of rocks");
        if (SpawningRoutine != null) StopCoroutine(SpawningRoutine);
        SpawningRoutine = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetComponent<Damageable>().LethalDamage();
        }
    }
}
