using System;
using UnityEngine;
using UnityEngine.UI;

public class StartLevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action StartLevelButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnReset);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnReset);
    }

    public void OnReset()
    {
        StartLevelButtonPressed?.Invoke();
    }
}