using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int MaxHealth;
    public bool Invinsible;
    internal int Health;
    public EmptyDelegate OnDamage;
    public EmptyDelegate OnLethalDamage;

    public delegate void EmptyDelegate();
    public virtual void Start()
    {
        Health = MaxHealth;
    }
    private void OnLeave()
    {
        LethalDamage();
    }
    public virtual void Damage(int amount)
    {
        if (Invinsible) return;
        Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
        if (Health == 0) LethalDamage();
        else OnDamage?.Invoke();
    }
    public virtual void Heal(int amount)
    {
        Health = (Mathf.Clamp(Health + amount, 0, MaxHealth));
    }
    public virtual void LethalDamage()
    {
        OnLethalDamage?.Invoke();
        Destroy(gameObject);
    }
}
