using System;
using System.Collections.Generic;

public class JellyShaker : ITickable
{
    private readonly EventBus _eventBus;

    private readonly List<Jelly> _shackedJellies;

    public JellyShaker(int capacity, EventBus eventBus)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _shackedJellies = new List<Jelly>();

        _eventBus.Subscribe<JellyShackedSignal>(AddJelly);

        _eventBus.Subscribe<EnabledSignal<Game>>(Enable);
        _eventBus.Subscribe<DisabledSignal<Game>>(Disable);

        _eventBus.Subscribe<ClearedSignal<Game>>(Clear);
    }

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public void Tick(float _)
    {
        if (_shackedJellies.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _shackedJellies.Count; i++)
        {
            _shackedJellies[i].Shake();
        }
    }

    private void Clear(ClearedSignal<Game> _)
    {
        for (int i = _shackedJellies.Count - 1; i >= 0; i--)
        {
            RemoveJelly(_shackedJellies[i]);
        }

        _eventBus.Unsubscribe<EnabledSignal<Game>>(Enable);
        _eventBus.Unsubscribe<DisabledSignal<Game>>(Disable);

        _eventBus.Unsubscribe<ClearedSignal<Game>>(Clear);
    }

    private void Enable(EnabledSignal<Game> _)
    {
        Activated?.Invoke(this);
    }

    private void Disable(DisabledSignal<Game> _)
    {
        Deactivated?.Invoke(this);
    }

    private void AddJelly(JellyShackedSignal jellyShackedSignal)
    {
        _shackedJellies.Add(jellyShackedSignal.Jelly);

        jellyShackedSignal.Jelly.HesitationFinished += RemoveJelly;
    }

    private void RemoveJelly(Jelly jelly)
    {
        jelly.HesitationFinished -= RemoveJelly;

        _shackedJellies.Remove(jelly);
    }
}