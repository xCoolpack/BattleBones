[System.Serializable]
public struct ResourceCost
{
    public int goldCost;
    public int woodCost;
    public int stoneCost;
    public int doggiumCost;

    public ResourceCost(int goldCost, int woodCost, int stoneCost, int doggiumCost)
    {
        this.goldCost = goldCost;
        this.woodCost = woodCost;
        this.stoneCost = stoneCost;
        this.doggiumCost = doggiumCost;
    }
}
