using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Mission2PrimaryObjective : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _player;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $@"Current resources amount: 
    Gold: {_player.ResourceManager.ResourcesAmount.Gold}/3600 
    Wood: {_player.ResourceManager.ResourcesAmount.Wood}/2000 
    Stone: {_player.ResourceManager.ResourcesAmount.Stone}/1600";

    private bool CompletionCheck()
    {
        return _player.ResourceManager.ResourcesAmount >= new Resources(3600, 2000, 1600);
    }
}
