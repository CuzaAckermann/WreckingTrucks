using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class FillingStrategiesCreator
{
    private readonly ModelProductionCreator _modelProductionCreator;
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly Random _random;
    private readonly SpawnDetectorFactory _spawnDetectorFactory;
    private readonly FillerSettings _fillerSettings;

    public FillingStrategiesCreator(ModelProductionCreator modelProductionCreator,
                                    StopwatchCreator stopwatchCreator,
                                    SpawnDetectorFactory spawnDetectorFactory,
                                    FillerSettings fillerSettings)
    {
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
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

        fillingStrategy.PrepareFilling(fillable, recordStorage);

        return fillingStrategy;
    }

    public FillingStrategy<M> CreateRowFiller<M>(IFillable fillable, IRecordStorage recordStorage, float frequency) where M : Model
    {
        RowFiller<M> rowFiller = new RowFiller<M>(_stopwatchCreator.Create(),
                                                  frequency,
                                                  _spawnDetectorFactory.Create(),
                                                  fillable.AmountColumns * fillable.AmountLayers,
                                                  _modelProductionCreator.CreateFactory<M>());
        rowFiller.PrepareFilling(fillable, recordStorage);

        return rowFiller;
    }

    private RowFiller<M> CreateRowFiller<M>(IFillable fillable) where M : Model
    {
        return new RowFiller<M>(_stopwatchCreator.Create(),
                                _fillerSettings.RowFillerSettings.Frequency,
                                _spawnDetectorFactory.Create(),
                                fillable.AmountColumns * fillable.AmountLayers,
                                _modelProductionCreator.CreateFactory<M>());
    }

    private CascadeFiller<M> CreateCascadeFiller<M>() where M : Model
    {
        return new CascadeFiller<M>(_stopwatchCreator.Create(),
                                    _fillerSettings.CascadeFillerSettings.Frequency,
                                    _spawnDetectorFactory.Create(),
                                    _modelProductionCreator.CreateFactory<M>());
    }
}