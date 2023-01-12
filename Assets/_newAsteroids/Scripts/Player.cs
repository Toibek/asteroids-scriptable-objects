using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Art")]
    [SerializeField] Sprite bodySprite;
    [SerializeField] Sprite thrustSprite;
    [SerializeField] Sprite forcefieldSprite;
    [Header("Controls")]
    [SerializeField] float thrust;
    [SerializeField] float torque;
    [SerializeField] float fireRate;

    ShipObject[] shipObjects;

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
    Coroutine invRoutine;

    IEnumerator invEnum()
    {
        ForcefieldVisual(true);
        dam.Invinsible = true;
        while (it >= 0)
        {
            it -= 0.1f;
            yield return new WaitForSeconds(0.25f);

            if (it <= 1)
            {
                ForcefieldVisual(!ForcefieldVisual());
            }
        }
        ForcefieldVisual(false);
        dam.Invinsible = false;
    }
    Vector2 screenSize;

    Coroutine thrustRoutine;
    float currentThrust;

    Coroutine fireRoutine;
    bool firing;

    Coroutine torqueRoutine;
    float currentTorque;


    PlayerDamageable dam;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dam = GetComponent<PlayerDamageable>();

        shipObjects = new ShipObject[5];
        screenSize = Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height));

        shipObjects[0] = CreateShip();
        shipObjects[0].Body.transform.parent = transform;
        for (int i = 1; i < shipObjects.Length; i++)
        {
            shipObjects[i] = (CreateShip());
        }
        InvTime = 2;
    }
    private void LateUpdate()
    {
        for (int i = 1; i < shipObjects.Length; i++)
        {
            Quaternion dir = Quaternion.Euler(0, 0, 90 * (i - 1));
            Vector2 position = dir * Vector2.up * screenSize * 2;
            shipObjects[i].Body.transform.SetPositionAndRotation((Vector2)transform.position + position, transform.rotation);
        }
        if (transform.position.x > screenSize.x) transform.position -= new Vector3(screenSize.x * 2, 0);
        if (transform.position.x < -screenSize.x) transform.position += new Vector3(screenSize.x * 2, 0);

        if (transform.position.y > screenSize.y) transform.position -= new Vector3(0, screenSize.y * 2);
        if (transform.position.y < -screenSize.y) transform.position += new Vector3(0, screenSize.y * 2);
    }
    ShipObject CreateShip()
    {
        ShipObject obj = new();

        obj.Body = new GameObject("ship");
        obj.Thrust = new GameObject("thrust");
        obj.Forcefield = new GameObject("forcefield");

        SpriteRenderer bsr = obj.Body.AddComponent<SpriteRenderer>();
        bsr.sprite = bodySprite;

        SpriteRenderer tsr = obj.Thrust.AddComponent<SpriteRenderer>();
        tsr.sprite = thrustSprite;
        tsr.sortingOrder = 10;

        SpriteRenderer fsr = obj.Forcefield.AddComponent<SpriteRenderer>();
        fsr.sprite = forcefieldSprite;
        fsr.sortingOrder = 20;

        obj.Thrust.transform.parent = obj.Body.transform;
        obj.Forcefield.transform.parent = obj.Body.transform;

        obj.Thrust.SetActive(false);
        obj.Forcefield.SetActive(false);

        return obj;
    }
    void ThrustVisual(bool value)
    {
        if (shipObjects[0].Thrust.activeInHierarchy == value) return;
        for (int i = 0; i < shipObjects.Length; i++)
            shipObjects[i].Thrust.SetActive(value);
    }
    bool ThrustVisual()
    {
        return shipObjects[0].Thrust.activeInHierarchy;
    }
    void ForcefieldVisual(bool value)
    {
        if (shipObjects[0].Forcefield.activeInHierarchy == value) return;
        for (int i = 0; i < shipObjects.Length; i++)
            shipObjects[i].Forcefield.SetActive(value);
    }
    bool ForcefieldVisual()
    {
        return shipObjects[0].Forcefield.activeInHierarchy;
    }

    #region movement
    void OnTorque(InputValue value)
    {
        currentTorque = value.Get<float>();
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
    void OnThrust(InputValue context)
    {
        currentThrust = context.Get<float>();
        if (currentThrust != 0 && thrustRoutine == null)
            thrustRoutine = StartCoroutine(thrustEnum());
    }
    IEnumerator thrustEnum()
    {
        ThrustVisual(true);
        while (currentThrust != 0)
        {
            rb.AddForce(transform.up * currentThrust * thrust);
            yield return new WaitForEndOfFrame();
        }
        ThrustVisual(false);
        thrustRoutine = null;
    }
    #endregion

    #region fire
    void OnFire(InputValue value)
    {
        firing = value.Get<float>() != 0;
        Debug.Log("Firing: " + firing);
        if (firing && fireRoutine == null) fireRoutine =
                StartCoroutine(fireEnum());
    }
    IEnumerator fireEnum()
    {
        while (firing)
        {
            Debug.Log("pew!");
            yield return new WaitForSeconds(1 / fireRate);
        }
        fireRoutine = null;
    }
    #endregion
}
[System.Serializable]
public class ShipObject : object
{
    public GameObject Body;
    public GameObject Thrust;
    public GameObject Forcefield;
}
