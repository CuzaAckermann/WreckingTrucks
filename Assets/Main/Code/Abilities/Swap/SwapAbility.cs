using System;
using System.Collections.Generic;

public class SwapAbility : IModelPositionObserver
{
    private Field _field;

    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;

    public void SetField(Field field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
    }

    public void Enable()
    {
        SubscribeToField();
    }

    public void Disable()
    {
        UnsubscribeFromField();
    }

    private void SubscribeToField()
    {
        _field.PositionChanged += OnPositionChanged;
        _field.PositionsChanged += OnPositionsChanged;
    }

    private void UnsubscribeFromField()
    {
        _field.PositionChanged -= OnPositionChanged;
        _field.PositionsChanged -= OnPositionsChanged;
    }

    private void OnPositionChanged(Model model)
    {
        PositionChanged?.Invoke(model);
    }

    private void OnPositionsChanged(List<Model> models)
    {
        PositionsChanged?.Invoke(models);
    }
}