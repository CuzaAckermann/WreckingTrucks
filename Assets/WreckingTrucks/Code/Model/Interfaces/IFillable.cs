public interface IFillable
{
    public int Width { get; }

    public void PlaceModel(Model model, int columnIndex, int positionInColumn);
}