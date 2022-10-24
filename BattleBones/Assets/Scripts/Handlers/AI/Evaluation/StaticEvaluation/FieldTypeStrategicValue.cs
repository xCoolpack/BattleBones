using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldTypeStrategicValue
{
    public FieldValues Values;
    public static int defaultEval = 1;

    public FieldTypeStrategicValue(FieldValues values)
    {
        Values = values;
    }

    public double EvaluateFieldType(string fieldType)
    {
        FieldValueRecord queriedValue = Values.fieldValues.SingleOrDefault(a => a.fieldName == fieldType);

        return queriedValue.fieldValue is not 0
            ? queriedValue.fieldValue
            : defaultEval;
    }
}
