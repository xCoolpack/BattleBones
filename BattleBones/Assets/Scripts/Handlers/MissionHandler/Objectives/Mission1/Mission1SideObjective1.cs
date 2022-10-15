using System.Linq;
using UnityEngine;

public class Mission1SideObjective1 : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _player;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo =>
        $@"Current income: \t {_player.ResourceManager.ResourcesIncome.Gold}/150 
        \t{_player.ResourceManager.ResourcesIncome.Wood}/200 
        \t{_player.ResourceManager.ResourcesIncome.Stone}/200";

    private bool CompletionCheck()
    {
        return _player.ResourceManager.ResourcesIncome > new Resources(150, 200, 200);
    }
}
