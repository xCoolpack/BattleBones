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
        //SetAttackableFields();
        //ToggleAttackableFields();
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

    public void SetAttackableFields()
    {
        (FieldsWithinAttackRange, AttackableFields) = GetAttackableFields();
    }

    private (List<Field> FieldsWithinAttackRange, List<Field> AttackableFields) GetAttackableFields()
    {
        List<Field> fieldsWithinAttackRange = new();
        List<Field> attackableFields = new();
        //Attack range is always smaller or equal to sight range
        fieldsWithinAttackRange.AddRange(GraphSearch.BreadthFirstSearchList(_building.Field, AttackRange,
            (currentField, startingField) => _attack.CanAttack(_building, _building.Field, currentField), _ => 1));

        foreach (Field field in fieldsWithinAttackRange)
        {
            if (_attack.CanTarget(_building, field))
                attackableFields.Add(field);
        }

        return (fieldsWithinAttackRange, attackableFields);
    }

    public void Attack(Field targetField)
    {
        Logger.Log($"{_building.Player.name} has ordered {_building.BaseBuildingStats.BuildingName} " +
                   $"at {_building.Field.ThreeAxisCoordinates} " +
                   $"to attack {targetField.Unit?.BaseUnitStats.UnitName} " +
                   $"at {targetField.ThreeAxisCoordinates}");

        if (targetField == _building.Field)
        {
            Logger.Log($"{_building.Player.name}'s {_building.BaseBuildingStats.BuildingName} is built at {targetField.ThreeAxisCoordinates}");
            return;
        }

        if (AttackableFields.Contains(targetField))
        {
            if (_attack.CanTargetBuilding(_building, targetField))
            {
                Logger.Log($"{_building.Player.name} has ordered {_building.BaseBuildingStats.BuildingName} " +
                           $"at {_building.Field.ThreeAxisCoordinates} " +
                           $"to attack {targetField.Building.BaseBuildingStats.BuildingName} " +
                           $"at {targetField.ThreeAxisCoordinates}");
                DealDamage(targetField.Building);
                
            }
            else
            {
                Logger.Log($"{_building.Player.name} has ordered {_building.BaseBuildingStats.BuildingName} " +
                           $"at {_building.Field.ThreeAxisCoordinates} " +
                           $"to attack {targetField.Unit.BaseUnitStats.UnitName} " +
                           $"at {targetField.ThreeAxisCoordinates}");
                DealDamage(targetField.Unit);
            }
        }
    }

    private void DealDamage(Building building)
    {
        building.TakeDamage(CurrentDamage);
    }

    private void DealDamage(Unit unit)
    {
        unit.TakeDamage(CurrentDamage);
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
}
