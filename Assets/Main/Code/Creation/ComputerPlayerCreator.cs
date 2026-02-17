using System;

public class ComputerPlayerCreator
{
    private readonly EventBus _eventBus;
    private readonly ComputerPlayerSettings _computerPlayerSettings;
    private readonly TypesCalculatorCreator _typesCalculatorCreator;

    public ComputerPlayerCreator(ComputerPlayerSettings computerPlayerSettings,
                                 EventBus eventBus)
    {
        Validator.ValidateNotNull(computerPlayerSettings, eventBus);

        _computerPlayerSettings = computerPlayerSettings;
        _eventBus = eventBus;
        _typesCalculatorCreator = new TypesCalculatorCreator();
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