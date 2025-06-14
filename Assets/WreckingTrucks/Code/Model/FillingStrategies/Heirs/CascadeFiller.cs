public class CascadeFiller : FillingStrategy
{
    public CascadeFiller(float frequency)
                  : base(frequency)
    {

    }

    protected override void Fill(FillingCard<Model> fillingCard)
    {
        PlaceModel();
    }
}
