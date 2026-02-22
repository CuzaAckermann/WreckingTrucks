using System;
using System.Collections.Generic;

public class PauseConditions
{
    private readonly List<Type> _inputStatePause;

    public PauseConditions()
    {
        _inputStatePause = new List<Type>
        {
            typeof(LevelSelectionInputState),
            typeof(PausedInputState)
        };
    }

    public List<Type> GetConditions()
    {
        return new List<Type>(_inputStatePause);
    }
}