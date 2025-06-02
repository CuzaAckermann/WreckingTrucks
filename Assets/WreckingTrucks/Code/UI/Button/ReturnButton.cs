using System;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action ReturnButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnReturnButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnReturnButtonPressed);
    }

    private void OnReturnButtonPressed()
    {
        ReturnButtonPressed?.Invoke();
    }
}