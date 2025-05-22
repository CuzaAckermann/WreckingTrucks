using System;
using System.Collections.Generic;

public class BlockMover : ITickable, IClearable
{
    private List<Block> _movableBlocks;
    private float _movementSpeed;
    private float _minSqrDistanceToTargetPosition;

    public BlockMover(int capacity, float movementSpeed, float minSqrDistanceToTargetPosition)
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

        _movableBlocks = new List<Block>(capacity);
        _movementSpeed = movementSpeed;
        _minSqrDistanceToTargetPosition = minSqrDistanceToTargetPosition;
    }

    public void AddBlocks(List<Block> blocks)
    {
        if (blocks == null)
        {
            throw new ArgumentNullException(nameof(blocks));
        }

        foreach (var block in blocks)
        {
            if (block == null)
            {
                throw new ArgumentNullException($"{nameof(block)} in collection cannot be null");
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
        float sqrFrameMovement = frameMovement * frameMovement;

        for (int i = _movableBlocks.Count - 1; i >= 0; i--)
        {
            if (_movableBlocks[i] == null)
            {
                _movableBlocks.Remove(_movableBlocks[i]);
                continue;
            }

            MoveBlock(_movableBlocks[i], frameMovement, sqrFrameMovement);
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

    private void MoveBlock(Block block, float frameMovement, float sqrFrameMovement)
    {
        float sqrDistanceToTarget = block.DirectionToTarget.sqrMagnitude;

        if (sqrDistanceToTarget <= _minSqrDistanceToTargetPosition)
        {
            CompleteBlockMovement(block);

            return;
        }

        if (sqrDistanceToTarget > sqrFrameMovement)
        {
            block.Move(frameMovement);
        }
        else
        {
            CompleteBlockMovement(block);
        }
    }

    private void CompleteBlockMovement(Block block)
    {
        block.FinishMovement();
        block.Destroyed -= OnBlockDestroyed;
        _movableBlocks.Remove(block);
    }

    private void OnBlockDestroyed(Block destroyedBlock)
    {
        destroyedBlock.Destroyed -= OnBlockDestroyed;
        _movableBlocks.Remove(destroyedBlock);
    }
}