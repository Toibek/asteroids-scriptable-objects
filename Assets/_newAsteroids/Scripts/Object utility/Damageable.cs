using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int MaxHealth;
    public bool Invinsible;
    internal int Health;
    public virtual void Start()
    {
        Health = MaxHealth;
    }
    private void OnLeave()
    {
        GetComponent<Damageable>().LethalDamage();
    }
    public virtual void Damage(int amount)
    {
        if (Invinsible) return;
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
