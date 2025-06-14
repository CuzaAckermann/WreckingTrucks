using UnityEngine;

public interface IFillable
{
    public Vector3 Position { get; }

    public void PlaceModel(RecordModelToPosition<Model> record);
}