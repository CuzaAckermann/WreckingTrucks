using System;

public class ComputerPlayerCreator
{
    private readonly ComputerPlayerSettings _computerPlayerSettings;
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly TypesCalculatorCreator _typesCalculatorCreator;

    public ComputerPlayerCreator(ComputerPlayerSettings computerPlayerSettings,
                                 StopwatchCreator stopwatchCreator,
                                 TypesCalculatorCreator typesCalculatorCreator)
    {
        _computerPlayerSettings = computerPlayerSettings ?? throw new ArgumentNullException(nameof(computerPlayerSettings));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _typesCalculatorCreator = typesCalculatorCreator ?? throw new ArgumentNullException(nameof(typesCalculatorCreator));
    }

    public ComputerPlayer Create()
    {
        return new ComputerPlayer(_stopwatchCreator.Create(),
                                  _typesCalculatorCreator.Create(),
                                  _computerPlayerSettings.StartDelay,
                                  _computerPlayerSettings.MinFrequency,
                                  _computerPlayerSettings.MaxFrequency);
    }
}