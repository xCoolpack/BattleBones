using UnityEngine;

public class TestObjective : MonoBehaviour, IObjective
{
    public Player Player;

    [field: SerializeField]
    public bool IsPrimary { get; set; } = true;

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    [field: SerializeField]
    public string ObjectiveInfo { get; set; }

    /// <summary>
    /// Mission for 10 gold amount
    /// </summary>
    /// <returns></returns>
    private bool CompletionCheck()
    {
        return Player.ResourceManager.ResourcesAmount.Gold > 10;
    }
}
