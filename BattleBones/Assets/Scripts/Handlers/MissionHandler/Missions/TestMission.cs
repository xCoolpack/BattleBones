using UnityEngine;

public class TestMission : MonoBehaviour, IMission
{
    public Player Player;

    /// <summary>
    /// Mission for 10 gold amount
    /// </summary>
    /// <returns></returns>
    public bool Check()
    {
        return Player.ResourceManager.ResourcesAmount.Gold > 10;
    }
}