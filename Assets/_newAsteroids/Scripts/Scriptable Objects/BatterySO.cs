using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Battery", menuName = "Ship/Battery")]
public class BatterySO : ScriptableObject
{
    public string ID = System.Guid.NewGuid().ToString().ToUpper();
    public string Name;
    public float MaxCharge = 100f;
    public float RechargeRate = 5f;

    float lastRecharge;
    public float Battery;
    public void Reset()
    {
        Battery = 1;
    }
    public bool Continous(float amount) { return Drain(amount * Time.deltaTime); }
    public bool Impulse(float amount) { return Drain(amount); }
    private bool Drain(float amount)
    {
        //Recharge();
        if (Battery * MaxCharge > amount)
            Battery -= amount / MaxCharge;
        else return false;
        return true;
    }
    public IEnumerator RechargeEnum()
    {
        while (true)
        {
            Battery = Mathf.Clamp(Battery + (RechargeRate * Time.deltaTime) / MaxCharge, 0, 1);
            yield return new WaitForEndOfFrame();
        }
    }
}
