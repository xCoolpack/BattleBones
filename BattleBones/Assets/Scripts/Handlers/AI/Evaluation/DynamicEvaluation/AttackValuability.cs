using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackValuability
{
    public UnitStrategicValue UnitStrategicValue;

    public AttackValuability(UnitStrategicValue unitStrategicValue)
    {
        UnitStrategicValue = unitStrategicValue;
    }

    public int EvaluateAttackValuability(Unit source, Field target, int unitValue, int fieldValue)
    {
        //TO-DO (field might not have a unit)
        return 0;
    }
}
