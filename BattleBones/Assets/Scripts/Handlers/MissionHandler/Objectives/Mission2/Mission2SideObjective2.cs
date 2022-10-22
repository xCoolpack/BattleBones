using System.Linq;
using UnityEngine;

public class Mission2SideObjective2 : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _enemyPlayer;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Lost {_enemyPlayer.UnitsKilled}/8 units";

    private bool CompletionCheck()
    {
        return _enemyPlayer.UnitsKilled <= 8;
    }
}
