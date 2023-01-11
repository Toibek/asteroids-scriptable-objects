using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    PlayerInputManager pim;
    private void Start()
    {
        pim = GetComponent<PlayerInputManager>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
