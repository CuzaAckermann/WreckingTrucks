using System;
using System.Collections.Generic;

public class Mover : ITickable
{
    private readonly TickEngine _tickEngine;
    private readonly List<Model> _movables;
    private readonly float _movementSpeed;
    private readonly float _minSqrDistanceToTargetPosition;

    private IModelPositionObserver _positionsObserver;
    private bool _isRunned;

    public Mover(TickEngine tickEngine,
                 IModelPositionObserver positionsObserver,
                 int capacity,
                 float movementSpeed,
                 float minSqrDistanceToTargetPosition)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be positive");
        }

        if (movementSpeed <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(movementSpeed)} must be positive");
        }

        if (minSqrDistanceToTargetPosition < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(minSqrDistanceToTargetPosition)} cannot negative");
        }

        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
        _positionsObserver = positionsObserver ?? throw new ArgumentNullException(nameof(positionsObserver));
        _movables = new List<Model>(capacity);
        _movementSpeed = movementSpeed;
        _minSqrDistanceToTargetPosition = minSqrDistanceToTargetPosition;
    }

    public void Clear()
    {
        foreach (Model model in _movables)
        {
            if (model != null)
            {
                model.Destroyed -= OnDestroyed;
            }
        }

        Disable();
        _movables.Clear();
    }

    public void Enable()
    {
        if (_isRunned == false)
        {
            _isRunned = true;

            _positionsObserver.PositionsChanged += AddModels;
            _positionsObserver.PositionChanged += AddModel;
            _positionsObserver.PositionReached += OnDestroyed;

            _tickEngine.AddTickable(this);
        }
    }

    public void Tick(float deltaTime)
    {
        if (_movables.Count == 0)
        {
            return;
        }

        float frameMovement = _movementSpeed * deltaTime;

        for (int i = _movables.Count - 1; i >= 0; i--)
        {
            if (_movables[i] == null)
            {
                _movables.RemoveAt(i);
                continue;
            }

            MoveModel(_movables[i], frameMovement);
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
            _positionsObserver.PositionReached -= OnDestroyed;
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

        if (_movables.Contains(model) == false)
        {
            _movables.Add(model);
            model.Destroyed += OnDestroyed;
            //model.TargetPositionReached += OnDestroyed;
        }
    }

    private void MoveModel(Model model, float frameMovement)
    {
        model.Move(frameMovement);
    }

    private void OnDestroyed(Model destroyedModel)
    {
        destroyedModel.Destroyed -= OnDestroyed;
        //destroyedModel.TargetPositionReached -= OnDestroyed;
        _movables.Remove(destroyedModel);
    }
}