using UnityEngine;

public class PlayerSettings
{
    [SerializeField] private int _currentLevel;

    public int CurrentLevel => _currentLevel;
}