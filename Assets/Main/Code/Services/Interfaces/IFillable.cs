using System;
using UnityEngine;

public interface IFillable
{
    public event Action<int, int, int> ModelRemoved;

    public Vector3 Position { get; }

    public Vector3 Forward { get; }

    public Vector3 Right { get; }

    public Vector3 Up { get; }

    public int AmountLayers { get; }

    public int AmountColumns { get; }

    public int AmountRows { get; }

    public float IntervalBetweenLayers { get; }

    public float IntervalBetweenColumns { get; }
    
    public float IntervalBetweenRows { get; }

    public void AddModel(Model model, int indexOfLayer, int indexOfColumn);

    public void InsertModel(Model model,
                            int indexOfLayer,
                            int indexOfColumn,
                            int indexOfRow);

    public int GetAmountModelsInColumn(int indexOfLayer, int indexOfColumn);
}