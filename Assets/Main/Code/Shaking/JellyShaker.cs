using System;
using System.Collections.Generic;

public class JellyShaker : ITickable, IAbility
{
    private readonly EventBus _eventBus;

    private readonly List<Jelly> _shakedJellies;

    public JellyShaker(int capacity, EventBus eventBus)
    {
        Validator.ValidateNotNull(eventBus);
        Validator.ValidateMin(capacity, 0, true);

        _eventBus = eventBus;

        _shakedJellies = new List<Jelly>(capacity);
    }

    public event Action<IDestroyable> Destroyed;

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void Tick(float _)
    {
        if (_shakedJellies.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _shakedJellies.Count; i++)
        {
            _shakedJellies[i].Shake();
        }
    }

    public void Start()
    {
        _eventBus.Subscribe<JellyShakedSignal>(AddJelly);

        Activated?.Invoke(this);
    }

    public void Finish()
    {
        _eventBus.Unsubscribe<JellyShakedSignal>(AddJelly);

        for (int i = _shakedJellies.Count - 1; i >= 0; i--)
        {
            _shakedJellies[i].HesitationFinished -= RemoveJelly;
        }

        _shakedJellies.Clear();

        Deactivated?.Invoke(this);
    }

    private void AddJelly(JellyShakedSignal jellyShackedSignal)
    {
        _shakedJellies.Add(jellyShackedSignal.Jelly);

        jellyShackedSignal.Jelly.HesitationFinished += RemoveJelly;
    }

    private void RemoveJelly(Jelly jelly)
    {
        jelly.HesitationFinished -= RemoveJelly;

        _shakedJellies.Remove(jelly);
    }
}