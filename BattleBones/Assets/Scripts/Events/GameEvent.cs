﻿public class GameEvent
{
    public int Timer { get; set; }

    public delegate void Action();

    /// <summary>
    /// Delegate function of Event Action
    /// </summary>
    public Action Action_ { get; set; }

    public GameEvent(int timer, Action action)
    {
        Timer = timer;
        Action_ = action;
    }

    /// <summary>
    /// Decrements timer and calls Action if Timer hits 0
    /// </summary>
    /// <param name="count">Defaults to 1</param>
    public void DecrementTimer(int count = 1)
    {
        Timer -= count;
        if (Timer <= 0)
        {
            Action_();
        }
    }
}