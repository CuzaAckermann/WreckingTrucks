using System;
using System.Collections.Generic;

public class FillingStrategiesCreator
{
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly Random _random;

    public FillingStrategiesCreator(StopwatchCreator stopwatchCreator)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _random = new Random();
    }

    public FillingStrategy Create(FillerSettings fillerSettings)
    {
        List<FillingStrategy> fillingStrategies = new List<FillingStrategy>();

        if (fillerSettings.RowFillerSettings.IsUsing)
        {
            fillingStrategies.Add(new RowFiller(_stopwatchCreator.Create(), fillerSettings.RowFillerSettings.Frequency));
        }

        if (fillerSettings.CascadeFillerSettings.IsUsing)
        {
            fillingStrategies.Add(new CascadeFiller(_stopwatchCreator.Create(), fillerSettings.CascadeFillerSettings.Frequency));
        }

        FillingStrategy fillingStrategy = fillingStrategies[_random.Next(0, fillingStrategies.Count)];

        return fillingStrategy;
    }
}