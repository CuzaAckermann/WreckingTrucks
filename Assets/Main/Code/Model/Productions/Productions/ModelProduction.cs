using System;
using System.Collections.Generic;

public class ModelProduction
{
    private readonly List<ICreator<Model>> _factories;
    private readonly EventBus _eventBus;

    public ModelProduction(EventBus eventBus)
    {
        Validator.ValidateNotNull(eventBus);

        _factories = new List<ICreator<Model>>();
        _eventBus = eventBus;
    }

    public event Action<Model> Created;

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

    public bool TryCreate<M>(out M requiredModel) where M : Model
    {
        requiredModel = null;

        for (int currentFactory = 0; currentFactory < _factories.Count; currentFactory++)
        {
            if (_factories[currentFactory] is ModelFactory<M> modelFactory)
            {
                requiredModel = modelFactory.Create();

                break;
            }
        }

        return requiredModel != null;
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
        _eventBus.Invoke(new CreatedSignal<Model>(model));

        Created?.Invoke(model);
    }
}