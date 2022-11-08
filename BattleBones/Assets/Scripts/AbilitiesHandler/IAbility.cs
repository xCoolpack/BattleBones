using UnityEngine;

public interface IAbility
{
    public string Id { get; set; }

    public string Name { get; set; }
    public Sprite Sprite { get; set; }

    public string Description { get; }

    public double Modifier { get; set; }

    public Resources Cost { get; set; }

    public bool CanUse(Field targetField, Player targetPlayer, Player castingPlayer);

    public void Use(Field targetField, Player targetPlayer, Player castingPlayer);
}
