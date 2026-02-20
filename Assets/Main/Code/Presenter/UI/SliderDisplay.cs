using UnityEngine;
using UnityEngine.UI;

public class SliderDisplay : MonoBehaviourSubscriber
{
    [SerializeField] private Slider _slider;

    private IClampedAmount _clampedValue;
    private IAmount _valueFollower;

    public void Init(IClampedAmount clampedValue, IAmount valueFollower)
    {
        Validator.ValidateNotNull(clampedValue, valueFollower);

        _clampedValue = clampedValue;
        _valueFollower = valueFollower;

        _slider.wholeNumbers = true;
        _slider.maxValue = _clampedValue.Max.Value;

        Init();
    }

    protected override void Subscribe()
    {
        _valueFollower.Changed += UpdateCurrent;
        _clampedValue.Max.Changed += UpdateMax;
    }

    protected override void Unsubscribe()
    {
        _valueFollower.Changed -= UpdateCurrent;
        _clampedValue.Max.Changed -= UpdateMax;
    }

    private void UpdateCurrent(float value)
    {
        _slider.value = value;
    }

    private void UpdateMax(float value)
    {
        _slider.maxValue = value;
    }
}