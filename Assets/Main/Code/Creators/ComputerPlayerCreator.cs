using System;

public class ComputerPlayerCreator
{
    private readonly EventBus _eventBus;
    private readonly TypesCalculatorCreator _typesCalculatorCreator;
    private readonly ComputerPlayerSettings _computerPlayerSettings;

    public ComputerPlayerCreator(ComputerPlayerSettings computerPlayerSettings,
                                 TypesCalculatorCreator typesCalculatorCreator,
                                 EventBus eventBus)
    {
        _computerPlayerSettings = computerPlayerSettings ?? throw new ArgumentNullException(nameof(computerPlayerSettings));
        _typesCalculatorCreator = typesCalculatorCreator ?? throw new ArgumentNullException(nameof(typesCalculatorCreator));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public ComputerPlayer Create()
    {
        ComputerPlayer computerPlayer = new ComputerPlayer(_eventBus,
                                                           new TruckSelector(_eventBus,
                                                                             _typesCalculatorCreator.Create()),
                                                           _computerPlayerSettings.StartDelay,
                                                           _computerPlayerSettings.MinFrequency,
                                                           _computerPlayerSettings.MaxFrequency);

        _eventBus.Invoke(new CreatedSignal<ICommandCreator>(computerPlayer));

        return computerPlayer;
    }
}