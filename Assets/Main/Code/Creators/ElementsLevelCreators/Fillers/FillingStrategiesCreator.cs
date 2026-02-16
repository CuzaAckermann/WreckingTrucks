using System;
using System.Collections.Generic;

public class FillingStrategiesCreator
{
    private readonly EventBus _eventBus;
    private readonly ModelProductionCreator _modelProductionCreator;
    private readonly Random _random;
    private readonly SpawnDetectorFactory _spawnDetectorFactory;
    private readonly FillerSettings _fillerSettings;

    public FillingStrategiesCreator(EventBus eventBus,
                                    ModelProductionCreator modelProductionCreator,
                                    SpawnDetectorFactory spawnDetectorFactory,
                                    FillerSettings fillerSettings)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
        _random = new Random();
        _spawnDetectorFactory = spawnDetectorFactory ? spawnDetectorFactory : throw new ArgumentNullException(nameof(spawnDetectorFactory));
        _fillerSettings = fillerSettings ?? throw new ArgumentNullException(nameof(fillerSettings));
    }

    public FillingStrategy<M> Create<M>(IFillable fillable, IRecordStorage recordStorage) where M : Model
    {
        List<FillingStrategy<M>> fillingStrategies = new List<FillingStrategy<M>>();

        if (_fillerSettings.RowFillerSettings.IsUsing)
        {
            fillingStrategies.Add(CreateRowFiller<M>(fillable));
        }

        if (_fillerSettings.CascadeFillerSettings.IsUsing)
        {
            fillingStrategies.Add(CreateCascadeFiller<M>());
        }

        FillingStrategy<M> fillingStrategy = fillingStrategies[_random.Next(0, fillingStrategies.Count)];

        //Logger.Log(fillingStrategy.GetType());
        _eventBus.Invoke(new CreatedSignal<ICommandCreator>(fillingStrategy));

        fillingStrategy.PrepareFilling(fillable, recordStorage);

        return fillingStrategy;
    }

    // for NonStop Game
    public FillingStrategy<M> CreateRowFiller<M>(IFillable fillable, IRecordStorage recordStorage, float frequency) where M : Model
    {
        RowFiller<M> rowFiller = new RowFiller<M>(frequency,
                                                  _spawnDetectorFactory.Create(),
                                                  fillable.AmountColumns * fillable.AmountLayers,
                                                  _modelProductionCreator.CreateFactory<M>(),
                                                  new Placer(_eventBus),
                                                  _fillerSettings.RowFillerSettings.SpawnDistance);

        Logger.Log(rowFiller.GetType());
        _eventBus.Invoke(new CreatedSignal<ICommandCreator>(rowFiller));

        rowFiller.PrepareFilling(fillable, recordStorage);

        return rowFiller;
    }

    private RowFiller<M> CreateRowFiller<M>(IFillable fillable) where M : Model
    {
        return new RowFiller<M>(_fillerSettings.RowFillerSettings.Frequency,
                                _spawnDetectorFactory.Create(),
                                fillable.AmountColumns * fillable.AmountLayers,
                                _modelProductionCreator.CreateFactory<M>(),
                                new Placer(_eventBus),
                                _fillerSettings.RowFillerSettings.SpawnDistance);
    }

    private CascadeFiller<M> CreateCascadeFiller<M>() where M : Model
    {
        return new CascadeFiller<M>(_fillerSettings.CascadeFillerSettings.Frequency,
                                    _spawnDetectorFactory.Create(),
                                    _modelProductionCreator.CreateFactory<M>(),
                                    new Placer(_eventBus),
                                    _fillerSettings.CascadeFillerSettings.SpawnDistance);
    }
}