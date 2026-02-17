using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockLayerSettings
{
    [SerializeField] private List<BlockRowSettings> _rows = new List<BlockRowSettings>();

    public IReadOnlyList<BlockRowSettings> Rows => _rows;

    public void Validate(int rowsCount, int columnsCount)
    {
        while (_rows.Count < rowsCount)
        {
            _rows.Add(new BlockRowSettings());
        }

        while (_rows.Count > rowsCount)
        {
            _rows.RemoveAt(_rows.Count - 1);
        }

        foreach (var row in _rows)
        {
            row?.Validate(columnsCount);
        }
    }
}
