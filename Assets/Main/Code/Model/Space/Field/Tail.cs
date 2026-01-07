using System;
using System.Collections.Generic;
using UnityEngine;

public class Tail
{
    private readonly Pointer _layerPointer;
    private readonly Pointer _columnPointer;
    private readonly Pointer _rowPointer;

    public Tail(Pointer layerPointer, Pointer columnPointer, Pointer rowPointer)
    {
        _layerPointer = layerPointer ?? throw new ArgumentNullException(nameof(layerPointer));
        _columnPointer = columnPointer ?? throw new ArgumentNullException(nameof(columnPointer));
        _rowPointer = rowPointer ?? throw new ArgumentNullException(nameof(rowPointer));
    }
}