using System;

public class Amount : IAmount
{
    public Amount(float initialValue)
    {
        Value = initialValue;
    }

    public event Action<float> Changed;

    public float Value { get; private set; }

    public virtual void Increase(float increment)
    {
        Change(Value + increment);
    }

    public virtual void Decrease(float decrement)
    {
        Change(Value - decrement);
    }

    public virtual void Change(float newValue)
    {
        Value = newValue;

        Changed?.Invoke(newValue);
    }
}