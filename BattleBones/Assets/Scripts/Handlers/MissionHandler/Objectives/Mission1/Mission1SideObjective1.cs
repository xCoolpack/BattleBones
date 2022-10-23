using System.Linq;
using UnityEngine;

public class Mission1SideObjective1 : MonoBehaviour, IObjective
{
    [SerializeField] private Player _player;
    [SerializeField] private Resources _resources;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();
    
    public string ObjectiveInfo => $@"Current income: 
    Gold: {_player.ResourceManager.ResourcesIncome.Gold}/{_resources.Gold} 
    Wood: {_player.ResourceManager.ResourcesIncome.Wood}/{_resources.Wood} 
    Stone: {_player.ResourceManager.ResourcesIncome.Stone}/{_resources.Stone}";

    private bool CompletionCheck()
    {
        return _player.ResourceManager.ResourcesIncome >= _resources;
    }
}
