using System;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action ResetButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnReset);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnReset);
    }

    private void OnReset()
    {
        ResetButtonPressed?.Invoke();
    }
}