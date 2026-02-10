using System;
using System.Collections.Generic;

public class ModelProduction
{
    private readonly EventBus _eventBus;
    private readonly List<ICreator<Model>> _factories;

    public ModelProduction(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _factories = new List<ICreator<Model>>();
    }

    public void AddFactory(ICreator<Model> modelFactory)
    {
        if (modelFactory == null)
        {
            throw new ArgumentNullException(nameof(modelFactory));
        }

        if (_factories.Contains(modelFactory))
        {
            throw new InvalidOperationException($"{modelFactory} is already added.");
        }

        _factories.Add(modelFactory);
    }

    public void SubscribeToFactories()
    {
        for (int i = 0; i < _factories.Count; i++)
        {
            SubscribeToFactory(_factories[i]);
        }
    }

    public void UnsubscribeFromFactories()
    {
        for (int i = 0; i < _factories.Count; i++)
        {
            UnsubscribeFromFactory(_factories[i]);
        }
    }

    private void SubscribeToFactory(ICreator<Model> modelFactory)
    {
        modelFactory.Created += OnModelCreated;
    }

    private void UnsubscribeFromFactory(ICreator<Model> modelFactory)
    {
        modelFactory.Created -= OnModelCreated;
    }

    private void OnModelCreated(Model model)
    {
        SubscribeToModel(model);
    }

    private void SubscribeToModel(Model model)
    {
        model.Destroyed += UnsubscribeFromModel;
        model.Placed += OnFirstPositionDefined;
    }

    private void UnsubscribeFromModel(IDestroyable destroyable)
    {
        destroyable.Destroyed -= UnsubscribeFromModel;

        if (Validator.IsRequiredType(destroyable, out Model model) == false)
        {
            return;
        }

        model.Placed -= OnFirstPositionDefined;
    }

    private void OnFirstPositionDefined(Model model)
    {
        UnsubscribeFromModel(model);

        _eventBus.Invoke(new CreatedSignal<Model>(model));
    }
}