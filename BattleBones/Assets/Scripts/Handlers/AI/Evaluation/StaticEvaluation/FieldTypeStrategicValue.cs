using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTypeStrategicValue
{
    public FieldValues Values;

    public FieldTypeStrategicValue(FieldValues values)
    {
        Values = values;
    }

    public int EvaluateFieldType(string fieldType)
    {
        return Values.fieldValues.Find(a => a.fieldName == fieldType).fieldValue;
    }
}
