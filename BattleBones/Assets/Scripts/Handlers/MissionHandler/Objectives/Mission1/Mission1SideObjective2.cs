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
    public string ObjectiveInfo { get; set; }

    private bool CompletionCheck()
    {
        ObjectiveInfo = $"Current units {_player.Units.Count}/10";
        return _player.Units.Count >= 10;
    }
}
