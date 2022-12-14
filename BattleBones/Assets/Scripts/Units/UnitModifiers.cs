using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitModifiers
{
    public double Health;
    public double Damage;
    public double Defense;

    public UnitModifiers(int health = 0, int damage = 0, int defense = 0)
    {
        Health = Math.Round((double)health / 100, 2);
        Damage = Math.Round((double)damage / 100, 2);
        Defense = Math.Round((double)defense / 100, 2);
    }

    public UnitModifiers(double health = 0, double damage = 0, double defense = 0)
    {
        Health = health;
        Damage = damage;
        Defense = defense;
    }

    public static UnitModifiers operator -(UnitModifiers a)
    {
        return new(-a.Health, -a.Damage, -a.Defense);
    }

    public static UnitModifiers operator +(UnitModifiers a, UnitModifiers b)
    {
        return new(a.Health +b.Health, a.Damage+b.Damage, a.Defense+b.Defense);
    }

    public static UnitModifiers operator -(UnitModifiers a, UnitModifiers b)
    {
        return a + (-b);
    }

    public (int Health, int Damage, int Defense) CalculateModifiers(int health, int damage, int defense)
    {
        return (AddModifier(health, Health), AddModifier(damage, Damage), AddModifier(defense, Defense));
    }

    public (int Health, int Damage, int Defense) CalculateModifiers(Unit unit)
    {
        return CalculateModifiers(unit.BaseUnitStats.BaseHealth, unit.BaseUnitStats.BaseDamage, unit.BaseUnitStats.BaseDefense);
    }

    private static int AddModifier(int a, double b)
    {
        return (int)Math.Ceiling(a * (1 + b));
    }

    public override string ToString()
    {
        return $"Health: {Health*100}%, Damage: {Damage*100}%, Defense: {Defense*100}%";
    }
}
