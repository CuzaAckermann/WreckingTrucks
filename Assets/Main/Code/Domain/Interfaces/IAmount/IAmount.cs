using System;

public interface IAmount
{
    public event Action<float> ValueChanged;

    public float Value { get; }
}