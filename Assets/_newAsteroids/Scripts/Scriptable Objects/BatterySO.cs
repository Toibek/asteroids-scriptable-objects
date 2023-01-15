using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battery", menuName = "Ship/Battery")]
[System.Serializable]
public class BatterySO : ScriptableObject
{
    public float MaxCharge = 100f;
    public float RechargeRate = 5f;

    float lastRecharge;
    float battery;

    public void Reset()
    {
        battery = MaxCharge;
    }
    public bool Drain(float amount)
    {
        Recharge();
        if (battery > amount)
            battery -= amount;
        else return false;
        return true;
    }
    void Recharge()
    {
        battery = Mathf.Clamp(battery + (Time.time - lastRecharge) * RechargeRate, 0, MaxCharge);
        lastRecharge = Time.time;
    }
}
