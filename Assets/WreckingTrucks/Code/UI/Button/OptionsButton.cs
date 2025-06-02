using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action OptionsButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnOptionsButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnOptionsButtonPressed);
    }

    private void OnOptionsButtonPressed()
    {
        OptionsButtonPressed?.Invoke();
    }
}