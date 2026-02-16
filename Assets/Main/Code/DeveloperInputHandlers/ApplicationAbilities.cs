using System.Collections.Generic;

public class ApplicationAbilities
{
    private readonly ApplicationStateStorage _applicationStateStorage;

    private readonly Subscriber _subscriber;

    private readonly List<IAbility> _abilities;

    public ApplicationAbilities(ApplicationStateStorage applicationStateStorage, List<IAbility> abilities)
    {
        Validator.ValidateNotNull(applicationStateStorage, abilities);

        _applicationStateStorage = applicationStateStorage;

        _abilities = abilities;

        _subscriber = new Subscriber(new SubscriptionUnsubscriptionPair(Subscribe, Unsubscribe));

        _subscriber.Subscribe();
    }

    private void Subscribe()
    {
        _applicationStateStorage.PrepareApplicationState.Triggered += OnPrepareApplicationStateTriggered;
        _applicationStateStorage.StopApplicationState.Triggered += OnStopApplicationStateTriggered;
    }

    private void Unsubscribe()
    {
        _applicationStateStorage.PrepareApplicationState.Triggered -= OnPrepareApplicationStateTriggered;
        _applicationStateStorage.StopApplicationState.Triggered -= OnStopApplicationStateTriggered;
    }

    private void OnPrepareApplicationStateTriggered()
    {
        for (int currentAbility = 0; currentAbility < _abilities.Count; currentAbility++)
        {
            _abilities[currentAbility].Start();
        }
    }

    private void OnStopApplicationStateTriggered()
    {
        _subscriber.Unsubscribe();

        for (int currentAbility = 0; currentAbility < _abilities.Count; currentAbility++)
        {
            _abilities[currentAbility].Finish();
        }
    }
}