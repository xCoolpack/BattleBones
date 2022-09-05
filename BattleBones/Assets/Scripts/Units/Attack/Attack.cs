using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public abstract List<Field> GetAttackableFields(Unit unit, Field startingField);
}
