using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class StorageLevelSettings
{
    [Header("Level Settings")]
    [SerializeField] private List<LevelSettings> _levels;

    private readonly Random _random = new Random();

    public int AmountLevels => _levels.Count;

    public LevelSettings GetLevelSettings(int index)
    {
        if (index < 0 || index >= _levels.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _levels[index];
    }

    public LevelSettings GetRandomBlockFieldSettings()
    {
        int index = _random.Next(0, _levels.Count);

        return GetLevelSettings(index);
    }
}