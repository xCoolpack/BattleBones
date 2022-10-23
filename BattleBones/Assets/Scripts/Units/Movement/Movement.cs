using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Movement : MonoBehaviour
{
    public abstract int GetMovementPointsCost(Unit unit, Field field);

    public abstract bool IsBlockingSight(Field field);

    public int GetMovementPointsCostAll(Unit unit, Field field)
    {
        return field.IsSeenBy(unit.Player) ? GetMovementPointsCost(unit, field) : 1;
    }

    public bool CanMove(Unit unit, Field field)
    {
        return field.IsSeenBy(unit.Player) && CanMoveVisible(unit, field);
    }

    public bool CanMoveVisible(Unit unit, Field field)
    {
        return !field.IsObstacle() && !field.HasUnit() && (!field.HasBuilding() || field.Building.IsPassable(unit.Player));
    }

    public bool CanMoveAll(Unit unit, Field field)
    {
        return !field.IsSeenBy(unit.Player) || CanMove(unit, field);
    }
}
