using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public bool CanAttack(Unit unit, Field startingField, Field targetField)
    {
        //Debug.Log(targetField.Coordinates);
        return targetField.IsVisibleFor(startingField) && !targetField.IsObstacle() && HaveEnoughMovementpoints(unit, targetField);
    }

    public bool CanAttack(Field startingField, Field targetField)
    {
        //Debug.Log(targetField.Coordinates);
        return targetField.IsVisibleFor(startingField) && !targetField.IsObstacle();
    }

    public bool CanTarget(Unit unit, Field field)
    {
        return (field.HasUnit() && field.Unit.IsEnemy(unit.Player)) || (field.HasBuilding() && field.Building.IsEnemy(unit.Player));
    }

    public bool CanTarget(Building building, Field field)
    {
        return (field.HasUnit() && field.Unit.IsEnemy(building.Player)) || (field.HasBuilding() && field.Building.IsEnemy(building.Player));
    }

    public abstract bool HaveEnoughMovementpoints(Unit unit, Field field);
}
