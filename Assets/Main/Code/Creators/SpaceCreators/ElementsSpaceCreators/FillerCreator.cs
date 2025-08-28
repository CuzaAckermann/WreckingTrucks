using System;

public class FillerCreator
{
    private readonly StopwatchCreator _stopwatchCreator;

    public FillerCreator(StopwatchCreator stopwatchCreator)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
    }

    public Filler Create(FillerSettings settings, IFillable fillable, FillingCard fillingCard)
    {
        Filler filler = new Filler(fillable, _stopwatchCreator.Create(), fillingCard);

        if (settings.RowFillerSettings.IsUsing)
        {
            filler.AddFillingStrategy(new RowFiller(settings.RowFillerSettings.Frequency));
        }

        if (settings.CascadeFillerSettings.IsUsing)
        {
            filler.AddFillingStrategy(new CascadeFiller(settings.CascadeFillerSettings.Frequency));
        }

        return filler;
    }
}