using System;
using System.Collections.Generic;

public class BlocksMover : ITickable
{
    private HashSet<Block> _movableBlocks;
    private readonly List<Block> _blocksToRemove;
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

        _movableBlocks = new HashSet<Block>(capacity);
        _blocksToRemove = new List<Block>(capacity);
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

            if (_movableBlocks.Add(block))
            {
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

        _blocksToRemove.Clear();
        float frameMovement = _movementSpeed * deltaTime;

        foreach (var block in _movableBlocks)
        {
            if (block == null)
            {
                _blocksToRemove.Add(block);
                continue;
            }

            if (block.SqrDistanceToTarget <= _minSqrDistanceToTargetPosition)
            {
                CompleteBlockMovement(block);
            }
            else
            {
                block.Move(frameMovement);
            }
        }

        RemoveCompletedBlocks();
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
        _blocksToRemove.Clear();
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
        _blocksToRemove.Add(block);
    }

    private void RemoveCompletedBlocks()
    {
        foreach (var block in _blocksToRemove)
        {
            _movableBlocks.Remove(block);
        }
    }
}