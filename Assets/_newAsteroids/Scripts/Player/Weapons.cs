using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapons : MonoBehaviour
{
    internal WeaponSO weapon;
    BatterySO battery;

    Coroutine fireRoutine;
    bool firing;

    public void Setup(WeaponSO weapon, BatterySO battery)
    {
        this.weapon = weapon;
        this.battery = battery;
    }
    private void OnFire(InputValue value)
    {
        firing = value.Get<float>() != 0;
        if (firing && fireRoutine == null) fireRoutine = StartCoroutine(fireEnum());
    }
    private IEnumerator fireEnum()
    {
        while (firing && battery.Drain(weapon.FiringCost))
        {
            GameObject bul = Instantiate(weapon.PrefabBullet, transform.position, transform.rotation, null);
            bul.GetComponent<Rigidbody2D>().AddForce(transform.rotation * Vector2.up * weapon.ShootForce);
            yield return new WaitForSeconds(1 / weapon.FireRate);
        }
        fireRoutine = null;
    }
}
