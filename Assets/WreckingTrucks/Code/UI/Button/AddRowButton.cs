using System;
using UnityEngine;
using UnityEngine.UI;

public class AddRowButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action AddRowButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnAddRowButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnAddRowButtonPressed);
    }

    private void OnAddRowButtonPressed()
    {
        AddRowButtonPressed?.Invoke();
    }
}