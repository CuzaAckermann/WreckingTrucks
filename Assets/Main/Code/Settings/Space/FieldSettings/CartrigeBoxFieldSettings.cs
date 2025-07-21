using System;
using UnityEngine;

[Serializable]
public class CartrigeBoxFieldSettings : FieldSettings
{
    [SerializeField, Min(1)] private int _amountCartrigeBoxes;

    public int AmountCartrigeBoxes => _amountCartrigeBoxes;
}