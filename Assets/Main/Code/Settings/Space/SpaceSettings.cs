using System;
using UnityEngine;

[Serializable]
public class SpaceSettings<FS> where FS : FieldSettings
{
    [SerializeField] private FS _fieldSettings;
    [SerializeField] private Transform _fieldTransform;
    [SerializeField] private FieldIntervals _fieldIntervals;
    [SerializeField] private MoverSettings _moverSettings;

    public void SetFieldSettings(Transform fieldTransform, FS fieldSettings)
    {
        _fieldSettings = fieldSettings ?? throw new ArgumentNullException(nameof(fieldSettings));
        _fieldTransform = fieldTransform;
    }

    public FS FieldSettings => _fieldSettings;

    public Transform FieldTransform => _fieldTransform;

    public FieldIntervals FieldIntervals => _fieldIntervals;

    public MoverSettings MoverSettings => _moverSettings;
}