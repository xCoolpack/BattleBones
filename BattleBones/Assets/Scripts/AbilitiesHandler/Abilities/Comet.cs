using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour, IAbility
{
    [SerializeField]
    private int _damage;

    [field: SerializeField]
    public string Id { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public Sprite Sprite { get; set; }

    public string Description => $"Summon comet that deal {(int)Math.Ceiling(_damage * Modifier)} defense ignoring damage to unit at field";

    [field: SerializeField]
    public double Modifier { get; set; }

    [field: SerializeField]
    public Resources Cost { get; set; }

    public bool CanUse(Field targetField, Player targetPlayer, Player castingPlayer)
    {
        return targetField.HasUnit() && targetField.Unit.IsEnemy(castingPlayer);
    }

    public void Use(Field targetField, Player targetPlayer, Player castingPlayer)
    {
        targetField.Unit.TakeDamageWithoutDef((int)Math.Ceiling(_damage * Modifier));
    }

}
