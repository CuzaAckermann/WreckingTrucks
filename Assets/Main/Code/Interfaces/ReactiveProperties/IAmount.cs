using System;

public interface IAmount
{
    public event Action<float> Changed;

    public float Value { get; }
}