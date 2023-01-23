using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BatterySO : ScriptableObject
{
    public string ID = System.Guid.NewGuid().ToString().ToUpper();
    public string Name;
    public float MaxCharge = 100f;
    public float RechargeRate = 5f;
    public bool Impulse(FloatSO charge, float cost)
    {
        return Drain(charge, cost);
    }
    public bool Continous(FloatSO charge, float cost)
    {
        return Drain(charge, cost * Time.deltaTime);
    }
    bool Drain(FloatSO charge, float cost)
    {
        if (charge.Value > (cost / MaxCharge))
        {
            charge.Value -= (cost / MaxCharge);
            return true;
        }
        return false;
    }
    public IEnumerator RechargeEnum(FloatSO charge)
    {
        while (true)
        {
            charge.Value = Mathf.Clamp(charge.Value + (RechargeRate * Time.deltaTime) / MaxCharge, 0, 1);
            yield return new WaitForEndOfFrame();
        }
    }
}
