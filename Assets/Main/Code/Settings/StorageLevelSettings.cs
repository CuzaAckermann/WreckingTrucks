using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageLevelSettings : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private List<LevelSettings> _levels;

    public LevelSettings CurrentLevelSettings { get; private set; }

    public int AmountLevels => _levels.Count;

    public bool HasNextLevelSettings(int currentIndexOfLevelSettings)
    {
        if (currentIndexOfLevelSettings + 1 <  _levels.Count)
        {
            return true;
        }

        return false;
    }

    public bool HasPreviousLevelSettings(int currentIndexOfLevelSettings)
    {
        if (currentIndexOfLevelSettings - 1 >= 0)
        {
            return true;
        }

        return false;
    }

    public LevelSettings GetLevelSettings(int index)
    {
        if (index < 0 || index >= _levels.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        CurrentLevelSettings = _levels[index];

        return _levels[index];
    }
}