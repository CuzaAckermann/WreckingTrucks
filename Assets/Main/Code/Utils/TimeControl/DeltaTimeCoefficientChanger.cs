using System;

public class DeltaTimeFactorDefiner
{
    private readonly BackgroundInput _backgroundInput;
    private readonly ClampedAmount _deltaTimeFactor;

    public DeltaTimeFactorDefiner(BackgroundInput backgroundInput,
                                  DeltaTimeFactorSettings settings)
    {
        _backgroundInput = backgroundInput ?? throw new ArgumentNullException(nameof(backgroundInput));

        _deltaTimeFactor = new ClampedAmount(settings.Initial,
                                            settings.Min,
                                            settings.Max);

        SubscribeToBackgroundInput();
    }

    public IClampedAmount DeltaTimeFactor => _deltaTimeFactor;

    private void SubscribeToBackgroundInput()
    {
        _backgroundInput.TimeCoefficientInstalled += Change;
        _backgroundInput.TimeCoefficientIncreased += Increase;
        _backgroundInput.TimeCoefficientDecreased += Decrease;
    }

    private void UnsubscribeFromBackgroundInput()
    {
        _backgroundInput.TimeCoefficientInstalled -= Change;
        _backgroundInput.TimeCoefficientIncreased -= Increase;
        _backgroundInput.TimeCoefficientDecreased -= Decrease;
    }

    private void Increase(float magnificationValue)
    {
        _deltaTimeFactor.Increase(magnificationValue);
    }

    private void Decrease(float reductionValue)
    {
        _deltaTimeFactor.Decrease(reductionValue);
    }

    private void Change(float deltaTimeFactor)
    {
        _deltaTimeFactor.Change(deltaTimeFactor);
    }
}