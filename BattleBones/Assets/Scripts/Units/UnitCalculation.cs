using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class UnitCalculation
{
    public const int DmgLossFromHealth = 40;
    public const int MaxDefense = 75;

    /// <summary>
    /// Method calculating damage modifier from missing health
    /// </summary>
    /// <param name="currentHealth"></param>
    /// <param name="maxHealth"></param>
    /// <returns></returns>
    public static int CalculateDamageModifier(int currentHealth, int maxHealth)
    {
        return (int)Math.Ceiling(DmgLossFromHealth * (1.0 - (double)currentHealth / maxHealth)) * -1;
    }

    /// <summary>
    /// Method calculating dealt damaged not blocked by defense
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="defense"></param>
    /// <returns></returns>
    public static int CalculateDealtDamage(int damage, int defense)
    {
        return (int)Math.Ceiling(damage * (1.0 - Math.Min(defense, MaxDefense) / 100.0));
    }
}

