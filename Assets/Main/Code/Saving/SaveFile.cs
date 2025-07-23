using System;
using UnityEngine;

[Serializable]
public class SaveFile
{
    [SerializeField] private int _currentLevel;

    public int CurrentLevel => _currentLevel;

    public void SetCurrentLevel(int indexOfLevel)
    {
        if (indexOfLevel < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLevel));
        }

        _currentLevel = indexOfLevel;
    }
}