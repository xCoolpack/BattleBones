using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private List<GameEvent> _endTurnEvents;
    private List<GameEvent> _startTurnEvents;

    public EventHandler()
    {
        _endTurnEvents = new List<GameEvent>();
        _startTurnEvents = new List<GameEvent>();
    }

    public void AddEndTurnEvent(GameEvent e)
    {
        _endTurnEvents.Add(e);
    }

    public void AddStartTurnEvent(GameEvent e)
    {
        e.Timer += 1;
        _startTurnEvents.Add(e);
    }

    public void TurnEnd()
    {
        _endTurnEvents.ForEach(e => e.DecrementTimer());
    }

    public void TurnStart()
    {
        _startTurnEvents.ForEach(e => e.DecrementTimer());
    }
}
