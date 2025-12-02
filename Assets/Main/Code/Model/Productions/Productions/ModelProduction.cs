using System;
using System.Collections.Generic;

public class ModelProduction
{
    private readonly List<IModelCreator<Model>> _factories = new List<IModelCreator<Model>>();

    public event Action<Model> ModelCreated;

    public void AddFactory(IModelCreator<Model> modelFactory)
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

    private void SubscribeToFactory(IModelCreator<Model> modelFactory)
    {
        modelFactory.ModelCreated += OnModelCreated;
    }

    private void UnsubscribeFromFactory(IModelCreator<Model> modelFactory)
    {
        modelFactory.ModelCreated -= OnModelCreated;
    }

    private void OnModelCreated(Model model)
    {
        SubscribeToModel(model);
    }

    private void SubscribeToModel(Model model)
    {
        model.Destroyed += UnsubscribeFromModel;
        model.FirstPositionDefined += OnFirstPositionDefined;
    }

    private void UnsubscribeFromModel(Model model)
    {
        model.Destroyed -= UnsubscribeFromModel;
        model.FirstPositionDefined -= OnFirstPositionDefined;
    }

    private void OnFirstPositionDefined(Model model)
    {
        UnsubscribeFromModel(model);
        ModelCreated?.Invoke(model);
    }
}