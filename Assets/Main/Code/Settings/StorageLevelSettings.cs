using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StorageLevelSettings
{
    [Header("Level Settings")]
    [SerializeField] private List<LevelSettings> _levels;

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

    public bool TryGetPreviousLevelSettings(LevelSettings currentLevel,
                                            out LevelSettings previousLevel)
    {
        if (currentLevel == null)
        {
            throw new ArgumentNullException(nameof(currentLevel));
        }

        int index = _levels.IndexOf(currentLevel);

        if (index > 0)
        {
            previousLevel = _levels[index - 1];

            return true;
        }

        previousLevel = null;

        return false;
    }

    public LevelSettings CurrentLevelSettings { get; private set; }

    public LevelSettings GetLevelSettings(int index)
    {
        if (index < 0 || index >= _levels.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        CurrentLevelSettings = _levels[index];

        return _levels[index];
    }

    public bool TryGetNextLevelSettings(LevelSettings currentLevel,
                                        out LevelSettings nextLevel)
    {
        if (currentLevel == null)
        {
            throw new ArgumentNullException(nameof(currentLevel));
        }

        int index = _levels.IndexOf(currentLevel);

        if (index >= 0 && index < _levels.Count - 1)
        {
            nextLevel = _levels[index + 1];

            return true;
        }

        nextLevel = null;

        return false;
    }
}