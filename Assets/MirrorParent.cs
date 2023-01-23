using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorParent : MonoBehaviour
{
    private void Start()
    {
        Mirror.Parent = transform;
    }
    private void OnDestroy()
    {
        Mirror.Parent = null;
    }
}
