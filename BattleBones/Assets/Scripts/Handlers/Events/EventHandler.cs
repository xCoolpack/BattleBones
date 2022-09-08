using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private List<GameEvent> _endTurnEvents = new();
    private List<GameEvent> _startTurnEvents = new();

    /// <summary>
    /// Adds event to end turn queue
    /// </summary>
    /// <param name="e"></param>
    public void AddEndTurnEvent(GameEvent e)
    {
        if (e.Timer <= 0)
            e.Action_();
        else
            _endTurnEvents.Add(e);
    }

    /// <summary>
    /// Adds event to start turn queue
    /// </summary>
    /// <param name="e"></param>
    public void AddStartTurnEvent(GameEvent e)
    {
        if (e.Timer <= 0)
            e.Action_();
        else
            _startTurnEvents.Add(e);
    }

    /// <summary>
    /// Informs EventHandler that the turn has ended
    /// </summary>
    public void TurnEnd()
    {
        _endTurnEvents.ForEach(e => e.DecrementTimer());
    }


    /// <summary>
    /// Informs EventHandler that the turn has started
    /// </summary>
    public void TurnStart()
    {
        _startTurnEvents.ForEach(e => e.DecrementTimer());
    }
}
