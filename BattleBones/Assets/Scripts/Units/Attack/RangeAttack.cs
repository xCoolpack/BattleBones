using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeAttack : Attack
{
    public override bool HaveEnoughMovementpoints(Unit unit, Field field)
    {
        return unit.CurrentMovementPoints > 0;
    }
}
