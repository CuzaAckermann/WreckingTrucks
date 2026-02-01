using System;
using UnityEngine;

[Serializable]
public class FieldIntervals
{
    [SerializeField, Min(0.01f)] private float _betweenLayers = 1;
    [SerializeField, Min(0.01f)] private float _betweenColumns = 1;
    [SerializeField, Min(0.01f)] private float _betweenRows = 1;

    public float BetweenLayers => _betweenLayers;

    public float BetweenColumns => _betweenColumns;

    public float BetweenRows => _betweenRows;
}