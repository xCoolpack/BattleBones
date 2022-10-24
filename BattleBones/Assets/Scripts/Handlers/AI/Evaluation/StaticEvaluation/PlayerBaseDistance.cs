using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseDistance
{
    public int EvaluateDistance(Field source, Field target)
    {
        // this returns 1-100, grows the closer you get to the target
        int distanceMod = (int) Math.Ceiling(100f / GraphSearch.GetDistance(source, target));
        int baseMultiplier = 5;

        // 5-500
        return distanceMod * baseMultiplier;
    }
}
