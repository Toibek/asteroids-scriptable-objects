using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSO : ScriptableObject
{
    public string ID = System.Guid.NewGuid().ToString().ToUpper();
    public string Name;

    public Sprite ShipSprite;
    public Sprite ThrustSprite;
    public Sprite ShieldSprite;

    public float Mass = 0.5f;
    public int Health = 10;
    public  EmptyDelegate OnMassChanged;

    public EngineSO Engine;
    public WeaponSO Weapon;
    public BatterySO Battery;

    public delegate void EmptyDelegate();
}
