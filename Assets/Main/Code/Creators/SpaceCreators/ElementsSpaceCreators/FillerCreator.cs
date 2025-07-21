using System;

public class FillerCreator
{
    private readonly FillerCreatorSettings _settings;
    private readonly StopwatchCreator _stopwatchCreator;

    public FillerCreator(FillerCreatorSettings settings,
                         StopwatchCreator stopwatchCreator)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
    }

    public Filler Create(IFillable fillable)
    {
        Filler filler = new Filler(fillable, _stopwatchCreator.Create());

        if (_settings.UseRowFiller && _settings.RowFillerSettings != null)
        {
            filler.AddFillingStrategy(new RowFiller(_settings.RowFillerSettings.Frequency));
        }

        if (_settings.UseCascadeFiller && _settings.CascadeFillerSettings != null)
        {
            filler.AddFillingStrategy(new CascadeFiller(_settings.CascadeFillerSettings.Frequency));
        }

        return filler;
    }
}