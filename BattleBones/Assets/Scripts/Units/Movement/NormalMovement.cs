using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMovement : Movement
{
    public override int GetMovementPointsCost(Unit unit, Field field)
    {
        return field.GetMovementPointsCost();
    }

    public override bool IsBlockingSight(Field field)
    {
        return field.Type.IsBlockingSight;
    }
}
