using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Core", menuName = "Ship/Core")]
public class ShipSO : ScriptableObject
{
    public Sprite[] Sprites;
    public float Mass = 0.5f;
    public int Health = 10;
    public EngineSO Engine;
    public WeaponSO Weapon;
    public BatterySO Battery;
}
