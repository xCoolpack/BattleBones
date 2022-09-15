using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public abstract int GetMovementPointsCostForUnit(Unit unit, Field field);

    public bool CanMove(Unit unit, Field field) 
    {
        return !field.IsObstacle() && !field.HasUnit() && (!field.HasBuilding() || !field.Building.IsEnemy(unit.Player));
    }
}