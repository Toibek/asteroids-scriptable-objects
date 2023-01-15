using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableChecker : MonoBehaviour
{
    public Empty OnShow;
    public Empty OnHide;
    public delegate void Empty();
    private void OnEnable()
    {
        OnShow?.Invoke();
    }
    private void OnDisable()
    {
        OnHide?.Invoke();
    }
    private void OnApplicationQuit()
    {
        OnShow = null;
        OnHide = null;
    }
}
