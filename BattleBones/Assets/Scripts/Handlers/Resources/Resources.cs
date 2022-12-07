using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public static Resources operator *(Resources a, int b)
    {
        return new(a.Gold * b, a.Wood * b, a.Stone * b, a.Doggium * b, a.Bone * b);
    }

    public static Resources operator /(Resources a, int b)
    {
        return new(a.Gold / b, a.Wood / b, a.Stone / b, a.Doggium / b, a.Bone / b);
    }

    public static bool operator ==(Resources a, Resources b)
    {
        return (a.Gold == b.Gold && a.Wood == b.Wood && a.Stone == b.Stone && a.Doggium == b.Doggium && a.Bone == b.Bone);
    }

    public static bool operator !=(Resources a, Resources b)
    {
        return (a.Gold != b.Gold || a.Wood != b.Wood || a.Stone != b.Stone || a.Doggium != b.Doggium || a.Bone != b.Bone);
    }

    public override bool Equals(object obj)
    {
        return obj is Resources resources && this == resources ;
    }

    public override int GetHashCode()
    {
        return Gold.GetHashCode()+Wood.GetHashCode()+Stone.GetHashCode()+Doggium.GetHashCode()+Bone.GetHashCode();
    }

    public static bool operator >(Resources a, Resources b)
    {
        if (a.Gold != 0 || b.Gold != 0)
            if (a.Gold <= b.Gold)
                return false;

        if (a.Wood != 0 || b.Wood != 0)
            if (a.Wood <= b.Wood)
                return false;

        if (a.Stone != 0 || b.Stone != 0)
            if (a.Stone <= b.Stone)
                return false;

        if (a.Doggium != 0 || b.Doggium != 0)
            if (a.Doggium <= b.Doggium)
                return false;

        if (a.Bone != 0 || b.Bone != 0)
            if (a.Bone <= b.Bone)
                return false;

        return a != b;
    }

    public static bool operator <(Resources a, Resources b)
    {
        return b > a;
    }

    public static bool operator >=(Resources a, Resources b)
    {
        if (a.Gold != 0 || b.Gold != 0)
            if (a.Gold < b.Gold)
                return false;

        if (a.Wood != 0 || b.Wood != 0)
            if (a.Wood < b.Wood)
                return false;

        if (a.Stone != 0 || b.Stone != 0)
            if (a.Stone < b.Stone)
                return false;

        if (a.Doggium != 0 || b.Doggium != 0)
            if (a.Doggium < b.Doggium)
                return false;

        if (a.Bone != 0 || b.Bone != 0)
            if (a.Bone < b.Bone)
                return false;

        return true;
    }

    public static bool operator <=(Resources a, Resources b)
    {
        return b >= a;
    }
}
