using System;
using UnityEngine;
using UnityEngine.UI;

public class AddRowButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action AddRowButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnAddRow);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnAddRow);
    }

    private void OnAddRow()
    {
        AddRowButtonPressed?.Invoke();
    }
}