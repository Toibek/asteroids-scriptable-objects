using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    public bool Invinsible;
    public override void Damage(int amount)
    {
        if (!Invinsible)
            base.Damage(amount);
    }
}
