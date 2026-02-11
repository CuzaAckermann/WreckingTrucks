using System;

public interface IAmount : INotifier
{
    public event Action<float> ValueChanged;

    public float Value { get; }
}