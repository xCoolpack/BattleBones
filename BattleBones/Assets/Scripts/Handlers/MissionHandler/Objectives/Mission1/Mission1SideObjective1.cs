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
    
    public string ObjectiveInfo => $@"Current income: 
    Gold: {_player.ResourceManager.ResourcesIncome.Gold}/50 
    Wood: {_player.ResourceManager.ResourcesIncome.Wood}/100 
    Stone: {_player.ResourceManager.ResourcesIncome.Stone}/100";

    private bool CompletionCheck()
    {
        return _player.ResourceManager.ResourcesIncome > new Resources(50, 100, 100);
    }
}
