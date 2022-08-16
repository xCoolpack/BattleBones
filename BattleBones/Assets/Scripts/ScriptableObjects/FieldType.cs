using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FieldType", menuName = "FieldType")]
public class FieldType : ScriptableObject
{
    public string fieldName;
    public float movementPointsCost;
    public bool isObstacle;
    public bool isBlockSight;
}
