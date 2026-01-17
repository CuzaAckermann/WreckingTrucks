using System;

public class BlockFillerCreator
{
    private readonly FillingStrategiesCreator _fillingStrategiesCreator;

    public BlockFillerCreator(FillingStrategiesCreator fillingStrategiesCreator)
    {
        _fillingStrategiesCreator = fillingStrategiesCreator ?? throw new ArgumentNullException(nameof(fillingStrategiesCreator));
    }

    public BlockFieldFiller Create(Field field, IRecordStorage recordStorage, EventBus eventBus)
    {
        return new BlockFieldFiller(_fillingStrategiesCreator.Create<Block>(field, recordStorage), eventBus);
    }

    public BlockFieldFiller CreateNonstop(Field field, IRecordStorage recordStorage, float frequency, EventBus eventBus)
    {
        return new BlockFieldFiller(_fillingStrategiesCreator.CreateRowFiller<Block>(field, recordStorage, frequency), eventBus);
    }
}