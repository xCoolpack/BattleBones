public interface IObjective
{
    /// <summary>
    /// Is objective primary
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Is objective complited
    /// </summary>
    /// <returns></returns>
    public bool IsComplited { get; }

    /// <summary>
    /// Objective information
    /// </summary>
    public string ObjectiveInfo { get; set; }
}
