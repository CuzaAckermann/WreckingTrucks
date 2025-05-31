using System;
using System.Collections.Generic;

public class RowsFiller : FillingStrategy
{
    private BlocksField _modelsField;
    private Queue<Block> _blocks;

    public event Action FillingCompleted;

    public override void Fill()
    {
        List<Block> blocks = new List<Block>(_modelsField.AmountColumns);

        for (int i = 0; i < _modelsField.AmountColumns; i++)
        {
            blocks.Add(_blocks.Dequeue());
        }

        _modelsField.PlaceModels(blocks);

        if (_blocks.Count == 0)
        {
            FillingCompleted?.Invoke();
        }
    }
}