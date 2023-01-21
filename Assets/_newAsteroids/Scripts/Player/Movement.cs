using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    EngineSO engine;
    BatterySO battery;

    Coroutine thrustRoutine;
    float currentThrust;

    Coroutine torqueRoutine;
    float currentTorque;
    Rigidbody2D rb;
    public void Setup(EngineSO engine, BatterySO battery)
    {
        this.engine = engine;
        this.battery = battery;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTorque(InputValue value)
    {
        currentTorque = value.Get<float>();
        if (currentTorque != 0 && torqueRoutine == null)
            torqueRoutine = StartCoroutine(TorqueEnum());
    }
    private IEnumerator TorqueEnum()
    {
        while (currentTorque != 0)
        {
            rb.AddTorque(-currentTorque * engine.Torque);
            yield return new WaitForEndOfFrame();
        }
        torqueRoutine = null;
    }
    private void OnThrust(InputValue context)
    {
        currentThrust = context.Get<float>();
        if (currentThrust != 0 && thrustRoutine == null)
            thrustRoutine = StartCoroutine(ThrustEnum());
    }
    private IEnumerator ThrustEnum()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        while (currentThrust != 0 && battery.Continous(engine.ThrustCost * Time.deltaTime))
        {
            rb.AddForce(transform.up * currentThrust * engine.Thrust);
            yield return new WaitForEndOfFrame();
        }
        transform.GetChild(1).gameObject.SetActive(false);
        thrustRoutine = null;
    }
}
