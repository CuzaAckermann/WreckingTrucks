using System;
using System.Collections.Generic;

public class FillingStrategiesCreator
{
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly Random _random;
    private readonly SpawnDetectorFactory _spawnDetectorFactory;
    private readonly FillerSettings _fillerSettings;

    public FillingStrategiesCreator(StopwatchCreator stopwatchCreator,
                                    SpawnDetectorFactory spawnDetectorFactory,
                                    FillerSettings fillerSettings)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _random = new Random();
        _spawnDetectorFactory = spawnDetectorFactory ? spawnDetectorFactory : throw new ArgumentNullException(nameof(spawnDetectorFactory));
        _fillerSettings = fillerSettings ?? throw new ArgumentNullException(nameof(fillerSettings));
    }

    public FillingStrategy Create(IFillable fillable, IRecordStorage recordStorage)
    {
        List<FillingStrategy> fillingStrategies = new List<FillingStrategy>();

        if (_fillerSettings.RowFillerSettings.IsUsing)
        {
            fillingStrategies.Add(new RowFiller(_stopwatchCreator.Create(),
                                                _fillerSettings.RowFillerSettings.Frequency,
                                                _spawnDetectorFactory.Create(),
                                                fillable.AmountColumns * fillable.AmountLayers));
        }

        if (_fillerSettings.CascadeFillerSettings.IsUsing)
        {
            fillingStrategies.Add(new CascadeFiller(_stopwatchCreator.Create(),
                                                    _fillerSettings.CascadeFillerSettings.Frequency,
                                                    _spawnDetectorFactory.Create()));
        }

        FillingStrategy fillingStrategy = fillingStrategies[_random.Next(0, fillingStrategies.Count)];
        fillingStrategy.PrepareFilling(fillable, recordStorage);

        return fillingStrategy;
    }
}