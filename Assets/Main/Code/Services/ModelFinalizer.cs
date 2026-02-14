using System;
using System.Collections.Generic;

public class ModelFinalizer
{
    private readonly EventBus _eventBus;

    private readonly List<IDestroyable> _createdModels;

    public ModelFinalizer(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _createdModels = new List<IDestroyable>();

        _eventBus.Subscribe<ClearedSignal<ApplicationSignal>>(Clear);

        _eventBus.Subscribe<EnabledSignal<ApplicationSignal>>(Enable);
        _eventBus.Subscribe<DisabledSignal<ApplicationSignal>>(Disable);
        _eventBus.Subscribe<ClearedSignal<Level>>(DestroyModels);
    }

    private void Clear(ClearedSignal<ApplicationSignal> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<ApplicationSignal>>(Clear);

        _eventBus.Unsubscribe<EnabledSignal<ApplicationSignal>>(Enable);
        _eventBus.Unsubscribe<DisabledSignal<ApplicationSignal>>(Disable);
        _eventBus.Unsubscribe<ClearedSignal<Level>>(DestroyModels);
    }

    private void Enable(EnabledSignal<ApplicationSignal> _)
    {
        _eventBus.Subscribe<CreatedSignal<Model>>(OnModelCreated);
    }

    private void Disable(DisabledSignal<ApplicationSignal> _)
    {
        _eventBus.Unsubscribe<CreatedSignal<Model>>(OnModelCreated);
    }

    private void DestroyModels(ClearedSignal<Level> _)
    {
        for (int i = _createdModels.Count - 1; i >= 0; i--)
        {
            _createdModels[i].Destroyed -= OnDestroyed;
            _createdModels[i].Destroy();
        }
    }

    private void OnModelCreated(CreatedSignal<Model> modelSignal)
    {
        Model model = modelSignal.Creatable;

        if (Validator.IsRequiredType(model, out IDestroyable destroyable) == false)
        {
            return;
        }

        if (Validator.IsContains(_createdModels, destroyable))
        {
            return;
        }

        _createdModels.Add(model);

        model.Destroyed += OnDestroyed;
    }

    private void OnDestroyed(IDestroyable destroyable)
    {
        destroyable.Destroyed -= OnDestroyed;

        if (Validator.IsContains(_createdModels, destroyable) == false)
        {
            return;
        }

        _createdModels.Remove(destroyable);
    }
}