public class CascadeFiller : FillingStrategy
{
    public CascadeFiller(Stopwatch stopwatch, float frequency) : base(stopwatch, frequency)
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