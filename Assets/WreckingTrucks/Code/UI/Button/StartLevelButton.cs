using System;
using UnityEngine;
using UnityEngine.UI;

public class StartLevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action StartLevelButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnStartLevelButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnStartLevelButtonPressed);
    }

    private void OnStartLevelButtonPressed()
    {
        StartLevelButtonPressed?.Invoke();
    }
}