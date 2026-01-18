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

        _eventBus.Subscribe<GameStartedSignal>(Enable);
        _eventBus.Subscribe<GameEndedSignal>(Disable);
        _eventBus.Subscribe<DestroyedGameWorldSignal>(DestroyModels);
    }

    private void Enable(GameStartedSignal _)
    {
        _eventBus.Subscribe<CreatedSignal<Model>>(OnModelCreated);
    }

    private void Disable(GameEndedSignal _)
    {
        _eventBus.Unsubscribe<CreatedSignal<Model>>(OnModelCreated);
    }

    private void DestroyModels(DestroyedGameWorldSignal _)
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