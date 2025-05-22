using System;

// Õ≈ √Œ“Œ¬Œ
public class LevelProgress : ITickable
{
    private FieldWithBlocks _fieldWithBlocks;
    private float _normalizeTargetValue;
    private float _normalizeCurrentValue;

    public LevelProgress(FieldWithBlocks fieldWithBlocks, float timeToTarget)
    {
        if (timeToTarget <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(timeToTarget)} must be positive.");
        }

        _fieldWithBlocks = fieldWithBlocks ?? throw new ArgumentNullException(nameof(fieldWithBlocks));

        _fieldWithBlocks.Reseted += OnReseted;
        _fieldWithBlocks.AmountBlockChanged += OnAmountBlockChanged;
    }

    public event Action CurrentValueChanged;

    public float CurrentValue { get; private set; }

    public void Tick(float deltaTime)
    {

    }

    private void OnReseted()
    {
        
    }

    private void OnAmountBlockChanged()
    {

    }
}