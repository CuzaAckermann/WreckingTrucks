using System;
using System.Collections.Generic;
using System.Linq;

public class LockedStorage<T> where T : ITargetAction
{
    private readonly HashSet<T> _active;

    private readonly HashSet<T> _toAddActive;
    private readonly HashSet<T> _toRemoveActive;

    private bool _isLocked;

    public LockedStorage(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be positive");
        }

        _active = new HashSet<T>(capacity);
        _toAddActive = new HashSet<T>(capacity);
        _toRemoveActive = new HashSet<T>(capacity);

        _isLocked = false;
    }

    public void Register(T item)
    {
        SubscribeToCreated(item);
    }

    public void Lock()
    {
        _isLocked = true;
    }

    public void Unlock()
    {
        _isLocked = false;

        ProcessChanges();
    }

    public bool HasActive()
    {
        return _active.Count > 0;
    }

    public List<T> GetClearedActive()
    {
        return _active.Where(model => model != null && _toRemoveActive.Contains(model) == false).ToList();
    }

    private void ProcessChanges()
    {
        if (_toRemoveActive.Count > 0)
        {
            foreach (var item in _toRemoveActive)
            {
                if (_active.Contains(item))
                {
                    UnsubscribeFromActive(item);

                    _active.Remove(item);
                }
            }

            _toRemoveActive.Clear();
        }

        if (_toAddActive.Count > 0)
        {
            foreach (var item in _toAddActive)
            {
                if (_active.Contains(item) == false)
                {
                    _active.Add(item);

                    SubscribeToActive(item);
                }
            }

            _toAddActive.Clear();
        }
    }

    private void SubscribeToCreated(T item)
    {
        item.Destroyed += OnDestroyed;
        item.TargetChanged += OnTargetChanged;
    }

    private void UnsubscribeFromCreated(T item)
    {
        item.Destroyed -= OnDestroyed;
        item.TargetChanged -= OnTargetChanged;
    }

    private void SubscribeToActive(T item)
    {
        item.TargetReached += OnTargetReached;
    }

    private void UnsubscribeFromActive(T item)
    {
        item.TargetReached -= OnTargetReached;
    }

    private void OnDestroyed(IDestroyable item)
    {
        if (Validator.IsRequiredType(item, out T targetAction) == false)
        {
            return;
        }

        UnsubscribeFromCreated(targetAction);

        if (Validator.IsContains(_active, targetAction) == false)
        {
            return;
        }

        Remove(targetAction);
    }

    private void OnTargetChanged(ITargetAction item)
    {
        if (Validator.IsRequiredType(item, out T targetAction) == false)
        {
            return;
        }

        if (Validator.IsContains(_active, targetAction))
        {
            return;
        }

        Add(targetAction);
    }

    private void Add(T item)
    {
        if (_isLocked)
        {
            _toAddActive.Add(item);
            _toRemoveActive.Remove(item);
        }
        else
        {
            _active.Add(item);

            SubscribeToActive(item);
        }
    }

    private void OnTargetReached(ITargetAction item)
    {
        if (Validator.IsRequiredType(item, out T targetAction) == false)
        {
            return;
        }

        if (Validator.IsContains(_active, targetAction) == false)
        {
            return;
        }

        Remove(targetAction);
    }

    private void Remove(T item)
    {
        if (_isLocked)
        {
            _toRemoveActive.Add(item);
            _toAddActive.Remove(item);
        }
        else
        {
            //if (_active.Remove(item))
            //{
            //    UnsubscribeFromActive(item);
            //}

            _active.Remove(item);

            UnsubscribeFromActive(item);
        }
    }
}