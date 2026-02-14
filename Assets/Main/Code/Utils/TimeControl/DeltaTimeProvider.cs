using UnityEngine;

public class DeltaTimeProvider
{
    private readonly UpdateApplicationState _updateApplicationState;
    private readonly DeveloperInput _developerInput;

    private readonly Amount _deltaTime;
    private readonly ClampedAmount _deltaTimeFactor;

    public DeltaTimeProvider(UpdateApplicationState updateApplicationState,
                             DeveloperInput developerInput,
                             DeltaTimeFactorSettings settings)
    {
        Validator.ValidateNotNull(updateApplicationState, developerInput, settings);

        _updateApplicationState = updateApplicationState;
        _developerInput = developerInput;

        _deltaTime = new Amount(Time.deltaTime);
        _deltaTimeFactor = new ClampedAmount(settings.Initial,
                                             settings.Min,
                                             settings.Max);

        Subscribe();
    }

    public IAmount DeltaTime => _deltaTime;

    private void Subscribe()
    {
        _updateApplicationState.Updated += UpdateDeltaTime;

        //_backgroundInput.TimeCoefficientInstalled += Change;
        //_backgroundInput.TimeCoefficientIncreased += Increase;
        //_backgroundInput.TimeCoefficientDecreased += Decrease;
    }

    private void Unsubscribe()
    {
        _updateApplicationState.Updated -= UpdateDeltaTime;

        //_backgroundInput.TimeCoefficientInstalled -= Change;
        //_backgroundInput.TimeCoefficientIncreased -= Increase;
        //_backgroundInput.TimeCoefficientDecreased -= Decrease;
    }

    private void UpdateDeltaTime()
    {
        _deltaTime.Change(Time.deltaTime * _deltaTimeFactor.Value);
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