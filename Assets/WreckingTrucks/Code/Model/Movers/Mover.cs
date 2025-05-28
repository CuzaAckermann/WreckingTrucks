using System;
using System.Collections.Generic;

public class Mover<T> : ITickable, IClearable where T : Model
{
    private List<T> _models;
    private float _movementSpeed;
    private float _minSqrDistanceToTargetPosition;

    public Mover(int capacity, float movementSpeed, float minSqrDistanceToTargetPosition)
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

        _models = new List<T>(capacity);
        _movementSpeed = movementSpeed;
        _minSqrDistanceToTargetPosition = minSqrDistanceToTargetPosition;
    }

    public void AddModels(List<T> models)
    {
        if (models == null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        foreach (var model in models)
        {
            if (model == null)
            {
                throw new ArgumentNullException($"{nameof(model)} in collection cannot be null");
            }

            if (_models.Contains(model) == false)
            {
                _models.Add(model);
                model.Destroyed += OnBlockDestroyed;
            }
        }
    }

    public void Tick(float deltaTime)
    {
        if (_models.Count == 0)
        {
            return;
        }

        float frameMovement = _movementSpeed * deltaTime;
        float sqrFrameMovement = frameMovement * frameMovement;

        for (int i = _models.Count - 1; i >= 0; i--)
        {
            if (_models[i] == null)
            {
                _models.Remove(_models[i]);
                continue;
            }

            MoveBlock(_models[i], frameMovement, sqrFrameMovement);
        }
    }

    public void Clear()
    {
        foreach (var model in _models)
        {
            if (model != null)
            {
                model.Destroyed -= OnBlockDestroyed;
            }
        }

        _models.Clear();
    }

    private void MoveBlock(T model, float frameMovement, float sqrFrameMovement)
    {
        float sqrDistanceToTarget = model.DirectionToTarget.sqrMagnitude;

        if (sqrDistanceToTarget <= _minSqrDistanceToTargetPosition)
        {
            CompleteBlockMovement(model);

            return;
        }

        if (sqrDistanceToTarget > sqrFrameMovement)
        {
            model.Move(frameMovement);
        }
        else
        {
            CompleteBlockMovement(model);
        }
    }

    private void CompleteBlockMovement(T model)
    {
        model.FinishMovement();
        model.Destroyed -= OnBlockDestroyed;
        _models.Remove(model);
    }

    private void OnBlockDestroyed(Model destroyedModel)
    {
        destroyedModel.Destroyed -= OnBlockDestroyed;
        _models.Remove((T)destroyedModel);
    }
}