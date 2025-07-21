using System;
using System.Collections.Generic;

public class Rotator : ITickable
{
    private readonly TickEngine _tickEngine;
    private readonly IModelPositionObserver _positionsObserver;
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
        _positionsObserver = modelAddedNotifier ?? throw new ArgumentNullException(nameof(modelAddedNotifier));
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

    public void Enable()
    {
        if (_isRunned == false)
        {
            _isRunned = true;
            _positionsObserver.PositionsChanged += AddModels;
            _positionsObserver.PositionChanged += AddModel;

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

            RotateModel(model, frameRotation);
        }
    }

    public void Disable()
    {
        if (_isRunned)
        {
            _isRunned = false;
            _tickEngine.RemoveTickable(this);

            _positionsObserver.PositionsChanged -= AddModels;
            _positionsObserver.PositionChanged -= AddModel;
        }
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
        }
    }

    private void RotateModel(Model model, float frameRotation)
    {
        if (model.CurrentAngleToDirectionToTarget > _minAngleToTargetDirection || model.CurrentAngleToDirectionToTarget > frameRotation)
        {
            model.Rotate(frameRotation);
        }
        else
        {
            CompleteRotation(model);
        }
    }

    private void CompleteRotation(Model model)
    {
        OnDestroyed(model);
        model.FinishRotate();
    }

    private void OnDestroyed(Model destroyedModel)
    {
        destroyedModel.Destroyed -= OnDestroyed;
        _rotatables.Remove(destroyedModel);
    }
}