using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public int EvalValue { get; set; }
    public Action MoveAction;

    public Move(int evalValue, Action moveAction)
    {
        EvalValue = evalValue;
        MoveAction = moveAction;
    }

    public void Execute()
    {
        MoveAction();
    }
}
