using System.Linq;
using UnityEngine;

public class Mission1SideObjective1 : MonoBehaviour, IObjective
{
    [SerializeField]
    private Player _player;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    public bool IsComplited => CompletionCheck();

    [field: SerializeField]
    public string ObjectiveInfo { get; set; }

    private bool CompletionCheck()
    {
        return _player.ResourceManager.ResourcesIncome > new Resources(150, 200, 200);
    }
}
