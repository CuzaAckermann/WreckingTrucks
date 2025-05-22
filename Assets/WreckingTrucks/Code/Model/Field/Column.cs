using System;
using System.Collections.Generic;
using UnityEngine;

public class Column
{
    private readonly List<Block> _blocks;
    private readonly List<Block> _blocksForMovement;

    private readonly Vector3 _position;
    private readonly Vector3 _direction;

    public Column(Vector3 position, Vector3 direction, int capacity)
    {
        _position = position;
        _direction = direction;

        _blocks = new List<Block>(capacity);
        _blocksForMovement = new List<Block>(capacity);
    }

    public event Action AmountBlocksChanged;
    public event Action<List<Block>> TargetPositionsForBlocksChanged;
    public event Action AllBlocksDestroyed;

    public bool HasBlocks => _blocks.Count > 0;

    public int AmountBlocks => _blocks.Count;

    public void AddBlock(Block block)
    {
        if (block == null)
        {
            throw new ArgumentNullException(nameof(block));
        }

        block.Destroyed += OnBlockDestroyed;
        _blocks.Add(block);

        // позици€ находитьс€ слишком далеко, что делать если нужно будет использовать разные позиции дл€ спавна блоков, например дл€ ƒќ∆ƒя, »«ћ≈Ќ»“№
        block.SetStartPosition(CalculateBlockPosition(_blocks.Capacity)); 

        block.SetTargetPosition(CalculateBlockPosition(_blocks.Count - 1));
        ShiftBlocks();
    }

    public void Clear()
    {
        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_blocks[i] != null)
            {
                _blocks[i].Destroyed -= OnBlockDestroyed;
                _blocks[i].Destroy();
            }
        }

        _blocksForMovement.Clear();
        _blocks.Clear();
    }

    private Vector3 CalculateBlockPosition(int index)
    {
        return _position + _direction * index;
    }

    private void NullifyBlock(Block block)
    {
        int index = _blocks.IndexOf(block);

        if (index != -1)
        {
            _blocks[index] = null;
        }
    }

    private void ShiftBlocks()
    {
        _blocksForMovement.Clear();
        Vector3 targetPosition = _position;
        int writeIndex = 0;

        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_blocks[i] != null)
            {
                if (writeIndex != i)
                {
                    _blocks[writeIndex] = _blocks[i];
                    _blocks[i] = null;
                }

                _blocks[writeIndex].SetTargetPosition(targetPosition);
                _blocksForMovement.Add(_blocks[writeIndex]);
                targetPosition += _direction;
                writeIndex++;
            }
        }

        TrimExcessNulls(writeIndex);

        AmountBlocksChanged?.Invoke();

        if (_blocksForMovement.Count > 0)
        {
            TargetPositionsForBlocksChanged?.Invoke(_blocksForMovement);
        }

        if (_blocks.Count == 0)
        {
            NotifyAboutEmptyBlocks();
        }
    }

    private void TrimExcessNulls(int startIndex)
    {
        if (startIndex < _blocks.Count)
        {
            _blocks.RemoveRange(startIndex, _blocks.Count - startIndex);
        }
    }

    private void OnBlockDestroyed(Block block)
    {
        if (block == null)
        {
            throw new ArgumentNullException(nameof(block));
        }

        block.Destroyed -= OnBlockDestroyed;
        NullifyBlock(block);
        ShiftBlocks();
    }

    private void NotifyAboutEmptyBlocks()
    {
        AllBlocksDestroyed?.Invoke();
    }
}