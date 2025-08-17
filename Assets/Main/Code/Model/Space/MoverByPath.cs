using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverByPath : ITickable
{
    private readonly TickEngine _tickEngine;
    private readonly Dictionary<Model, List<Vector3>> _movablesByPath;
    private readonly float _movementSpeed;
    private readonly float _minSqrDistanceToTargetPosition;

    private IModelPositionObserver _positionsObserver;
    private bool _isRunned;

    public MoverByPath(TickEngine tickEngine,
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
        _movablesByPath = new Dictionary<Model, List<Vector3>>(capacity);
        _movementSpeed = movementSpeed;
        _minSqrDistanceToTargetPosition = minSqrDistanceToTargetPosition;
    }

    public void Clear()
    {
        foreach (var model in _movablesByPath)
        {
            if (model.Key != null)
            {
                model.Key.Destroyed -= OnDestroyed;
                model.Value.Clear();
            }
        }

        Disable();
        _movablesByPath.Clear();
    }

    public void Enable()
    {
        if (_isRunned == false)
        {
            _isRunned = true;

            _positionsObserver.PositionsChanged += AddModels;
            _positionsObserver.ModelPositionChanged += AddModel;

            _tickEngine.AddTickable(this);
        }
    }

    public void Tick(float deltaTime)
    {
        if (_movablesByPath.Count == 0)
        {
            return;
        }

        List<Model> removedModels = new List<Model>();

        float frameMovement = _movementSpeed * deltaTime;
        float sqrFrameMovement = frameMovement * frameMovement;

        foreach (var model in _movablesByPath)
        {
            if (model.Key == null)
            {
                removedModels.Add(model.Key);
                continue;
            }

            MoveModel(model.Key, frameMovement, sqrFrameMovement);
        }

        if (removedModels.Count > 0)
        {
            for (int i = 0; i < removedModels.Count; i++)
            {
                _movablesByPath.Remove(removedModels[i]);
            }
        }
    }

    public void Disable()
    {
        if (_isRunned)
        {
            _isRunned = false;

            _tickEngine.RemoveTickable(this);

            _positionsObserver.PositionsChanged -= AddModels;
            _positionsObserver.ModelPositionChanged -= AddModel;
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

        if (_movablesByPath.ContainsKey(model))
        {
            List<Vector3> path = _movablesByPath[model];
            path.Add(model.TargetPosition);
        }
        else
        {
            _movablesByPath.Add(model, new List<Vector3>() { model.TargetPosition});
            model.Destroyed += OnDestroyed;
        }
    }

    private void MoveModel(Model model, float frameMovement, float sqrFrameMovement)
    {
        float sqrDistanceToTarget = model.DirectionToTarget.sqrMagnitude;

        if (sqrDistanceToTarget <= _minSqrDistanceToTargetPosition || sqrDistanceToTarget <= sqrFrameMovement)
        {
            if (TryGetNextTargetPosition(model) == false)
            {
                return;
            }
        }

        if (sqrDistanceToTarget > sqrFrameMovement)
        {
            model.Move(frameMovement);
        }
    }

    private bool TryGetNextTargetPosition(Model model)
    {
        model.SetPosition(_movablesByPath[model][0]);
        _movablesByPath[model].RemoveAt(0);

        if (_movablesByPath[model].Count > 0)
        {
            model.SetTargetPosition(_movablesByPath[model][0]);
            return true;
        }
        else
        {
            OnDestroyed(model);
            return false;
        }
    }

    private void OnDestroyed(Model destroyedModel)
    {
        destroyedModel.Destroyed -= OnDestroyed;
        _movablesByPath[destroyedModel].Clear();
        _movablesByPath.Remove(destroyedModel);
    }
}