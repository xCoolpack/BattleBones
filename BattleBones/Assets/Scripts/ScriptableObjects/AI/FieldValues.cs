using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FieldValues", menuName = "FieldValues")]
public class FieldValues : ScriptableObject
{
    public List<FieldValueRecord> fieldValues;
}
