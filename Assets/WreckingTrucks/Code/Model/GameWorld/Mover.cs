using System;
using System.Collections.Generic;

public class Mover : ITickable, IClearable
{
    private readonly IPositionsModelsChangedNotifier _positionsModelsChangedNotifier;
    private readonly List<Model> _movableModels;
    private readonly float _movementSpeed;
    private readonly float _minSqrDistanceToTargetPosition;

    public Mover(IPositionsModelsChangedNotifier positionsModelsChangedNotifier,
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

        _positionsModelsChangedNotifier = positionsModelsChangedNotifier ?? throw new ArgumentNullException(nameof(positionsModelsChangedNotifier));
        _movableModels = new List<Model>(capacity);
        _movementSpeed = movementSpeed;
        _minSqrDistanceToTargetPosition = minSqrDistanceToTargetPosition;
    }

    public void Clear()
    {
        foreach (var model in _movableModels)
        {
            if (model != null)
            {
                model.Destroyed -= OnDestroyed;
            }
        }

        _positionsModelsChangedNotifier.TargetPositionsModelsChanged -= AddModels;
        _movableModels.Clear();
    }

    public void Enable()
    {
        _positionsModelsChangedNotifier.TargetPositionsModelsChanged += AddModels;
    }

    public void Disable()
    {
        _positionsModelsChangedNotifier.TargetPositionsModelsChanged -= AddModels;
    }

    public void Tick(float deltaTime)
    {
        if (_movableModels.Count == 0)
        {
            return;
        }

        float frameMovement = _movementSpeed * deltaTime;
        float sqrFrameMovement = frameMovement * frameMovement;

        for (int i = _movableModels.Count - 1; i >= 0; i--)
        {
            if (_movableModels[i] == null)
            {
                _movableModels.Remove(_movableModels[i]);
                continue;
            }

            MoveModel(_movableModels[i], frameMovement, sqrFrameMovement);
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

        if (_movableModels.Contains(model) == false)
        {
            _movableModels.Add(model);
            model.Destroyed += OnDestroyed;
        }
    }

    private void MoveModel(Model model, float frameMovement, float sqrFrameMovement)
    {
        float sqrDistanceToTarget = model.DirectionToTarget.sqrMagnitude;

        if (sqrDistanceToTarget <= _minSqrDistanceToTargetPosition)
        {
            CompleteMovement(model);

            return;
        }

        if (sqrDistanceToTarget <= sqrFrameMovement)
        {
            CompleteMovement(model);

            return;
        }

        if (sqrDistanceToTarget > sqrFrameMovement)
        {
            model.Move(frameMovement);
        }
    }

    private void CompleteMovement(Model model)
    {
        _movableModels.Remove(model);
        model.FinishMovement();
        model.Destroyed -= OnDestroyed;
    }

    private void OnDestroyed(Model destroyedModel)
    {
        destroyedModel.Destroyed -= OnDestroyed;
        _movableModels.Remove(destroyedModel);
    }
}