using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRelatedEvaluation : MonoBehaviour
{
    public FieldEconomicValue FieldEconomicValue;
    public UnitStrategicValue UnitStrategicValue;
    public UnitCostValue UnitCostValue;
    public UnitRoleValue UnitRoleValue;

    public CustomEvaluationCharacteristics CustomEval;

    void Awake()
    {
        UnitStrategicValues usv = EvaluationEngine.LoadAssetByName<UnitStrategicValues>("UnitTypeValues");
        FieldValues fev = EvaluationEngine.LoadAssetByName<FieldValues>("FieldEconomicValues");

        UnitStrategicValue = new UnitStrategicValue(new UnitTypeValue(usv));
        FieldEconomicValue = new FieldEconomicValue(fev);
        UnitCostValue = new UnitCostValue();
        UnitRoleValue = new UnitRoleValue();
    }

    public int Evaluate(string moveType, UnityEngine.Object source, UnityEngine.Object target)
    {
        int eval = 0;

        switch (moveType)
        {
            case "buildingAttack":
                eval = AttackEval(target as Field);
                break;

            case "recruitment":
                eval = RecruitmentEval(source as Player, target as Unit);
                break;

            case "construction":
                eval = ConstructionEval(source as Building, target as Field);
                break;

            default:
                break;
        }

        //TO-DO: customEval
        return eval;
    }

    public int AttackEval(Field target)
    {
        if (target.Building is not null)
        {
            return 0;
        }

        return UnitStrategicValue.EvaluateUnit(target.Unit);
    }

    public int RecruitmentEval(Player source, Unit target)
    {
        double usv = UnitStrategicValue.EvaluateUnit(target) * 0.5;
        double urv = UnitRoleValue.EvaluateUnitRoleValue(target, source);
        double ucv = UnitCostValue.EvaluateUnitCostValue(target);

        return (int) Math.Ceiling(usv * urv / ucv);
    }

    public int ConstructionEval(Building source, Field target)
    {
        //TO-DO
        return 0;
    }
}
