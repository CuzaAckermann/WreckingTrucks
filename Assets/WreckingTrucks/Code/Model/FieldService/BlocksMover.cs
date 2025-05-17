using System;
using System.Collections.Generic;

public class BlocksMover : ITickable, IClearable
{
    private List<Block> _movableBlocks;
    private float _movementSpeed;
    private float _minSqrDistanceToTargetPosition;

    public BlocksMover(int capacity, float movementSpeed, float minDistanceToTargetPosition)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be positive");
        }

        if (movementSpeed <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(movementSpeed), "Movement speed must be positive");
        }
        
        if (minDistanceToTargetPosition <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minDistanceToTargetPosition), "Distance must be positive");
        }

        _movableBlocks = new List<Block>(capacity);
        _movementSpeed = movementSpeed;
        _minSqrDistanceToTargetPosition = minDistanceToTargetPosition * minDistanceToTargetPosition;
    }

    public void AddBlocks(IEnumerable<Block> blocks)
    {
        if (blocks == null)
        {
            throw new ArgumentNullException(nameof(blocks));
        }

        foreach (var block in blocks)
        {
            if (block == null)
            {
                throw new ArgumentNullException(nameof(block), "Block in collection cannot be null");
            }

            if (_movableBlocks.Contains(block) == false)
            {
                _movableBlocks.Add(block);
                block.Destroyed += OnBlockDestroyed;
            }
        }
    }

    public void Tick(float deltaTime)
    {
        if (_movableBlocks.Count == 0)
        {
            return;
        }

        float frameMovement = _movementSpeed * deltaTime;

        for (int i = _movableBlocks.Count - 1; i >= 0; i--)
        {
            if (_movableBlocks[i] == null)
            {
                _movableBlocks.Remove(_movableBlocks[i]);
                continue;
            }

            if (_movableBlocks[i].SqrDistanceToTarget <= _minSqrDistanceToTargetPosition)
            {
                CompleteBlockMovement(_movableBlocks[i]);
            }
            else
            {
                _movableBlocks[i].Move(frameMovement);
            }
        }
    }

    public void Clear()
    {
        foreach (var block in _movableBlocks)
        {
            if (block != null)
            {
                block.Destroyed -= OnBlockDestroyed;
            }
        }

        _movableBlocks.Clear();
    }

    private void OnBlockDestroyed(Block destroyedBlock)
    {
        destroyedBlock.Destroyed -= OnBlockDestroyed;
        _movableBlocks.Remove(destroyedBlock);
    }

    private void CompleteBlockMovement(Block block)
    {
        block.FinishMovement();
        block.Destroyed -= OnBlockDestroyed;
        _movableBlocks.Remove(block);
    }
}