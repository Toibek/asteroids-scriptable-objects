using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSO : ScriptableObject
{
    public string ID = System.Guid.NewGuid().ToString().ToUpper();
    public string Name;
    public float Thrust = 1.5f;
    public float Torque = 0.5f;
    public float ThrustCost = 5;
}
