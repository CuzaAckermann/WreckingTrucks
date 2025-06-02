using System;
using UnityEngine;
using UnityEngine.UI;

public class ResetLevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action ResetLevelButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnResetLevelButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnResetLevelButtonPressed);
    }

    private void OnResetLevelButtonPressed()
    {
        ResetLevelButtonPressed?.Invoke();
    }
}