public interface IFillable
{
    public int AmountColumns { get; }

    public void PlaceModel(Model model, int columnIndex);
}