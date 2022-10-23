public class MeleeAttack : Attack
{
    public override bool HaveEnoughMovementPoints(int currentMovementPoints, Unit unit, Field field)
    {
        return currentMovementPoints >= unit.MovementScript.GetMovementPointsCost(unit, field);
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
