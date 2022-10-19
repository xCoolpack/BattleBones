using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationEngine : MonoBehaviour
{
    public UnitRelatedEvaluation UnitRelatedEvaluation;
    public BuildingRelatedEvaluation BuildingRelatedEvaluation;

    public int Evaluate(string moveType, Object source, Object target)
    {
        return 0;
    }

    public int EvaluateUnitAttack(Unit source, Field target)
    {
        return 1;
    }

    public int EvaluateUnitMovement(Field target)
    {
        return 2;
    }

    public int EvaluateBuildingAttack(Building source, Field target)
    {
        return 3;
    }

    public int EvaluateRecruitment()
    {
        return 4;
    }

    public int EvaluateBuilding()
    {
        return 5;
    }
}
