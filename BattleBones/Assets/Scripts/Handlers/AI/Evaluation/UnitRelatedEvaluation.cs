using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnitRelatedEvaluation : MonoBehaviour
{
    public FieldStrategicValue FieldStrategicValue;
    public UnitStrategicValue UnitStrategicValue;
    public PlayerBaseDistance PlayerBaseDistance;
    public AttackDanger AttackDanger;
    public AttackValuability AttackValuability;

    public CustomEvaluationCharacteristics CustomEval;

    void Awake()
    {
        FieldTypeStrategicValue ftsv = new FieldTypeStrategicValue
            (EvaluationEngine.LoadAssetByName<FieldValues>("FieldTypeStrategicValues"));
        UnitStrategicValues usv = EvaluationEngine.LoadAssetByName<UnitStrategicValues>("UnitTypeValues");

        FieldStrategicValue = new FieldStrategicValue(ftsv);
        UnitStrategicValue = new UnitStrategicValue(new UnitTypeValue(usv));
        AttackDanger = new AttackDanger(UnitStrategicValue);
        AttackValuability = new AttackValuability(UnitStrategicValue);
        PlayerBaseDistance = new PlayerBaseDistance();
    }

    public int Evaluate(string moveType, Object source, Object target)
    {
        int eval = 0;

        switch (moveType)
        {
            case "unitAttack":
                eval = AttackEval(source as Unit, target as Field);
                break;

            case "move":
                eval = MovementEval(source as Unit, target as Field);
                break;

            default:
                break;
        }

        Debug.Log($"{moveType} - eval={eval}");
        return CustomEval.ProcessEvaluation(moveType, source, target, eval);
    }

    public double FieldEval(Field target)
    {
        return FieldStrategicValue.EvaluateField(target);
    }

    public int AttackEval(Unit source, Field target)
    {
        double fieldEval = FieldEval(target);
        int attackDanger = AttackDanger.EvaluateAttackDanger(source, target, fieldEval);
        int attackValuability = AttackValuability.EvaluateAttackValuability(source, target, fieldEval);
        
        return attackValuability - attackDanger;
    }

    public int MovementEval(Unit source, Field target)
    {
        double fieldEval = FieldEval(target);
        fieldEval += PlayerBaseDistance.EvaluateDistance(target);
        fieldEval *= 10;

        return (int) fieldEval;
    }
}
