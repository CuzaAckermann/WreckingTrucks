using System;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action Pressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnPressed);
    }

    private void OnPressed()
    {
        Pressed?.Invoke();
    }
}