using UnityEngine.XR;

[System.Serializable]
public struct UnitModifiers
{
    public int Health;
    public int Damage;
    public int Defense;

    public UnitModifiers(int health = 0, int damage = 0, int defense = 0)
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
        return new(a.Health +b.Health, a.Damage+b.Damage, a.Defense+a.Defense);
    }

    public static UnitModifiers operator -(UnitModifiers a, UnitModifiers b)
    {
        return a + (-b);
    }
}
