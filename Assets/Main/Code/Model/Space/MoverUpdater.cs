using System;

public class MoverUpdater : ModelProcessorUpdater
{
    protected override string ProcessorName => nameof(MoverUpdater);

    protected override Action<Model, float> ProcessAction => (model, deltaTime) => model.Move(deltaTime);

    public MoverUpdater(EventBus eventBus, int capacity)
          : base(eventBus, capacity)
    {

    }

    protected override void SubscribeToCreatedModel(Model model)
    {
        model.DestroyedModel += OnDestroyed;
        model.TargetPositionChanged += OnTargetPositionChanged;
    }

    protected override void UnsubscribeFromCreatedModel(Model model)
    {
        model.DestroyedModel -= OnDestroyed;
        model.TargetPositionChanged -= OnTargetPositionChanged;
    }

    protected override void SubscribeToActiveModel(Model model)
    {
        model.TargetPositionReached += OnTargetPositionReached;
    }

    protected override void UnsubscribeFromActiveModel(Model model)
    {
        model.TargetPositionReached -= OnTargetPositionReached;
    }

    private void OnTargetPositionChanged(Model model)
    {
        if (_activeModels.Contains(model) == false)
        {
            ActivateModel(model);
        }
    }

    private void OnTargetPositionReached(Model model)
    {
        DeactivateModel(model);
    }
}