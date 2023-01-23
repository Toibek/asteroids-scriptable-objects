using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public static Transform Parent;
    public bool StartOn;
    Transform[] Mirrors;
    Vector2 screenSize;
    private void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height));
        if (StartOn) Setup();
    }
    public void Setup()
    {
        Mirrors = new Transform[4];
        for (int i = 0; i < Mirrors.Length; i++)
        {
            Mirrors[i] = new GameObject("Mirror").transform;
            Mirrors[i].parent = Parent;
            if (TryGetComponent(out SpriteRenderer sr))
            {
                Mirrors[i].gameObject.AddComponent<SpriteRenderer>().sprite = sr.sprite;
            }
        }

        for (int c = 0; c < transform.childCount; c++)
        {
            GameObject go = transform.GetChild(c).gameObject;

            for (int m = 0; m < Mirrors.Length; m++)
                Instantiate(go, Mirrors[m]);

            go.name = c.ToString();
            EnableChecker ec = go.AddComponent<EnableChecker>();
            ec.OnShow += () => Active(int.Parse(go.name), true);
            ec.OnHide += () => Active(int.Parse(go.name), false);
        }
    }
    public void Active(int layer, bool set)
    {
        for (int i = 0; i < Mirrors.Length; i++)
        {
            Mirrors[i].GetChild(layer).gameObject.SetActive(set);
        }
    }

    public void LateUpdate()
    {
        if (Mirrors != null)
            for (int i = 0; i < Mirrors.Length; i++)
            {
                Quaternion dir = Quaternion.Euler(0, 0, 90 * i);
                Vector2 position = dir * Vector2.up * screenSize * 2;
                Mirrors[i].SetPositionAndRotation((Vector2)transform.position + position, transform.rotation);
            }
    }
    private void OnDestroy()
    {
        if (Mirrors != null)
            for (int g = 0; g < Mirrors.Length; g++)
            {
                if (Mirrors[g] != null)
                    Destroy(Mirrors[g].gameObject);
            }
    }
}
