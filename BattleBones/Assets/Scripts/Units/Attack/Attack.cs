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

    public bool CanTarget(Unit unit, Field field)
    {
        return (field.HasUnit() && field.Unit.IsEnemy(unit.Player)) || (field.HasBuidling() && field.Building.IsEnemy(unit.Player));
    }

    public abstract bool HaveEnoughMovementpoints(Unit unit, Field field);
}
