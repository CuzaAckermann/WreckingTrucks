using System;
using System.Collections.Generic;

public class Mover<T> : ITickable, IClearable where T : Model
{
    private List<T> _entities;
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

        _entities = new List<T>(capacity);
        _movementSpeed = movementSpeed;
        _minSqrDistanceToTargetPosition = minSqrDistanceToTargetPosition;
    }

    public void AddModels(List<T> entities)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        foreach (var entity in entities)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} in collection cannot be null");
            }

            if (_entities.Contains(entity) == false)
            {
                _entities.Add(entity);
                entity.Destroyed += OnBlockDestroyed;
            }
        }
    }

    public void Tick(float deltaTime)
    {
        if (_entities.Count == 0)
        {
            return;
        }

        float frameMovement = _movementSpeed * deltaTime;
        float sqrFrameMovement = frameMovement * frameMovement;

        for (int i = _entities.Count - 1; i >= 0; i--)
        {
            if (_entities[i] == null)
            {
                _entities.Remove(_entities[i]);
                continue;
            }

            MoveBlock(_entities[i], frameMovement, sqrFrameMovement);
        }
    }

    public void Clear()
    {
        foreach (var entity in _entities)
        {
            if (entity != null)
            {
                entity.Destroyed -= OnBlockDestroyed;
            }
        }

        _entities.Clear();
    }

    private void MoveBlock(T entity, float frameMovement, float sqrFrameMovement)
    {
        float sqrDistanceToTarget = entity.DirectionToTarget.sqrMagnitude;

        if (sqrDistanceToTarget <= _minSqrDistanceToTargetPosition)
        {
            CompleteBlockMovement(entity);

            return;
        }

        if (sqrDistanceToTarget > sqrFrameMovement)
        {
            entity.Move(frameMovement);
        }
        else
        {
            CompleteBlockMovement(entity);
        }
    }

    private void CompleteBlockMovement(T entity)
    {
        entity.FinishMovement();
        entity.Destroyed -= OnBlockDestroyed;
        _entities.Remove(entity);
    }

    private void OnBlockDestroyed(Model destroyedEntity)
    {
        destroyedEntity.Destroyed -= OnBlockDestroyed;
        _entities.Remove((T)destroyedEntity);
    }
}