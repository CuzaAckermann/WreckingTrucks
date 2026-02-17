using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockField : Field
{
    public BlockField(List<Layer> layers,
                      Vector3 position,
                      Vector3 layerDirection,
                      Vector3 columnDirection,
                      Vector3 rowDirection, 
                      float intervalBetweenLayers,
                      float intervalBetweenRows,
                      float intervalBetweenColumns,
                      int amountColumns,
                      int sizeColumn,
                      EventBus eventBus)
               : base(layers,
                      position,
                      layerDirection,
                      columnDirection,
                      rowDirection,
                      intervalBetweenLayers,
                      intervalBetweenRows,
                      intervalBetweenColumns,
                      amountColumns,
                      sizeColumn,
                      eventBus)
    {
        ContinueShiftModels();
    }

    public event Action Cleared;

    protected override FieldWastedSignal InvokeDevastated()
    {
        return new BlockFieldWastedSignal();
    }

    protected override void Clear(ClearedSignal<Level> _)
    {
        base.Clear(_);

        Cleared?.Invoke();
    }
}