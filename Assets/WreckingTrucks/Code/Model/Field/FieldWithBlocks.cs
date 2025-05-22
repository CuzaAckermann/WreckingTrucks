using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldWithBlocks : IClearable, IResetable
{
    private Vector3 _position;
    private Vector3 _columnDirection;
    private Vector3 _rowDirection;

    private List<Column> _columns;
    private BlockMover _blocksMover;
    
    public FieldWithBlocks(Vector3 position, Vector3 columnDirection, Vector3 rowDirection,
                           int amountColumns, int capacityColumn, BlockMover blocksMover)
    {
        if (amountColumns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountColumns));
        }

        if (capacityColumn <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacityColumn));
        }

        _blocksMover = blocksMover ?? throw new ArgumentNullException(nameof(blocksMover));

        _position = position;
        _columnDirection = columnDirection;
        _rowDirection = rowDirection;

        CreateColumns(amountColumns, capacityColumn);

        SubscribeAllColumns();
    }

    public event Action Reseted;
    public event Action<Block> BlockTaken;
    public event Action AmountBlockChanged;
    public event Action AllColumnIsEmpty;

    public int AllAmountBlocks { get; private set; }

    public int AmountColumns => _columns.Count;

    public void PlaceBlocks(List<Block> blocks)
    {
        if (blocks == null)
        {
            throw new ArgumentNullException(nameof(blocks));
        }

        for (int i = 0; i < blocks.Count; i++)
        {
            BlockTaken?.Invoke(blocks[i]);
            _columns[i].AddBlock(blocks[i]);
        }

        AllAmountBlocks += blocks.Count;
        AmountBlockChanged?.Invoke();
    }

    public void PlaceBlock(Block block, int numberOfColumn)
    {
        BlockTaken?.Invoke(block);
        _columns[numberOfColumn].AddBlock(block);
        AllAmountBlocks++;
        AmountBlockChanged?.Invoke();
    }

    public void Reset()
    {
        SubscribeAllColumns();

        AllAmountBlocks = 0;
        Reseted?.Invoke();
    }

    public void Clear()
    {
       for (int i = 0; i < _columns.Count; i++)
       {
            _columns[i].Clear();
       }

        UnsubscribeAllColumns();
    }

    public int CalculateAmountBlocks()
    {
        int amount = 0;

        for (int i = 0; i < _columns.Count; i++)
        {
            amount += _columns[i].AmountBlocks;
        }

        return amount;
    }

    private void CreateColumns(int amountColumns, int capacityColumn)
    {
        _columns = new List<Column>(amountColumns);

        for (int i = 0; i < amountColumns; i++)
        {
            _columns.Add(new Column(_position + _rowDirection * i, _columnDirection, capacityColumn));
        }
    }

    private void SubscribeAllColumns()
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            SubscribeColumn(_columns[i]);
        }
    }

    private void UnsubscribeAllColumns()
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            UnsubscribeColumn(_columns[i]);
        }
    }

    private void SubscribeColumn(Column column)
    {
        column.AllBlocksDestroyed += OnAllBlocksInColumnDestroyed;
        column.TargetPositionsForBlocksChanged += OnTargetPositionsForBlocksChanged;
        column.AmountBlocksChanged += OnAmountBlocksChanged;
    }

    private void UnsubscribeColumn(Column column)
    {
        column.AllBlocksDestroyed -= OnAllBlocksInColumnDestroyed;
        column.TargetPositionsForBlocksChanged -= OnTargetPositionsForBlocksChanged;
        column.AmountBlocksChanged -= OnAmountBlocksChanged;
    }

    private void OnAllBlocksInColumnDestroyed()
    {
        int amountColumnsWithBlocks = 0;

        for (int i = 0; i < _columns.Count; i++)
        {
            if (_columns[i] != null)
            {
                if (_columns[i].HasBlocks)
                {
                    amountColumnsWithBlocks++;
                }
            }
        }

        if (amountColumnsWithBlocks == 0)
        {
            AllColumnIsEmpty?.Invoke();
        }
    }

    private void OnTargetPositionsForBlocksChanged(List<Block> blocks)
    {
        _blocksMover?.AddBlocks(blocks);
    }

    private void OnAmountBlocksChanged()
    {
        AmountBlockChanged?.Invoke();
    }
}