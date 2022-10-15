using System.Linq;
using UnityEngine;

public class Mission1SideObjective2 : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _player;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    [field: SerializeField]
    public string ObjectiveInfo => $"Current units {_player.Units.Count}/10";

    private bool CompletionCheck()
    {
        return _player.Units.Count >= 10;
    }
}
