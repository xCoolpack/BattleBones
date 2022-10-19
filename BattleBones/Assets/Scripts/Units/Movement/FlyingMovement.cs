using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMovement : Movement
{
    public int ForestCost;
    public int HillsCost;

    public override int GetMovementPointsCost(Unit unit, Field field)
    {
        switch (field.Type.FieldName)
        {
            case "Forest":
                return ForestCost;
            case "Hills":
                return HillsCost;
            default:
                return field.GetMovementPointsCost();
        }
    }

    public override bool IsBlockingSight(Field field)
    {
        switch (field.Type.FieldName)
        {
            case "Forest":
                return false;
            case "Hills":
                return false;
            default:
                return field.Type.IsBlockingSight;
        }
    }
}
