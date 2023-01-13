using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPersistance : MonoBehaviour
{
    private void LateUpdate()
    {

        Vector2 screenSize = Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height)); // move to start unless we're changing the camera size during play

        if (transform.position.x > screenSize.x) transform.position -= new Vector3(screenSize.x * 2, 0);
        if (transform.position.x < -screenSize.x) transform.position += new Vector3(screenSize.x * 2, 0);

        if (transform.position.y > screenSize.y) transform.position -= new Vector3(0, screenSize.y * 2);
        if (transform.position.y < -screenSize.y) transform.position += new Vector3(0, screenSize.y * 2);
    }
}
