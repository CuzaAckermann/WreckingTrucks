using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathSettings
{
    [SerializeField] private List<Transform> _path;
    [SerializeField, Min(0)] private int _indexCheckPointForStartShooting;

    public List<Transform> Path => _path;

    public int IndexCheckPointForStartShooting => _indexCheckPointForStartShooting;
}