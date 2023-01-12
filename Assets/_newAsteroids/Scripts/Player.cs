using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float thrust;
    [SerializeField] float torque;
    [SerializeField] float fireRate;

    Coroutine thrustRoutine;
    bool thrusting;
    Coroutine fireRoutine;
    bool firing;
    Coroutine torqueRoutine;
    float currentTorque;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnMove(InputValue value)
    {
        currentTorque = value.Get<Vector2>().y;
        if (currentTorque != 0 && torqueRoutine == null)
            torqueRoutine = StartCoroutine(TorqueEnum());
    }
    IEnumerator TorqueEnum()
    {
        while (currentTorque != 0)
        {
            rb.AddTorque(-currentTorque * torque);
            yield return new WaitForEndOfFrame();
        }
        torqueRoutine = null;
    }
    void OnThrust()
    {
        thrusting = false;
        if (thrusting && thrustRoutine == null) thrustRoutine = StartCoroutine(thrustEnum());
    }
    IEnumerator thrustEnum()
    {
        while (thrusting)
        {
            rb.AddForce(transform.up * thrust);
            yield return new WaitForEndOfFrame();
        }
        thrustRoutine = null;
    }
    void OnFire(InputValue value)
    {
        if (value.isPressed) Debug.Log("Fire pressed");
        else Debug.Log("Fire not pressed");
    }
    IEnumerator fireEnum()
    {
        while (firing)
        {
            //pew
            yield return new WaitForSeconds(1 / fireRate);
        }
        fireRoutine = null;
    }
}
