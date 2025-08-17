using System;
using System.Collections.Generic;

public class SwapAbility : IModelPositionObserver
{
    private Field _field;



    public event Action<Model> ModelPositionChanged;

    public event Action<Model> PositionReached;

    public event Action<IModel> InterfacePositionChanged;

    public event Action<List<Model>> PositionsChanged;

    public event Action<List<IModel>> InterfacePositionsChanged;



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
        _field.ModelPositionChanged += OnPositionChanged;
        _field.PositionsChanged += OnPositionsChanged;
    }

    private void UnsubscribeFromField()
    {
        _field.ModelPositionChanged -= OnPositionChanged;
        _field.PositionsChanged -= OnPositionsChanged;
    }

    private void OnPositionChanged(Model model)
    {
        ModelPositionChanged?.Invoke(model);
    }

    private void OnPositionsChanged(List<Model> models)
    {
        PositionsChanged?.Invoke(models);
    }
}