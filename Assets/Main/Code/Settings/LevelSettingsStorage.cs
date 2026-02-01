using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettingsStorage : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private List<LevelSettings> _levels;

    public int AmountLevels => _levels.Count;

    public bool HasNextLevelSettings(int currentIndexOfLevelSettings)
    {
        return currentIndexOfLevelSettings + 1 < _levels.Count;
    }

    public bool HasPreviousLevelSettings(int currentIndexOfLevelSettings)
    {
        return currentIndexOfLevelSettings - 1 >= 0;
    }

    public LevelSettings GetLevelSettings(int index)
    {
        if (index < 0 || index >= _levels.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _levels[index];
    }
}