using System;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : ITickable
{
    private readonly TickEngine _tickEngine;
    private readonly List<IModelPositionObserver> _positionsObservers;
    private readonly List<Model> _rotatables;
    private readonly float _speedRotation;
    private readonly float _minAngleToTargetDirection;

    private bool _isRunned;

    public Rotator(TickEngine tickEngine,
                   IModelPositionObserver modelAddedNotifier,
                   int capacityCollection,
                   float speedRotation,
                   float minAngleToTargetDirection)
    {
        if (capacityCollection <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacityCollection));
        }

        if (speedRotation <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(speedRotation));
        }

        if (minAngleToTargetDirection < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minAngleToTargetDirection));
        }

        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));

        //_positionsObserver = modelAddedNotifier ?? throw new ArgumentNullException(nameof(modelAddedNotifier));
        _positionsObservers = new List<IModelPositionObserver>();
        AddNotifier(modelAddedNotifier);

        _rotatables = new List<Model>(capacityCollection);
        _speedRotation = speedRotation;
        _minAngleToTargetDirection = minAngleToTargetDirection;

        _isRunned = false;
    }

    public void Clear()
    {
        foreach (Model model in _rotatables)
        {
            if (model != null)
            {
                model.Destroyed -= OnDestroyed;
            }
        }

        Disable();
        _rotatables.Clear();
    }

    public void AddNotifier(IModelPositionObserver modelAddedNotifier)
    {
        _positionsObservers.Add(modelAddedNotifier);
    }

    public void Enable()
    {
        if (_isRunned == false)
        {
            _isRunned = true;

            for (int i = 0; i < _positionsObservers.Count; i++)
            {
                SubscribeToNotifier(_positionsObservers[i]);
            }

            _tickEngine.AddTickable(this);
        }
    }

    public void Tick(float deltaTime)
    {
        if (_rotatables.Count == 0)
        {
            return;
        }

        float frameRotation = _speedRotation * deltaTime;

        for (int i = _rotatables.Count - 1; i >= 0; i--)
        {
            Model model = _rotatables[i];

            if (model == null)
            {
                _rotatables.RemoveAt(i);
                continue;
            }

            model.Rotate(frameRotation);
        }
    }

    public void Disable()
    {
        if (_isRunned)
        {
            _isRunned = false;
            _tickEngine.RemoveTickable(this);

            for (int i = 0; i < _positionsObservers.Count; i++)
            {
                UnsubscribeFromNotifier(_positionsObservers[i]);
            }
        }
    }

    private void SubscribeToNotifier(IModelPositionObserver observer)
    {
        observer.PositionsChanged += AddModels;
        observer.ModelPositionChanged += AddModel;
    }

    private void UnsubscribeFromNotifier(IModelPositionObserver observer)
    {
        observer.PositionsChanged -= AddModels;
        observer.ModelPositionChanged -= AddModel;
    }

    private void AddModels(List<Model> models)
    {
        if (models == null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        foreach (var model in models)
        {
            AddModel(model);
        }
    }

    private void AddModel(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (_rotatables.Contains(model) == false)
        {
            _rotatables.Add(model);
            model.Destroyed += OnDestroyed;
            model.TargetRotationReached += OnTargetRotationReached;
        }
    }

    private void OnTargetRotationReached(Model model)
    {
        model.TargetRotationReached -= OnTargetRotationReached;
        OnDestroyed(model);
    }

    private void OnDestroyed(Model destroyedModel)
    {
        destroyedModel.Destroyed -= OnDestroyed;
        _rotatables.Remove(destroyedModel);
    }
}