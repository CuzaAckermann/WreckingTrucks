using System;

public class RotatorUpdater : ModelProcessorUpdater
{
    protected override string ProcessorName => nameof(RotatorUpdater);

    protected override Action<Model, float> ProcessAction => (model, deltaTime) => model.Rotate(deltaTime);

    public RotatorUpdater(EventBus eventBus, int capacity)
            : base(eventBus, capacity)
    {
    }

    protected override void SubscribeToCreatedModel(Model model)
    {
        model.DestroyedModel += OnDestroyed;
        model.TargetRotationChanged += OnTargetRotationChanged;
    }

    protected override void UnsubscribeFromCreatedModel(Model model)
    {
        model.DestroyedModel -= OnDestroyed;
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