using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public abstract int GetMovementPointsCostForUnit(Field field);
}