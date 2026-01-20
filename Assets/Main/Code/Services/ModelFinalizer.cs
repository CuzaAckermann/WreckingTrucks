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

        _eventBus.Subscribe<EnabledSignal<Game>>(Enable);
        _eventBus.Subscribe<DisabledSignal<Game>>(Disable);
        _eventBus.Subscribe<ClearedSignal<GameWorld>>(DestroyModels);
    }

    private void Enable(EnabledSignal<Game> _)
    {
        _eventBus.Subscribe<CreatedSignal<Model>>(OnModelCreated);
    }

    private void Disable(DisabledSignal<Game> _)
    {
        _eventBus.Unsubscribe<CreatedSignal<Model>>(OnModelCreated);
    }

    private void DestroyModels(ClearedSignal<GameWorld> _)
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
            //throw new InvalidOperationException($"{model} is already added");
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