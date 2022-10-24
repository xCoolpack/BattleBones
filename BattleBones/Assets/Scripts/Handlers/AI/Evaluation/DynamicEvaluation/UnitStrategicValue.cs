using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStrategicValue
{
    UnitTypeValue UnitTypeValue;

    public UnitStrategicValue(UnitTypeValue unitTypeValue)
    {
        UnitTypeValue = unitTypeValue;
    }

    public int EvaluateUnit(Unit source)
    {
        int typeValue = UnitTypeValue.EvaluateUnitType(source.BaseUnitStats.UnitName);
        double healthModifier = Math.Max(0.5, source.CurrentHealth / source.BaseUnitStats.BaseHealth);

        return (int)(typeValue * healthModifier);
    }
}
