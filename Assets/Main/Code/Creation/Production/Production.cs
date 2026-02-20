using System;
using System.Collections.Generic;

public class Production
{
    private readonly EventBus _eventBus;
    private readonly Dictionary<Type, Factory> _factories;

    public Production(EventBus eventBus, List<Factory> creators)
    {
        Validator.ValidateNotNull(eventBus, creators);
        Validator.ValidateNotEmpty(creators);

        _eventBus = eventBus;
        _factories = new Dictionary<Type, Factory>();

        for (int i = 0; i < creators.Count; i++)
        {
            Type type = creators[i].GetCreatableType();

            if (_factories.TryAdd(type, creators[i]) == false)
            {
                Logger.Log($"Factory for type {type} already exists. Skipping duplicate.");
            }
        }
    }

    public bool TryCreate(Type targetType, out IDestroyable requiredElement)
    {
        requiredElement = default;

        if (_factories.TryGetValue(targetType, out Factory factory) == false)
        {
            return false;
        }

        requiredElement = factory.Create();

        _eventBus.Invoke(new CreatedSignal<IDestroyable>(requiredElement));

        return true;
    }

    public bool TryCreate<T>(out T requiredElement) where T : IDestroyable
    {
        requiredElement = default;

        if (_factories.TryGetValue(typeof(T), out Factory factory) == false)
        {
            return false;
        }

        if (factory.Create() is not T creatable)
        {
            Logger.LogError($"Factory for {typeof(T).Name} created wrong type");

            return false;
        }

        requiredElement = creatable;

        _eventBus.Invoke(new CreatedSignal<IDestroyable>(requiredElement));
        _eventBus.Invoke(new CreatedSignal<T>(requiredElement));

        if (requiredElement is BlockPresenter blockPresenter)
        {
            _eventBus.Invoke(new CreatedSignal<IDestroyable>(blockPresenter.Jelly));
        }

        return true;
    }
}