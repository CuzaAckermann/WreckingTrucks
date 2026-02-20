using System;
using System.Collections.Generic;
using UnityEngine;

public class ProductionCreator
{
    private readonly ModelFactoriesCreator _modelFactoriesCreator;
    private readonly PresenterFactoriesCreator _presenterFactoriesCreator;
    private readonly ServiceFactoriesCreator _serviceFactoriesCreator;

    private readonly Production _production;

    public ProductionCreator(EventBus eventBus,
                                  ModelFactoriesSettings modelFactoriesSettings,
                                  ModelsSettings modelsSettings,
                                  PresenterFactoriesSettings presenterFactoriesSettings,
                                  Transform poolParent,
                                  Func<Presenter, Transform, Presenter> createFunction,
                                  CommonLevelSettings commonLevelSettings)
    {
        _modelFactoriesCreator = new ModelFactoriesCreator(modelFactoriesSettings,
                                                           modelsSettings,
                                                           eventBus);
        _presenterFactoriesCreator = new PresenterFactoriesCreator(presenterFactoriesSettings,
                                                                   poolParent,
                                                                   createFunction);
        _serviceFactoriesCreator = new ServiceFactoriesCreator(modelFactoriesSettings,
                                                               commonLevelSettings,
                                                               eventBus);

        _production = new Production(eventBus, CreateFactories());
    }

    public Production GetProduction()
    {
        return _production;
    }

    private List<Factory> CreateFactories()
    {
        List<Factory> factories = new List<Factory>();

        factories.AddRange(_modelFactoriesCreator.Create());
        factories.AddRange(_presenterFactoriesCreator.Create());
        factories.AddRange(_serviceFactoriesCreator.Create());

        return factories;
    }
}