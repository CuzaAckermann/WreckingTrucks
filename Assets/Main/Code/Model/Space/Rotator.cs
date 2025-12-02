using System;

public class Rotator : ModelProcessor
{
    protected override string ProcessorName => nameof(Rotator);

    protected override Action<Model, float> ProcessAction => (model, deltaTime) => model.Rotate(deltaTime);

    public Rotator(TickEngine tickEngine,
                   int capacity,
                   ModelProduction modelProduction)
            : base(tickEngine,
                   capacity,
                   modelProduction)
    {
        Enable();
    }

    protected override void SubscribeToCreatedModel(Model model)
    {
        model.Destroyed += OnDestroyed;
        model.TargetRotationChanged += OnTargetRotationChanged;
    }

    protected override void UnsubscribeFromCreatedModel(Model model)
    {
        model.Destroyed -= OnDestroyed;
        model.TargetRotationChanged -= OnTargetRotationChanged;
    }

    protected override void SubscribeToActiveModel(Model model)
    {
        model.TargetRotationReached += OnTargetRotationReached;
    }

    protected override void UnsubscribeFromActiveModel(Model model)
    {
        model.TargetRotationReached -= OnTargetRotationReached;
    }

    private void OnTargetRotationChanged(Model model)
    {
        if (_activeModels.Contains(model) == false)
        {
            ActivateModel(model);
        }
    }

    private void OnTargetRotationReached(Model model)
    {
        DeactivateModel(model);
    }
}