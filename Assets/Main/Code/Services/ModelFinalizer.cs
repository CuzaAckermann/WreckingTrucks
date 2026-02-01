using System;
using System.Collections.Generic;

public class ModelFinalizer
{
    private readonly EventBus _eventBus;

    private readonly List<Model> _createdModels;

    public ModelFinalizer(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _createdModels = new List<Model>();

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<EnabledSignal<GameSignalEmitter>>(Enable);
        _eventBus.Subscribe<DisabledSignal<GameSignalEmitter>>(Disable);
        _eventBus.Subscribe<ClearedSignal<Level>>(DestroyModels);
    }

    private void Clear(ClearedSignal<GameSignalEmitter> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Unsubscribe<EnabledSignal<GameSignalEmitter>>(Enable);
        _eventBus.Unsubscribe<DisabledSignal<GameSignalEmitter>>(Disable);
        _eventBus.Unsubscribe<ClearedSignal<Level>>(DestroyModels);
    }

    private void Enable(EnabledSignal<GameSignalEmitter> _)
    {
        _eventBus.Subscribe<CreatedSignal<Model>>(OnModelCreated);
    }

    private void Disable(DisabledSignal<GameSignalEmitter> _)
    {
        _eventBus.Unsubscribe<CreatedSignal<Model>>(OnModelCreated);
    }

    private void DestroyModels(ClearedSignal<Level> _)
    {
        for (int i = _createdModels.Count - 1; i >= 0; i--)
        {
            _createdModels[i].DestroyedModel -= OnDestroyed;
            _createdModels[i].Destroy();
        }
    }

    private void OnModelCreated(CreatedSignal<Model> modelSignal)
    {
        Model model = modelSignal.Creatable;

        if (_createdModels.Contains(model))
        {
            return;
        }

        _createdModels.Add(model);

        model.DestroyedModel += OnDestroyed;
    }

    private void OnDestroyed(Model model)
    {
        model.DestroyedModel -= OnDestroyed;

        if (_createdModels.Contains(model) == false)
        {
            throw new InvalidOperationException($"{model} does not exist");
        }

        _createdModels.Remove(model);
    }
}