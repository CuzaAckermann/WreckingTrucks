using System;
using System.Collections.Generic;
using System.Linq;

public class TickEngine
{
    private readonly EventBus _eventBus;

    private readonly List<ITickableCreator> _tickableCreators;

    private readonly List<ITickable> _activatedTickables;
    private readonly List<ITickable> _toAdd;
    private readonly List<ITickable> _toRemove;

    private bool _isUpdating;
    private bool _isPaused;

    public TickEngine(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _tickableCreators = new List<ITickableCreator>();

        _activatedTickables = new List<ITickable>();
        _toAdd = new List<ITickable>();
        _toRemove = new List<ITickable>();

        _isUpdating = false;
        _isPaused = true;

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<EnabledSignal<GameSignalEmitter>>(Start);
        _eventBus.Subscribe<UpdateSignal>(Tick);
    }

    public void AddTickableCreator(ITickableCreator tickableCreator)
    {
        if (tickableCreator == null)
        {
            throw new ArgumentNullException(nameof(tickableCreator));
        }

        if (_tickableCreators.Contains(tickableCreator))
        {
            throw new InvalidOperationException($"{nameof(tickableCreator)} is already added");
        }

        _tickableCreators.Add(tickableCreator);

        SubscribeToTickableCreator(tickableCreator);
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Continue()
    {
        _isPaused = false;
    }

    private void Clear(ClearedSignal<GameSignalEmitter> _)
    {
        _toAdd.Clear();
        _toRemove.Clear();

        for (int tickableCreator = 0; tickableCreator < _tickableCreators.Count; tickableCreator++)
        {
            UnsubscribeFromTickableCreator(_tickableCreators[tickableCreator]);
        }

        _eventBus.Unsubscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Unsubscribe<EnabledSignal<GameSignalEmitter>>(Start);
        _eventBus.Unsubscribe<UpdateSignal>(Tick);
    }

    private void Start(EnabledSignal<GameSignalEmitter> _)
    {
        Continue();
    }

    private void Tick(UpdateSignal updateSignal)
    {
        if (_isPaused)
        {
            return;
        }

        if (_activatedTickables.Count == 0)
        {
            return;
        }

        _isUpdating = true;

        for (int i = 0; i < _activatedTickables.Count; i++)
        {
            ITickable tickable = _activatedTickables[i];

            if (_toRemove.Contains(tickable))
            {
                continue;
            }

            tickable.Tick(updateSignal.DeltaTime);
        }

        _isUpdating = false;

        ProcessChangeAmountTickables();
    }

    private void SubscribeToTickableCreator(ITickableCreator tickableCreator)
    {
        tickableCreator.TickableCreated += OnCreated;
    }

    private void UnsubscribeFromTickableCreator(ITickableCreator tickableCreator)
    {
        tickableCreator.TickableCreated -= OnCreated;
    }

    private void OnCreated(ITickable tickable)
    {
        SubscribeToCreatedTickable(tickable);
    }

    private void SubscribeToCreatedTickable(ITickable tickable)
    {
        tickable.DestroyedIDestroyable += UnsubscribeFromCreatedTickable;

        tickable.Activated += AddTickable;
        tickable.Deactivated += RemoveTickable;
    }

    private void UnsubscribeFromCreatedTickable(IDestroyable destroyable)
    {
        if (destroyable is not ITickable tickable)
        {
            return;
        }

        tickable.DestroyedIDestroyable -= UnsubscribeFromCreatedTickable;

        tickable.Activated -= AddTickable;
        tickable.Deactivated -= RemoveTickable;

        //RemoveTickable(tickable);
    }

    private void AddTickable(ITickable tickable)
    {
        if (tickable == null)
        {
            throw new ArgumentNullException(nameof(tickable));
        }

        if (_activatedTickables.Contains(tickable))
        {
            //Logger.Log(tickable.GetType());

            if (_toRemove.Contains(tickable))
            {
                //Logger.Log($"{nameof(tickable)} is contained in {nameof(_toRemove)} and will be removed from {nameof(_toRemove)}");
                //Logger.Log($"{nameof(tickable)} will not removed from {nameof(_activatedTickables)}");

                _toRemove.Remove(tickable);

                return;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(tickable)} already added.");
            }
        }

        if (_isUpdating)
        {
            _toAdd.Add(tickable);
            _toRemove.Remove(tickable);
        }
        else
        {
            _activatedTickables.Add(tickable);
        }
    }

    private void RemoveTickable(ITickable tickable)
    {
        if (tickable == null)
        {
            throw new ArgumentNullException(nameof(tickable));
        }

        if (_isUpdating)
        {
            _toRemove.Add(tickable);
            _toAdd.Remove(tickable);
        }
        else if (_activatedTickables.Remove(tickable) == false)
        {
            throw new InvalidOperationException($"{nameof(tickable)} not found.");
        }
    }

    private void ProcessChangeAmountTickables()
    {
        if (_toRemove.Count > 0)
        {
            _activatedTickables.RemoveAll(tickable => _toRemove.Contains(tickable));
            _toRemove.Clear();
        }

        if (_toAdd.Count > 0)
        {
            _activatedTickables.AddRange(_toAdd.Except(_activatedTickables));
            _toAdd.Clear();
        }
    }
}