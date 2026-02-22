using System;
using System.Collections.Generic;

public class ApplicationAbilities
{
    private readonly ApplicationStateStorage _applicationStateStorage;

    private readonly Subscriber _subscriber;

    private readonly List<IApplicationAbility> _abilities;

    public ApplicationAbilities(ApplicationStateStorage applicationStateStorage, List<IApplicationAbility> abilities)
    {
        Validator.ValidateNotNull(applicationStateStorage, abilities);

        _applicationStateStorage = applicationStateStorage;

        _abilities = abilities;

        _subscriber = new Subscriber(Subscribe, Unsubscribe);

        _subscriber.Subscribe();
    }

    public bool TryGetAbility<T>(out T result) where T : IApplicationAbility
    {
        result = default;

        foreach (IApplicationAbility ability in _abilities)
        {
            if (ability.GetType() is T foundAbility)
            {
                result = foundAbility;

                break;
            }
        }

        return result != null;
    }

    private void Subscribe()
    {
        if (_applicationStateStorage.TryGet(out PrepareApplicationState prepareApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        if (_applicationStateStorage.TryGet(out StopApplicationState stopApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        prepareApplicationState.Triggered += OnPrepareApplicationStateTriggered;
        stopApplicationState.Triggered += OnStopApplicationStateTriggered;
    }

    private void Unsubscribe()
    {
        if (_applicationStateStorage.TryGet(out PrepareApplicationState prepareApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        if (_applicationStateStorage.TryGet(out StopApplicationState stopApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        prepareApplicationState.Triggered -= OnPrepareApplicationStateTriggered;
        stopApplicationState.Triggered -= OnStopApplicationStateTriggered;
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