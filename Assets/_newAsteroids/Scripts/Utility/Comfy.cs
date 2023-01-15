using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Comfy
{
    public static bool InRange(float val, float min, float max)
    {
        return min < val && val < max;
    }
    public static bool InRange(Vector2 val, Vector2 min, Vector2 max)
    {
        return min.x < val.x && val.x < max.y && min.y < val.y && val.y < max.y;
    }
}
