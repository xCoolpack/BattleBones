using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDanger
{
    public UnitStrategicValue UnitStrategicValue;

    public AttackDanger(UnitStrategicValue unitStrategicValue)
    {
        UnitStrategicValue = unitStrategicValue;
    }

    public int EvaluateAttackDanger(Unit source, Field target, int unitValue, int fieldValue)
    {
        //TO-DO (field might not have a unit)
        return 0;
    }
}
