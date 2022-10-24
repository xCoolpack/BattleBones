using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDanger
{
    public UnitStrategicValue UnitStrategicValue;
    public static int defaultAttackEval = 30;

    public AttackDanger(UnitStrategicValue unitStrategicValue)
    {
        UnitStrategicValue = unitStrategicValue;
    }

    public int EvaluateAttackDanger(Unit source, Field target, double fieldMod)
    {
        if (target.Unit is null)
            return defaultAttackEval;

        int sourceEval = (int)(UnitStrategicValue.EvaluateUnit(source) * 0.6);
        int targetEval = (int)(UnitStrategicValue.EvaluateUnit(target.Unit) * 0.2);
        int predictedDamage = target.Unit.PredictDamage(source);
        double damageMod = 1;

        if (predictedDamage / target.Unit.CurrentHealth > 0.30)
            damageMod = 1.2;

        if (predictedDamage / target.Unit.CurrentHealth > 1)
            damageMod = 1.7;

        sourceEval = (int)(targetEval * damageMod);

        return (int)((targetEval + sourceEval) * fieldMod);
    }
}
