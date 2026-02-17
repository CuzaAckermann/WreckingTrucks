using System;
using UnityEngine;

[Serializable]
public class UpdaterSettings
{
    [SerializeField] private int _capacity;

    public int Capacity => _capacity;
}
