using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector
{
    private Level _currentLevel;

    // Тут машина состояний уровня

    public void Start(Level level)
    {
        Validator.ValidateNotNull(level);

        _currentLevel = level;

        _currentLevel.Enable();
    }

    public void Stop()
    {
        _currentLevel.Clear();
    }
}