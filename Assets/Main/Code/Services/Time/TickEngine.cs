using System;
using System.Collections.Generic;
using System.Linq;

public class TickEngine
{
    private readonly List<ITickableCreator> _tickableCreators;

    private readonly List<ITickable> _activatedTickables;
    private readonly List<ITickable> _toAdd;
    private readonly List<ITickable> _toRemove;

    private bool _isUpdating;
    private bool _isPaused;

    public TickEngine()
    {
        _tickableCreators = new List<ITickableCreator>();

        _activatedTickables = new List<ITickable>();
        _toAdd = new List<ITickable>();
        _toRemove = new List<ITickable>();

        _isUpdating = false;
        _isPaused = true;
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

    public void Clear()
    {
        _toAdd.Clear();
        _toRemove.Clear();
    }

    public void Tick(float deltaTime)
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

            if (_toRemove.Contains(tickable) == false)
            {
                //Logger.Log(tickable.GetType());

                tickable.Tick(deltaTime);
            }
        }

        _isUpdating = false;

        ProcessChangeAmountTickables();
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Continue()
    {
        _isPaused = false;
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
        tickable.Activated += AddTickable;
        tickable.Deactivated += RemoveTickable;
    }

    private void UnsubscribeFromCreatedTickable(ITickable tickable)
    {
        tickable.Activated -= AddTickable;
        tickable.Deactivated -= RemoveTickable;
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