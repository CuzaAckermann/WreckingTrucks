using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathSettings
{
    [SerializeField] private List<Transform> _path;
    [SerializeField, Min(0)] private int _indexCheckPointForStartShooting;
    [SerializeField, Min(0)] private int _indexCheckPointForFinishShooting; 

    public List<Transform> Path => _path;

    public int IndexCheckPointForStartShooting => _indexCheckPointForStartShooting;

    public int IndexCheckPointForFinishShooting => _indexCheckPointForFinishShooting;
}