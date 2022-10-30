using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private List<GameEvent> _endTurnEvents = new();
    private List<GameEvent> _startTurnEvents = new();

    /// <summary>
    /// Adds event to start turn queue
    /// </summary>
    /// <param name="e"></param>
    public void AddStartTurnEvent(GameEvent e)
    {
        //if (e.Timer <= 0)
        //    e.Action_();
        //else
        _startTurnEvents.Add(e);
    }

    public void RemoveStartTurnEvent(GameEvent e)
    {
        _startTurnEvents.Remove(e);
    }

    /// <summary>
    /// Adds event to end turn queue
    /// </summary>
    /// <param name="e"></param>
    public void AddEndTurnEvent(GameEvent e)
    {
        //if (e.Timer < 0)
        //    e.Action_();
        //else
        _endTurnEvents.Add(e);
    }

    /// <summary>
    /// Informs EventHandler that the turn has ended
    /// </summary>
    public void TurnEnd()
    {
        for (var i = 0; i < _endTurnEvents.Count; i++)
        {
            if (!_endTurnEvents[i].DecrementTimer()) continue;
            _endTurnEvents.RemoveAt(i);
            i--;
        }
    }


    /// <summary>
    /// Informs EventHandler that the turn has started
    /// </summary>
    public void TurnStart()
    {
        for (var i = 0; i < _startTurnEvents.Count; i++)
        {
            if (!_startTurnEvents[i].DecrementTimer()) continue;
            _startTurnEvents.RemoveAt(i);
            i--;
        }
    }
}
