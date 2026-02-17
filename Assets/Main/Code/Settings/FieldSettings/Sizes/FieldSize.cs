using System;
using UnityEngine;

[Serializable]
public class FieldSize
{
    [SerializeField, Min(1)] private int _amountLayers = 1;
    [SerializeField, Min(1)] private int _amountColumns = 1;
    [SerializeField, Min(1)] private int _amountRows = 1;

    public int AmountLayers => _amountLayers;

    public int AmountColumns => _amountColumns;

    public int AmountRows => _amountRows;
}