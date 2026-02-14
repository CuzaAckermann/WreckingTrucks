using System;
using System.Collections.Generic;

public class ModelProduction
{
    private readonly List<ICreator<Model>> _factories;

    public ModelProduction()
    {
        _factories = new List<ICreator<Model>>();
    }

    public event Action<Model> ModelCreated;

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
        ModelCreated?.Invoke(model);
    }
}