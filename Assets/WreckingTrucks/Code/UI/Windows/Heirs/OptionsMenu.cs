using System;
using UnityEngine;

public class OptionsMenu : Window
{
    [SerializeField] private ReturnButton _returnButton;

    public event Action ReturnButtonPressed;

    private void OnEnable()
    {
        _returnButton.ReturnButtonPressed += OnReturnButtonPressed;
    }

    private void OnDisable()
    {
        _returnButton.ReturnButtonPressed -= OnReturnButtonPressed;
    }

    private void OnReturnButtonPressed()
    {
        ReturnButtonPressed?.Invoke();
    }
}