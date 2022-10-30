using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMovement : Movement
{
    public int ForestCost;
    public int HillsCost;

    public override int GetMovementPointsCost(Unit unit, Field field)
    {
        return field.Type.FieldName switch
        {
            "Forest" => ForestCost,
            "Wood" => ForestCost,
            "Hills" => HillsCost,
            "StoneHills" => HillsCost,
            "GoldHills" => HillsCost,
            "DoggiumHills" => HillsCost,
            _ => field.GetMovementPointsCost()
        };
    }

    public override bool IsBlockingSight(Field field)
    {
        return field.Type.FieldName switch
        {
            "Forest" => false,
            "Wood" => false,
            "Hills" => false,
            "StoneHills" => false,
            "GoldHills" => false,
            "DoggiumHills" => false,
            _ => field.Type.IsBlockingSight
        };
    }
}
