using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public ShipSO ShipCore;
    Coroutine invRoutine;
    Coroutine batRoutine;
    FloatSO batteryCharge;
    float InvTime
    {
        get
        {
            return it;
        }
        set
        {
            it = value;
            if (invRoutine == null)
                invRoutine = StartCoroutine(invEnum());
        }
    }
    float it;

    Damageable dam;
    Rigidbody2D rb;

    public void Setup(ShipSO shipCore)
    {
        ShipCore = shipCore;
        rb = Component<Rigidbody2D>();
        ShipCore.OnMassChanged += UpdateMass;
        UpdateMass();


        batteryCharge = ScriptableObject.CreateInstance<FloatSO>();
        batRoutine = StartCoroutine(shipCore.Battery.RechargeEnum(batteryCharge));

        Sprite[] sprites = { ShipCore.ShipSprite, ShipCore.ThrustSprite, ShipCore.ShieldSprite };
        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject go = new(i.ToString());
            go.transform.parent = transform;
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprites[i];
            sr.sortingOrder = 10 * i;
        }


        Component<Mirror>().Setup();
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);

        dam = Component<Damageable>();
        dam.Health = ShipCore.Health;
        InvTime = 2;

        Component<Movement>().Setup(ShipCore.Engine, ShipCore.Battery, batteryCharge);
        Component<Weapons>().Setup(ShipCore.Weapon, ShipCore.Battery, batteryCharge);
    }
    private void OnDestroy()
    {
        ShipCore.OnMassChanged -= UpdateMass;
        StopCoroutine(batRoutine);
    }
    void UpdateMass()
    {
        rb.mass = 1 + (ShipCore.Mass * 2);
    }
    T Component<T>()
    {
        if (TryGetComponent(out T component))
        {
            return component;
        }
        else
        {
            gameObject.AddComponent(typeof(T));
            return gameObject.GetComponent<T>();
        }
    }
    IEnumerator invEnum()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        dam.Invinsible = true;
        while (it >= 0)
        {
            it -= 0.1f;
            yield return new WaitForSeconds(0.25f);

            if (it <= 1)
            {
                transform.GetChild(2).gameObject.SetActive(!transform.GetChild(2).gameObject.activeInHierarchy);
            }
        }
        transform.GetChild(2).gameObject.SetActive(false);
        dam.Invinsible = false;
    }
}
