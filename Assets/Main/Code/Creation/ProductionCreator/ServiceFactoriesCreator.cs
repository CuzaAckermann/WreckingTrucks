using System.Collections.Generic;

public class ServiceFactoriesCreator
{
    private readonly ModelFactoriesSettings _modelFactoriesSettings;
    private readonly CommonLevelSettings _commonLevelSettings;
    private readonly EventBus _eventBus;

    public ServiceFactoriesCreator(ModelFactoriesSettings modelFactoriesSettings,
                                   CommonLevelSettings commonLevelSettings,
                                   EventBus eventBus)
    {
        Validator.ValidateNotNull(modelFactoriesSettings,
                                  commonLevelSettings,
                                  eventBus);

        _modelFactoriesSettings = modelFactoriesSettings;
        _commonLevelSettings = commonLevelSettings;
        _eventBus = eventBus;
    }

    public List<Factory> Create()
    {
        StopwatchCreator stopwatchCreator = new StopwatchCreator(_modelFactoriesSettings.StopwatchFactorySettings);
        SmoothValueFollowerFactory smoothValueFollowerFactory = new SmoothValueFollowerFactory(_modelFactoriesSettings.SmoothValueFollowerFactorySettings,
                                                                                               _commonLevelSettings.GlobalSettings.SmoothValueFollowerSettings);

        return new List<Factory>
        {
            stopwatchCreator,
            smoothValueFollowerFactory
        };
    }
}