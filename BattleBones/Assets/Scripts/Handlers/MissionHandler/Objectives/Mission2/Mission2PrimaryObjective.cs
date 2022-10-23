using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Mission2PrimaryObjective : MonoBehaviour, IObjective
{
    [SerializeField] private Player _player;
    [SerializeField] private Resources _resources;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $@"Current resources amount: 
    Gold: {_player.ResourceManager.ResourcesAmount.Gold}/{_resources.Gold} 
    Wood: {_player.ResourceManager.ResourcesAmount.Wood}/{_resources.Wood} 
    Stone: {_player.ResourceManager.ResourcesAmount.Stone}/{_resources.Stone}";

    private bool CompletionCheck()
    {
        return _player.ResourceManager.ResourcesAmount >= _resources;
    }
}
