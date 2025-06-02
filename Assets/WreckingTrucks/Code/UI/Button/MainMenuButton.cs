using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action MainMenuButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnMainMenuButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnMainMenuButtonPressed);
    }

    private void OnMainMenuButtonPressed()
    {
        MainMenuButtonPressed?.Invoke();
    }
}