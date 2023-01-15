using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int Damage;
    public int DestroyAfterTargets;

    List<Damageable> damaged = new();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Damageable>(out Damageable dmg))
        {
            DamageTarget(dmg);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Damageable>(out Damageable dmg))
        {
            DamageTarget(dmg);
        }
    }
    void DamageTarget(Damageable dmg)
    {
        if (damaged.Contains(dmg)) return;
        damaged.Add(dmg);
        dmg.Damage(Damage);
        if (damaged.Count == DestroyAfterTargets) Destroy(gameObject);
    }
}
