using System;
using System.Collections.Generic;

public class JellyShaker : ITickable
{
    private readonly EventBus _eventBus;

    private readonly List<Jelly> _shakedJellies;

    public JellyShaker(int capacity, EventBus eventBus)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _shakedJellies = new List<Jelly>();

        _eventBus.Subscribe<ClearedSignal<ApplicationSignal>>(Clear);

        _eventBus.Subscribe<JellyShakedSignal>(AddJelly);

        _eventBus.Subscribe<EnabledSignal<ApplicationSignal>>(Enable);
        _eventBus.Subscribe<DisabledSignal<ApplicationSignal>>(Disable);
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

    private void Clear(ClearedSignal<ApplicationSignal> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<ApplicationSignal>>(Clear);

        _eventBus.Unsubscribe<JellyShakedSignal>(AddJelly);

        _eventBus.Unsubscribe<EnabledSignal<ApplicationSignal>>(Enable);
        _eventBus.Unsubscribe<DisabledSignal<ApplicationSignal>>(Disable);

        for (int i = _shakedJellies.Count - 1; i >= 0; i--)
        {
            RemoveJelly(_shakedJellies[i]);
        }
    }

    private void Enable(EnabledSignal<ApplicationSignal> _)
    {
        Activated?.Invoke(this);
    }

    private void Disable(DisabledSignal<ApplicationSignal> _)
    {
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