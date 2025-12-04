using System;
using System.Collections.Generic;

public class FillingStrategiesCreator
{
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly Random _random;
    private readonly SpawnDetectorFactory _spawnDetectorFactory;

    public FillingStrategiesCreator(StopwatchCreator stopwatchCreator, SpawnDetectorFactory spawnDetectorFactory)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _random = new Random();
        _spawnDetectorFactory = spawnDetectorFactory ? spawnDetectorFactory : throw new ArgumentNullException(nameof(spawnDetectorFactory));
    }

    public FillingStrategy Create(FillerSettings fillerSettings)
    {
        List<FillingStrategy> fillingStrategies = new List<FillingStrategy>();

        if (fillerSettings.RowFillerSettings.IsUsing)
        {
            fillingStrategies.Add(new RowFiller(_stopwatchCreator.Create(),
                                                fillerSettings.RowFillerSettings.Frequency,
                                                _spawnDetectorFactory.Create()));
        }

        if (fillerSettings.CascadeFillerSettings.IsUsing)
        {
            fillingStrategies.Add(new CascadeFiller(_stopwatchCreator.Create(),
                                                    fillerSettings.CascadeFillerSettings.Frequency,
                                                    _spawnDetectorFactory.Create()));
        }

        FillingStrategy fillingStrategy = fillingStrategies[_random.Next(0, fillingStrategies.Count)];

        return fillingStrategy;
    }
}