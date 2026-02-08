using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveOfPlayer", menuName = "Save/SaveOfPlayer")]
public class SaveOfPlayer : ScriptableObject
{
    [SerializeField] private int _availableLevelsAmount;
    [SerializeField] private int _currentLevel;

    public void SetCurrentLevel(int currentLevel)
    {
        if (currentLevel < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentLevel));
        }

        _currentLevel = currentLevel;
    }

    public int AvailableLevelsAmount => _availableLevelsAmount;

    public int CurrentLevel => _currentLevel;
}