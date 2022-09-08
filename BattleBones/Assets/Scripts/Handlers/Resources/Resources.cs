[System.Serializable]
public struct Resources
{
    public int Gold;
    public int Wood;
    public int Stone;
    public int Doggium;
    public int Bone;

    public Resources(int gold = 0, int wood = 0, int stone = 0, int doggium = 0, int bone = 0)
    {
        Gold = gold;
        Wood = wood;
        Stone = stone;
        Doggium = doggium;
        Bone = bone;
    }

    public static Resources operator -(Resources a)
    {
        return new(-a.Gold, -a.Wood, -a.Stone, -a.Doggium, -a.Bone);
    }

    public static Resources operator +(Resources a, Resources b)
    {
        return new(a.Gold + b.Gold, a.Wood + b.Wood, a.Stone + b.Stone, a.Doggium + b.Doggium, a.Bone + b.Bone);
    }

    public static Resources operator -(Resources a, Resources b)
    {
        return a + (-b);
    }
}
