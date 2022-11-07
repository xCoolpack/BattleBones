using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSpirit: MonoBehaviour, IAbility
{
    [SerializeField] private double _bonus;
    [SerializeField] private int _turnsDuration;

    [field: SerializeField]
    public string Id { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public Sprite Sprite { get; set; }

    public string Description => $"Chosen unit gain {_bonus*Modifier*100}% more health and damage for {_turnsDuration} turns";

    [field: SerializeField]
    public double Modifier { get; set; }

    [field: SerializeField]
    public Resources Cost { get; set; }

    public bool CanUse(Field targetField, Player targetPlayer, Player castingPlayer)
    {
        return targetField.HasUnit() && !targetField.Unit.IsEnemy(castingPlayer);
    }

    public void Use(Field targetField, Player targetPlayer, Player castingPlayer)
    {
        targetField.Unit.AddUnitModifiers(new UnitModifiers(_bonus, _bonus));

        castingPlayer.PlayerEventHandler.AddStartTurnEvent(new GameEvent(_turnsDuration, () =>
        {
            targetField.Unit.RemoveUnitModifiers(new UnitModifiers(_bonus, _bonus));
        }));
    }

}
