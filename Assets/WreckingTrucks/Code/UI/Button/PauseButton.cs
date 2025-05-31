using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action PauseButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnPause);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnPause);
    }

    public void OnPause()
    {
        PauseButtonPressed?.Invoke();
    }
}