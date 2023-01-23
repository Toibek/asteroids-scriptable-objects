using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] GameObject[] prefab;
    [SerializeField] int amount;
    private void Start()
    {
        Enable();
    }
    private void OnDestroy()
    {
        Disable();
    }
    public void Enable()
    {
        GetComponent<Damageable>().OnLethalDamage += SpawnObjects;
    }
    public void Disable()
    {
        GetComponent<Damageable>().OnLethalDamage -= SpawnObjects;
    }
    private void SpawnObjects()
    {
        for (int i = 0; i < amount; i++)
        {
            for (int p = 0; p < prefab.Length; p++)
            {
                Instantiate(prefab[p], transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)), Quaternion.identity, transform.parent);
            }
        }
    }
}
