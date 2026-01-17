using System;

public class ComputerPlayerCreator
{
    private readonly ComputerPlayerSettings _computerPlayerSettings;
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly TypesCalculatorCreator _typesCalculatorCreator;
    private readonly EventBus _eventBus;

    public ComputerPlayerCreator(ComputerPlayerSettings computerPlayerSettings,
                                 StopwatchCreator stopwatchCreator,
                                 TypesCalculatorCreator typesCalculatorCreator,
                                 EventBus eventBus)
    {
        _computerPlayerSettings = computerPlayerSettings ?? throw new ArgumentNullException(nameof(computerPlayerSettings));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _typesCalculatorCreator = typesCalculatorCreator ?? throw new ArgumentNullException(nameof(typesCalculatorCreator));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public ComputerPlayer Create()
    {
        return new ComputerPlayer(_stopwatchCreator.Create(),
                                  _typesCalculatorCreator.Create(),
                                  _computerPlayerSettings.StartDelay,
                                  _computerPlayerSettings.MinFrequency,
                                  _computerPlayerSettings.MaxFrequency,
                                  _eventBus);
    }
}