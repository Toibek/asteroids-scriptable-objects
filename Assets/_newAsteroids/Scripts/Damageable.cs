using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int MaxHealth;
    internal int Health;
    private void Start()
    {
        Health = MaxHealth;
    }
    public virtual void Damage(int amount)
    {
        Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
        if (Health == 0) LethalDamage();
    }
    public virtual void Heal(int amount)
    {
        Health = (Mathf.Clamp(Health + amount, 0, MaxHealth));
    }
    public virtual void LethalDamage()
    {
        Destroy(gameObject);
    }
}
