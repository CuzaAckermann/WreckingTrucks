using System.Collections.Generic;

public class CascadeFiller : FillingStrategy
{
    private int _currentColumn;

    public CascadeFiller(IFillable field, float frequency)
                  : base(field, frequency)
    {

    }

    protected override void Fill(IFillable field, Queue<Model> models)
    {
        field.PlaceModel(models.Dequeue(), _currentColumn);
        _currentColumn = (_currentColumn + 1) % field.AmountColumns;
    }
}