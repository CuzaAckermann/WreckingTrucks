using System;
using System.Collections.Generic;

public class Mover : ITickable, IClearable
{
    private Field _modelsField;
    private List<Model> _movableModels;
    private float _movementSpeed;
    private float _minSqrDistanceToTargetPosition;

    public Mover(Field modelsField, int capacity, float movementSpeed, float minSqrDistanceToTargetPosition)
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

        _modelsField = modelsField ?? throw new ArgumentNullException(nameof(modelsField));
        _movableModels = new List<Model>(capacity);
        _movementSpeed = movementSpeed;
        _minSqrDistanceToTargetPosition = minSqrDistanceToTargetPosition;

        _modelsField.ModelsAdded += AddModels;
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

            MoveBlock(_movableModels[i], frameMovement, sqrFrameMovement);
        }
    }

    public void Clear()
    {
        foreach (var model in _movableModels)
        {
            if (model != null)
            {
                model.Destroyed -= OnBlockDestroyed;
            }
        }

        _modelsField.ModelsAdded -= AddModels;
        _movableModels.Clear();
    }

    private void AddModels(List<Model> models)
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

            if (_movableModels.Contains(model) == false)
            {
                _movableModels.Add(model);
                model.Destroyed += OnBlockDestroyed;
            }
        }
    }

    private void MoveBlock(Model model, float frameMovement, float sqrFrameMovement)
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

    private void CompleteBlockMovement(Model model)
    {
        model.FinishMovement();
        model.Destroyed -= OnBlockDestroyed;
        _movableModels.Remove(model);
    }

    private void OnBlockDestroyed(Model destroyedModel)
    {
        destroyedModel.Destroyed -= OnBlockDestroyed;
        _movableModels.Remove(destroyedModel);
    }
}