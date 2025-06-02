using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action PlayButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnPlayButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnPlayButtonPressed);
    }

    private void OnPlayButtonPressed()
    {
        PlayButtonPressed?.Invoke();
    }
}