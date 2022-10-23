using UnityEngine;

public class Mission4PrimaryObjective : MonoBehaviour, IObjective
{
    [SerializeField] private Player _player;
    [SerializeField] private Field _field;

    [field: SerializeField]
    public bool IsPrimary { get; set; }

    [field: SerializeField]
    public int ObjectiveId { get; set; }

    public bool IsCompleted => CompletionCheck();

    public string ObjectiveInfo => $"Move your army to {_field.ThreeAxisCoordinates}";

    private bool CompletionCheck()
    {
        return _field.HasUnit() && !_field.Unit.IsEnemy(_player);
    }
}
