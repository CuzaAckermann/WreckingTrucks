using System;
using System.Collections.Generic;

public class FillingStrategiesCreator
{
    private readonly EventBus _eventBus;
    private readonly Random _random;
    private readonly Production _production;
    private readonly FillerSettings _fillerSettings;

    public FillingStrategiesCreator(EventBus eventBus,
                                    FillerSettings fillerSettings,
                                    Production production)
    {
        Validator.ValidateNotNull(eventBus, fillerSettings, production);

        _eventBus = eventBus;
        _fillerSettings = fillerSettings;
        _production = production;
        _random = new Random();
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
        if (_production.TryCreate(out SpawnDetector spawnDetector) == false)
        {
            throw new InvalidOperationException();
        }

        RowFiller<M> rowFiller = new RowFiller<M>(frequency,
                                                  spawnDetector,
                                                  fillable.AmountColumns * fillable.AmountLayers,
                                                  _production,
                                                  new Placer(_eventBus),
                                                  _fillerSettings.RowFillerSettings.SpawnDistance);

        Logger.Log(rowFiller.GetType());
        _eventBus.Invoke(new CreatedSignal<ICommandCreator>(rowFiller));

        rowFiller.PrepareFilling(fillable, recordStorage);

        return rowFiller;
    }

    private RowFiller<M> CreateRowFiller<M>(IFillable fillable) where M : Model
    {
        if (_production.TryCreate(out SpawnDetector spawnDetector) == false)
        {
            throw new InvalidOperationException();
        }

        return new RowFiller<M>(_fillerSettings.RowFillerSettings.Frequency,
                                spawnDetector,
                                fillable.AmountColumns * fillable.AmountLayers,
                                _production,
                                new Placer(_eventBus),
                                _fillerSettings.RowFillerSettings.SpawnDistance);
    }

    private CascadeFiller<M> CreateCascadeFiller<M>() where M : Model
    {
        if (_production.TryCreate(out SpawnDetector spawnDetector) == false)
        {
            throw new InvalidOperationException();
        }

        return new CascadeFiller<M>(_fillerSettings.CascadeFillerSettings.Frequency,
                                    spawnDetector,
                                    _production,
                                    new Placer(_eventBus),
                                    _fillerSettings.CascadeFillerSettings.SpawnDistance);
    }
}