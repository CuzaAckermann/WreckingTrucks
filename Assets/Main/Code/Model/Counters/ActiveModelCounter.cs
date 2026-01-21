using System;
using System.Collections.Generic;

public class ActiveModelCounter<M> where M : Model
{
    private readonly EventBus _eventBus;
    private readonly List<M> _countedModels;

    public ActiveModelCounter(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _countedModels = new List<M>();

        _eventBus.Subscribe<ClearedSignal<Level>>(Clear);
        _eventBus.Subscribe<ActivatedSignal<M>>(AddActivedModel);
    }

    public int Amount => _countedModels.Count;

    private void AddActivedModel(ActivatedSignal<M> activatedSignal)
    {
        M model = activatedSignal.Activatable;

        if (_countedModels.Contains(model))
        {
            throw new InvalidOperationException($"{nameof(model)} is contained");
        }

        _countedModels.Add(model);

        SubscribeToActivedModel(model);
    }

    private void Clear(ClearedSignal<Level> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<Level>>(Clear);
        _eventBus.Unsubscribe<ActivatedSignal<M>>(AddActivedModel);
    }

    private void SubscribeToActivedModel(M model)
    {
        model.DestroyedModel += UnsubscribeFromActivedModel;
    }

    private void UnsubscribeFromActivedModel(Model model)
    {
        model.DestroyedModel -= UnsubscribeFromActivedModel;

        RemoveActivedModel((M)model);
    }

    private void RemoveActivedModel(M model)
    {
        _countedModels.Remove(model);

        if (_countedModels.Count == 0)
        {
            _eventBus.Invoke(new ActiveModelIsEmptySignal<M>());
        }
    }
}