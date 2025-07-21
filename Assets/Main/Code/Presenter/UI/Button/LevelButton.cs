using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private int _indexOfLevel;

    public event Action<int> Pressed;

    public void Initailize(int indexOfLevel)
    {
        if (indexOfLevel < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLevel));
        }

        _indexOfLevel = indexOfLevel;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnPressed);
    }

    public void On()
    {
        _button.interactable = true;
    }

    public void Off()
    {
        _button.interactable = false;
    }

    private void OnPressed()
    {
        Pressed?.Invoke(_indexOfLevel);
    }
}