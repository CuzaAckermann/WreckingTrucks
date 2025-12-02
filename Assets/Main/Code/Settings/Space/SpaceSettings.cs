using System;
using UnityEngine;

[Serializable]
public class SpaceSettings<FS> where FS : FieldSettings
{
    [SerializeField] protected FS _fieldSettings;

    [SerializeField] private NonstopGameBlockFieldSettings _nonstopGameBlockFieldSettings;
    [SerializeField] private Transform _fieldTransform;
    [SerializeField] private FieldIntervals _fieldIntervals;
    [SerializeField] private FillerSettings _fillerSettings;

    public virtual void SetFieldSettings(Transform fieldTransform, FS fieldSettings)
    {
        _fieldTransform = fieldTransform;
        _fieldSettings = fieldSettings ?? throw new ArgumentNullException(nameof(fieldSettings));
    }

    public void SetTransform(Transform transform)
    {
        _fieldTransform = transform;
    }

    public void SetNonstopGameBlockFieldSettings(NonstopGameBlockFieldSettings nonstopGameBlockFieldSettings)
    {
        _nonstopGameBlockFieldSettings = nonstopGameBlockFieldSettings ?? throw new ArgumentNullException(nameof(nonstopGameBlockFieldSettings));
    }

    public void SetFieldTransform(Transform fieldTransform)
    {
        _fieldTransform = fieldTransform;
    }

    public FS FieldSettings => _fieldSettings;

    public NonstopGameBlockFieldSettings NonstopGameBlockFieldSettings => _nonstopGameBlockFieldSettings;

    public Transform FieldTransform => _fieldTransform;

    public FieldIntervals FieldIntervals => _fieldIntervals;

    public FillerSettings FillerSettings => _fillerSettings;
}