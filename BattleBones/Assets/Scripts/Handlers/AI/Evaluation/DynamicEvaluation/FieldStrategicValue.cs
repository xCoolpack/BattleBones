using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStrategicValue
{
    public FieldTypeStrategicValue FieldTypeStrategicValue;

    public FieldStrategicValue(FieldTypeStrategicValue fieldTypeStrategicValue)
    {
        FieldTypeStrategicValue = fieldTypeStrategicValue;
    }

    public double EvaluateFieldType(Field target)
    {
        return FieldTypeStrategicValue.EvaluateFieldType(target.Type.FieldName);
    }

    public double EvaluateField(Field target)
    {
        double fieldStrategicEval = EvaluateFieldType(target);

        if (target.Building != null
            && ((target.Building.BaseBuildingStats.BuildingName == "Defensive tower"
                ||  target.Building.BaseBuildingStats.BuildingName == "Barricade")
            && target.Building.GetBuildingStateName() == "Fine"))
        {
            fieldStrategicEval += 0.5;
        }

        return fieldStrategicEval;
    }
}
