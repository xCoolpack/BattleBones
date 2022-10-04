public class RangeAttack : Attack
{
    public override bool HaveEnoughMovementPoints(int currentMovementPoints, Unit unit, Field field)
    {
        return currentMovementPoints > 0;
    }

    public override bool IsProvokingCounterAttack()
    {
        return false;
    }

    public override int GetCounterAttackModifier()
    {
        return -50;
    }
}
