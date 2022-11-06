using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public bool CanAttack(Unit unit, Field startingField, Field targetField)
    {
        return targetField.IsVisibleFor(unit, startingField) && !targetField.IsObstacle() && targetField.IsSeenBy(unit.Player);
    }

    public bool CanAttack(Building building, Field startingField, Field targetField)
    {
        return targetField.IsVisibleFor(building, startingField) && !targetField.IsObstacle() && targetField.IsSeenBy(building.Player);
    }

    public bool CanTarget(Unit unit, Field field)
    {
        return (field.HasUnit() && field.Unit.IsEnemy(unit.Player)) 
               || CanTargetBuilding(unit, field);
    }

    public bool CanTargetBuilding(Unit unit, Field field)
    {
        return (field.HasBuilding() && field.Building.CanBeTargeted(unit.Player));
    }

    public bool CanTarget(Building building, Field field)
    {
        return (field.HasUnit() && field.Unit.IsEnemy(building.Player)) 
               || CanTargetBuilding(building, field);
    }

    public bool CanTargetBuilding(Building building, Field field)
    {
        return (field.HasBuilding() && field.Building.CanBeTargeted(building.Player));
    }

    public abstract bool HaveEnoughMovementPoints(int currentMovementPoints, Unit unit, Field field);

    public abstract bool IsProvokingCounterAttack();

    public abstract int GetCounterAttackModifier();
}
