using UnityEngine;

public class DeltaTimeProvider : IApplicationAbility
{
    private readonly UpdateApplicationState _updateApplicationState;

    private readonly Amount _deltaTime;
    private readonly IAmount _deltaTimeFactor;

    public DeltaTimeProvider(UpdateApplicationState updateApplicationState,
                             IAmount deltaTimeFactor)
    {
        Validator.ValidateNotNull(updateApplicationState, deltaTimeFactor);

        _updateApplicationState = updateApplicationState;
        _deltaTimeFactor = deltaTimeFactor;

        _deltaTime = new Amount(Time.deltaTime);
    }

    public IAmount DeltaTime => _deltaTime;

    public void Start()
    {
        _updateApplicationState.Triggered += UpdateDeltaTime;
    }

    public void Finish()
    {
        _updateApplicationState.Triggered -= UpdateDeltaTime;
    }

    private void UpdateDeltaTime()
    {
        _deltaTime.Change(Time.deltaTime * _deltaTimeFactor.Value);
    }
}