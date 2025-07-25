public class RowFiller : FillingStrategy
{
    public RowFiller(float frequency) : base(frequency)
    {

    }

    protected override void Fill(FillingCard fillingCard)
    {
        for (int i = 0; i < fillingCard.AmountColumns && fillingCard.Amount > 0; i++)
        {
            PlaceModel(GetRecord(fillingCard));
        }

        if (fillingCard.Amount == 0)
        {
            OnFillingCardEmpty();
        }
    }
}