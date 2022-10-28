using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRelatedEvaluation : MonoBehaviour
{
    public FieldEconomicValue FieldEconomicValue;
    public UnitStrategicValue UnitStrategicValue;

    public CustomEvaluationCharacteristics CustomEval;

    void Awake()
    {
        UnitStrategicValues usv = EvaluationEngine.LoadAssetByName<UnitStrategicValues>("UnitTypeValues");
        FieldValues fev = EvaluationEngine.LoadAssetByName<FieldValues>("FieldEconomicValues");

        UnitStrategicValue = new UnitStrategicValue(new UnitTypeValue(usv));
        FieldEconomicValue = new FieldEconomicValue(fev);
    }

    public int EvaluateBuildingAttack(Field target)
    {
        return 0;
    }

    public int Evaluate(string moveType, Object source, Object target)
    {
        int eval = 0;

        switch (moveType)
        {
            case "buildingAttack":
                eval = AttackEval(target as Field);
                break;

            case "recruitment":
                eval = RecruitmentEval(target as Unit);
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
        //TO-DO
        return 0;
    }

    public int RecruitmentEval(Unit target)
    {
        //TO-DO
        return 0;
    }

    public int ConstructionEval(Building source, Field target)
    {
        //TO-DO
        return 0;
    }
}
