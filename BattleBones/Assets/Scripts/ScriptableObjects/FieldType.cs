using UnityEngine;

[CreateAssetMenu(fileName = "New FieldType", menuName = "FieldType")]
public class FieldType : ScriptableObject
{
    public string fieldName;
    public int movementPointsCost;
    public bool isObstacle;
    public bool isBlockSight;
}
