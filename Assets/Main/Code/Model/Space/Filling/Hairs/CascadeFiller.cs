public class CascadeFiller : FillingStrategy
{
    public CascadeFiller(float frequency) : base(frequency)
    {

    }

    protected override void Fill(FillingCard fillingCard)
    {
        PlaceModel(GetRecord(fillingCard));

        if (fillingCard.Amount == 0)
        {
            OnFillingCardEmpty();
        }
    }
}