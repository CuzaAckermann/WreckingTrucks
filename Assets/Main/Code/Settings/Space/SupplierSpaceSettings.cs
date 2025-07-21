using System;
using UnityEngine;

[Serializable]
public class SupplierSpaceSettings
{
    [SerializeField] private MoverSettings _moverSettings;

    public MoverSettings MoverSettings => _moverSettings;
}