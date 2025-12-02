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

    private bool _isRunned;

    public MoverByPath(TickEngine tickEngine,
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