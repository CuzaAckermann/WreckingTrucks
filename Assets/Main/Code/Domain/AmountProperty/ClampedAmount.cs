using UnityEngine;

public class ClampedAmount : Amount, IClampedAmount
{
    private readonly Amount _min;
    private readonly Amount _max;

    public ClampedAmount(float initialValue,
                         float min, float max)
                  : base(initialValue)
    {
        Validator.ValidateMin(initialValue, min, false);
        Validator.ValidateMax(initialValue, max, false);

        _min = new Amount(min);
        _max = new Amount(max);

        //Subscribe();
    }

    public IAmount Max => _max;

    public IAmount Min => _min;

    public override void Increase(float increment)
    {
        Change(Value + increment);
    }

    public override void Decrease(float decrement)
    {
        Change(Value - decrement);
    }

    public override void Change(float newValue)
    {
        base.Change(Mathf.Clamp(newValue, Min.Value, Max.Value));
    }

    public void Unsubscribe()
    {
        Max.ValueChanged -= ClampCurrent;
        Min.ValueChanged -= ClampCurrent;
    }

    private void Subscribe()
    {
        Max.ValueChanged += ClampCurrent;
        Min.ValueChanged += ClampCurrent;
    }

    private void ClampCurrent(float _)
    {
        Validator.ValidateMax(Min.Value, Max.Value, true);

        Change(Mathf.Clamp(Value, Min.Value, Max.Value));
    }
}