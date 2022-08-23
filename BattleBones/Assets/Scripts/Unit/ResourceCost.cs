[System.Serializable]
public struct ResourceCost
{
    public int GoldCost;
    public int WoodCost;
    public int StoneCost;
    public int DoggiumCost;

    public ResourceCost(int goldCost, int woodCost, int stoneCost, int doggiumCost)
    {
        this.GoldCost = goldCost;
        this.WoodCost = woodCost;
        this.StoneCost = stoneCost;
        this.DoggiumCost = doggiumCost;
    }
}
