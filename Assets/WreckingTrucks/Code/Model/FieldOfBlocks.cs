using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfBlocks
{
    private Vector3 _position;
    private Vector3 _columnDirection;
    private Vector3 _rowDirection;

    private List<Column> _columns;
    private BlocksMover _blocksMover;

    private int _amountColumnsWithBlocks;

    public FieldOfBlocks(Vector3 position, Vector3 columnDirection, Vector3 rowDirection,
                         int amountColumns, int capacityColumn, BlocksMover blocksMover)
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
        _amountColumnsWithBlocks = _columns.Count;
    }

    public event Action<Block> BlockTaken;
    public event Action AllColumnIsEmpty;

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
    }

    public void Clear()
    {
        UnsubscribeAllColumns();
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
            _columns[i].AllBlocksDestroyed += OnAllBlocksDestroyed;
            _columns[i].TargetPositionsForBlocksChanged += OnTargetPositionsForBlocksChanged;
        }
    }

    private void UnsubscribeAllColumns()
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            UnsubscribeColumn(_columns[i]);
        }
    }

    private void UnsubscribeColumn(Column column)
    {
        column.AllBlocksDestroyed -= OnAllBlocksDestroyed;
        column.TargetPositionsForBlocksChanged -= OnTargetPositionsForBlocksChanged;
    }

    private void OnAllBlocksDestroyed(Column column)
    {
        if (column == null)
        {
            throw new ArgumentNullException(nameof(column));
        }

        UnsubscribeColumn(column);

        _amountColumnsWithBlocks--;
        Debug.Log(_amountColumnsWithBlocks);

        if (_amountColumnsWithBlocks == 0)
        {
            AllColumnIsEmpty?.Invoke();
        }
    }

    private void OnTargetPositionsForBlocksChanged(List<Block> blocks)
    {
        _blocksMover?.AddBlocks(blocks);
    }
}