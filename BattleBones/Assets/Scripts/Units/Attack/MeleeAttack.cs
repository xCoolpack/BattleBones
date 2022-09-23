public class MeleeAttack : Attack
{
    public override bool HaveEnoughMovementPoints(int currentMovementPoints, Unit unit = null, Field field = null)
    {
        return currentMovementPoints > unit.MovementScript.GetMovementPointsCostForUnit(unit, field);
    }

    public override bool IsProvokingCounterAttack()
    {
        return true;
    }

    public override int GetCounterAttackModifier()
    {
        return 0;
    }
}
