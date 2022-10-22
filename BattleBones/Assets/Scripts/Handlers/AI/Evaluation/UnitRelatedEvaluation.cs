using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnitRelatedEvaluation : MonoBehaviour
{
    public FieldStrategicValue FieldStrategicValue;
    public UnitStrategicValue UnitStrategicValue;
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
        AttackDanger = new AttackDanger();
        AttackValuability = new AttackValuability();
    }
}
