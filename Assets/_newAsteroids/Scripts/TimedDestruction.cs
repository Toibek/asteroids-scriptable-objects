using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestruction : MonoBehaviour
{
    [SerializeField] float destructionTime;
    [SerializeField] bool Fading;
    [SerializeField] AnimationCurve FadeCurve;
    SpriteRenderer sr;
    Color baseColor;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        StartCoroutine(Timer(destructionTime));
    }
    IEnumerator Timer(float time)
    {
        while (time > 0)
        {
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
            if (Fading) sr.color = new(baseColor.r, baseColor.g, baseColor.b, FadeCurve.Evaluate(time / destructionTime));
        }
        Destroy(gameObject);
    }
}
