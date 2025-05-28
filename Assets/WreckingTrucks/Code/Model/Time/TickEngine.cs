using System;
using System.Collections.Generic;
using System.Linq;

public class TickEngine
{
    private readonly List<ITickable> _tickables = new List<ITickable>();
    private readonly List<ITickable> _toAdd = new List<ITickable>();
    private readonly List<ITickable> _toRemove = new List<ITickable>();

    private bool _isUpdating = false;
    private bool _isPaused = false;

    public void Tick(float deltaTime)
    {
        if (_isPaused || _tickables.Count == 0)
        {
            return;
        }

        _isUpdating = true;

        for (int i = 0; i < _tickables.Count; i++)
        {
            ITickable tickable = _tickables[i];

            if (_toRemove.Contains(tickable) == false)
            {
                tickable.Tick(deltaTime);
            }
        }

        _isUpdating = false;

        ProcessChangeAmountTickables();
    }

    public void AddTickable(ITickable tickable)
    {
        if (tickable == null)
        {
            throw new ArgumentNullException(nameof(tickable));
        }

        if (_tickables.Contains(tickable))
        {
            throw new InvalidOperationException($"{nameof(tickable)} already added.");
        }

        if (_isUpdating)
        {
            _toAdd.Add(tickable);
            _toRemove.Remove(tickable);
        }
        else
        {
            _tickables.Add(tickable);
        }
    }

    public void RemoveTickable(ITickable tickable)
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
        else if (_tickables.Remove(tickable) == false)
        {
            throw new InvalidOperationException($"{nameof(tickable)} not found.");
        }
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Continue()
    {
        _isPaused = false;
    }

    public void ClearAll()
    {
        _tickables.Clear();
        _toAdd.Clear();
        _toRemove.Clear();
    }

    private void ProcessChangeAmountTickables()
    {
        if (_toRemove.Count > 0)
        {
            _tickables.RemoveAll(tickable => _toRemove.Contains(tickable));
            _toRemove.Clear();
        }

        if (_toAdd.Count > 0)
        {
            _tickables.AddRange(_toAdd.Except(_tickables));
            _toAdd.Clear();
        }
    }
}