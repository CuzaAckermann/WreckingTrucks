using System;
using UnityEngine;

[Serializable]
public class GlobalSettings
{
    [Header("Block Field Settings")]
    [SerializeField] private Transform _blockFieldTransform;
    [SerializeField] private FieldIntervals _blockFieldIntervals;

    [Header("Truck Field Settings")]
    [SerializeField] private Transform _truckFieldTransform;
    [SerializeField] private FieldSize _truckFieldSize;
    [SerializeField] private FieldIntervals _truckFieldIntervals;

    [Header("Cartrige Box Field Settings")]
    [SerializeField] private Transform _cartrigeBoxFieldTransform;
    [SerializeField] private FieldSize _cartrigeBoxFieldSize;
    [SerializeField] private FieldIntervals _cartrigeBoxFieldIntervals;

    [Header("Global Entities Settings")]
    [SerializeField] private UpdaterSettings _moverSettings;
    [SerializeField] private UpdaterSettings _rotatorSettings;

    [Header("Generator Settings")]
    [SerializeField] private ModelGeneratorSettings _modelTypeGeneratorSettings;

    [Header("Filler Settings")]
    [SerializeField] private FillerSettings _fillerSettings;

    [Header("Delta Time Factor Settings")]
    [SerializeField] private DeltaTimeFactorSettings _deltaTimeFactorSettings;

    public Transform BlockFieldTransform => _blockFieldTransform;

    public FieldIntervals BlockFieldIntervals => _blockFieldIntervals;

    public Transform TruckFieldTransform => _truckFieldTransform;

    public FieldSize TruckFieldSize => _truckFieldSize;

    public FieldIntervals TruckFieldIntervals => _truckFieldIntervals;

    public Transform CartrigeBoxFieldTransform => _cartrigeBoxFieldTransform;

    public FieldSize CartrigeBoxFieldSize => _cartrigeBoxFieldSize;

    public FieldIntervals CartrigeBoxFieldIntervals => _cartrigeBoxFieldIntervals;

    public UpdaterSettings MoverSettings => _moverSettings;

    public UpdaterSettings RotatorSettings => _rotatorSettings;

    public ModelGeneratorSettings ModelTypeGeneratorSettings => _modelTypeGeneratorSettings;

    public FillerSettings FillerSettings => _fillerSettings;

    public DeltaTimeFactorSettings DeltaTimeFactorSettings => _deltaTimeFactorSettings;

    public void SetFieldTransforms(PlacementSettings placementSettings)
    {
        _blockFieldTransform = placementSettings.BlockFieldTransform;
        _truckFieldTransform = placementSettings.TruckFieldTransform;
        _cartrigeBoxFieldTransform = placementSettings.CartrigeBoxFieldTransform;
    }
}