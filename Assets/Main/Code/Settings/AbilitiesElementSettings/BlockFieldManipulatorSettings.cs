using System;
using UnityEngine;

[Serializable]
public class BlockFieldManipulatorSettings
{
    [SerializeField] private int _amountShiftedRows;

    public int AmountShiftedRows => _amountShiftedRows;
}