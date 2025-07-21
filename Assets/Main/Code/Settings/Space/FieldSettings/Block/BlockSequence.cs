using System;
using UnityEngine;

[Serializable]
public class BlockSequence
{
    [SerializeField] private ColorType _colorType;
    [SerializeField] private int _amount;

    public ColorType ColorType { get => _colorType; set => _colorType = value; }

    public int Amount { get => _amount; set => _amount = value; }
}