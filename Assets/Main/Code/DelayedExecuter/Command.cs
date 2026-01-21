using System;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    //private Receiver _receiver;

    private readonly Action _action;
    private readonly float _delay;

    public Command(Action action, float delay)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
        _delay = delay >= 0 ? delay : throw new ArgumentOutOfRangeException(nameof(delay));
    }

    public float Delay => _delay;

    public void Execute()
    {
        _action.Invoke();
    }
}