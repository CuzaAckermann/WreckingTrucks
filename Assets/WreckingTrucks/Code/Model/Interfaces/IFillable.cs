using UnityEngine;

public interface IFillable
{
    public Vector3 Position { get; }

    public Vector3 Forward { get; }

    public Vector3 Right { get; }

    public Vector3 Up { get; }

    public float IntervalBetweenModels { get; }

    public float DistanceBetweenModels { get; }

    public void PlaceModel(RecordModelToPosition<Model> record);

    public void PlaceModel(Model model, int numberOfColumn);

    public int GetAmountElementsInColumn(int indexOfColumn);
}