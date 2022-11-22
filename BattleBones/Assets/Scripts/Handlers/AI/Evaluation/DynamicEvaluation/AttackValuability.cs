using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackValuability
{
    public UnitStrategicValue UnitStrategicValue;
    public static int defaultAttackEval = 15;

    public AttackValuability(UnitStrategicValue unitStrategicValue)
    {
        UnitStrategicValue = unitStrategicValue;
    }

    public int EvaluateAttackValuability(Unit source, Field target, double fieldMod)
    {
        if (target.Unit is null)
            return defaultAttackEval;

        int sourceEval = (int) (UnitStrategicValue.EvaluateUnit(source) * 0.1);
        int targetEval = UnitStrategicValue.EvaluateUnit(target.Unit);
        int predictedDamage = source.PredictDamage(target.Unit);
        double damageMod = 1;

        if (predictedDamage / target.Unit.CurrentHealth > 0.30)
            damageMod = 1.5;

        if (predictedDamage / target.Unit.CurrentHealth > 1)
            damageMod = 2;

        targetEval = (int)(targetEval * damageMod);

        return (int)((targetEval - sourceEval) * fieldMod);
    }
}
