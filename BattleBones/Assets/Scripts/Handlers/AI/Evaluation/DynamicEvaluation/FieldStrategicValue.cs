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

    public int EvaluateFieldType(Field target)
    {
        return FieldTypeStrategicValue.EvaluateFieldType(target.Type.FieldName);
    }

    public int EvaluateField(Field target)
    {
        int fieldStrategicEval = EvaluateFieldType(target);

        if (target.Building != null
            && (target.Building.BaseBuildingStats.BuildingName == "Defensive tower"
            ||  target.Building.BaseBuildingStats.BuildingName == "Barricade"))
        {
            fieldStrategicEval = (int)(1.2 * fieldStrategicEval);
        }

        return fieldStrategicEval;
    }
}
