using System;
using UnityEngine;

[Serializable]
public class CartrigeBoxFieldSettings : FieldSettings
{
    [SerializeField, Min(1)] private int _amountCartrigeBoxes;
    [SerializeField, Min(0.01f)] private float _frequency;

    public int AmountCartrigeBoxes => _amountCartrigeBoxes;

    public float Frequency => _frequency;

    public void SetAmountCartrigeBoxes(int amountCartrigeBoxes)
    {
        _amountCartrigeBoxes = amountCartrigeBoxes;
    }
}