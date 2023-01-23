using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPersistant : MonoBehaviour
{
    Vector2 screenSize;
    bool ready = false;
    private void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height));
    }
    private void SetReady()
    {
        ready = true;
        if(TryGetComponent<Mirror>(out Mirror mirror))
        {
            mirror.Setup();
        }
    }
    private void LateUpdate()
    {
        if (ready)
        {
            if (transform.position.x > screenSize.x) transform.position -= new Vector3(screenSize.x * 2, 0);
            if (transform.position.x < -screenSize.x) transform.position += new Vector3(screenSize.x * 2, 0);

            if (transform.position.y > screenSize.y) transform.position -= new Vector3(0, screenSize.y * 2);
            if (transform.position.y < -screenSize.y) transform.position += new Vector3(0, screenSize.y * 2);
        }
        else if
            (
            transform.position.x < screenSize.x &&
            transform.position.x > -screenSize.x &&
            transform.position.y < screenSize.y &&
            transform.position.y > -screenSize.y
            ) SetReady();
    }
}
