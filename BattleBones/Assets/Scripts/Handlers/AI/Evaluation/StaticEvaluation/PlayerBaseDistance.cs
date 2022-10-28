using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseDistance
{
    public int EvaluateDistance(Field source, Field target)
    {
        int distanceMod = -GraphSearch.GetDistance(source, target);
        int baseMultiplier = 2;

        return distanceMod * baseMultiplier;
    }
}
