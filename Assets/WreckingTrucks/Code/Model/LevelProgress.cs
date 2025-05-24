using System;

// Õ≈ √Œ“Œ¬Œ
public class LevelProgress : ITickable
{
    private BlocksField _blocksField;
    private float _normalizeTargetValue;
    private float _normalizeCurrentValue;

    public LevelProgress(BlocksField fieldWithBlocks, float timeToTarget)
    {
        if (timeToTarget <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(timeToTarget)} must be positive.");
        }

        _blocksField = fieldWithBlocks ?? throw new ArgumentNullException(nameof(fieldWithBlocks));

        _blocksField.Reseted += OnReseted;
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