using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefensiveBuilding : MonoBehaviour
{
    // Max stats
    public int MaxDamage;

    // Current stats
    public int CurrentDamage;
    public int AttackRange;

    public List<Field> FieldsWithinAttackRange;
    public List<Field> AttackableFields;

    private Building _building;
    private RangeAttack _attack;

    private void Awake()
    {
        _building = GetComponent<Building>();
        _attack = gameObject.AddComponent<RangeAttack>();
        SetStats();
    }

    private void OnMouseDown()
    {
        SetAttackableFields();
        ToggleAttackableFields();
    }

    /// <summary>
    /// Methods setting unit stats from BaseUnitStats object
    /// </summary>
    private void SetStats()
    {
        MaxDamage = _building.BaseBuildingStats.BaseDamage;
        CurrentDamage = MaxDamage;
        AttackRange = _building.BaseBuildingStats.BaseAttackRange;
    }

    #region AttackableFields
    private void SetAttackableFields()
    {
        (FieldsWithinAttackRange, AttackableFields) = GetAttackableFields();
    }

    public (List<Field> FieldsWithinAttackRange, List<Field> AttackableFields) GetAttackableFields()
    {
        List<Field> fieldsWithinAttackRange = new();
        List<Field> attackableFields = new();
        //Attack range is always smaller or equal to sight range
        fieldsWithinAttackRange.AddRange(GraphSearch.BreadthFirstSearchList(_building.Field, AttackRange,
            (currentField, startingField) => _attack.CanAttack(_building.Field, currentField), _ => 1));

        foreach (Field field in fieldsWithinAttackRange)
        {
            if (_attack.CanTarget(_building, field))
                attackableFields.Add(field);
        }

        return (fieldsWithinAttackRange, attackableFields);
    }

    private void ToggleFieldsWithinAttackRange()
    {
        foreach (var field in FieldsWithinAttackRange)
        {
            var mark = field.transform.Find("Mark").gameObject;
            mark.SetActive(!mark.activeSelf);
        }
    }

    private void ToggleAttackableFields()
    {
        foreach (var field in AttackableFields)
        {
            var mark = field.transform.Find("Mark").gameObject;
            mark.SetActive(!mark.activeSelf);
        }
    }
    #endregion
}
