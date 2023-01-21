using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement", menuName = "Ship/Movement")]
public class EngineSO : ScriptableObject
{
    public string ID = System.Guid.NewGuid().ToString().ToUpper();
    public string Name;
    public float Thrust = 1.5f;
    public float Torque = 0.5f;
    public float ThrustCost = 5;
}
