using System;
using UnityEngine;

[Serializable]
public class SpaceSettings<FS> where FS : FieldSettings
{
    [SerializeField] protected FS _fieldSettings;

    [SerializeField] private Transform _fieldTransform;
    [SerializeField] private FieldIntervals _fieldIntervals;
    [SerializeField] private FillerSettings _fillerSettings;

    public void SetFieldSettings(FS fieldSettings)
    {
        _fieldSettings = fieldSettings ?? throw new ArgumentNullException(nameof(fieldSettings));
    }

    public void SetTransform(Transform transform)
    {
        _fieldTransform = transform;
    }

    public FS FieldSettings => _fieldSettings;

    public Transform FieldTransform => _fieldTransform;

    public FieldIntervals FieldIntervals => _fieldIntervals;

    public FillerSettings FillerSettings => _fillerSettings;
}