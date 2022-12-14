using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassHeal : MonoBehaviour, IAbility
{
    [SerializeField]
    private double _healRatio;

    [field: SerializeField]
    public string Id { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public Sprite Sprite { get; set; }

    public string Description => $"Heal every owned unit for {_healRatio * Modifier * 100}% of itself health";

    [field: SerializeField]
    public double Modifier { get; set; }

    [field: SerializeField]
    public Resources Cost { get; set; }

    public bool CanUse(Field targetField, Player targetPlayer, Player castingPlayer)
    {
        return true;
    }

    public void Use(Field targetField, Player targetPlayer, Player castingPlayer)
    {
        castingPlayer.Units.ForEach(unit => unit.Heal(_healRatio * Modifier));
    }

}
