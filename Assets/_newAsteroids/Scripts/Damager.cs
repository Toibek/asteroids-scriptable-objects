using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int Damage;
    public int DestroyAfterTargets;
    int targetsHit;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Damageable>(out Damageable dmg))
        {
            dmg.Damage(Damage);
            if (++targetsHit == DestroyAfterTargets) Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Damageable>(out Damageable dmg))
        {
            dmg.Damage(Damage);
            if (++targetsHit == DestroyAfterTargets) Destroy(gameObject);
        }
    }
}
