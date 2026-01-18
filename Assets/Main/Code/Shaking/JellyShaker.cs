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

        _eventBus.Subscribe<GameStartedSignal>(Enable);
        _eventBus.Subscribe<GameEndedSignal>(Disable);

        _eventBus.Subscribe<GameClearedSignal>(Clear);
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

    private void Clear(GameClearedSignal _)
    {
        for (int i = _shackedJellies.Count - 1; i >= 0; i--)
        {
            RemoveJelly(_shackedJellies[i]);
        }

        _eventBus.Unsubscribe<GameStartedSignal>(Enable);
        _eventBus.Unsubscribe<GameEndedSignal>(Disable);

        _eventBus.Unsubscribe<GameClearedSignal>(Clear);
    }

    private void Enable(GameStartedSignal _)
    {
        Activated?.Invoke(this);
    }

    private void Disable(GameEndedSignal _)
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