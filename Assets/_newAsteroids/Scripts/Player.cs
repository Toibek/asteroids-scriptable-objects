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


    Vector2 screenSize;

    Coroutine thrustRoutine;
    float currentThrust;

    Coroutine fireRoutine;
    bool firing;

    Coroutine torqueRoutine;
    float currentTorque;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        shipObjects = new ShipObject[4];
        screenSize = Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height));

        ShipObject obj = createShip(transform.position);
        obj.Body.transform.parent = transform;
        for (int i = 0; i < shipObjects.Length; i++)
        {
            shipObjects[i] = (createShip(Vector2.zero));
        }
    }
    ShipObject createShip(Vector2 position)
    {
        ShipObject obj = new();

        obj.Body = new GameObject("ship");
        obj.Thrust = new GameObject("thrust");
        obj.ForceField = new GameObject("forcefield");

        SpriteRenderer bsr = obj.Body.AddComponent<SpriteRenderer>();
        bsr.sprite = bodySprite;

        SpriteRenderer tsr = obj.Thrust.AddComponent<SpriteRenderer>();
        tsr.sprite = thrustSprite;
        tsr.sortingOrder = 10;

        SpriteRenderer fsr = obj.ForceField.AddComponent<SpriteRenderer>();
        fsr.sprite = forcefieldSprite;
        fsr.sortingOrder = 20;

        obj.Thrust.transform.parent = obj.Body.transform;
        obj.ForceField.transform.parent = obj.Body.transform;

        obj.Body.transform.position = position;
        return obj;
    }
    private void LateUpdate()
    {
        for (int i = 0; i < shipObjects.Length; i++)
        {
            Quaternion dir = Quaternion.Euler(0, 0, 90 * i);
            Vector2 position = dir * Vector2.up * screenSize * 2;
            shipObjects[i].Body.transform.SetPositionAndRotation((Vector2)transform.position + position, transform.rotation);
        }
        if (transform.position.x > screenSize.x) transform.position -= new Vector3(screenSize.x * 2, 0);
        if (transform.position.x < -screenSize.x) transform.position += new Vector3(screenSize.x * 2, 0);

        if (transform.position.y > screenSize.y) transform.position -= new Vector3(0, screenSize.y * 2);
        if (transform.position.y < -screenSize.y) transform.position += new Vector3(0, screenSize.y * 2);
    }
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
        while (currentThrust != 0)
        {
            rb.AddForce(transform.up * currentThrust * thrust);
            yield return new WaitForEndOfFrame();
        }
        thrustRoutine = null;
    }

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
}
[System.Serializable]
public class ShipObject : object
{
    public GameObject Body;
    public GameObject Thrust;
    public GameObject ForceField;
}
