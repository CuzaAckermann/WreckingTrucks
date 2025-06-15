public class RowFiller : FillingStrategy
{
    public RowFiller(float frequency)
              : base(frequency)
    {

    }

    protected override void Fill(FillingCard<Model> fillingCard)
    {
        for (int i = 0; i < fillingCard.Width; i++)
        {
            PlaceModel();
        }
    }
}