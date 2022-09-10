using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMovement : Movement
{
    public override int GetMovementPointsCostForUnit(Unit unit, Field field)
    {
        return field.GetMovementPointsCost();
    }
}
