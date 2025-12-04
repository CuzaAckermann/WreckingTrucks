using System;
using UnityEngine;

public interface IDetector
{
    public event Action<GameObject> Detected;

    public event Action<GameObject> Leaved;
}