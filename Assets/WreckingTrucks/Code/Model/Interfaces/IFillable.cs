using UnityEngine;

public interface IFillable
{
    public Vector3 Position { get; }

    public Vector3 Forward { get; }

    public Vector3 Right { get; }

    public Vector3 Up { get; }

    public void PlaceModel(RecordModelToPosition<Model> record);
}