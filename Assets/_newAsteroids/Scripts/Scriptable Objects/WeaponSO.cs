using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Ship/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string ID = System.Guid.NewGuid().ToString().ToUpper();
    public string Name;
    public GameObject PrefabBullet;
    public float FireRate = 5;
    public float ShootForce;
    public float FiringCost;
}