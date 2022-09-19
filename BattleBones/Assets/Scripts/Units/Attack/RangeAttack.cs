using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeAttack : Attack
{
    public override bool HaveEnoughMovementPoints(int currentMovementPoints, Unit unit = null, Field field = null)
    {
        return currentMovementPoints > 0;
    }
}
